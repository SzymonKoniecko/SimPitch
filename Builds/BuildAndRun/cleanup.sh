#!/bin/bash
# scripts/cleanup.sh - Czyści wszystkie SimPitch buildy i pliki

set -e

# ============================================
# 2. Usuń pliki .env
# ============================================
echo ""
echo "--- Removing .env files ---"

if [ -f ".env" ]; then
  echo "Removing .env"
  rm -f .env
  echo "✓ .env removed"
fi