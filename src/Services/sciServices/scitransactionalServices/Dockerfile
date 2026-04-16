FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5402
ENV ASPNETCORE_URLS=http://+:5402

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/SciTransactional.API/SciTransactional.API.csproj", "src/SciTransactional.API/"]
COPY ["src/SciTransactional.Application/SciTransactional.Application.csproj", "src/SciTransactional.Application/"]
COPY ["src/SciTransactional.Domain/SciTransactional.Domain.csproj", "src/SciTransactional.Domain/"]
COPY ["src/SciTransactional.Infrastructure/SciTransactional.Infrastructure.csproj", "src/SciTransactional.Infrastructure/"]
COPY ["src/SciTransactional.Functions/SciTransactional.Functions.csproj", "src/SciTransactional.Functions/"]
RUN dotnet restore "src/SciTransactional.API/SciTransactional.API.csproj"
COPY . .
WORKDIR "/src/src/SciTransactional.API"
RUN dotnet build "SciTransactional.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SciTransactional.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "SciTransactional.API.dll"]
