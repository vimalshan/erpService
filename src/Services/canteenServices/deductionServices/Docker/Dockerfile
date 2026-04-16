# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5174
ENV ASPNETCORE_URLS=http://+:5174

COPY src/DeductionService.slnx ./src/

COPY src/DeductionService.Domain/DeductionService.Domain.csproj \
     ./src/DeductionService.Domain/
COPY src/DeductionService.Application/DeductionService.Application.csproj \
     ./src/DeductionService.Application/
COPY src/DeductionService.Infrastructure/DeductionService.Infrastructure.csproj \
     ./src/DeductionService.Infrastructure/
COPY src/DeductionService.API/DeductionService.API.csproj \
     ./src/DeductionService.API/
COPY src/DeductionService.Functions/DeductionService.Functions.csproj \
     ./src/DeductionService.Functions/

RUN dotnet restore src/DeductionService.slnx

COPY . .

WORKDIR /app/src/DeductionService.API
RUN dotnet build DeductionService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/DeductionService.API
RUN dotnet publish DeductionService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5192/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "DeductionService.API.dll"]
