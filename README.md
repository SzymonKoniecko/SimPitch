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
**ML worker in XGBOOST (in development)**
*   **Tech:** SimPitch simulations by machine learning (XGBOOST)
*   **Role:** Simulate the results by XGBOOST ML


### 🔗 [SimPitch.Shared](https://github.com/SzymonKoniecko/SimPitchPROTOS)
**The Contracts.**
*   **Tech:** Protocol Buffers (.proto).
*   **Role:** The **Single Source of Truth** for data structures. Defines the shared schemas for API communication to ensure type safety between the .NET backend and Vue frontend.


### 🔗 [SimPitch.Selenium](https://github.com/SzymonKoniecko/SimPitchSelenium)
**Automated tests.**

## Setup & Run Instructions

You can run SimPitch locally or in a production-like environment using **Docker** and **Docker Compose**.

In main directory:

***If first time: --seed true***
```
chmod +x scripts/generate-env.sh
scripts/./generate-env.sh --seed false
docker-compose up -d
```
***Re-runs***
set in .env file variable: SEED_DATA=false 

then
```
docker-compose up -d
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
| **MSelenium** | Test software |

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
Dedicated logging service aggregating logs from all components.  
Simplifies monitoring, debugging, and system health tracking.

---

## Development Roadmap

- 🔹 **AI microservice (Python)** to introduce intelligent simulations and predictions  
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

