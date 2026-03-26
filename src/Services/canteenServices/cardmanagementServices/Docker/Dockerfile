# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app

COPY src/CardManagement.slnx ./src/

COPY src/CardManagement.Domain/CardManagement.Domain.csproj \
     ./src/CardManagement.Domain/
COPY src/CardManagement.Application/CardManagement.Application.csproj \
     ./src/CardManagement.Application/
COPY src/CardManagement.Infrastructure/CardManagement.Infrastructure.csproj \
     ./src/CardManagement.Infrastructure/
COPY src/CardManagement.API/CardManagement.API.csproj \
     ./src/CardManagement.API/
COPY src/CardManagement.Functions/CardManagement.Functions.csproj \
     ./src/CardManagement.Functions/

RUN dotnet restore src/CardManagement.slnx

COPY . .

WORKDIR /app/src/CardManagement.API
RUN dotnet build CardManagement.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/CardManagement.API
RUN dotnet publish CardManagement.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 5191

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5191/health || exit 1

ENV ASPNETCORE_URLS=http://+:5191
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CardManagement.API.dll"]
