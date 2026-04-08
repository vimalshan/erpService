FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/ProductionManagement.API/ProductionManagement.API.csproj", "src/ProductionManagement.API/"]
COPY ["src/ProductionManagement.Application/ProductionManagement.Application.csproj", "src/ProductionManagement.Application/"]
COPY ["src/ProductionManagement.Domain/ProductionManagement.Domain.csproj", "src/ProductionManagement.Domain/"]
COPY ["src/ProductionManagement.Infrastructure/ProductionManagement.Infrastructure.csproj", "src/ProductionManagement.Infrastructure/"]
COPY ["src/ProductionManagement.Functions/ProductionManagement.Functions.csproj", "src/ProductionManagement.Functions/"]
RUN dotnet restore "src/ProductionManagement.API/ProductionManagement.API.csproj"
COPY . .
WORKDIR "/src/src/ProductionManagement.API"
RUN dotnet build "ProductionManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductionManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ProductionManagement.API.dll"]
