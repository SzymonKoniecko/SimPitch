# Running tests

Quick reference for every test suite in SimPitch. All commands are run from the repository root.

| Suite | Needs | Time |
|---|---|---|
| [Unit + property-based tests](#1-unit--property-based-tests) | .NET 9 SDK | seconds |
| [API tests (Testcontainers)](#2-api-tests-testcontainers) | .NET 9 SDK + Docker | ~30 s |
| [gRPC contract tests](#3-grpc-contract-tests) | .NET 9 SDK | ~1 s |
| [ML tests (pytest)](#4-ml-tests-pytest) | Python 3 | seconds |
| [E2E – Playwright](#5-e2e--playwright) | running stack | ~1–2 min |
| [E2E – Selenium](#6-e2e--selenium) | running stack + Selenium container | a few min |
| [Performance – k6](#7-performance--k6) | running stack + Grafana/InfluxDB containers | ~2 min (smoke) |

---

## 1. Unit + property-based tests

No Docker, no database. Includes the FsCheck/Bogus property-based tests (`PropertyBased/` folders).

```bash
dotnet test src/Services/SportsDataService/SportsDataService.sln
dotnet test src/Services/StatisticsService/StatisticsService.sln
dotnet test src/Services/LoggingService/LoggingService.sln
dotnet test src/Services/SimulationService/SimulationService.sln --filter "FullyQualifiedName!~ApiTests"
dotnet test src/Services/EngineService/EngineService.sln --filter "FullyQualifiedName!~ApiTests"
```

Only the property-based tests:

```bash
dotnet test src/Services/SimulationService/tests/SimulationService.Tests --filter "FullyQualifiedName~PropertyBased"
dotnet test src/Services/EngineService/tests/EngineService.Tests --filter "FullyQualifiedName~PropertyBased"
```

## 2. API tests (Testcontainers)

Only **Docker** has to be running – the SimPitch stack does not. Testcontainers starts SQL Server (schema from `sql/init.sql`) and Redis,
`WebApplicationFactory` hosts the services in-process. Containers are removed automatically after the run.

```bash
dotnet test src/Services/SimulationService/tests/SimulationService.Tests --filter "FullyQualifiedName~ApiTests"
dotnet test src/Services/EngineService/tests/EngineService.Tests --filter "FullyQualifiedName~ApiTests"
```

Without a filter, `dotnet test` on these two solutions runs unit **and** API tests together.

## 3. gRPC contract tests

Dedicated project checking the client ↔ server contracts between services – no Docker, no database.
Covered: EngineService → SimulationService, EngineService → StatisticsService, SimulationService → StatisticsService.

- **Schema** – each service compiles its own copy of the SimPitch protos; the client and server copies must describe the same wire format.
- **Data** – the real server gRPC service (with a mocked application layer) is called by the real client of the other service,
  so server mapping → wire → client mapping is verified (dates, GUIDs, optional fields, JSON payloads, NotFound status).
  The run uses the `pl-PL` culture on purpose – contracts must not depend on the culture of the machine.

```bash
dotnet test src/Services/ContractTests/ContractTests.sln
```

## 4. ML tests (pytest)

```bash
cd src/ML
python3 -m venv venv              # first time only
source venv/bin/activate
pip install -r requirements.txt   # first time only
pytest
```

> The ML project is obsolete – `tests/services/test_simulation_service.py` is commented out (its imports no longer match the project layout).

## 5. E2E – Playwright

Start the stack first (see [README – Setup & Run](README.md#setup--run-instructions)), the app must be available at `http://localhost:8080`.

```bash
cd src/Playwright
dotnet build
pwsh SimPitchPlaywright/bin/Debug/net9.0/playwright.ps1 install chromium   # first time only
dotnet test
```

Useful options:

```bash
dotnet test -- Playwright.LaunchOptions.Headless=false     # watch the browser
dotnet test --filter "Name~pw_SimulationItem"              # single area
BASEURL=http://localhost:5173 dotnet test                  # other app URL
```

Traces of failed tests are saved to `SimPitchPlaywright/bin/Debug/net9.0/playwright-traces/`. More details (also running without PowerShell): [src/Playwright/README.md](src/Playwright/README.md).

## 6. E2E – Selenium

Start the stack **including the Selenium container** (`selenium/standalone-chromium`, port `4444`, defined in `docker-compose.app.yml`).
By default the tests drive the remote browser in that container and open the app at the Docker network address from `appsettings.json`.

```bash
cd src/Selenium
dotnet test
```

Run with a local Chrome instead of the container (settings can be overridden by environment variables):

```bash
cd src/Selenium
DRIVERMODE=local BASEURL=http://localhost:8080 HEADLESS=true dotnet test
```

---

## 7. Performance – k6

Load test of the simulation engine: `perf/load-test-simulations.js` creates simulations (all 4 models), waits for the result
and measures the engine (queue time, total time, time per iteration) and the API latency, while other users browse the simulation list.
Hard acceptance thresholds (p95) are in `perf/performance-thresholds.json` – exceeding any of them fails the run (exit code 99).

k6, InfluxDB and Grafana run as containers of the stack. Start Grafana once, then run k6 (it is started only on demand – profile `perf`):

```bash
docker compose -p sim-pitch-stack -f docker-compose.yml -f docker-compose.app.yml up -d influxdb grafana
docker compose -p sim-pitch-stack -f docker-compose.yml -f docker-compose.app.yml --profile perf run --rm k6
```

Results: **Grafana** at `http://localhost:3000` → dashboard *SimPitch – k6 simulation engine load test*.

| Option | Meaning |
|---|---|
| `-e PROFILE=smoke` (default) | 1 simulation per model + light browsing – baseline, ~2 min |
| `-e PROFILE=load` | 4 users computing in parallel for 5 min |
| `-e PROFILE=stress` | up to 20 users – looking for the limit of the queue/worker |
| `-e ITERATIONS=50` | iterations of one simulation (thresholds are calibrated for 50) |

Example: `docker compose ... --profile perf run --rm -e PROFILE=load k6`. With a local k6 installation: `k6 run perf/load-test-simulations.js`.

## Everything at once

```bash
for sln in SportsDataService StatisticsService LoggingService SimulationService EngineService; do
  dotnet test src/Services/$sln/$sln.sln
done
dotnet test src/Services/ContractTests/ContractTests.sln
(cd src/ML && source venv/bin/activate && pytest)
(cd src/Playwright && dotnet test)
```

The loop needs Docker (API tests); Playwright needs the running stack.
