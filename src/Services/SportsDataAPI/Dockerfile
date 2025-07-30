# Base image with runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only project file first to improve cache
COPY ["Services/SportsDataAPI/SportsDataAPI.csproj", "Services/SportsDataAPI/"]
RUN dotnet restore "Services/SportsDataAPI/SportsDataAPI.csproj"

# Copy the rest of the source
COPY . .

# Set working directory and build
WORKDIR "/src/Services/SportsDataAPI"
RUN dotnet build "SportsDataAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SportsDataAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Start the app
ENTRYPOINT ["dotnet", "SportsDataAPI.dll"]
