-- =====================================================
-- Docker init script for TDSDB
-- Module: TDS (Tax Deduction at Source)
-- Runs on first container startup via sqlcmd
-- =====================================================

USE MASTER;
GO

-- 1. Create database
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'TDSDB')
BEGIN
    CREATE DATABASE [TDSDB];
    PRINT '+ TDSDB created';
END
ELSE
    PRINT '+ TDSDB already exists';
GO

USE [TDSDB];
GO

-- 2. Tables

-- TDS_VENDORS
-- Purpose: Stores vendor information for TDS reporting
IF OBJECT_ID(N'[dbo].[TDS_VENDORS]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[TDS_VENDORS] (
        [VENDOR_ID]      BIGINT        NULL,
        [VENDOR_NAME]    VARCHAR(240)  NULL,
        [EMAIL_ADDRESS]  VARCHAR(3000) NULL,
        [PAN_NO]         VARCHAR(30)   NULL
    );
    PRINT '+ Table TDS_VENDORS created';
END
ELSE
    PRINT '+ Table TDS_VENDORS already exists';
GO

-- TDSFILE_DETAILS
-- Purpose: Tracks TDS file uploads and processing
IF OBJECT_ID(N'[dbo].[TDSFILE_DETAILS]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[TDSFILE_DETAILS] (
        [FILE_ID]       BIGINT        NOT NULL,
        [FILE_NAME]     VARCHAR(100)  NULL,
        [PAN_NO]        VARCHAR(15)   NULL,
        [EMAIL_STATUS]  VARCHAR(1)    NULL,
        [FILE_TYPE]     VARCHAR(3)    NULL,
        CONSTRAINT [PK_TDSFILE_DETAILS] PRIMARY KEY ([FILE_ID])
    );
    PRINT '+ Table TDSFILE_DETAILS created';
END
ELSE
    PRINT '+ Table TDSFILE_DETAILS already exists';
GO

-- 3. Indexes

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_TDS_VENDORS_PANNO' AND object_id = OBJECT_ID('dbo.TDS_VENDORS'))
BEGIN
    CREATE INDEX [IDX_TDS_VENDORS_PANNO] ON [dbo].[TDS_VENDORS]([PAN_NO]);
    PRINT '+ Index IDX_TDS_VENDORS_PANNO created';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_TDSFILE_PANNO' AND object_id = OBJECT_ID('dbo.TDSFILE_DETAILS'))
BEGIN
    CREATE INDEX [IDX_TDSFILE_PANNO] ON [dbo].[TDSFILE_DETAILS]([PAN_NO]);
    PRINT '+ Index IDX_TDSFILE_PANNO created';
END
GO

-- 4. EF Core Migrations History

IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
    PRINT '+ Table __EFMigrationsHistory created';
END
GO

-- Mark the initial migration as applied so EF Core doesn't try to run it
IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = '20260310053133_InitialCreate')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260310053133_InitialCreate', '10.0.0');
    PRINT '+ EF Core initial migration marked as applied';
END
GO

-- Seed TDS_VENDORS
IF NOT EXISTS (SELECT 1 FROM [dbo].[TDS_VENDORS])
BEGIN
    INSERT INTO [dbo].[TDS_VENDORS] ([VENDOR_ID], [VENDOR_NAME], [EMAIL_ADDRESS], [PAN_NO])
    VALUES
        (1001, 'Acme Corporation Ltd', 'vendor1@acmecorp.com', 'AAACR5055K'),
        (1002, 'Global Supplies Inc', 'contact@globalsupplies.com', 'BGLS5022M'),
        (1003, 'TechPro Solutions', 'sales@techpro.com', 'CTEC4018P'),
        (1004, 'Premium Services Group', 'info@premiumservices.com', 'DPRS3009Q'),
        (1005, 'International Trade Co', 'trade@intltrade.com', 'EINT2015R'),
        (1006, 'Excel Enterprises', 'vendor@excelent.com', 'FEXC1020S'),
        (1007, 'Quality Materials Ltd', 'supplier@qualmat.com', 'GQUA6035T'),
        (1008, 'Standard Industries', 'sales@stdind.com', 'HSTD7042U'),
        (1009, 'Metro Distribution', 'metro@metrodist.com', 'IMET8019V'),
        (1010, 'Prime Logistics', 'logistics@prime.com', 'JPRI9025W');
    PRINT '+ TDS_VENDORS sample data inserted (10 records)';
END
ELSE
BEGIN
    PRINT '+ TDS_VENDORS already contains data';
END
GO

-- Seed TDSFILE_DETAILS
IF NOT EXISTS (SELECT 1 FROM [dbo].[TDSFILE_DETAILS])
BEGIN
    INSERT INTO [dbo].[TDSFILE_DETAILS] ([FILE_ID], [FILE_NAME], [PAN_NO], [EMAIL_STATUS], [FILE_TYPE])
    VALUES
        (2001, 'TDS_Q1_2026_Acme.xlsx', 'AAACR5055K', 'P', 'XLS'),
        (2002, 'TDS_Q1_2026_Global.xlsx', 'BGLS5022M', 'S', 'XLS'),
        (2003, 'TDS_Q1_2026_TechPro.csv', 'CTEC4018P', 'P', 'CSV'),
        (2004, 'TDS_Q1_2026_Premium.xlsx', 'DPRS3009Q', 'S', 'XLS'),
        (2005, 'TDS_Q1_2026_IntlTrade.txt', 'EINT2015R', 'F', 'TXT'),
        (2006, 'TDS_Q2_2026_Excel.xlsx', 'FEXC1020S', 'P', 'XLS'),
        (2007, 'TDS_Q2_2026_Quality.csv', 'GQUA6035T', 'S', 'CSV'),
        (2008, 'TDS_Q2_2026_Standard.xlsx', 'HSTD7042U', 'P', 'XLS'),
        (2009, 'TDS_Q2_2026_Metro.txt', 'IMET8019V', 'S', 'TXT'),
        (2010, 'TDS_Q2_2026_Prime.xlsx', 'JPRI9025W', 'P', 'XLS'),
        (2011, 'TDS_Final_2025_Acme.xlsx', 'AAACR5055K', 'S', 'XLS'),
        (2012, 'TDS_Final_2025_Global.csv', 'BGLS5022M', 'S', 'CSV'),
        (2013, 'TDS_Final_2025_TechPro.xlsx', 'CTEC4018P', 'S', 'XLS'),
        (2014, 'TDS_Final_2025_Premium.txt', 'DPRS3009Q', 'F', 'TXT'),
        (2015, 'TDS_Final_2025_IntlTrade.xlsx', 'EINT2015R', 'S', 'XLS');
    PRINT '+ TDSFILE_DETAILS sample data inserted (15 records)';
END
ELSE
BEGIN
    PRINT '+ TDSFILE_DETAILS already contains data';
END
GO

-- 5. Summary

PRINT '';
PRINT '========================================';
PRINT 'TDSDB Initialization Complete';
PRINT '========================================';
PRINT 'Database: TDSDB';
PRINT 'Module: TDS (Tax Deduction at Source)';
PRINT 'Version: 1.0.0';
PRINT 'Status: Successfully Deployed';
PRINT '';
PRINT 'Objects Created:';
PRINT '  + 2 Tables (TDS_VENDORS, TDSFILE_DETAILS)';
PRINT '  + 2 Indexes (PAN_NO indexes)';
PRINT '';
PRINT 'Sample Data Seeded:';
PRINT '  + 10 Vendor records (TDS_VENDORS)';
PRINT '  + 15 File records (TDSFILE_DETAILS)';
PRINT '========================================';
PRINT '';
GO

-- =====================================================
-- END OF init-database.sql
-- =====================================================
