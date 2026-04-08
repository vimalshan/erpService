FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/PFTransactionalService.API/PFTransactionalService.API.csproj", "src/PFTransactionalService.API/"]
COPY ["src/PFTransactionalService.Application/PFTransactionalService.Application.csproj", "src/PFTransactionalService.Application/"]
COPY ["src/PFTransactionalService.Domain/PFTransactionalService.Domain.csproj", "src/PFTransactionalService.Domain/"]
COPY ["src/PFTransactionalService.Infrastructure/PFTransactionalService.Infrastructure.csproj", "src/PFTransactionalService.Infrastructure/"]
RUN dotnet restore "src/PFTransactionalService.API/PFTransactionalService.API.csproj"

COPY . .
RUN dotnet publish "src/PFTransactionalService.API/PFTransactionalService.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PFTransactionalService.API.dll"]
