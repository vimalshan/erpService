FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/PayTransactionalService.API/PayTransactionalService.API.csproj", "src/PayTransactionalService.API/"]
COPY ["src/PayTransactionalService.Application/PayTransactionalService.Application.csproj", "src/PayTransactionalService.Application/"]
COPY ["src/PayTransactionalService.Domain/PayTransactionalService.Domain.csproj", "src/PayTransactionalService.Domain/"]
COPY ["src/PayTransactionalService.Functions/PayTransactionalService.Functions.csproj", "src/PayTransactionalService.Functions/"]
COPY ["src/PayTransactionalService.Infrastructure/PayTransactionalService.Infrastructure.csproj", "src/PayTransactionalService.Infrastructure/"]
RUN dotnet restore "src/PayTransactionalService.API/PayTransactionalService.API.csproj"

COPY . .
WORKDIR "/src/src/PayTransactionalService.API"
RUN dotnet build "PayTransactionalService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PayTransactionalService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/Logs

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80
ENTRYPOINT ["dotnet", "PayTransactionalService.API.dll"]
