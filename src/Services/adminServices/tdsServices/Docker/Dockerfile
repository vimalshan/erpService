# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5106
ENV ASPNETCORE_URLS=http://+:5106

# Copy solution file
COPY TdsService.slnx ./

# Copy each project file individually to maximise Docker layer cache on restore
COPY src/TdsService.Domain/TdsService.Domain.csproj \
     ./src/TdsService.Domain/
COPY src/TdsService.Application/TdsService.Application.csproj \
     ./src/TdsService.Application/
COPY src/TdsService.Infrastructure/TdsService.Infrastructure.csproj \
     ./src/TdsService.Infrastructure/
COPY src/TdsService.API/TdsService.API.csproj \
     ./src/TdsService.API/
COPY functions/TdsService.Functions/TdsService.Functions.csproj \
     ./functions/TdsService.Functions/
COPY tests/TdsService.Application.Tests/TdsService.Application.Tests.csproj \
     ./tests/TdsService.Application.Tests/
COPY tests/TdsService.Domain.Tests/TdsService.Domain.Tests.csproj \
     ./tests/TdsService.Domain.Tests/
COPY tests/TdsService.Integration.Tests/TdsService.Integration.Tests.csproj \
     ./tests/TdsService.Integration.Tests/

# Restore all projects
RUN dotnet restore TdsService.slnx

# Copy remaining source code
COPY . .

# Build the API project in Release mode
WORKDIR /app/src/TdsService.API
RUN dotnet build TdsService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/TdsService.API
RUN dotnet publish TdsService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Health check against the /health endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5181/health || exit 1

# Environment
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "TdsService.API.dll"]
