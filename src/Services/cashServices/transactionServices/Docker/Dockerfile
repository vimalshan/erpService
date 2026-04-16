# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5197
ENV ASPNETCORE_URLS=http://+:5197

# Copy project files for restore
COPY TransactionProcessing.API/TransactionProcessing.API.csproj ./TransactionProcessing.API/
COPY TransactionProcessing.Application/TransactionProcessing.Application.csproj ./TransactionProcessing.Application/
COPY TransactionProcessing.Domain/TransactionProcessing.Domain.csproj ./TransactionProcessing.Domain/
COPY TransactionProcessing.Infrastructure/TransactionProcessing.Infrastructure.csproj ./TransactionProcessing.Infrastructure/

# Restore dependencies
RUN dotnet restore TransactionProcessing.API/TransactionProcessing.API.csproj

# Copy all source code
COPY . .

# Build
WORKDIR /app/TransactionProcessing.API
RUN dotnet build TransactionProcessing.API.csproj -c Release -o /app/build

# Publish stage
FROM builder AS publish
WORKDIR /app/TransactionProcessing.API
RUN dotnet publish TransactionProcessing.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5100/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TransactionProcessing.API.dll"]
