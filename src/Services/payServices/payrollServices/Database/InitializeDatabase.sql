-- ============================================
-- Payroll Database Manual Setup Script
-- Target Database: PAYDB on (localdb)\MSSQLLocalDB
-- ============================================

-- Create Database (if not exists)
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'PAYDB')
BEGIN
    CREATE DATABASE PAYDB;
    PRINT 'Database PAYDB created successfully';
END
GO

USE PAYDB;
GO

-- ============================================
-- Drop existing tables (if any) in reverse order of dependencies
-- ============================================
IF OBJECT_ID('[dbo].[PAY_TRANDET]', 'U') IS NOT NULL DROP TABLE [dbo].[PAY_TRANDET];
IF OBJECT_ID('[dbo].[PAY_ARR]', 'U') IS NOT NULL DROP TABLE [dbo].[PAY_ARR];
IF OBJECT_ID('[dbo].[PAYROLL_BATCH]', 'U') IS NOT NULL DROP TABLE [dbo].[PAYROLL_BATCH];

PRINT 'Existing tables dropped (if any)';
GO

-- ============================================
-- Create PAYROLL_BATCH Table
-- ============================================
CREATE TABLE [dbo].[PAYROLL_BATCH]
(
    [BATCH_ID] BIGINT NOT NULL PRIMARY KEY,
    [BATCH_MONTH] NVARCHAR(7) NOT NULL UNIQUE,
    [BATCH_STATUS] NVARCHAR(MAX) NOT NULL,
    [BATCH_CREATEDBY] BIGINT NOT NULL,
    [BATCH_CREATEDON] DATETIME2 NOT NULL,
    [BATCH_UPDATEDON] DATETIME2 NULL,
    [BATCH_UPDATEDBY] BIGINT NULL
);

-- Create index on BATCH_MONTH for unique constraint
CREATE UNIQUE INDEX [IX_PAYROLL_BATCH_BATCH_MONTH] ON [dbo].[PAYROLL_BATCH] ([BATCH_MONTH]);

PRINT 'PAYROLL_BATCH table created successfully';
GO

-- ============================================
-- Create PAY_TRANDET Table
-- ============================================
CREATE TABLE [dbo].[PAY_TRANDET]
(
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
    CONSTRAINT [FK_PAY_TRANDET_PAYROLL_BATCH_TRN_BATCHID] 
        FOREIGN KEY ([TRN_BATCHID]) REFERENCES [dbo].[PAYROLL_BATCH]([BATCH_ID]) ON DELETE CASCADE
);

-- Create indexes for performance
CREATE INDEX [IX_PAY_TRANDET_TRN_BATCHID] ON [dbo].[PAY_TRANDET] ([TRN_BATCHID]);
CREATE INDEX [IX_PAY_TRANDET_TRN_EMPSYSID_TRN_MONTH] ON [dbo].[PAY_TRANDET] ([TRN_EMPSYSID], [TRN_MONTH]);

PRINT 'PAY_TRANDET table created successfully';
GO

-- ============================================
-- Create PAY_ARR Table
-- ============================================
CREATE TABLE [dbo].[PAY_ARR]
(
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

-- Create indexes for performance
CREATE INDEX [IX_PAY_ARR_PAY_EMPSYSID] ON [dbo].[PAY_ARR] ([PAY_EMPSYSID]);
CREATE INDEX [IX_PAY_ARR_PAY_EMPSYSID_AR_DATE] ON [dbo].[PAY_ARR] ([PAY_EMPSYSID], [AR_DATE]);

PRINT 'PAY_ARR table created successfully';
GO

-- ============================================
-- Seed Data - PAYROLL_BATCH
-- ============================================
INSERT INTO [dbo].[PAYROLL_BATCH] ([BATCH_ID], [BATCH_MONTH], [BATCH_STATUS], [BATCH_CREATEDBY], [BATCH_CREATEDON], [BATCH_UPDATEDON], [BATCH_UPDATEDBY])
VALUES
    (1, '2024-01', 'Completed', 1, '2024-01-01 10:00:00', '2024-01-05 17:30:00', 1),
    (2, '2024-02', 'Completed', 1, '2024-02-01 10:00:00', '2024-02-05 17:30:00', 1),
    (3, '2024-03', 'Processing', 1, '2024-03-01 10:00:00', NULL, NULL);

PRINT 'PAYROLL_BATCH seed data inserted - 3 batches';
GO

-- ============================================
-- Seed Data - PAY_TRANDET
-- ============================================
INSERT INTO [dbo].[PAY_TRANDET] ([TRN_EMPSYSID], [TRN_BATCHID], [TRN_MONTH], [TRN_GROSS], [TRN_DEDUCTIONS], [TRN_NET], [TRN_STATUS], [TRN_CREATEDBY], [TRN_CREATEDON], [TRN_UPDATEDON], [TRN_UPDATEDBY])
VALUES
    -- Batch 1 (January 2024)
    (101, 1, '2024-01', 55000, 5500, 49500, 'Disbursed', 1, '2024-01-02 08:00:00', '2024-01-05 16:00:00', 1),
    (102, 1, '2024-01', 60000, 6000, 54000, 'Disbursed', 1, '2024-01-02 08:00:00', '2024-01-05 16:00:00', 1),
    (103, 1, '2024-01', 50000, 5000, 45000, 'Disbursed', 1, '2024-01-02 08:00:00', '2024-01-05 16:00:00', 1),
    -- Batch 2 (February 2024)
    (101, 2, '2024-02', 55000, 5500, 49500, 'Disbursed', 1, '2024-02-02 08:00:00', '2024-02-05 16:00:00', 1),
    (102, 2, '2024-02', 60000, 6000, 54000, 'Disbursed', 1, '2024-02-02 08:00:00', '2024-02-05 16:00:00', 1);

PRINT 'PAY_TRANDET seed data inserted - 5 transactions';
GO

-- ============================================
-- Seed Data - PAY_ARR
-- ============================================
INSERT INTO [dbo].[PAY_ARR] ([AR_ID], [PAY_EMPSYSID], [AR_AMOUNT], [AR_TYPE], [AR_DATE], [AR_DESCRIPTION], [AR_CREATEDBY], [AR_CREATEDON], [AR_APPROVEDON], [AR_APPROVEDBY])
VALUES
    -- Allowances
    (1, 101, 2000, 'Allowance', '2024-01-01', 'Performance Bonus', 1, '2024-01-01 09:00:00', '2024-01-01 10:00:00', 1),
    (2, 102, 1500, 'Allowance', '2024-01-01', 'HRA', 1, '2024-01-01 09:00:00', '2024-01-01 10:00:00', 1),
    -- Deductions
    (3, 101, 1000, 'Deduction', '2024-01-05', 'Loan EMI', 1, '2024-01-05 14:00:00', '2024-01-05 15:00:00', 1),
    (4, 103, 500, 'Deduction', '2024-01-05', 'Canteen Charges', 1, '2024-01-05 14:00:00', '2024-01-05 15:00:00', 1),
    -- Arrears
    (5, 102, 1200, 'Arrear', '2024-01-10', 'Previous Month Arrear', 1, '2024-01-10 11:00:00', '2024-01-10 12:00:00', 1);

PRINT 'PAY_ARR seed data inserted - 5 adjustments';
GO

-- ============================================
-- Verification Queries
-- ============================================
PRINT '';
PRINT 'Database Initialization Complete!';
PRINT '';
PRINT 'Summary:';

SELECT 'PAYROLL_BATCH' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[PAYROLL_BATCH]
UNION ALL
SELECT 'PAY_TRANDET', COUNT(*) FROM [dbo].[PAY_TRANDET]
UNION ALL
SELECT 'PAY_ARR', COUNT(*) FROM [dbo].[PAY_ARR];

PRINT '';
PRINT 'Schema verification:';

SELECT 'PAYROLL_BATCH' AS TableName, COUNT(*) AS ColumnCount FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PAYROLL_BATCH'
UNION ALL
SELECT 'PAY_TRANDET', COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PAY_TRANDET'
UNION ALL
SELECT 'PAY_ARR', COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PAY_ARR';
