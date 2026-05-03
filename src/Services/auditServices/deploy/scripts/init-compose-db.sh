#!/usr/bin/env bash
set -euo pipefail

SCRIPT_ROOT="/workspace"
INIT_SQL="$SCRIPT_ROOT/deploy/database/init.sql"
SA_PASSWORD="${SQL_SA_PASSWORD:-ErpStr0ng!Pass}"
SQL_HOST="${SQL_HOST:-sqlserver}"

if [ -x /opt/mssql-tools/bin/sqlcmd ]; then
  SQLCMD_BIN="/opt/mssql-tools/bin/sqlcmd"
elif [ -x /opt/mssql-tools18/bin/sqlcmd ]; then
  SQLCMD_BIN="/opt/mssql-tools18/bin/sqlcmd"
else
  echo "sqlcmd was not found in the db-init container" >&2
  exit 1
fi

run_sql_file() {
  local database="$1"
  local file_path="$2"
  echo "Applying $(basename "$file_path") to $database"
  "$SQLCMD_BIN" -S "$SQL_HOST" -U sa -P "$SA_PASSWORD" -d "$database" -i "$file_path" -C -b
}

run_sql_query() {
  local database="$1"
  local query="$2"
  "$SQLCMD_BIN" -S "$SQL_HOST" -U sa -P "$SA_PASSWORD" -d "$database" -Q "$query" -C -b
}

find_sql_dir() {
  local service_root="$1"
  shift

  local candidate
  for candidate in "$@"; do
    if [ -d "$service_root/$candidate" ]; then
      echo "$service_root/$candidate"
      return 0
    fi
  done

  return 1
}

apply_sql_tree() {
  local database="$1"
  local directory="$2"
  local label="$3"
  local fail_on_unresolved="${4:-1}"

  if [ -z "$directory" ] || [ ! -d "$directory" ]; then
    echo "Skipping $label for $database because the folder was not found"
    return
  fi

  local pending_files=()
  local next_pending=()
  local file_path
  local pass=1
  local made_progress=0

  while IFS= read -r file_path; do
    case "$(basename "$file_path")" in
      parameter.sql)
        echo "Skipping non-deployment script $(basename "$file_path") in $directory"
        continue
        ;;
    esac

    pending_files+=("$file_path")
  done < <(find "$directory" -type f -name '*.sql' | sort)

  while [ ${#pending_files[@]} -gt 0 ]; do
    echo "Applying $label for $database (pass $pass, pending ${#pending_files[@]})"
    next_pending=()
    made_progress=0

    for file_path in "${pending_files[@]}"; do
      if run_sql_file "$database" "$file_path"; then
        made_progress=1
      else
        next_pending+=("$file_path")
      fi
    done

    if [ ${#next_pending[@]} -eq 0 ]; then
      return 0
    fi

    if [ $made_progress -eq 0 ]; then
      echo "Unable to resolve remaining $label scripts for $database:" >&2
      printf '  %s\n' "${next_pending[@]}" >&2
      if [ "$fail_on_unresolved" = "1" ]; then
        return 1
      fi

      echo "Continuing despite unresolved $label scripts for $database" >&2
      return 0
    fi

    pending_files=("${next_pending[@]}")
    pass=$((pass + 1))
  done
}

recreate_database() {
  local database="$1"
  echo "Recreating database $database"
  run_sql_query master "IF DB_ID(N'$database') IS NOT NULL BEGIN ALTER DATABASE [$database] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$database]; END; CREATE DATABASE [$database];"
}

apply_service_schema() {
  local database="$1"
  local service_root="$2"

  recreate_database "$database"

  local tables_dir=""
  local stored_procedures_dir=""
  local insert_scripts_dir=""

  tables_dir="$(find_sql_dir "$service_root" "tables" "Tables" "Database/tables" "Database/Tables" || true)"
  stored_procedures_dir="$(find_sql_dir "$service_root" "Stored-procedure" "Stored-Procedures" "Database/Stored-procedure" "Database/Stored-Procedures" || true)"
  insert_scripts_dir="$(find_sql_dir "$service_root" "insert-scripts" "Insert-scripts" "Insert-Scripts" "Database/insert-scripts" "Database/Insert-scripts" "Database/Insert-Scripts" || true)"

  apply_sql_tree "$database" "$tables_dir" "tables" 1
  apply_sql_tree "$database" "$stored_procedures_dir" "stored procedures" 0
  apply_sql_tree "$database" "$insert_scripts_dir" "insert scripts" 0
}

if [ -f "$INIT_SQL" ]; then
  echo "Applying shared bootstrap init.sql"
  if ! run_sql_file master "$INIT_SQL"; then
    echo "Shared bootstrap init.sql failed; continuing with service-specific database rebuilds" >&2
  fi
fi

apply_service_schema "ERPActionDB" "$SCRIPT_ROOT/actionapiServices"
apply_service_schema "ERPAuditDB" "$SCRIPT_ROOT/auditapiServices"
apply_service_schema "ERPCertificateDB" "$SCRIPT_ROOT/certificateapiServices"
apply_service_schema "ERPContractDB" "$SCRIPT_ROOT/contractapiServices"
apply_service_schema "ERPFinanceDB" "$SCRIPT_ROOT/financeapiServices"
apply_service_schema "ERPFindingsDB" "$SCRIPT_ROOT/findingsapiServices"
apply_service_schema "ERPNotificationDB" "$SCRIPT_ROOT/notificationapiServices"
apply_service_schema "ERPScheduleDB" "$SCRIPT_ROOT/scheduleapiServices"
apply_service_schema "ERPSettingsDB" "$SCRIPT_ROOT/settingsapiServices"

echo "Database bootstrap completed successfully."