# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app

# Copy solution file
COPY StationeryServices.slnx ./

# Copy each project file individually to maximise Docker layer cache on restore
COPY src/Stationery.Domain/Stationery.Domain.csproj \
     ./src/Stationery.Domain/
COPY src/Stationery.Application/Stationery.Application.csproj \
     ./src/Stationery.Application/
COPY src/Stationery.Infrastructure/Stationery.Infrastructure.csproj \
     ./src/Stationery.Infrastructure/
COPY src/Stationery.Api/Stationery.Api.csproj \
     ./src/Stationery.Api/
COPY src/Stationery.Functions/Stationery.Functions.csproj \
     ./src/Stationery.Functions/
COPY tests/Stationery.UnitTests/Stationery.UnitTests.csproj \
     ./tests/Stationery.UnitTests/

# Restore all projects
RUN dotnet restore StationeryServices.slnx

# Copy remaining source code
COPY . .

# Build the API project in Release mode
WORKDIR /app/src/Stationery.Api
RUN dotnet build Stationery.Api.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/Stationery.Api
RUN dotnet publish Stationery.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 5181

# Copy published output
COPY --from=publish /app/publish .

# Health check against the /health endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5181/health || exit 1

# Environment
ENV ASPNETCORE_URLS=http://+:5181
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "Stationery.Api.dll"]
