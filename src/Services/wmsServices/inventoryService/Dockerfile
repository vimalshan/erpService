FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5515
ENV ASPNETCORE_URLS=http://+:5515
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/InventoryService.API/InventoryService.API.csproj", "src/InventoryService.API/"]
COPY ["src/InventoryService.Application/InventoryService.Application.csproj", "src/InventoryService.Application/"]
COPY ["src/InventoryService.Domain/InventoryService.Domain.csproj", "src/InventoryService.Domain/"]
COPY ["src/InventoryService.Infrastructure/InventoryService.Infrastructure.csproj", "src/InventoryService.Infrastructure/"]
RUN dotnet restore "src/InventoryService.API/InventoryService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/InventoryService.API/InventoryService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InventoryService.API.dll"]
