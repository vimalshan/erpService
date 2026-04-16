FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5431
ENV ASPNETCORE_URLS=http://+:5431

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY src/ApprovalGroup.API/ApprovalGroup.API.csproj src/ApprovalGroup.API/
COPY src/ApprovalGroup.Application/ApprovalGroup.Application.csproj src/ApprovalGroup.Application/
COPY src/ApprovalGroup.Domain/ApprovalGroup.Domain.csproj src/ApprovalGroup.Domain/
COPY src/ApprovalGroup.Infrastructure/ApprovalGroup.Infrastructure.csproj src/ApprovalGroup.Infrastructure/
RUN dotnet restore src/ApprovalGroup.API/ApprovalGroup.API.csproj

COPY . .
RUN dotnet publish src/ApprovalGroup.API/ApprovalGroup.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApprovalGroup.API.dll"]
