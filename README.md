# LanWatch# 🚀 LANWatch

A lightweight, scalable **LAN-based system monitoring platform** that collects and displays real-time metrics (CPU, RAM, Disk, Network usage) from multiple Windows PCs over LAN using a **.NET Agent**, **.NET Web API backend**, and a **React.js Dashboard**.

---

## 📌 Overview

LANWatch is a distributed monitoring system designed for local networks. It allows you to:

* Monitor multiple PCs on the same LAN
* Collect system telemetry: CPU, RAM, Disk, Network usage
* Push telemetry from Windows clients to a centralized server
* Display real-time metrics on a modern React dashboard
* Store historical data for analysis
* Trigger alerts or actions for clients

LANWatch is ideal for **home labs, offices, small IT environments, classrooms, or production floors**.

---

## 🏗️ Architecture

```
[ Windows Client Agent ] → REST/SignalR → [ .NET Web API Server ] → SQL Server / Redis → [ React Dashboard ]
```

### Components

* **LANWatch.Agent** — Windows console/service app that collects metrics
* **LANWatch.ServerAPI** — .NET Web API backend (REST + SignalR)
* **LANWatch.Dashboard** — React web dashboard
* **Database** — SQL Server (primary store) + Redis (optional cache)

---

## ✨ Features

### 🔹 **LAN-based Client Monitoring**

* Each client sends metrics every X seconds
* Auto-register on first contact

### 🔹 **Real-Time Dashboard**

* Live updates using SignalR
* Color-coded system health
* Search, filter, and group clients

### 🔹 **Historical Data & Charts**

* Query based on time range
* CPU/RAM/Disk/Network trend charts

### 🔹 **Agent Commands**

* Restart agent
* Update configuration remotely

### 🔹 **Scalable & Secure**

* Works for 1 to 1000+ clients
* JWT + API-Key based authentication
* HTTPS enforced

---

## 📂 Project Structure

```
LANWatch/
│
├── LANWatch.Agent/          # Windows client agent
├── LANWatch.ServerAPI/      # .NET Web API (REST + SignalR)
├── LANWatch.Dashboard/      # React web UI
├── docs/                    # documentation, diagrams, architecture
└── docker-compose.yml       # for local deployment
```

---

## ⚙️ Technologies Used

### Backend

* **.NET 8 Web API**
* **SignalR** for realtime updates
* **Entity Framework Core**
* **SQL Server** (or PostgreSQL/TimescaleDB)
* **Redis** (optional)

### Frontend

* **React.js + TypeScript**
* **Recharts / Chart.js**
* **TailwindCSS**
* **SignalR JS client**

### Agent

* **C# Console App or Windows Service**
* Uses `PerformanceCounter`, `WMI`, and `NetworkInterface` APIs

---

## 📡 Telemetry JSON Format

```json
{
  "clientId": "PC-01",
  "timestampUtc": "2025-11-22T10:10:00Z",
  "cpuPercent": 22.4,
  "ramUsedMB": 4230,
  "ramTotalMB": 8192,
  "disk": [
    { "drive": "C:", "usedMB": 120500, "totalMB": 256000 }
  ],
  "netInBytes": 52400,
  "netOutBytes": 17300
}
```

---

## 🚀 Quick Start

### 1️⃣ Clone the repo

```
git clone https://github.com/<your-username>/LANWatch.git
```

### 2️⃣ Start Backend (Server API)

```
cd LANWatch.ServerAPI
dotnet run
```

### 3️⃣ Start Dashboard

```
cd LANWatch.Dashboard
npm install
npm start
```

### 4️⃣ Run Client Agent

```
cd LANWatch.Agent
LANWatch.Agent.exe
```

---

## 🔐 Authentication

* **Agents** authenticate via API Key or Client Certificate
* **Dashboard users** authenticate via JWT

---

## 🧪 Testing

* Unit tests for API controllers
* Integration tests with TestContainers
* Load testing using k6

---

## 📦 Deployment

* Docker Compose (local dev)
* Kubernetes (scaling)
* Supports Azure, AWS, DigitalOcean

---

## 🛠️ Roadmap

* [ ] Add system alerts (high CPU, low RAM)
* [ ] Add process-level monitoring
* [ ] Add user login roles
* [ ] Add dark/light dashboard themes
* [ ] Add mobile-friendly UI

---

## 🤝 Contributing

Pull requests are welcome! Follow standard branching practices.

---

## 📄 License

MIT License — free to use, modify, and distribute.

---

## ❤️ Credits

Created by **Anil Dangi** — Systems Monitoring, Full-stack, and .NET Developer.

---

### ✨ Feel free to ask if you want

* Logo design
* Project badges
* Docker deployment files
* API documentation (Swagger-based)
* Full starter code for all modules
