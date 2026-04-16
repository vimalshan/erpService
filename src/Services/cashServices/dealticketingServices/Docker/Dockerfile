# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5193
ENV ASPNETCORE_URLS=http://+:5193

# Copy project files for restore
COPY src/DealTicketing.API/DealTicketing.API.csproj ./src/DealTicketing.API/
COPY src/DealTicketing.Application/DealTicketing.Application.csproj ./src/DealTicketing.Application/
COPY src/DealTicketing.Domain/DealTicketing.Domain.csproj ./src/DealTicketing.Domain/
COPY src/DealTicketing.Infrastructure/DealTicketing.Infrastructure.csproj ./src/DealTicketing.Infrastructure/

# Restore dependencies
RUN dotnet restore src/DealTicketing.API/DealTicketing.API.csproj

# Copy all source code
COPY . .

# Build
WORKDIR /app/src/DealTicketing.API
RUN dotnet build DealTicketing.API.csproj -c Release -o /app/build

# Publish stage
FROM builder AS publish
WORKDIR /app/src/DealTicketing.API
RUN dotnet publish DealTicketing.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5081/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "DealTicketing.API.dll"]
