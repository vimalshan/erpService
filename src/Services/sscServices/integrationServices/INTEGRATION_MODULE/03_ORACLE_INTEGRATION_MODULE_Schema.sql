-- ==========================================
-- Module: ORACLE INTEGRATION MODULE
-- Description: Oracle ERP integration - PO, Vendor, MRC, and OU mapping
-- Created: March 9, 2026
-- Database: SSCDB
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- ORA_POMAST - Oracle PO Master Data
-- ==========================================
IF OBJECT_ID('[ORA_POMAST]', 'U') IS NOT NULL DROP TABLE [ORA_POMAST];
GO
CREATE TABLE [ORA_POMAST] (
    [PO_SEQID] BIGINT NOT NULL  -- PO Sequence ID,
    [PO_OUID] BIGINT NOT NULL  -- Oracle Org ID,
    [PO_ID] BIGINT NOT NULL  -- Oracle PO ID,
    [PO_NO] VARCHAR(25) NOT NULL  -- Oracle PO No,
    [PO_VENDORSITEID] BIGINT NOT NULL  -- Oracle Vendor Site ID,
    [PO_DUEDAYS] BIGINT NOT NULL  -- OracleDefault Due Days for Payment,
    [PO_DUE_DAY_MONTHOFF] BIGINT NOT NULL  -- Add days with Month forward,
    [PO_MONTHFORWARD] BIGINT NOT NULL  -- Number of months to be added,
    CONSTRAINT [PK_ORA_POMAST] PRIMARY KEY ([PO_SEQID]),
    CONSTRAINT [UK_ORA_POMAST_PO_ID] UNIQUE ([PO_ID])
);
GO

-- ==========================================
-- ORA_VENDORMAST - Oracle Vendor Master Data
-- ==========================================
IF OBJECT_ID('[ORA_VENDORMAST]', 'U') IS NOT NULL DROP TABLE [ORA_VENDORMAST];
GO
CREATE TABLE [ORA_VENDORMAST] (
    [VENDOR_ID] INT NOT NULL  -- Oracle Vendor ID,
    [VENDOR_NAME] VARCHAR(200) NOT NULL  -- Oracle Vendor Name,
    [VENDOR_CODE] VARCHAR(200) NOT NULL  -- Oracle Vendor Code,
    CONSTRAINT [PK_ORA_VENDORMAST] PRIMARY KEY ([VENDOR_ID])
);
GO

-- ==========================================
-- ORA_VENDORSITEMAST - Oracle Vendor Site Master Data
-- ==========================================
IF OBJECT_ID('[ORA_VENDORSITEMAST]', 'U') IS NOT NULL DROP TABLE [ORA_VENDORSITEMAST];
GO
CREATE TABLE [ORA_VENDORSITEMAST] (
    [VENDOR_SITEID] BIGINT NOT NULL  -- Oracle Vendor Site ID,
    [VENDOR_ID] BIGINT NOT NULL  -- Oracle Vendor ID,
    [VENDOR_SITECODE] VARCHAR(200) NOT NULL  -- Oracle Site Code,
    [VENDOR_OUID] VARCHAR(25) NOT NULL  -- Oracle Site OU ID,
    CONSTRAINT [PK_ORA_VENDORSITEMAST] PRIMARY KEY ([VENDOR_SITEID])
);
GO

-- ==========================================
-- ORA_VENDORSITEBUMAP - Oracle Vendor Site BU Mapping
-- ==========================================
IF OBJECT_ID('[ORA_VENDORSITEBUMAP]', 'U') IS NOT NULL DROP TABLE [ORA_VENDORSITEBUMAP];
GO
CREATE TABLE [ORA_VENDORSITEBUMAP] (
    [VENDOR_SITEID] BIGINT NOT NULL  -- Vendor Site ID,
    [VENDOR_BUID] BIGINT NOT NULL  -- Oracle BU ID,
    CONSTRAINT [PK_ORA_VENDORSITEBUMAP] PRIMARY KEY ([VENDOR_SITEID])
);
GO

-- ==========================================
-- ORA_OUMAST - Oracle Organization Unit Master Data
-- ==========================================
IF OBJECT_ID('[ORA_OUMAST]', 'U') IS NOT NULL DROP TABLE [ORA_OUMAST];
GO
CREATE TABLE [ORA_OUMAST] (
    [OU_ID] VARCHAR(25) NOT NULL  -- OU ID,
    [OU_NAME] VARCHAR(250) NOT NULL  -- OU Name,
    [OU_BUID] VARCHAR(25) NOT NULL  -- BU ID,
    CONSTRAINT [PK_ORA_OUMAST] PRIMARY KEY ([OU_ID])
);
GO

-- ==========================================
-- ORA_MRCMAST - Oracle Material Receipt Certificate Master
-- ==========================================
IF OBJECT_ID('[ORA_MRCMAST]', 'U') IS NOT NULL DROP TABLE [ORA_MRCMAST];
GO
CREATE TABLE [ORA_MRCMAST] (
    [MRC_SEQID] BIGINT NOT NULL  -- Oracle MRC Sequence ID,
    [MRC_POID] BIGINT NOT NULL  -- Oracle PO Sequence ID,
    [MRC_NO] VARCHAR(25) NOT NULL  -- Oracle MRC No,
    [MRC_SEQNO] BIGINT NULL,
    [MRC_RECDATE] DATETIME2(3) NULL,
    [MRC_VENDID] BIGINT NULL,
    [MRC_VENSITEID] BIGINT NULL,
    CONSTRAINT [PK_ORA_MRCMAST] PRIMARY KEY ([MRC_SEQID])
);
GO

-- ==========================================
-- ORA_OU_BUMAP - Oracle OU and BU Mapping
-- ==========================================
IF OBJECT_ID('[ORA_OU_BUMAP]', 'U') IS NOT NULL DROP TABLE [ORA_OU_BUMAP];
GO
CREATE TABLE [ORA_OU_BUMAP] (
    [OU_ID] BIGINT NOT NULL,
    [OU_BUID] VARCHAR(25) NOT NULL
);
GO

PRINT 'ORACLE_INTEGRATION_MODULE Schema created successfully.';
GO
