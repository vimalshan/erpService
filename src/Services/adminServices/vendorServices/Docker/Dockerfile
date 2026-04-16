# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5108
ENV ASPNETCORE_URLS=http://+:5108

# Copy solution file
COPY VendorService.slnx ./

# Copy each project file individually to maximise Docker layer cache on restore
COPY src/VendorService.Domain/VendorService.Domain.csproj \
     ./src/VendorService.Domain/
COPY src/VendorService.Application/VendorService.Application.csproj \
     ./src/VendorService.Application/
COPY src/VendorService.Infrastructure/VendorService.Infrastructure.csproj \
     ./src/VendorService.Infrastructure/
COPY src/VendorService.API/VendorService.API.csproj \
     ./src/VendorService.API/
COPY src/VendorService.Functions/VendorService.Functions.csproj \
     ./src/VendorService.Functions/
COPY tests/VendorService.IntegrationTests/VendorService.IntegrationTests.csproj \
     ./tests/VendorService.IntegrationTests/
COPY tests/VendorService.UnitTests/VendorService.UnitTests.csproj \
     ./tests/VendorService.UnitTests/

# Restore all projects
RUN dotnet restore VendorService.slnx

# Copy remaining source code
COPY . .

# Build the API project in Release mode
WORKDIR /app/src/VendorService.API
RUN dotnet build VendorService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/VendorService.API
RUN dotnet publish VendorService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

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
ENTRYPOINT ["dotnet", "VendorService.API.dll"]
