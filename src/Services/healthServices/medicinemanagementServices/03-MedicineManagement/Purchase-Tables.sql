-- ==========================================
-- Module: PurchaseManagement
-- Purpose: Medicine Purchase & Vendor Management
-- Generated: 2026-03-09
-- ==========================================

USE HEALTHDB;
GO

-- =====================================================
-- Table: PURCHASE_MAIN
-- Description: Purchase Main Record
-- =====================================================
IF OBJECT_ID('dbo.PURCHASE_MAIN', 'U') IS NOT NULL
    DROP TABLE dbo.PURCHASE_MAIN;
GO

CREATE TABLE [dbo.PURCHASE_MAIN] (
    [MD_COM_COD] CHAR(3) NOT NULL  -- Company Code,
    [MD_TRN_NUM] BIGINT NOT NULL  -- Transaction Code,
    [MD_VND_NAM] VARCHAR(100) NOT NULL  -- Vendor name,
    [MD_INV_NUM] VARCHAR(30) NOT NULL  -- Invoice Number,
    [MD_INV_DAT] DATETIME2(3) NOT NULL  -- Invoice Date,
    [MD_INV_AMT] DECIMAL(38) NOT NULL  -- Invoice Amount,
    [MD_ENT_USR] VARCHAR(25) NOT NULL  -- Enter User,
    [MD_USR_NUM] DECIMAL(20,0) NOT NULL  -- Enter User Pin Number,
    [MD_ENT_DAT] DATETIME2(3) NOT NULL  -- Entry Date,
    [MD_CAN_FLG] CHAR(1) NOT NULL  -- Modified User,
    [MD_MOD_USR] VARCHAR(25) NULL  -- Modified user Pin Number,
    [MD_MOD_NUM] DECIMAL(20,0) NULL  -- Modified date,
    [MD_MOD_DAT] DATETIME2(3) NULL,
    CONSTRAINT [PK_PURCHASE_MAIN] PRIMARY KEY ([MD_COM_COD], [MD_TRN_NUM])
);

-- =====================================================
-- Table: PURCHASE_SUB
-- Description: Purchase Sub Record (Line Items)
-- =====================================================
IF OBJECT_ID('dbo.PURCHASE_SUB', 'U') IS NOT NULL
    DROP TABLE dbo.PURCHASE_SUB;
GO

CREATE TABLE [dbo.PURCHASE_SUB] (
    [MD_COM_COD] CHAR(3) NOT NULL  -- Company Code,
    [MD_TRN_NUM] BIGINT NOT NULL  -- Transaction Code,
    [MD_SRL_NUM] VARCHAR(255) NOT NULL  -- Serial Number,
    [MD_MED_COD] CHAR(3) NOT NULL  -- Medicine Code,
    [MD_PKG_TYP] CHAR(3) NOT NULL  -- Packaging type,
    [MD_PKG_QNT] BIGINT NULL  -- Packaging Amount,
    [MD_PKG_NOS] BIGINT NULL,
    [MD_TOT_QNT] BIGINT NULL,
    [MD_MFG_DAT] DATETIME2(3) NULL,
    [MD_EXP_DAT] DATETIME2(3) NULL,
    [MD_LOT_NUM] VARCHAR(50) NULL,
    [MD_ENT_USR] CHAR(25) NULL  -- Enter User,
    [MD_USR_NUM] DECIMAL(20,0) NULL  -- Enter User Pin Number,
    [MD_ENT_DAT] DATETIME2(3) NULL  -- Entry Date,
    [MD_MOD_USR] VARCHAR(25) NULL  -- Modified User,
    [MD_MOD_NUM] DECIMAL(20,0) NULL  -- Modified user Pin Number,
    [MD_MOD_DAT] DATETIME2(3) NULL  -- Modified date,
    [MD_CAN_FLG] CHAR(1) NOT NULL,
    CONSTRAINT [PK_PURCHASE_SUB] PRIMARY KEY ([MD_COM_COD], [MD_TRN_NUM], [MD_SRL_NUM])
);

-- =====================================================
-- Indexes
-- =====================================================
CREATE INDEX [IDX_PURCHASE_MAIN_MD_COM_COD] ON [dbo.PURCHASE_MAIN]([MD_COM_COD]);
CREATE INDEX [IDX_PURCHASE_MAIN_MD_INV_DAT] ON [dbo.PURCHASE_MAIN]([MD_INV_DAT]);
CREATE INDEX [IDX_PURCHASE_SUB_MD_TRN_NUM] ON [dbo.PURCHASE_SUB]([MD_TRN_NUM]);

PRINT 'PurchaseManagement: Table creation completed successfully.';
GO
