FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5332
ENV ASPNETCORE_URLS=http://+:5332

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/BatchService.Domain/BatchService.Domain.csproj",           "src/BatchService.Domain/"]
COPY ["src/BatchService.Application/BatchService.Application.csproj", "src/BatchService.Application/"]
COPY ["src/BatchService.Infrastructure/BatchService.Infrastructure.csproj", "src/BatchService.Infrastructure/"]
COPY ["src/BatchService.API/BatchService.API.csproj",                 "src/BatchService.API/"]
RUN dotnet restore "src/BatchService.API/BatchService.API.csproj"

COPY . .
RUN dotnet publish "src/BatchService.API/BatchService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BatchService.API.dll"]
