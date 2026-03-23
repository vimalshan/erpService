FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/BusServices.API/BusServices.API.csproj BusServices.API/
COPY src/BusServices.Application/BusServices.Application.csproj BusServices.Application/
COPY src/BusServices.Domain/BusServices.Domain.csproj BusServices.Domain/
COPY src/BusServices.Infrastructure/BusServices.Infrastructure.csproj BusServices.Infrastructure/
RUN dotnet restore BusServices.API/BusServices.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore BusServices.API/BusServices.API.csproj
WORKDIR /src/BusServices.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "BusServices.API.dll"]
