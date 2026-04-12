FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/InsuranceService.API/InsuranceService.API.csproj", "src/InsuranceService.API/"]
COPY ["src/InsuranceService.Application/InsuranceService.Application.csproj", "src/InsuranceService.Application/"]
COPY ["src/InsuranceService.Domain/InsuranceService.Domain.csproj", "src/InsuranceService.Domain/"]
COPY ["src/InsuranceService.Infrastructure/InsuranceService.Infrastructure.csproj", "src/InsuranceService.Infrastructure/"]
COPY ["src/InsuranceService.Functions/InsuranceService.Functions.csproj", "src/InsuranceService.Functions/"]

RUN dotnet restore "src/InsuranceService.API/InsuranceService.API.csproj"

COPY . .

RUN dotnet publish "src/InsuranceService.API/InsuranceService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InsuranceService.API.dll"]
