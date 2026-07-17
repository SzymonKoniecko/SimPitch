#!/bin/bash
# scripts/build-project.sh - Buduje wszystkie Docker images z tagami

set -e

BUILD_NUMBER=${BUILD_NUMBER:-local}

echo "=== Building SimPitch Docker Images ==="
echo "Build Number: ${BUILD_NUMBER}"
echo "Current directory: $(pwd)"

# ============================================
# 1. Generowanie .env
# ============================================
echo ""
echo "--- Generating Environment Variables ---"

if [ -f "scripts/generate-env.sh" ]; then
  chmod +x scripts/generate-env.sh
  ./scripts/generate-env.sh --seed true
else
  echo "WARNING: scripts/generate-env.sh not found"
fi

# ============================================
# 2. Konfiguracja Docker BuildKit
# ============================================
echo ""
echo "--- Docker Configuration ---"

export DOCKER_BUILDKIT=0
export COMPOSE_DOCKER_CLI_BUILD=0

echo "✓ BuildKit disabled"

# ============================================
# 3. Lista obrazów do zbudowania
# ============================================

#!/bin/bash
set -e

echo "🐳 Building SimPitch images (APP)"

docker compose -f docker-compose.yml -p simpitch-tc build

echo "📦 Images built:"
docker images | grep simpitch