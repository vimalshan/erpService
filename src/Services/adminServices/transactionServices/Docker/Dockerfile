# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5107
ENV ASPNETCORE_URLS=http://+:5107

# Copy solution file
COPY TransactionService.slnx ./

# Copy each project file individually to maximise Docker layer cache on restore
COPY src/TransactionService.Domain/TransactionService.Domain.csproj \
     ./src/TransactionService.Domain/
COPY src/TransactionService.Application/TransactionService.Application.csproj \
     ./src/TransactionService.Application/
COPY src/TransactionService.Infrastructure/TransactionService.Infrastructure.csproj \
     ./src/TransactionService.Infrastructure/
COPY src/TransactionService.API/TransactionService.API.csproj \
     ./src/TransactionService.API/
COPY src/TransactionService.Functions/TransactionService.Functions.csproj \
     ./src/TransactionService.Functions/
COPY tests/TransactionService.IntegrationTests/TransactionService.IntegrationTests.csproj \
     ./tests/TransactionService.IntegrationTests/
COPY tests/TransactionService.UnitTests/TransactionService.UnitTests.csproj \
     ./tests/TransactionService.UnitTests/

# Restore all projects
RUN dotnet restore TransactionService.slnx

# Copy remaining source code
COPY . .

# Build the API project in Release mode
WORKDIR /app/src/TransactionService.API
RUN dotnet build TransactionService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/TransactionService.API
RUN dotnet publish TransactionService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Install curl for health checks
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Health check against the /health endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5185/health || exit 1

# Environment
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "TransactionService.API.dll"]
