# Multi-stage build for CompensationBenefits.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["compensationbenefitsServices/src/CompensationBenefits.API/CompensationBenefits.API.csproj", "compensationbenefitsServices/src/CompensationBenefits.API/"]
COPY ["compensationbenefitsServices/src/CompensationBenefits.Application/CompensationBenefits.Application.csproj", "compensationbenefitsServices/src/CompensationBenefits.Application/"]
COPY ["compensationbenefitsServices/src/CompensationBenefits.Domain/CompensationBenefits.Domain.csproj", "compensationbenefitsServices/src/CompensationBenefits.Domain/"]
COPY ["compensationbenefitsServices/src/CompensationBenefits.Infrastructure/CompensationBenefits.Infrastructure.csproj", "compensationbenefitsServices/src/CompensationBenefits.Infrastructure/"]

RUN dotnet restore "compensationbenefitsServices/src/CompensationBenefits.API/CompensationBenefits.API.csproj"

COPY . .

RUN dotnet build "compensationbenefitsServices/src/CompensationBenefits.API/CompensationBenefits.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "compensationbenefitsServices/src/CompensationBenefits.API/CompensationBenefits.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5009
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5009

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CompensationBenefits.API.dll"]
