FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ExpenseService/src/ExpenseService.API/ExpenseService.API.csproj", "ExpenseService/src/ExpenseService.API/"]
COPY ["ExpenseService/src/ExpenseService.Application/ExpenseService.Application.csproj", "ExpenseService/src/ExpenseService.Application/"]
COPY ["ExpenseService/src/ExpenseService.Domain/ExpenseService.Domain.csproj", "ExpenseService/src/ExpenseService.Domain/"]
COPY ["ExpenseService/src/ExpenseService.Infrastructure/ExpenseService.Infrastructure.csproj", "ExpenseService/src/ExpenseService.Infrastructure/"]
COPY ["ExpenseService/src/ExpenseService.Functions/ExpenseService.Functions.csproj", "ExpenseService/src/ExpenseService.Functions/"]

RUN dotnet restore "ExpenseService/src/ExpenseService.API/ExpenseService.API.csproj"

COPY . .

RUN dotnet publish "ExpenseService/src/ExpenseService.API/ExpenseService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ExpenseService.API.dll"]
