# FleetPulse

FleetPulse is a .NET 8 backend application for fleet tracking, real-time telemetry processing and geofence violation detection.

The project is being developed to practice and demonstrate clean backend architecture, real-time communication and event-driven messaging.

## 🚀 Features

- Asset and telemetry management
- Telemetry simulation
- Geofence violation detection
- MongoDB persistence
- Real-time telemetry updates with SignalR
- Event-driven geofence violation messaging with RabbitMQ
- Background RabbitMQ consumer
- Manual ACK/NACK message handling

## 🛠 Technologies

- C#
- .NET 8
- ASP.NET Core Web API
- MongoDB
- MediatR
- SignalR
- RabbitMQ
- Docker
- Git

## 🏗 Architecture

The project follows Clean Architecture principles and uses CQRS with MediatR to separate application commands and queries.

Current telemetry flow:

Telemetry → MediatR → MongoDB → SignalR → Client

Geofence violation event flow:

Geofence Violation → RabbitMQ Exchange → Queue → Consumer → ACK/NACK

## 📂 Project Structure

The solution is organized into separate layers to keep business logic independent from infrastructure and presentation concerns.

- **Application** — Use cases, CQRS handlers, abstractions and application events
- **Domain** — Core domain models and business rules
- **Persistence** — MongoDB persistence
- **WebAPI** — API, SignalR, RabbitMQ integration and background services

## 🚧 Project Status

FleetPulse is currently under active development.

Current work is focused on improving RabbitMQ message reliability, retry handling and dead-letter queue strategies.

More features, tests, documentation and deployment improvements will be added as the project evolves.
