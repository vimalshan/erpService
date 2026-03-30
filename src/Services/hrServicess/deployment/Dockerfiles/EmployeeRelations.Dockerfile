# Multi-stage build for EmployeeRelations.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["employeerelationsServices/EmployeeRelations.API/EmployeeRelations.API.csproj", "employeerelationsServices/EmployeeRelations.API/"]
COPY ["employeerelationsServices/EmployeeRelations.Application/EmployeeRelations.Application.csproj", "employeerelationsServices/EmployeeRelations.Application/"]
COPY ["employeerelationsServices/EmployeeRelations.Domain/EmployeeRelations.Domain.csproj", "employeerelationsServices/EmployeeRelations.Domain/"]
COPY ["employeerelationsServices/EmployeeRelations.Infrastructure/EmployeeRelations.Infrastructure.csproj", "employeerelationsServices/EmployeeRelations.Infrastructure/"]

RUN dotnet restore "employeerelationsServices/EmployeeRelations.API/EmployeeRelations.API.csproj"

COPY . .

RUN dotnet build "employeerelationsServices/EmployeeRelations.API/EmployeeRelations.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "employeerelationsServices/EmployeeRelations.API/EmployeeRelations.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5075
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5075

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "EmployeeRelations.API.dll"]
