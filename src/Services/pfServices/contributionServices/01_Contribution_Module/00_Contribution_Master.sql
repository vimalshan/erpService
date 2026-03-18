-- =========================================================================
-- CONTRIBUTION MODULE - Master Setup Script
-- Database: PFDB
-- Module: Contribution Management
-- Description: Execute this script to create all Contribution module objects
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

PRINT '========================================';
PRINT 'CONTRIBUTION MODULE - SETUP STARTED';
PRINT '========================================';
GO

-- Step 1: Create additional supporting tables
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'CONTRIBUTION_PROCESS_LOG' AND type = 'U')
BEGIN
    CREATE TABLE [CONTRIBUTION_PROCESS_LOG] (
        [LOG_ID] BIGINT IDENTITY(1,1) PRIMARY KEY,
        [LOG_TYPE] VARCHAR(20) NOT NULL,
        [LOG_MESSAGE] VARCHAR(MAX) NOT NULL,
        [PROCESS_DATE] DATETIME2(3) NOT NULL,
        [USER_ID] BIGINT NOT NULL
    );
    PRINT 'Created: CONTRIBUTION_PROCESS_LOG table';
END
GO

-- Step 2: Execute table creation script
PRINT 'Creating Contribution module tables...';
GO

-- Tables are assumed to be created via 01_Contribution_Tables.sql
-- Including references here for documentation

-- Step 3: Execute procedures and functions creation script
PRINT 'Creating Contribution module procedures and functions...';
GO

-- Procedures and functions are created via 02_Contribution_Procedures.sql

-- Step 4: Create Views for Reporting
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_ContributionSummary' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_ContributionSummary AS
    SELECT 
        cm.CONTRIBUTION_BATCH_NO,
        cm.CONTRIBUTION_TRUST_CODE,
        cm.CONTRIBUTION_PAYUNIT_CODE,
        cm.CONTRIBUTION_STATUS,
        cm.CONTRIBUTION_PAY_MONTHSTART,
        cm.CONTRIBUTION_PAY_MONTHEND,
        COUNT(DISTINCT cd.CONTRIBUTION_MEMBER_NO) AS MemberCount,
        ISNULL(SUM(cd.CONTRIBUTION_EE_AMOUNT), 0) AS TotalEEAmount,
        ISNULL(SUM(cd.CONTRIBUTION_ER_AMOUNT), 0) AS TotalERAmount,
        ISNULL(SUM(cd.CONTRIBUTION_EE_AMOUNT) + SUM(cd.CONTRIBUTION_ER_AMOUNT), 0) AS TotalAmount
    FROM dbo.CONTRIBUTION_MAIN cm
    LEFT JOIN dbo.CONTRIBUTION_DETAILS cd ON cm.CONTRIBUTION_BATCH_NO = cd.CONTRIBUTION_BATCH_NO
    GROUP BY 
        cm.CONTRIBUTION_BATCH_NO,
        cm.CONTRIBUTION_TRUST_CODE,
        cm.CONTRIBUTION_PAYUNIT_CODE,
        cm.CONTRIBUTION_STATUS,
        cm.CONTRIBUTION_PAY_MONTHSTART,
        cm.CONTRIBUTION_PAY_MONTHEND;
    PRINT 'Created: vw_ContributionSummary view';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_SuperannuationSummary' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_SuperannuationSummary AS
    SELECT 
        sb.SN_BATCH_NO,
        sb.SN_TRUST_CODE,
        sb.SN_PAYUNIT_CODE,
        sb.SN_STATUS,
        sb.SN_PAY_MONTHSTART,
        sb.SN_PAY_MONTHEND,
        COUNT(DISTINCT sc.SN_PIN_NUM) AS EmployeeCount,
        ISNULL(SUM(CAST(sb.SN_CON_AMT AS DECIMAL(19,0))), 0) AS TotalContribution,
        sb.SN_PAY_DATE
    FROM dbo.SUPERANN_BATCH sb
    LEFT JOIN dbo.SUPERANN_CONTRIBUTION sc ON sb.SN_BATCH_NO = sc.SN_BAT_NO
    GROUP BY 
        sb.SN_BATCH_NO,
        sb.SN_TRUST_CODE,
        sb.SN_PAYUNIT_CODE,
        sb.SN_STATUS,
        sb.SN_PAY_MONTHSTART,
        sb.SN_PAY_MONTHEND,
        sb.SN_PAY_DATE;
    PRINT 'Created: vw_SuperannuationSummary view';
END
GO

-- Step 5: Create Stored Procedures if they don't exist
PRINT 'Validating Contribution module procedures...';
GO

-- Grant basic permissions (adjust as needed)
-- GRANT EXECUTE ON dbo.usp_ProcessMonthlyPFContribution TO [your_app_role];
-- GRANT SELECT ON dbo.vw_ContributionSummary TO [your_app_role];

PRINT '========================================';
PRINT 'CONTRIBUTION MODULE - SETUP COMPLETED';
PRINT '========================================';
GO

-- Summary Report
SELECT 
    'Tables' AS ObjectType,
    COUNT(*) AS Count
FROM sys.objects
WHERE schema_id = SCHEMA_ID('dbo')
  AND type = 'U'
  AND name IN (
    'CONTRIBUTION_MAIN', 'CONTRIBUTION_DETAILS', 'CONTRIBUTION_BREAKUP', 
    'CONTRIBUTION_TEMP', 'SUPERANN_CONTRIBUTION', 'SUPERANN_BATCH', 
    'SUPERANN_BREAKUP', 'SUPERANN_RATE', 'SUPERANN_TRUSTNAME', 'CONTRIBUTION_PROCESS_LOG'
  )
UNION ALL
SELECT 
    'Views' AS ObjectType,
    COUNT(*) AS Count
FROM sys.objects
WHERE schema_id = SCHEMA_ID('dbo')
  AND type = 'V'
  AND name IN ('vw_ContributionSummary', 'vw_SuperannuationSummary')
UNION ALL
SELECT 
    'Procedures' AS ObjectType,
    COUNT(*) AS Count
FROM sys.objects
WHERE schema_id = SCHEMA_ID('dbo')
  AND type = 'P'
  AND name LIKE 'usp_ProcessMonthlyPFContribution%'
    OR name LIKE 'usp_ValidateContributionDetails%'
    OR name LIKE 'usp_PostContributionBatch%'
    OR name LIKE 'usp_GetContributionSummary%'
UNION ALL
SELECT 
    'Functions' AS ObjectType,
    COUNT(*) AS Count
FROM sys.objects
WHERE schema_id = SCHEMA_ID('dbo')
  AND type = 'FN'
  AND name IN ('fn_CalculatePFContribution');
GO
