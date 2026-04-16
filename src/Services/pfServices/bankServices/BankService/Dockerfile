FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5372
ENV ASPNETCORE_URLS=http://+:5372
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/BankService.API/BankService.API.csproj", "src/BankService.API/"]
COPY ["src/BankService.Application/BankService.Application.csproj", "src/BankService.Application/"]
COPY ["src/BankService.Domain/BankService.Domain.csproj", "src/BankService.Domain/"]
COPY ["src/BankService.Infrastructure/BankService.Infrastructure.csproj", "src/BankService.Infrastructure/"]
RUN dotnet restore "src/BankService.API/BankService.API.csproj"

COPY . .
RUN dotnet publish "src/BankService.API/BankService.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BankService.API.dll"]
