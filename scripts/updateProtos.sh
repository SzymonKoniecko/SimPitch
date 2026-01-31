#!/bin/bash

set -euo pipefail

error_handler() {
    local exit_code=$?
    local line_no=$1
    echo "❌ ERROR: Script failed at line $line_no with exit code $exit_code"
    echo "   ➤ Command: ${BASH_COMMAND}"
    exit $exit_code
}
trap 'error_handler $LINENO' ERR
print() {
    echo -e "$1"
}

BRANCH="${1:-main}"

services=(
  "LoggingService|src/Services/LoggingService/src/SimPitchProtos"
  "SportsDataService|src/Services/SportsDataService/src/SimPitchProtos"
  "SimulationService|src/Services/SimulationService/src/SimPitchProtos"
  "StatisticsService|src/Services/StatisticsService/src/SimPitchProtos"
  "EngineService|src/Services/EngineService/src/SimPitchProtos"
  "SimPitchML|src/ML/proto"
)

# Safe cd
cd .. || { echo "❌ Cannot cd to project root"; exit 1; }

for entry in "${services[@]}"; do
  service="${entry%%|*}"
  proto_path="${entry##*|}"

  if [ -d "$proto_path" ]; then
    print "----------------------------------------------------------------------------------------"
    print "▶ Updating << $service >> (branch: $BRANCH)"
    print "----------------------------------------------------------------------------------------"

    cd "$proto_path"

    git fetch origin
    git checkout "$BRANCH"
    git pull origin "$BRANCH"

    print "✔ $service updated successfully\n"

    cd - > /dev/null
  else
    print "----------------------------------------------------------------------------------------"
    print "⚠ WARNING: Path $proto_path for $service does not exist."
    print "----------------------------------------------------------------------------------------\n"
  fi
done

print "========================================================================================"
print "✅ All proto submodules updated to branch '$BRANCH'."
print "========================================================================================"
