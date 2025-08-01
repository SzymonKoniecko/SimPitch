FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY SportsDataService.sln . 

COPY src/SportsDataService.API/*.csproj src/SportsDataService.API/
COPY src/SportsDataService.Application/*.csproj src/SportsDataService.Application/
COPY src/SportsDataService.Domain/*.csproj src/SportsDataService.Domain/
COPY src/SportsDataService.Infrastructure/*.csproj src/SportsDataService.Infrastructure/
COPY tests/SportsDataService.Tests/*.csproj tests/SportsDataService.Tests/



RUN dotnet restore

COPY . .

RUN dotnet publish src/SportsDataService.API/SportsDataService.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out ./

##EXPOSE 5001
ENTRYPOINT ["dotnet", "SportsDataService.API.dll"]
