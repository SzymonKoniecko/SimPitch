# SimPitch

> **Advanced sports match simulation platform built with modern microservice architecture.**

SimPitch was created to demonstrate my skills in developing advanced web and microservice-based applications.  
It’s a comprehensive system for simulating sports matches using mathematical algorithms, with future plans to integrate AI-based predictive models.  
The project showcases how to combine modern technologies, scalable architecture, and clean design principles to build a high-performance, extensible system.

---

## Tech Stack & Architecture

The system is built with **modern, battle-tested technologies** and **clean architectural patterns**:

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
- 🔹 **Enhanced frontend dashboards** with visualization, filtering, and export tools  

---

## Setup & Run Instructions

You can run SimPitch locally or in a production-like environment using **Docker** and **Docker Compose**.

### **Requirements**

- Docker & Docker Compose  
- .NET SDK (for local backend development)  
- Node.js + npm (for frontend development)  
- Microsoft SQL Server
