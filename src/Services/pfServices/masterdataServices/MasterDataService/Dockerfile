FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5376
ENV ASPNETCORE_URLS=http://+:5376
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/MasterDataService.API/MasterDataService.API.csproj", "src/MasterDataService.API/"]
COPY ["src/MasterDataService.Application/MasterDataService.Application.csproj", "src/MasterDataService.Application/"]
COPY ["src/MasterDataService.Domain/MasterDataService.Domain.csproj", "src/MasterDataService.Domain/"]
COPY ["src/MasterDataService.Infrastructure/MasterDataService.Infrastructure.csproj", "src/MasterDataService.Infrastructure/"]
RUN dotnet restore "src/MasterDataService.API/MasterDataService.API.csproj"

COPY . .
RUN dotnet publish "src/MasterDataService.API/MasterDataService.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MasterDataService.API.dll"]
