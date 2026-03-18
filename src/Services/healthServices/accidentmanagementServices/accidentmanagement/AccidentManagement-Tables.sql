-- ==========================================
-- Module: AccidentManagement
-- Purpose: Accident/Injury Management & Reporting
-- Generated: 2026-03-09
-- Updated: 2026-03-13 - Schema enhancements
-- ==========================================

USE HEALTHDB;
GO

-- =====================================================
-- Table: ACCIDENT_SEVERITY
-- Description: Master table for accident severity levels
-- =====================================================
IF OBJECT_ID('dbo.ACCIDENT_SEVERITY', 'U') IS NOT NULL
    DROP TABLE dbo.ACCIDENT_SEVERITY;
GO

CREATE TABLE [dbo.ACCIDENT_SEVERITY] (
    [SEVERITY_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [SEVERITY_CODE] VARCHAR(10) NOT NULL UNIQUE,
    [SEVERITY_NAME] VARCHAR(50) NOT NULL,
    [DESCRIPTION] VARCHAR(200) NULL,
    [CreatedDate] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedDate] DATETIME2(3) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

-- =====================================================
-- Table: ACCIDENT_STATUS
-- Description: Master table for accident status types
-- =====================================================
IF OBJECT_ID('dbo.ACCIDENT_STATUS', 'U') IS NOT NULL
    DROP TABLE dbo.ACCIDENT_STATUS;
GO

CREATE TABLE [dbo.ACCIDENT_STATUS] (
    [STATUS_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [STATUS_CODE] VARCHAR(10) NOT NULL UNIQUE,
    [STATUS_NAME] VARCHAR(50) NOT NULL,
    [DESCRIPTION] VARCHAR(200) NULL,
    [CreatedDate] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedDate] DATETIME2(3) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

-- =====================================================
-- Table: CATEGORY_INJURY
-- Description: Categories of injuries that can occur
-- =====================================================
IF OBJECT_ID('dbo.CATEGORY_INJURY', 'U') IS NOT NULL
    DROP TABLE dbo.CATEGORY_INJURY;
GO

CREATE TABLE [dbo.CATEGORY_INJURY] (
    [CAT_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [CAT_GUID] UNIQUEIDENTIFIER NOT NULL UNIQUE DEFAULT NEWID(),
    [CAT_NAME] VARCHAR(100) NOT NULL,
    [DESCRIPTION] VARCHAR(200) NULL,
    [CreatedDate] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedDate] DATETIME2(3) NULL,
    [CreatedBy] VARCHAR(100) NULL,
    [UpdatedBy] VARCHAR(100) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

-- =====================================================
-- Table: NATURE_INJURY
-- Description: Nature/Type of injuries
-- =====================================================
IF OBJECT_ID('dbo.NATURE_INJURY', 'U') IS NOT NULL
    DROP TABLE dbo.NATURE_INJURY;
GO

CREATE TABLE [dbo.NATURE_INJURY] (
    [NATURE_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [NATURE_GUID] UNIQUEIDENTIFIER NOT NULL UNIQUE DEFAULT NEWID(),
    [NATURE_NAME] VARCHAR(100) NOT NULL,
    [DESCRIPTION] VARCHAR(200) NULL,
    [CreatedDate] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedDate] DATETIME2(3) NULL,
    [CreatedBy] VARCHAR(100) NULL,
    [UpdatedBy] VARCHAR(100) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

-- =====================================================
-- Table: ACC_CONTRCT_LST
-- Description: Contractor list for accident tracking
-- =====================================================
IF OBJECT_ID('dbo.ACC_CONTRCT_LST', 'U') IS NOT NULL
    DROP TABLE dbo.ACC_CONTRCT_LST;
GO

CREATE TABLE [dbo.ACC_CONTRCT_LST] (
    [ACL_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [ACL_GUID] UNIQUEIDENTIFIER NOT NULL UNIQUE DEFAULT NEWID(),
    [ACL_CONT_NAM] VARCHAR(100) NOT NULL,
    [ACL_CONT_ID] BIGINT NOT NULL,
    [ACL_STATUS] CHAR(1) NOT NULL CHECK (ACL_STATUS IN ('A', 'I')),
    [CreatedDate] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedDate] DATETIME2(3) NULL,
    [CreatedBy] VARCHAR(100) NULL,
    [UpdatedBy] VARCHAR(100) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

-- =====================================================
-- Table: ACC_PERS_INJ
-- Description: Person Injured details
-- =====================================================
IF OBJECT_ID('dbo.ACC_PERS_INJ', 'U') IS NOT NULL
    DROP TABLE dbo.ACC_PERS_INJ;
GO

CREATE TABLE [dbo.ACC_PERS_INJ] (
    [API_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [API_GUID] UNIQUEIDENTIFIER NOT NULL UNIQUE DEFAULT NEWID(),
    [API_SRL_NUM] BIGINT NOT NULL,
    [API_PERS_NAM] VARCHAR(100) NOT NULL,
    [API_EMP_STATUS] CHAR(1) NOT NULL CHECK (API_EMP_STATUS IN ('S', 'C')),  -- S=Staff, C=Contractor
    [CreatedDate] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedDate] DATETIME2(3) NULL,
    [CreatedBy] VARCHAR(100) NULL,
    [UpdatedBy] VARCHAR(100) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

-- =====================================================
-- Table: DAILY_ACC_FIR
-- Description: Daily Accident First Information Report
-- =====================================================
IF OBJECT_ID('dbo.DAILY_ACC_FIR', 'U') IS NOT NULL
    DROP TABLE dbo.DAILY_ACC_FIR;
GO

CREATE TABLE [dbo.DAILY_ACC_FIR] (
    [DAF_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [DAF_GUID] UNIQUEIDENTIFIER NOT NULL UNIQUE DEFAULT NEWID(),
    [DAF_ACC_NUM] BIGINT NOT NULL UNIQUE,
    [DAF_EMP_NUM] VARCHAR(20) NULL,
    [DAF_EMP_NAM] VARCHAR(100) NULL,
    [DAF_WRK_NAM] VARCHAR(100) NULL,
    [DAF_CONT_ID] BIGINT NULL,
    [DAF_CONT_NAM] VARCHAR(100) NULL,
    [DAF_EMP_DEPT] VARCHAR(100) NULL,
    [DAF_ACC_DAT] DATETIME2(3) NOT NULL,
    [DAF_ACC_LOC] VARCHAR(255) NOT NULL,
    [DAF_NATURE_INJ] VARCHAR(100) NOT NULL,
    [DAF_BODY_PART] VARCHAR(100) NOT NULL,
    [DAF_SHIFT] VARCHAR(100) NULL,
    [DAF_MEDCENTRE_NAM] VARCHAR(100) NOT NULL,
    [DAF_TRT_GIVEN] VARCHAR(500) NOT NULL,
    [DAF_MEDCENTRE_DAT] DATETIME2(3) NOT NULL,
    [DAF_COM_COD] VARCHAR(10) NOT NULL,
    [DAF_ENT_USR] VARCHAR(100) NOT NULL,
    [DAF_ENT_NUM] BIGINT NOT NULL,
    [DAF_ENT_DATE] DATETIME2(3) NOT NULL,
    [DAF_CAT_INJ] BIGINT NOT NULL,
    [DAF_NAT_INJ] BIGINT NOT NULL,
    [DAF_PRV_MES] VARCHAR(500) NOT NULL,
    [DAF_CAU_INC] VARCHAR(500) NOT NULL,
    [DAF_SHFTINCHRG_NAM] VARCHAR(100) NULL,
    [DAF_SEVERITY_ID] BIGINT NOT NULL DEFAULT 1,
    [DAF_STATUS_ID] BIGINT NOT NULL DEFAULT 1,
    [CreatedDate] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedDate] DATETIME2(3) NULL,
    [CreatedBy] VARCHAR(100) NULL,
    [UpdatedBy] VARCHAR(100) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_DAILY_ACC_FIR] PRIMARY KEY ([DAF_ID]),
    CONSTRAINT [FK_DAILY_ACC_FIR_CATEGORY] FOREIGN KEY ([DAF_CAT_INJ]) REFERENCES [dbo.CATEGORY_INJURY]([CAT_ID]),
    CONSTRAINT [FK_DAILY_ACC_FIR_NATURE] FOREIGN KEY ([DAF_NAT_INJ]) REFERENCES [dbo.NATURE_INJURY]([NATURE_ID]),
    CONSTRAINT [FK_DAILY_ACC_FIR_SEVERITY] FOREIGN KEY ([DAF_SEVERITY_ID]) REFERENCES [dbo.ACCIDENT_SEVERITY]([SEVERITY_ID]),
    CONSTRAINT [FK_DAILY_ACC_FIR_STATUS] FOREIGN KEY ([DAF_STATUS_ID]) REFERENCES [dbo.ACCIDENT_STATUS]([STATUS_ID])
);

-- =====================================================
-- Audit Log Table
-- Description: Tracks all changes to critical tables
-- =====================================================
IF OBJECT_ID('dbo.AUDIT_LOG', 'U') IS NOT NULL
    DROP TABLE dbo.AUDIT_LOG;
GO

CREATE TABLE [dbo.AUDIT_LOG] (
    [AUDIT_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [TABLE_NAME] VARCHAR(100) NOT NULL,
    [RECORD_ID] BIGINT NOT NULL,
    [OPERATION] VARCHAR(10) NOT NULL CHECK (OPERATION IN ('INSERT', 'UPDATE', 'DELETE')),
    [OLD_VALUES] NVARCHAR(MAX) NULL,
    [NEW_VALUES] NVARCHAR(MAX) NULL,
    [CHANGED_BY] VARCHAR(100) NOT NULL,
    [CHANGED_DATE] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
    [IP_ADDRESS] VARCHAR(50) NULL
);

-- =====================================================
-- Indexes
-- =====================================================
CREATE INDEX [IDX_DAILY_ACC_FIR_DAF_COM_COD] ON [dbo.DAILY_ACC_FIR]([DAF_COM_COD]);
CREATE INDEX [IDX_DAILY_ACC_FIR_DAF_ACC_DAT] ON [dbo.DAILY_ACC_FIR]([DAF_ACC_DAT]);
CREATE INDEX [IDX_DAILY_ACC_FIR_DAF_EMP_NUM] ON [dbo.DAILY_ACC_FIR]([DAF_EMP_NUM]);
CREATE INDEX [IDX_DAILY_ACC_FIR_GUID] ON [dbo.DAILY_ACC_FIR]([DAF_GUID]);
CREATE INDEX [IDX_CATEGORY_INJURY_GUID] ON [dbo.CATEGORY_INJURY]([CAT_GUID]);
CREATE INDEX [IDX_NATURE_INJURY_GUID] ON [dbo.NATURE_INJURY]([NATURE_GUID]);
CREATE INDEX [IDX_AUDIT_LOG_TABLE_DATE] ON [dbo.AUDIT_LOG]([TABLE_NAME], [CHANGED_DATE]);

-- =====================================================
-- Insert Reference Data
-- =====================================================

-- TODO: Populate ACCIDENT_SEVERITY with severity levels (Critical, High, Medium, Low)
-- TODO: Populate ACCIDENT_STATUS with status types (New, InProgress, Resolved, Closed)
-- TODO: Populate CATEGORY_INJURY with injury categories (Chemical Burn, Fracture, Cut, etc.)
-- TODO: Populate NATURE_INJURY with injury types (Deep, Superficial, Severe, etc.)

PRINT 'AccidentManagement: Table creation completed successfully.';
GO
