FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Services/SportsDataService/SportsDataService.sln . 

COPY Services/SportsDataService/SportsDataService.API/*.csproj SportsDataService.API/
COPY Services/SportsDataService/SportsDataService.Application/*.csproj SportsDataService.Application/
COPY Services/SportsDataService/SportsDataService.Domain/*.csproj SportsDataService.Domain/
COPY Services/SportsDataService/SportsDataService.Infrastructure/*.csproj SportsDataService.Infrastructure/
COPY Services/SportsDataService/SportsDataService.Contracts/*.csproj SportsDataService.Contracts/
COPY Services/SportsDataService/SportsDataService.Tests/*.csproj SportsDataService.Tests/



RUN dotnet restore

COPY . .

RUN dotnet publish Services/SportsDataService/SportsDataService.API/SportsDataService.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out ./

##EXPOSE 5001
ENTRYPOINT ["dotnet", "SportsDataService.API.dll"]
