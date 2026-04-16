# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 5314
ENV ASPNETCORE_URLS=http://+:5314

# Copy project files for restore
COPY ["CompensationService.API/CompensationService.API.csproj", "CompensationService.API/"]
COPY ["CompensationService.Application/CompensationService.Application.csproj", "CompensationService.Application/"]
COPY ["CompensationService.Domain/CompensationService.Domain.csproj", "CompensationService.Domain/"]
COPY ["CompensationService.Infrastructure/CompensationService.Infrastructure.csproj", "CompensationService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "CompensationService.API/CompensationService.API.csproj"

# Copy source code
COPY CompensationService.API/ CompensationService.API/
COPY CompensationService.Application/ CompensationService.Application/
COPY CompensationService.Domain/ CompensationService.Domain/
COPY CompensationService.Infrastructure/ CompensationService.Infrastructure/

# Publish
RUN dotnet publish "CompensationService.API/CompensationService.API.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
COPY --from=build /publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "CompensationService.API.dll"]
