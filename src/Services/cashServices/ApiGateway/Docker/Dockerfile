# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5190
ENV ASPNETCORE_URLS=http://+:5190

# Copy project files for restore
COPY ApiGateway.API/ApiGateway.API.csproj ./ApiGateway.API/

# Restore dependencies
RUN dotnet restore ApiGateway.API/ApiGateway.API.csproj

# Copy all source code
COPY . .

# Build
WORKDIR /app/ApiGateway.API
RUN dotnet build ApiGateway.API.csproj -c Release -o /app/build

# Publish stage
FROM builder AS publish
WORKDIR /app/ApiGateway.API
RUN dotnet publish ApiGateway.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5000/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ApiGateway.API.dll"]
