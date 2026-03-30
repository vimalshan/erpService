-- HR Microservices Database Initialization Script
-- This script creates all necessary databases for the HR microservices

-- Set the database context
USE master;
GO

-- Create databases if they don't exist
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'AlertsNotificationsDB')
    CREATE DATABASE AlertsNotificationsDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'CompensationBenefitsDB')
    CREATE DATABASE CompensationBenefitsDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'EmployeeManagementDB')
    CREATE DATABASE EmployeeManagementDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'EmployeeRelationsDB')
    CREATE DATABASE EmployeeRelationsDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'ExitManagementDB')
    CREATE DATABASE ExitManagementDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'OrganizationStructureDB')
    CREATE DATABASE OrganizationStructureDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'RecruitmentDB')
    CREATE DATABASE RecruitmentDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'TimeAttendanceDB')
    CREATE DATABASE TimeAttendanceDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'TrainingDevelopmentDB')
    CREATE DATABASE TrainingDevelopmentDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'UserSecurityDB')
    CREATE DATABASE UserSecurityDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'EmployeeTransactionsDB')
    CREATE DATABASE EmployeeTransactionsDB;
GO

-- Verify databases were created
SELECT name FROM sys.databases 
WHERE name IN (
    'AlertsNotificationsDB',
    'CompensationBenefitsDB',
    'EmployeeManagementDB',
    'EmployeeRelationsDB',
    'ExitManagementDB',
    'OrganizationStructureDB',
    'RecruitmentDB',
    'TimeAttendanceDB',
    'TrainingDevelopmentDB',
    'UserSecurityDB',
    'EmployeeTransactionsDB'
)
ORDER BY name;

-- Log completion
PRINT 'Database initialization completed successfully at ' + CONVERT(VARCHAR(30), GETUTCDATE(), 121);
