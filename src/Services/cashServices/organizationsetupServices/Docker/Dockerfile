# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5196
ENV ASPNETCORE_URLS=http://+:5196

# Copy project files for restore
COPY src/OrganizationSetup.API/OrganizationSetup.API.csproj ./src/OrganizationSetup.API/
COPY src/OrganizationSetup.Application/OrganizationSetup.Application.csproj ./src/OrganizationSetup.Application/
COPY src/OrganizationSetup.Domain/OrganizationSetup.Domain.csproj ./src/OrganizationSetup.Domain/
COPY src/OrganizationSetup.Infrastructure/OrganizationSetup.Infrastructure.csproj ./src/OrganizationSetup.Infrastructure/

# Restore dependencies
RUN dotnet restore src/OrganizationSetup.API/OrganizationSetup.API.csproj

# Copy all source code
COPY . .

# Build
WORKDIR /app/src/OrganizationSetup.API
RUN dotnet build OrganizationSetup.API.csproj -c Release -o /app/build

# Publish stage
FROM builder AS publish
WORKDIR /app/src/OrganizationSetup.API
RUN dotnet publish OrganizationSetup.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:5099/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "OrganizationSetup.API.dll"]
