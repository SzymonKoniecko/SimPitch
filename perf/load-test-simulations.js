// k6 load test of the SimPitch simulation engine.
//
// Goes through the gateway exactly like the frontend:
//   POST   /api/engine/simulation              create a simulation (queued for the background worker)
//   GET    /api/engine/simulation/{id}         poll - 404 while the simulation is being prepared, then state
//   GET    /api/engine/simulationStats/{id}    team statistics of a finished simulation
//   GET    /api/engine/simulation/overviews    list of simulations (read load while the engine computes)
//
// Run:   k6 run perf/load-test-simulations.js
//        k6 run -e PROFILE=load -e BASE_URL=http://localhost:8080 perf/load-test-simulations.js
// Hard acceptance thresholds are in performance-thresholds.json - exceeding any of them fails the run (exit code 99).

import http from 'k6/http';
import { check, sleep } from 'k6';
import { Counter, Rate, Trend } from 'k6/metrics';
import exec from 'k6/execution';

const BASE_URL = __ENV.BASE_URL || 'http://localhost:8080';
const PROFILE = __ENV.PROFILE || 'smoke';
const LEAGUE = __ENV.LEAGUE || 'PKO BP Ekstraklasa';
const SEASON = __ENV.SEASON || '2026/2027';
const ITERATIONS = parseInt(__ENV.ITERATIONS || '50', 10);   // iterations of ONE simulation
const MAX_WAIT_S = parseInt(__ENV.MAX_WAIT_S || '300', 10);  // give up waiting for a simulation after this
const POLL_INTERVAL_S = 1;

const MODELS = ['StandardPoisson', 'DixonColes', 'BivariatePoisson', 'Advanced'];

const thresholdsConfig = JSON.parse(open('./performance-thresholds.json'));

// ---------- custom metrics ----------
// Time from creating the simulation until the worker picked it up (API stops answering 404)
const queueTime = new Trend('simulation_queue_time', true);
// Time from creating the simulation until it is Completed - the actual work of the engine
const simulationDuration = new Trend('simulation_duration', true);
// Engine time per simulated iteration (compare runs with a different ITERATIONS value)
const timePerIteration = new Trend('simulation_time_per_iteration', true);
const simulationSuccess = new Rate('simulation_success');
const simulationsCompleted = new Counter('simulations_completed');

// ---------- load profiles ----------
const PROFILES = {
    // 1 simulation per model - checks the script and gives a baseline
    smoke: {
        simulations: { executor: 'per-vu-iterations', vus: 1, iterations: MODELS.length, maxDuration: '15m' },
        browse: { executor: 'constant-arrival-rate', rate: 1, timeUnit: '1s', duration: '1m', preAllocatedVUs: 2 },
    },
    // several users computing in parallel
    load: {
        simulations: {
            executor: 'ramping-vus', startVUs: 1,
            stages: [{ duration: '1m', target: 4 }, { duration: '5m', target: 4 }, { duration: '1m', target: 0 }],
            gracefulRampDown: '5m',
        },
        browse: { executor: 'constant-arrival-rate', rate: 5, timeUnit: '1s', duration: '7m', preAllocatedVUs: 10 },
    },
    // looking for the breaking point of the queue/worker
    stress: {
        simulations: {
            executor: 'ramping-vus', startVUs: 2,
            stages: [{ duration: '2m', target: 10 }, { duration: '5m', target: 20 }, { duration: '2m', target: 0 }],
            gracefulRampDown: '5m',
        },
        browse: { executor: 'constant-arrival-rate', rate: 20, timeUnit: '1s', duration: '9m', preAllocatedVUs: 40 },
    },
};

if (!PROFILES[PROFILE]) {
    throw new Error(`Unknown PROFILE '${PROFILE}'. Use one of: ${Object.keys(PROFILES).join(', ')}`);
}

export const options = {
    scenarios: {
        simulations: { ...PROFILES[PROFILE].simulations, exec: 'runSimulation' },
        browse: { ...PROFILES[PROFILE].browse, exec: 'browseSimulations' },
    },
    thresholds: thresholdsConfig.thresholds,
    summaryTrendStats: ['avg', 'min', 'med', 'p(90)', 'p(95)', 'max'],
};

const JSON_HEADERS = { headers: { 'Content-Type': 'application/json' } };

// ---------- setup: find the league the simulations run in ----------
export function setup() {
    const response = http.get(`${BASE_URL}/api/sportsdata/league`, { tags: { name: 'setup' } });
    if (response.status !== 200) {
        throw new Error(`Cannot load leagues from ${BASE_URL} (HTTP ${response.status}). Is the stack running?`);
    }
    const league = response.json().find(l => l.name === LEAGUE);
    if (!league) {
        throw new Error(`League '${LEAGUE}' not found.`);
    }
    return { leagueId: league.id };
}

// ---------- scenario: create a simulation and wait for the result ----------
export function runSimulation(data) {
    const model = MODELS[exec.scenario.iterationInTest % MODELS.length];
    const tags = { model };

    const created = http.post(`${BASE_URL}/api/engine/simulation`, JSON.stringify({
        title: `k6 ${PROFILE} ${model} #${exec.scenario.iterationInTest}`,
        seasonYears: [SEASON],
        leagueId: data.leagueId,
        iterations: ITERATIONS,
        seed: 1000 + exec.scenario.iterationInTest,
        gamesToReachTrust: 10,
        confidenceLevel: 0.95,
        homeAdvantage: 1.05,
        noiseFactor: 0.1,
        createScoreboardOnCompleteIteration: true,
        modelType: model,
    }), { ...JSON_HEADERS, tags: { name: 'create_simulation', ...tags } });

    if (!check(created, { 'simulation created': r => r.status === 200 })) {
        simulationSuccess.add(false, tags);
        return;
    }
    const simulationId = created.body.replace(/"/g, '').trim();
    const startedAt = Date.now();

    const finalState = waitForSimulation(simulationId, startedAt, tags);
    const finished = finalState === 'Completed';
    simulationSuccess.add(finished, tags);
    check(finalState, { 'simulation completed': s => s === 'Completed' });

    if (!finished) {
        // do not leave the engine busy after the test
        http.del(`${BASE_URL}/api/engine/simulation/stop/${simulationId}`, null, { tags: { name: 'stop_simulation' } });
        return;
    }

    const elapsed = Date.now() - startedAt;
    simulationDuration.add(elapsed, tags);
    timePerIteration.add(elapsed / ITERATIONS, tags);
    simulationsCompleted.add(1, tags);

    // what the user opens next: result page and team statistics
    const result = http.get(
        `${BASE_URL}/api/engine/simulation/${simulationId}?pageNumber=1&pageSize=10&sortingOption=CreatedDate&order=DESC`,
        { tags: { name: 'get_simulation_result', ...tags } });
    check(result, { 'result page loaded': r => r.status === 200 });

    const stats = http.get(`${BASE_URL}/api/engine/simulationStats/${simulationId}`, { tags: { name: 'get_simulation_stats', ...tags } });
    check(stats, { 'statistics loaded': r => r.status === 200 && r.json().length > 0 });
}

/** Polls the simulation until it reaches a final state. Returns the state or 'Timeout'. */
function waitForSimulation(simulationId, startedAt, tags) {
    let queued = true;
    while ((Date.now() - startedAt) / 1000 < MAX_WAIT_S) {
        const response = http.get(`${BASE_URL}/api/engine/simulation/${simulationId}?pageNumber=1&pageSize=1`, {
            tags: { name: 'poll_simulation', ...tags },
            // 404 is the expected answer while the simulation is being prepared
            responseCallback: http.expectedStatuses(200, 404),
        });

        if (response.status === 200) {
            if (queued) {
                queueTime.add(Date.now() - startedAt, tags);
                queued = false;
            }
            const state = response.json('state.state');
            if (state === 'Completed' || state === 'Failed' || state === 'Cancelled') {
                return state;
            }
        }
        sleep(POLL_INTERVAL_S);
    }
    return 'Timeout';
}

// ---------- scenario: browsing simulations while the engine computes ----------
export function browseSimulations() {
    const response = http.get(`${BASE_URL}/api/engine/simulation/overviews?pageNumber=1&pageSize=10`, {
        tags: { name: 'list_overviews' },
    });
    check(response, { 'overviews loaded': r => r.status === 200 });
}
