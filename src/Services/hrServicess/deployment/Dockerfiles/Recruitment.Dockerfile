# Multi-stage build for RecruitmentService.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["recruitmentServices/src/RecruitmentService.API/RecruitmentService.API.csproj", "recruitmentServices/src/RecruitmentService.API/"]
COPY ["recruitmentServices/src/RecruitmentService.Application/RecruitmentService.Application.csproj", "recruitmentServices/src/RecruitmentService.Application/"]
COPY ["recruitmentServices/src/RecruitmentService.Domain/RecruitmentService.Domain.csproj", "recruitmentServices/src/RecruitmentService.Domain/"]
COPY ["recruitmentServices/src/RecruitmentService.Infrastructure/RecruitmentService.Infrastructure.csproj", "recruitmentServices/src/RecruitmentService.Infrastructure/"]

RUN dotnet restore "recruitmentServices/src/RecruitmentService.API/RecruitmentService.API.csproj"

COPY . .

RUN dotnet build "recruitmentServices/src/RecruitmentService.API/RecruitmentService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "recruitmentServices/src/RecruitmentService.API/RecruitmentService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5265
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5265

RUN apt-get update --fix-missing && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/* || \
    (apt-get update --fix-missing && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*)

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "RecruitmentService.API.dll"]
