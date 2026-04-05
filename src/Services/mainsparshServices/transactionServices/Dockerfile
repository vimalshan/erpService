# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy project files for restore
COPY ["src/TransactionService.API/TransactionService.API.csproj", "src/TransactionService.API/"]
COPY ["src/TransactionService.Application/TransactionService.Application.csproj", "src/TransactionService.Application/"]
COPY ["src/TransactionService.Domain/TransactionService.Domain.csproj", "src/TransactionService.Domain/"]
COPY ["src/TransactionService.Infrastructure/TransactionService.Infrastructure.csproj", "src/TransactionService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/TransactionService.API/TransactionService.API.csproj"

# Copy source code
COPY src/TransactionService.API/ src/TransactionService.API/
COPY src/TransactionService.Application/ src/TransactionService.Application/
COPY src/TransactionService.Domain/ src/TransactionService.Domain/
COPY src/TransactionService.Infrastructure/ src/TransactionService.Infrastructure/

# Publish
RUN dotnet publish "src/TransactionService.API/TransactionService.API.csproj" -c Release -o /publish

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

ENTRYPOINT ["dotnet", "TransactionService.API.dll"]
