# Multi-stage build for Hr.ApiGateway
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["apiGatewayServices/src/Hr.ApiGateway/Hr.ApiGateway.csproj", "apiGatewayServices/src/Hr.ApiGateway/"]

RUN dotnet restore "apiGatewayServices/src/Hr.ApiGateway/Hr.ApiGateway.csproj"

COPY . .

RUN dotnet build "apiGatewayServices/src/Hr.ApiGateway/Hr.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "apiGatewayServices/src/Hr.ApiGateway/Hr.ApiGateway.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5310
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5310

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Hr.ApiGateway.dll"]
