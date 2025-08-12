#!/bin/bash

set -e
BRANCH="${1:-main}"

services=(
  "src/Services/LoggingService/src/SimPitchProtos"
  "src/Services/SportsDataService/src/SimPitchProtos"
  "src/Services/SimulationService/src/SimPitchProtos"
)

cd ..
for proto_path in "${services[@]}"; do
  if [ -d "$proto_path" ]; then
    echo "Updating submodule in $proto_path (branch: $BRANCH)"
    cd "$proto_path"

    git fetch origin

    git checkout "$BRANCH"

    git pull origin "$BRANCH"

    cd - > /dev/null
  else
    echo "WARNING: Path $proto_path does not exist."
  fi
done

echo "All proto submodules updated to branch '$BRANCH'."
