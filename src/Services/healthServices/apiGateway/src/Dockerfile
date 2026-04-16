FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5230
ENV ASPNETCORE_URLS=http://+:5230

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["HealthGateway/HealthGateway.csproj", "HealthGateway/"]
RUN dotnet restore "HealthGateway/HealthGateway.csproj"
COPY . .
WORKDIR "/src/HealthGateway"
RUN dotnet build "HealthGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "HealthGateway.dll"]
