# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first (for layer cache)
COPY LoanTransaction.slnx* ./
COPY src/LoanTransaction.API/LoanTransaction.API.csproj                   src/LoanTransaction.API/
COPY src/LoanTransaction.Application/LoanTransaction.Application.csproj   src/LoanTransaction.Application/
COPY src/LoanTransaction.Domain/LoanTransaction.Domain.csproj             src/LoanTransaction.Domain/
COPY src/LoanTransaction.Infrastructure/LoanTransaction.Infrastructure.csproj src/LoanTransaction.Infrastructure/

RUN dotnet restore src/LoanTransaction.API/LoanTransaction.API.csproj

# Copy everything and build
COPY . .
RUN dotnet publish src/LoanTransaction.API/LoanTransaction.API.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5295
ENV ASPNETCORE_URLS=http://+:5295

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production


HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "LoanTransaction.API.dll"]
