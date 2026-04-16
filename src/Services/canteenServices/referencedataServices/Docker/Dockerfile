# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5177
ENV ASPNETCORE_URLS=http://+:5177

COPY ReferenceDataService.slnx ./

COPY src/ReferenceDataService.Domain/ReferenceDataService.Domain.csproj \
     ./src/ReferenceDataService.Domain/
COPY src/ReferenceDataService.Application/ReferenceDataService.Application.csproj \
     ./src/ReferenceDataService.Application/
COPY src/ReferenceDataService.Infrastructure/ReferenceDataService.Infrastructure.csproj \
     ./src/ReferenceDataService.Infrastructure/
COPY src/ReferenceDataService.API/ReferenceDataService.API.csproj \
     ./src/ReferenceDataService.API/
COPY src/ReferenceDataService.Functions/ReferenceDataService.Functions.csproj \
     ./src/ReferenceDataService.Functions/

RUN dotnet restore ReferenceDataService.slnx

COPY . .

WORKDIR /app/src/ReferenceDataService.API
RUN dotnet build ReferenceDataService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/ReferenceDataService.API
RUN dotnet publish ReferenceDataService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5195/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ReferenceDataService.API.dll"]
