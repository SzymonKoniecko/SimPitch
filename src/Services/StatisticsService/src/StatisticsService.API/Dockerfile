FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY StatisticsService.sln ./
COPY src/StatisticsService.API/*.csproj src/StatisticsService.API/
COPY src/StatisticsService.Application/*.csproj src/StatisticsService.Application/
COPY src/StatisticsService.Domain/*.csproj src/StatisticsService.Domain/
COPY src/StatisticsService.Infrastructure/*.csproj src/StatisticsService.Infrastructure/
COPY tests/StatisticsService.Tests/*.csproj tests/StatisticsService.Tests/
COPY src/SimPitchProtos/ SimPitchProtos/

RUN dotnet restore StatisticsService.sln

COPY . .

RUN dotnet publish src/StatisticsService.API/StatisticsService.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "StatisticsService.API.dll"]
