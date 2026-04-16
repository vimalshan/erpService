FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5335
ENV ASPNETCORE_URLS=http://+:5335

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/RiskService.Domain/RiskService.Domain.csproj",             "src/RiskService.Domain/"]
COPY ["src/RiskService.Application/RiskService.Application.csproj",   "src/RiskService.Application/"]
COPY ["src/RiskService.Infrastructure/RiskService.Infrastructure.csproj", "src/RiskService.Infrastructure/"]
COPY ["src/RiskService.API/RiskService.API.csproj",                   "src/RiskService.API/"]
RUN dotnet restore "src/RiskService.API/RiskService.API.csproj"

COPY . .
RUN dotnet publish "src/RiskService.API/RiskService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RiskService.API.dll"]
