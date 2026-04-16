FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5518
ENV ASPNETCORE_URLS=http://+:5518
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PurchaseOrderService.API/PurchaseOrderService.API.csproj", "src/PurchaseOrderService.API/"]
COPY ["src/PurchaseOrderService.Application/PurchaseOrderService.Application.csproj", "src/PurchaseOrderService.Application/"]
COPY ["src/PurchaseOrderService.Domain/PurchaseOrderService.Domain.csproj", "src/PurchaseOrderService.Domain/"]
COPY ["src/PurchaseOrderService.Infrastructure/PurchaseOrderService.Infrastructure.csproj", "src/PurchaseOrderService.Infrastructure/"]
RUN dotnet restore "src/PurchaseOrderService.API/PurchaseOrderService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/PurchaseOrderService.API/PurchaseOrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PurchaseOrderService.API.dll"]
