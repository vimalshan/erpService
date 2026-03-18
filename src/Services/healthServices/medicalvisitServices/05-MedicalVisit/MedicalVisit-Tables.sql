-- ==========================================
-- Module: MedicalVisit
-- Purpose: Medical Clinic Visit & Consultation
-- Generated: 2026-03-09
-- ==========================================

USE HEALTHDB;
GO

-- =====================================================
-- Table: VISIT_MAIN
-- Description: Medical Visit Main Record
-- =====================================================
IF OBJECT_ID('dbo.VISIT_MAIN', 'U') IS NOT NULL
    DROP TABLE dbo.VISIT_MAIN;
GO

CREATE TABLE [dbo.VISIT_MAIN] (
    [VM_COM_COD] CHAR(3) NULL,
    [VM_VIS_NUM] BIGINT NULL,
    [VM_USR_ID] VARCHAR(25) NULL,
    [VM_PIN_NUM] DECIMAL(20,0) NULL,
    [VM_WRK_NAM] VARCHAR(50) NULL,
    [VM_CONTRCT_ID] VARCHAR(20) NULL,
    [VM_CONTRCT_NAM] VARCHAR(20) NULL,
    [VM_VIS_DAT] DATETIME2(3) NULL,
    [VM_OTH_HOSP] VARCHAR(200) NULL,
    [VM_VIS_SHIFT] CHAR(1) NULL,
    [VM_VIS_TYP] CHAR(1) NULL,
    [VM_ATT_COD] VARCHAR(10) NULL,
    [VM_DOC_COD] VARCHAR(10) NOT NULL,
    [VM_PAT_DIA] VARCHAR(200) NOT NULL,
    [VM_TRT_REM] VARCHAR(200) NOT NULL,
    [VM_TST_ADV] VARCHAR(200) NULL,
    [VM_MED_GIV] CHAR(3) NULL,
    [VM_NXT_REV] DATETIME2(3) NULL,
    [VM_ENT_USR] VARCHAR(25) NULL,
    [VM_ENT_NUM] DECIMAL(20,0) NULL,
    [VM_ENT_DAT] DATETIME2(3) NULL,
    [DV_MOD_USR] VARCHAR(25) NULL,
    [VM_MOD_NUM] DECIMAL(20,0) NULL,
    [VM_MOD_DAT] DATETIME2(3) NULL,
    [VM_CAN_FLG] CHAR(1) NULL,
    [VM_DIA_CAT] CHAR(3) NULL,
    [VM_DIA_SUBCAT] BIGINT NULL,
    [VM_DOC_REMARKS] VARCHAR(1000) NULL
);

-- =====================================================
-- Table: VISIT_SUB
-- Description: Medical Visit Sub Record (Tests/Vitals)
-- =====================================================
IF OBJECT_ID('dbo.VISIT_SUB', 'U') IS NOT NULL
    DROP TABLE dbo.VISIT_SUB;
GO

CREATE TABLE [dbo.VISIT_SUB] (
    [VS_COM_COD] CHAR(3) NOT NULL  -- Company Code,
    [VS_VIS_NUM] BIGINT NOT NULL  -- Visit Num,
    [VS_TST_TYP] VARCHAR(20) NULL  -- Check Type,
    [VS_TST_VAL] VARCHAR(25) NULL  -- Check Value,
    [VS_SRL_NUM] BIGINT NULL  -- Serial Number
);

-- =====================================================
-- Indexes
-- =====================================================
CREATE INDEX [IDX_VISIT_MAIN_VM_COM_COD] ON [dbo.VISIT_MAIN]([VM_COM_COD]);
CREATE INDEX [IDX_VISIT_MAIN_VM_VIS_DAT] ON [dbo.VISIT_MAIN]([VM_VIS_DAT]);
CREATE INDEX [IDX_VISIT_SUB_VS_VIS_NUM] ON [dbo.VISIT_SUB]([VS_VIS_NUM]);

PRINT 'MedicalVisit: Table creation completed successfully.';
GO
