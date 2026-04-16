FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5399
ENV ASPNETCORE_URLS=http://+:5399

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/OrderScheduleService.API/OrderScheduleService.API.csproj", "src/OrderScheduleService.API/"]
COPY ["src/OrderScheduleService.Application/OrderScheduleService.Application.csproj", "src/OrderScheduleService.Application/"]
COPY ["src/OrderScheduleService.Domain/OrderScheduleService.Domain.csproj", "src/OrderScheduleService.Domain/"]
COPY ["src/OrderScheduleService.Infrastructure/OrderScheduleService.Infrastructure.csproj", "src/OrderScheduleService.Infrastructure/"]
COPY ["src/OrderScheduleService.IntegrationEvents/OrderScheduleService.IntegrationEvents.csproj", "src/OrderScheduleService.IntegrationEvents/"]
RUN dotnet restore "src/OrderScheduleService.API/OrderScheduleService.API.csproj"
COPY . .
WORKDIR "/src/src/OrderScheduleService.API"
RUN dotnet build "OrderScheduleService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderScheduleService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "OrderScheduleService.API.dll"]
