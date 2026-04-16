FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5397
ENV ASPNETCORE_URLS=http://+:5397

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/MamAllocationService.Api/MamAllocationService.Api.csproj", "src/MamAllocationService.Api/"]
COPY ["src/MamAllocationService.Application/MamAllocationService.Application.csproj", "src/MamAllocationService.Application/"]
COPY ["src/MamAllocationService.Domain/MamAllocationService.Domain.csproj", "src/MamAllocationService.Domain/"]
COPY ["src/MamAllocationService.Infrastructure/MamAllocationService.Infrastructure.csproj", "src/MamAllocationService.Infrastructure/"]
COPY ["src/MamAllocationService.Functions/MamAllocationService.Functions.csproj", "src/MamAllocationService.Functions/"]
RUN dotnet restore "src/MamAllocationService.Api/MamAllocationService.Api.csproj"
COPY . .
WORKDIR "/src/src/MamAllocationService.Api"
RUN dotnet build "MamAllocationService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MamAllocationService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "MamAllocationService.Api.dll"]
