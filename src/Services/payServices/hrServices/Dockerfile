FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HRService.API/HRService.API.csproj", "HRService.API/"]
COPY ["HRService.Domain/HRService.Domain.csproj", "HRService.Domain/"]
COPY ["HRService.Application/HRService.Application.csproj", "HRService.Application/"]
COPY ["HRService.Infrastructure/HRService.Infrastructure.csproj", "HRService.Infrastructure/"]
COPY ["HRService.Common/HRService.Common.csproj", "HRService.Common/"]
RUN dotnet restore "HRService.API/HRService.API.csproj"

COPY . .
WORKDIR "/src/HRService.API"
RUN dotnet build "HRService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HRService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 5353
ENV ASPNETCORE_URLS=http://+:5353
COPY --from=publish /app/publish .
RUN mkdir -p /app/Logs

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "HRService.API.dll"]
