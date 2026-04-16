# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5194
ENV ASPNETCORE_URLS=http://+:5194

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
WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5032/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "EmailNotification.API.dll"]
