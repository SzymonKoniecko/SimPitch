#!/bin/bash

# chmod +x scripts/generate-env.sh 
# scripts/./generate-env.sh --seed false

set -e

ENV_FILE=".env"

GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m'

SEED_DATA_VALUE="true"

usage() {
  echo "Usage: $0 [--seed true|false]"
  exit 1
}

# Parse args: --seed true|false
while [[ $# -gt 0 ]]; do
  case "$1" in
    --seed)
      shift
      [[ -z "${1:-}" ]] && usage
      SEED_DATA_VALUE="$1"
      shift
      ;;
    -h|--help)
      usage
      ;;
    *)
      echo "Unknown argument: $1"
      usage
      ;;
  esac
done

if [[ "$SEED_DATA_VALUE" != "true" && "$SEED_DATA_VALUE" != "false" ]]; then
  echo -e "${YELLOW}Error: --seed must be 'true' or 'false'${NC}"
  exit 1
fi

echo -e "${BLUE}=== SimPitch - .env Generator ===${NC}\n"

# Check if .env already exists
if [ -f "$ENV_FILE" ]; then
  echo -e "${YELLOW}Warning: $ENV_FILE already exists${NC}"
  read -p "Do you want to overwrite it? (y/N) " -n 1 -r
  echo
  if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo -e "${YELLOW}Aborted. Using existing $ENV_FILE${NC}"
    exit 0
  fi
fi

cat > "$ENV_FILE" <<EOF
################################################################################
# SimPitch - Environment Variables
# Generated: $(date)
# Purpose: Docker Compose configuration for microservices architecture
################################################################################

# ============================================================================
# GENERAL SETTINGS
# ============================================================================

# Seed data initialization (set to true to auto-populate database)
SEED_DATA=$SEED_DATA_VALUE

# ASP.NET Core settings
ASPNETCORE_URL=http://0.0.0.0
ASPNETCORE_ENVIRONMENT=Development

# ============================================================================
# SPORTSDATA SERVICE CONFIGURATION
# Host Port → Container Port mapping for gRPC and REST
# ============================================================================

# gRPC Communication (Port 81 in container)
SPORTSDATA_SERVICE_HOST_PORT_GRPC=40011
SPORTSDATA_SERVICE_CONTAINER_PORT_GRPC=81

# REST API (Port 80 in container)
SPORTSDATA_SERVICE_HOST_PORT_REST=4001
SPORTSDATA_SERVICE_CONTAINER_PORT_REST=80

# ============================================================================
# LOGGING SERVICE CONFIGURATION
# gRPC-based centralized logging service
# ============================================================================

LOGGING_SERVICE_HOST_PORT_GRPC=40022
LOGGING_SERVICE_CONTAINER_PORT_GRPC=81
LOGGING_SERVICE_NAME="logging-service"

# ============================================================================
# SIMULATION SERVICE CONFIGURATION
# gRPC service for match simulation engine
# ============================================================================

SIMULATION_SERVICE_HOST_PORT_GRPC=40033
SIMULATION_SERVICE_CONTAINER_PORT_GRPC=81
SIMULATION_SERVICE_NAME="simulation-service"

# ============================================================================
# STATISTICS SERVICE CONFIGURATION
# gRPC service for statistical analysis and predictions
# ============================================================================

STATISTICS_SERVICE_HOST_PORT_GRPC=40044
STATISTICS_SERVICE_CONTAINER_PORT_GRPC=81
STATISTICS_SERVICE_NAME="statistics-service"

# ============================================================================
# ENGINE SERVICE CONFIGURATION
# REST API - Main orchestrator service
# ============================================================================

ENGINE_SERVICE_HOST_PORT_REST=4005
ENGINE_SERVICE_CONTAINER_PORT_REST=80
ENGINE_SERVICE_NAME="engine-service"

# ============================================================================
# SPORTSDATA SERVICE NAME
# ============================================================================

SPORTSDATA_SERVICE_NAME="sportsdata-service"

# ============================================================================
# REDIS CACHE CONFIGURATION
# Used for caching team data, match results (TTL: 24h)
# ============================================================================

REDIS_PORT=6379
REDIS_PASS=MySuperSecretPassword
REDIS_CONN_STRING="redis-cache:6379,password=MySuperSecretPassword,abortConnect=false"
REDIS_SERVICE_NAME="redis-cache"

# ============================================================================
# DATABASE CONFIGURATION
# MSSQL Server credentials for 5 databases:
# - EngineDb
# - SimulationDb
# - StatisticsDb
# - SportsDataDb
# - LoggingDb
# ============================================================================

DB_ADMIN=sa
DB_PASSWORD=Zaq1@wsx

# ============================================================================
# FRONTEND CONFIGURATION (Vue 3 + Vite)
# Development server settings
# ============================================================================

NODE_ENV=development
VITE_API_TARGET=http://gateway:80
DOCKER_ENV=true
HMR_HOST=localhost
WEB_DEV_PORT=5173

# ============================================================================
# NGINX GATEWAY CONFIGURATION
# API Gateway for routing requests to microservices
# ============================================================================

GATEWAY_HOST_PORT=8080
GATEWAY_SERVICE_CONTAINER_PORT=80

# ============================================================================
# INTERNAL SERVICE COMMUNICATION
# Service discovery names (used internally in Docker network)
# ============================================================================

# Already defined above:
# LOGGING_SERVICE_NAME="logging-service"
# SPORTSDATA_SERVICE_NAME="sportsdata-service"
# SIMULATION_SERVICE_NAME="simulation-service"
# STATISTICS_SERVICE_NAME="statistics-service"
# ENGINE_SERVICE_NAME="engine-service"
# REDIS_SERVICE_NAME="redis-cache"
EOF

echo -e "${GREEN}✓ Successfully created $ENV_FILE${NC}\n"

echo -e "${BLUE}Configuration Summary:${NC}"
echo -e "  ${GREEN}✓${NC} SEED_DATA: $SEED_DATA_VALUE"

echo " * * * * "
echo -e "${BLUE}Port Mapping:${NC}"
echo -e "  Frontend:           ${GREEN}http://localhost:5173${NC}"
echo -e "  NGINX Gateway:      ${GREEN}http://localhost:8080${NC}"
echo -e "  Engine Service:     ${GREEN}http://localhost:4005${NC}"
echo -e "  SportsData REST:    ${GREEN}http://localhost:4001${NC}"
echo -e "  SportsData gRPC:    ${GREEN}grpc://localhost:40011${NC}"
echo -e "  Logging gRPC:       ${GREEN}grpc://localhost:40022${NC}"
echo -e "  Simulation gRPC:    ${GREEN}grpc://localhost:40033${NC}"
echo -e "  Statistics gRPC:    ${GREEN}grpc://localhost:40044${NC}"
echo -e "  MSSQL Database:     ${GREEN}localhost:1433${NC}"
echo -e "  Redis Cache:        ${GREEN}localhost:6379${NC}"
echo ""

echo -e "${GREEN}Setup complete!${NC}\n"
