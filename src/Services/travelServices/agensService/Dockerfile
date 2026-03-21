FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["AgencyService.sln", "."]
COPY ["src/Domain/AgencyService.Domain/AgencyService.Domain.csproj", "src/Domain/AgencyService.Domain/"]
COPY ["src/Application/AgencyService.Application/AgencyService.Application.csproj", "src/Application/AgencyService.Application/"]
COPY ["src/Infrastructure/AgencyService.Infrastructure/AgencyService.Infrastructure.csproj", "src/Infrastructure/AgencyService.Infrastructure/"]
COPY ["src/API/AgencyService.Api/AgencyService.Api.csproj", "src/API/AgencyService.Api/"]

RUN dotnet restore "AgencyService.sln"

# Copy source code
COPY . .

# Build
RUN dotnet build "AgencyService.sln" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "src/API/AgencyService.Api/AgencyService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "AgencyService.Api.dll"]
