# SimPitch

> **Advanced sports match simulation platform built with modern microservice architecture.**

SimPitch was created to demonstrate my skills in developing advanced web and microservice-based applications.  
It’s a comprehensive system for simulating sports matches using mathematical algorithms, with future plans to integrate AI-based predictive models.  
The project showcases how to combine modern technologies, scalable architecture, and clean design principles to build a high-performance, extensible system.

<img src="https://github.com/SzymonKoniecko/SimPitchWeb/blob/main/sim-pitch-web/public/readme-assets/main.png" >
---> (more images are at the end of readme) <---
---


## 📂 Project Ecosystem

The SimPitch solution is modularized into three distinct repositories to ensure a clean separation of concerns between the simulation engine, the user interface, and the data contracts.

### 🧠 [SimPitch.Backend](https://github.com/SzymonKoniecko/SimPitch)
**The Simulation Engine.**
*   **Tech:** C# .NET 8, MediatR (CQRS), EF Core.
*   **Role:** Executes the core mathematical logic (Poisson, Dixon-Coles, Momentum), manages the DDD domain state, and processes simulation strategies.

### 🖥️ [SimPitch.Web](https://github.com/SzymonKoniecko/SimPitchWeb)
**The Visualization Dashboard.**
*   **Tech:** Vue 3, Vite, TypeScript.
*   **Role:** Provides the UI for configuring `SimulationParams`, visualizing complex metrics (Posterior vs. Likelihood), and displaying league iterations.
https://github.com/SzymonKoniecko/SimPitchAI

### 🔗 [SimPitch.ML](https://github.com/SzymonKoniecko/SimPitchML)
**ML worker in XGBOOST (almost completed)**
*   **Tech:** SimPitch simulations by machine learning (XGBOOST)
*   **Role:** AI microservice (Python) to introduce intelligent simulations and predictions  


### 🔗 [SimPitch.Shared](https://github.com/SzymonKoniecko/SimPitchPROTOS)
**The Contracts.**
*   **Tech:** Protocol Buffers (.proto).
*   **Role:** The **Single Source of Truth** for data structures. Defines the shared schemas for API communication to ensure type safety between the .NET backend and Vue frontend.


### 🔗 [SimPitch.Selenium](https://github.com/SzymonKoniecko/SimPitchSelenium)
**Automated tests.**

### 🎭 [SimPitch.Playwright](https://github.com/SzymonKoniecko/SimPitchPlaywright)
**Automated E2E tests (Playwright).**
*   **Tech:** Playwright for .NET, NUnit, C# .NET 9.
*   **Role:** End-to-end tests of the Vue frontend against the running stack. Mirrors the Selenium suite 1:1 (tests are prefixed with `pw_`), uses the same `selenium-id` selectors and records a Playwright trace for every failed test.

## Setup & Run Instructions

You can run SimPitch locally or in a production-like environment using **Docker** and **Docker Compose**.

In main directory:

***[BUILD] If first time: --seed true ***
```
chmod +x scripts/generate-env.sh
scripts/./generate-env.sh --seed true
```
***[BUILD] Re-runs ***
generate-env.sh change to '--seed false' -> execute
```
chmod +x scripts/generate-env.sh
scripts/./generate-env.sh --seed false
```
***[RUN] anytime ***
```
docker compose -p sim-pitch-stack -f docker-compose.yml -f docker-compose.app.yml  up -d --build
```

***[TEST] E2E tests (Playwright) ***

Requires the running stack (app on `http://localhost:8080`). First time only – build and install the browser:
```
cd src/Playwright
dotnet build
pwsh SimPitchPlaywright/bin/Debug/net9.0/playwright.ps1 install chromium
```
Run the tests:
```
cd src/Playwright
dotnet test
```
More options (headed mode, other browsers, traces, running without PowerShell) are described in [src/Playwright/README.md](src/Playwright/README.md).

***[TEST] Fast domain unit tests (FsCheck / Bogus) ***

Property-based tests of the domain logic – no database, network or Docker needed, they run in about a second.
**FsCheck** generates hundreds of inputs per property (including the seed), **Bogus** turns the seed into realistic,
reproducible domain data (teams, season stats, fixtures, scoreboards).

***[TEST] API tests with Testcontainers (WebApplicationFactory) ***

Integration tests of the real APIs – only **Docker** is needed (the stack does not have to run).
**Testcontainers** starts SQL Server (schema from `sql/init.sql`) and Redis, **WebApplicationFactory** hosts the services in-process:
SimulationService is tested over gRPC, EngineService over REST with real gRPC calls to SimulationService (only StatisticsService is faked).
```
dotnet test src/Services/SimulationService/tests/SimulationService.Tests --filter "FullyQualifiedName~ApiTests"
dotnet test src/Services/EngineService/tests/EngineService.Tests --filter "FullyQualifiedName~ApiTests"
```

## Tech Stack & Architecture

The system is built with **modern, battle-tested technologies**, **DDD + Strategy Pattern** and **clean architectural patterns**:

| Technology | Purpose |
|-------------|----------|
| **Docker** | Containerization for independent service deployment, easy scalability, and CI/CD integration |
| **Kubernates** | Integration in future |
| **NGINX** | Acts as a reverse proxy, load balancer, and traffic security layer |
| **Vue.js + TypeScript** | Modern, fast, and responsive front-end framework for intuitive user experience |
| **C# (.NET Core)** | Core backend with **CQRS** and **Clean Architecture** for clear separation of concerns |
| **gRPC** | Fast, strongly-typed communication between microservices and central logging service |
| **Redis** | Background job processing and asynchronous task execution (e.g., iterative simulations) |
| **Microsoft SQL Server** | Persistent database for input data, results, and configurations |
| **Elasticsearch** | Secondary log store indexed by `LoggingService` — enables full-text search and analytics over centralized logs |
| **Kibana** | Web UI for exploring/searching indexed logs and visualizing APM traces, service maps, and dependencies |
| **Elastic APM (APM Server + .NET/Python agents)** | Distributed tracing across all microservices — HTTP/gRPC calls, SQL queries, Redis commands, and errors, all correlated into end-to-end traces |
| **Selenium** | Test software |
| **Playwright** | End-to-end UI tests (Playwright for .NET + NUnit) with traces for failed tests |
| **k6 + InfluxDB + Grafana** | Load tests of the simulation engine with hard p95 thresholds; results visualized in a provisioned Grafana dashboard |
| **xUnit + FsCheck + Bogus** | Fast domain unit tests — property-based testing (FsCheck) on realistic, reproducible generated data (Bogus) |
| **Testcontainers + WebApplicationFactory** | API integration tests — services hosted in-process against real SQL Server and Redis started in Docker |
| **XgBoost** | Machine learning library to predict match results |

---

## Testing Strategy

SimPitch is tested on several levels — from fast, isolated domain checks up to full end-to-end UI flows:

| Level | Tools | What is tested | Needs running stack |
|---|---|---|---|
| **Domain property-based tests** | xUnit, FsCheck, Bogus | Invariants of the simulation domain (`SeasonStats`, `TeamStrength`, all simulation strategies, the whole match simulation workflow) and EngineService mapping/paging — verified on hundreds of generated inputs per property | No (~1 s) |
| **Unit tests** | xUnit, Moq, AutoFixture | Handlers, services and simulation models of each microservice | No |
| **API integration tests** | xUnit, Testcontainers, WebApplicationFactory | SimulationService gRPC API and EngineService REST API end-to-end through the real database and cache: creating, stopping and paging simulations, iteration results and their caching, 404 contract for simulations being prepared | Docker only (~30 s) |
| **gRPC contract tests** | xUnit, Grpc.AspNetCore TestServer, Moq | Client ↔ server contracts EngineService → SimulationService, EngineService → StatisticsService, SimulationService → StatisticsService: identical proto copies on both sides and real client/server mapping over gRPC (dates, GUIDs, optional fields, JSON payloads, NotFound) | No (~1 s) |
| **Performance tests** | k6, InfluxDB, Grafana | Simulation engine under load: queue time, compute time per model and per iteration, API p95 latency while users browse; hard thresholds in `perf/performance-thresholds.json` | Yes |
| **End-to-end UI tests** | Playwright (.NET), Selenium | User flows in the Vue frontend against the real backend (navigation, preparing/running simulations, results, heat map, league overviews) | Yes |

How to run each level (unit, API with Testcontainers, gRPC contracts, ML, Playwright, Selenium, k6) is described in **[TESTING.md](TESTING.md)**.

---

## Core Features

- Start and stop football match simulations with live progress tracking  
- Process real football data to generate realistic simulations  
- Store detailed iteration results for future analysis  
- Generate advanced statistics, leaderboards, and reports from simulation data  
- Reliable microservice communication ensuring flexibility and resilience  
- Centralized logging and monitoring of all events and errors via **LoggerService**  
- Asynchronous background workflows that do not block frontend interaction  
- Multiple simulation modes with customizable parameters and configurations
- **gRPC data chunking** for efficient large response handling  
- **Memory optimization** in `SimulationService` for multi-iteration performance  
- **Property-based domain tests** (FsCheck + Bogus) guarding the simulation math — every model and the whole simulation workflow are checked against invariants on hundreds of generated leagues and matches
- **Full observability stack** — logs dual-written to MSSQL + Elasticsearch, searchable/visualized in **Kibana**, plus distributed tracing via **Elastic APM** across every microservice (HTTP, gRPC, SQL, Redis) with an end-to-end service dependency map

---

## Microservices Overview

<img src="https://github.com/SzymonKoniecko/SimPitchWeb/blob/main/sim-pitch-web/public/readme-assets/sys-diagram.png" >

### **SportDataService**
Collects and maintains real sports data used as the foundation for simulations.  
Ensures data integrity and freshness. 
> (currently only football data)

### **SimulationService**
Manages simulation lifecycle — starting, stopping, and processing simulations in the background using Redis.  
Stores iteration results efficiently while optimizing memory usage.
> Based on System.Collections.Concurrent.ConcurrentQueue

### **StatisticsService**
Generates detailed reports, leaderboards, and statistics from simulation results.  
Enables visualization and comparative insights.

### **EngineService**
Coordinates the entire simulation workflow.  
Communicates with all other services and controls the simulation algorithm.
> In future that microservice will choose the exact service to handle a simulation (Can be in GO, Python, C#)

### **LoggerService**
Dedicated logging service aggregating logs from all components over gRPC.  
Dual-writes every log entry to MSSQL (source of truth) and to **Elasticsearch** (best-effort, for search/analytics in Kibana).  
Simplifies monitoring, debugging, and system health tracking.

---

## Observability Stack (ELK + APM)

On top of the core services, SimPitch ships a full observability stack, wired into every microservice out of the box:

| Component | What it does | Local URL |
|-----------|--------------|-----------|
| **Elasticsearch** | Stores centralized logs (`simpitch-logs-*` indices) and APM trace data | `http://localhost:9200` |
| **Kibana** | UI for searching/filtering logs, browsing APM traces, and viewing the live service dependency map | `http://localhost:5601` |
| **APM Server** | Intake endpoint that receives trace/span/error data from every service's Elastic APM agent and ships it to Elasticsearch | `http://localhost:8200` |
| **Grafana** + **InfluxDB** | Results of k6 load tests (`perf/`) – provisioned dashboard *SimPitch – k6 simulation engine load test* | `http://localhost:3000` / `:8086` |

**What gets traced automatically, with no code changes needed per feature:**
- Every incoming HTTP/gRPC request (ASP.NET Core instrumentation)
- Outgoing gRPC calls between microservices (`Elastic.Apm.GrpcClient`)
- SQL Server queries (`Elastic.Apm.NetCoreAll` SqlClient instrumentation)
- Redis commands in `SportsDataService` and `SimulationService` (`Elastic.Apm.StackExchange.Redis`)
- The `SimPitchMl` Python service (via the `elastic-apm` Starlette middleware)

Because trace context (`traceparent`) is propagated automatically across HTTP/gRPC calls, a single request that hops through several microservices (e.g. `EngineService → SimulationService → SportsDataService`) shows up in Kibana as **one distributed trace** with a full waterfall of every hop's duration — not isolated per-service fragments.

---

## Development Roadmap

- 🔹 **Full Kubernetes migration** for scalability, auto-deployment, and high availability  
- 🔹 **More content**



  

## UI
Dashboard & League Selection<br />
<img src="https://github.com/SzymonKoniecko/SimPitchWeb/blob/main/sim-pitch-web//public/readme-assets/main.png" alt="Main Dashboard" width="800">

<img src="https://github.com/SzymonKoniecko/SimPitchWeb/blob/main/sim-pitch-web//public/readme-assets/prepare-laliga.png" alt="Prepare Simulation" width="800">

Statistical Legend<br />
<img src="https://github.com/SzymonKoniecko/SimPitchWeb/blob/main/sim-pitch-web//public/readme-assets/legend.png" alt="Metrics Legend" width="600">

Iteration Preview<br />
<img src="https://github.com/SzymonKoniecko/SimPitchWeb/blob/main/sim-pitch-web//public/readme-assets/iteration-preview.png" alt="Iteration Preview" width="800">

Match Results<br />
<img src="https://github.com/SzymonKoniecko/SimPitchWeb/blob/main/sim-pitch-web//public/readme-assets/match-result.png" alt="Match Results" width="800">

