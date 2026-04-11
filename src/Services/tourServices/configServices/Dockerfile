FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ConfigService.API/ConfigService.API.csproj", "src/ConfigService.API/"]
COPY ["src/ConfigService.Application/ConfigService.Application.csproj", "src/ConfigService.Application/"]
COPY ["src/ConfigService.Domain/ConfigService.Domain.csproj", "src/ConfigService.Domain/"]
COPY ["src/ConfigService.Infrastructure/ConfigService.Infrastructure.csproj", "src/ConfigService.Infrastructure/"]
RUN dotnet restore "src/ConfigService.API/ConfigService.API.csproj"
COPY . .
WORKDIR "/src/src/ConfigService.API"
RUN dotnet build "ConfigService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "ConfigService.API.dll"]
