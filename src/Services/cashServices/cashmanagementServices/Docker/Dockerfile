# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app

# Copy project files for restore
COPY CashManagement.API/CashManagement.API.csproj ./CashManagement.API/
COPY CashManagement.Application/CashManagement.Application.csproj ./CashManagement.Application/
COPY CashManagement.Domain/CashManagement.Domain.csproj ./CashManagement.Domain/
COPY CashManagement.Infrastructure/CashManagement.Infrastructure.csproj ./CashManagement.Infrastructure/

# Restore dependencies
RUN dotnet restore CashManagement.API/CashManagement.API.csproj

# Copy all source code
COPY . .

# Build
WORKDIR /app/CashManagement.API
RUN dotnet build CashManagement.API.csproj -c Release -o /app/build

# Publish stage
FROM builder AS publish
WORKDIR /app/CashManagement.API
RUN dotnet publish CashManagement.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 5249

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5249/health || exit 1

ENV ASPNETCORE_URLS=http://+:5249
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CashManagement.API.dll"]
