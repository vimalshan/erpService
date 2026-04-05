# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy project files for restore
COPY ["ProxyModule/src/ProxyModule.API/ProxyModule.API.csproj", "ProxyModule/src/ProxyModule.API/"]
COPY ["ProxyModule/src/ProxyModule.Application/ProxyModule.Application.csproj", "ProxyModule/src/ProxyModule.Application/"]
COPY ["ProxyModule/src/ProxyModule.Domain/ProxyModule.Domain.csproj", "ProxyModule/src/ProxyModule.Domain/"]
COPY ["ProxyModule/src/ProxyModule.Infrastructure/ProxyModule.Infrastructure.csproj", "ProxyModule/src/ProxyModule.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "ProxyModule/src/ProxyModule.API/ProxyModule.API.csproj"

# Copy source code
COPY ProxyModule/src/ProxyModule.API/ ProxyModule/src/ProxyModule.API/
COPY ProxyModule/src/ProxyModule.Application/ ProxyModule/src/ProxyModule.Application/
COPY ProxyModule/src/ProxyModule.Domain/ ProxyModule/src/ProxyModule.Domain/
COPY ProxyModule/src/ProxyModule.Infrastructure/ ProxyModule/src/ProxyModule.Infrastructure/

# Publish
RUN dotnet publish "ProxyModule/src/ProxyModule.API/ProxyModule.API.csproj" -c Release -o /publish

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

ENTRYPOINT ["dotnet", "ProxyModule.API.dll"]
