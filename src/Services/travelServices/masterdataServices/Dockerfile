FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/MasterDataService.API/MasterDataService.API.csproj", "src/MasterDataService.API/"]
COPY ["src/MasterDataService.Application/MasterDataService.Application.csproj", "src/MasterDataService.Application/"]
COPY ["src/MasterDataService.Domain/MasterDataService.Domain.csproj", "src/MasterDataService.Domain/"]
COPY ["src/MasterDataService.Infrastructure/MasterDataService.Infrastructure.csproj", "src/MasterDataService.Infrastructure/"]
COPY ["src/MasterDataService.Functions/MasterDataService.Functions.csproj", "src/MasterDataService.Functions/"]

RUN dotnet restore "src/MasterDataService.API/MasterDataService.API.csproj"

COPY . .

RUN dotnet publish "src/MasterDataService.API/MasterDataService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5497
ENV ASPNETCORE_URLS=http://+:5497

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MasterDataService.API.dll"]
