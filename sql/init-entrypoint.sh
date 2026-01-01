#!/bin/bash

for sql_file in /seed/*.sql; do
  if [ -f "$sql_file" ]; then
    echo "  **Running init file: $sql_file**"
    /opt/mssql-tools/bin/sqlcmd \
      -S "mssql,1433" \
      -U "${DB_ADMIN}" \
      -P "$SA_PASSWORD" \
      -d SportsDataDb \
      -i "$sql_file"
  fi
done