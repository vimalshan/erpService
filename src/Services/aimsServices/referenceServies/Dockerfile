FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/API/ReferenceService.API/ReferenceService.API.csproj API/ReferenceService.API/
COPY src/Application/ReferenceService.Application/ReferenceService.Application.csproj Application/ReferenceService.Application/
COPY src/Domain/ReferenceService.Domain/ReferenceService.Domain.csproj Domain/ReferenceService.Domain/
COPY src/Infrastructure/ReferenceService.Infrastructure/ReferenceService.Infrastructure.csproj Infrastructure/ReferenceService.Infrastructure/
RUN dotnet restore API/ReferenceService.API/ReferenceService.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore API/ReferenceService.API/ReferenceService.API.csproj
WORKDIR /src/API/ReferenceService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ReferenceService.API.dll"]
