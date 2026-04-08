FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/SettlementService.API/SettlementService.API.csproj", "src/SettlementService.API/"]
COPY ["src/SettlementService.Application/SettlementService.Application.csproj", "src/SettlementService.Application/"]
COPY ["src/SettlementService.Domain/SettlementService.Domain.csproj", "src/SettlementService.Domain/"]
COPY ["src/SettlementService.Infrastructure/SettlementService.Infrastructure.csproj", "src/SettlementService.Infrastructure/"]
RUN dotnet restore "src/SettlementService.API/SettlementService.API.csproj"

COPY . .
RUN dotnet publish "src/SettlementService.API/SettlementService.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SettlementService.API.dll"]
