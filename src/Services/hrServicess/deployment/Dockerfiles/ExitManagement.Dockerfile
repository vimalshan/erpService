# Multi-stage build for ExitManagement.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["exitmanagementServices/src/ExitManagement.API/ExitManagement.API.csproj", "exitmanagementServices/src/ExitManagement.API/"]
COPY ["exitmanagementServices/src/ExitManagement.Application/ExitManagement.Application.csproj", "exitmanagementServices/src/ExitManagement.Application/"]
COPY ["exitmanagementServices/src/ExitManagement.Domain/ExitManagement.Domain.csproj", "exitmanagementServices/src/ExitManagement.Domain/"]
COPY ["exitmanagementServices/src/ExitManagement.Infrastructure/ExitManagement.Infrastructure.csproj", "exitmanagementServices/src/ExitManagement.Infrastructure/"]

RUN dotnet restore "exitmanagementServices/src/ExitManagement.API/ExitManagement.API.csproj"

COPY . .

RUN dotnet build "exitmanagementServices/src/ExitManagement.API/ExitManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "exitmanagementServices/src/ExitManagement.API/ExitManagement.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5094
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5094

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ExitManagement.API.dll"]
