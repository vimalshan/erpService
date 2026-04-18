Now I have all the data needed. Here are the complete docker run commands for every service, module by module, with correct port mappings from the actual docker-compose files.

Prerequisites for all modules:
1. Create a shared Docker network: `docker network create erp-network`
2. Start SQL Server and RabbitMQ on the same network first (see below).
3. All `docker run` commands below include `--network erp-network` so containers can resolve each other by hostname.

```bash
# SQL Server
docker run -d --name sqlserver --network erp-network \
  -p 1433:1433 \
  -e ACCEPT_EULA=Y -e "SA_PASSWORD=YourStrong@Passw0rd" \
  mcr.microsoft.com/mssql/server:2022-latest

# RabbitMQ (default vhost "/")
docker run -d --name rabbitmq --network erp-network \
  -p 5672:5672 -p 15672:15672 \
  -e RABBITMQ_DEFAULT_USER=erpadmin -e RABBITMQ_DEFAULT_PASS=ErpR@bbit1 \
  rabbitmq:3-management
```

🔑 AuthProvider
bash
docker run -d --name auth-provider --network erp-network \
  -p 5200:5200 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://+:5200 \
  -e "ConnectionStrings__AuthProviderDB=Data Source=sqlserver,1433;Initial Catalog=AuthProviderDB;User ID=sa;Password=YourStrong@Passw0rd;Encrypt=False;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/auth-provider:latest

🔧 adminServices
Service	Host Port → Container Port
api-gateway	5000 → 5000
finyear-api	5186 → 5000
location-services	7136 → 7136
vendor-service	5181 → 5181
lov-service	5184 → 5181
scholarship-service	5166 → 5166
stationery-service	5182 → 5181
tds-service	5183 → 5181
transaction-service	5185 → 5185
bash

docker run -d --name finyear-api --network erp-network \
  -p 5186:5000 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5000 \
  -e "ConnectionStrings__AdminDbConnection=Data Source=sqlserver,1433;Initial Catalog=ADMINDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/finyear-api:latest

docker run -d --name location-services --network erp-network \
  -p 7136:7136 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:7136 \
  -e "ConnectionStrings__LocationDb=Data Source=sqlserver,1433;Initial Catalog=LOCATIONDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/location-services:latest

docker run -d --name vendor-service --network erp-network \
  -p 5181:5181 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5181 \
  -e "ConnectionStrings__VendorDb=Data Source=sqlserver,1433;Initial Catalog=VENDORDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/vendor-service:latest

docker run -d --name lov-service --network erp-network \
  -p 5184:5181 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5181 \
  -e "ConnectionStrings__LovDb=Data Source=sqlserver,1433;Initial Catalog=LOVDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/lov-service:latest

docker run -d --name scholarship-service --network erp-network \
  -p 5166:5166 \
  -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS=http://+:5166 \
  -e "ConnectionStrings__ScholarshipDb=Data Source=sqlserver,1433;Initial Catalog=ADMINDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/scholarship-service:latest

docker run -d --name stationery-service --network erp-network \
  -p 5182:5181 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5181 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=STATIONERYDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/stationery-service:latest

docker run -d --name tds-service --network erp-network \
  -p 5183:5181 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5181 \
  -e "ConnectionStrings__TdsDb=Data Source=sqlserver,1433;Initial Catalog=TDSDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/tds-service:latest

docker run -d --name transaction-service --network erp-network \
  -p 5185:5185 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5185 \
  -e "ConnectionStrings__TransactionDb=Data Source=sqlserver,1433;Initial Catalog=ADMINDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/transaction-service:latest

docker run -d --name admin-api-gateway --network erp-network \
  -p 5000:5000 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5000 \
  ghcr.io/vimalshan/erp/api-gateway:latest
🎯 aimsServices
Service	Host Port → Container Port
api-gateway	5020 → 80
access-service	5010 → 80
attendance-service	5011 → 80
bus-service	5012 → 80
calendar-service	5013 → 80
employee-service	5014 → 80
groupincentive-service	5015 → 80
leave-service	5016 → 80
reference-service	5017 → 80
visitor-service	5018 → 80
aimstransaction-service	5019 → 80
bash
docker run -d --name aims-access-service --network erp-network \
  -p 5010:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ACCESSDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/access-service:latest

docker run -d --name aims-attendance-service --network erp-network \
  -p 5011:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__AttendanceDb=Server=sqlserver,1433;Database=ATTENDANCEDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/attendance-service:latest

docker run -d --name aims-bus-service --network erp-network \
  -p 5012:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__BusDb=Server=sqlserver,1433;Database=BUSDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/bus-services:latest

docker run -d --name aims-calendar-service --network erp-network \
  -p 5013:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__CalendarDb=Server=sqlserver,1433;Database=CALENDARDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/calendar-service:latest

docker run -d --name aims-employee-service --network erp-network \
  -p 5014:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__EmployeeDb=Server=sqlserver,1433;Database=EMPLOYEEDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/employee-service:latest

docker run -d --name aims-groupincentive-service --network erp-network \
  -p 5015:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=GROUPINCENTIVEDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/groupincentive-service:latest

docker run -d --name aims-leave-service --network erp-network \
  -p 5016:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__LeaveDb=Server=sqlserver,1433;Database=LEAVEDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/leave-services:latest

docker run -d --name aims-reference-service --network erp-network \
  -p 5017:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=REFERENCEDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/reference-service:latest

docker run -d --name aims-visitor-service --network erp-network \
  -p 5018:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__VisitorDb=Server=sqlserver,1433;Database=VISITORDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/visitor-services:latest

docker run -d --name aims-transaction-service --network erp-network \
  -p 5019:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__AimsTransactionDb=Server=sqlserver,1433;Database=AIMSDB;User Id=sa;******;TrustServerCertificate=true;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/aims-transaction-service:latest

docker run -d --name aims-api-gateway --network erp-network \
  -p 5020:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/api-gateway:latest
🔍 auditServices
Service	Host Port → Container Port
api-gateway	5000 → 8080
action-service	5001 → 8080
audit-service	5002 → 8080
certificate-service	5003 → 8080
contract-service	5004 → 8080
finance-service	5005 → 8080
findings-service	5006 → 8080
notification-service	5007 → 8080
schedule-service	5008 → 8080
settings-service	5009 → 8080
bash
docker run -d --name audit-action-service --network erp-network \
  -p 5001:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ERPActionDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=erpadmin -e RabbitMQ__Password=ErpR@bbit1 \
  ghcr.io/vimalshan/erp/action-service:latest

docker run -d --name audit-audit-service --network erp-network \
  -p 5002:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ERPAuditDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=erpadmin -e RabbitMQ__Password=ErpR@bbit1 \
  ghcr.io/vimalshan/erp/audit-service:latest

docker run -d --name audit-certificate-service --network erp-network \
  -p 5003:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ERPCertificateDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=erpadmin -e RabbitMQ__Password=ErpR@bbit1 \
  ghcr.io/vimalshan/erp/certificate-service:latest

docker run -d --name audit-contract-service --network erp-network \
  -p 5004:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ERPContractDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=erpadmin -e RabbitMQ__Password=ErpR@bbit1 \
  ghcr.io/vimalshan/erp/contract-service:latest

docker run -d --name audit-finance-service --network erp-network \
  -p 5005:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ERPFinanceDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=erpadmin -e RabbitMQ__Password=ErpR@bbit1 \
  ghcr.io/vimalshan/erp/finance-service:latest

docker run -d --name audit-findings-service --network erp-network \
  -p 5006:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ERPFindingsDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=erpadmin -e RabbitMQ__Password=ErpR@bbit1 \
  ghcr.io/vimalshan/erp/findings-service:latest

docker run -d --name audit-notification-service --network erp-network \
  -p 5007:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ERPNotificationDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=erpadmin -e RabbitMQ__Password=ErpR@bbit1 \
  ghcr.io/vimalshan/erp/notification-service:latest

docker run -d --name audit-schedule-service --network erp-network \
  -p 5008:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ERPScheduleDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=erpadmin -e RabbitMQ__Password=ErpR@bbit1 \
  ghcr.io/vimalshan/erp/schedule-service:latest

docker run -d --name audit-settings-service --network erp-network \
  -p 5009:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ERPSettingsDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=erpadmin -e RabbitMQ__Password=ErpR@bbit1 \
  ghcr.io/vimalshan/erp/settings-service:latest

docker run -d --name audit-api-gateway --network erp-network \
  -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "Jwt__Key=YourSuperSecretKeyThatIsAtLeast32Characters!" -e Jwt__Issuer=ERPSystem -e Jwt__Audience=ERPUsers \
  ghcr.io/vimalshan/erp/api-gateway:latest
🍽️ canteenServices
Service	Host Port
api-gateway	5200
canteen-unit-service	5190
card-management-service	5191
deduction-service	5192
eligibility-service	5193
itemmaster-service	5194
referencedata-service	5195
swipe-transaction-service	5196
canteen-transaction-service	5197
bash
docker run -d --name canteen-unit-service --network erp-network \
  -p 5190:5190 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5190 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=CanteenUnitDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/canteen-unit-service:latest

docker run -d --name card-management-service --network erp-network \
  -p 5191:5191 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5191 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=CardManagementDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/card-management-service:latest

docker run -d --name canteen-deduction-service --network erp-network \
  -p 5192:5192 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5192 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=DeductionServiceDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/deduction-service:latest

docker run -d --name canteen-eligibility-service --network erp-network \
  -p 5193:5193 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5193 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=EligibilityServiceDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/eligibility-service:latest

docker run -d --name canteen-itemmaster-service --network erp-network \
  -p 5194:5194 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5194 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=ItemMasterDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/item-master-service:latest

docker run -d --name canteen-referencedata-service --network erp-network \
  -p 5195:5195 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5195 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=ReferenceDataDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/reference-data-service:latest

docker run -d --name canteen-swipe-transaction-service --network erp-network \
  -p 5196:5196 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5196 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=SwipeTransactionDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/swipe-transaction-service:latest

docker run -d --name canteen-transaction-service --network erp-network \
  -p 5197:5197 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5197 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=CanteenTransactionDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/canteen-transaction-service:latest

docker run -d --name canteen-api-gateway --network erp-network \
  -p 5200:5200 \
  -e ASPNETCORE_ENVIRONMENT=Docker -e ASPNETCORE_URLS=http://+:5200 \
  ghcr.io/vimalshan/erp/api-gateway:latest
💵 cashServices
Service	Host Port
api-gateway	5000
organization-setup-api	5099
currency-management-api	5031
deal-ticketing-api	5081
loan-management-api	5268
cash-management-api	5249
email-notification-api	5032
transaction-processing-api	5100
bash
docker run -d --name cash-organization-setup --network erp-network \
  -p 5099:5099 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5099 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=CASHDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/organization-setup-service:latest

docker run -d --name cash-currency-management --network erp-network \
  -p 5031:5031 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5031 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=CASHDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/current-management-service:latest

docker run -d --name cash-deal-ticketing --network erp-network \
  -p 5081:5081 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5081 \
  -e "ConnectionStrings__DealTicketingDb=Data Source=sqlserver,1433;Initial Catalog=CASHDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/deal-ticketing-service:latest

docker run -d --name cash-loan-management --network erp-network \
  -p 5268:5268 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5268 \
  -e "ConnectionStrings__LoanManagement=Data Source=sqlserver,1433;Initial Catalog=CASHDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/loan-management-service:latest

docker run -d --name cash-management-api --network erp-network \
  -p 5249:5249 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5249 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=CASHDB;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/cash-management-service:latest

docker run -d --name cash-email-notification --network erp-network \
  -p 5032:5032 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5032 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=EmailNotificationDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/email-notification-service:latest

docker run -d --name cash-transaction-processing --network erp-network \
  -p 5100:5100 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5100 \
  -e "ConnectionStrings__DefaultConnection=Data Source=sqlserver,1433;Initial Catalog=TransactionProcessingDb;User ID=sa;******;Encrypt=False;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/transaction-service:latest

docker run -d --name cash-api-gateway --network erp-network \
  -p 5000:5000 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:5000 \
  ghcr.io/vimalshan/erp/api-gateway:latest
📊 ddServices
Service	Host Port
api-gateway	5200
appraisal-service	5100
authorization-service	5177
compensation-service	5000
competency-service	5261
demandmanagement-service	5210
document-service	5081
employee-service	5049
feedback-service	5101
learning-service	5102
objective-service	5258
other-service	5224
promotion-service	5103
recruitment-service	5237
reporting-service	5104
transaction-service	5178
bash
docker run -d --name dd-appraisal-service --network erp-network -p 5100:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__AppraisalDb=Server=sqlserver,1433;Database=AppraisalDb;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=erp-rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/appraisal-service:latest

docker run -d --name dd-authorization-service --network erp-network -p 5177:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=AuthorizationServiceDb;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Hostname=erp-rabbitmq -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/authorization-service:latest

docker run -d --name dd-compensation-service --network erp-network -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__CompensationDb=Server=sqlserver,1433;Database=CompensationDb;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/compensation-service:latest

docker run -d --name dd-competency-service --network erp-network -p 5261:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__CompetencyDb=Server=sqlserver,1433;Database=DDDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/competency-service:latest

docker run -d --name dd-demandmanagement-service --network erp-network -p 5210:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=DDDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/demand-management-service:latest

docker run -d --name dd-document-service --network erp-network -p 5081:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=DDDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/document-service:latest

docker run -d --name dd-employee-service --network erp-network -p 5049:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=EmployeeServiceDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/employee-service:latest

docker run -d --name dd-feedback-service --network erp-network -p 5101:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=DDDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/feedback-service:latest

docker run -d --name dd-learning-service --network erp-network -p 5102:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TodosDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/learning-service:latest

docker run -d --name dd-objective-service --network erp-network -p 5258:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=DDDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/objective-service:latest

docker run -d --name dd-other-service --network erp-network -p 5224:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=DDDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/other-services:latest

docker run -d --name dd-promotion-service --network erp-network -p 5103:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=DDDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/promotion-service:latest

docker run -d --name dd-recruitment-service --network erp-network -p 5237:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=RecruitmentDb;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/recruitment-service:latest

docker run -d --name dd-reporting-service --network erp-network -p 5104:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ReportingServiceDb;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/reporting-service:latest

docker run -d --name dd-transaction-service --network erp-network -p 5178:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TransactionServiceDb;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/transaction-service:latest

docker run -d --name dd-api-gateway --network erp-network -p 5200:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
🏥 healthServices
Service	Host Port
api-gateway	5600
accident-service	5000
checkup-service	7101
insurance-service	5100
masters-service	5200
medicalvisit-service	5300
medicine-service	5400
transaction-service	5500
bash
docker run -d --name health-accident-service --network erp-network -p 5000:5000 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__HealthDb=Server=sqlserver,1433;Database=HEALTHDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/accident-management-service:latest

docker run -d --name health-checkup-service --network erp-network -p 7101:7101 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__HealthDb=Server=sqlserver,1433;Database=HEALTHDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/healthcheckup-service:latest

docker run -d --name health-insurance-service --network erp-network -p 5100:5100 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=HEALTHDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/insurance-management-service:latest

docker run -d --name health-masters-service --network erp-network -p 5200:5200 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=HEALTHDB;User Id=sa;******;TrustServerCertificate=True;" \
  ghcr.io/vimalshan/erp/master-service:latest

docker run -d --name health-medicalvisit-service --network erp-network -p 5300:5300 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=HEALTHDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/medicalvisit-service:latest

docker run -d --name health-medicine-service --network erp-network -p 5400:5400 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=HEALTHDB_MedicineManagement;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/medicine-management-service:latest

docker run -d --name health-transaction-service --network erp-network -p 5500:5500 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=HEALTHDB_HealthTransactions;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/health-transaction-service:latest

docker run -d --name health-api-gateway --network erp-network -p 5600:5600 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
👥 hrServicess
Service	Host Port
api-gateway	5310
alerts-service	5154
compensation-service	5009
employee-management-service	5004
employee-relations-service	5075
exit-management-service	5094
organization-service	5027
recruitment-service	5265
time-attendance-service	5235
training-service	5003
user-security-service	5140
employee-transactions-service	5204
bash
docker run -d --name hr-alerts-service --network erp-network -p 5154:5154 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=AlertsNotificationsDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/alerts-notifications-service:latest

docker run -d --name hr-compensation-service --network erp-network -p 5009:5009 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=CompensationBenefitsDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/compensation-benefits-service:latest

docker run -d --name hr-employee-management --network erp-network -p 5004:5004 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=EmployeeManagementDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/employee-management-service:latest

docker run -d --name hr-employee-relations --network erp-network -p 5075:5075 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=EmployeeRelationsDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/employee-relations-service:latest

docker run -d --name hr-exit-management --network erp-network -p 5094:5094 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ExitManagementDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/exit-management-service:latest

docker run -d --name hr-organization-service --network erp-network -p 5027:5027 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=OrganizationStructureDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/organization-structure-service:latest

docker run -d --name hr-recruitment-service --network erp-network -p 5265:5265 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=RecruitmentDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/recruitment-service:latest

docker run -d --name hr-time-attendance --network erp-network -p 5235:5235 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TimeAttendanceDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/time-attendance-service:latest

docker run -d --name hr-training-service --network erp-network -p 5003:5003 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TrainingDevelopmentDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/training-development-service:latest

docker run -d --name hr-user-security --network erp-network -p 5140:5140 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=UserSecurityDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/user-security-service:latest

docker run -d --name hr-employee-transactions --network erp-network -p 5204:5204 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=EmployeeTransactionsDB;User=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/employee-transactions-service:latest

docker run -d --name hr-api-gateway --network erp-network -p 5310:5310 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
📚 letServices
Service	Host Port
api-gateway	5400
leave-service	5166
course-service	5215
request-service	5006
review-service	5114
development-service	5216
master-service	5279
let-transaction-service	5320
bash
docker run -d --name let-leave-service --network erp-network -p 5166:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LETDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__UserName=letadmin -e RabbitMQ__Password=LetR@bbit2026 \
  ghcr.io/vimalshan/erp/leave-service:latest

docker run -d --name let-course-service --network erp-network -p 5215:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LETDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__UserName=letadmin -e RabbitMQ__Password=LetR@bbit2026 \
  ghcr.io/vimalshan/erp/course-service:latest

docker run -d --name let-request-service --network erp-network -p 5006:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LETDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__UserName=letadmin -e RabbitMQ__Password=LetR@bbit2026 \
  ghcr.io/vimalshan/erp/request-service:latest

docker run -d --name let-review-service --network erp-network -p 5114:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LETDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__UserName=letadmin -e RabbitMQ__Password=LetR@bbit2026 \
  ghcr.io/vimalshan/erp/review-service:latest

docker run -d --name let-development-service --network erp-network -p 5216:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LETDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__UserName=letadmin -e RabbitMQ__Password=LetR@bbit2026 \
  ghcr.io/vimalshan/erp/development-service:latest

docker run -d --name let-master-service --network erp-network -p 5279:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LETDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__UserName=letadmin -e RabbitMQ__Password=LetR@bbit2026 \
  ghcr.io/vimalshan/erp/master-service:latest

docker run -d --name let-transaction-service --network erp-network -p 5320:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LETDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__UserName=letadmin -e RabbitMQ__Password=LetR@bbit2026 \
  ghcr.io/vimalshan/erp/let-transaction-service:latest

docker run -d --name let-api-gateway --network erp-network -p 5400:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
🏦 loanServices
Service	Host Port
api-gateway	6100
loan-transaction	5292
loan-application	5282
loan-account	5150
loan-definition	5077
document-service	5280
lov-service	5008
utility-service	5143
bash
docker run -d --name loan-transaction --network erp-network -p 5292:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LOANDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/loan-transaction-service:latest

docker run -d --name loan-application --network erp-network -p 5282:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LOANDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/loanapplication-service:latest

docker run -d --name loan-account --network erp-network -p 5150:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__LoanAccountDb=Server=sqlserver,1433;Database=LOANDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/loanaccount-service:latest

docker run -d --name loan-definition --network erp-network -p 5077:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__LoanDb=Server=sqlserver,1433;Database=LOANDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMq__HostName=rabbitmq -e RabbitMq__Port=5672 -e RabbitMq__UserName=guest -e RabbitMq__Password=guest \
  ghcr.io/vimalshan/erp/loandefinition-service:latest

docker run -d --name loan-document-service --network erp-network -p 5280:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LOANDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/document-service:latest

docker run -d --name loan-lov-service --network erp-network -p 5008:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__LovDb=Server=sqlserver,1433;Database=LOANDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/lov-service:latest

docker run -d --name loan-utility-service --network erp-network -p 5143:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=LOANDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/utility-service:latest

docker run -d --name loan-api-gateway --network erp-network -p 6100:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
🌐 mainsparshServices
⚠️ Note: Only the API gateway has an external port (5100). All 14 microservices run on internal ports only (no -p mapping). Access them through the gateway.

bash
# Start gateway with external port
docker run -d --name srfsparsh-api-gateway --network erp-network -p 5100:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest

# Internal services (no external ports — access via gateway on 5100)
docker run -d --name srfsparsh-approval --network erp-network       -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/approval-service:latest
docker run -d --name srfsparsh-booking --network erp-network        -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/booking-service:latest
docker run -d --name srfsparsh-community --network erp-network      -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/community-service:latest
docker run -d --name srfsparsh-compensation --network erp-network   -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/compensation-service:latest
docker run -d --name srfsparsh-groupmanagement --network erp-network -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/groupmanagement-service:latest
docker run -d --name srfsparsh-location --network erp-network       -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/location-service:latest
docker run -d --name srfsparsh-meeting --network erp-network        -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/meeting-service:latest
docker run -d --name srfsparsh-proxy --network erp-network          -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/proxy-service:latest
docker run -d --name srfsparsh-reimbursement --network erp-network  -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/reimbursement-service:latest
docker run -d --name srfsparsh-stipend --network erp-network        -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/stipend-service:latest
docker run -d --name srfsparsh-timesheet --network erp-network      -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/timesheet-service:latest
docker run -d --name srfsparsh-transaction --network erp-network    -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/transaction-service:latest
docker run -d --name srfsparsh-usermanagement --network erp-network -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/usermanagement-service:latest
docker run -d --name srfsparsh-websitecontent --network erp-network -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/vimalshan/erp/websitecontent-service:latest
🗂️ myworkServices
Service	Host Port
erp-gateway	5000
audit-service	5037
batch-service	5111
csa-service	5035
project-service	5290
risk-service	5033
team-service	5183
timesheet-service	5188
workorder-service	5138
bash
docker run -d --name mywork-audit-service --network erp-network -p 5037:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=MYWORKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/audit-service:latest

docker run -d --name mywork-batch-service --network erp-network -p 5111:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=MYWORKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/batch-service:latest

docker run -d --name mywork-csa-service --network erp-network -p 5035:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__CsaDatabase=Server=sqlserver,1433;Database=MYWORKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/csa-service:latest

docker run -d --name mywork-project-service --network erp-network -p 5290:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=MYWORKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/project-service:latest

docker run -d --name mywork-risk-service --network erp-network -p 5033:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=MYWORKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/risk-service:latest

docker run -d --name mywork-team-service --network erp-network -p 5183:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=MYWORKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/team-service:latest

docker run -d --name mywork-timesheet-service --network erp-network -p 5188:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=MYWORKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/timesheet-service:latest

docker run -d --name mywork-workorder-service --network erp-network -p 5138:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=MYWORKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/workorder-service:latest

docker run -d --name mywork-erp-gateway --network erp-network -p 5000:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
💰 payServices
Service	Host Port
api-gateway	5100
employee-service	5104
hr-service	5000
faq-service	5032
payroll-service	5002
tax-service	5010
paytransactional-service	5020
bash
docker run -d --name pay-employee-service --network erp-network -p 5104:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PAYDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=erp_user -e RabbitMQ__Password=erp_password \
  ghcr.io/vimalshan/erp/employee-service:latest

docker run -d --name pay-hr-service --network erp-network -p 5000:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PAYDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=erp_user -e RabbitMQ__Password=erp_password \
  ghcr.io/vimalshan/erp/hr-service:latest

docker run -d --name pay-faq-service --network erp-network -p 5032:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PAYDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=erp_user -e RabbitMQ__Password=erp_password \
  ghcr.io/vimalshan/erp/faq-service:latest

docker run -d --name pay-payroll-service --network erp-network -p 5002:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PAYDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=erp_user -e RabbitMQ__Password=erp_password \
  ghcr.io/vimalshan/erp/payroll-service:latest

docker run -d --name pay-tax-service --network erp-network -p 5010:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TaxService;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=erp_user -e RabbitMQ__Password=erp_password \
  ghcr.io/vimalshan/erp/tax-service:latest

docker run -d --name pay-transactional-service --network erp-network -p 5020:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PayTransactionalService;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=erp_user -e RabbitMQ__Password=erp_password \
  ghcr.io/vimalshan/erp/pay-transaction-service:latest

docker run -d --name pay-api-gateway --network erp-network -p 5100:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
🏛️ pfServices
Service	Host Port
api-gateway	5800
accounting-service	5068
bank-service	5125
contribution-service	5225
investment-service	5171
loan-service	5004
masterdata-service	5090
member-service	5278
pftransactional-service	5160
settlement-service	5149
trust-service	5079
bash
docker run -d --name pf-accounting-service --network erp-network -p 5068:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/accounting-service:latest

docker run -d --name pf-bank-service --network erp-network -p 5125:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__BankDb=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/bank-service:latest

docker run -d --name pf-contribution-service --network erp-network -p 5225:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/contribution-service:latest

docker run -d --name pf-investment-service --network erp-network -p 5171:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__InvestmentDb=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/investment-service:latest

docker run -d --name pf-loan-service --network erp-network -p 5004:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/loan-service:latest

docker run -d --name pf-masterdata-service --network erp-network -p 5090:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/masterdata-service:latest

docker run -d --name pf-member-service --network erp-network -p 5278:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/member-service:latest

docker run -d --name pf-transactional-service --network erp-network -p 5160:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__PFTransactionalDb=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/pf-transaction-service:latest

docker run -d --name pf-settlement-service --network erp-network -p 5149:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__SettlementDb=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/settlement-service:latest

docker run -d --name pf-trust-service --network erp-network -p 5079:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_HTTP_PORTS=8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PFDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=pfuser -e RabbitMQ__Password=pfpassword \
  ghcr.io/vimalshan/erp/trust-service:latest

docker run -d --name pf-api-gateway --network erp-network -p 5800:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
🔬 sciServices
Service	Host Port
api-gateway	5200
security-service	5009
vehicle-tracking-service	5102
dispatch-planning-service	5255
order-schedule-service	5160
filling-operation-service	5058
exim-management-service	5085
gst-compliance-service	5282
inventory-management-service	5097
production-management-service	5087
mam-allocation-service	5140
purchase-sales-service	5170
master-data-service	5180
strategic-stock-service	5045
error-logging-service	5292
sci-transactional-service	5150
bash
docker run -d --name sci-security-service --network erp-network -p 5009:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/security-service:latest

docker run -d --name sci-vehicle-tracking --network erp-network -p 5102:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/vechicletracking-service:latest

docker run -d --name sci-dispatch-planning --network erp-network -p 5255:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/dispatchplanning-service:latest

docker run -d --name sci-order-schedule --network erp-network -p 5160:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/orderschedule-service:latest

docker run -d --name sci-filling-operation --network erp-network -p 5058:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/fillingoperation-service:latest

docker run -d --name sci-exim-management --network erp-network -p 5085:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/eximmanagement-service:latest

docker run -d --name sci-gst-compliance --network erp-network -p 5282:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/gstcompliance-service:latest

docker run -d --name sci-inventory-management --network erp-network -p 5097:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/inventorymanagement-service:latest

docker run -d --name sci-production-management --network erp-network -p 5087:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/productionmanagement-service:latest

docker run -d --name sci-mam-allocation --network erp-network -p 5140:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/mamallocation-service:latest

docker run -d --name sci-purchase-sales --network erp-network -p 5170:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/purchasesales-service:latest

docker run -d --name sci-master-data --network erp-network -p 5180:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/masterdata-service:latest

docker run -d --name sci-strategic-stock --network erp-network -p 5045:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/strategicstock-service:latest

docker run -d --name sci-error-logging --network erp-network -p 5292:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/errorlogging-service:latest

docker run -d --name sci-transactional --network erp-network -p 5150:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SCIDB;User Id=sa;******;TrustServerCertificate=true" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sci_admin -e RabbitMQ__Password=SciRabbit@2026! \
  ghcr.io/vimalshan/erp/sci-transaction-service:latest

docker run -d --name sci-api-gateway --network erp-network -p 5200:80 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:80 \
  ghcr.io/vimalshan/erp/api-gateway:latest
📱 sparshServices
Service	Host Port
api-gateway	5200
employee-pride-api	5082
mobile-app-api	5154
mobile-expense-api	5000
problem-api	5165
transactional-api	5170
bash
docker run -d --name sparsh-employee-pride --network erp-network -p 5082:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SPARSHDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sparsh_user -e RabbitMQ__Password=Sparsh@RMQ2026 \
  ghcr.io/vimalshan/erp/employeepridemanagement-service:latest

docker run -d --name sparsh-mobile-app --network erp-network -p 5154:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SPARSHDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sparsh_user -e RabbitMQ__Password=Sparsh@RMQ2026 \
  ghcr.io/vimalshan/erp/mobileappmanagement-service:latest

docker run -d --name sparsh-mobile-expense --network erp-network -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SPARSHDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Username=sparsh_user -e RabbitMQ__Password=Sparsh@RMQ2026 \
  ghcr.io/vimalshan/erp/mobileexpense-service:latest

docker run -d --name sparsh-problem-management --network erp-network -p 5165:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=ProblemManagementDb;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sparsh_user -e RabbitMQ__Password=Sparsh@RMQ2026 \
  ghcr.io/vimalshan/erp/problemmanagement-service:latest

docker run -d --name sparsh-transactional --network erp-network -p 5170:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SparshTransactionalDb;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=sparsh_user -e RabbitMQ__Password=Sparsh@RMQ2026 \
  ghcr.io/vimalshan/erp/sparsh-transaction-service:latest

docker run -d --name sparsh-api-gateway --network erp-network -p 5200:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
🗃️ sscServices
Service	Host Port
api-gateway	5000
ssc-transactional	8080
batch-and-envelope	8081
category-and-vendor	8082
club-membership	8083
filing-and-archive	8084
hr-document	8085
integration-service	8086
invoice-processing	8087
master-data	8088
menu-and-security	8089
approval-group	8090
user-service	8091
bash
docker run -d --name ssc-transactional --network erp-network -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/ssc-transaction-service:latest

docker run -d --name ssc-batch-envelope --network erp-network -p 8081:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/batchandenvelope-service:latest

docker run -d --name ssc-category-vendor --network erp-network -p 8082:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB_CategoryVendor;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/categoryandvendor-service:latest

docker run -d --name ssc-club-membership --network erp-network -p 8083:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/clubmembership-service:latest

docker run -d --name ssc-filing-archive --network erp-network -p 8084:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/fillingandarchive-service:latest

docker run -d --name ssc-hr-document --network erp-network -p 8085:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/hrdocument-service:latest

docker run -d --name ssc-integration --network erp-network -p 8086:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/integration-service:latest

docker run -d --name ssc-invoice-processing --network erp-network -p 8087:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB_InvoiceProcessing;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__UserName=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/invoiceprocessing-service:latest

docker run -d --name ssc-master-data --network erp-network -p 8088:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/masterdata-service:latest

docker run -d --name ssc-menu-security --network erp-network -p 8089:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/menuandsecurity-service:latest

docker run -d --name ssc-approval-group --network erp-network -p 8090:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/approvalgroup-service:latest

docker run -d --name ssc-user-service --network erp-network -p 8091:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=SSCDB;User Id=sa;******;TrustServerCertificate=True" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__Port=5672 -e
1 premium request · Claude Sonnet 4.6
bash
  RabbitMQ__Username=ssc_user -e RabbitMQ__Password=ssc_password \
  ghcr.io/vimalshan/erp/user-service:latest

docker run -d --name ssc-api-gateway --network erp-network -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
✅ taskServices
Service	Host Port	Notes
api-gateway	5000	External access point
lookup-service	(internal only)	No external port
task-service	(internal only)	No external port
transactional-service	(internal only)	No external port
complaint-service	(internal only)	No external port
energy-service	(internal only)	No external port
unit-service	(internal only)	No external port
⚠️ Note: The task module microservices have no ports: mapping — they are internal-only and accessed exclusively through the API gateway on port 5000.

bash
docker run -d --name task-api-gateway --network erp-network -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest

# Internal services — no external ports; accessed via gateway on 5000
docker run -d --name task-lookup-service --network erp-network \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TASKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/lookup-service:latest

docker run -d --name task-task-service --network erp-network \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__TaskDb=Server=sqlserver,1433;Database=TASKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/task-service:latest

docker run -d --name task-transactional-service --network erp-network \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TASKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/task-transactional-service:latest

docker run -d --name task-complaint-service --network erp-network \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__TaskDb=Server=sqlserver,1433;Database=TASKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__Host=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/complaint-service:latest

docker run -d --name task-energy-service --network erp-network \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__EnergyDb=Server=sqlserver,1433;Database=TASKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMq__HostName=rabbitmq -e RabbitMq__UserName=guest -e RabbitMq__Password=guest \
  ghcr.io/vimalshan/erp/energy-service:latest

docker run -d --name task-unit-service --network erp-network \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TASKDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/unit-service:latest
🗺️ tourServices
Service	Host Port
api-gateway	5000
admin-service	5016
booking-service	5045
config-service	5057
tourplan-service	5143
tour-service	5188
transaction-service	5192
travel-service	5200
bash
docker run -d --name tour-admin-service --network erp-network -p 5016:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TOURDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=tour_user -e RabbitMQ__Password=tour_password \
  ghcr.io/vimalshan/erp/admin-service:latest

docker run -d --name tour-booking-service --network erp-network -p 5045:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TOURDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=tour_user -e RabbitMQ__Password=tour_password \
  ghcr.io/vimalshan/erp/booking-service:latest

docker run -d --name tour-config-service --network erp-network -p 5057:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TOURDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=tour_user -e RabbitMQ__Password=tour_password \
  ghcr.io/vimalshan/erp/config-service:latest

docker run -d --name tour-tourplan-service --network erp-network -p 5143:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TOURDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=tour_user -e RabbitMQ__Password=tour_password \
  ghcr.io/vimalshan/erp/tourplan-service:latest

docker run -d --name tour-tour-service --network erp-network -p 5188:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TOURDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=tour_user -e RabbitMQ__Password=tour_password \
  ghcr.io/vimalshan/erp/tour-service:latest

docker run -d --name tour-transaction-service --network erp-network -p 5192:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TOURDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=tour_user -e RabbitMQ__Password=tour_password \
  ghcr.io/vimalshan/erp/transaction-service:latest

docker run -d --name tour-travel-service --network erp-network -p 5200:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=TOURDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=tour_user -e RabbitMQ__Password=tour_password \
  ghcr.io/vimalshan/erp/travel-service:latest

docker run -d --name tour-api-gateway --network erp-network -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
✈️ travelServices
Service	Host Port
api-gateway	5100
travel-request-api	5205
travel-transaction-api	5082
booking-api	5117
expense-api	5090
finance-api	5294
insurance-api	5179
masterdata-api	5166
agency-api	5000
admin-api	5001
bash
docker run -d --name travel-request-api --network erp-network -p 5205:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__TravelDb=Server=sqlserver,1433;Database=TRAVELDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/travelrequest-service:latest

docker run -d --name travel-transaction-api --network erp-network -p 5082:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__TravelDb=Server=sqlserver,1433;Database=TRAVELDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/traveltransaction-service:latest

docker run -d --name travel-booking-api --network erp-network -p 5117:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__TravelDb=Server=sqlserver,1433;Database=TRAVELDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/booking-service:latest

docker run -d --name travel-expense-api --network erp-network -p 5090:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__TravelDb=Server=sqlserver,1433;Database=TRAVELDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/expense-service:latest

docker run -d --name travel-finance-api --network erp-network -p 5294:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=FinanceServiceDb;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/finance-service:latest

docker run -d --name travel-insurance-api --network erp-network -p 5179:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__TravelDb=Server=sqlserver,1433;Database=TRAVELDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/insurance-service:latest

docker run -d --name travel-masterdata-api --network erp-network -p 5166:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=MasterDataDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/masterdata-service:latest

docker run -d --name travel-agency-api --network erp-network -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=AgencyServiceDb;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/agens-service:latest

docker run -d --name travel-admin-api --network erp-network -p 5001:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=AdminServiceDb;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=guest -e RabbitMQ__Password=guest \
  ghcr.io/vimalshan/erp/admin-service:latest

docker run -d --name travel-api-gateway --network erp-network -p 5100:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 \
  ghcr.io/vimalshan/erp/api-gateway:latest
🏭 wmsServices
Service	Host Port
api-gateway	5000
security	5249
warehouse	5217
racking	5241
employee	5014
product	5058
inventory	5059
supplier	5156
customer	5054
purchaseorder	5289
receiving	5215
salesorder	5075
shipment	5148
order	5117
fleet	5163
auditlog	5277
transactional	5080
bash
docker run -d --name wms-security --network erp-network -p 5249:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-security-service:latest

docker run -d --name wms-warehouse --network erp-network -p 5217:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-warehouse-service:latest

docker run -d --name wms-racking --network erp-network -p 5241:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-racking-service:latest

docker run -d --name wms-employee --network erp-network -p 5014:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-employee-service:latest

docker run -d --name wms-product --network erp-network -p 5058:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-product-service:latest

docker run -d --name wms-inventory --network erp-network -p 5059:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-inventory-service:latest

docker run -d --name wms-supplier --network erp-network -p 5156:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-supplier-service:latest

docker run -d --name wms-customer --network erp-network -p 5054:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-customer-service:latest

docker run -d --name wms-purchaseorder --network erp-network -p 5289:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-purchaseorder-service:latest

docker run -d --name wms-receiving --network erp-network -p 5215:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-receiving-service:latest

docker run -d --name wms-salesorder --network erp-network -p 5075:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-salesorder-service:latest

docker run -d --name wms-shipment --network erp-network -p 5148:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-shipment-service:latest

docker run -d --name wms-order --network erp-network -p 5117:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-order-service:latest

docker run -d --name wms-fleet --network erp-network -p 5163:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-fleet-service:latest

docker run -d --name wms-auditlog --network erp-network -p 5277:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-auditlog-service:latest

docker run -d --name wms-transactional --network erp-network -p 5080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=WMSDB;User Id=sa;******;TrustServerCertificate=True;" \
  -e RabbitMQ__HostName=rabbitmq -e RabbitMQ__UserName=wms_user -e RabbitMQ__Password=wms_password \
  ghcr.io/vimalshan/erp/wms-transaction-service:latest

docker run -d --name wms-api-gateway --network erp-network -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  ghcr.io/vimalshan/erp/api-gateway:latest
📋 Complete Port Reference Summary
Module	Gateway Port	Service Ports
AuthProvider	—	5200
adminServices	5000	5166, 5181–5186, 7136
aimsServices	5020	5010–5019
auditServices	5000	5001–5009
canteenServices	5200	5190–5197
cashServices	5000	5031, 5032, 5081, 5099, 5100, 5249, 5268
ddServices	5200	5000, 5049, 5081, 5100–5104, 5177–5178, 5210, 5224, 5237, 5258, 5261
healthServices	5600	5000, 5100, 5200, 5300, 5400, 5500, 7101
hrServicess	5310	5003, 5004, 5009, 5027, 5075, 5094, 5140, 5154, 5204, 5235, 5265
letServices	5400	5006, 5114, 5166, 5215–5216, 5279, 5320
loanServices	6100	5008, 5077, 5143, 5150, 5280, 5282, 5292
mainsparshServices	5100	(all internal — no external ports)
myworkServices	5000	5033, 5035, 5037, 5111, 5138, 5183, 5188, 5290
payServices	5100	5000, 5002, 5010, 5020, 5032, 5104
pfServices	5800	5004, 5068, 5079, 5090, 5125, 5149, 5160, 5171, 5225, 5278
sciServices	5200	5009, 5045, 5058, 5085, 5087, 5097, 5102, 5140, 5150, 5160, 5170, 5180, 5255, 5282, 5292
sparshServices	5200	5000, 5082, 5154, 5165, 5170
sscServices	5000	8080–8091
taskServices	5000	(microservices internal only)
tourServices	5000	5016, 5045, 5057, 5143, 5188, 5192, 5200
travelServices	5100	5000, 5001, 5082, 5090, 5117, 5166, 5179, 5205, 5294
wmsServices	5000	5014, 5054, 5058–5059, 5075, 5080, 5117, 5148, 5156, 5163, 5215, 5217, 5241, 5249, 5277, 5289
💡 Important Notes:

Replace all placeholder passwords with your actual values before running in production.
All services that share the same database (e.g., HEALTHDB, LETDB) must use the same SQL Server instance or a shared connection string.
All commands above already include `--network erp-network`. Create the network first: `docker network create erp-network`.
Infrastructure services (SQL Server, RabbitMQ) must be started and healthy before starting the microservices (see Prerequisites at top).
auditServices require `Jwt__Key`, `Jwt__Issuer`, and `Jwt__Audience` environment variables for JWT authentication.
All services use the SQL Server hostname `sqlserver` and RabbitMQ hostname `rabbitmq` — matching the container names on the `erp-network`.
