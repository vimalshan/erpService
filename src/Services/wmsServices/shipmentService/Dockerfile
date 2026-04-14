FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ShipmentService.API/ShipmentService.API.csproj", "src/ShipmentService.API/"]
COPY ["src/ShipmentService.Application/ShipmentService.Application.csproj", "src/ShipmentService.Application/"]
COPY ["src/ShipmentService.Domain/ShipmentService.Domain.csproj", "src/ShipmentService.Domain/"]
COPY ["src/ShipmentService.Infrastructure/ShipmentService.Infrastructure.csproj", "src/ShipmentService.Infrastructure/"]
RUN dotnet restore "src/ShipmentService.API/ShipmentService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/ShipmentService.API/ShipmentService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ShipmentService.API.dll"]
