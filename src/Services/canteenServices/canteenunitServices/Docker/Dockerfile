# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5172
ENV ASPNETCORE_URLS=http://+:5172

COPY CanteenUnit.slnx ./

COPY src/CanteenUnit.Domain/CanteenUnit.Domain.csproj \
     ./src/CanteenUnit.Domain/
COPY src/CanteenUnit.Application/CanteenUnit.Application.csproj \
     ./src/CanteenUnit.Application/
COPY src/CanteenUnit.Infrastructure/CanteenUnit.Infrastructure.csproj \
     ./src/CanteenUnit.Infrastructure/
COPY src/CanteenUnit.API/CanteenUnit.API.csproj \
     ./src/CanteenUnit.API/
COPY src/CanteenUnit.Functions/CanteenUnit.Functions.csproj \
     ./src/CanteenUnit.Functions/

RUN dotnet restore CanteenUnit.slnx

COPY . .

WORKDIR /app/src/CanteenUnit.API
RUN dotnet build CanteenUnit.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/CanteenUnit.API
RUN dotnet publish CanteenUnit.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5190/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CanteenUnit.API.dll"]
