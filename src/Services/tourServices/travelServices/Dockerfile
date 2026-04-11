FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/TravelService.Domain/TravelService.Domain.csproj src/TravelService.Domain/
COPY src/TravelService.Application/TravelService.Application.csproj src/TravelService.Application/
COPY src/TravelService.Infrastructure/TravelService.Infrastructure.csproj src/TravelService.Infrastructure/
COPY src/TravelService.API/TravelService.API.csproj src/TravelService.API/
RUN dotnet restore src/TravelService.API/TravelService.API.csproj
COPY . .
WORKDIR /src/src/TravelService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "TravelService.API.dll"]
