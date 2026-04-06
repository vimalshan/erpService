-- ============================================================
-- ERP Microservice Database Initialization Script
-- Creates all databases and base schemas for Docker/Production
-- ============================================================

-- ============================================================
-- 1. Create Databases
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'PAYDB')
BEGIN
    CREATE DATABASE PAYDB;
    PRINT '>>> Created database: PAYDB';
END
ELSE
    PRINT '>>> Database PAYDB already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'TaxService')
BEGIN
    CREATE DATABASE TaxService;
    PRINT '>>> Created database: TaxService';
END
ELSE
    PRINT '>>> Database TaxService already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'PayTransactionalService')
BEGIN
    CREATE DATABASE PayTransactionalService;
    PRINT '>>> Created database: PayTransactionalService';
END
ELSE
    PRINT '>>> Database PayTransactionalService already exists';
GO

-- ============================================================
-- 2. PAYDB Schema (Employee, HR, FAQ, Payroll)
-- ============================================================
USE PAYDB;
GO

-- Employee / Cost Centre
IF OBJECT_ID('dbo.EMP_COSTCENTREDET', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[EMP_COSTCENTREDET] (
        [FK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: EMP_COSTCENTREDET';
END
GO

-- Conditional Deduction Master
IF OBJECT_ID('dbo.CONDED_MAST', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[CONDED_MAST] (
        [FK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: CONDED_MAST';
END
GO

-- Professional Rate Sitemap
IF OBJECT_ID('dbo.PROFRATE_SITEMAP', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PROFRATE_SITEMAP] (
        [FK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: PROFRATE_SITEMAP';
END
GO

-- Employee Increment CTC
IF OBJECT_ID('dbo.EMPLOYEE_INCCTC', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[EMPLOYEE_INCCTC] (
        [FK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: EMPLOYEE_INCCTC';
END
GO

-- Tax Marginal Details
IF OBJECT_ID('dbo.TAX_MARDET', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[TAX_MARDET] (
        [FK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: TAX_MARDET';
END
GO

-- FAQ Grade
IF OBJECT_ID('dbo.FAQ_GRADE', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[FAQ_GRADE] (
        [PK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: FAQ_GRADE';
END
GO

-- FAQ Question
IF OBJECT_ID('dbo.FAQ_QUESTION', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[FAQ_QUESTION] (
        [PK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: FAQ_QUESTION';
END
GO

-- FAQ Answers
IF OBJECT_ID('dbo.FAQ_ANSWERS', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[FAQ_ANSWERS] (
        [PK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: FAQ_ANSWERS';
END
GO

-- Payroll Batch Revoke
IF OBJECT_ID('dbo.PAYROLL_BATCHREVOKE', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PAYROLL_BATCHREVOKE] (
        [PK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: PAYROLL_BATCHREVOKE';
END
GO

-- HR International Language Code
IF OBJECT_ID('dbo.HR_INTLANGUAGECODE', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[HR_INTLANGUAGECODE] (
        [UK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: HR_INTLANGUAGECODE';
END
GO

-- Employee PF Specific
IF OBJECT_ID('dbo.EMPPF_EMPSPECIFIC', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[EMPPF_EMPSPECIFIC] (
        [PK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: EMPPF_EMPSPECIFIC';
END
GO

-- Pay Adjustment Work
IF OBJECT_ID('dbo.PAY_ADJWRK', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PAY_ADJWRK] (
        [FK] VARCHAR(255) NULL
    );
    PRINT '>>> Created table: PAY_ADJWRK';
END
GO

-- ============================================================
-- 3. Payroll Tables
-- ============================================================
IF OBJECT_ID('dbo.PAYROLL_BATCH', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PAYROLL_BATCH] (
        [BATCH_ID] BIGINT NOT NULL PRIMARY KEY,
        [BATCH_MONTH] NVARCHAR(7) NOT NULL,
        [BATCH_STATUS] NVARCHAR(MAX) NOT NULL,
        [BATCH_CREATEDBY] BIGINT NOT NULL,
        [BATCH_CREATEDON] DATETIME2 NOT NULL,
        [BATCH_UPDATEDON] DATETIME2 NULL,
        [BATCH_UPDATEDBY] BIGINT NULL
    );
    CREATE UNIQUE INDEX [IX_PAYROLL_BATCH_BATCH_MONTH] ON [dbo].[PAYROLL_BATCH] ([BATCH_MONTH]);
    PRINT '>>> Created table: PAYROLL_BATCH';
END
GO

IF OBJECT_ID('dbo.PAY_TRANDET', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PAY_TRANDET] (
        [TRN_ID] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
        [TRN_EMPSYSID] BIGINT NOT NULL,
        [TRN_BATCHID] BIGINT NOT NULL,
        [TRN_MONTH] NVARCHAR(7) NOT NULL,
        [TRN_GROSS] DECIMAL(19,0) NOT NULL,
        [TRN_DEDUCTIONS] DECIMAL(19,0) NOT NULL,
        [TRN_NET] DECIMAL(19,0) NOT NULL,
        [TRN_STATUS] NVARCHAR(MAX) NOT NULL,
        [TRN_CREATEDBY] BIGINT NOT NULL,
        [TRN_CREATEDON] DATETIME2 NOT NULL,
        [TRN_UPDATEDON] DATETIME2 NULL,
        [TRN_UPDATEDBY] BIGINT NULL,
        CONSTRAINT [FK_PAY_TRANDET_PAYROLL_BATCH] FOREIGN KEY ([TRN_BATCHID])
            REFERENCES [dbo].[PAYROLL_BATCH]([BATCH_ID]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_PAY_TRANDET_TRN_BATCHID] ON [dbo].[PAY_TRANDET] ([TRN_BATCHID]);
    CREATE INDEX [IX_PAY_TRANDET_TRN_EMPSYSID_MONTH] ON [dbo].[PAY_TRANDET] ([TRN_EMPSYSID], [TRN_MONTH]);
    PRINT '>>> Created table: PAY_TRANDET';
END
GO

IF OBJECT_ID('dbo.PAY_ARR', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PAY_ARR] (
        [AR_ID] BIGINT NOT NULL PRIMARY KEY,
        [PAY_EMPSYSID] BIGINT NOT NULL,
        [AR_AMOUNT] DECIMAL(19,0) NOT NULL,
        [AR_TYPE] NVARCHAR(MAX) NOT NULL,
        [AR_DATE] DATETIME2 NOT NULL,
        [AR_DESCRIPTION] NVARCHAR(500) NULL,
        [AR_CREATEDBY] BIGINT NOT NULL,
        [AR_CREATEDON] DATETIME2 NOT NULL,
        [AR_APPROVEDON] DATETIME2 NULL,
        [AR_APPROVEDBY] BIGINT NULL
    );
    CREATE INDEX [IX_PAY_ARR_PAY_EMPSYSID] ON [dbo].[PAY_ARR] ([PAY_EMPSYSID]);
    PRINT '>>> Created table: PAY_ARR';
END
GO

-- ============================================================
-- 4. Stored Procedures & Functions
-- ============================================================
IF OBJECT_ID('dbo.fn_CalculateNetSalary', 'FN') IS NULL
BEGIN
    EXEC('
    CREATE FUNCTION dbo.fn_CalculateNetSalary
    (
        @p_EmpSysID BIGINT,
        @p_MonthYear VARCHAR(7)
    )
    RETURNS DECIMAL(19,0)
    AS
    BEGIN
        DECLARE @BasicPay DECIMAL(19,0);
        DECLARE @Allowances DECIMAL(19,0);
        DECLARE @Deductions DECIMAL(19,0);
        DECLARE @NetSalary DECIMAL(19,0);

        SELECT TOP 1 @BasicPay = EIC_BASIC_SALARY
        FROM EMPLOYEE_INCCTC WHERE EIC_EMPSYSID = @p_EmpSysID;

        SELECT @Allowances = ISNULL(SUM(AR_AMOUNT), 0)
        FROM PAY_ARR
        WHERE PAY_EMPSYSID = @p_EmpSysID AND PAY_TYPE = ''A''
          AND YEAR(PAY_DATE) = CAST(LEFT(@p_MonthYear, 4) AS INT)
          AND MONTH(PAY_DATE) = CAST(RIGHT(@p_MonthYear, 2) AS INT);

        SELECT @Deductions = ISNULL(SUM(AR_AMOUNT), 0)
        FROM PAY_ARR
        WHERE PAY_EMPSYSID = @p_EmpSysID AND PAY_TYPE = ''D''
          AND YEAR(PAY_DATE) = CAST(LEFT(@p_MonthYear, 4) AS INT)
          AND MONTH(PAY_DATE) = CAST(RIGHT(@p_MonthYear, 2) AS INT);

        SET @NetSalary = ISNULL(@BasicPay, 0) + @Allowances - ISNULL(@Deductions, 0);
        RETURN ISNULL(@NetSalary, 0);
    END
    ');
    PRINT '>>> Created function: fn_CalculateNetSalary';
END
GO

IF OBJECT_ID('dbo.fn_GetTaxableIncome', 'FN') IS NULL
BEGIN
    EXEC('
    CREATE FUNCTION dbo.fn_GetTaxableIncome
    (
        @p_EmpSysID BIGINT,
        @p_FinancialYear INT
    )
    RETURNS DECIMAL(19,0)
    AS
    BEGIN
        DECLARE @GrossIncome DECIMAL(19,0);
        DECLARE @StandardDeduction DECIMAL(19,0) = 50000;
        DECLARE @TaxableIncome DECIMAL(19,0);

        SELECT @GrossIncome = ISNULL(SUM(EIC_GROSS_CTC), 0) * 12
        FROM EMPLOYEE_INCCTC WHERE EIC_EMPSYSID = @p_EmpSysID;

        SET @TaxableIncome = @GrossIncome - @StandardDeduction;
        RETURN CASE WHEN @TaxableIncome < 0 THEN 0 ELSE @TaxableIncome END;
    END
    ');
    PRINT '>>> Created function: fn_GetTaxableIncome';
END
GO

-- ============================================================
-- 5. HR Seed Data
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'HR_Department')
BEGIN
    PRINT '>>> HR_Department table not found — EF migrations will create it. Skipping HR seed data.';
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM HR_Department WHERE DepartmentCode = 'HR')
    BEGIN
        INSERT INTO HR_Department (Id, DepartmentCode, DepartmentName, Description, ManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
        VALUES (CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000001'), 'HR', 'Human Resources', 'Human Resources Department', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
    END

    IF NOT EXISTS (SELECT 1 FROM HR_Department WHERE DepartmentCode = 'IT')
    BEGIN
        INSERT INTO HR_Department (Id, DepartmentCode, DepartmentName, Description, ManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
        VALUES (CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000002'), 'IT', 'Information Technology', 'IT Department', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
    END

    IF NOT EXISTS (SELECT 1 FROM HR_Department WHERE DepartmentCode = 'FIN')
    BEGIN
        INSERT INTO HR_Department (Id, DepartmentCode, DepartmentName, Description, ManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
        VALUES (CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000003'), 'FIN', 'Finance', 'Finance Department', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
    END

    IF NOT EXISTS (SELECT 1 FROM HR_Department WHERE DepartmentCode = 'OPS')
    BEGIN
        INSERT INTO HR_Department (Id, DepartmentCode, DepartmentName, Description, ManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
        VALUES (CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000004'), 'OPS', 'Operations', 'Operations Department', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
    END

    PRINT '>>> HR seed data applied';
END
GO

-- ============================================================
-- 6. TaxService Seed Data
-- ============================================================
USE TaxService;
GO

-- Note: ConditionalMasters table created by EF migrations.
-- Seed data will be applied after EF migrations run.
PRINT '>>> TaxService database ready for EF migrations';
GO

-- ============================================================
-- 7. PayTransactionalService
-- ============================================================
USE PayTransactionalService;
GO

PRINT '>>> PayTransactionalService database ready for EF migrations';
GO

PRINT '============================================================';
PRINT '>>> All databases initialized successfully';
PRINT '============================================================';
GO
