# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Hr.ApiGateway/Hr.ApiGateway.csproj src/Hr.ApiGateway/

RUN dotnet restore src/Hr.ApiGateway/Hr.ApiGateway.csproj

COPY . .
RUN dotnet publish src/Hr.ApiGateway/Hr.ApiGateway.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5250
ENV ASPNETCORE_URLS=http://+:5250

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5250/health/live || exit 1

ENTRYPOINT ["dotnet", "Hr.ApiGateway.dll"]
