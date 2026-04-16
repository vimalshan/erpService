FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PayrollServices.API/PayrollServices.API.csproj", "PayrollServices.API/"]
COPY ["PayrollServices.Application/PayrollServices.Application.csproj", "PayrollServices.Application/"]
COPY ["PayrollServices.Domain/PayrollServices.Domain.csproj", "PayrollServices.Domain/"]
COPY ["PayrollServices.Functions/PayrollServices.Functions.csproj", "PayrollServices.Functions/"]
COPY ["PayrollServices.Infrastructure/PayrollServices.Infrastructure.csproj", "PayrollServices.Infrastructure/"]
RUN dotnet restore "PayrollServices.API/PayrollServices.API.csproj"

COPY . .
WORKDIR "/src/PayrollServices.API"
RUN dotnet build "PayrollServices.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PayrollServices.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 5354
ENV ASPNETCORE_URLS=http://+:5354
COPY --from=publish /app/publish .
RUN mkdir -p /app/Logs

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "PayrollServices.API.dll"]
