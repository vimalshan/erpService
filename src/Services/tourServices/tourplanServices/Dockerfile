FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TourPlanService.API/TourPlanService.API.csproj", "src/TourPlanService.API/"]
COPY ["src/TourPlanService.Application/TourPlanService.Application.csproj", "src/TourPlanService.Application/"]
COPY ["src/TourPlanService.Domain/TourPlanService.Domain.csproj", "src/TourPlanService.Domain/"]
COPY ["src/TourPlanService.Infrastructure/TourPlanService.Infrastructure.csproj", "src/TourPlanService.Infrastructure/"]
RUN dotnet restore "src/TourPlanService.API/TourPlanService.API.csproj"
COPY . .
WORKDIR "/src/src/TourPlanService.API"
RUN dotnet build "TourPlanService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TourPlanService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "TourPlanService.API.dll"]
