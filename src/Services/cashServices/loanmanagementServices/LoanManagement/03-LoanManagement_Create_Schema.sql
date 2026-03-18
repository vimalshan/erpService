-- ==========================================
-- Module: LoanManagement
-- Purpose: Loan Processing and Repayment Schedule
-- Created: March 9, 2026
-- Database: CASHDB
-- ==========================================

USE CASHDB;
GO

-- =====================================================
-- CREATE TABLES FOR LOAN MANAGEMENT MODULE
-- =====================================================

-- Table: LOAN_MAIN - Loan Master
CREATE TABLE [LOAN_MAIN] (
    [LOAN_ID] DECIMAL(38) NOT NULL  -- Loan ID,
    [LOAN_KEY] VARCHAR(15) NOT NULL  -- Loan Key,
    [LOAN_ORGID] DECIMAL(38) NOT NULL  -- Organization ID,
    [LOAN_ORGCURR] DECIMAL(38) NULL  -- Organization Currency Code,
    [LOAN_CURR] DECIMAL(38) NULL  -- Loan Currency Code,
    [LOAN_DATE] DATETIME2(3) NOT NULL  -- Loan Date,
    [LOAN_TYPEID] DECIMAL(38) NOT NULL  -- Loan Type,
    [LOAN_BANKID] DECIMAL(38) NOT NULL  -- Bank,
    [LOAN_CREATEDBY] DECIMAL(38) NOT NULL  -- Created By,
    [LOAN_CREATEDON] DATETIME2(3) NOT NULL  -- Created On,
    [LOAN_MODIFIEDBY] DECIMAL(38) NULL  -- Modified By,
    [LOAN_MODIFIEDON] DATETIME2(3) NULL  -- Modified On,
    [LOAN_AMOUNT] DECIMAL(38) NOT NULL  -- Loan Amount,
    [LOAN_STATUS] CHAR(1) NULL,
    CONSTRAINT [PK_LOAN_MAIN] PRIMARY KEY ([LOAN_ID])
);

-- Table: LOAN_DISBSCH - Loan Disbursement Schedule
CREATE TABLE [LOAN_DISBSCH] (
    [DISB_ID] BIGINT NOT NULL  -- Disbursement ID,
    [DISB_LOANID] BIGINT NULL  -- Loan ID,
    [DISB_DATE] DATETIME2(3) NULL  -- Disbursement Date,
    [DISB_AMOUNT] DECIMAL(19,0) NULL  -- Amount,
    [DISB_EXCRATE] DECIMAL(19,0) NULL  -- Exchange Rate,
    [DISB_EXCAMT] DECIMAL(19,0) NULL  -- Amount Accounted,
    [DISB_MODIFIEDBY] BIGINT NULL  -- Modified By,
    [DISB_MODIFIEDON] DATETIME2(3) NULL  -- Modified On,
    CONSTRAINT [PK_LOAN_DISBSCH] PRIMARY KEY ([DISB_ID]),
    CONSTRAINT [FK_LOAN_DISBSCH_MAIN] FOREIGN KEY ([DISB_LOANID]) REFERENCES [LOAN_MAIN]([LOAN_ID])
);

-- Table: LOAN_INTEREST - Loan Interest Configuration
CREATE TABLE [LOAN_INTEREST] (
    [INT_ID] BIGINT NOT NULL  -- Interest ID,
    [INT_LOANID] BIGINT NULL  -- Loan ID,
    [INT_RATETYPE] CHAR(2) NULL  -- Rate Type(FX/FL),
    [INT_PER] DECIMAL(19,0) NULL  -- Fixed Percentage / Float Spread %,
    [INT_FLOATTYPEID] BIGINT NULL  -- Float Type,
    [INT_EFFDATE] DATETIME2(3) NULL  -- Effective Date,
    [INT_CLSDATE] DATETIME2(3) NULL  -- Closure Date,
    CONSTRAINT [PK_LOAN_INTEREST] PRIMARY KEY ([INT_ID]),
    CONSTRAINT [FK_LOAN_INTEREST_MAIN] FOREIGN KEY ([INT_LOANID]) REFERENCES [LOAN_MAIN]([LOAN_ID])
);

-- Table: LOAN_REPAYSCH - Loan Repayment Schedule
CREATE TABLE [LOAN_REPAYSCH] (
    [REPAY_ID] BIGINT NOT NULL  -- Repayment ID,
    [REPAY_LOANID] BIGINT NULL  -- Repayment Loan ID,
    [REPAY_DATE] DATETIME2(3) NULL  -- Repayment Date,
    [REPAY_AMT] DECIMAL(19,0) NULL  -- Repayment Amount,
    [REPAY_FLAG] CHAR(1) NULL  -- O - Original ; A - Ammended,
    [REPAY_MODIFIEDON] DATETIME2(3) NULL  -- Modified On,
    [REPAY_MODIFIEDBY] BIGINT NULL  -- Modified By,
    CONSTRAINT [PK_LOAN_REPAYSCH] PRIMARY KEY ([REPAY_ID]),
    CONSTRAINT [FK_LOAN_REPAYSCH_MAIN] FOREIGN KEY ([REPAY_LOANID]) REFERENCES [LOAN_MAIN]([LOAN_ID])
);

-- =====================================================
-- CREATE INDEXES
-- =====================================================

CREATE INDEX [IX_LOAN_MAIN_ORGID] ON [LOAN_MAIN] ([LOAN_ORGID]);
CREATE INDEX [IX_LOAN_MAIN_DATE] ON [LOAN_MAIN] ([LOAN_DATE]);
CREATE INDEX [IX_LOAN_DISBSCH_LOANID] ON [LOAN_DISBSCH] ([DISB_LOANID]);
CREATE INDEX [IX_LOAN_DISBSCH_DATE] ON [LOAN_DISBSCH] ([DISB_DATE]);
CREATE INDEX [IX_LOAN_INTEREST_LOANID] ON [LOAN_INTEREST] ([INT_LOANID]);
CREATE INDEX [IX_LOAN_REPAYSCH_LOANID] ON [LOAN_REPAYSCH] ([REPAY_LOANID]);
CREATE INDEX [IX_LOAN_REPAYSCH_DATE] ON [LOAN_REPAYSCH] ([REPAY_DATE]);

-- =====================================================
-- VERIFICATION
-- =====================================================

PRINT 'LoanManagement Module Schema created successfully.';
GO

-- Verify table creation
IF OBJECT_ID('LOAN_MAIN', 'U') IS NOT NULL
    PRINT 'Table LOAN_MAIN: OK'
ELSE
    PRINT 'Table LOAN_MAIN: FAILED'
GO

IF OBJECT_ID('LOAN_DISBSCH', 'U') IS NOT NULL
    PRINT 'Table LOAN_DISBSCH: OK'
ELSE
    PRINT 'Table LOAN_DISBSCH: FAILED'
GO

IF OBJECT_ID('LOAN_INTEREST', 'U') IS NOT NULL
    PRINT 'Table LOAN_INTEREST: OK'
ELSE
    PRINT 'Table LOAN_INTEREST: FAILED'
GO

IF OBJECT_ID('LOAN_REPAYSCH', 'U') IS NOT NULL
    PRINT 'Table LOAN_REPAYSCH: OK'
ELSE
    PRINT 'Table LOAN_REPAYSCH: FAILED'
GO
