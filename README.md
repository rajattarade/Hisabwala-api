# Hisabwala API 

Hisabwala API is a **.NET 8 Web API** powered by **PostgreSQL**.
It is designed for easy collaboration with a **zero-manual-setup local development environment** using Docker. 
 
---

## ✨ Features

- ⚡ **.NET 8 Web API** backend  
- 🐘 **PostgreSQL database**  
- 🐳 **Docker-powered** local development (DB auto-starts when you run API)  
- 🔄 **Code-First EF Core migrations**  
- 🧪 Designed for **contribution & collaboration**  
- 🚀 **Production-ready**: cloud-hosted DB, no local containers required  

---

## 📦 Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (or Docker Engine)  
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Git](https://git-scm.com/)  

---

## 🚀 Getting Started (Development)

### 1. Clone the repository
`bash
git clone [https://github.com/](https://github.com/)rajattarade/Hisabwala-api.git
cd hisabwala-api
`

### 2. Ensure Docker is running
Make sure Docker Desktop (or Docker Engine) is up.

### 3. Run the API in Debug mode
#### Option A: Visual Studio
1. Open the solution in Visual Studio  
2. Select **Hisabwala.Api** as the startup project  
3. Hit **Debug ▶**

#### Option B: CLI
`bash
dotnet run --project src/Hisabwala.Api
``

### 4. What happens under the hood
When you run the API in **Debug** mode:

1. API checks if a container named `hisabwala_db` is running.  
2. If not running:
   - Removes any old container with same name  
   - Starts a fresh **Postgres 15 container**
   - Waits until DB is healthy  
3. Once DB is ready, API starts on:  
   ``
   [http://localhost:5000
](http://localhost:5000
)   `

👉 This automation works only in **Debug mode**.  
In Production, the API connects directly to a managed PostgreSQL instance.

---

## 🏭 Running in Production

In Production, Docker DB is **not used**.  
Instead, API connects directly to a managed PostgreSQL database.

---

## 🐳 Docker (Database Only)

Normally you don’t need to run DB manually in development (API handles it).  
But here are some useful commands if needed:

- **Start DB manually**
  `bash
  docker compose up -d db
  `

- **Stop DB**
  `bash
  docker stop hisabwala_db
  `

- **Remove DB container**
  `bash
  docker rm -f hisabwala_db
  `

- **Check logs**
  `bash
  docker logs hisabwala_db
  `

- **Check running containers**
  `bash
  docker ps
  `

---

## 📂 Project Structure

`
Hisabwala-api/
├── docker-compose.yml   # Local Postgres service definition
├── src/
│   └── Hisabwala.Api/   # Main API project
│       ├── Controllers/ # API controllers
│       ├── Program.cs   # Entry point (spins up DB in Debug mode)
│       └── ...
└── README.md
`

---

## 🧰 Development Notes

- **Ephemeral DB**: local DB is wiped & reseeded every time a new container starts  
- **Debug-only automation**: DB auto-launching happens only in Debug builds  
- **Production**: API expects external DB, no Docker container launched  
`