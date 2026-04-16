FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5395
ENV ASPNETCORE_URLS=http://+:5395

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["GSTComplianceService/src/GSTComplianceService.API/GSTComplianceService.API.csproj", "GSTComplianceService/src/GSTComplianceService.API/"]
COPY ["GSTComplianceService/src/GSTComplianceService.Application/GSTComplianceService.Application.csproj", "GSTComplianceService/src/GSTComplianceService.Application/"]
COPY ["GSTComplianceService/src/GSTComplianceService.Domain/GSTComplianceService.Domain.csproj", "GSTComplianceService/src/GSTComplianceService.Domain/"]
COPY ["GSTComplianceService/src/GSTComplianceService.Infrastructure/GSTComplianceService.Infrastructure.csproj", "GSTComplianceService/src/GSTComplianceService.Infrastructure/"]
COPY ["GSTComplianceService/src/GSTComplianceService.Functions/GSTComplianceService.Functions.csproj", "GSTComplianceService/src/GSTComplianceService.Functions/"]
RUN dotnet restore "GSTComplianceService/src/GSTComplianceService.API/GSTComplianceService.API.csproj"
COPY . .
WORKDIR "/src/GSTComplianceService/src/GSTComplianceService.API"
RUN dotnet build "GSTComplianceService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GSTComplianceService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "GSTComplianceService.API.dll"]
