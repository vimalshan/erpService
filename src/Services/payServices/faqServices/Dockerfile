FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/FaqServices.API/FaqServices.API.csproj", "src/FaqServices.API/"]
COPY ["src/FaqServices.Application/FaqServices.Application.csproj", "src/FaqServices.Application/"]
COPY ["src/FaqServices.Domain/FaqServices.Domain.csproj", "src/FaqServices.Domain/"]
COPY ["src/FaqServices.Functions/FaqServices.Functions.csproj", "src/FaqServices.Functions/"]
COPY ["src/FaqServices.Infrastructure/FaqServices.Infrastructure.csproj", "src/FaqServices.Infrastructure/"]
RUN dotnet restore "src/FaqServices.API/FaqServices.API.csproj"

COPY . .
WORKDIR "/src/src/FaqServices.API"
RUN dotnet build "FaqServices.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FaqServices.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/Logs

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80
ENTRYPOINT ["dotnet", "FaqServices.API.dll"]
