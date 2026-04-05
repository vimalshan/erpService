FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/CSA.Service.Domain/CSA.Service.Domain.csproj",             "src/CSA.Service.Domain/"]
COPY ["src/CSA.Service.Application/CSA.Service.Application.csproj",   "src/CSA.Service.Application/"]
COPY ["src/CSA.Service.Infrastructure/CSA.Service.Infrastructure.csproj", "src/CSA.Service.Infrastructure/"]
COPY ["src/CSA.Service.API/CSA.Service.API.csproj",                   "src/CSA.Service.API/"]
RUN dotnet restore "src/CSA.Service.API/CSA.Service.API.csproj"

COPY . .
RUN dotnet publish "src/CSA.Service.API/CSA.Service.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CSA.Service.API.dll"]
