FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/TrustService.API/TrustService.API.csproj", "src/TrustService.API/"]
COPY ["src/TrustService.Application/TrustService.Application.csproj", "src/TrustService.Application/"]
COPY ["src/TrustService.Domain/TrustService.Domain.csproj", "src/TrustService.Domain/"]
COPY ["src/TrustService.Infrastructure/TrustService.Infrastructure.csproj", "src/TrustService.Infrastructure/"]
RUN dotnet restore "src/TrustService.API/TrustService.API.csproj"

COPY . .
RUN dotnet publish "src/TrustService.API/TrustService.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrustService.API.dll"]
