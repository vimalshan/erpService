-- ==========================================
-- TDS MODULE - STANDALONE DATABASE
-- Complete Deployment Script
-- Version: 1.0
-- Generated: 2026-03-09
-- ==========================================

USE MASTER;
GO

PRINT '=== Creating TDSDB ===';
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TDSDB')
BEGIN
    CREATE DATABASE TDSDB
    ON PRIMARY (
        NAME = 'TDSDB_data',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\TDSDB.mdf',
        SIZE = 50MB,
        MAXSIZE = 500MB,
        FILEGROWTH = 10%
    )
    LOG ON (
        NAME = 'TDSDB_log',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\TDSDB.ldf',
        SIZE = 25MB,
        MAXSIZE = 250MB,
        FILEGROWTH = 10%
    );
    PRINT '✓ TDSDB created';
END
GO

USE [TDSDB];
GO

PRINT '';
PRINT '=== Deploying TDS Tables ===';
GO

CREATE TABLE [TDS_VENDORS] (
    [VENDOR_ID] BIGINT NULL,
    [VENDOR_NAME] VARCHAR(240) NULL,
    [EMAIL_ADDRESS] VARCHAR(3000) NULL,
    [PAN_NO] VARCHAR(30) NULL
);
PRINT '✓ TDS_VENDORS table created';
GO

CREATE TABLE [TDSFILE_DETAILS] (
    [FILE_ID] BIGINT NOT NULL,
    [FILE_NAME] VARCHAR(100) NULL,
    [PAN_NO] VARCHAR(15) NULL,
    [EMAIL_STATUS] VARCHAR(1) NULL,
    [FILE_TYPE] VARCHAR(3) NULL,
    CONSTRAINT [PK_TDSFILE_DETAILS] PRIMARY KEY ([FILE_ID])
);
PRINT '✓ TDSFILE_DETAILS table created';
GO

CREATE INDEX [IDX_TDS_VENDORS_PANNO] ON [TDS_VENDORS]([PAN_NO]);
CREATE INDEX [IDX_TDSFILE_PANNO] ON [TDSFILE_DETAILS]([PAN_NO]);
PRINT '✓ Indexes created';
GO

PRINT '';
PRINT '========================================';
PRINT 'TDSDB DEPLOYMENT COMPLETE';
PRINT '========================================';
PRINT '';
PRINT 'Database: TDSDB';
PRINT 'Status: ✓ Successfully Deployed';
PRINT '';
PRINT 'Objects Created:';
PRINT '  ✓ 2 Tables (TDS_VENDORS, TDSFILE_DETAILS)';
PRINT '  ✓ 2 Indexes';
PRINT '';
PRINT '========================================';
GO

-- ==========================================
-- END OF TDSDB DEPLOYMENT
-- ==========================================
