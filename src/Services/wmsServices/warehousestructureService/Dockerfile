FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WarehouseStructure.API/WarehouseStructure.API.csproj", "src/WarehouseStructure.API/"]
COPY ["src/WarehouseStructure.Application/WarehouseStructure.Application.csproj", "src/WarehouseStructure.Application/"]
COPY ["src/WarehouseStructure.Domain/WarehouseStructure.Domain.csproj", "src/WarehouseStructure.Domain/"]
COPY ["src/WarehouseStructure.Infrastructure/WarehouseStructure.Infrastructure.csproj", "src/WarehouseStructure.Infrastructure/"]
RUN dotnet restore "src/WarehouseStructure.API/WarehouseStructure.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/WarehouseStructure.API/WarehouseStructure.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WarehouseStructure.API.dll"]
