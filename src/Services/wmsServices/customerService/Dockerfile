FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5512
ENV ASPNETCORE_URLS=http://+:5512
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CustomerService.API/CustomerService.API.csproj", "src/CustomerService.API/"]
COPY ["src/CustomerService.Application/CustomerService.Application.csproj", "src/CustomerService.Application/"]
COPY ["src/CustomerService.Domain/CustomerService.Domain.csproj", "src/CustomerService.Domain/"]
COPY ["src/CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "src/CustomerService.Infrastructure/"]
RUN dotnet restore "src/CustomerService.API/CustomerService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/CustomerService.API/CustomerService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.API.dll"]
