FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5401
ENV ASPNETCORE_URLS=http://+:5401

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/PurchaseSalesService.API/PurchaseSalesService.API.csproj", "src/PurchaseSalesService.API/"]
COPY ["src/PurchaseSalesService.Application/PurchaseSalesService.Application.csproj", "src/PurchaseSalesService.Application/"]
COPY ["src/PurchaseSalesService.Domain/PurchaseSalesService.Domain.csproj", "src/PurchaseSalesService.Domain/"]
COPY ["src/PurchaseSalesService.Infrastructure/PurchaseSalesService.Infrastructure.csproj", "src/PurchaseSalesService.Infrastructure/"]
COPY ["src/PurchaseSalesService.Functions/PurchaseSalesService.Functions.csproj", "src/PurchaseSalesService.Functions/"]
RUN dotnet restore "src/PurchaseSalesService.API/PurchaseSalesService.API.csproj"
COPY . .
WORKDIR "/src/src/PurchaseSalesService.API"
RUN dotnet build "PurchaseSalesService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PurchaseSalesService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "PurchaseSalesService.API.dll"]
