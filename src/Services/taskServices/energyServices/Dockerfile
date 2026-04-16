FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5452
ENV ASPNETCORE_URLS=http://+:5452

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY EnergyService/src/EnergyService.Domain/EnergyService.Domain.csproj EnergyService/src/EnergyService.Domain/
COPY EnergyService/src/EnergyService.Application/EnergyService.Application.csproj EnergyService/src/EnergyService.Application/
COPY EnergyService/src/EnergyService.Infrastructure/EnergyService.Infrastructure.csproj EnergyService/src/EnergyService.Infrastructure/
COPY EnergyService/src/EnergyService.API/EnergyService.API.csproj EnergyService/src/EnergyService.API/
COPY EnergyService/src/EnergyService.Functions/EnergyService.Functions.csproj EnergyService/src/EnergyService.Functions/
RUN dotnet restore EnergyService/src/EnergyService.API/EnergyService.API.csproj
COPY . .
WORKDIR /src/EnergyService/src/EnergyService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "EnergyService.API.dll"]
