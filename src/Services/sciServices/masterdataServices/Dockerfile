FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["MasterDataService/src/MasterData.API/MasterData.API.csproj", "MasterDataService/src/MasterData.API/"]
COPY ["MasterDataService/src/MasterData.Application/MasterData.Application.csproj", "MasterDataService/src/MasterData.Application/"]
COPY ["MasterDataService/src/MasterData.Domain/MasterData.Domain.csproj", "MasterDataService/src/MasterData.Domain/"]
COPY ["MasterDataService/src/MasterData.Infrastructure/MasterData.Infrastructure.csproj", "MasterDataService/src/MasterData.Infrastructure/"]
COPY ["MasterDataService/src/MasterData.Functions/MasterData.Functions.csproj", "MasterDataService/src/MasterData.Functions/"]
RUN dotnet restore "MasterDataService/src/MasterData.API/MasterData.API.csproj"
COPY . .
WORKDIR "/src/MasterDataService/src/MasterData.API"
RUN dotnet build "MasterData.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MasterData.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "MasterData.API.dll"]
