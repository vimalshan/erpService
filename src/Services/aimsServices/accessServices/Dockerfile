FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5121
ENV ASPNETCORE_URLS=http://+:5121

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/AccessService.API/AccessService.API.csproj AccessService.API/
COPY src/AccessService.Application/AccessService.Application.csproj AccessService.Application/
COPY src/AccessService.Domain/AccessService.Domain.csproj AccessService.Domain/
COPY src/AccessService.Infrastructure/AccessService.Infrastructure.csproj AccessService.Infrastructure/
RUN dotnet restore AccessService.API/AccessService.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore AccessService.API/AccessService.API.csproj
WORKDIR /src/AccessService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "AccessService.API.dll"]
