#!/bin/bash

set -e
BRANCH="${1:-main}"

# Lista: NAZWA|ŚCIEŻKA
services=(
  "LoggingService|src/Services/LoggingService/src/SimPitchProtos"
  "SportsDataService|src/Services/SportsDataService/src/SimPitchProtos"
  "SimulationService|src/Services/SimulationService/src/SimPitchProtos"
  "StatisticsService|src/Services/StatisticsService/src/SimPitchProtos"
)

cd ..
for entry in "${services[@]}"; do
  service="${entry%%|*}"   # część przed |
  proto_path="${entry##*|}" # część po |

  if [ -d "$proto_path" ]; then
    echo "--------------------------------------------"
    echo "▶ Updating $service (branch: $BRANCH)"
    echo "--------------------------------------------"
    cd "$proto_path"

    git fetch origin > /dev/null 2>&1
    git checkout "$BRANCH" > /dev/null 2>&1
    git pull origin "$BRANCH"

    echo "✔ $service updated successfully"
    echo

    cd - > /dev/null
  else
    echo "--------------------------------------------"
    echo "⚠ WARNING: Path $proto_path for $service does not exist."
    echo "--------------------------------------------"
    echo
  fi
done

echo "============================================"
echo "✅ All proto submodules updated to branch '$BRANCH'."
echo "============================================"
