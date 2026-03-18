-- ==========================================
-- ReimbursementModule
-- Database: SRFSPARSHDB
-- Module Purpose: Reimbursement and Expense Management
-- Created: March 09, 2026
-- ==========================================

USE SRFSPARSHDB;
GO

-- Drop table if exists
IF OBJECT_ID('[REIM_TRAN]', 'U') IS NOT NULL DROP TABLE [REIM_TRAN];
GO

-- ==========================================
-- Table: REIM_TRAN - Reimbursement Transactions
-- Description: Employee reimbursement claims and transactions
-- ==========================================
CREATE TABLE [REIM_TRAN] (
    [REIM_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [REIM_REF_NO] VARCHAR(50) NOT NULL UNIQUE,
    [EMP_SYSID] BIGINT NOT NULL,
    [REIM_TYPE] VARCHAR(100), -- TRAVEL, MEAL, ACCOMMODATION, MISC, etc.
    [REIM_AMOUNT] DECIMAL(19,2) NOT NULL,
    [REIM_CURRENCY] VARCHAR(10) DEFAULT 'INR',
    [REIM_DATE] DATE NOT NULL,
    [EXPENSE_DATE] DATE NOT NULL,
    [DESCRIPTION] NVARCHAR(MAX),
    [LOCATION] VARCHAR(255),
    [REIM_STATUS] VARCHAR(20) DEFAULT 'DRAFT', -- DRAFT, SUBMITTED, APPROVED, REJECTED, PAID
    [APPROVAL_LEVEL] INT,
    [APPROVED_BY] BIGINT,
    [APPROVED_ON] DATETIME2(3),
    [REJECTION_REASON] NVARCHAR(MAX),
    [PAYMENT_DATE] DATE,
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [UPDATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3)
);
GO

-- Create Indexes
CREATE INDEX [IX_REIM_TRAN_EMP_SYSID] ON [REIM_TRAN]([EMP_SYSID]);
CREATE INDEX [IX_REIM_TRAN_STATUS] ON [REIM_TRAN]([REIM_STATUS]);
CREATE INDEX [IX_REIM_TRAN_TYPE] ON [REIM_TRAN]([REIM_TYPE]);
CREATE INDEX [IX_REIM_TRAN_DATE] ON [REIM_TRAN]([REIM_DATE]);
CREATE INDEX [IX_REIM_TRAN_REF_NO] ON [REIM_TRAN]([REIM_REF_NO]);
GO

PRINT 'ReimbursementModule_Schema created successfully.';
GO
