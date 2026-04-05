# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy project files for restore
COPY ["src/ReimbursementService.API/ReimbursementService.API.csproj", "src/ReimbursementService.API/"]
COPY ["src/ReimbursementService.Application/ReimbursementService.Application.csproj", "src/ReimbursementService.Application/"]
COPY ["src/ReimbursementService.Domain/ReimbursementService.Domain.csproj", "src/ReimbursementService.Domain/"]
COPY ["src/ReimbursementService.Infrastructure/ReimbursementService.Infrastructure.csproj", "src/ReimbursementService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/ReimbursementService.API/ReimbursementService.API.csproj"

# Copy source code
COPY src/ReimbursementService.API/ src/ReimbursementService.API/
COPY src/ReimbursementService.Application/ src/ReimbursementService.Application/
COPY src/ReimbursementService.Domain/ src/ReimbursementService.Domain/
COPY src/ReimbursementService.Infrastructure/ src/ReimbursementService.Infrastructure/

# Publish
RUN dotnet publish "src/ReimbursementService.API/ReimbursementService.API.csproj" -c Release -o /publish

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

ENTRYPOINT ["dotnet", "ReimbursementService.API.dll"]
