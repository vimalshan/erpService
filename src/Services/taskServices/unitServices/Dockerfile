FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5456
ENV ASPNETCORE_URLS=http://+:5456

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY UnitService/src/UnitService.Domain/UnitService.Domain.csproj UnitService/src/UnitService.Domain/
COPY UnitService/src/UnitService.Application/UnitService.Application.csproj UnitService/src/UnitService.Application/
COPY UnitService/src/UnitService.Infrastructure/UnitService.Infrastructure.csproj UnitService/src/UnitService.Infrastructure/
COPY UnitService/src/UnitService.API/UnitService.API.csproj UnitService/src/UnitService.API/
COPY UnitService/src/UnitService.Functions/UnitService.Functions.csproj UnitService/src/UnitService.Functions/
RUN dotnet restore UnitService/src/UnitService.API/UnitService.API.csproj
COPY . .
WORKDIR /src/UnitService/src/UnitService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "UnitService.API.dll"]
