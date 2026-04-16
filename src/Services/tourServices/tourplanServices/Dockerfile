FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5475
ENV ASPNETCORE_URLS=http://+:5475

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/TourPlanService.Domain/TourPlanService.Domain.csproj src/TourPlanService.Domain/
COPY src/TourPlanService.Application/TourPlanService.Application.csproj src/TourPlanService.Application/
COPY src/TourPlanService.Infrastructure/TourPlanService.Infrastructure.csproj src/TourPlanService.Infrastructure/
COPY src/TourPlanService.API/TourPlanService.API.csproj src/TourPlanService.API/
RUN dotnet restore src/TourPlanService.API/TourPlanService.API.csproj
COPY . .
WORKDIR /src/src/TourPlanService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "TourPlanService.API.dll"]
