FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5437
ENV ASPNETCORE_URLS=http://+:5437

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY src/IntegrationService.API/IntegrationService.API.csproj src/IntegrationService.API/
COPY src/IntegrationService.Application/IntegrationService.Application.csproj src/IntegrationService.Application/
COPY src/IntegrationService.Domain/IntegrationService.Domain.csproj src/IntegrationService.Domain/
COPY src/IntegrationService.Infrastructure/IntegrationService.Infrastructure.csproj src/IntegrationService.Infrastructure/
RUN dotnet restore src/IntegrationService.API/IntegrationService.API.csproj

COPY . .
RUN dotnet publish src/IntegrationService.API/IntegrationService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IntegrationService.API.dll"]
