FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5522
ENV ASPNETCORE_URLS=http://+:5522
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SecurityService.API/SecurityService.API.csproj", "src/SecurityService.API/"]
COPY ["src/SecurityService.Application/SecurityService.Application.csproj", "src/SecurityService.Application/"]
COPY ["src/SecurityService.Domain/SecurityService.Domain.csproj", "src/SecurityService.Domain/"]
COPY ["src/SecurityService.Infrastructure/SecurityService.Infrastructure.csproj", "src/SecurityService.Infrastructure/"]
RUN dotnet restore "src/SecurityService.API/SecurityService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/SecurityService.API/SecurityService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SecurityService.API.dll"]
