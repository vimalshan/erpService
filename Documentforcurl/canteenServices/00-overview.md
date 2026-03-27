# Canteen Services API Documentation

---

## Overview

Canteen Services is a microservices-based module managing canteen operations including units, cards, meals, deductions, eligibility, items/pricing, swipe transactions, and daily transactions.

**Tech Stack:**
- ASP.NET Core (.NET 8+) with CQRS (MediatR), Repository pattern
- HotChocolate GraphQL (queries, mutations)
- Ocelot API Gateway with rate limiting, circuit breaker, JWT auth, CacheManager
- Entity Framework Core + Dapper (read side)
- SQL Server 2022, RabbitMQ, Azurite
- API Versioning (v1 controllers, v2 Minimal APIs)

---

