# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY LoanDefinition.slnx* ./
COPY src/LoanDefinition.API/LoanDefinition.API.csproj                           src/LoanDefinition.API/
COPY src/LoanDefinition.Application/LoanDefinition.Application.csproj           src/LoanDefinition.Application/
COPY src/LoanDefinition.Domain/LoanDefinition.Domain.csproj                     src/LoanDefinition.Domain/
COPY src/LoanDefinition.Infrastructure/LoanDefinition.Infrastructure.csproj     src/LoanDefinition.Infrastructure/
COPY src/LoanDefinition.SharedKernel/LoanDefinition.SharedKernel.csproj         src/LoanDefinition.SharedKernel/

RUN dotnet restore src/LoanDefinition.API/LoanDefinition.API.csproj

COPY . .
RUN dotnet publish src/LoanDefinition.API/LoanDefinition.API.csproj \
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

ENTRYPOINT ["dotnet", "LoanDefinition.API.dll"]
