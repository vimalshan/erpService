# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /app
EXPOSE 5101
ENV ASPNETCORE_URLS=http://+:5101

# Copy solution and project files
COPY FinyearServices.sln ./
COPY src/FinyearAPI/FinyearAPI.csproj ./src/FinyearAPI/
COPY src/FinyearAPI.Domain/FinyearAPI.Domain.csproj ./src/FinyearAPI.Domain/
COPY src/FinyearAPI.Application/FinyearAPI.Application.csproj ./src/FinyearAPI.Application/
COPY src/FinyearAPI.Infrastructure/FinyearAPI.Infrastructure.csproj ./src/FinyearAPI.Infrastructure/
COPY src/FinyearAPI.Gateway/FinyearAPI.Gateway.csproj ./src/FinyearAPI.Gateway/
COPY src/FinyearAPI.GraphQL/FinyearAPI.GraphQL.csproj ./src/FinyearAPI.GraphQL/
COPY src/Services/AuthProvider/AuthProvider.csproj ./src/Services/AuthProvider/

# Restore dependencies
RUN dotnet restore FinyearServices.sln

# Copy all source code
COPY . .

# Build the solution
WORKDIR /app/src/FinyearAPI
RUN dotnet build FinyearAPI.csproj -c Release -o /app/build

# Publish stage
FROM builder AS publish
WORKDIR /app/src/FinyearAPI
RUN dotnet publish FinyearAPI.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Install curl for health checks
WORKDIR /app

# Copy published application
COPY --from=publish /app/publish .

# Health check - Check if port is listening (not API endpoint which requires DB)
HEALTHCHECK --interval=30s --timeout=10s --start-period=10s --retries=3 \
  CMD wget -qO- http://localhost:5000/swagger || exit 1

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "FinyearAPI.dll"]
