FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5231
ENV ASPNETCORE_URLS=http://+:5231

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AccidentManagementService/AccidentManagementService.csproj", "AccidentManagementService/"]
RUN dotnet restore "AccidentManagementService/AccidentManagementService.csproj"
COPY . .
WORKDIR "/src/AccidentManagementService"
RUN dotnet build "AccidentManagementService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccidentManagementService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "AccidentManagementService.dll"]
