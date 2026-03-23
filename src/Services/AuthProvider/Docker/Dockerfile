# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app

# Copy solution file
COPY AuthProvider.sln ./

# Copy each project file individually to maximise Docker layer cache on restore
COPY src/AuthProvider.Domain/AuthProvider.Domain.csproj \
     ./src/AuthProvider.Domain/
COPY src/AuthProvider.Application/AuthProvider.Application.csproj \
     ./src/AuthProvider.Application/
COPY src/AuthProvider.Infrastructure/AuthProvider.Infrastructure.csproj \
     ./src/AuthProvider.Infrastructure/
COPY src/AuthProvider.API/AuthProvider.API.csproj \
     ./src/AuthProvider.API/
COPY functions/AuthProvider.Functions/AuthProvider.Functions.csproj \
     ./functions/AuthProvider.Functions/

# Restore all projects
RUN dotnet restore AuthProvider.sln

# Copy remaining source code
COPY . .

# Build the API project in Release mode
WORKDIR /app/src/AuthProvider.API
RUN dotnet build AuthProvider.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/AuthProvider.API
RUN dotnet publish AuthProvider.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 5200

# Copy published output
COPY --from=publish /app/publish .

# Health check against the /health endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5200/api/v1/minimal/auth/health || exit 1

# Environment
ENV ASPNETCORE_URLS=http://+:5200
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "AuthProvider.API.dll"]
