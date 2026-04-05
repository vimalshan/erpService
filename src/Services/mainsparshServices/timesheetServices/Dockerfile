# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy project files for restore
COPY ["src/TimesheetService.API/TimesheetService.API.csproj", "src/TimesheetService.API/"]
COPY ["src/TimesheetService.Application/TimesheetService.Application.csproj", "src/TimesheetService.Application/"]
COPY ["src/TimesheetService.Domain/TimesheetService.Domain.csproj", "src/TimesheetService.Domain/"]
COPY ["src/TimesheetService.Infrastructure/TimesheetService.Infrastructure.csproj", "src/TimesheetService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/TimesheetService.API/TimesheetService.API.csproj"

# Copy source code
COPY src/TimesheetService.API/ src/TimesheetService.API/
COPY src/TimesheetService.Application/ src/TimesheetService.Application/
COPY src/TimesheetService.Domain/ src/TimesheetService.Domain/
COPY src/TimesheetService.Infrastructure/ src/TimesheetService.Infrastructure/

# Publish
RUN dotnet publish "src/TimesheetService.API/TimesheetService.API.csproj" -c Release -o /publish

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

ENTRYPOINT ["dotnet", "TimesheetService.API.dll"]
