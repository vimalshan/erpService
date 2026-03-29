FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7101

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CheckupManagementService/CheckupManagementService.csproj", "CheckupManagementService/"]
COPY ["Shared/Shared.Core/Shared.Core.csproj", "Shared/Shared.Core/"]
COPY ["Shared/Shared.Events/Shared.Events.csproj", "Shared/Shared.Events/"]
COPY ["Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared/Shared.Infrastructure/"]
RUN dotnet restore "CheckupManagementService/CheckupManagementService.csproj"
COPY . .
WORKDIR "/src/CheckupManagementService"
RUN dotnet build "CheckupManagementService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CheckupManagementService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:7101
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "CheckupManagementService.dll"]
