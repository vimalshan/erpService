FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5473
ENV ASPNETCORE_URLS=http://+:5473

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/ConfigService.Domain/ConfigService.Domain.csproj src/ConfigService.Domain/
COPY src/ConfigService.Application/ConfigService.Application.csproj src/ConfigService.Application/
COPY src/ConfigService.Infrastructure/ConfigService.Infrastructure.csproj src/ConfigService.Infrastructure/
COPY src/ConfigService.API/ConfigService.API.csproj src/ConfigService.API/
RUN dotnet restore src/ConfigService.API/ConfigService.API.csproj
COPY . .
WORKDIR /src/src/ConfigService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ConfigService.API.dll"]
