# Base image with runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only project file first to improve cache
COPY ["Services/SimulationAPI/SimulationAPI.csproj", "Services/SimulationAPI/"]
RUN dotnet restore "Services/SimulationAPI/SimulationAPI.csproj"

# Copy the rest of the source
COPY . .

# Set working directory and build
WORKDIR "/src/Services/SimulationAPI"
RUN dotnet build "SimulationAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimulationAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Start the app
ENTRYPOINT ["dotnet", "SimulationAPI.dll"]
