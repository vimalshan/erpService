# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Increase NuGet HTTP timeout to 600s to handle slow nuget.org responses
ENV NUGET_HTTP_TIMEOUT=600

COPY nuget.config ./
COPY LoanApplication.slnx* ./
COPY LoanApplication.API/LoanApplication.API.csproj                       LoanApplication.API/
COPY LoanApplication.Application/LoanApplication.Application.csproj       LoanApplication.Application/
COPY LoanApplication.Domain/LoanApplication.Domain.csproj                 LoanApplication.Domain/
COPY LoanApplication.Infrastructure/LoanApplication.Infrastructure.csproj LoanApplication.Infrastructure/

RUN dotnet restore LoanApplication.API/LoanApplication.API.csproj --configfile nuget.config

COPY . .
RUN dotnet publish LoanApplication.API/LoanApplication.API.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "LoanApplication.API.dll"]
