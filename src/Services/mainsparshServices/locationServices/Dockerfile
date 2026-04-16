# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 5316
ENV ASPNETCORE_URLS=http://+:5316

# Copy project files for restore
COPY ["LocationService.API/LocationService.API.csproj", "LocationService.API/"]
COPY ["LocationService.Application/LocationService.Application.csproj", "LocationService.Application/"]
COPY ["LocationService.Domain/LocationService.Domain.csproj", "LocationService.Domain/"]
COPY ["LocationService.Infrastructure/LocationService.Infrastructure.csproj", "LocationService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "LocationService.API/LocationService.API.csproj"

# Copy source code
COPY LocationService.API/ LocationService.API/
COPY LocationService.Application/ LocationService.Application/
COPY LocationService.Domain/ LocationService.Domain/
COPY LocationService.Infrastructure/ LocationService.Infrastructure/

# Publish
RUN dotnet publish "LocationService.API/LocationService.API.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
COPY --from=build /publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "LocationService.API.dll"]
