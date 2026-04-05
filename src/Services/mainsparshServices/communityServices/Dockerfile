# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files for restore
COPY ["CommunityService.API/CommunityService.API.csproj", "CommunityService.API/"]
COPY ["CommunityService.Application/CommunityService.Application.csproj", "CommunityService.Application/"]
COPY ["CommunityService.Domain/CommunityService.Domain.csproj", "CommunityService.Domain/"]
COPY ["CommunityService.Infrastructure/CommunityService.Infrastructure.csproj", "CommunityService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "CommunityService.API/CommunityService.API.csproj"

# Copy source code
COPY CommunityService.API/ CommunityService.API/
COPY CommunityService.Application/ CommunityService.Application/
COPY CommunityService.Domain/ CommunityService.Domain/
COPY CommunityService.Infrastructure/ CommunityService.Infrastructure/

# Publish
RUN dotnet publish "CommunityService.API/CommunityService.API.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
COPY --from=build /publish .

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 80

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "CommunityService.API.dll"]
