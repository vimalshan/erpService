FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/EmployeeService.API/EmployeeService.API.csproj EmployeeService.API/
COPY src/EmployeeService.Application/EmployeeService.Application.csproj EmployeeService.Application/
COPY src/EmployeeService.Domain/EmployeeService.Domain.csproj EmployeeService.Domain/
COPY src/EmployeeService.Infrastructure/EmployeeService.Infrastructure.csproj EmployeeService.Infrastructure/
RUN dotnet restore EmployeeService.API/EmployeeService.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore EmployeeService.API/EmployeeService.API.csproj
WORKDIR /src/EmployeeService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "EmployeeService.API.dll"]
