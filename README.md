# P2P Banking System ??

A distributed peer-to-peer banking system built with .NET 8, implementing Clean Architecture principles and Event-Driven microservices.

## ??? Architecture Overview

This system follows **Clean Architecture** principles with **Domain-Driven Design (DDD)** and implements an **Event-Driven Architecture** using microservices pattern.

### Services

- **Transfer Service**: Handles money transfer operations between users
- **Notification Service**: Manages notifications for transfer events
- **Contracts**: Shared contracts and events between services

## ??? Technology Stack

### Backend
- **.NET 8** - Runtime and framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM with PostgreSQL
- **MassTransit** - Distributed application framework with RabbitMQ
- **MediatR** - In-process messaging and CQRS implementation
- **FluentValidation** - Input validation

### Infrastructure
- **PostgreSQL** - Primary database
- **RabbitMQ** - Message broker for inter-service communication
- **Docker & Docker Compose** - Containerization

### Patterns & Practices
- **Clean Architecture** with separation of concerns
- **Domain-Driven Design (DDD)**
- **CQRS** (Command Query Responsibility Segregation)
- **Event Sourcing** with Outbox Pattern
- **Repository Pattern**
- **Unit of Work Pattern**

## ??? Project Structure

```
P2PBankingSystem/
??? TransferService/
?   ??? TransferService.API/          # Web API layer
?   ??? TransferService.Application/  # Application logic & CQRS
?   ??? TransferService.Domain/       # Domain entities & business logic
?   ??? TransferService.Infrastructure/ # External dependencies
?   ??? TransferService.Persistence/  # Data access layer
??? NotificationService/
?   ??? NotificationService.API/      # Web API layer
?   ??? NotificationService.Application/ # Application logic
?   ??? NotificationService.Domain/   # Domain entities
?   ??? NotificationService.Infrastructure/ # Data access & messaging
??? Contracts/                        # Shared contracts & events
??? docker-compose.yml               # Container orchestration
```

## ?? Features

### Transfer Service
- **Create Money Transfers**: Process P2P money transfers between users
- **Domain Events**: Publishes `TransferCreatedDomainEvent` for other services
- **Validation**: Input validation using FluentValidation
- **Outbox Pattern**: Ensures reliable message delivery
- **Swagger Documentation**: API documentation and testing interface

### Notification Service
- **Event Processing**: Consumes transfer events via RabbitMQ
- **Notification Handling**: Processes transfer notifications
- **Inbox Pattern**: Ensures exactly-once message processing

## ?? Getting Started

### Prerequisites
- **.NET 8 SDK**
- **Docker & Docker Compose**
- **Git**

### Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd P2PBankingSystem
   ```

2. **Start the infrastructure services**
   ```bash
   docker-compose up -d postgres rabbitmq
   ```

3. **Run the services**
   ```bash
   # Terminal 1 - Transfer Service
   cd TransferService/TransferService.API
   dotnet run

   # Terminal 2 - Notification Service
   cd NotificationService/NotificationService.API
   dotnet run
   ```

   **Or using Docker Compose:**
   ```bash
   docker-compose up --build
   ```

### API Endpoints

#### Transfer Service
- **POST** `/api/transfers` - Create a new transfer
  ```json
  {
    "senderUserId": "guid",
    "receiverUserId": "guid",
    "amount": {
      "amount": 100.00,
      "currency": "USD"
    }
  }
  ```

### Accessing Services
- **Transfer Service API**: http://localhost:5000
- **Transfer Service Swagger**: http://localhost:5000/swagger
- **Notification Service**: Running as background service
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **PostgreSQL**: localhost:5432 (postgres/postgres)

## ??? Database Schema

### Transfer Service Database (TransferDb)
- **Transfers**: Main transfer entities with money value objects
- **OutboxState**: Outbox pattern state management
- **OutboxMessage**: Outbox pattern message storage
- **InboxState**: Inbox pattern for received messages

### Notification Service Database (NotificationDb)
- **InboxState**: Inbox pattern for message deduplication
- **OutboxState**: Outbox pattern state management

## ?? Event Flow

1. **Transfer Creation**: User creates a transfer via Transfer Service API
2. **Domain Event**: `TransferCreatedDomainEvent` is raised
3. **Event Publishing**: Event is published to RabbitMQ via Outbox pattern
4. **Event Consumption**: Notification Service consumes the event
5. **Notification Processing**: Notification is processed and stored

## ?? Development

### Adding a New Migration

**Transfer Service:**
```bash
cd TransferService/TransferService.Persistence
dotnet ef migrations add <MigrationName> --startup-project ../TransferService.API
```

**Notification Service:**
```bash
cd NotificationService/NotificationService.Infrastructure
dotnet ef migrations add <MigrationName> --startup-project ../NotificationService.API
```

### Building the Project
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

## ?? Configuration

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Application environment (Development/Production/Docker)
- `ConnectionStrings__Default`: PostgreSQL connection string
- `RabbitMq__Host`: RabbitMQ host
- `RabbitMq__Username`: RabbitMQ username
- `RabbitMq__Password`: RabbitMQ password

### Configuration Files
- `appsettings.json`: Base configuration
- `appsettings.Development.json`: Development-specific settings
- `appsettings.Docker.json`: Docker environment settings

## ?? Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## ?? TODO / Roadmap

- [ ] Add authentication and authorization
- [ ] Implement user management service
- [ ] Add account balance management
- [ ] Implement transaction history
- [ ] Add email/SMS notifications
- [ ] Add integration tests
- [ ] Implement API rate limiting
- [ ] Add monitoring and logging
- [ ] Add caching layer
- [ ] Implement saga pattern for complex workflows

## ?? License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## ?? Related Technologies

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [MassTransit](https://masstransit-project.com/)
- [MediatR](https://github.com/jbogard/MediatR)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [PostgreSQL](https://www.postgresql.org/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Docker](https://www.docker.com/)

---

**Built with ?? using Clean Architecture and Domain-Driven Design principles**