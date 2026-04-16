FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5396
ENV ASPNETCORE_URLS=http://+:5396

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/InventoryManagement.API/InventoryManagement.API.csproj", "src/InventoryManagement.API/"]
COPY ["src/InventoryManagement.Application/InventoryManagement.Application.csproj", "src/InventoryManagement.Application/"]
COPY ["src/InventoryManagement.Domain/InventoryManagement.Domain.csproj", "src/InventoryManagement.Domain/"]
COPY ["src/InventoryManagement.Infrastructure/InventoryManagement.Infrastructure.csproj", "src/InventoryManagement.Infrastructure/"]
COPY ["src/InventoryManagement.Functions/InventoryManagement.Functions.csproj", "src/InventoryManagement.Functions/"]
RUN dotnet restore "src/InventoryManagement.API/InventoryManagement.API.csproj"
COPY . .
WORKDIR "/src/src/InventoryManagement.API"
RUN dotnet build "InventoryManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InventoryManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "InventoryManagement.API.dll"]
