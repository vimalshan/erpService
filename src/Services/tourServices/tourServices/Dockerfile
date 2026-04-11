FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TourServices.API/TourServices.API.csproj", "src/TourServices.API/"]
COPY ["src/TourServices.Application/TourServices.Application.csproj", "src/TourServices.Application/"]
COPY ["src/TourServices.Domain/TourServices.Domain.csproj", "src/TourServices.Domain/"]
COPY ["src/TourServices.Infrastructure/TourServices.Infrastructure.csproj", "src/TourServices.Infrastructure/"]
RUN dotnet restore "src/TourServices.API/TourServices.API.csproj"
COPY . .
WORKDIR "/src/src/TourServices.API"
RUN dotnet build "TourServices.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TourServices.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "TourServices.API.dll"]
