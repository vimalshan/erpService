FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TravelRequestService/src/TravelRequestService.API/TravelRequestService.API.csproj", "TravelRequestService/src/TravelRequestService.API/"]
COPY ["TravelRequestService/src/TravelRequestService.Application/TravelRequestService.Application.csproj", "TravelRequestService/src/TravelRequestService.Application/"]
COPY ["TravelRequestService/src/TravelRequestService.Domain/TravelRequestService.Domain.csproj", "TravelRequestService/src/TravelRequestService.Domain/"]
COPY ["TravelRequestService/src/TravelRequestService.Infrastructure/TravelRequestService.Infrastructure.csproj", "TravelRequestService/src/TravelRequestService.Infrastructure/"]
COPY ["TravelRequestService/src/TravelRequestService.Functions/TravelRequestService.Functions.csproj", "TravelRequestService/src/TravelRequestService.Functions/"]

RUN dotnet restore "TravelRequestService/src/TravelRequestService.API/TravelRequestService.API.csproj"

COPY . .

RUN dotnet publish "TravelRequestService/src/TravelRequestService.API/TravelRequestService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5499
ENV ASPNETCORE_URLS=http://+:5499

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TravelRequestService.API.dll"]
