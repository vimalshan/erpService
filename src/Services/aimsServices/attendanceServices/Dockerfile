FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/AttendanceService.API/AttendanceService.API.csproj AttendanceService.API/
COPY src/AttendanceService.Application/AttendanceService.Application.csproj AttendanceService.Application/
COPY src/AttendanceService.Domain/AttendanceService.Domain.csproj AttendanceService.Domain/
COPY src/AttendanceService.Infrastructure/AttendanceService.Infrastructure.csproj AttendanceService.Infrastructure/
RUN dotnet restore AttendanceService.API/AttendanceService.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore AttendanceService.API/AttendanceService.API.csproj
WORKDIR /src/AttendanceService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "AttendanceService.API.dll"]
