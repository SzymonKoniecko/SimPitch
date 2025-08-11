#!/bin/bash

if [ "$SEED_DATA" = "true" ]; then
  echo 'Waiting for SQL Server...';
  sleep 10;
  echo "Seeding data into SportsDataDb..."
  /opt/mssql-tools/bin/sqlcmd \
    -S "mssql,1433" \
    -U "${DB_ADMIN}" \
    -P "$SA_PASSWORD" \
    -d SportsDataDb \
    -i /seed-data-SportsDataDb.sql
  echo "Seeding completed."
else
  echo "SEED_DATA=false — skipping seed process."
fi
