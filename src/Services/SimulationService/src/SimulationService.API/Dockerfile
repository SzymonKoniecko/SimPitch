FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY SimulationService.sln ./
COPY src/SimulationService.API/*.csproj src/SimulationService.API/
COPY src/SimulationService.Application/*.csproj src/SimulationService.Application/
COPY src/SimulationService.Domain/*.csproj src/SimulationService.Domain/
COPY src/SimulationService.Infrastructure/*.csproj src/SimulationService.Infrastructure/
COPY tests/SimulationService.Tests/*.csproj tests/SimulationService.Tests/
COPY src/SimPitchProtos/ SimPitchProtos/

RUN dotnet restore SimulationService.sln

COPY . .

RUN dotnet publish src/SimulationService.API/SimulationService.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "SimulationService.API.dll"]
