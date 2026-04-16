FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5511
ENV ASPNETCORE_URLS=http://+:5511
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AuditLogService.API/AuditLogService.API.csproj", "src/AuditLogService.API/"]
COPY ["src/AuditLogService.Application/AuditLogService.Application.csproj", "src/AuditLogService.Application/"]
COPY ["src/AuditLogService.Domain/AuditLogService.Domain.csproj", "src/AuditLogService.Domain/"]
COPY ["src/AuditLogService.Infrastructure/AuditLogService.Infrastructure.csproj", "src/AuditLogService.Infrastructure/"]
RUN dotnet restore "src/AuditLogService.API/AuditLogService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/AuditLogService.API/AuditLogService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuditLogService.API.dll"]
