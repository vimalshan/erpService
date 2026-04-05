# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy project files for restore
COPY ["BookingService.API/BookingService.API.csproj", "BookingService.API/"]
COPY ["BookingService.Application/BookingService.Application.csproj", "BookingService.Application/"]
COPY ["BookingService.Domain/BookingService.Domain.csproj", "BookingService.Domain/"]
COPY ["BookingService.Infrastructure/BookingService.Infrastructure.csproj", "BookingService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "BookingService.API/BookingService.API.csproj"

# Copy source code
COPY BookingService.API/ BookingService.API/
COPY BookingService.Application/ BookingService.Application/
COPY BookingService.Domain/ BookingService.Domain/
COPY BookingService.Infrastructure/ BookingService.Infrastructure/

# Publish
RUN dotnet publish "BookingService.API/BookingService.API.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
COPY --from=build /publish .

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 80

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "BookingService.API.dll"]
