FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5517
ENV ASPNETCORE_URLS=http://+:5517
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ProductService.API/ProductService.API.csproj", "src/ProductService.API/"]
COPY ["src/ProductService.Application/ProductService.Application.csproj", "src/ProductService.Application/"]
COPY ["src/ProductService.Domain/ProductService.Domain.csproj", "src/ProductService.Domain/"]
COPY ["src/ProductService.Infrastructure/ProductService.Infrastructure.csproj", "src/ProductService.Infrastructure/"]
RUN dotnet restore "src/ProductService.API/ProductService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/ProductService.API/ProductService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductService.API.dll"]
