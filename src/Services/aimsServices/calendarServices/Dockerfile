FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/CalendarService.API/CalendarService.API.csproj CalendarService.API/
COPY src/CalendarService.Application/CalendarService.Application.csproj CalendarService.Application/
COPY src/CalendarService.Domain/CalendarService.Domain.csproj CalendarService.Domain/
COPY src/CalendarService.Infrastructure/CalendarService.Infrastructure.csproj CalendarService.Infrastructure/
RUN dotnet restore CalendarService.API/CalendarService.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore CalendarService.API/CalendarService.API.csproj
WORKDIR /src/CalendarService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "CalendarService.API.dll"]
