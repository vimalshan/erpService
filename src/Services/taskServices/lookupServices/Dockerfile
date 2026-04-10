FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/LookupService.Domain/LookupService.Domain.csproj src/LookupService.Domain/
COPY src/LookupService.Application/LookupService.Application.csproj src/LookupService.Application/
COPY src/LookupService.Infrastructure/LookupService.Infrastructure.csproj src/LookupService.Infrastructure/
COPY src/LookupService.API/LookupService.API.csproj src/LookupService.API/
COPY src/LookupService.Functions/LookupService.Functions.csproj src/LookupService.Functions/
RUN dotnet restore src/LookupService.API/LookupService.API.csproj
COPY . .
WORKDIR /src/src/LookupService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "LookupService.API.dll"]
