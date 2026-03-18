-- ==========================================
-- MODULE: VENDOR
-- Component: Tables
-- Description: Vendor management tables
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- Table: VENDOR_MASTER
-- Purpose: Master list of vendors for stationery and other procurements
CREATE TABLE [VENDOR_MASTER] (
    [VM_ID] BIGINT NOT NULL,
    [VM_CATID] BIGINT NOT NULL,
    [VM_LOC_ID] BIGINT NOT NULL,
    [VM_NAME] VARCHAR(100) NOT NULL,
    [VM_EMAIL] VARCHAR(50) NULL,
    [VM_ADDRESS] VARCHAR(200) NOT NULL,
    [VM_UPDATED_BY] BIGINT NOT NULL,
    [VM_UPDATED_ON] DATETIME2(3) NOT NULL,
    [VM_LIVESTATUS] CHAR(1) NOT NULL,
    CONSTRAINT [PK_VENDOR_MASTER] PRIMARY KEY ([VM_ID])
);
GO

-- Create indexes for performance
CREATE INDEX [IDX_VENDOR_MASTER_LOCID] ON [VENDOR_MASTER]([VM_LOC_ID]);
GO

CREATE INDEX [IDX_VENDOR_MASTER_STATUS] ON [VENDOR_MASTER]([VM_LIVESTATUS]);
GO

-- ==========================================
-- END OF VENDOR TABLES
-- ==========================================
