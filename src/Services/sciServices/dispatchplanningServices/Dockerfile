FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/DispatchPlanning.API/DispatchPlanning.API.csproj", "src/DispatchPlanning.API/"]
COPY ["src/DispatchPlanning.Application/DispatchPlanning.Application.csproj", "src/DispatchPlanning.Application/"]
COPY ["src/DispatchPlanning.Domain/DispatchPlanning.Domain.csproj", "src/DispatchPlanning.Domain/"]
COPY ["src/DispatchPlanning.Infrastructure/DispatchPlanning.Infrastructure.csproj", "src/DispatchPlanning.Infrastructure/"]
COPY ["src/DispatchPlanning.Functions/DispatchPlanning.Functions.csproj", "src/DispatchPlanning.Functions/"]
RUN dotnet restore "src/DispatchPlanning.API/DispatchPlanning.API.csproj"
COPY . .
WORKDIR "/src/src/DispatchPlanning.API"
RUN dotnet build "DispatchPlanning.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DispatchPlanning.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "DispatchPlanning.API.dll"]
