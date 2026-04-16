# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5102
ENV ASPNETCORE_URLS=http://+:5102

# Copy solution file
COPY LocationServices.slnx ./

# Copy each project file individually to maximise Docker layer cache on restore
COPY src/LocationServices.Domain/LocationServices.Domain.csproj \
     ./src/LocationServices.Domain/
COPY src/LocationServices.Application/LocationServices.Application.csproj \
     ./src/LocationServices.Application/
COPY src/LocationServices.Infrastructure/LocationServices.Infrastructure.csproj \
     ./src/LocationServices.Infrastructure/
COPY src/LocationServices.API/LocationServices.API.csproj \
     ./src/LocationServices.API/

# Restore all projects
RUN dotnet restore LocationServices.slnx

# Copy remaining source code
COPY . .

# Build the API project in Release mode
WORKDIR /app/src/LocationServices.API
RUN dotnet build LocationServices.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/LocationServices.API
RUN dotnet publish LocationServices.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Health check against the /health endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:7136/health || exit 1

# Environment
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "LocationServices.API.dll"]
