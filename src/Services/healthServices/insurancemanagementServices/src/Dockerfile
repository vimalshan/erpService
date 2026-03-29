FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5100

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InsuranceManagement.API/InsuranceManagement.API.csproj", "InsuranceManagement.API/"]
COPY ["InsuranceManagement.Application/InsuranceManagement.Application.csproj", "InsuranceManagement.Application/"]
COPY ["InsuranceManagement.Domain/InsuranceManagement.Domain.csproj", "InsuranceManagement.Domain/"]
COPY ["InsuranceManagement.Infrastructure/InsuranceManagement.Infrastructure.csproj", "InsuranceManagement.Infrastructure/"]
RUN dotnet restore "InsuranceManagement.API/InsuranceManagement.API.csproj"
COPY . .
WORKDIR "/src/InsuranceManagement.API"
RUN dotnet build "InsuranceManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InsuranceManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5100
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "InsuranceManagement.API.dll"]
