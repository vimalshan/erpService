FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["HRService.API/HRService.API.csproj", "HRService.API/"]
COPY ["HRService.Domain/HRService.Domain.csproj", "HRService.Domain/"]
COPY ["HRService.Application/HRService.Application.csproj", "HRService.Application/"]
COPY ["HRService.Infrastructure/HRService.Infrastructure.csproj", "HRService.Infrastructure/"]
COPY ["HRService.Common/HRService.Common.csproj", "HRService.Common/"]

# Restore
RUN dotnet restore "HRService.API/HRService.API.csproj"

# Copy source code
COPY . .

# Build
WORKDIR "/src/HRService.API"
RUN dotnet build "HRService.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "HRService.API.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

# Environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=https://+:443;http://+:80

EXPOSE 80 443

ENTRYPOINT ["dotnet", "HRService.API.dll"]
