FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["travelTransactionService/src/travelTransactionService.API/travelTransactionService.API.csproj", "travelTransactionService/src/travelTransactionService.API/"]
COPY ["travelTransactionService/src/travelTransactionService.Application/travelTransactionService.Application.csproj", "travelTransactionService/src/travelTransactionService.Application/"]
COPY ["travelTransactionService/src/travelTransactionService.Domain/travelTransactionService.Domain.csproj", "travelTransactionService/src/travelTransactionService.Domain/"]
COPY ["travelTransactionService/src/travelTransactionService.Infrastructure/travelTransactionService.Infrastructure.csproj", "travelTransactionService/src/travelTransactionService.Infrastructure/"]
COPY ["travelTransactionService/src/travelTransactionService.Functions/travelTransactionService.Functions.csproj", "travelTransactionService/src/travelTransactionService.Functions/"]

RUN dotnet restore "travelTransactionService/src/travelTransactionService.API/travelTransactionService.API.csproj"

COPY . .

RUN dotnet publish "travelTransactionService/src/travelTransactionService.API/travelTransactionService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5498
ENV ASPNETCORE_URLS=http://+:5498

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "travelTransactionService.API.dll"]
