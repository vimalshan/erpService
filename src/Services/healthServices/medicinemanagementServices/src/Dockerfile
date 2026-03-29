FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5400

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MedicineManagement.API/MedicineManagement.API.csproj", "MedicineManagement.API/"]
COPY ["MedicineManagement.Application/MedicineManagement.Application.csproj", "MedicineManagement.Application/"]
COPY ["MedicineManagement.Domain/MedicineManagement.Domain.csproj", "MedicineManagement.Domain/"]
COPY ["MedicineManagement.Infrastructure/MedicineManagement.Infrastructure.csproj", "MedicineManagement.Infrastructure/"]
RUN dotnet restore "MedicineManagement.API/MedicineManagement.API.csproj"
COPY . .
WORKDIR "/src/MedicineManagement.API"
RUN dotnet build "MedicineManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MedicineManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5400
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "MedicineManagement.API.dll"]
