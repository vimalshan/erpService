FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RackingSystem.API/RackingSystem.API.csproj", "src/RackingSystem.API/"]
COPY ["src/RackingSystem.Application/RackingSystem.Application.csproj", "src/RackingSystem.Application/"]
COPY ["src/RackingSystem.Domain/RackingSystem.Domain.csproj", "src/RackingSystem.Domain/"]
COPY ["src/RackingSystem.Infrastructure/RackingSystem.Infrastructure.csproj", "src/RackingSystem.Infrastructure/"]
RUN dotnet restore "src/RackingSystem.API/RackingSystem.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/RackingSystem.API/RackingSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RackingSystem.API.dll"]
