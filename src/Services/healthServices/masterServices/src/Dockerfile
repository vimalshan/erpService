FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5235
ENV ASPNETCORE_URLS=http://+:5235

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Masters.API/Masters.API.csproj", "Masters.API/"]
COPY ["Masters.Application/Masters.Application.csproj", "Masters.Application/"]
COPY ["Masters.Domain/Masters.Domain.csproj", "Masters.Domain/"]
COPY ["Masters.Infrastructure/Masters.Infrastructure.csproj", "Masters.Infrastructure/"]
RUN dotnet restore "Masters.API/Masters.API.csproj"
COPY . .
WORKDIR "/src/Masters.API"
RUN dotnet build "Masters.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Masters.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Masters.API.dll"]
