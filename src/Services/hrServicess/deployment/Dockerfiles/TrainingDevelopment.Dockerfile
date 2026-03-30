# Multi-stage build for TrainingDevelopment.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["trainingdevelopmentServices/src/TrainingDevelopment.API/TrainingDevelopment.API.csproj", "trainingdevelopmentServices/src/TrainingDevelopment.API/"]
COPY ["trainingdevelopmentServices/src/TrainingDevelopment.Application/TrainingDevelopment.Application.csproj", "trainingdevelopmentServices/src/TrainingDevelopment.Application/"]
COPY ["trainingdevelopmentServices/src/TrainingDevelopment.Domain/TrainingDevelopment.Domain.csproj", "trainingdevelopmentServices/src/TrainingDevelopment.Domain/"]
COPY ["trainingdevelopmentServices/src/TrainingDevelopment.Infrastructure/TrainingDevelopment.Infrastructure.csproj", "trainingdevelopmentServices/src/TrainingDevelopment.Infrastructure/"]

RUN dotnet restore "trainingdevelopmentServices/src/TrainingDevelopment.API/TrainingDevelopment.API.csproj"

COPY . .

RUN dotnet build "trainingdevelopmentServices/src/TrainingDevelopment.API/TrainingDevelopment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "trainingdevelopmentServices/src/TrainingDevelopment.API/TrainingDevelopment.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5003
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5003

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TrainingDevelopment.API.dll"]
