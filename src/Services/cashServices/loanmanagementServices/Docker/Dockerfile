# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5195
ENV ASPNETCORE_URLS=http://+:5195

# Copy project files for restore
COPY LoanManagement.API/LoanManagement.API.csproj ./LoanManagement.API/
COPY LoanManagement.Application/LoanManagement.Application.csproj ./LoanManagement.Application/
COPY LoanManagement.Domain/LoanManagement.Domain.csproj ./LoanManagement.Domain/
COPY LoanManagement.Infrastructure/LoanManagement.Infrastructure.csproj ./LoanManagement.Infrastructure/

# Restore dependencies
RUN dotnet restore LoanManagement.API/LoanManagement.API.csproj

# Copy all source code
COPY . .

# Build
WORKDIR /app/LoanManagement.API
RUN dotnet build LoanManagement.API.csproj -c Release -o /app/build

# Publish stage
FROM builder AS publish
WORKDIR /app/LoanManagement.API
RUN dotnet publish LoanManagement.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5268/health || exit 1

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "LoanManagement.API.dll"]
