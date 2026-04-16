# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file
COPY ["ApiGateway.csproj", "."]

# Restore dependencies
RUN dotnet restore "ApiGateway.csproj"

# Copy source code
COPY . .

# Build the application
RUN dotnet build "ApiGateway.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 5100
ENV ASPNETCORE_URLS=http://+:5100

# Install curl for health checks
# Copy published application
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p logs

# Expose port

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD wget -qO- http://localhost:5000/health || exit 1

# Start application
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
