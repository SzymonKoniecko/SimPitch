#!/bin/bash

set -e

services=(
  "src/Services/LoggingService/src/SimPitchProtos"
  "src/Services/SportsDataService/src/SimPitchProtos"
  "src/Services/SimulationService/src/SimPitchProtos"
)
cd ..
for proto_path in "${services[@]}"; do
  if [ -d "$proto_path" ]; then
    echo "Updating submodule in $proto_path"
    cd "$proto_path"
    git pull origin main
    cd - > /dev/null
  else
    echo "WARNING: Path $proto_path does not exist."
  fi
done

echo "All proto submodules updated."
