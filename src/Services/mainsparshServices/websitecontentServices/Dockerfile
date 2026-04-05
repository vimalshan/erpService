# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy project files for restore
COPY ["WebsiteContentService.API/WebsiteContentService.API.csproj", "WebsiteContentService.API/"]
COPY ["WebsiteContentService.Application/WebsiteContentService.Application.csproj", "WebsiteContentService.Application/"]
COPY ["WebsiteContentService.Domain/WebsiteContentService.Domain.csproj", "WebsiteContentService.Domain/"]
COPY ["WebsiteContentService.Infrastructure/WebsiteContentService.Infrastructure.csproj", "WebsiteContentService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "WebsiteContentService.API/WebsiteContentService.API.csproj"

# Copy source code
COPY WebsiteContentService.API/ WebsiteContentService.API/
COPY WebsiteContentService.Application/ WebsiteContentService.Application/
COPY WebsiteContentService.Domain/ WebsiteContentService.Domain/
COPY WebsiteContentService.Infrastructure/ WebsiteContentService.Infrastructure/

# Publish
RUN dotnet publish "WebsiteContentService.API/WebsiteContentService.API.csproj" -c Release -o /publish

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

ENTRYPOINT ["dotnet", "WebsiteContentService.API.dll"]
