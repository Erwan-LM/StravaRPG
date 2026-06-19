# Strava RPG

Minimal full-stack starter with ASP.NET Core 8 Minimal API, Angular standalone, TailwindCSS, Heroicons, DiceBear and PostgreSQL through Docker.

## Requirements

- .NET 8 SDK
- Node.js 20+
- Docker

## Run locally

```powershell
docker compose up -d
dotnet run --project backend/StravaRpg.Api --urls http://localhost:5208
cd frontend
npm install
npm start
```

Frontend: http://localhost:4200

Backend health: http://localhost:5208/health
