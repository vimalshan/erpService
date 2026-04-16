FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5331
ENV ASPNETCORE_URLS=http://+:5331

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/AuditService.Domain/AuditService.Domain.csproj",         "src/AuditService.Domain/"]
COPY ["src/AuditService.Application/AuditService.Application.csproj", "src/AuditService.Application/"]
COPY ["src/AuditService.Infrastructure/AuditService.Infrastructure.csproj", "src/AuditService.Infrastructure/"]
COPY ["src/AuditService.API/AuditService.API.csproj",               "src/AuditService.API/"]
RUN dotnet restore "src/AuditService.API/AuditService.API.csproj"

COPY . .
RUN dotnet publish "src/AuditService.API/AuditService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuditService.API.dll"]
