# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY LoanApiGateway.sln* ./
COPY src/LoanApiGateway/LoanApiGateway.csproj src/LoanApiGateway/

RUN dotnet restore src/LoanApiGateway/LoanApiGateway.csproj

COPY . .
RUN dotnet publish src/LoanApiGateway/LoanApiGateway.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5290
ENV ASPNETCORE_URLS=http://+:5290

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production


HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:8080/health/live || exit 1

ENTRYPOINT ["dotnet", "LoanApiGateway.dll"]
