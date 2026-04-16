# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5103
ENV ASPNETCORE_URLS=http://+:5103

# Copy solution file
COPY LovService.slnx ./

# Copy each project file individually to maximise Docker layer cache on restore
COPY src/LovService.Domain/LovService.Domain.csproj \
     ./src/LovService.Domain/
COPY src/LovService.Application/LovService.Application.csproj \
     ./src/LovService.Application/
COPY src/LovService.Infrastructure/LovService.Infrastructure.csproj \
     ./src/LovService.Infrastructure/
COPY src/LovService.API/LovService.API.csproj \
     ./src/LovService.API/
COPY src/LovService.AzureFunctions/LovService.AzureFunctions.csproj \
     ./src/LovService.AzureFunctions/

# Restore all projects
RUN dotnet restore LovService.slnx

# Copy remaining source code
COPY . .

# Build the API project in Release mode
WORKDIR /app/src/LovService.API
RUN dotnet build LovService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/LovService.API
RUN dotnet publish LovService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

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
ENTRYPOINT ["dotnet", "LovService.API.dll"]
