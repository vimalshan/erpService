# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY ["ApprovalService.sln", "."]
COPY ["src/ApprovalService.Domain/ApprovalService.Domain.csproj", "src/ApprovalService.Domain/"]
COPY ["src/ApprovalService.Application/ApprovalService.Application.csproj", "src/ApprovalService.Application/"]
COPY ["src/ApprovalService.Infrastructure/ApprovalService.Infrastructure.csproj", "src/ApprovalService.Infrastructure/"]
COPY ["src/ApprovalService.API/ApprovalService.API.csproj", "src/ApprovalService.API/"]

# Restore dependencies
RUN dotnet restore src/ApprovalService.API/ApprovalService.API.csproj

# Copy source code
COPY src/ApprovalService.Domain/ src/ApprovalService.Domain/
COPY src/ApprovalService.Application/ src/ApprovalService.Application/
COPY src/ApprovalService.Infrastructure/ src/ApprovalService.Infrastructure/
COPY src/ApprovalService.API/ src/ApprovalService.API/

# Build application
RUN dotnet build src/ApprovalService.API/ApprovalService.API.csproj -c Release -o /build

# Publish
RUN dotnet publish src/ApprovalService.API/ApprovalService.API.csproj -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=build /publish .

# Set environment
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose ports
EXPOSE 5000

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:5000/health || exit 1

# Run application
ENTRYPOINT ["dotnet", "ApprovalService.API.dll"]
