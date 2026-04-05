FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/ProjectService.Domain/ProjectService.Domain.csproj",           "src/ProjectService.Domain/"]
COPY ["src/ProjectService.Application/ProjectService.Application.csproj", "src/ProjectService.Application/"]
COPY ["src/ProjectService.Infrastructure/ProjectService.Infrastructure.csproj", "src/ProjectService.Infrastructure/"]
COPY ["src/ProjectService.API/ProjectService.API.csproj",                 "src/ProjectService.API/"]
RUN dotnet restore "src/ProjectService.API/ProjectService.API.csproj"

COPY . .
RUN dotnet publish "src/ProjectService.API/ProjectService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjectService.API.dll"]
