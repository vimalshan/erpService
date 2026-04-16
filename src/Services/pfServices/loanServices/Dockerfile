FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5375
ENV ASPNETCORE_URLS=http://+:5375
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/LoanService.Api/LoanService.Api.csproj", "src/LoanService.Api/"]
COPY ["src/LoanService.Application/LoanService.Application.csproj", "src/LoanService.Application/"]
COPY ["src/LoanService.Domain/LoanService.Domain.csproj", "src/LoanService.Domain/"]
COPY ["src/LoanService.Infrastructure/LoanService.Infrastructure.csproj", "src/LoanService.Infrastructure/"]
COPY ["src/LoanService.AzureFunctions/LoanService.AzureFunctions.csproj", "src/LoanService.AzureFunctions/"]
RUN dotnet restore "src/LoanService.Api/LoanService.Api.csproj"

COPY . .
RUN dotnet publish "src/LoanService.Api/LoanService.Api.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LoanService.Api.dll"]
