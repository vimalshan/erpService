# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

WORKDIR /app
EXPOSE 5192
ENV ASPNETCORE_URLS=http://+:5192

# Copy project files for restore
COPY src/CurrencyManagement.API/CurrencyManagement.API.csproj ./src/CurrencyManagement.API/
COPY src/CurrencyManagement.Application/CurrencyManagement.Application.csproj ./src/CurrencyManagement.Application/
COPY src/CurrencyManagement.Domain/CurrencyManagement.Domain.csproj ./src/CurrencyManagement.Domain/
COPY src/CurrencyManagement.Infrastructure/CurrencyManagement.Infrastructure.csproj ./src/CurrencyManagement.Infrastructure/

# Restore dependencies
RUN dotnet restore src/CurrencyManagement.API/CurrencyManagement.API.csproj

# Copy all source code
COPY . .

# Build
WORKDIR /app/src/CurrencyManagement.API
RUN dotnet build CurrencyManagement.API.csproj -c Release -o /app/build

# Publish stage
FROM builder AS publish
WORKDIR /app/src/CurrencyManagement.API
RUN dotnet publish CurrencyManagement.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5031/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CurrencyManagement.API.dll"]
