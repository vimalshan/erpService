-- ==========================================
-- Module: MedicineManagement
-- Purpose: Medicine/Pharmacy & Inventory Management
-- Generated: 2026-03-09
-- ==========================================

USE HEALTHDB;
GO

-- =====================================================
-- Table: MEDICINE_TYPMAST
-- Description: Medicine Type Master
-- =====================================================
IF OBJECT_ID('dbo.MEDICINE_TYPMAST', 'U') IS NOT NULL
    DROP TABLE dbo.MEDICINE_TYPMAST;
GO

CREATE TABLE [dbo.MEDICINE_TYPMAST] (
    [MT_TYP_COD] CHAR(3) NOT NULL  -- Medicine Type Code,
    [MT_TYP_NAM] VARCHAR(30) NULL  -- Medicine Type Name,
    [MT_ENT_USR] VARCHAR(25) NULL  -- Enter User,
    [MT_USR_NUM] DECIMAL(20,0) NULL  -- Enter User Pin Number,
    [MT_ENT_DAT] DATETIME2(3) NOT NULL  -- Entry Date,
    [MT_MOD_USR] VARCHAR(25) NULL  -- Modified User,
    [MT_MOD_NUM] DECIMAL(20,0) NULL  -- Modified user Pin Number,
    [MT_MOD_DAT] DATETIME2(3) NULL  -- Modified date,
    CONSTRAINT [PK_MEDICINE_TYPMAST] PRIMARY KEY ([MT_TYP_COD])
);

-- =====================================================
-- Table: MEDICINE_PKG
-- Description: Medicine Packaging Master
-- =====================================================
IF OBJECT_ID('dbo.MEDICINE_PKG', 'U') IS NOT NULL
    DROP TABLE dbo.MEDICINE_PKG;
GO

CREATE TABLE [dbo.MEDICINE_PKG] (
    [PK_PKG_COD] CHAR(3) NOT NULL  -- Packaging Code,
    [PK_PKG_TYP] VARCHAR(20) NULL  -- Packing Type,
    [PK_ENT_USR] VARCHAR(25) NULL  -- Enter User,
    [PK_USR_NUM] DECIMAL(20,0) NULL  -- Enter User Pin Number,
    [PK_ENT_DAT] DATETIME2(3) NOT NULL  -- Entry Date,
    [PK_MOD_USR] VARCHAR(25) NULL  -- Modified User,
    [PK_MOD_NUM] DECIMAL(38) NULL  -- Modified user Pin Number,
    [PK_MOD_DAT] DATETIME2(3) NULL  -- Modified date,
    CONSTRAINT [PK_MEDICINE_PKG] PRIMARY KEY ([PK_PKG_COD])
);

-- =====================================================
-- Table: MEDICINE_MAST
-- Description: Medicine Master
-- =====================================================
IF OBJECT_ID('dbo.MEDICINE_MAST', 'U') IS NOT NULL
    DROP TABLE dbo.MEDICINE_MAST;
GO

CREATE TABLE [dbo.MEDICINE_MAST] (
    [MM_MED_COD] CHAR(3) NOT NULL  -- Medicine Code,
    [MM_MED_NAM] VARCHAR(50) NOT NULL  -- Medicine Name,
    [MM_MED_TYP] CHAR(3) NOT NULL  -- Medicine Type Code,
    [MM_MED_CAT] CHAR(1) NULL  -- Medicine Category (H=High,M=Medium,L=Low),
    [MM_ORD_MIN] DECIMAL(20,0) NULL  -- Medicine Order Level Min,
    [MM_ORD_MAX] DECIMAL(20,0) NULL  -- Medicine Order Level Max,
    [MM_ENT_USR] VARCHAR(25) NULL  -- Enter User,
    [MM_USR_NUM] DECIMAL(20,0) NULL  -- Entered User Pin Number,
    [MM_ENT_DAT] DATETIME2(3) NULL  -- Entry Date,
    [MM_MOD_USR] VARCHAR(25) NULL  -- Modified User,
    [MM_MOD_NUM] DECIMAL(20,0) NULL  -- Modified user Pin Number,
    [MM_MOD_DAT] DATETIME2(3) NULL  -- Modified date
);

-- =====================================================
-- Table: DOCATTEND_MAST
-- Description: Doctor/Attendant Master
-- =====================================================
IF OBJECT_ID('dbo.DOCATTEND_MAST', 'U') IS NOT NULL
    DROP TABLE dbo.DOCATTEND_MAST;
GO

CREATE TABLE [dbo.DOCATTEND_MAST] (
    [DM_COD] VARCHAR(20) NULL  -- CODE,
    [DM_FLAG] CHAR(1) NULL  -- D-DOCTOR,A-ATTENDANT,
    [DM_NAME] VARCHAR(30) NULL  -- D/A NAME,
    [DM_SYSID] BIGINT NULL
);

-- =====================================================
-- Table: MED_DRCRFLG
-- Description: Medicine Doctor/Credit Flag
-- =====================================================
IF OBJECT_ID('dbo.MED_DRCRFLG', 'U') IS NOT NULL
    DROP TABLE dbo.MED_DRCRFLG;
GO

CREATE TABLE [dbo.MED_DRCRFLG] (
    [MED_FLG] CHAR(1) NULL,
    [MED_DRCR] INT NULL
);

-- =====================================================
-- Table: MEDICINE_CREDIT
-- Description: Medicine Credit/Stock Transactions
-- =====================================================
IF OBJECT_ID('dbo.MEDICINE_CREDIT', 'U') IS NOT NULL
    DROP TABLE dbo.MEDICINE_CREDIT;
GO

CREATE TABLE [dbo.MEDICINE_CREDIT] (
    [MD_COM_COD] CHAR(3) NOT NULL  -- Comapny Code,
    [MD_TRN_COD] BIGINT NOT NULL  -- Transaction Code,
    [MD_MED_COD] CHAR(3) NOT NULL  -- Medicine Code,
    [MD_REC_TYP] CHAR(1) NOT NULL  -- Record Type(O=Opening Balance,P=Purchase,I=Issue,E=expire),
    [MD_MED_QNT] BIGINT NOT NULL  -- Medicine Quantity,
    [MD_TRN_DAT] DATETIME2(3) NOT NULL  -- Transaction date,
    [MD_ENT_USR] VARCHAR(25) NOT NULL  -- Enter User,
    [MD_USR_NUM] DECIMAL(20,0) NOT NULL  -- Entered User Pin Number,
    [MD_ENT_DAT] DATETIME2(3) NOT NULL  -- Entry Date,
    [MD_MOD_USR] VARCHAR(25) NULL  -- Modified User,
    [MD_MOD_NUM] DECIMAL(20,0) NULL  -- Modified User Pin Number,
    [MD_MOD_DAT] DATETIME2(3) NULL  -- Modified Date,
    [MD_LOT_NUM] VARCHAR(50) NULL,
    [MD_CAN_FLG] CHAR(1) NULL,
    [MD_TRN_NUM] BIGINT NULL,
    CONSTRAINT [PK_MEDICINE_CREDIT] PRIMARY KEY ([MD_COM_COD])
);

-- =====================================================
-- Table: MEDICINE_ISSUE
-- Description: Medicine Issue/Dispensing
-- =====================================================
IF OBJECT_ID('dbo.MEDICINE_ISSUE', 'U') IS NOT NULL
    DROP TABLE dbo.MEDICINE_ISSUE;
GO

CREATE TABLE [dbo.MEDICINE_ISSUE] (
    [MD_COM_COD] CHAR(3) NULL  -- Comapny Code,
    [MD_TRN_NUM] VARCHAR(255) NULL  -- Transaction Code,
    [MD_TRN_DAT] VARCHAR(255) NULL  -- Medicine Code,
    [MD_ISS_QNT] BIGINT NULL  -- Record Type(O=Opening Balance,P=Purchase,I=Issue,E=expire),
    [MD_ENT_USR] VARCHAR(25) NULL  -- Medicine Quantity,
    [MD_USR_NUM] VARCHAR(255) NULL  -- Transaction date,
    [MD_ENT_DAT] VARCHAR(255) NULL  -- Enter User,
    [MD_VIS_NUM] VARCHAR(255) NULL  -- Visit Number,
    [MD_MOD_USR] VARCHAR(25) NULL  -- Modified User,
    [MD_MOD_NUM] VARCHAR(255) NULL  -- Modified user Pin Number,
    [MD_MOD_DAT] VARCHAR(255) NULL  -- Modified date,
    [MD_MED_COD] CHAR(3) NULL
);

-- =====================================================
-- Indexes
-- =====================================================
CREATE INDEX [IDX_MEDICINE_CREDIT_MD_COM_COD] ON [dbo.MEDICINE_CREDIT]([MD_COM_COD]);
CREATE INDEX [IDX_MEDICINE_CREDIT_MD_TRN_DAT] ON [dbo.MEDICINE_CREDIT]([MD_TRN_DAT]);
CREATE INDEX [IDX_MEDICINE_MAST_MM_MED_COD] ON [dbo.MEDICINE_MAST]([MM_MED_COD]);

PRINT 'MedicineManagement: Table creation completed successfully.';
GO
