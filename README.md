# <img alt="Kongroo" src="./logo.png" width="40"/> Kongroo.Notifications

Notifications microservice for FIAP Cloud Games (Phase 2). Consume-only service that
simulates email delivery by writing structured log entries to stdout.

## Consumed events

| Event                                         | Source   | Action                                       |
| --------------------------------------------- | -------- | -------------------------------------------- |
| `UserCreatedIntegrationEvent`                 | Identity | Logs a simulated welcome email               |
| `PaymentProcessedIntegrationEvent` (Approved) | Payments | Logs a simulated purchase-confirmation email |

Messaging uses MassTransit over RabbitMQ. Contract projects are copied into
`src/Kongroo.Identity.Contracts` and `src/Kongroo.Payments.Contracts`; they must
match the publishers' namespace, type names, and property names.

## Environment variables

| Variable                 | Description                                 | Local default |
| ------------------------ | ------------------------------------------- | ------------- |
| `ASPNETCORE_ENVIRONMENT` | ASP.NET Core environment                    | `Development` |
| `RabbitMq__Host`         | RabbitMQ host (k8s Service name in-cluster) | `localhost`   |
| `RabbitMq__Port`         | RabbitMQ AMQP port (optional)               | `5672`        |
| `RabbitMq__User`         | RabbitMQ username                           | `kongroo`     |
| `RabbitMq__Pass`         | RabbitMQ password                           | `development` |

> MassTransit binds credentials from `RabbitMq__User` / `RabbitMq__Pass`. Using
> `Username` / `Password` silently falls back to `guest` / `guest`.

## Running locally

```bash
dotnet run --project src/Kongroo.Notifications
```

Requires a reachable RabbitMQ broker (see the orchestration repo's `compose.yaml`).
`/health` reports unhealthy until the broker connection is established.

## Docker

```bash
dotnet restore
docker build -t kongroo-notifications .
```

## Tests

```bash
dotnet test
```

Integration and BDD tests start a RabbitMQ container via Testcontainers and require Docker.
