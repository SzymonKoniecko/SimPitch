FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY EngineService.sln ./
COPY src/EngineService.API/*.csproj src/EngineService.API/
COPY src/EngineService.Application/*.csproj src/EngineService.Application/
COPY src/EngineService.Domain/*.csproj src/EngineService.Domain/
COPY src/EngineService.Infrastructure/*.csproj src/EngineService.Infrastructure/
COPY tests/EngineService.Tests/*.csproj tests/EngineService.Tests/
COPY src/SimPitchProtos/ SimPitchProtos/

RUN dotnet restore EngineService.sln

COPY . .

RUN dotnet publish src/EngineService.API/EngineService.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "EngineService.API.dll"]
