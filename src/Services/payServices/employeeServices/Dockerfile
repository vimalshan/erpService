FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["EmployeeService.API/EmployeeService.API.csproj", "EmployeeService.API/"]
COPY ["EmployeeService.Application/EmployeeService.Application.csproj", "EmployeeService.Application/"]
COPY ["EmployeeService.Domain/EmployeeService.Domain.csproj", "EmployeeService.Domain/"]
COPY ["EmployeeService.Infrastructure/EmployeeService.Infrastructure.csproj", "EmployeeService.Infrastructure/"]
COPY ["EmployeeService.Shared/EmployeeService.Shared.csproj", "EmployeeService.Shared/"]
RUN dotnet restore "EmployeeService.API/EmployeeService.API.csproj"

COPY . .
WORKDIR "/src/EmployeeService.API"
RUN dotnet build "EmployeeService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeeService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/Logs

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80
ENTRYPOINT ["dotnet", "EmployeeService.API.dll"]
