# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
EXPOSE 5312
ENV ASPNETCORE_URLS=http://+:5312

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
COPY --from=build /publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD wget -qO- http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "BookingService.API.dll"]
