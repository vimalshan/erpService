# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY UtilityService.slnx* ./
COPY src/UtilityService.API/UtilityService.API.csproj                           src/UtilityService.API/
COPY src/UtilityService.Application/UtilityService.Application.csproj           src/UtilityService.Application/
COPY src/UtilityService.Domain/UtilityService.Domain.csproj                     src/UtilityService.Domain/
COPY src/UtilityService.Infrastructure/UtilityService.Infrastructure.csproj     src/UtilityService.Infrastructure/

RUN dotnet restore src/UtilityService.API/UtilityService.API.csproj

COPY . .
RUN dotnet publish src/UtilityService.API/UtilityService.API.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5297
ENV ASPNETCORE_URLS=http://+:5297

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production


HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "UtilityService.API.dll"]
