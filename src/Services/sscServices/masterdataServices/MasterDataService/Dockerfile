FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY src/MasterDataService.API/MasterDataService.API.csproj src/MasterDataService.API/
COPY src/MasterDataService.Application/MasterDataService.Application.csproj src/MasterDataService.Application/
COPY src/MasterDataService.Domain/MasterDataService.Domain.csproj src/MasterDataService.Domain/
COPY src/MasterDataService.Infrastructure/MasterDataService.Infrastructure.csproj src/MasterDataService.Infrastructure/
RUN dotnet restore src/MasterDataService.API/MasterDataService.API.csproj

COPY . .
RUN dotnet publish src/MasterDataService.API/MasterDataService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MasterDataService.API.dll"]
