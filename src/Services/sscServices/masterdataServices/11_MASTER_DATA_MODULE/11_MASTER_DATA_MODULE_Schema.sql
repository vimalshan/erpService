-- ==========================================
-- Module: MASTER DATA MODULE
-- Description: Reference data and LOV (List of Values) master data
-- Created: March 9, 2026
-- Database: SSCDB
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- LOV_MAST - List of Values Master
-- ==========================================
IF OBJECT_ID('[LOV_MAST]', 'U') IS NOT NULL DROP TABLE [LOV_MAST];
GO
CREATE TABLE [LOV_MAST] (
    [LOV_ID] BIGINT NOT NULL,
    [LOV_TYPE] VARCHAR(10) NOT NULL,
    [LOV_NAME] VARCHAR(200) NOT NULL,
    CONSTRAINT [PK_LOV_MAST] PRIMARY KEY ([LOV_ID])
);
GO

-- ==========================================
-- LOV_TYPEMAST - LOV Type Master
-- ==========================================
IF OBJECT_ID('[LOV_TYPEMAST]', 'U') IS NOT NULL DROP TABLE [LOV_TYPEMAST];
GO
CREATE TABLE [LOV_TYPEMAST] (
    [LOV_TYPECODE] VARCHAR(10) NOT NULL,
    [LOV_TYPENAME] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_LOV_TYPEMAST] PRIMARY KEY ([LOV_TYPECODE])
);
GO

-- ==========================================
-- HOLDTYPE_MAST - Hold Type Master
-- ==========================================
IF OBJECT_ID('[HOLDTYPE_MAST]', 'U') IS NOT NULL DROP TABLE [HOLDTYPE_MAST];
GO
CREATE TABLE [HOLDTYPE_MAST] (
    [HOLD_ID] BIGINT NULL,
    [HOLD_NAME] VARCHAR(100) NULL,
    [HOLD_CATEGORY] CHAR(1) NULL,
    CONSTRAINT [PK_HOLDTYPE_MAST] PRIMARY KEY ([HOLD_ID])
);
GO

-- ==========================================
-- LOCATION_SCANPARAMS - Location Scan Parameters
-- ==========================================
IF OBJECT_ID('[LOCATION_SCANPARAMS]', 'U') IS NOT NULL DROP TABLE [LOCATION_SCANPARAMS];
GO
CREATE TABLE [LOCATION_SCANPARAMS] (
    [LOCSCANPARAM_ID] BIGINT NOT NULL,
    [LOC_ID] BIGINT NOT NULL,
    [LOC_EFFDATE] DATETIME2(3) NOT NULL,
    [LOC_CLSDATE] DATETIME2(3) NULL
);
GO

-- ==========================================
-- SCANNER_MASTER - Scanner Device Master
-- ==========================================
IF OBJECT_ID('[SCANNER_MASTER]', 'U') IS NOT NULL DROP TABLE [SCANNER_MASTER];
GO
CREATE TABLE [SCANNER_MASTER] (
    [DEVICE_ID] BIGINT NOT NULL,
    [DEVICE_NAME] VARCHAR(100) NULL,
    [DEVICE_LOCID] BIGINT NOT NULL,
    [DEVICE_PATH] VARCHAR(1000) NULL
);
GO

-- ==========================================
-- NEW_TABLE - Sample Data Table
-- ==========================================
IF OBJECT_ID('[NEW_TABLE]', 'U') IS NOT NULL DROP TABLE [NEW_TABLE];
GO
CREATE TABLE [NEW_TABLE] (
    [USER_NAME] VARCHAR(100) NULL,
    [Services] DECIMAL(38) NULL,
    [Materials] DECIMAL(38) NULL,
    [Freight] DECIMAL(38) NULL,
    [Export Charges (EIMS)] DECIMAL(38) NULL,
    [Statutory] DECIMAL(38) NULL,
    [Utility] DECIMAL(38) NULL,
    [Total] DECIMAL(38) NULL
);
GO

PRINT 'MASTER_DATA_MODULE Schema created successfully.';
GO
