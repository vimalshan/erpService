FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/VisitorServices.API/VisitorServices.API.csproj VisitorServices.API/
COPY src/VisitorServices.Application/VisitorServices.Application.csproj VisitorServices.Application/
COPY src/VisitorServices.Domain/VisitorServices.Domain.csproj VisitorServices.Domain/
COPY src/VisitorServices.Infrastructure/VisitorServices.Infrastructure.csproj VisitorServices.Infrastructure/
RUN dotnet restore VisitorServices.API/VisitorServices.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore VisitorServices.API/VisitorServices.API.csproj
WORKDIR /src/VisitorServices.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "VisitorServices.API.dll"]
