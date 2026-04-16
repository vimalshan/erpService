# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /app
EXPOSE 5171
ENV ASPNETCORE_URLS=http://+:5171

COPY src/CanteenTransactionService.Domain/CanteenTransactionService.Domain.csproj \
     ./src/CanteenTransactionService.Domain/
COPY src/CanteenTransactionService.Application/CanteenTransactionService.Application.csproj \
     ./src/CanteenTransactionService.Application/
COPY src/CanteenTransactionService.Infrastructure/CanteenTransactionService.Infrastructure.csproj \
     ./src/CanteenTransactionService.Infrastructure/
COPY src/CanteenTransactionService.API/CanteenTransactionService.API.csproj \
     ./src/CanteenTransactionService.API/
COPY src/CanteenTransactionService.Functions/CanteenTransactionService.Functions.csproj \
     ./src/CanteenTransactionService.Functions/

RUN dotnet restore src/CanteenTransactionService.API/CanteenTransactionService.API.csproj

COPY . .

WORKDIR /app/src/CanteenTransactionService.API
RUN dotnet build CanteenTransactionService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/CanteenTransactionService.API
RUN dotnet publish CanteenTransactionService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5195/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CanteenTransactionService.API.dll"]
