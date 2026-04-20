#!/bin/bash
# ==============================================================================
# Create all ERP databases on SQL Server running in Docker
# Usage: bash scripts/create-databases.sh
# ==============================================================================

set -e

# Prevent Git Bash (MSYS) from converting /opt/... paths to C:/Program Files/Git/opt/...
export MSYS_NO_PATHCONV=1

CONTAINER_NAME="${SQL_CONTAINER:-sqlserver}"
SA_PASSWORD="${SA_PASSWORD:-YourStrong@Passw0rd}"
SQLCMD="/opt/mssql-tools18/bin/sqlcmd"

echo "⏳ Waiting for SQL Server ($CONTAINER_NAME) to be ready..."
for i in $(seq 1 30); do
  if docker exec "$CONTAINER_NAME" "$SQLCMD" -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" &>/dev/null; then
    echo "✅ SQL Server is ready."
    break
  fi
  if [ "$i" -eq 30 ]; then
    echo "❌ SQL Server did not become ready in time."
    exit 1
  fi
  sleep 2
done

echo "🗄️  Creating all 81 databases (idempotent — skips existing)..."

docker exec "$CONTAINER_NAME" "$SQLCMD" -S localhost -U sa -P "$SA_PASSWORD" -C -Q "
-- AuthProvider
IF DB_ID('AuthProviderDB') IS NULL CREATE DATABASE AuthProviderDB;

-- adminServices
IF DB_ID('ADMINDB') IS NULL CREATE DATABASE ADMINDB;
IF DB_ID('LOCATIONDB') IS NULL CREATE DATABASE LOCATIONDB;
IF DB_ID('VENDORDB') IS NULL CREATE DATABASE VENDORDB;
IF DB_ID('LOVDB') IS NULL CREATE DATABASE LOVDB;
IF DB_ID('STATIONERYDB') IS NULL CREATE DATABASE STATIONERYDB;
IF DB_ID('TDSDB') IS NULL CREATE DATABASE TDSDB;

-- aimsServices
IF DB_ID('ACCESSDB') IS NULL CREATE DATABASE ACCESSDB;
IF DB_ID('ATTENDANCEDB') IS NULL CREATE DATABASE ATTENDANCEDB;
IF DB_ID('BUSDB') IS NULL CREATE DATABASE BUSDB;
IF DB_ID('CALENDARDB') IS NULL CREATE DATABASE CALENDARDB;
IF DB_ID('EMPLOYEEDB') IS NULL CREATE DATABASE EMPLOYEEDB;
IF DB_ID('GROUPINCENTIVEDB') IS NULL CREATE DATABASE GROUPINCENTIVEDB;
IF DB_ID('LEAVEDB') IS NULL CREATE DATABASE LEAVEDB;
IF DB_ID('REFERENCEDB') IS NULL CREATE DATABASE REFERENCEDB;
IF DB_ID('VISITORDB') IS NULL CREATE DATABASE VISITORDB;
IF DB_ID('AIMSDB') IS NULL CREATE DATABASE AIMSDB;

-- auditServices
IF DB_ID('ERPActionDB') IS NULL CREATE DATABASE ERPActionDB;
IF DB_ID('ERPAuditDB') IS NULL CREATE DATABASE ERPAuditDB;
IF DB_ID('ERPCertificateDB') IS NULL CREATE DATABASE ERPCertificateDB;
IF DB_ID('ERPContractDB') IS NULL CREATE DATABASE ERPContractDB;
IF DB_ID('ERPFinanceDB') IS NULL CREATE DATABASE ERPFinanceDB;
IF DB_ID('ERPFindingsDB') IS NULL CREATE DATABASE ERPFindingsDB;
IF DB_ID('ERPNotificationDB') IS NULL CREATE DATABASE ERPNotificationDB;
IF DB_ID('ERPScheduleDB') IS NULL CREATE DATABASE ERPScheduleDB;
IF DB_ID('ERPSettingsDB') IS NULL CREATE DATABASE ERPSettingsDB;

-- canteenServices
IF DB_ID('CanteenUnitDb') IS NULL CREATE DATABASE CanteenUnitDb;
IF DB_ID('CardManagementDb') IS NULL CREATE DATABASE CardManagementDb;
IF DB_ID('DeductionServiceDb') IS NULL CREATE DATABASE DeductionServiceDb;
IF DB_ID('EligibilityServiceDb') IS NULL CREATE DATABASE EligibilityServiceDb;
IF DB_ID('ItemMasterDb') IS NULL CREATE DATABASE ItemMasterDb;
IF DB_ID('ReferenceDataDb') IS NULL CREATE DATABASE ReferenceDataDb;
IF DB_ID('SwipeTransactionDb') IS NULL CREATE DATABASE SwipeTransactionDb;
IF DB_ID('CanteenTransactionDb') IS NULL CREATE DATABASE CanteenTransactionDb;

-- cashServices
IF DB_ID('CASHDB') IS NULL CREATE DATABASE CASHDB;
IF DB_ID('EmailNotificationDb') IS NULL CREATE DATABASE EmailNotificationDb;
IF DB_ID('TransactionProcessingDb') IS NULL CREATE DATABASE TransactionProcessingDb;

-- ddServices
IF DB_ID('AppraisalDb') IS NULL CREATE DATABASE AppraisalDb;
IF DB_ID('AuthorizationServiceDb') IS NULL CREATE DATABASE AuthorizationServiceDb;
IF DB_ID('CompensationDb') IS NULL CREATE DATABASE CompensationDb;
IF DB_ID('DDDB') IS NULL CREATE DATABASE DDDB;
IF DB_ID('EmployeeServiceDB') IS NULL CREATE DATABASE EmployeeServiceDB;
IF DB_ID('TodosDB') IS NULL CREATE DATABASE TodosDB;
IF DB_ID('RecruitmentDb') IS NULL CREATE DATABASE RecruitmentDb;
IF DB_ID('ReportingServiceDb') IS NULL CREATE DATABASE ReportingServiceDb;
IF DB_ID('TransactionServiceDb') IS NULL CREATE DATABASE TransactionServiceDb;

-- healthServices
IF DB_ID('HEALTHDB') IS NULL CREATE DATABASE HEALTHDB;
IF DB_ID('HEALTHDB_MedicineManagement') IS NULL CREATE DATABASE HEALTHDB_MedicineManagement;
IF DB_ID('HEALTHDB_HealthTransactions') IS NULL CREATE DATABASE HEALTHDB_HealthTransactions;

-- hrServicess
IF DB_ID('AlertsNotificationsDB') IS NULL CREATE DATABASE AlertsNotificationsDB;
IF DB_ID('CompensationBenefitsDB') IS NULL CREATE DATABASE CompensationBenefitsDB;
IF DB_ID('EmployeeManagementDB') IS NULL CREATE DATABASE EmployeeManagementDB;
IF DB_ID('EmployeeRelationsDB') IS NULL CREATE DATABASE EmployeeRelationsDB;
IF DB_ID('ExitManagementDB') IS NULL CREATE DATABASE ExitManagementDB;
IF DB_ID('OrganizationStructureDB') IS NULL CREATE DATABASE OrganizationStructureDB;
IF DB_ID('RecruitmentDB') IS NULL CREATE DATABASE RecruitmentDB;
IF DB_ID('TimeAttendanceDB') IS NULL CREATE DATABASE TimeAttendanceDB;
IF DB_ID('TrainingDevelopmentDB') IS NULL CREATE DATABASE TrainingDevelopmentDB;
IF DB_ID('UserSecurityDB') IS NULL CREATE DATABASE UserSecurityDB;
IF DB_ID('EmployeeTransactionsDB') IS NULL CREATE DATABASE EmployeeTransactionsDB;

-- letServices
IF DB_ID('LETDB') IS NULL CREATE DATABASE LETDB;

-- loanServices
IF DB_ID('LOANDB') IS NULL CREATE DATABASE LOANDB;

-- myworkServices
IF DB_ID('MYWORKDB') IS NULL CREATE DATABASE MYWORKDB;

-- payServices
IF DB_ID('PAYDB') IS NULL CREATE DATABASE PAYDB;
IF DB_ID('TaxService') IS NULL CREATE DATABASE TaxService;
IF DB_ID('PayTransactionalService') IS NULL CREATE DATABASE PayTransactionalService;

-- pfServices
IF DB_ID('PFDB') IS NULL CREATE DATABASE PFDB;

-- sciServices
IF DB_ID('SCIDB') IS NULL CREATE DATABASE SCIDB;

-- sparshServices
IF DB_ID('SPARSHDB') IS NULL CREATE DATABASE SPARSHDB;
IF DB_ID('ProblemManagementDb') IS NULL CREATE DATABASE ProblemManagementDb;
IF DB_ID('SparshTransactionalDb') IS NULL CREATE DATABASE SparshTransactionalDb;

-- sscServices
IF DB_ID('SSCDB') IS NULL CREATE DATABASE SSCDB;
IF DB_ID('SSCDB_CategoryVendor') IS NULL CREATE DATABASE SSCDB_CategoryVendor;
IF DB_ID('SSCDB_InvoiceProcessing') IS NULL CREATE DATABASE SSCDB_InvoiceProcessing;

-- taskServices
IF DB_ID('TASKDB') IS NULL CREATE DATABASE TASKDB;

-- tourServices
IF DB_ID('TOURDB') IS NULL CREATE DATABASE TOURDB;

-- travelServices
IF DB_ID('TRAVELDB') IS NULL CREATE DATABASE TRAVELDB;
IF DB_ID('FinanceServiceDb') IS NULL CREATE DATABASE FinanceServiceDb;
IF DB_ID('MasterDataDB') IS NULL CREATE DATABASE MasterDataDB;
IF DB_ID('AgencyServiceDb') IS NULL CREATE DATABASE AgencyServiceDb;
IF DB_ID('AdminServiceDb') IS NULL CREATE DATABASE AdminServiceDb;

-- wmsServices
IF DB_ID('WMSDB') IS NULL CREATE DATABASE WMSDB;
"

echo ""
echo "📋 Verifying — listing all user databases:"
docker exec "$CONTAINER_NAME" "$SQLCMD" -S localhost -U sa -P "$SA_PASSWORD" -C \
  -Q "SELECT name FROM sys.databases WHERE database_id > 4 ORDER BY name;"

echo ""
echo "✅ Done. All databases are ready."
