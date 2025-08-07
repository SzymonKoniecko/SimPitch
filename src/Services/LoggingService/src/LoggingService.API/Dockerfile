FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY LoggingService.sln ./
COPY src/LoggingService.API/*.csproj src/LoggingService.API/
COPY src/LoggingService.Application/*.csproj src/LoggingService.Application/
COPY src/LoggingService.Domain/*.csproj src/LoggingService.Domain/
COPY src/LoggingService.Infrastructure/*.csproj src/LoggingService.Infrastructure/
COPY tests/LoggingService.Tests/*.csproj tests/LoggingService.Tests/
COPY src/SimPitchProtos/ SimPitchProtos/

RUN dotnet restore LoggingService.sln

COPY . .

RUN dotnet publish src/LoggingService.API/LoggingService.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "LoggingService.API.dll"]
