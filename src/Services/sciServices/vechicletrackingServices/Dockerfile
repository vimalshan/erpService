FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5405
ENV ASPNETCORE_URLS=http://+:5405

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/VehicleTracking.API/VehicleTracking.API.csproj", "src/VehicleTracking.API/"]
COPY ["src/VehicleTracking.Application/VehicleTracking.Application.csproj", "src/VehicleTracking.Application/"]
COPY ["src/VehicleTracking.Domain/VehicleTracking.Domain.csproj", "src/VehicleTracking.Domain/"]
COPY ["src/VehicleTracking.Infrastructure/VehicleTracking.Infrastructure.csproj", "src/VehicleTracking.Infrastructure/"]
COPY ["src/VehicleTracking.Functions/VehicleTracking.Functions.csproj", "src/VehicleTracking.Functions/"]
RUN dotnet restore "src/VehicleTracking.API/VehicleTracking.API.csproj"
COPY . .
WORKDIR "/src/src/VehicleTracking.API"
RUN dotnet build "VehicleTracking.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VehicleTracking.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "VehicleTracking.API.dll"]
