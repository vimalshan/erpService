FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5513
ENV ASPNETCORE_URLS=http://+:5513
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/EmployeeService.API/EmployeeService.API.csproj", "src/EmployeeService.API/"]
COPY ["src/EmployeeService.Application/EmployeeService.Application.csproj", "src/EmployeeService.Application/"]
COPY ["src/EmployeeService.Domain/EmployeeService.Domain.csproj", "src/EmployeeService.Domain/"]
COPY ["src/EmployeeService.Infrastructure/EmployeeService.Infrastructure.csproj", "src/EmployeeService.Infrastructure/"]
RUN dotnet restore "src/EmployeeService.API/EmployeeService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/EmployeeService.API/EmployeeService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeeService.API.dll"]
