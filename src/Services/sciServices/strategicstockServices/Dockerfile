FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/StrategicStock.API/StrategicStock.API.csproj", "src/StrategicStock.API/"]
COPY ["src/StrategicStock.Application/StrategicStock.Application.csproj", "src/StrategicStock.Application/"]
COPY ["src/StrategicStock.Domain/StrategicStock.Domain.csproj", "src/StrategicStock.Domain/"]
COPY ["src/StrategicStock.Infrastructure/StrategicStock.Infrastructure.csproj", "src/StrategicStock.Infrastructure/"]
COPY ["src/StrategicStock.Functions/StrategicStock.Functions.csproj", "src/StrategicStock.Functions/"]
RUN dotnet restore "src/StrategicStock.API/StrategicStock.API.csproj"
COPY . .
WORKDIR "/src/src/StrategicStock.API"
RUN dotnet build "StrategicStock.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StrategicStock.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "StrategicStock.API.dll"]
