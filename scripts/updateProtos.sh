#!/bin/bash

set -e
BRANCH="${1:-main}"

services=(
  "LoggingService|src/Services/LoggingService/src/SimPitchProtos"
  "SportsDataService|src/Services/SportsDataService/src/SimPitchProtos"
  "SimulationService|src/Services/SimulationService/src/SimPitchProtos"
  "StatisticsService|src/Services/StatisticsService/src/SimPitchProtos"
  "EngineService|src/Services/EngineService/src/SimPitchProtos"
)

cd ..
for entry in "${services[@]}"; do
  service="${entry%%|*}"
  proto_path="${entry##*|}"

  if [ -d "$proto_path" ]; then
    echo "----------------------------------------------------------------------------------------"
    echo "▶ Updating << $service >> (branch: $BRANCH)"
    echo "----------------------------------------------------------------------------------------"
    cd "$proto_path"

    git fetch origin > /dev/null 2>&1
    git checkout "$BRANCH" > /dev/null 2>&1
    git pull origin "$BRANCH"

    echo "✔ $service updated successfully"
    echo

    cd - > /dev/null
  else
    echo "----------------------------------------------------------------------------------------"
    echo "⚠ WARNING: Path $proto_path for $service does not exist."
    echo "----------------------------------------------------------------------------------------"
    echo
  fi
done

echo "========================================================================================"
echo "✅ All proto submodules updated to branch '$BRANCH'."
echo "========================================================================================"
