FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/TeamServices.Domain/TeamServices.Domain.csproj",               "src/TeamServices.Domain/"]
COPY ["src/TeamServices.Application/TeamServices.Application.csproj",     "src/TeamServices.Application/"]
COPY ["src/TeamServices.Infrastructure/TeamServices.Infrastructure.csproj", "src/TeamServices.Infrastructure/"]
COPY ["src/TeamServices.API/TeamServices.API.csproj",                     "src/TeamServices.API/"]
RUN dotnet restore "src/TeamServices.API/TeamServices.API.csproj"

COPY . .
RUN dotnet publish "src/TeamServices.API/TeamServices.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TeamServices.API.dll"]
