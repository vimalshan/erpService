FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5500

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["HealthTransaction.API/HealthTransaction.API.csproj", "HealthTransaction.API/"]
COPY ["HealthTransaction.Application/HealthTransaction.Application.csproj", "HealthTransaction.Application/"]
COPY ["HealthTransaction.Domain/HealthTransaction.Domain.csproj", "HealthTransaction.Domain/"]
COPY ["HealthTransaction.Infrastructure/HealthTransaction.Infrastructure.csproj", "HealthTransaction.Infrastructure/"]
RUN dotnet restore "HealthTransaction.API/HealthTransaction.API.csproj"
COPY . .
WORKDIR "/src/HealthTransaction.API"
RUN dotnet build "HealthTransaction.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthTransaction.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5500
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "HealthTransaction.API.dll"]
