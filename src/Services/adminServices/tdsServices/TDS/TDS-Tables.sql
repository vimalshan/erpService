-- ==========================================
-- MODULE: TDS (Tax Deduction at Source)
-- Component: Tables
-- Description: TDS vendor and file management tables
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- Table: TDS_VENDORS
-- Purpose: Stores vendor information for TDS reporting
CREATE TABLE [TDS_VENDORS] (
    [VENDOR_ID] BIGINT NULL,
    [VENDOR_NAME] VARCHAR(240) NULL,
    [EMAIL_ADDRESS] VARCHAR(3000) NULL,
    [PAN_NO] VARCHAR(30) NULL
);
GO

-- Table: TDSFILE_DETAILS
-- Purpose: Tracks TDS file uploads and processing
CREATE TABLE [TDSFILE_DETAILS] (
    [FILE_ID] BIGINT NOT NULL,
    [FILE_NAME] VARCHAR(100) NULL,
    [PAN_NO] VARCHAR(15) NULL,
    [EMAIL_STATUS] VARCHAR(1) NULL,
    [FILE_TYPE] VARCHAR(3) NULL,
    CONSTRAINT [PK_TDSFILE_DETAILS] PRIMARY KEY ([FILE_ID])
);
GO

-- Create indexes for performance
CREATE INDEX [IDX_TDS_VENDORS_PANNO] ON [TDS_VENDORS]([PAN_NO]);
GO

CREATE INDEX [IDX_TDSFILE_PANNO] ON [TDSFILE_DETAILS]([PAN_NO]);
GO

-- ==========================================
-- END OF TDS TABLES
-- ==========================================
