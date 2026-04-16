"""
Generate the root-level azure-pipelines.yml with every service from all 22 modules.
Reads the exact same MODULES registry as generate-module-pipelines.py.
"""

import pathlib, textwrap

REPO = pathlib.Path(r"E:\ERPMicroservice")
IMAGE_PREFIX = "ghcr.io/vimalshan/erp"

# ── Complete registry (copied from generate-module-pipelines.py) ──────────────
MODULES = {
    "adminServices": {
        "path": "src/Services/adminServices",
        "services": [
            ("api-gateway",          "src/Services/adminServices/ApiGateway",           "Dockerfile"),
            ("finyear-api",          "src/Services/adminServices/finyearServices",       "Docker/Dockerfile"),
            ("location-services",    "src/Services/adminServices/locationServices",      "Docker/Dockerfile"),
            ("lov-service",          "src/Services/adminServices/lovServices",           "Docker/Dockerfile"),
            ("scholarship-service",  "src/Services/adminServices/scholarshipServices",   "Docker/Dockerfile"),
            ("stationery-service",   "src/Services/adminServices/stationeryServices",    "Docker/Dockerfile"),
            ("tds-service",          "src/Services/adminServices/tdsServices",           "Docker/Dockerfile"),
            ("vendor-service",       "src/Services/adminServices/vendorServices",        "Docker/Dockerfile"),
            ("transaction-service",  "src/Services/adminServices/transactionServices",   "Docker/Dockerfile"),
        ],
    },
    "aimsServices": {
        "path": "src/Services/aimsServices",
        "services": [
            ("api-gateway",               "src/Services/aimsServices/ApiGateway",              "Dockerfile"),
            ("access-service",            "src/Services/aimsServices/accessServices",           "Dockerfile"),
            ("aims-transaction-service",  "src/Services/aimsServices/aimsTransactionServices",  "Dockerfile"),
            ("attendance-service",        "src/Services/aimsServices/attendanceServices",        "Dockerfile"),
            ("bus-services",              "src/Services/aimsServices/busServices",               "Dockerfile"),
            ("calendar-service",          "src/Services/aimsServices/calendarServices",          "Dockerfile"),
            ("employee-service",          "src/Services/aimsServices/employeeServices",          "Dockerfile"),
            ("groupincentive-service",    "src/Services/aimsServices/groupincentiveServices",    "Dockerfile"),
            ("leave-services",            "src/Services/aimsServices/leaveServices",             "Dockerfile"),
            ("reference-service",         "src/Services/aimsServices/referenceServices",         "Dockerfile"),
            ("visitor-services",          "src/Services/aimsServices/visitorServices",           "Dockerfile"),
        ],
    },
    "auditServices": {
        "path": "src/Services/auditServices",
        "services": [
            ("api-gateway",           "src/Services/auditServices/apigateway",              "Dockerfile"),
            ("action-service",        "src/Services/auditServices/actionapiServices",        "Dockerfile"),
            ("audit-service",         "src/Services/auditServices/auditapiServices",         "Dockerfile"),
            ("certificate-service",   "src/Services/auditServices/certificateapiServices",   "Dockerfile"),
            ("contract-service",      "src/Services/auditServices/contractapiServices",      "Dockerfile"),
            ("finance-service",       "src/Services/auditServices/financeapiServices",       "Dockerfile"),
            ("findings-service",      "src/Services/auditServices/findingsapiServices",      "Dockerfile"),
            ("notification-service",  "src/Services/auditServices/notificationapiServices",  "Dockerfile"),
            ("schedule-service",      "src/Services/auditServices/scheduleapiServices",      "Dockerfile"),
            ("settings-service",      "src/Services/auditServices/settingsapiServices",      "Dockerfile"),
        ],
    },
    "AuthProvider": {
        "path": "src/Services/AuthProvider",
        "services": [
            ("auth-provider", "src/Services/AuthProvider", "Docker/Dockerfile"),
        ],
    },
    "canteenServices": {
        "path": "src/Services/canteenServices",
        "services": [
            ("api-gateway",                 "src/Services/canteenServices/ApiGateway",                 "Dockerfile"),
            ("canteen-transaction-service", "src/Services/canteenServices/canteenTransactionServices",  "Docker/Dockerfile"),
            ("canteen-unit-service",        "src/Services/canteenServices/canteenunitServices",         "Docker/Dockerfile"),
            ("card-management-service",     "src/Services/canteenServices/cardmanagementServices",      "Docker/Dockerfile"),
            ("deduction-service",           "src/Services/canteenServices/deductionServices",           "Docker/Dockerfile"),
            ("eligibility-service",         "src/Services/canteenServices/eligibilityServices",         "Docker/Dockerfile"),
            ("item-master-service",         "src/Services/canteenServices/itemmasterServices",          "Docker/Dockerfile"),
            ("reference-data-service",      "src/Services/canteenServices/referencedataServices",       "Docker/Dockerfile"),
            ("swipe-transaction-service",   "src/Services/canteenServices/swipeTransactionServices",    "Docker/Dockerfile"),
        ],
    },
    "cashServices": {
        "path": "src/Services/cashServices",
        "services": [
            ("api-gateway",                 "src/Services/cashServices/ApiGateway",                  "Docker/Dockerfile"),
            ("cash-management-service",     "src/Services/cashServices/cashmanagementServices",      "Docker/Dockerfile"),
            ("current-management-service",  "src/Services/cashServices/currentmanagementServices",   "Docker/Dockerfile"),
            ("deal-ticketing-service",      "src/Services/cashServices/dealticketingServices",       "Docker/Dockerfile"),
            ("email-notification-service",  "src/Services/cashServices/emailnotificationServices",   "Docker/Dockerfile"),
            ("loan-management-service",     "src/Services/cashServices/loanmanagementServices",      "Docker/Dockerfile"),
            ("organization-setup-service",  "src/Services/cashServices/organizationsetupServices",   "Docker/Dockerfile"),
            ("transaction-service",         "src/Services/cashServices/transactionServices",         "Docker/Dockerfile"),
        ],
    },
    "ddServices": {
        "path": "src/Services/ddServices",
        "services": [
            ("api-gateway",               "src/Services/ddServices/apiGateway",               "Dockerfile"),
            ("appraisal-service",         "src/Services/ddServices/appraisalService",          "Dockerfile"),
            ("authorization-service",     "src/Services/ddServices/authorizationServices",     "Dockerfile"),
            ("compensation-service",      "src/Services/ddServices/compensationServices",      "Dockerfile"),
            ("competency-service",        "src/Services/ddServices/competencyServices",        "Dockerfile"),
            ("demand-management-service", "src/Services/ddServices/demandmanagementServices",  "Dockerfile"),
            ("document-service",          "src/Services/ddServices/documentServices",          "Dockerfile"),
            ("employee-service",          "src/Services/ddServices/employeeServices",          "Dockerfile"),
            ("feedback-service",          "src/Services/ddServices/feedbackServices",          "Dockerfile"),
            ("learning-service",          "src/Services/ddServices/learningServices",          "Dockerfile"),
            ("objective-service",         "src/Services/ddServices/objectiveServices",         "Dockerfile"),
            ("other-services",            "src/Services/ddServices/OtherServices",             "Dockerfile"),
            ("promotion-service",         "src/Services/ddServices/promotionServices",         "Dockerfile"),
            ("recruitment-service",       "src/Services/ddServices/recruitmentServices",       "Dockerfile"),
            ("reporting-service",         "src/Services/ddServices/reportingServices",         "Dockerfile"),
            ("transaction-service",       "src/Services/ddServices/transactionServices",       "Dockerfile"),
        ],
    },
    "healthServices": {
        "path": "src/Services/healthServices",
        "services": [
            ("api-gateway",                  "src/Services/healthServices/apiGateway/src",                     "Dockerfile"),
            ("accident-management-service",  "src/Services/healthServices/accidentmanagementServices/src",     "Dockerfile"),
            ("healthcheckup-service",        "src/Services/healthServices/healthcheckupServices/src",          "Dockerfile"),
            ("health-transaction-service",   "src/Services/healthServices/healthTransactionServices/src",      "Dockerfile"),
            ("insurance-management-service", "src/Services/healthServices/insurancemanagementServices/src",    "Dockerfile"),
            ("master-service",               "src/Services/healthServices/masterServices/src",                 "Dockerfile"),
            ("medicalvisit-service",         "src/Services/healthServices/medicalvisitServices/src",           "Dockerfile"),
            ("medicine-management-service",  "src/Services/healthServices/medicinemanagementServices/src",     "Dockerfile"),
        ],
    },
    "hrServicess": {
        "path": "src/Services/hrServicess",
        "services": [
            ("api-gateway",                    "src/Services/hrServicess", "deployment/Dockerfiles/ApiGateway.Dockerfile"),
            ("alerts-notifications-service",   "src/Services/hrServicess", "deployment/Dockerfiles/AlertsNotifications.Dockerfile"),
            ("compensation-benefits-service",  "src/Services/hrServicess", "deployment/Dockerfiles/CompensationBenefits.Dockerfile"),
            ("employee-management-service",    "src/Services/hrServicess", "deployment/Dockerfiles/EmployeeManagement.Dockerfile"),
            ("employee-relations-service",     "src/Services/hrServicess", "deployment/Dockerfiles/EmployeeRelations.Dockerfile"),
            ("employee-transactions-service",  "src/Services/hrServicess", "deployment/Dockerfiles/EmployeeTransactions.Dockerfile"),
            ("exit-management-service",        "src/Services/hrServicess", "deployment/Dockerfiles/ExitManagement.Dockerfile"),
            ("organization-structure-service", "src/Services/hrServicess", "deployment/Dockerfiles/OrganizationStructure.Dockerfile"),
            ("recruitment-service",            "src/Services/hrServicess", "deployment/Dockerfiles/Recruitment.Dockerfile"),
            ("time-attendance-service",        "src/Services/hrServicess", "deployment/Dockerfiles/TimeAttendance.Dockerfile"),
            ("training-development-service",   "src/Services/hrServicess", "deployment/Dockerfiles/TrainingDevelopment.Dockerfile"),
            ("user-security-service",          "src/Services/hrServicess", "deployment/Dockerfiles/UserSecurity.Dockerfile"),
        ],
    },
    "letServices": {
        "path": "src/Services/letServices",
        "services": [
            ("api-gateway",            "src/Services/letServices/apiGateway",             "Dockerfile"),
            ("course-service",         "src/Services/letServices/courseServices",          "Dockerfile"),
            ("development-service",    "src/Services/letServices/developmentServices",     "Dockerfile"),
            ("leave-service",          "src/Services/letServices/leaveServices",           "Dockerfile"),
            ("let-transaction-service","src/Services/letServices/letTransactionServices",  "Dockerfile"),
            ("master-service",         "src/Services/letServices/masterServices",          "Dockerfile"),
            ("request-service",        "src/Services/letServices/requestServices",         "Dockerfile"),
            ("review-service",         "src/Services/letServices/reviewServices",          "Dockerfile"),
        ],
    },
    "loanServices": {
        "path": "src/Services/loanServices",
        "services": [
            ("api-gateway",              "src/Services/loanServices/apiGateway",               "Dockerfile"),
            ("document-service",         "src/Services/loanServices/documentServices",         "Dockerfile"),
            ("loanaccount-service",      "src/Services/loanServices/loanaccountServices",      "Dockerfile"),
            ("loanapplication-service",  "src/Services/loanServices/loanapplicationServices",  "Dockerfile"),
            ("loandefinition-service",   "src/Services/loanServices/loandefinitionServices",   "Dockerfile"),
            ("loan-transaction-service", "src/Services/loanServices/loanTransactionServices",  "Dockerfile"),
            ("lov-service",              "src/Services/loanServices/lovServices",              "Dockerfile"),
            ("utility-service",          "src/Services/loanServices/utilityServices",          "Dockerfile"),
        ],
    },
    "mainsparshServices": {
        "path": "src/Services/mainsparshServices",
        "services": [
            ("api-gateway",             "src/Services/mainsparshServices/apiGateway",              "Dockerfile"),
            ("approval-service",        "src/Services/mainsparshServices/approvalServices",         "Dockerfile"),
            ("booking-service",         "src/Services/mainsparshServices/bookingServices",          "Dockerfile"),
            ("community-service",       "src/Services/mainsparshServices/communityServices",        "Dockerfile"),
            ("compensation-service",    "src/Services/mainsparshServices/compensationServices",     "Dockerfile"),
            ("groupmanagement-service", "src/Services/mainsparshServices/groupmanagementServices",  "Dockerfile"),
            ("location-service",        "src/Services/mainsparshServices/locationServices",         "Dockerfile"),
            ("meeting-service",         "src/Services/mainsparshServices/meetingServices",          "Dockerfile"),
            ("proxy-service",           "src/Services/mainsparshServices/proxyServices",            "Dockerfile"),
            ("reimbursement-service",   "src/Services/mainsparshServices/reimbursementServices",    "Dockerfile"),
            ("stipend-service",         "src/Services/mainsparshServices/stipendservices",          "Dockerfile"),
            ("timesheet-service",       "src/Services/mainsparshServices/timesheetServices",        "Dockerfile"),
            ("transaction-service",     "src/Services/mainsparshServices/transactionServices",      "Dockerfile"),
            ("usermanagement-service",  "src/Services/mainsparshServices/usermanagementServices",   "Dockerfile"),
            ("websitecontent-service",  "src/Services/mainsparshServices/websitecontentServices",   "Dockerfile"),
        ],
    },
    "myworkServices": {
        "path": "src/Services/myworkServices",
        "services": [
            ("api-gateway",       "src/Services/myworkServices/Gateway",                            "Dockerfile"),
            ("audit-service",     "src/Services/myworkServices/auditServices",                      "Dockerfile"),
            ("batch-service",     "src/Services/myworkServices/batchServices",                      "Dockerfile"),
            ("csa-service",       "src/Services/myworkServices/csaServices/CSA.Service",            "Dockerfile"),
            ("project-service",   "src/Services/myworkServices/projectServices",                    "Dockerfile"),
            ("risk-service",      "src/Services/myworkServices/riskServices",                       "Dockerfile"),
            ("team-service",      "src/Services/myworkServices/teamServices",                       "Dockerfile"),
            ("timesheet-service", "src/Services/myworkServices/timeSheetServices",                  "Dockerfile"),
            ("workorder-service", "src/Services/myworkServices/workorderServices/WorkOrderService", "Dockerfile"),
        ],
    },
    "payServices": {
        "path": "src/Services/payServices",
        "services": [
            ("api-gateway",            "src/Services/payServices/apiGateway",              "Dockerfile"),
            ("employee-service",       "src/Services/payServices/employeeServices",        "Dockerfile"),
            ("faq-service",            "src/Services/payServices/faqServices",             "Dockerfile"),
            ("hr-service",             "src/Services/payServices/hrServices",              "Dockerfile"),
            ("payroll-service",        "src/Services/payServices/payrollServices",         "Dockerfile"),
            ("pay-transaction-service","src/Services/payServices/payTransactionalServices","Dockerfile"),
            ("tax-service",            "src/Services/payServices/taxServices",             "Dockerfile"),
        ],
    },
    "pfServices": {
        "path": "src/Services/pfServices",
        "services": [
            ("api-gateway",           "src/Services/pfServices/apiGateway",                               "Dockerfile"),
            ("accounting-service",    "src/Services/pfServices/accountingServices",                       "Dockerfile"),
            ("bank-service",          "src/Services/pfServices/bankServices/BankService",                 "Dockerfile"),
            ("contribution-service",  "src/Services/pfServices/contributionServices/ContributionService", "Dockerfile"),
            ("investment-service",    "src/Services/pfServices/investmentServices/InvestmentService",     "Dockerfile"),
            ("loan-service",          "src/Services/pfServices/loanServices",                             "Dockerfile"),
            ("masterdata-service",    "src/Services/pfServices/masterdataServices/MasterDataService",     "Dockerfile"),
            ("member-service",        "src/Services/pfServices/memberServices",                           "Dockerfile"),
            ("pf-transaction-service","src/Services/pfServices/pftransactionalServices",                  "Dockerfile"),
            ("settlement-service",    "src/Services/pfServices/settlementServices",                       "Dockerfile"),
            ("trust-service",         "src/Services/pfServices/trustServices/TrustService",               "Dockerfile"),
        ],
    },
    "sciServices": {
        "path": "src/Services/sciServices",
        "services": [
            ("api-gateway",                   "src/Services/sciServices/ApiGateway",                    "Dockerfile"),
            ("dispatchplanning-service",       "src/Services/sciServices/dispatchplanningServices",      "Dockerfile"),
            ("errorlogging-service",           "src/Services/sciServices/errorloggingServices",          "Dockerfile"),
            ("eximmanagement-service",         "src/Services/sciServices/eximmanagementServices",        "Dockerfile"),
            ("fillingoperation-service",       "src/Services/sciServices/fillingoperationServices",      "Dockerfile"),
            ("gstcompliance-service",          "src/Services/sciServices/gstcomplianceServices",         "Dockerfile"),
            ("inventorymanagement-service",    "src/Services/sciServices/inventorymanagementServices",   "Dockerfile"),
            ("mamallocation-service",          "src/Services/sciServices/mamallocationServices",         "Dockerfile"),
            ("masterdata-service",             "src/Services/sciServices/masterdataServices",            "Dockerfile"),
            ("orderschedule-service",          "src/Services/sciServices/orderscheduleServices",         "Dockerfile"),
            ("productionmanagement-service",   "src/Services/sciServices/productionmanagementServices",  "Dockerfile"),
            ("purchasesales-service",          "src/Services/sciServices/purchasesalesService",          "Dockerfile"),
            ("sci-transaction-service",        "src/Services/sciServices/scitransactionalServices",      "Dockerfile"),
            ("security-service",               "src/Services/sciServices/SecurityServices",              "Dockerfile"),
            ("strategicstock-service",         "src/Services/sciServices/strategicstockServices",        "Dockerfile"),
            ("vechicletracking-service",       "src/Services/sciServices/vechicletrackingServices",      "Dockerfile"),
        ],
    },
    "sparshServices": {
        "path": "src/Services/sparshServices",
        "services": [
            ("api-gateway",                     "src/Services/sparshServices/apigateway/SparshApiGateway",                    "Dockerfile"),
            ("employeepridemanagement-service", "src/Services/sparshServices/employeepridemanagementServices",                "Dockerfile"),
            ("mobileappmanagement-service",     "src/Services/sparshServices/mobileappmanagementServices",                    "Dockerfile"),
            ("mobileexpense-service",           "src/Services/sparshServices/mobileexpenseServices",                          "Dockerfile"),
            ("problemmanagement-service",       "src/Services/sparshServices/problemmanagementServices/ProblemManagement",    "Dockerfile"),
            ("sparsh-transaction-service",      "src/Services/sparshServices/sparshtransactionalServices/SparshTransactional","Dockerfile"),
        ],
    },
    "sscServices": {
        "path": "src/Services/sscServices",
        "services": [
            ("api-gateway",               "src/Services/sscServices/apigateway",                                      "Dockerfile"),
            ("approvalgroup-service",     "src/Services/sscServices/approvalgroupServices",                           "Dockerfile"),
            ("batchandenvelope-service",  "src/Services/sscServices/batchandenvelopeServices",                        "Dockerfile"),
            ("categoryandvendor-service", "src/Services/sscServices/categoryandvendorServices",                       "Dockerfile"),
            ("clubmembership-service",    "src/Services/sscServices/clubmembershipServices",                          "Dockerfile"),
            ("fillingandarchive-service", "src/Services/sscServices/fillingandarchiveServices",                       "Dockerfile"),
            ("hrdocument-service",        "src/Services/sscServices/hrdocumentServices",                              "Dockerfile"),
            ("integration-service",       "src/Services/sscServices/integrationServices/IntegrationService",          "Dockerfile"),
            ("invoiceprocessing-service", "src/Services/sscServices/invoiceprocessingServices/InvoiceProcessing.Service","Dockerfile"),
            ("masterdata-service",        "src/Services/sscServices/masterdataServices/MasterDataService",            "Dockerfile"),
            ("menuandsecurity-service",   "src/Services/sscServices/menuandsecurityServices",                         "Dockerfile"),
            ("menu-service",              "src/Services/sscServices/menuServices/01_USER_MODULE",                     "Dockerfile"),
            ("ssc-transaction-service",   "src/Services/sscServices/ssctransactionalServices",                        "Dockerfile"),
        ],
    },
    "taskServices": {
        "path": "src/Services/taskServices",
        "services": [
            ("api-gateway",                "src/Services/taskServices/apiGateway",              "Dockerfile"),
            ("complaint-service",          "src/Services/taskServices/complaintServices",        "Dockerfile"),
            ("energy-service",             "src/Services/taskServices/energyServices",           "Dockerfile"),
            ("lookup-service",             "src/Services/taskServices/lookupServices",           "Dockerfile"),
            ("task-service",               "src/Services/taskServices/taskServices",             "Dockerfile"),
            ("task-transactional-service", "src/Services/taskServices/taskTransactionalServices","Dockerfile"),
            ("unit-service",               "src/Services/taskServices/unitServices",             "Dockerfile"),
        ],
    },
    "tourServices": {
        "path": "src/Services/tourServices",
        "services": [
            ("api-gateway",        "src/Services/tourServices/apiGateway",                   "Dockerfile"),
            ("admin-service",      "src/Services/tourServices/adminServices",                "Dockerfile"),
            ("booking-service",    "src/Services/tourServices/bookingServices/BookingService","Dockerfile"),
            ("config-service",     "src/Services/tourServices/configServices",               "Dockerfile"),
            ("tourplan-service",   "src/Services/tourServices/tourplanServices",             "Dockerfile"),
            ("tour-service",       "src/Services/tourServices/tourServices",                 "Dockerfile"),
            ("transaction-service","src/Services/tourServices/transactionServices",          "Dockerfile"),
            ("travel-service",     "src/Services/tourServices/travelServices",               "Dockerfile"),
        ],
    },
    "travelServices": {
        "path": "src/Services/travelServices",
        "services": [
            ("api-gateway",              "src/Services/travelServices/ApiGateway",               "Dockerfile"),
            ("admin-service",            "src/Services/travelServices/adminServices",             "Dockerfile"),
            ("agens-service",            "src/Services/travelServices/agensService",              "Dockerfile"),
            ("booking-service",          "src/Services/travelServices/bookingServices",           "Dockerfile"),
            ("travelexpense-service",    "src/Services/travelServices/expenseServices",           "Dockerfile"),
            ("travelfinance-service",    "src/Services/travelServices/financeServices",           "Dockerfile"),
            ("insurance-service",        "src/Services/travelServices/insuranceServices",         "Dockerfile"),
            ("masterdata-service",       "src/Services/travelServices/masterdataServices",        "Dockerfile"),
            ("travelrequest-service",    "src/Services/travelServices/travelRequestServices",     "Dockerfile"),
            ("travel-transaction-service","src/Services/travelServices/traveltransactionServices","Dockerfile"),
        ],
    },
    "wmsServices": {
        "path": "src/Services/wmsServices",
        "services": [
            ("api-gateway",                "src/Services/wmsServices/apiGateway",                        "Dockerfile"),
            ("auditlog-service",           "src/Services/wmsServices/auditlogService/AuditLogService",   "Dockerfile"),
            ("customer-service",           "src/Services/wmsServices/customerService",                   "Dockerfile"),
            ("employee-service",           "src/Services/wmsServices/emplyeeService",                    "Dockerfile"),
            ("fleet-management-service",   "src/Services/wmsServices/fleetManagementService",            "Dockerfile"),
            ("inventory-service",          "src/Services/wmsServices/inventoryService",                  "Dockerfile"),
            ("order-service",              "src/Services/wmsServices/orderService",                      "Dockerfile"),
            ("product-service",            "src/Services/wmsServices/productService",                    "Dockerfile"),
            ("purchaseorder-service",      "src/Services/wmsServices/purchaseorderService",              "Dockerfile"),
            ("rackingsystem-service",      "src/Services/wmsServices/rackingsystemService",              "Dockerfile"),
            ("receiving-service",          "src/Services/wmsServices/receivingService",                  "Dockerfile"),
            ("salesorder-service",         "src/Services/wmsServices/salesorderService",                 "Dockerfile"),
            ("security-service",           "src/Services/wmsServices/securityService",                   "Dockerfile"),
            ("shipment-service",           "src/Services/wmsServices/shipmentService",                   "Dockerfile"),
            ("supplier-service",           "src/Services/wmsServices/supplierService",                   "Dockerfile"),
            ("warehousestructure-service", "src/Services/wmsServices/warehousestructureService",         "Dockerfile"),
            ("wmtransactional-service",    "src/Services/wmsServices/wmtransactionalService",            "Dockerfile"),
        ],
    },
}

# ── Count totals ──────────────────────────────────────────────────────────────
total_services = sum(len(v["services"]) for v in MODULES.values())

# ── Build module paths for trigger ───────────────────────────────────────────
trigger_paths = "\n".join(
    f"      - {cfg['path']}/**" for cfg in MODULES.values()
)

# ── Build module options for parameter ───────────────────────────────────────
module_options = "\n".join(f"      - {m}" for m in MODULES)

# ── Build matrix block: one entry per service, keyed as "module__name" ───────
matrix_lines = []
for module, cfg in MODULES.items():
    for name, ctx, df in cfg["services"]:
        key = f"{module}__{name}".replace("-", "_")
        matrix_lines.append(f"            {key}:")
        matrix_lines.append(f"              module: '{module}'")
        matrix_lines.append(f"              imageName: '{name}'")
        matrix_lines.append(f"              context: '{ctx}'")
        matrix_lines.append(f"              dockerfile: '{ctx}/{df}'")
matrix_block = "\n".join(matrix_lines)

# ── Service name list for parameter values ────────────────────────────────────
all_names_set = []
seen = set()
for cfg in MODULES.values():
    for name, _, _ in cfg["services"]:
        if name not in seen:
            all_names_set.append(name)
            seen.add(name)
service_options = "\n".join(f"      - {n}" for n in all_names_set)

yaml = f"""\
# =============================================================================
# ERP Microservice — Root Azure Pipelines
# Builds & pushes ALL {total_services} Docker images across {len(MODULES)} modules to GHCR.
# Registry: ghcr.io/$(githubOwner)/erp/<service-name>
#
# Setup required (once per Azure DevOps project):
#   Project Settings -> Service connections -> New -> Docker Registry
#   Name     : ghcr-service-connection
#   Type     : Other (Docker Registry)
#   Registry : https://ghcr.io
#   Username : <your-github-username>
#   Password : <GitHub PAT with packages:write scope>
# =============================================================================

name: Build & Push All ERP Services

trigger:
  branches:
    include:
      - main
      - develop
  paths:
    include:
{trigger_paths}

pr:
  branches:
    include:
      - main
  paths:
    include:
{trigger_paths}

# ── Parameters ────────────────────────────────────────────────────────────────
parameters:
  - name: module
    displayName: Module to build
    type: string
    default: all
    values:
      - all
{module_options}

  - name: service
    displayName: Service to build (within module, or "all")
    type: string
    default: all

  - name: pushImage
    displayName: Push images to GHCR
    type: boolean
    default: true

  - name: smokeTest
    displayName: Run smoke test after push
    type: boolean
    default: false

# ── Variables ─────────────────────────────────────────────────────────────────
variables:
  imagePrefix: 'ghcr.io/vimalshan/erp'
  tag: '$(Build.BuildId)'
  vmImageName: 'ubuntu-latest'

pool:
  vmImage: $(vmImageName)

# ── Stages ────────────────────────────────────────────────────────────────────
stages:

  # ── BUILD ──────────────────────────────────────────────────────────────────
  - stage: Build
    displayName: Build Docker Images
    jobs:
      - job: BuildImages
        displayName: Build
        strategy:
          matrix:
{matrix_block}

        steps:
          - checkout: self
            fetchDepth: 1

          - task: Docker@2
            displayName: 'Build $(module)/$(imageName)'
            condition: |
              and(
                or(eq('${{{{ parameters.module }}}}', 'all'), eq('${{{{ parameters.module }}}}', variables['module'])),
                or(eq('${{{{ parameters.service }}}}', 'all'), eq('${{{{ parameters.service }}}}', variables['imageName']))
              )
            inputs:
              command: build
              repository: $(imagePrefix)/$(imageName)
              dockerfile: $(dockerfile)
              buildContext: $(context)
              tags: |
                $(tag)
                latest

  # ── PUSH ───────────────────────────────────────────────────────────────────
  - stage: Push
    displayName: Push to GHCR
    dependsOn: Build
    condition: and(succeeded(), eq('${{{{ parameters.pushImage }}}}', true))
    jobs:
      - job: PushImages
        displayName: Push
        strategy:
          matrix:
{matrix_block}

        steps:
          - checkout: none

          - script: |
              echo "$GITHUB_TOKEN" | docker login ghcr.io -u "$GITHUB_ACTOR" --password-stdin
            displayName: Login to GHCR
            env:
              GITHUB_TOKEN: $(GITHUB_TOKEN)
              GITHUB_ACTOR: $(GITHUB_ACTOR)

          - task: Docker@2
            displayName: 'Push $(module)/$(imageName)'
            condition: |
              and(
                or(eq('${{{{ parameters.module }}}}', 'all'), eq('${{{{ parameters.module }}}}', variables['module'])),
                or(eq('${{{{ parameters.service }}}}', 'all'), eq('${{{{ parameters.service }}}}', variables['imageName']))
              )
            inputs:
              command: push
              repository: $(imagePrefix)/$(imageName)
              tags: |
                $(tag)
                latest

  # ── SMOKE TEST (optional) ──────────────────────────────────────────────────
  - stage: SmokeTest
    displayName: Smoke Test
    dependsOn: Push
    condition: and(succeeded(), eq('${{{{ parameters.smokeTest }}}}', true))
    jobs:
      - job: SmokeTestImages
        displayName: Smoke test
        strategy:
          matrix:
{matrix_block}

        steps:
          - script: |
              IMAGE="$(imagePrefix)/$(imageName):latest"
              echo "Pulling $IMAGE ..."
              docker pull "$IMAGE"
              CNAME="smoke-$(imageName)-$(Build.BuildId)"
              docker run -d --name "$CNAME" \\
                -e ASPNETCORE_ENVIRONMENT=Development \\
                "$IMAGE"
              sleep 10
              docker logs "$CNAME"
              docker rm -f "$CNAME"
            displayName: 'Smoke test $(module)/$(imageName)'
            condition: |
              and(
                or(eq('${{{{ parameters.module }}}}', 'all'), eq('${{{{ parameters.module }}}}', variables['module'])),
                or(eq('${{{{ parameters.service }}}}', 'all'), eq('${{{{ parameters.service }}}}', variables['imageName']))
              )
"""

out = REPO / "azure-pipelines.yml"
out.write_text(yaml, encoding="utf-8")
print(f"Written: {out}")
print(f"Modules : {len(MODULES)}")
print(f"Services: {total_services}")
