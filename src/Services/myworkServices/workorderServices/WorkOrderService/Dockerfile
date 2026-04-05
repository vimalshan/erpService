FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/WorkOrderService.Domain/WorkOrderService.Domain.csproj",           "src/WorkOrderService.Domain/"]
COPY ["src/WorkOrderService.Application/WorkOrderService.Application.csproj", "src/WorkOrderService.Application/"]
COPY ["src/WorkOrderService.Infrastructure/WorkOrderService.Infrastructure.csproj", "src/WorkOrderService.Infrastructure/"]
COPY ["src/WorkOrderService.API/WorkOrderService.API.csproj",                 "src/WorkOrderService.API/"]
RUN dotnet restore "src/WorkOrderService.API/WorkOrderService.API.csproj"

COPY . .
RUN dotnet publish "src/WorkOrderService.API/WorkOrderService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WorkOrderService.API.dll"]
