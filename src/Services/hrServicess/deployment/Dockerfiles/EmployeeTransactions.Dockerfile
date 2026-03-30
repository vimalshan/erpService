# Multi-stage build for EmployeeTransactions.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["employeeTransactionsServices/src/EmployeeTransactionsService.API/EmployeeTransactionsService.API.csproj", "employeeTransactionsServices/src/EmployeeTransactionsService.API/"]
COPY ["employeeTransactionsServices/src/EmployeeTransactionsService.Application/EmployeeTransactionsService.Application.csproj", "employeeTransactionsServices/src/EmployeeTransactionsService.Application/"]
COPY ["employeeTransactionsServices/src/EmployeeTransactionsService.Domain/EmployeeTransactionsService.Domain.csproj", "employeeTransactionsServices/src/EmployeeTransactionsService.Domain/"]
COPY ["employeeTransactionsServices/src/EmployeeTransactionsService.Infrastructure/EmployeeTransactionsService.Infrastructure.csproj", "employeeTransactionsServices/src/EmployeeTransactionsService.Infrastructure/"]

RUN dotnet restore "employeeTransactionsServices/src/EmployeeTransactionsService.API/EmployeeTransactionsService.API.csproj"

COPY . .

RUN dotnet build "employeeTransactionsServices/src/EmployeeTransactionsService.API/EmployeeTransactionsService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "employeeTransactionsServices/src/EmployeeTransactionsService.API/EmployeeTransactionsService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5204
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5204

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "EmployeeTransactionsService.API.dll"]
