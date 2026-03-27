# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app

# Copy project files for restore
COPY EmailNotification/src/EmailNotification.API/EmailNotification.API.csproj ./EmailNotification/src/EmailNotification.API/
COPY EmailNotification/src/EmailNotification.Application/EmailNotification.Application.csproj ./EmailNotification/src/EmailNotification.Application/
COPY EmailNotification/src/EmailNotification.Domain/EmailNotification.Domain.csproj ./EmailNotification/src/EmailNotification.Domain/
COPY EmailNotification/src/EmailNotification.Infrastructure/EmailNotification.Infrastructure.csproj ./EmailNotification/src/EmailNotification.Infrastructure/

# Restore dependencies
RUN dotnet restore EmailNotification/src/EmailNotification.API/EmailNotification.API.csproj

# Copy all source code
COPY . .

# Build
WORKDIR /app/EmailNotification/src/EmailNotification.API
RUN dotnet build EmailNotification.API.csproj -c Release -o /app/build

# Publish stage
FROM builder AS publish
WORKDIR /app/EmailNotification/src/EmailNotification.API
RUN dotnet publish EmailNotification.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 5032

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5032/health || exit 1

ENV ASPNETCORE_URLS=http://+:5032
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "EmailNotification.API.dll"]
