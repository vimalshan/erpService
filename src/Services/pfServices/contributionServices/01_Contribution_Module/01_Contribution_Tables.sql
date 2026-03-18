-- =========================================================================
-- CONTRIBUTION MODULE - Database Tables
-- Database: PFDB
-- Module: Contribution Management
-- Description: Manages employee and employer PF contributions
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- =========================================================================
-- 1. CONTRIBUTION_MAIN - Main Contribution Batch Records
-- =========================================================================
CREATE TABLE [CONTRIBUTION_MAIN] (
    [CONTRIBUTION_BATCH_NO] BIGINT NOT NULL  -- Contribution Batch No,
    [CONTRIBUTION_TRUST_CODE] CHAR(3) NOT NULL  -- Trust Code,
    [CONTRIBUTION_CATEGORY] CHAR(3) NOT NULL  -- Contribution Category,
    [CONTRIBUTION_PAYUNIT_CODE] CHAR(3) NOT NULL  -- Unit Code,
    [CONTRIBUTION_PAY_MONTHSTART] DATETIME2(3) NOT NULL  -- Pay Month Start Date,
    [CONTRIBUTION_PAY_MONTHEND] DATETIME2(3) NOT NULL  -- Pay Month End Date,
    [CONTRIBUTION_STATUS] CHAR(2) NOT NULL  -- PAY STATUS,
    [CONTRIBUTION_JVNO] DECIMAL(38) NULL  -- JV Transaction No,
    [CONTRIBUTION_REC_ACTRAN_NO] DECIMAL(38) NULL  -- Receipt AC Transaction No,
    [CONTRIBUTION_ENT_ON] DATETIME2(3) NULL,
    [CONTRIBUTION_REFNO] BIGINT NOT NULL,
    CONSTRAINT [PK_CONTRIBUTION_MAIN] PRIMARY KEY ([CONTRIBUTION_BATCH_NO])
);
GO

-- =========================================================================
-- 2. CONTRIBUTION_DETAILS - Detailed Contribution Records per Member
-- =========================================================================
CREATE TABLE [CONTRIBUTION_DETAILS] (
    [CONTRIBUTION_BATCH_NO] DECIMAL(38) NOT NULL  -- Contribution Batch No,
    [CONTRIBUTION_ID] DECIMAL(38) NOT NULL  -- Contribution Serial No,
    [CONTRIBUTION_MEMBER_NO] DECIMAL(38) NOT NULL  -- Member No,
    [CONTRIBUTION_UNIT_CODE] CHAR(1) NOT NULL  -- Current Unit Code,
    [CONTRIBUTION_EMPLOYEE_NO] DECIMAL(38) NOT NULL  -- Current Employee No,
    [CONTRIBUTION_REFERENCE_NO] DECIMAL(38) NULL  -- Reference No,
    [CONTRIBUTION_REFERENCE_REMARKS] VARCHAR(255) NULL  -- Remarks,
    [CONTRIBUTION_BASIC_AMOUNT] DECIMAL(38) NOT NULL  -- Basic Amount,
    [CONTRIBUTION_FPSBASIC_AMOUNT] DECIMAL(38) NOT NULL  -- FPS Basic Amount,
    [CONTRIBUTION_EE_AMOUNT] DECIMAL(38) NOT NULL  -- Employee Contribution,
    [CONTRIBUTION_ER_AMOUNT] DECIMAL(38) NOT NULL  -- Employer Contribution,
    [CONTRIBUTION_VE_AMOUNT] DECIMAL(38) NOT NULL  -- Voluntary Contribution,
    [CONTRIBUTION_FP_AMOUNT] DECIMAL(38) NOT NULL  -- FPF Amount,
    [CONTRIBUTION_LOAN_PRINCIPAL] DECIMAL(38) NOT NULL  -- Loan Principal Recovery,
    [CONTRIBUTION_LOAN_INTEREST] DECIMAL(38) NOT NULL  -- Loan Interest Recovery,
    [CONTRIBUTION_ENT_BY_USER_ID] VARCHAR(255) NOT NULL  -- Entered By User ID,
    [CONTRIBUTION_ENT_EMP_SYS_ID] DECIMAL(38) NOT NULL  -- Entered By User No,
    [CONTRIBUTION_ENT_ON] DATETIME2(3) NOT NULL  -- Entered On,
    [CONTRIBUTION_TYPE_CODE] CHAR(1) NOT NULL  -- Type Code,
    [CONTRIBUTION_EMP_SYSID] DECIMAL(38) NULL,
    CONSTRAINT [PK_CONTRIBUTION_DETAILS] PRIMARY KEY ([CONTRIBUTION_ID])
);
GO

-- =========================================================================
-- 3. CONTRIBUTION_BREAKUP - Contribution Amount Breakup
-- =========================================================================
CREATE TABLE [CONTRIBUTION_BREAKUP] (
    [CONTRIBUTION_BATCH_NO] BIGINT NOT NULL  -- Contribution Batch No,
    [CONTRIBUTION_ID] BIGINT NOT NULL  -- Contribution Serial No,
    [CONTRIBUTION_PAYTRANNO] BIGINT NOT NULL  -- Contribution Pay Transaction No,
    [CONTRIBUTION_EDCODE] CHAR(6) NOT NULL  -- Contribution ED Code,
    [CONTRIBUTION_PAYAMOUNT] DECIMAL(19,0) NOT NULL  -- Contribution Salary Gross Amount,
    [CONTRIBUTION_EEAMOUNT] DECIMAL(19,0) NOT NULL  -- Contribution EE Amount,
    [CONTRIBUTION_ERAMOUNT] DECIMAL(19,0) NOT NULL  -- Contribution ER Amount,
    [CONTRIBUTION_COM_COD] CHAR(3) NOT NULL,
    CONSTRAINT [PK_CONTRIBUTION_BREAKUP] PRIMARY KEY ([CONTRIBUTION_PAYTRANNO], [CONTRIBUTION_BATCH_NO], [CONTRIBUTION_ID])
);
GO

-- =========================================================================
-- 4. CONTRIBUTION_TEMP - Temporary Contribution Staging Table
-- =========================================================================
CREATE TABLE [CONTRIBUTION_TEMP] (
    [CONTRIBUTION_BATCH_NO] BIGINT NOT NULL  -- Contribution Batch No,
    [CONTRIBUTION_ID] BIGINT NOT NULL  -- Contribution Serial No,
    [CONTRIBUTION_MEMBER_NO] BIGINT NOT NULL  -- Member No,
    [CONTRIBUTION_UNIT_CODE] CHAR(3) NOT NULL  -- Current Unit Code,
    [CONTRIBUTION_EMPLOYEE_NO] INT NOT NULL  -- Current Employee No,
    [CONTRIBUTION_REFERENCE_NO] INT NULL  -- Reference No,
    [CONTRIBUTION_REFERENCE_REMARKS] VARCHAR(200) NULL  -- Remarks,
    [CONTRIBUTION_BASIC_AMOUNT] DECIMAL(19,0) NOT NULL  -- Basic Amount,
    [CONTRIBUTION_FPSBASIC_AMOUNT] DECIMAL(19,0) NOT NULL  -- FPS Basic Amount,
    [CONTRIBUTION_EE_AMOUNT] DECIMAL(19,0) NOT NULL  -- Employee Contribution,
    [CONTRIBUTION_ER_AMOUNT] DECIMAL(19,0) NOT NULL  -- Employer Contribution,
    [CONTRIBUTION_VE_AMOUNT] DECIMAL(19,0) NOT NULL  -- Voluntary Contribution,
    [CONTRIBUTION_FP_AMOUNT] DECIMAL(19,0) NOT NULL  -- FPF Amount,
    [CONTRIBUTION_LOAN_PRINCIPAL] DECIMAL(19,0) NOT NULL  -- Loan Principal Recovery,
    [CONTRIBUTION_LOAN_INTEREST] DECIMAL(19,0) NOT NULL  -- Loan Interest Recovery,
    [CONTRIBUTION_ENT_BY_USER_ID] VARCHAR(25) NOT NULL  -- Entered By User ID,
    [CONTRIBUTION_ENT_EMP_SYS_ID] INT NOT NULL  -- Entered By User No,
    [CONTRIBUTION_ENT_ON] DATETIME2(3) NOT NULL  -- Entered On,
    [CONTRIBUTION_TYPE_CODE] CHAR(1) NOT NULL  -- Type Code,
    CONSTRAINT [PK_CONTRIBUTION_TEMP] PRIMARY KEY ([CONTRIBUTION_ID])
);
GO

-- =========================================================================
-- 5. SUPERANN_CONTRIBUTION - Superannuation Contribution Records
-- =========================================================================
CREATE TABLE [SUPERANN_CONTRIBUTION] (
    [SN_SLR_NUM] BIGINT NOT NULL,
    [SN_FIN_YER] BIGINT NULL  -- Financial Year,
    [SN_PIN_NUM] DECIMAL(38) NULL  -- PIN No,
    [SN_EMP_NAM] VARCHAR(100) NULL  -- Emp Name,
    [SN_FUD_NUM] DECIMAL(38) NULL  -- Fund code,
    [SN_CON_DAT] DATETIME2(3) NULL  -- Month,
    [SN_UNT_NOS] DECIMAL(19,0) NULL  -- Units,
    [SN_NAV_AMT] DECIMAL(19,0) NULL  -- NAV,
    [SN_CON_AMT] DECIMAL(19,0) NULL  -- Contribution,
    [SN_CON_TYP] CHAR(1) NULL  -- Contribution type (C) - (O),
    [SN_ENT_DAT] DATETIME2(3) NULL  -- Uploaded On,
    CONSTRAINT [PK_SUPERANN_CONTRIBUTION] PRIMARY KEY ([SN_SLR_NUM])
);
GO

-- =========================================================================
-- 6. SUPERANN_BATCH - Superannuation Batch Processing Records
-- =========================================================================
CREATE TABLE [SUPERANN_BATCH] (
    [SN_BATCH_NO] BIGINT NOT NULL,
    [SN_TRUST_CODE] BIGINT NULL,
    [SN_CATEGORY] CHAR(3) NULL,
    [SN_PAYUNIT_CODE] CHAR(3) NULL,
    [SN_PAY_MONTHSTART] VARCHAR(255) NULL,
    [SN_PAY_MONTHEND] DATETIME2(3) NULL,
    [SN_STATUS] CHAR(1) NULL,
    [SN_ENT_ON] VARCHAR(255) NULL,
    [SN_CON_AMT] VARCHAR(255) NULL,
    [SN_PAY_DATE] DATETIME2(3) NULL,
    CONSTRAINT [PK_SUPERANN_BATCH] PRIMARY KEY ([SN_BATCH_NO])
);
GO

-- =========================================================================
-- 7. SUPERANN_BREAKUP - Superannuation Breakup Records
-- =========================================================================
CREATE TABLE [SUPERANN_BREAKUP] (
    [SN_FIN_YER] BIGINT NULL,
    [SN_PIN_NUM] BIGINT NULL,
    [SN_EMP_NAM] VARCHAR(100) NULL,
    [SN_FUD_NUM] DECIMAL(38) NULL,
    [SN_CON_DAT] DATETIME2(3) NULL,
    [SN_TRS_AMT] DECIMAL(19,0) NULL,
    [SN_EXG_AMT] DECIMAL(19,0) NULL,
    [SN_CON_TYP] CHAR(1) NULL,
    [SN_ENT_DAT] DATETIME2(3) NULL,
    [SN_BAT_NO] BIGINT NULL,
    [SN_GRS_AMT] DECIMAL(19,0) NULL,
    [SN_ACT_AMT] DECIMAL(19,0) NULL,
    [SN_PAY_AMT] DECIMAL(19,0) NULL
);
GO

-- =========================================================================
-- 8. SUPERANN_RATE - Superannuation Rate Master
-- =========================================================================
CREATE TABLE [SUPERANN_RATE] (
    [SN_FUD_NUM] BIGINT NULL,
    [SN_MONTH] DATETIME2(3) NULL,
    [SN_RATE] DECIMAL(19,0) NULL
);
GO

-- =========================================================================
-- 9. SUPERANN_TRUSTNAME - Superannuation Trust Names
-- =========================================================================
CREATE TABLE [SUPERANN_TRUSTNAME] (
    [ST_FND_NUM] DECIMAL(38) NOT NULL  -- FUND CODE,
    [ST_FND_NAM] VARCHAR(100) NULL  -- FUND NAME,
    CONSTRAINT [PK_SUPERANN_TRUSTNAME] PRIMARY KEY ([ST_FND_NUM])
);
GO

-- =========================================================================
-- Indexes for Performance Optimization
-- =========================================================================

-- Index on CONTRIBUTION_MAIN for trust and status queries
CREATE NONCLUSTERED INDEX [IDX_CONTRIBUTION_MAIN_TRUST_STATUS]
ON [CONTRIBUTION_MAIN] ([CONTRIBUTION_TRUST_CODE], [CONTRIBUTION_STATUS])
INCLUDE ([CONTRIBUTION_BATCH_NO], [CONTRIBUTION_PAY_MONTHSTART]);
GO

-- Index on CONTRIBUTION_DETAILS for member and batch queries
CREATE NONCLUSTERED INDEX [IDX_CONTRIBUTION_DETAILS_MEMBER]
ON [CONTRIBUTION_DETAILS] ([CONTRIBUTION_MEMBER_NO], [CONTRIBUTION_BATCH_NO])
INCLUDE ([CONTRIBUTION_EE_AMOUNT], [CONTRIBUTION_ER_AMOUNT]);
GO

-- Index on SUPERANN_CONTRIBUTION for year and fund queries
CREATE NONCLUSTERED INDEX [IDX_SUPERANN_CONTRIBUTION_FUND]
ON [SUPERANN_CONTRIBUTION] ([SN_FUD_NUM], [SN_FIN_YER])
INCLUDE ([SN_CON_AMT]);
GO

-- Index on SUPERANN_BATCH for status and date queries
CREATE NONCLUSTERED INDEX [IDX_SUPERANN_BATCH_STATUS]
ON [SUPERANN_BATCH] ([SN_STATUS], [SN_PAY_MONTHEND])
INCLUDE ([SN_BATCH_NO]);
GO

PRINT 'Contribution Module Tables created successfully!';
GO
