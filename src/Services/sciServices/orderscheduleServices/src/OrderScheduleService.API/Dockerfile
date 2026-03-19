FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderScheduleService.API/OrderScheduleService.API.csproj", "OrderScheduleService.API/"]
COPY ["OrderScheduleService.Application/OrderScheduleService.Application.csproj", "OrderScheduleService.Application/"]
COPY ["OrderScheduleService.Domain/OrderScheduleService.Domain.csproj", "OrderScheduleService.Domain/"]
COPY ["OrderScheduleService.Infrastructure/OrderScheduleService.Infrastructure.csproj", "OrderScheduleService.Infrastructure/"]
COPY ["OrderScheduleService.IntegrationEvents/OrderScheduleService.IntegrationEvents.csproj", "OrderScheduleService.IntegrationEvents/"]

RUN dotnet restore "OrderScheduleService.API/OrderScheduleService.API.csproj"

COPY . .

WORKDIR "/src/OrderScheduleService.API"
RUN dotnet build "OrderScheduleService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderScheduleService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderScheduleService.API.dll"]
