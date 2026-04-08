FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/AccountingService.API/AccountingService.API.csproj", "src/AccountingService.API/"]
COPY ["src/AccountingService.Application/AccountingService.Application.csproj", "src/AccountingService.Application/"]
COPY ["src/AccountingService.Domain/AccountingService.Domain.csproj", "src/AccountingService.Domain/"]
COPY ["src/AccountingService.Infrastructure/AccountingService.Infrastructure.csproj", "src/AccountingService.Infrastructure/"]
RUN dotnet restore "src/AccountingService.API/AccountingService.API.csproj"

COPY . .
RUN dotnet publish "src/AccountingService.API/AccountingService.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AccountingService.API.dll"]
