#!/bin/bash

echo "Waiting for SQL Server..."
for i in {1..60}; do
  /opt/mssql-tools/bin/sqlcmd -S mssql -U sa -P "$SA_PASSWORD" -d master -Q "SELECT 1" && break
  sleep 5
done

for sql_file in /seed/*.sql; do
  if [ -f "$sql_file" ]; then
    echo "  **Running init file: $sql_file**"
    /opt/mssql-tools/bin/sqlcmd \
      -S "mssql,1433" \
      -U "${DB_ADMIN}" \
      -P "$SA_PASSWORD" \
      -d master \
      -i "$sql_file"
  fi
done