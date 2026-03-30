# Multi-stage build for AlertsNotifications.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy necessary project files
COPY ["alertsnotificationsServices/src/AlertsNotifications.API/AlertsNotifications.API.csproj", "alertsnotificationsServices/src/AlertsNotifications.API/"]
COPY ["alertsnotificationsServices/src/AlertsNotifications.Application/AlertsNotifications.Application.csproj", "alertsnotificationsServices/src/AlertsNotifications.Application/"]
COPY ["alertsnotificationsServices/src/AlertsNotifications.Domain/AlertsNotifications.Domain.csproj", "alertsnotificationsServices/src/AlertsNotifications.Domain/"]
COPY ["alertsnotificationsServices/src/AlertsNotifications.Infrastructure/AlertsNotifications.Infrastructure.csproj", "alertsnotificationsServices/src/AlertsNotifications.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "alertsnotificationsServices/src/AlertsNotifications.API/AlertsNotifications.API.csproj"

# Copy remaining source code
COPY . .

# Build the application
RUN dotnet build "alertsnotificationsServices/src/AlertsNotifications.API/AlertsNotifications.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "alertsnotificationsServices/src/AlertsNotifications.API/AlertsNotifications.API.csproj" -c Release -o /app/publish

# Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5154
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5154

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AlertsNotifications.API.dll"]
