# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/CompensationBenefits.API/CompensationBenefits.API.csproj src/CompensationBenefits.API/

RUN dotnet restore src/CompensationBenefits.API/CompensationBenefits.API.csproj

COPY . .
RUN dotnet publish src/CompensationBenefits.API/CompensationBenefits.API.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5252
ENV ASPNETCORE_URLS=http://+:5252

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5252/health/live || exit 1

ENTRYPOINT ["dotnet", "CompensationBenefits.API.dll"]
