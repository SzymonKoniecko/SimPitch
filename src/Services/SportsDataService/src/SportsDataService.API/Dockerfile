FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY SportsDataService.sln ./
COPY src/SportsDataService.API/*.csproj src/SportsDataService.API/
COPY src/SportsDataService.Application/*.csproj src/SportsDataService.Application/
COPY src/SportsDataService.Domain/*.csproj src/SportsDataService.Domain/
COPY src/SportsDataService.Infrastructure/*.csproj src/SportsDataService.Infrastructure/
COPY tests/SportsDataService.Tests/*.csproj tests/SportsDataService.Tests/
COPY src/SimPitchProtos/ SimPitchProtos/

RUN dotnet restore SportsDataService.sln

COPY . .

RUN dotnet publish src/SportsDataService.API/SportsDataService.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "SportsDataService.API.dll"]
