FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/ProblemManagement.API/ProblemManagement.API.csproj", "src/ProblemManagement.API/"]
COPY ["src/ProblemManagement.Application/ProblemManagement.Application.csproj", "src/ProblemManagement.Application/"]
COPY ["src/ProblemManagement.Domain/ProblemManagement.Domain.csproj", "src/ProblemManagement.Domain/"]
COPY ["src/ProblemManagement.Infrastructure/ProblemManagement.Infrastructure.csproj", "src/ProblemManagement.Infrastructure/"]
RUN dotnet restore "src/ProblemManagement.API/ProblemManagement.API.csproj"
COPY . .
WORKDIR "/src/src/ProblemManagement.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ProblemManagement.API.dll"]
