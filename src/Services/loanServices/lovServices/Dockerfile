# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY LovService.slnx* ./
COPY src/LovService.API/LovService.API.csproj                           src/LovService.API/
COPY src/LovService.Application/LovService.Application.csproj           src/LovService.Application/
COPY src/LovService.Domain/LovService.Domain.csproj                     src/LovService.Domain/
COPY src/LovService.Infrastructure/LovService.Infrastructure.csproj     src/LovService.Infrastructure/

RUN dotnet restore src/LovService.API/LovService.API.csproj

COPY . .
RUN dotnet publish src/LovService.API/LovService.API.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5296
ENV ASPNETCORE_URLS=http://+:5296

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production


HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "LovService.API.dll"]
