# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5178
ENV ASPNETCORE_URLS=http://+:5178

COPY src/SwipeTransactionService.slnx ./src/

COPY src/SwipeTransactionService.Domain/SwipeTransactionService.Domain.csproj \
     ./src/SwipeTransactionService.Domain/
COPY src/SwipeTransactionService.Application/SwipeTransactionService.Application.csproj \
     ./src/SwipeTransactionService.Application/
COPY src/SwipeTransactionService.Infrastructure/SwipeTransactionService.Infrastructure.csproj \
     ./src/SwipeTransactionService.Infrastructure/
COPY src/SwipeTransactionService.API/SwipeTransactionService.API.csproj \
     ./src/SwipeTransactionService.API/
COPY src/SwipeTransactionService.Functions/SwipeTransactionService.Functions.csproj \
     ./src/SwipeTransactionService.Functions/
COPY src/SwipeTransactionService.Tests/SwipeTransactionService.Tests.csproj \
     ./src/SwipeTransactionService.Tests/

RUN dotnet restore src/SwipeTransactionService.slnx

COPY . .

WORKDIR /app/src/SwipeTransactionService.API
RUN dotnet build SwipeTransactionService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/SwipeTransactionService.API
RUN dotnet publish SwipeTransactionService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5196/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "SwipeTransactionService.API.dll"]
