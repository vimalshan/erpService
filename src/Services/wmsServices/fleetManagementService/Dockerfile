FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5514
ENV ASPNETCORE_URLS=http://+:5514
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FleetManagement.API/FleetManagement.API.csproj", "src/FleetManagement.API/"]
COPY ["src/FleetManagement.Application/FleetManagement.Application.csproj", "src/FleetManagement.Application/"]
COPY ["src/FleetManagement.Domain/FleetManagement.Domain.csproj", "src/FleetManagement.Domain/"]
COPY ["src/FleetManagement.Infrastructure/FleetManagement.Infrastructure.csproj", "src/FleetManagement.Infrastructure/"]
RUN dotnet restore "src/FleetManagement.API/FleetManagement.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/FleetManagement.API/FleetManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FleetManagement.API.dll"]
