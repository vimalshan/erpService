FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5521
ENV ASPNETCORE_URLS=http://+:5521
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "./"]
COPY ["src/SalesOrderService.API/SalesOrderService.API.csproj", "src/SalesOrderService.API/"]
COPY ["src/SalesOrderService.Application/SalesOrderService.Application.csproj", "src/SalesOrderService.Application/"]
COPY ["src/SalesOrderService.Domain/SalesOrderService.Domain.csproj", "src/SalesOrderService.Domain/"]
COPY ["src/SalesOrderService.Infrastructure/SalesOrderService.Infrastructure.csproj", "src/SalesOrderService.Infrastructure/"]
RUN dotnet restore "src/SalesOrderService.API/SalesOrderService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/SalesOrderService.API/SalesOrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SalesOrderService.API.dll"]
