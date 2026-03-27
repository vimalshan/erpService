# AuthProvider API Documentation

---

## Overview

AuthProvider is a standalone authentication and authorization microservice built with:

- **ASP.NET Core** (.NET 8+) with CQRS (MediatR), Repository + Unit of Work pattern
- **HotChocolate** GraphQL server (queries, mutations, subscriptions via WebSocket)
- **Entity Framework Core** + **Dapper** (read side) with SQL Server 2022
- **JWT Bearer** authentication with refresh token rotation
- **API Versioning** (URL segment `/v1/`, `/v2/` + `X-Api-Version` header)
- **Azure Functions** (Token cleanup timer, User created Service Bus trigger)
- **RabbitMQ** for domain event publishing
- **Azure Blob Storage** for event archival
- **Polly** resilience (retry + circuit breaker for external auth)
- **Serilog** structured logging

### Authorization Policies

| Policy | Requirement |
|---|---|
| `AdminOnly` | Role = `ADMIN` |
| `UserOrAdmin` | Role = `USER` or `ADMIN` |
| `RequireEmailVerified` | Custom assertion on email verification |

---

