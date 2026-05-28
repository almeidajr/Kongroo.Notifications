# Kongroo.Notifications

Notifications microservice for FIAP Cloud Games (Phase 2 scaffold — domain logic TBD).
Simulates sending emails by logging to console.

## Running Locally

```bash
dotnet run --project src/Kongroo.Notifications.Api
```

## Docker

```bash
dotnet restore
docker build -t kongroo-notifications .
```
