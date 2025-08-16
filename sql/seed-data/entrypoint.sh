#!/bin/bash

if [ "$SEED_DATA" = "true" ]; then
  echo 'Waiting for SQL Server...'
  sleep 10
  echo "Seeding data into SportsDataDb..."

  for sql_file in /seed/*.sql; do
    if [ -f "$sql_file" ]; then
      echo "  **Running seed file: $sql_file**"
      /opt/mssql-tools/bin/sqlcmd \
        -S "mssql,1433" \
        -U "${DB_ADMIN}" \
        -P "$SA_PASSWORD" \
        -d SportsDataDb \
        -i "$sql_file"
    fi
  done

  echo "Seeding completed."
else
  echo "SEED_DATA=false — skipping seed process."
fi
