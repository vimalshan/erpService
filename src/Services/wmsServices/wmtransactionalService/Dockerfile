FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5526
ENV ASPNETCORE_URLS=http://+:5526
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WMTransactional.API/WMTransactional.API.csproj", "src/WMTransactional.API/"]
COPY ["src/WMTransactional.Application/WMTransactional.Application.csproj", "src/WMTransactional.Application/"]
COPY ["src/WMTransactional.Domain/WMTransactional.Domain.csproj", "src/WMTransactional.Domain/"]
COPY ["src/WMTransactional.Infrastructure/WMTransactional.Infrastructure.csproj", "src/WMTransactional.Infrastructure/"]
RUN dotnet restore "src/WMTransactional.API/WMTransactional.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/WMTransactional.API/WMTransactional.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WMTransactional.API.dll"]
