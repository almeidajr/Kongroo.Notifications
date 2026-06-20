# Kongroo Notifications

Notifications microservice for the Kongroo platform. Built with ASP.NET Core,
it consumes integration events from RabbitMQ via MassTransit and simulates
email delivery by writing structured log entries to stdout (Serilog). It has no
database.

It reacts to two integration events:

- `UserCreatedIntegrationEvent` → sends a welcome email
- `PaymentProcessedIntegrationEvent` (Approved) → sends a purchase confirmation email

## Tags

- `latest` — most recent stable release
- `x.y.z`  — specific version (e.g. `0.0.2`)
- `dev`    — in-progress development build

## Quick start

The container listens on port **8080** and requires a RabbitMQ broker.

```bash
docker run -p 8080:8080 \
  -e RabbitMq__Host="rabbitmq" \
  -e RabbitMq__User="kongroo" \
  -e RabbitMq__Pass="development" \
  josealmeidajr/kongroo-notifications:latest
```

## Endpoints

| Method & path | Description |
|---|---|
| `GET /health` | Health check |

This service has no business HTTP API — it works by consuming messages from the
broker. Email delivery is simulated via structured log output.

## Configuration

Configured via environment variables. The double underscore (`__`) maps to
nested configuration sections.

| Variable | Description |
|---|---|
| `RabbitMq__Host` | RabbitMQ broker hostname |
| `RabbitMq__User` | RabbitMQ username |
| `RabbitMq__Pass` | RabbitMQ password |

## Requirements

- A reachable RabbitMQ broker

## Source

Part of the Kongroo platform.
