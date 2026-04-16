# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
EXPOSE 5310
ENV ASPNETCORE_URLS=http://+:5310

# Copy project files for restore
COPY ["ApiGateway/ApiGateway.csproj", "ApiGateway/"]

# Restore dependencies
RUN dotnet restore "ApiGateway/ApiGateway.csproj"

# Copy source code
COPY ApiGateway/ ApiGateway/

# Publish
RUN dotnet publish "ApiGateway/ApiGateway.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD wget -qO- http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
