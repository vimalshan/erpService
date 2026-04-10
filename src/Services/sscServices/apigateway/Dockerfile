FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/SSC.ApiGateway/SSC.ApiGateway.csproj", "src/SSC.ApiGateway/"]
RUN dotnet restore "src/SSC.ApiGateway/SSC.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/SSC.ApiGateway"
RUN dotnet build "SSC.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SSC.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy ocelot config
COPY ["src/SSC.ApiGateway/ocelot.json", "."]

HEALTHCHECK --interval=30s --timeout=10s --retries=3 \
    CMD curl -f http://localhost:8080/health/live || exit 1

ENTRYPOINT ["dotnet", "SSC.ApiGateway.dll"]
