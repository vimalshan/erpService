# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5173
ENV ASPNETCORE_URLS=http://+:5173

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

WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
     CMD bash -c 'echo > /dev/tcp/localhost/5191' || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CardManagement.API.dll"]
