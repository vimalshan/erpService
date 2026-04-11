FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TravelService.API/TravelService.API.csproj", "src/TravelService.API/"]
COPY ["src/TravelService.Application/TravelService.Application.csproj", "src/TravelService.Application/"]
COPY ["src/TravelService.Domain/TravelService.Domain.csproj", "src/TravelService.Domain/"]
COPY ["src/TravelService.Infrastructure/TravelService.Infrastructure.csproj", "src/TravelService.Infrastructure/"]
RUN dotnet restore "src/TravelService.API/TravelService.API.csproj"
COPY . .
WORKDIR "/src/src/TravelService.API"
RUN dotnet build "TravelService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TravelService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "TravelService.API.dll"]
