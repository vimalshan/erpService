# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app

COPY src/EligibilityService.slnx ./src/

COPY src/EligibilityService.Domain/EligibilityService.Domain.csproj \
     ./src/EligibilityService.Domain/
COPY src/EligibilityService.Application/EligibilityService.Application.csproj \
     ./src/EligibilityService.Application/
COPY src/EligibilityService.Infrastructure/EligibilityService.Infrastructure.csproj \
     ./src/EligibilityService.Infrastructure/
COPY src/EligibilityService.API/EligibilityService.API.csproj \
     ./src/EligibilityService.API/
COPY src/EligibilityService.Functions/EligibilityService.Functions.csproj \
     ./src/EligibilityService.Functions/

RUN dotnet restore src/EligibilityService.slnx

COPY . .

WORKDIR /app/src/EligibilityService.API
RUN dotnet build EligibilityService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/EligibilityService.API
RUN dotnet publish EligibilityService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 5193

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5193/health || exit 1

ENV ASPNETCORE_URLS=http://+:5193
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "EligibilityService.API.dll"]
