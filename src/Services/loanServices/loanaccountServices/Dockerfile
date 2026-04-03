# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY LoanAccountService.sln* ./
COPY src/LoanAccount.API/LoanAccount.API.csproj                         src/LoanAccount.API/
COPY src/LoanAccount.Application/LoanAccount.Application.csproj         src/LoanAccount.Application/
COPY src/LoanAccount.Domain/LoanAccount.Domain.csproj                   src/LoanAccount.Domain/
COPY src/LoanAccount.Infrastructure/LoanAccount.Infrastructure.csproj   src/LoanAccount.Infrastructure/

RUN dotnet restore src/LoanAccount.API/LoanAccount.API.csproj

COPY . .
RUN dotnet publish src/LoanAccount.API/LoanAccount.API.csproj \
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

ENTRYPOINT ["dotnet", "LoanAccount.API.dll"]
