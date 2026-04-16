# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /app
EXPOSE 5176
ENV ASPNETCORE_URLS=http://+:5176

COPY src/ItemMasterService.Domain/ItemMasterService.Domain.csproj \
     ./src/ItemMasterService.Domain/
COPY src/ItemMasterService.Application/ItemMasterService.Application.csproj \
     ./src/ItemMasterService.Application/
COPY src/ItemMasterService.Infrastructure/ItemMasterService.Infrastructure.csproj \
     ./src/ItemMasterService.Infrastructure/
COPY src/ItemMasterService.API/ItemMasterService.API.csproj \
     ./src/ItemMasterService.API/
COPY src/ItemMasterService.Functions/ItemMasterService.Functions.csproj \
     ./src/ItemMasterService.Functions/
COPY src/ItemMasterService.Tests/ItemMasterService.Tests.csproj \
     ./src/ItemMasterService.Tests/

RUN dotnet restore src/ItemMasterService.API/ItemMasterService.API.csproj

COPY . .

WORKDIR /app/src/ItemMasterService.API
RUN dotnet build ItemMasterService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/ItemMasterService.API
RUN dotnet publish ItemMasterService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=publish /app/publish .

# Health check is managed by docker-compose.yml (start_period: 120s)

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ItemMasterService.API.dll"]
