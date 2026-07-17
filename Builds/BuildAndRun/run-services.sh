#!/bin/bash
set -e

echo "🚀 Running APP stack"


docker compose \
  -f docker-compose.app.yml \
  -p simpitch-tc \
  up -d


echo "⏳ Waiting for containers"
sleep 20

docker ps