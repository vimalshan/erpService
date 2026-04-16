FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5474
ENV ASPNETCORE_URLS=http://+:5474

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/TourServices.Domain/TourServices.Domain.csproj src/TourServices.Domain/
COPY src/TourServices.Application/TourServices.Application.csproj src/TourServices.Application/
COPY src/TourServices.Infrastructure/TourServices.Infrastructure.csproj src/TourServices.Infrastructure/
COPY src/TourServices.API/TourServices.API.csproj src/TourServices.API/
RUN dotnet restore src/TourServices.API/TourServices.API.csproj
COPY . .
WORKDIR /src/src/TourServices.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "TourServices.API.dll"]
