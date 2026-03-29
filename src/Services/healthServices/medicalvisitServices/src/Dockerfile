FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5300

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MedicalVisit.API/MedicalVisit.API.csproj", "MedicalVisit.API/"]
COPY ["MedicalVisit.Application/MedicalVisit.Application.csproj", "MedicalVisit.Application/"]
COPY ["MedicalVisit.Domain/MedicalVisit.Domain.csproj", "MedicalVisit.Domain/"]
COPY ["MedicalVisit.Infrastructure/MedicalVisit.Infrastructure.csproj", "MedicalVisit.Infrastructure/"]
RUN dotnet restore "MedicalVisit.API/MedicalVisit.API.csproj"
COPY . .
WORKDIR "/src/MedicalVisit.API"
RUN dotnet build "MedicalVisit.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MedicalVisit.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5300
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "MedicalVisit.API.dll"]
