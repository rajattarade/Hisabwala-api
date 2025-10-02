# Hisabwala API

**Hisabwala API** is a **.NET 8 Web API** powered by **PostgreSQL**.
It provides a **minimal-manual-setup local development environment** using Docker, making collaboration easy.

---

## ✨ Features

- ⚡ **.NET 8 Web API** backend
- 🐘 **PostgreSQL** database
- 🐳 **Docker-powered local development**
- 🔄 **Code-First EF Core migrations**
- 🧪 Designed for **collaboration & contributions**
- 🚀 **Production-ready**: cloud-hosted DB support, no local containers required

---

## 📦 Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (or Docker Engine)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/)

---

## 🚀 Getting Started (Development)

### 1. Clone the repository

```bash
git clone https://github.com/rajattarade/Hisabwala-db.git
git clone https://github.com/rajattarade/Hisabwala-api.git
cd Hisabwala-api
```

### 2. Ensure Docker Database is running

Make sure Docker Desktop (or Docker Engine) is running.
Then initialize the database following the instructions in the [Hisabwala-db README](https://github.com/rajattarade/Hisabwala-db)."

### 3. Apply EF Core migrations

```bash
dotnet ef database update -p Hisabwala.Infrastructure -s Hisabwala.Api
```

### 4. Run the API

#### Option A: Visual Studio
1. Open the solution in Visual Studio
2. Set **Hisabwala.Api** as the startup project
3. Click **Debug ▶**

#### Option B: CLI
```bash
dotnet run --project Hisabwala.Api
```

---

## 🏭 Running in Production

- In production, Docker is **not required**.
- The API connects directly to a managed PostgreSQL database.

---

## 🐳 Docker (Database Only)

These commands are optional for local development:

- **Start DB manually**
```bash
docker compose up -d db
```

- **Stop DB**
```bash
docker stop hisabwala-db
```

- **Remove DB container**
```bash
docker rm -f hisabwala-db
```

- **Check DB logs**
```bash
docker logs hisabwala-db
```

- **Check running containers**
```bash
docker ps
```

---

## 📂 Project Structure

```plaintext
Hisabwala.Api
├── Controllers
├── Program.cs
├── Properties
Hisabwala.Application
├── Features
│   ├── Tags
│   │   ├── GetTags
│   │   └── AddTag
│   └── Expenses
├── Interfaces
Hisabwala.Core
└── Entities
Hisabwala.Infrastructure
├── Persistence
│   └── Migrations
└── Repositories
```
