# Multi-stage build for OrganizationStructure.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["organizationstructureServices/src/OrganizationStructureService.API/OrganizationStructureService.API.csproj", "organizationstructureServices/src/OrganizationStructureService.API/"]
COPY ["organizationstructureServices/src/OrganizationStructureService.Application/OrganizationStructureService.Application.csproj", "organizationstructureServices/src/OrganizationStructureService.Application/"]
COPY ["organizationstructureServices/src/OrganizationStructureService.Domain/OrganizationStructureService.Domain.csproj", "organizationstructureServices/src/OrganizationStructureService.Domain/"]
COPY ["organizationstructureServices/src/OrganizationStructureService.Infrastructure/OrganizationStructureService.Infrastructure.csproj", "organizationstructureServices/src/OrganizationStructureService.Infrastructure/"]

RUN dotnet restore "organizationstructureServices/src/OrganizationStructureService.API/OrganizationStructureService.API.csproj"

COPY . .

RUN dotnet build "organizationstructureServices/src/OrganizationStructureService.API/OrganizationStructureService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "organizationstructureServices/src/OrganizationStructureService.API/OrganizationStructureService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5027
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5027

RUN apt-get update --fix-missing && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/* || \
    (apt-get update --fix-missing && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*)

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OrganizationStructureService.API.dll"]
