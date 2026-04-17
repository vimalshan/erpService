# Multi-stage build for EmployeeManagement.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["employeemanagementServices/src/EmployeeManagement.API/EmployeeManagement.API.csproj", "employeemanagementServices/src/EmployeeManagement.API/"]
COPY ["employeemanagementServices/src/EmployeeManagement.Application/EmployeeManagement.Application.csproj", "employeemanagementServices/src/EmployeeManagement.Application/"]
COPY ["employeemanagementServices/src/EmployeeManagement.Domain/EmployeeManagement.Domain.csproj", "employeemanagementServices/src/EmployeeManagement.Domain/"]
COPY ["employeemanagementServices/src/EmployeeManagement.Infrastructure/EmployeeManagement.Infrastructure.csproj", "employeemanagementServices/src/EmployeeManagement.Infrastructure/"]

RUN dotnet restore "employeemanagementServices/src/EmployeeManagement.API/EmployeeManagement.API.csproj"

COPY . .

RUN dotnet build "employeemanagementServices/src/EmployeeManagement.API/EmployeeManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "employeemanagementServices/src/EmployeeManagement.API/EmployeeManagement.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5004
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5004

RUN apt-get update --fix-missing && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/* || \
    (apt-get update --fix-missing && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*)

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "EmployeeManagement.API.dll"]
