FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5524
ENV ASPNETCORE_URLS=http://+:5524
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SupplierService.API/SupplierService.API.csproj", "src/SupplierService.API/"]
COPY ["src/SupplierService.Application/SupplierService.Application.csproj", "src/SupplierService.Application/"]
COPY ["src/SupplierService.Domain/SupplierService.Domain.csproj", "src/SupplierService.Domain/"]
COPY ["src/SupplierService.Infrastructure/SupplierService.Infrastructure.csproj", "src/SupplierService.Infrastructure/"]
RUN dotnet restore "src/SupplierService.API/SupplierService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/SupplierService.API/SupplierService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SupplierService.API.dll"]
