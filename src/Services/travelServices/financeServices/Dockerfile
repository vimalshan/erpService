FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["FinanceService/src/FinanceService.API/FinanceService.API.csproj", "FinanceService/src/FinanceService.API/"]
COPY ["FinanceService/src/FinanceService.Application/FinanceService.Application.csproj", "FinanceService/src/FinanceService.Application/"]
COPY ["FinanceService/src/FinanceService.Domain/FinanceService.Domain.csproj", "FinanceService/src/FinanceService.Domain/"]
COPY ["FinanceService/src/FinanceService.Infrastructure/FinanceService.Infrastructure.csproj", "FinanceService/src/FinanceService.Infrastructure/"]
COPY ["FinanceService/src/FinanceService.AzureFunctions/FinanceService.AzureFunctions.csproj", "FinanceService/src/FinanceService.AzureFunctions/"]

RUN dotnet restore "FinanceService/src/FinanceService.API/FinanceService.API.csproj"

COPY . .

RUN dotnet publish "FinanceService/src/FinanceService.API/FinanceService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinanceService.API.dll"]
