FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/TaxService.API/TaxService.API.csproj", "src/TaxService.API/"]
COPY ["src/TaxService.Application/TaxService.Application.csproj", "src/TaxService.Application/"]
COPY ["src/TaxService.Background/TaxService.Background.csproj", "src/TaxService.Background/"]
COPY ["src/TaxService.Domain/TaxService.Domain.csproj", "src/TaxService.Domain/"]
COPY ["src/TaxService.Infrastructure/TaxService.Infrastructure.csproj", "src/TaxService.Infrastructure/"]
RUN dotnet restore "src/TaxService.API/TaxService.API.csproj"

COPY . .
WORKDIR "/src/src/TaxService.API"
RUN dotnet build "TaxService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaxService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 5356
ENV ASPNETCORE_URLS=http://+:5356
COPY --from=publish /app/publish .
RUN mkdir -p /app/Logs

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TaxService.API.dll"]
