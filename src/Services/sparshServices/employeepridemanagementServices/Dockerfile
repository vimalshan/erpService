FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5411
ENV ASPNETCORE_URLS=http://+:5411

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/EmployeePrideManagement.API/EmployeePrideManagement.API.csproj", "src/EmployeePrideManagement.API/"]
COPY ["src/EmployeePrideManagement.Application/EmployeePrideManagement.Application.csproj", "src/EmployeePrideManagement.Application/"]
COPY ["src/EmployeePrideManagement.Domain/EmployeePrideManagement.Domain.csproj", "src/EmployeePrideManagement.Domain/"]
COPY ["src/EmployeePrideManagement.Infrastructure/EmployeePrideManagement.Infrastructure.csproj", "src/EmployeePrideManagement.Infrastructure/"]
RUN dotnet restore "src/EmployeePrideManagement.API/EmployeePrideManagement.API.csproj"
COPY . .
WORKDIR "/src/src/EmployeePrideManagement.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "EmployeePrideManagement.API.dll"]
