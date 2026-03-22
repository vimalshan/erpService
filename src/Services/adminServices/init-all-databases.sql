-- ================================================================
-- Combined Database Initialization Script for Admin Services
-- ================================================================
-- This script combines all individual init-database.sql scripts
-- from each admin microservice into a single initialization file.
-- 
-- Services included:
--   1. Finyear Service     (ADMINDB)
--   2. Location Service    (LOCATIONDB)
--   3. Vendor Service      (VENDORDB)
--   4. LOV Service         (LOVDB)
--   5. Scholarship Service (ADMINDB)
--   6. Stationery Service  (STATIONERYDB)
--   7. TDS Service         (TDSDB)
--   8. Transaction Service (ADMINDB)
--
-- Generated on: 2026-03-22
-- ================================================================


-- ============================================================
-- Finyear Service
-- ============================================================

-- Create ADMINDB database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ADMINDB')
BEGIN
    CREATE DATABASE ADMINDB;
END
GO

USE ADMINDB;
GO

-- Create FINYEAR_MASTER table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FINYEAR_MASTER')
BEGIN
    CREATE TABLE FINYEAR_MASTER (
        [FY_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
        [FY_NAME] NVARCHAR(27) NOT NULL,
        [FY_STARTDATE] DATETIME2(3) NOT NULL,
        [FY_CLOSEDATE] DATETIME2(3) NOT NULL,
        [FY_UPDATED_BY] BIGINT DEFAULT 1,
        [FY_UPDATED_ON] DATETIME2(3) DEFAULT GETDATE()
    );
    
    -- Create index on FY_NAME
    CREATE NONCLUSTERED INDEX [IX_FINYEAR_MASTER_NAME] ON [FINYEAR_MASTER] ([FY_NAME]);
    
    -- Insert sample data
    INSERT INTO FINYEAR_MASTER ([FY_NAME], [FY_STARTDATE], [FY_CLOSEDATE], [FY_UPDATED_BY], [FY_UPDATED_ON])
    VALUES 
        ('FY 2024-25', '2024-04-01', '2025-03-31', 1, GETDATE()),
        ('FY 2025-26', '2025-04-01', '2026-03-31', 1, GETDATE()),
        ('FY 2026-27', '2026-04-01', '2027-03-31', 1, GETDATE());
END
GO


GO


-- ============================================================
-- Location Service
-- ============================================================

-- =====================================================
-- Docker init script for LOCATIONDB
-- Runs automatically on first container startup
-- =====================================================

USE MASTER;
GO

-- Create LOCATIONDB database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LOCATIONDB')
BEGIN
    CREATE DATABASE [LOCATIONDB];
    PRINT '+ LOCATIONDB created';
END
ELSE
    PRINT '+ LOCATIONDB already exists';
GO

USE [LOCATIONDB];
GO

-- =====================================================
-- LOCATION_APP_MAP  (base table)
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOCATION_APP_MAP]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[LOCATION_APP_MAP]
    (
        [LOCATION_ID]        DECIMAL(22,0)  NOT NULL,
        [APP_NAME]           VARCHAR(255)   NOT NULL,
        [SITE_CATEGORY_CODE] BIGINT         NULL,
        [SELF_ACCESS]        VARCHAR(255)   NULL,
        [DEEMED_APPROVAL]    CHAR(1)        NULL,
        [CREATED_DATE]       DATETIME       NOT NULL DEFAULT GETUTCDATE(),
        [CREATED_BY]         VARCHAR(100)   NULL,
        [MODIFIED_DATE]      DATETIME       NULL,
        [MODIFIED_BY]        VARCHAR(100)   NULL,
        [IS_ACTIVE]          BIT            NOT NULL DEFAULT 1,
        CONSTRAINT [PK_LOCATION_APP_MAP] PRIMARY KEY ([LOCATION_ID], [APP_NAME])
    );
    PRINT '+ LOCATION_APP_MAP created';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_APPNAME' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_APPNAME]    ON [dbo].[LOCATION_APP_MAP]([APP_NAME]);
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_LOCATIONID' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_LOCATIONID] ON [dbo].[LOCATION_APP_MAP]([LOCATION_ID]);
GO
SET QUOTED_IDENTIFIER ON;
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_ACTIVE' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_ACTIVE]    ON [dbo].[LOCATION_APP_MAP]([IS_ACTIVE], [LOCATION_ID]) WHERE [IS_ACTIVE] = 1;
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_CATEGORY' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_CATEGORY]  ON [dbo].[LOCATION_APP_MAP]([SITE_CATEGORY_CODE]) WHERE [SITE_CATEGORY_CODE] IS NOT NULL;
GO

-- =====================================================
-- AUDIT_LOG
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AUDIT_LOG]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[AUDIT_LOG]
    (
        [AUDIT_ID]       BIGINT        IDENTITY(1,1) PRIMARY KEY,
        [TABLE_NAME]     VARCHAR(128)  NOT NULL,
        [RECORD_ID]      VARCHAR(500)  NOT NULL,
        [OPERATION_TYPE] VARCHAR(10)   NOT NULL,
        [OLD_VALUES]     NVARCHAR(MAX) NULL,
        [NEW_VALUES]     NVARCHAR(MAX) NULL,
        [CHANGED_BY]     VARCHAR(100)  NOT NULL,
        [CHANGED_DATE]   DATETIME      NOT NULL DEFAULT GETUTCDATE(),
        [IP_ADDRESS]     VARCHAR(50)   NULL
    );
    CREATE INDEX [IDX_AUDIT_TABLE_NAME]   ON [dbo].[AUDIT_LOG]([TABLE_NAME]);
    CREATE INDEX [IDX_AUDIT_CHANGED_DATE] ON [dbo].[AUDIT_LOG]([CHANGED_DATE]);
    PRINT '+ AUDIT_LOG created';
END
GO

-- =====================================================
-- LOCATION_APP_MAP_HISTORY
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOCATION_APP_MAP_HISTORY]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[LOCATION_APP_MAP_HISTORY]
    (
        [HISTORY_ID]         BIGINT        IDENTITY(1,1) PRIMARY KEY,
        [LOCATION_ID]        DECIMAL(22,0) NOT NULL,
        [APP_NAME]           VARCHAR(255)  NOT NULL,
        [SITE_CATEGORY_CODE] BIGINT        NULL,
        [SELF_ACCESS]        VARCHAR(255)  NULL,
        [DEEMED_APPROVAL]    CHAR(1)       NULL,
        [IS_ACTIVE]          BIT           NOT NULL DEFAULT 1,
        [CHANGED_BY]         VARCHAR(100)  NOT NULL,
        [CHANGED_DATE]       DATETIME      NOT NULL DEFAULT GETUTCDATE(),
        [OPERATION_TYPE]     VARCHAR(10)   NOT NULL
    );
    CREATE INDEX [IDX_HISTORY_LOCATION] ON [dbo].[LOCATION_APP_MAP_HISTORY]([LOCATION_ID]);
    CREATE INDEX [IDX_HISTORY_DATE]     ON [dbo].[LOCATION_APP_MAP_HISTORY]([CHANGED_DATE]);
    PRINT '+ LOCATION_APP_MAP_HISTORY created';
END
GO

-- =====================================================
-- Seed data
-- =====================================================
IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP])
BEGIN
    INSERT INTO [dbo].[LOCATION_APP_MAP]
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
    VALUES
        (1001, 'WarehouseApp',      100, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (1001, 'InventoryApp',      101, 'Y', 'N', GETUTCDATE(), 'Admin', 1),
        (1001, 'ReportingApp',      102, 'N', 'Y', GETUTCDATE(), 'Admin', 1),
        (2001, 'ManufacturingApp',  200, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (2001, 'QualityApp',        201, 'Y', 'N', GETUTCDATE(), 'Admin', 1),
        (3001, 'RetailApp',         300, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (3001, 'POSApp',            301, 'N', 'N', GETUTCDATE(), 'Admin', 1),
        (4001, 'OfficeApp',         400, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (4001, 'HRApp',             401, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (5001, 'DistributionApp',   500, 'Y', 'N', GETUTCDATE(), 'Admin', 1);
    PRINT '+ Seed data inserted (10 rows)';
END
ELSE
    PRINT '+ Seed data already present, skipping';
GO

PRINT '';
PRINT '=== LOCATIONDB initialisation complete ===';
GO


GO


-- ============================================================
-- Vendor Service
-- ============================================================

-- =====================================================
-- Docker init script for VENDORDB
-- Runs on first container startup via sqlcmd
-- =====================================================

USE MASTER;
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 1. Create database
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'VENDORDB')
BEGIN
    CREATE DATABASE [VENDORDB];
    PRINT '+ VENDORDB created';
END
ELSE
    PRINT '+ VENDORDB already exists';
GO

USE [VENDORDB];
GO

-- ==================================================
-- 2. Create Tables
-- ==================================================

-- Table: VENDOR_MASTER
-- Purpose: Master list of vendors for procurement
IF OBJECT_ID(N'[dbo].[VENDOR_MASTER]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[VENDOR_MASTER] (
        [VM_ID]           BIGINT        NOT NULL,
        [VM_CATID]        BIGINT        NOT NULL,
        [VM_LOC_ID]       BIGINT        NOT NULL,
        [VM_NAME]         VARCHAR(100)  NOT NULL,
        [VM_EMAIL]        VARCHAR(50)   NULL,
        [VM_ADDRESS]      VARCHAR(200)  NOT NULL,
        [VM_UPDATED_BY]   BIGINT        NOT NULL,
        [VM_UPDATED_ON]   DATETIME2(3)  NOT NULL,
        [VM_LIVESTATUS]   CHAR(1)       NOT NULL,
        CONSTRAINT [PK_VENDOR_MASTER] PRIMARY KEY ([VM_ID])
    );
    PRINT '+ Table VENDOR_MASTER created';
END
ELSE
    PRINT '+ Table VENDOR_MASTER already exists';
GO

-- Create indexes for VENDOR_MASTER
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_VENDOR_MASTER_LOCID')
    CREATE INDEX [IDX_VENDOR_MASTER_LOCID] ON [dbo].[VENDOR_MASTER]([VM_LOC_ID]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_VENDOR_MASTER_STATUS')
    CREATE INDEX [IDX_VENDOR_MASTER_STATUS] ON [dbo].[VENDOR_MASTER]([VM_LIVESTATUS]);
PRINT '+ Indexes created on VENDOR_MASTER';
GO

-- Table: TDS_VENDORS
-- Purpose: TDS vendor information from tax files
IF OBJECT_ID(N'[dbo].[TDS_VENDORS]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[TDS_VENDORS] (
        [VENDOR_ID]     BIGINT        NULL,
        [VENDOR_NAME]   VARCHAR(240)  NULL,
        [EMAIL_ADDRESS] VARCHAR(3000) NULL,
        [PAN_NO]        VARCHAR(30)   NULL
    );
    PRINT '+ Table TDS_VENDORS created';
END
ELSE
    PRINT '+ Table TDS_VENDORS already exists';
GO

-- Table: TDSFILE_DETAILS
-- Purpose: TDS file transaction details
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

-- ==================================================
-- 3. Create Stored Procedures
-- ==================================================

-- Procedure: usp_AddUpdateVendor
-- Purpose: Insert or update vendor master records with transaction support
IF OBJECT_ID(N'[dbo].[usp_AddUpdateVendor]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_AddUpdateVendor];
GO
CREATE PROCEDURE [dbo].[usp_AddUpdateVendor]
(
    @p_VM_ID          BIGINT        = NULL,
    @p_VM_CATID       BIGINT,
    @p_VM_LOC_ID      BIGINT,
    @p_VM_NAME        VARCHAR(100),
    @p_VM_EMAIL       VARCHAR(50)   = NULL,
    @p_VM_ADDRESS     VARCHAR(200),
    @p_UpdatedBy      BIGINT,
    @p_VM_LIVESTATUS  CHAR(1)       = 'A'
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @p_VM_ID IS NULL
        BEGIN
            -- Insert new vendor
            SELECT @p_VM_ID = ISNULL(MAX(VM_ID), 0) + 1 FROM VENDOR_MASTER;
            INSERT INTO VENDOR_MASTER
            (
                VM_ID, VM_CATID, VM_LOC_ID, VM_NAME, VM_EMAIL,
                VM_ADDRESS, VM_UPDATED_BY, VM_UPDATED_ON, VM_LIVESTATUS
            )
            VALUES
            (
                @p_VM_ID, @p_VM_CATID, @p_VM_LOC_ID, @p_VM_NAME, @p_VM_EMAIL,
                @p_VM_ADDRESS, @p_UpdatedBy, GETDATE(), @p_VM_LIVESTATUS
            );
        END
        ELSE
        BEGIN
            -- Update existing vendor
            UPDATE VENDOR_MASTER
            SET VM_CATID        = @p_VM_CATID,
                VM_LOC_ID       = @p_VM_LOC_ID,
                VM_NAME         = @p_VM_NAME,
                VM_EMAIL        = @p_VM_EMAIL,
                VM_ADDRESS      = @p_VM_ADDRESS,
                VM_UPDATED_BY   = @p_UpdatedBy,
                VM_UPDATED_ON   = GETDATE(),
                VM_LIVESTATUS   = @p_VM_LIVESTATUS
            WHERE VM_ID = @p_VM_ID;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '+ Procedure usp_AddUpdateVendor created';
GO

-- ==================================================
-- 4. Create Triggers
-- ==================================================

-- Trigger: trg_VendorMaster_UpdateAudit
-- Purpose: Automatically update VM_UPDATED_ON on vendor master changes
IF OBJECT_ID(N'[dbo].[trg_VendorMaster_UpdateAudit]', N'TR') IS NOT NULL
    DROP TRIGGER [dbo].[trg_VendorMaster_UpdateAudit];
GO
CREATE TRIGGER [dbo].[trg_VendorMaster_UpdateAudit]
ON [dbo].[VENDOR_MASTER]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE VM
    SET VM_UPDATED_ON = GETDATE()
    FROM dbo.VENDOR_MASTER VM
    INNER JOIN inserted I ON VM.VM_ID = I.VM_ID;
END;
GO
PRINT '+ Trigger trg_VendorMaster_UpdateAudit created';
GO

-- ==================================================
-- 5. Seed Data
-- ==================================================

PRINT '';
PRINT 'Seeding initial vendor data...';
GO

-- Insert seed data into VENDOR_MASTER
IF (SELECT COUNT(*) FROM [dbo].[VENDOR_MASTER]) = 0
BEGIN
    INSERT INTO [dbo].[VENDOR_MASTER]
    (
        [VM_ID], [VM_CATID], [VM_LOC_ID], [VM_NAME], [VM_EMAIL],
        [VM_ADDRESS], [VM_UPDATED_BY], [VM_UPDATED_ON], [VM_LIVESTATUS]
    )
    VALUES
    (1, 1, 1, 'ABC Stationery Supplies', 'sales@abcstationery.com', '123 Market Street, New Delhi', 1, GETDATE(), 'A'),
    (2, 1, 1, 'XYZ Office Equipment', 'contact@xyzoffice.com', '456 Business Park, Gurgaon', 1, GETDATE(), 'A'),
    (3, 2, 2, 'Tech Solutions Ltd', 'support@techsol.com', '789 Tech Way, Bangalore', 1, GETDATE(), 'A'),
    (4, 1, 1, 'Premium Office Supplies', 'info@premiumoffice.com', '321 Corporate Blvd, Noida', 1, GETDATE(), 'A'),
    (5, 3, 3, 'Global Vendors Inc', 'sales@globalvendors.com', '654 Industrial Zone, Chennai', 1, GETDATE(), 'A');
    PRINT '+ Inserted 5 seed records into VENDOR_MASTER';
END
ELSE
    PRINT '+ VENDOR_MASTER already contains data, skipping seed insertion';
GO

-- Insert seed data into TDS_VENDORS
IF (SELECT COUNT(*) FROM [dbo].[TDS_VENDORS]) = 0
BEGIN
    INSERT INTO [dbo].[TDS_VENDORS]
    (
        [VENDOR_ID], [VENDOR_NAME], [EMAIL_ADDRESS], [PAN_NO]
    )
    VALUES
    (1, 'ABC Stationery Supplies', 'sales@abcstationery.com', 'AAABR5055K'),
    (2, 'XYZ Office Equipment', 'contact@xyzoffice.com', 'XYZOP1234M'),
    (3, 'Tech Solutions Ltd', 'support@techsol.com', 'AAAAK5678P'),
    (4, 'Premium Office Supplies', 'info@premiumoffice.com', 'PREM5001Q');
    PRINT '+ Inserted 4 seed records into TDS_VENDORS';
END
ELSE
    PRINT '+ TDS_VENDORS already contains data, skipping seed insertion';
GO

-- ==================================================
-- 6. Verification
-- ==================================================

PRINT '';
PRINT 'Verifying VENDORDB setup...';
GO

SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;

SELECT 'Vendor Count: ' + CAST(COUNT(*) AS VARCHAR(10)) AS [Verification]
FROM [dbo].[VENDOR_MASTER];
GO

PRINT '';
PRINT '======================================================';
PRINT 'VENDORDB initialisation complete. Service is ready.';
PRINT '======================================================';
GO
BEGIN
    CREATE TABLE [dbo].[LOV_TYPE] (
        [LOV_TYPE_ID]   BIGINT        NOT NULL,
        [LOV_TYPE_NAME] NVARCHAR(30)  NOT NULL,
        CONSTRAINT [PK_LOV_TYPE] PRIMARY KEY ([LOV_TYPE_ID])
    );
    PRINT '+ Table LOV_TYPE created';
END
ELSE
    PRINT '+ Table LOV_TYPE already exists';
GO

-- LOV_MASTER
IF OBJECT_ID(N'[dbo].[LOV_MASTER]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[LOV_MASTER] (
        [LOV_ID]         BIGINT        NOT NULL,
        [LOV_TYPE_ID]    BIGINT        NOT NULL,
        [LOV_NAME]       NVARCHAR(30)  NOT NULL,
        [LOV_UPDATED_BY] BIGINT        NOT NULL,
        [LOV_UPDATED_ON] DATETIME2(3)  NOT NULL,
        CONSTRAINT [PK_LOV_MASTER]      PRIMARY KEY ([LOV_ID]),
        CONSTRAINT [FK_LOV_MASTER_TYPE] FOREIGN KEY ([LOV_TYPE_ID]) REFERENCES [dbo].[LOV_TYPE]([LOV_TYPE_ID])
    );
    CREATE INDEX [IDX_LOV_MASTER_TYPEID] ON [dbo].[LOV_MASTER]([LOV_TYPE_ID]);
    PRINT '+ Table LOV_MASTER created';
END
ELSE
    PRINT '+ Table LOV_MASTER already exists';
GO

-- ITEMDATA
IF OBJECT_ID(N'[dbo].[ITEMDATA]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ITEMDATA] (
        [ID]       INT IDENTITY(1,1) NOT NULL,
        [CATNAME]  NVARCHAR(40) NULL,
        [ITEMNAME] NVARCHAR(60) NULL,
        [MAKE]     NVARCHAR(30) NULL,
        [UOM]      NVARCHAR(20) NULL,
        [PRICE]    INT          NULL,
        CONSTRAINT [PK_ITEMDATA] PRIMARY KEY ([ID])
    );
    PRINT '+ Table ITEMDATA created';
END
ELSE
    PRINT '+ Table ITEMDATA already exists';
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 3. Stored Procedures
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬

-- usp_GetAllLovTypes
IF OBJECT_ID(N'[dbo].[usp_GetAllLovTypes]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_GetAllLovTypes];
GO
CREATE PROCEDURE [dbo].[usp_GetAllLovTypes]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT LOV_TYPE_ID AS LovTypeId, LOV_TYPE_NAME AS LovTypeName
    FROM [dbo].[LOV_TYPE] WITH (NOLOCK)
    ORDER BY LOV_TYPE_NAME;
END
GO
PRINT '+ usp_GetAllLovTypes created';
GO

-- usp_GetLovMastersByType
IF OBJECT_ID(N'[dbo].[usp_GetLovMastersByType]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_GetLovMastersByType];
GO
CREATE PROCEDURE [dbo].[usp_GetLovMastersByType]
    @LovTypeId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        LOV_ID         AS LovId,
        LOV_TYPE_ID    AS LovTypeId,
        LOV_NAME       AS LovName,
        LOV_UPDATED_BY AS LovUpdatedBy,
        LOV_UPDATED_ON AS LovUpdatedOn
    FROM [dbo].[LOV_MASTER] WITH (NOLOCK)
    WHERE LOV_TYPE_ID = @LovTypeId
    ORDER BY LOV_NAME;
END
GO
PRINT '+ usp_GetLovMastersByType created';
GO

-- usp_UpsertLovType
IF OBJECT_ID(N'[dbo].[usp_UpsertLovType]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_UpsertLovType];
GO
CREATE PROCEDURE [dbo].[usp_UpsertLovType]
    @LovTypeId   BIGINT,
    @LovTypeName VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM [dbo].[LOV_TYPE] WHERE LOV_TYPE_ID = @LovTypeId)
        UPDATE [dbo].[LOV_TYPE] SET LOV_TYPE_NAME = @LovTypeName WHERE LOV_TYPE_ID = @LovTypeId;
    ELSE
        INSERT INTO [dbo].[LOV_TYPE] (LOV_TYPE_ID, LOV_TYPE_NAME) VALUES (@LovTypeId, @LovTypeName);
END
GO
PRINT '+ usp_UpsertLovType created';
GO

-- usp_UpsertLovMaster
IF OBJECT_ID(N'[dbo].[usp_UpsertLovMaster]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_UpsertLovMaster];
GO
CREATE PROCEDURE [dbo].[usp_UpsertLovMaster]
    @LovId        BIGINT,
    @LovTypeId    BIGINT,
    @LovName      VARCHAR(30),
    @LovUpdatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME2(3) = SYSDATETIME();
    IF EXISTS (SELECT 1 FROM [dbo].[LOV_MASTER] WHERE LOV_ID = @LovId)
        UPDATE [dbo].[LOV_MASTER]
        SET LOV_NAME = @LovName, LOV_UPDATED_BY = @LovUpdatedBy, LOV_UPDATED_ON = @Now
        WHERE LOV_ID = @LovId;
    ELSE
        INSERT INTO [dbo].[LOV_MASTER] (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
        VALUES (@LovId, @LovTypeId, @LovName, @LovUpdatedBy, @Now);
END
GO
PRINT '+ usp_UpsertLovMaster created';
GO

-- usp_DeleteLovType
IF OBJECT_ID(N'[dbo].[usp_DeleteLovType]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_DeleteLovType];
GO
CREATE PROCEDURE [dbo].[usp_DeleteLovType]
    @LovTypeId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[LOV_MASTER] WHERE LOV_TYPE_ID = @LovTypeId;
    DELETE FROM [dbo].[LOV_TYPE]   WHERE LOV_TYPE_ID = @LovTypeId;
END
GO
PRINT '+ usp_DeleteLovType created';
GO

-- usp_SearchItemData
IF OBJECT_ID(N'[dbo].[usp_SearchItemData]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_SearchItemData];
GO
CREATE PROCEDURE [dbo].[usp_SearchItemData]
    @CatName  VARCHAR(40) = NULL,
    @ItemName VARCHAR(60) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, CATNAME, ITEMNAME, MAKE, UOM, PRICE
    FROM [dbo].[ITEMDATA] WITH (NOLOCK)
    WHERE (@CatName  IS NULL OR CATNAME  LIKE '%' + @CatName  + '%')
      AND (@ItemName IS NULL OR ITEMNAME LIKE '%' + @ItemName + '%')
    ORDER BY CATNAME, ITEMNAME;
END
GO
PRINT '+ usp_SearchItemData created';
GO

-- ==================================================
-- 6. Verification
-- ==================================================

PRINT '';
PRINT 'Verifying VENDORDB setup...';
GO

SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;

SELECT 'Vendor Count: ' + CAST(COUNT(*) AS VARCHAR(10)) AS [Verification]
FROM [dbo].[VENDOR_MASTER];
GO

PRINT '';
PRINT '======================================================';
PRINT 'VENDORDB initialisation complete. Service is ready.';
PRINT '======================================================';
GO

-- =====================================================
-- END OF init-database.sql
-- =====================================================

GO


-- ============================================================
-- LOV Service
-- ============================================================

-- =====================================================
-- Docker init script for LOVDB
-- Runs on first container startup via sqlcmd
-- =====================================================

USE MASTER;
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 1. Create database
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'LOVDB')
BEGIN
    CREATE DATABASE [LOVDB];
    PRINT '+ LOVDB created';
END
ELSE
    PRINT '+ LOVDB already exists';
GO

USE [LOVDB];
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 2. Tables
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬

-- LOV_TYPE
IF OBJECT_ID(N'[dbo].[LOV_TYPE]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[LOV_TYPE] (
        [LOV_TYPE_ID]   BIGINT        NOT NULL,
        [LOV_TYPE_NAME] NVARCHAR(30)  NOT NULL,
        CONSTRAINT [PK_LOV_TYPE] PRIMARY KEY ([LOV_TYPE_ID])
    );
    PRINT '+ Table LOV_TYPE created';
END
ELSE
    PRINT '+ Table LOV_TYPE already exists';
GO

-- LOV_MASTER
IF OBJECT_ID(N'[dbo].[LOV_MASTER]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[LOV_MASTER] (
        [LOV_ID]         BIGINT        NOT NULL,
        [LOV_TYPE_ID]    BIGINT        NOT NULL,
        [LOV_NAME]       NVARCHAR(30)  NOT NULL,
        [LOV_UPDATED_BY] BIGINT        NOT NULL,
        [LOV_UPDATED_ON] DATETIME2(3)  NOT NULL,
        CONSTRAINT [PK_LOV_MASTER]      PRIMARY KEY ([LOV_ID]),
        CONSTRAINT [FK_LOV_MASTER_TYPE] FOREIGN KEY ([LOV_TYPE_ID]) REFERENCES [dbo].[LOV_TYPE]([LOV_TYPE_ID])
    );
    CREATE INDEX [IDX_LOV_MASTER_TYPEID] ON [dbo].[LOV_MASTER]([LOV_TYPE_ID]);
    PRINT '+ Table LOV_MASTER created';
END
ELSE
    PRINT '+ Table LOV_MASTER already exists';
GO

-- ITEMDATA
IF OBJECT_ID(N'[dbo].[ITEMDATA]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ITEMDATA] (
        [ID]       INT IDENTITY(1,1) NOT NULL,
        [CATNAME]  NVARCHAR(40) NULL,
        [ITEMNAME] NVARCHAR(60) NULL,
        [MAKE]     NVARCHAR(30) NULL,
        [UOM]      NVARCHAR(20) NULL,
        [PRICE]    INT          NULL,
        CONSTRAINT [PK_ITEMDATA] PRIMARY KEY ([ID])
    );
    PRINT '+ Table ITEMDATA created';
END
ELSE
    PRINT '+ Table ITEMDATA already exists';
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 3. Stored Procedures
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬

-- usp_GetAllLovTypes
IF OBJECT_ID(N'[dbo].[usp_GetAllLovTypes]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_GetAllLovTypes];
GO
CREATE PROCEDURE [dbo].[usp_GetAllLovTypes]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT LOV_TYPE_ID AS LovTypeId, LOV_TYPE_NAME AS LovTypeName
    FROM [dbo].[LOV_TYPE] WITH (NOLOCK)
    ORDER BY LOV_TYPE_NAME;
END
GO
PRINT '+ usp_GetAllLovTypes created';
GO

-- usp_GetLovMastersByType
IF OBJECT_ID(N'[dbo].[usp_GetLovMastersByType]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_GetLovMastersByType];
GO
CREATE PROCEDURE [dbo].[usp_GetLovMastersByType]
    @LovTypeId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        LOV_ID         AS LovId,
        LOV_TYPE_ID    AS LovTypeId,
        LOV_NAME       AS LovName,
        LOV_UPDATED_BY AS LovUpdatedBy,
        LOV_UPDATED_ON AS LovUpdatedOn
    FROM [dbo].[LOV_MASTER] WITH (NOLOCK)
    WHERE LOV_TYPE_ID = @LovTypeId
    ORDER BY LOV_NAME;
END
GO
PRINT '+ usp_GetLovMastersByType created';
GO

-- usp_UpsertLovType
IF OBJECT_ID(N'[dbo].[usp_UpsertLovType]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_UpsertLovType];
GO
CREATE PROCEDURE [dbo].[usp_UpsertLovType]
    @LovTypeId   BIGINT,
    @LovTypeName VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM [dbo].[LOV_TYPE] WHERE LOV_TYPE_ID = @LovTypeId)
        UPDATE [dbo].[LOV_TYPE] SET LOV_TYPE_NAME = @LovTypeName WHERE LOV_TYPE_ID = @LovTypeId;
    ELSE
        INSERT INTO [dbo].[LOV_TYPE] (LOV_TYPE_ID, LOV_TYPE_NAME) VALUES (@LovTypeId, @LovTypeName);
END
GO
PRINT '+ usp_UpsertLovType created';
GO

-- usp_UpsertLovMaster
IF OBJECT_ID(N'[dbo].[usp_UpsertLovMaster]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_UpsertLovMaster];
GO
CREATE PROCEDURE [dbo].[usp_UpsertLovMaster]
    @LovId        BIGINT,
    @LovTypeId    BIGINT,
    @LovName      VARCHAR(30),
    @LovUpdatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME2(3) = SYSDATETIME();
    IF EXISTS (SELECT 1 FROM [dbo].[LOV_MASTER] WHERE LOV_ID = @LovId)
        UPDATE [dbo].[LOV_MASTER]
        SET LOV_NAME = @LovName, LOV_UPDATED_BY = @LovUpdatedBy, LOV_UPDATED_ON = @Now
        WHERE LOV_ID = @LovId;
    ELSE
        INSERT INTO [dbo].[LOV_MASTER] (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
        VALUES (@LovId, @LovTypeId, @LovName, @LovUpdatedBy, @Now);
END
GO
PRINT '+ usp_UpsertLovMaster created';
GO

-- usp_DeleteLovType
IF OBJECT_ID(N'[dbo].[usp_DeleteLovType]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_DeleteLovType];
GO
CREATE PROCEDURE [dbo].[usp_DeleteLovType]
    @LovTypeId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[LOV_MASTER] WHERE LOV_TYPE_ID = @LovTypeId;
    DELETE FROM [dbo].[LOV_TYPE]   WHERE LOV_TYPE_ID = @LovTypeId;
END
GO
PRINT '+ usp_DeleteLovType created';
GO

-- usp_SearchItemData
IF OBJECT_ID(N'[dbo].[usp_SearchItemData]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_SearchItemData];
GO
CREATE PROCEDURE [dbo].[usp_SearchItemData]
    @CatName  VARCHAR(40) = NULL,
    @ItemName VARCHAR(60) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, CATNAME, ITEMNAME, MAKE, UOM, PRICE
    FROM [dbo].[ITEMDATA] WITH (NOLOCK)
    WHERE (@CatName  IS NULL OR CATNAME  LIKE '%' + @CatName  + '%')
      AND (@ItemName IS NULL OR ITEMNAME LIKE '%' + @ItemName + '%')
    ORDER BY CATNAME, ITEMNAME;
END
GO
PRINT '+ usp_SearchItemData created';
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 4. Sample Data
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬

-- LOV_TYPE
MERGE INTO [dbo].[LOV_TYPE] AS target
USING (VALUES
    (1, 'CATEGORY'),
    (2, 'STATUS'),
    (3, 'PRIORITY'),
    (4, 'DEPARTMENT'),
    (5, 'UOM'),
    (6, 'PAYMENT_MODE'),
    (7, 'TAX_TYPE'),
    (8, 'CURRENCY')
) AS source (LOV_TYPE_ID, LOV_TYPE_NAME)
ON target.LOV_TYPE_ID = source.LOV_TYPE_ID
WHEN MATCHED THEN
    UPDATE SET LOV_TYPE_NAME = source.LOV_TYPE_NAME
WHEN NOT MATCHED THEN
    INSERT (LOV_TYPE_ID, LOV_TYPE_NAME) VALUES (source.LOV_TYPE_ID, source.LOV_TYPE_NAME);
PRINT '+ LOV_TYPE sample data inserted';
GO

-- LOV_MASTER
DECLARE @Now DATETIME2(3) = SYSDATETIME();
DECLARE @UserId BIGINT = 1;

MERGE INTO [dbo].[LOV_MASTER] AS target
USING (VALUES
    -- CATEGORY (Type 1)
    (101, 1, 'Electronics',   @UserId, @Now),
    (102, 1, 'Furniture',     @UserId, @Now),
    (103, 1, 'Stationery',    @UserId, @Now),
    (104, 1, 'Consumables',   @UserId, @Now),
    -- STATUS (Type 2)
    (201, 2, 'Active',        @UserId, @Now),
    (202, 2, 'Inactive',      @UserId, @Now),
    (203, 2, 'Pending',       @UserId, @Now),
    (204, 2, 'Cancelled',     @UserId, @Now),
    -- PRIORITY (Type 3)
    (301, 3, 'High',          @UserId, @Now),
    (302, 3, 'Medium',        @UserId, @Now),
    (303, 3, 'Low',           @UserId, @Now),
    -- DEPARTMENT (Type 4)
    (401, 4, 'IT',            @UserId, @Now),
    (402, 4, 'HR',            @UserId, @Now),
    (403, 4, 'Finance',       @UserId, @Now),
    (404, 4, 'Operations',    @UserId, @Now),
    -- UOM (Type 5)
    (501, 5, 'Nos',           @UserId, @Now),
    (502, 5, 'Kg',            @UserId, @Now),
    (503, 5, 'Ltr',           @UserId, @Now),
    (504, 5, 'Box',           @UserId, @Now),
    -- PAYMENT_MODE (Type 6)
    (601, 6, 'Cash',          @UserId, @Now),
    (602, 6, 'Credit Card',   @UserId, @Now),
    (603, 6, 'Bank Transfer', @UserId, @Now),
    -- TAX_TYPE (Type 7)
    (701, 7, 'GST',           @UserId, @Now),
    (702, 7, 'VAT',           @UserId, @Now),
    (703, 7, 'Exempt',        @UserId, @Now),
    -- CURRENCY (Type 8)
    (801, 8, 'INR',           @UserId, @Now),
    (802, 8, 'USD',           @UserId, @Now),
    (803, 8, 'EUR',           @UserId, @Now)
) AS source (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
ON target.LOV_ID = source.LOV_ID
WHEN MATCHED THEN
    UPDATE SET LOV_NAME = source.LOV_NAME, LOV_UPDATED_BY = source.LOV_UPDATED_BY, LOV_UPDATED_ON = source.LOV_UPDATED_ON
WHEN NOT MATCHED THEN
    INSERT (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
    VALUES (source.LOV_ID, source.LOV_TYPE_ID, source.LOV_NAME, source.LOV_UPDATED_BY, source.LOV_UPDATED_ON);
PRINT '+ LOV_MASTER sample data inserted';
GO

-- ITEMDATA
MERGE INTO [dbo].[ITEMDATA] AS target
USING (VALUES
    ('Electronics', 'Laptop Dell XPS 15',     'Dell',     'Nos',  85000),
    ('Electronics', 'Monitor 27 inch',         'LG',       'Nos',  22000),
    ('Electronics', 'Wireless Keyboard',       'Logitech', 'Nos',   3500),
    ('Electronics', 'Wireless Mouse',          'Logitech', 'Nos',   1800),
    ('Electronics', 'USB-C Hub 7-in-1',        'Anker',    'Nos',   4200),
    ('Furniture',   'Office Chair Ergonomic',  'Herman',   'Nos',  45000),
    ('Furniture',   'Standing Desk 180cm',     'IKEA',     'Nos',  32000),
    ('Furniture',   'Bookshelf 5-tier',        'IKEA',     'Nos',   8500),
    ('Stationery',  'A4 Paper 500 sheets',     'ITC',      'Box',    550),
    ('Stationery',  'Ballpoint Pen Box',       'Cello',    'Box',    250),
    ('Stationery',  'Sticky Notes Pack',       '3M',       'Box',    320),
    ('Consumables', 'Hand Sanitizer 500ml',    'Dettol',   'Ltr',    250),
    ('Consumables', 'Printer Ink Cartridge',   'HP',       'Nos',   1200),
    ('Consumables', 'Coffee Beans 1kg',        'Lavazza',  'Kg',    1800)
) AS source (CATNAME, ITEMNAME, MAKE, UOM, PRICE)
ON target.ITEMNAME = source.ITEMNAME AND target.CATNAME = source.CATNAME
WHEN MATCHED THEN
    UPDATE SET MAKE = source.MAKE, UOM = source.UOM, PRICE = source.PRICE
WHEN NOT MATCHED THEN
    INSERT (CATNAME, ITEMNAME, MAKE, UOM, PRICE)
    VALUES (source.CATNAME, source.ITEMNAME, source.MAKE, source.UOM, source.PRICE);
PRINT '+ ITEMDATA sample data inserted';
GO

-- ──────────────────────────────────────────
-- 5. EF Core migration history
-- ──────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
    PRINT '+ Table __EFMigrationsHistory created';
END
GO

-- Mark EF Core InitialCreate migration as already applied
IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = '20260309184431_InitialCreate')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260309184431_InitialCreate', '10.0.3');
    PRINT '+ EF Core migration history seeded';
END
GO

-- Verify counts
SELECT 'LOV_TYPE'   AS TableName, COUNT(*) AS RecordCount FROM [dbo].[LOV_TYPE]
UNION ALL
SELECT 'LOV_MASTER' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[LOV_MASTER]
UNION ALL
SELECT 'ITEMDATA'   AS TableName, COUNT(*) AS RecordCount FROM [dbo].[ITEMDATA];
GO

PRINT '';
PRINT '======================================';
PRINT 'LOVDB initialisation complete';
PRINT '======================================';
GO
-- =====================================================
-- END OF init-database.sql
-- ====================

GO


-- ============================================================
-- Scholarship Service
-- ============================================================

-- =====================================================
-- Docker init script for ADMINDB (Scholarship Service)
-- Runs on first container startup via sqlcmd
-- =====================================================

USE MASTER;
GO

-- =====================================================
-- 1. Create database
-- =====================================================
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'ADMINDB')
BEGIN
    CREATE DATABASE [ADMINDB];
    PRINT '+ ADMINDB created';
END
ELSE
    PRINT '+ ADMINDB already exists';
GO

USE [ADMINDB];
GO

-- =====================================================
-- 2. Scholarship Tables
-- =====================================================

-- SCHOLARSHIP_AMOUNT
IF OBJECT_ID(N'[dbo].[SCHOLARSHIP_AMOUNT]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SCHOLARSHIP_AMOUNT] (
        [SCH_AMTID]              BIGINT        NOT NULL,
        [SCH_ORGID]              BIGINT        NOT NULL,
        [SCH_GRADECAT]           CHAR(3)       NOT NULL,
        [SCH_ELGIBLEEXAM]        VARCHAR(2)    NOT NULL,
        [SCH_APPLICABLEALLGRADE] CHAR(1)       NOT NULL,
        [SCH_GRADEID]            DECIMAL(38,0) NOT NULL,
        [SCH_FROMYEAR]           DECIMAL(38,0) NOT NULL,
        [SCH_CLOSEYEAR]          DECIMAL(38,0) NULL,
        [SCH_ELGIBLEAMOUNT]      BIGINT        NOT NULL,
        [SCH_ELGIBLEYEAR]        INT           NOT NULL,
        [SCH_CUTOFFMARKS]        INT           NOT NULL,
        [SCH_CREATEDON]          BIGINT        NULL,
        [SCH_CREATEDBY]          DATETIME2(3)  NULL,
        [SCH_UPDATEDON]          DATETIME2(3)  NULL,
        [SCH_UPDATEDBY]          BIGINT        NULL,
        CONSTRAINT [PK_SCHOLARSHIP_AMOUNT] PRIMARY KEY ([SCH_AMTID])
    );
    CREATE INDEX [IDX_SCHOLARSHIP_AMOUNT_GRADECAT] ON [dbo].[SCHOLARSHIP_AMOUNT]([SCH_GRADECAT], [SCH_ELGIBLEEXAM]);
    PRINT '+ Table SCHOLARSHIP_AMOUNT created';
END
ELSE
    PRINT '+ Table SCHOLARSHIP_AMOUNT already exists';
GO

-- SCHOLARSHIP_MAIN must be created before SCHOLARSHIP_DETAIL (FK dependency)
IF OBJECT_ID(N'[dbo].[SCHOLARSHIP_MAIN]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SCHOLARSHIP_MAIN] (
        [SCH_ID]               INT           NOT NULL,
        [SCH_EMPSYSID]         INT           NOT NULL,
        [SCH_GRADEID]          INT           NOT NULL,
        [SCH_DEPENDID]         INT           NOT NULL,
        [SCH_CHILDNAME]        VARCHAR(100)  NOT NULL,
        [SCH_LASTSCHOOL]       VARCHAR(100)  NOT NULL,
        [SCH_LASTYEAROFSCHOOL] DECIMAL(38,0) NOT NULL,
        [SCH_LASTEXAM]         CHAR(2)       NOT NULL,
        [SCH_CGPAFLAG]         CHAR(1)       NOT NULL,
        [SCH_MARKSPER]         DECIMAL(19,0) NOT NULL,
        [SCH_MARKSGPA]         DECIMAL(19,0) NOT NULL,
        [SCH_MARKSFILE]        VARCHAR(100)  NOT NULL,
        [SCH_COURSENAME]       VARCHAR(100)  NOT NULL,
        [SCH_COURSEJOINYEAR]   INT           NOT NULL,
        [SCH_COURSEJOINMONTH]  DECIMAL(20,0) NOT NULL,
        [SCH_COURSEDURATION]   BIGINT        NOT NULL,
        [SCH_ADMRECPTFILE]     VARCHAR(100)  NULL,
        [SCH_PAYMODE]          CHAR(3)       NULL,
        [SCH_CHILDACCNO]       VARCHAR(20)   NULL,
        [SCH_CHILLDBANKIFSC]   VARCHAR(12)   NULL,
        [SCH_CHILLDBANKMICR]   VARCHAR(12)   NULL,
        [SCH_ENTRYSTATUS]      CHAR(1)       NULL,
        [SCH_SOURCE]           CHAR(1)       NOT NULL,
        [SCH_DISBAMOUNT]       DECIMAL(19,0) NOT NULL,
        [SCH_DISBFREQ]         CHAR(1)       NOT NULL,
        [SCH_LIVESTATUS]       CHAR(1)       NOT NULL,
        [SCH_CREATEDON]        DATETIME2(3)  NOT NULL,
        [SCH_CREATEDBY]        INT           NOT NULL,
        [SCH_UPDATEDON]        DATETIME2(3)  NOT NULL,
        [SCH_UPDATEDBY]        BIGINT        NOT NULL,
        [SCH_APPROVALBY]       INT           NOT NULL,
        [SCH_APPROVALON]       DATETIME2(3)  NOT NULL,
        [SCH_APPREMARKS]       VARCHAR(200)  NOT NULL,
        [SCH_STOPREASON]       VARCHAR(200)  NOT NULL,
        [SCH_STOPDATE]         DATETIME2(3)  NOT NULL,
        [SCH_STOPENTEREDON]    DATETIME2(3)  NOT NULL,
        [SCH_STOPENTEREDBY]    INT           NOT NULL,
        [SCH_OFFLINE]          CHAR(1)       NOT NULL,
        [SCH_OFFLINEYEAR]      INT           NULL,
        CONSTRAINT [PK_SCHOLARSHIP_MAIN] PRIMARY KEY ([SCH_ID])
    );
    CREATE INDEX [IDX_SCHOLARSHIP_MAIN_EMPSYSID] ON [dbo].[SCHOLARSHIP_MAIN]([SCH_EMPSYSID]);
    PRINT '+ Table SCHOLARSHIP_MAIN created';
END
ELSE
    PRINT '+ Table SCHOLARSHIP_MAIN already exists';
GO

-- SCHOLARSHIP_DETAIL
IF OBJECT_ID(N'[dbo].[SCHOLARSHIP_DETAIL]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SCHOLARSHIP_DETAIL] (
        [SCHDET_ID]            BIGINT       NOT NULL,
        [SCHDET_MAINID]        BIGINT       NOT NULL,
        [SCHDET_YEAR]          INT          NOT NULL,
        [SCHDET_MARKSFILE]     VARCHAR(100) NOT NULL,
        [SCHDET_MARKSTATUS]    CHAR(1)      NOT NULL,
        [SCHDET_PAYSTATUS]     CHAR(1)      NOT NULL,
        [SCHDET_CREATEDON]     DATETIME2(3) NOT NULL,
        [SCHDET_CREATEDBY]     BIGINT       NOT NULL,
        [SCHDET_UPDATEDON]     DATETIME2(3) NULL,
        [SCHDET_UPDATEDBY]     BIGINT       NULL,
        [SCHDET_APPROVEDON]    DATETIME2(3) NULL,
        [SCHDET_APPROVEDBY]    BIGINT       NULL,
        [SCHDET_PAYAPPROVEDON] DATETIME2(3) NULL,
        [SCHDET_PAYAPPROVEDBY] BIGINT       NULL,
        [SCHDET_PAYDATE]       DATETIME2(3) NULL,
        [SCHDET_PAYAMOUNT]     BIGINT       NULL,
        [SCHDET_PAYUPDATEDON]  DATETIME2(3) NULL,
        [SCHDET_PAYUPDATEDBY]  BIGINT       NULL,
        CONSTRAINT [PK_SCHOLARSHIP_DETAIL]      PRIMARY KEY ([SCHDET_ID]),
        CONSTRAINT [FK_SCHOLARSHIP_DETAIL_MAIN] FOREIGN KEY ([SCHDET_MAINID])
            REFERENCES [dbo].[SCHOLARSHIP_MAIN]([SCH_ID])
    );
    CREATE INDEX [IDX_SCHOLARSHIP_DETAIL_MAINID] ON [dbo].[SCHOLARSHIP_DETAIL]([SCHDET_MAINID]);
    PRINT '+ Table SCHOLARSHIP_DETAIL created';
END
ELSE
    PRINT '+ Table SCHOLARSHIP_DETAIL already exists';
GO

-- =====================================================
-- 3. Functions
-- =====================================================

CREATE OR ALTER FUNCTION dbo.fn_GetScholarshipEligibleAmount
(
    @p_GradeCat     CHAR(3),
    @p_EligibleExam VARCHAR(2),
    @p_Year         INT
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @Amount BIGINT;
    SELECT TOP 1 @Amount = SCH_ELGIBLEAMOUNT
    FROM dbo.SCHOLARSHIP_AMOUNT
    WHERE SCH_GRADECAT    = @p_GradeCat
      AND SCH_ELGIBLEEXAM = @p_EligibleExam
      AND @p_Year BETWEEN SCH_FROMYEAR AND ISNULL(SCH_CLOSEYEAR, @p_Year)
    ORDER BY SCH_FROMYEAR DESC;
    RETURN ISNULL(@Amount, 0);
END;
GO
PRINT '+ fn_GetScholarshipEligibleAmount created';
GO

-- =====================================================
-- 4. Stored Procedures
-- =====================================================

CREATE OR ALTER PROCEDURE dbo.usp_ScholarshipApplication
(
    @p_SCH_EMPSYSID         INT,
    @p_SCH_GRADEID          INT,
    @p_SCH_DEPENDID         INT,
    @p_SCH_CHILDNAME        VARCHAR(100),
    @p_SCH_LASTSCHOOL       VARCHAR(100),
    @p_SCH_LASTYEAROFSCHOOL DECIMAL(38,0),
    @p_SCH_LASTEXAM         CHAR(2),
    @p_SCH_CGPAFLAG         CHAR(1),
    @p_SCH_MARKSPER         DECIMAL(19,0),
    @p_SCH_MARKSGPA         DECIMAL(19,0),
    @p_SCH_MARKSFILE        VARCHAR(100),
    @p_SCH_COURSENAME       VARCHAR(100),
    @p_SCH_COURSEJOINYEAR   INT,
    @p_SCH_COURSEJOINMONTH  DECIMAL(20,0),
    @p_SCH_COURSEDURATION   BIGINT,
    @p_SCH_ADMRECPTFILE     VARCHAR(100) = NULL,
    @p_SCH_PAYMODE          CHAR(3)      = NULL,
    @p_SCH_CHILDACCNO       VARCHAR(20)  = NULL,
    @p_SCH_CHILLDBANKIFSC   VARCHAR(12)  = NULL,
    @p_SCH_CHILLDBANKMICR   VARCHAR(12)  = NULL,
    @p_SCH_ENTRYSTATUS      CHAR(1)      = 'E',
    @p_SCH_SOURCE           CHAR(1),
    @p_SCH_DISBAMOUNT       DECIMAL(19,0),
    @p_SCH_DISBFREQ         CHAR(1),
    @p_SCH_LIVESTATUS       CHAR(1)      = 'A',
    @p_CreatedBy            INT,
    @p_SCH_OFFLINE          CHAR(1)      = 'N',
    @p_SCH_OFFLINEYEAR      INT          = NULL,
    @p_NewSchID             INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @p_NewSchID = ISNULL(MAX(SCH_ID), 0) + 1 FROM dbo.SCHOLARSHIP_MAIN;

        INSERT INTO dbo.SCHOLARSHIP_MAIN
        (
            SCH_ID, SCH_EMPSYSID, SCH_GRADEID, SCH_DEPENDID, SCH_CHILDNAME,
            SCH_LASTSCHOOL, SCH_LASTYEAROFSCHOOL, SCH_LASTEXAM, SCH_CGPAFLAG,
            SCH_MARKSPER, SCH_MARKSGPA, SCH_MARKSFILE, SCH_COURSENAME,
            SCH_COURSEJOINYEAR, SCH_COURSEJOINMONTH, SCH_COURSEDURATION,
            SCH_ADMRECPTFILE, SCH_PAYMODE, SCH_CHILDACCNO, SCH_CHILLDBANKIFSC,
            SCH_CHILLDBANKMICR, SCH_ENTRYSTATUS, SCH_SOURCE, SCH_DISBAMOUNT,
            SCH_DISBFREQ, SCH_LIVESTATUS, SCH_CREATEDON, SCH_CREATEDBY,
            SCH_UPDATEDON, SCH_UPDATEDBY, SCH_APPROVALBY, SCH_APPROVALON,
            SCH_APPREMARKS, SCH_STOPREASON, SCH_STOPDATE, SCH_STOPENTEREDON,
            SCH_STOPENTEREDBY, SCH_OFFLINE, SCH_OFFLINEYEAR
        )
        VALUES
        (
            @p_NewSchID, @p_SCH_EMPSYSID, @p_SCH_GRADEID, @p_SCH_DEPENDID, @p_SCH_CHILDNAME,
            @p_SCH_LASTSCHOOL, @p_SCH_LASTYEAROFSCHOOL, @p_SCH_LASTEXAM, @p_SCH_CGPAFLAG,
            @p_SCH_MARKSPER, @p_SCH_MARKSGPA, @p_SCH_MARKSFILE, @p_SCH_COURSENAME,
            @p_SCH_COURSEJOINYEAR, @p_SCH_COURSEJOINMONTH, @p_SCH_COURSEDURATION,
            @p_SCH_ADMRECPTFILE, @p_SCH_PAYMODE, @p_SCH_CHILDACCNO, @p_SCH_CHILLDBANKIFSC,
            @p_SCH_CHILLDBANKMICR, @p_SCH_ENTRYSTATUS, @p_SCH_SOURCE, @p_SCH_DISBAMOUNT,
            @p_SCH_DISBFREQ, @p_SCH_LIVESTATUS, GETDATE(), @p_CreatedBy,
            GETDATE(), @p_CreatedBy, 0, GETDATE(),
            '', '', GETDATE(), GETDATE(), 0,
            @p_SCH_OFFLINE, @p_SCH_OFFLINEYEAR
        );

        DECLARE @NewDetID BIGINT;
        SELECT @NewDetID = ISNULL(MAX(SCHDET_ID), 0) + 1 FROM dbo.SCHOLARSHIP_DETAIL;

        INSERT INTO dbo.SCHOLARSHIP_DETAIL
        (
            SCHDET_ID, SCHDET_MAINID, SCHDET_YEAR, SCHDET_MARKSFILE,
            SCHDET_MARKSTATUS, SCHDET_PAYSTATUS, SCHDET_CREATEDON, SCHDET_CREATEDBY,
            SCHDET_UPDATEDON, SCHDET_UPDATEDBY, SCHDET_APPROVEDON, SCHDET_APPROVEDBY,
            SCHDET_PAYAPPROVEDON, SCHDET_PAYAPPROVEDBY, SCHDET_PAYDATE,
            SCHDET_PAYAMOUNT, SCHDET_PAYUPDATEDON, SCHDET_PAYUPDATEDBY
        )
        VALUES
        (
            @NewDetID, @p_NewSchID, @p_SCH_COURSEJOINYEAR, @p_SCH_MARKSFILE,
            'P', 'S',
            GETDATE(), @p_CreatedBy,
            GETDATE(), @p_CreatedBy,
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '+ usp_ScholarshipApplication created';
GO

CREATE OR ALTER PROCEDURE dbo.usp_ScholarshipApprove
(
    @p_SCH_ID     INT,
    @p_ApprovedBy INT,
    @p_AppRemarks VARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.SCHOLARSHIP_MAIN
        SET SCH_ENTRYSTATUS = 'A',
            SCH_APPROVALBY  = @p_ApprovedBy,
            SCH_APPROVALON  = GETDATE(),
            SCH_APPREMARKS  = ISNULL(@p_AppRemarks, ''),
            SCH_UPDATEDON   = GETDATE(),
            SCH_UPDATEDBY   = @p_ApprovedBy
        WHERE SCH_ID = @p_SCH_ID;

        UPDATE dbo.SCHOLARSHIP_DETAIL
        SET SCHDET_MARKSTATUS = 'A',
            SCHDET_APPROVEDON = GETDATE(),
            SCHDET_APPROVEDBY = @p_ApprovedBy,
            SCHDET_UPDATEDON  = GETDATE(),
            SCHDET_UPDATEDBY  = @p_ApprovedBy
        WHERE SCHDET_MAINID    = @p_SCH_ID
          AND SCHDET_MARKSTATUS = 'P';

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '+ usp_ScholarshipApprove created';
GO

CREATE OR ALTER PROCEDURE dbo.usp_ScholarshipStop
(
    @p_SCH_ID      INT,
    @p_StopReason  VARCHAR(200),
    @p_StopDate    DATETIME2(3),
    @p_EnteredBy   INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.SCHOLARSHIP_MAIN
        SET SCH_LIVESTATUS    = 'S',
            SCH_STOPREASON    = @p_StopReason,
            SCH_STOPDATE      = @p_StopDate,
            SCH_STOPENTEREDON = GETDATE(),
            SCH_STOPENTEREDBY = @p_EnteredBy,
            SCH_UPDATEDON     = GETDATE(),
            SCH_UPDATEDBY     = @p_EnteredBy
        WHERE SCH_ID = @p_SCH_ID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '+ usp_ScholarshipStop created';
GO

-- =====================================================
-- 5. Triggers
-- =====================================================

CREATE OR ALTER TRIGGER dbo.trg_ScholarshipDetail_UpdateAudit
ON dbo.SCHOLARSHIP_DETAIL
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE SD
    SET SCHDET_UPDATEDON = GETDATE()
    FROM dbo.SCHOLARSHIP_DETAIL SD
    INNER JOIN inserted I ON SD.SCHDET_ID = I.SCHDET_ID;
END;
GO
PRINT '+ trg_ScholarshipDetail_UpdateAudit created';
GO

-- =====================================================
-- 6. EF Core Migration History
-- =====================================================

IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
    PRINT '+ Table __EFMigrationsHistory created';
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = '20260310000000_InitialCreate')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260310000000_InitialCreate', '10.0.3');
    PRINT '+ EF Core migration history seeded';
END
GO

-- Verify table creation
SELECT 'SCHOLARSHIP_AMOUNT' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[SCHOLARSHIP_AMOUNT]
UNION ALL
SELECT 'SCHOLARSHIP_MAIN'   AS TableName, COUNT(*) AS RecordCount FROM [dbo].[SCHOLARSHIP_MAIN]
UNION ALL
SELECT 'SCHOLARSHIP_DETAIL' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[SCHOLARSHIP_DETAIL];
GO

PRINT '';
PRINT '======================================';
PRINT 'ADMINDB initialization complete';
PRINT '======================================';
GO

-- =====================================================
-- END OF init-database.sql
-- =====================================================


GO


-- ============================================================
-- Stationery Service
-- ============================================================

-- =====================================================
-- Docker init script for STATIONERYDB
-- Runs on first container startup via sqlcmd
-- =====================================================

USE MASTER;
GO

-- ====================================================
-- 1. Create database
-- ====================================================
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'STATIONERYDB')
BEGIN
    CREATE DATABASE [STATIONERYDB];
    PRINT '+ STATIONERYDB created';
END
ELSE
    PRINT '+ STATIONERYDB already exists';
GO

USE [STATIONERYDB];
GO

-- ====================================================
-- 2. Create Tables
-- ====================================================

-- STATIONARY_MASTER
IF OBJECT_ID(N'[dbo].[STATIONARY_MASTER]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[STATIONARY_MASTER] (
        [SM_STATIONARYID] BIGINT NOT NULL,
        [SM_CATID] BIGINT NOT NULL,
        [SM_LOC_ID] BIGINT NOT NULL,
        [SM_DESC] VARCHAR(200) NOT NULL,
        [SM_UOMID] BIGINT NOT NULL,
        [SM_MAKE] VARCHAR(10) NOT NULL,
        [SM_PRICE_PERUNIT] BIGINT NULL,
        [SM_REORDER_LEVEL] BIGINT NULL,
        [SM_UPDATED_BY] BIGINT NOT NULL,
        [SM_UPDATED_ON] DATETIME2(3) NOT NULL,
        [SM_VMID] BIGINT NOT NULL,
        [SM_CLOSED] CHAR(1) NOT NULL,
        [SM_OPENINGSTOCK] BIGINT NOT NULL,
        CONSTRAINT [PK_STATIONARY_MASTER] PRIMARY KEY ([SM_STATIONARYID])
    );
    PRINT '+ STATIONARY_MASTER table created';
END
ELSE
    PRINT '+ STATIONARY_MASTER table already exists';
GO

-- SP_REQUEST_MAIN
IF OBJECT_ID(N'[dbo].[SP_REQUEST_MAIN]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SP_REQUEST_MAIN] (
        [RM_REQUESTID] BIGINT NOT NULL,
        [RM_REQUESTEDBY] BIGINT NOT NULL,
        [RM_REQUESTEDON] DATETIME2(3) NOT NULL,
        [RM_LOCATIONID] BIGINT NULL,
        [RM_UNITCODE] CHAR(3) NULL,
        CONSTRAINT [PK_SP_REQUEST_MAIN] PRIMARY KEY ([RM_REQUESTID])
    );
    PRINT '+ SP_REQUEST_MAIN table created';
END
ELSE
    PRINT '+ SP_REQUEST_MAIN table already exists';
GO

-- SP_REQUEST_SUB
IF OBJECT_ID(N'[dbo].[SP_REQUEST_SUB]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SP_REQUEST_SUB] (
        [RS_REQUESTSUB_ID] BIGINT NOT NULL,
        [RS_REQUESTID] BIGINT NOT NULL,
        [RS_STATIONARYID] BIGINT NOT NULL,
        [RS_DEPTID] BIGINT NOT NULL,
        [RS_EXPECTED_DATE] DATETIME2(3) NOT NULL,
        [RS_USER_SYSID] BIGINT NULL,
        [RS_REQUESTEDQTY] BIGINT NOT NULL,
        [RS_INDENTEDQTY] BIGINT NULL,
        [RS_APPROVEDQTY] BIGINT NULL,
        [RS_APPROVER_SYSID] BIGINT NULL,
        [RS_APPROVER_RAMARKS] VARCHAR(255) NULL,
        [RS_RECEIVED_DATE] DATETIME2(3) NULL,
        [RS_STATUS] VARCHAR(1) NOT NULL,
        [RS_UPDATED_BY] BIGINT NOT NULL,
        [RS_UPDATED_ON] DATETIME2(3) NOT NULL,
        [RS_APPROVED_ON] DATETIME2(3) NULL,
        CONSTRAINT [PK_SP_REQUEST_SUB] PRIMARY KEY ([RS_REQUESTSUB_ID]),
        CONSTRAINT [FK_SP_REQUEST_SUB_MAIN] FOREIGN KEY ([RS_REQUESTID]) REFERENCES [SP_REQUEST_MAIN]([RM_REQUESTID])
    );
    PRINT '+ SP_REQUEST_SUB table created';
END
ELSE
    PRINT '+ SP_REQUEST_SUB table already exists';
GO

-- SP_ORDER_MAIN
IF OBJECT_ID(N'[dbo].[SP_ORDER_MAIN]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SP_ORDER_MAIN] (
        [OM_ORDERMAIN_ID] BIGINT NOT NULL,
        [OM_LOCATION_ID] BIGINT NOT NULL,
        [OM_VENDORID] BIGINT NOT NULL,
        [OM_DELIVERYDATE] DATETIME2(3) NOT NULL,
        [OM_ORDEREDDATE] DATETIME2(3) NOT NULL,
        [OM_ORDEREDBY] BIGINT NOT NULL,
        CONSTRAINT [PK_SP_ORDER_MAIN] PRIMARY KEY ([OM_ORDERMAIN_ID])
    );
    PRINT '+ SP_ORDER_MAIN table created';
END
ELSE
    PRINT '+ SP_ORDER_MAIN table already exists';
GO

-- SP_ORDER_SUB
IF OBJECT_ID(N'[dbo].[SP_ORDER_SUB]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SP_ORDER_SUB] (
        [OS_ORDERSUB_ID] BIGINT NOT NULL,
        [OS_ORDERMAIN_ID] BIGINT NOT NULL,
        [OS_REQUESTSUB_ID] BIGINT NOT NULL,
        [OS_ORDERED_QTY] BIGINT NOT NULL,
        [OS_RECEIVEDON] DATETIME2(3) NULL,
        [OS_RECEIVED_BY] BIGINT NOT NULL,
        [OS_ORDERPRICE] BIGINT NOT NULL,
        [OS_ACTUALPRICE] BIGINT NOT NULL,
        [OS_RECEIVEDDATE] DATETIME2(3) NOT NULL,
        [OS_DELIVERYDATE] DATETIME2(3) NOT NULL,
        [OS_RECEIPTENTRYBY] BIGINT NULL,
        [OS_RECEIPTENTRYON] DATETIME2(3) NULL,
        CONSTRAINT [PK_SP_ORDER_SUB] PRIMARY KEY ([OS_ORDERSUB_ID]),
        CONSTRAINT [FK_SP_ORDER_SUB_MAIN] FOREIGN KEY ([OS_ORDERMAIN_ID]) REFERENCES [SP_ORDER_MAIN]([OM_ORDERMAIN_ID])
    );
    PRINT '+ SP_ORDER_SUB table created';
END
ELSE
    PRINT '+ SP_ORDER_SUB table already exists';
GO

-- SP_DEPT_BUDGET
IF OBJECT_ID(N'[dbo].[SP_DEPT_BUDGET]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SP_DEPT_BUDGET] (
        [DB_LOCATION_ID] BIGINT NOT NULL,
        [DB_UNIT_CODE] CHAR(3) NOT NULL,
        [DB_DEPT_ID] BIGINT NOT NULL,
        [DB_FINYEAR_ID] BIGINT NOT NULL,
        [DB_BUDGETAMOUNT] BIGINT NOT NULL,
        [DB_UPDATED_BY] BIGINT NOT NULL,
        [DB_UPDATED_ON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_SP_DEPT_BUDGET] PRIMARY KEY ([DB_LOCATION_ID], [DB_DEPT_ID], [DB_FINYEAR_ID])
    );
    PRINT '+ SP_DEPT_BUDGET table created';
END
ELSE
    PRINT '+ SP_DEPT_BUDGET table already exists';
GO

-- SP_UNIT_BUDGET
IF OBJECT_ID(N'[dbo].[SP_UNIT_BUDGET]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SP_UNIT_BUDGET] (
        [UB_LOCATION_ID] BIGINT NOT NULL,
        [UB_UNIT_CODE] CHAR(3) NOT NULL,
        [UB_FINYEAR_ID] BIGINT NOT NULL,
        [UB_BUDGETAMOUNT] BIGINT NOT NULL,
        [UB_UPDATED_BY] BIGINT NOT NULL,
        [UB_UPDATED_ON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_SP_UNIT_BUDGET] PRIMARY KEY ([UB_LOCATION_ID], [UB_UNIT_CODE], [UB_FINYEAR_ID])
    );
    PRINT '+ SP_UNIT_BUDGET table created';
END
ELSE
    PRINT '+ SP_UNIT_BUDGET table already exists';
GO

-- SP_DEPT_APPROVER
IF OBJECT_ID(N'[dbo].[SP_DEPT_APPROVER]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SP_DEPT_APPROVER] (
        [DA_LOCATION_ID] BIGINT NOT NULL,
        [DA_UNIT_CODE] CHAR(3) NOT NULL,
        [DA_DEPT_ID] BIGINT NOT NULL,
        [DA_EMP_SYSID] BIGINT NOT NULL,
        [DA_TYPE] CHAR(1) NOT NULL,
        [DA_EFFECTIVE_DATE] DATETIME2(3) NOT NULL,
        [DA_CLOSURE_DATE] DATETIME2(3) NULL,
        [DA_UPDATED_BY] BIGINT NOT NULL,
        [DA_UPDATED_ON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_SP_DEPT_APPROVER] PRIMARY KEY ([DA_LOCATION_ID], [DA_DEPT_ID], [DA_EMP_SYSID], [DA_TYPE])
    );
    PRINT '+ SP_DEPT_APPROVER table created';
END
ELSE
    PRINT '+ SP_DEPT_APPROVER table already exists';
GO

-- SP_UNIT_APPROVER
IF OBJECT_ID(N'[dbo].[SP_UNIT_APPROVER]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SP_UNIT_APPROVER] (
        [UA_LOCATION_ID] BIGINT NOT NULL,
        [UA_UNIT_CODE] CHAR(3) NOT NULL,
        [UA_EMP_SYSID] BIGINT NOT NULL,
        [UA_TYPE] CHAR(1) NOT NULL,
        [UA_EFFECTIVE_DATE] DATETIME2(3) NOT NULL,
        [UA_CLOSURE_DATE] VARCHAR(255) NULL,
        [UA_UPDATED_BY] BIGINT NOT NULL,
        [UA_UPDATED_ON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_SP_UNIT_APPROVER] PRIMARY KEY ([UA_LOCATION_ID], [UA_UNIT_CODE], [UA_EMP_SYSID], [UA_TYPE])
    );
    PRINT '+ SP_UNIT_APPROVER table created';
END
ELSE
    PRINT '+ SP_UNIT_APPROVER table already exists';
GO

-- SP_CATEGORY_DEFAULT
IF OBJECT_ID(N'[dbo].[SP_CATEGORY_DEFAULT]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SP_CATEGORY_DEFAULT] (
        [CD_STATIONERYID] BIGINT NOT NULL,
        [CD_CATEGORYID] BIGINT NOT NULL,
        [CD_LOCATIONID] BIGINT NOT NULL,
        [CD_MODIFIEDBY] BIGINT NOT NULL,
        [CD_MODIFIEDON] DATETIME2(3) NOT NULL
    );
    PRINT '+ SP_CATEGORY_DEFAULT table created';
END
ELSE
    PRINT '+ SP_CATEGORY_DEFAULT table already exists';
GO

-- ITEMDATA
IF OBJECT_ID(N'[dbo].[ITEMDATA]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ITEMDATA] (
        [ID] INT IDENTITY(1,1) NOT NULL,
        [CATNAME] VARCHAR(40) NULL,
        [ITEMNAME] VARCHAR(60) NULL,
        [MAKE] VARCHAR(30) NULL,
        [UOM] VARCHAR(20) NULL,
        [PRICE] INT NULL,
        CONSTRAINT [PK_ITEMDATA] PRIMARY KEY ([ID])
    );
    PRINT '+ ITEMDATA table created';
END
ELSE
    PRINT '+ ITEMDATA table already exists';
GO

-- ====================================================
-- 3. Seed Data
-- ====================================================

-- Seed STATIONARY_MASTER
IF NOT EXISTS (SELECT 1 FROM [dbo].[STATIONARY_MASTER])
BEGIN
    INSERT INTO [dbo].[STATIONARY_MASTER] 
    ([SM_STATIONARYID], [SM_CATID], [SM_LOC_ID], [SM_DESC], [SM_UOMID], [SM_MAKE], [SM_PRICE_PERUNIT], 
     [SM_REORDER_LEVEL], [SM_UPDATED_BY], [SM_UPDATED_ON], [SM_VMID], [SM_CLOSED], [SM_OPENINGSTOCK])
    VALUES
    (1, 1, 1, 'A4 Paper (500 sheets)', 1, 'Generic', 250, 100, 1, GETUTCDATE(), 1, 'N', 1000),
    (2, 1, 1, 'Blue Ballpoint Pen', 2, 'Pilot', 15, 50, 1, GETUTCDATE(), 1, 'N', 500),
    (3, 1, 1, 'Black Ballpoint Pen', 2, 'Pilot', 15, 50, 1, GETUTCDATE(), 1, 'N', 450),
    (4, 2, 1, 'Stapler', 3, 'Kangaro', 180, 10, 1, GETUTCDATE(), 2, 'N', 30),
    (5, 2, 1, 'Stapler Pins (100 pcs)', 1, 'Kangaro', 20, 20, 1, GETUTCDATE(), 2, 'N', 80),
    (6, 3, 1, 'Sticky Notes (100 sheets)', 1, '3M', 60, 15, 1, GETUTCDATE(), 3, 'N', 75),
    (7, 3, 1, 'Highlighter Pen', 2, 'Camlin', 30, 20, 1, GETUTCDATE(), 3, 'N', 150),
    (8, 4, 1, 'Printer Ink Cartridge (Black)', 3, 'HP', 1200, 5, 1, GETUTCDATE(), 4, 'N', 12);
    PRINT '+ STATIONARY_MASTER seeded with 8 items';
END
ELSE
    PRINT '+ STATIONARY_MASTER already has data';
GO

-- Seed SP_DEPT_BUDGET
IF NOT EXISTS (SELECT 1 FROM [dbo].[SP_DEPT_BUDGET])
BEGIN
    INSERT INTO [dbo].[SP_DEPT_BUDGET] 
    ([DB_LOCATION_ID], [DB_UNIT_CODE], [DB_DEPT_ID], [DB_FINYEAR_ID], [DB_BUDGETAMOUNT], [DB_UPDATED_BY], [DB_UPDATED_ON])
    VALUES
    (1, 'HO ', 100, 2026, 50000, 1, GETUTCDATE()),
    (1, 'HO ', 101, 2026, 30000, 1, GETUTCDATE()),
    (1, 'HO ', 102, 2026, 25000, 1, GETUTCDATE());
    PRINT '+ SP_DEPT_BUDGET seeded with 3 budgets';
END
ELSE
    PRINT '+ SP_DEPT_BUDGET already has data';
GO

-- Seed SP_UNIT_BUDGET
IF NOT EXISTS (SELECT 1 FROM [dbo].[SP_UNIT_BUDGET])
BEGIN
    INSERT INTO [dbo].[SP_UNIT_BUDGET] 
    ([UB_LOCATION_ID], [UB_UNIT_CODE], [UB_FINYEAR_ID], [UB_BUDGETAMOUNT], [UB_UPDATED_BY], [UB_UPDATED_ON])
    VALUES
    (1, 'HO ', 2026, 200000, 1, GETUTCDATE());
    PRINT '+ SP_UNIT_BUDGET seeded with 1 budget';
END
ELSE
    PRINT '+ SP_UNIT_BUDGET already has data';
GO

-- Seed SP_DEPT_APPROVER
IF NOT EXISTS (SELECT 1 FROM [dbo].[SP_DEPT_APPROVER])
BEGIN
    INSERT INTO [dbo].[SP_DEPT_APPROVER] 
    ([DA_LOCATION_ID], [DA_UNIT_CODE], [DA_DEPT_ID], [DA_EMP_SYSID], [DA_TYPE], [DA_EFFECTIVE_DATE], 
     [DA_CLOSURE_DATE], [DA_UPDATED_BY], [DA_UPDATED_ON])
    VALUES
    (1, 'HO ', 100, 201, 'A', '2026-01-01', NULL, 1, GETUTCDATE()),
    (1, 'HO ', 100, 202, 'I', '2026-01-01', NULL, 1, GETUTCDATE()),
    (1, 'HO ', 101, 203, 'A', '2026-01-01', NULL, 1, GETUTCDATE());
    PRINT '+ SP_DEPT_APPROVER seeded with 3 approvers';
END
ELSE
    PRINT '+ SP_DEPT_APPROVER already has data';
GO

-- Seed SP_UNIT_APPROVER
IF NOT EXISTS (SELECT 1 FROM [dbo].[SP_UNIT_APPROVER])
BEGIN
    INSERT INTO [dbo].[SP_UNIT_APPROVER] 
    ([UA_LOCATION_ID], [UA_UNIT_CODE], [UA_EMP_SYSID], [UA_TYPE], [UA_EFFECTIVE_DATE], 
     [UA_CLOSURE_DATE], [UA_UPDATED_BY], [UA_UPDATED_ON])
    VALUES
    (1, 'HO ', 300, 'A', '2026-01-01', NULL, 1, GETUTCDATE()),
    (1, 'HO ', 301, 'I', '2026-01-01', NULL, 1, GETUTCDATE());
    PRINT '+ SP_UNIT_APPROVER seeded with 2 approvers';
END
ELSE
    PRINT '+ SP_UNIT_APPROVER already has data';
GO

-- Seed ITEMDATA
IF NOT EXISTS (SELECT 1 FROM [dbo].[ITEMDATA])
BEGIN
    INSERT INTO [dbo].[ITEMDATA] ([CATNAME], [ITEMNAME], [MAKE], [UOM], [PRICE])
    VALUES
    ('Writing', 'Ballpoint Pen', 'Pilot', 'Pieces', 15),
    ('Paper', 'A4 Paper', 'Generic', 'Reams', 250),
    ('Office Tools', 'Stapler', 'Kangaro', 'Pieces', 180),
    ('Printing', 'Ink Cartridge', 'HP', 'Cartridges', 1200),
    ('Writing', 'Pencil', 'Faber-Castell', 'Pieces', 10),
    ('Paper', 'Notebook', 'Generic', 'Pieces', 50);
    PRINT '+ ITEMDATA seeded with 6 items';
END
ELSE
    PRINT '+ ITEMDATA already has data';
GO

-- ====================================================
-- Verification
-- ====================================================
PRINT '';
PRINT '====================================================';
PRINT 'STATIONERYDB initialization complete';
PRINT '====================================================';

SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
ORDER BY TABLE_NAME;


GO


-- ============================================================
-- TDS Service
-- ============================================================

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


GO


-- ============================================================
-- Transaction Service
-- ============================================================

-- ============================================================
-- TransactionService Database Initialization Script
-- Creates the ADMINDB database and transaction-related tables
-- ============================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ADMINDB')
BEGIN
    CREATE DATABASE ADMINDB;
END
GO

USE ADMINDB;
GO

-- Location Admin
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_LOCATION_ADMIN')
BEGIN
    CREATE TABLE SP_LOCATION_ADMIN (
        LA_LOC_ID       INT           NOT NULL,
        LA_LOC_NAME     NVARCHAR(255) NOT NULL
    );
END
GO

-- Category Default
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_CATEGORY_DEFAULT')
BEGIN
    CREATE TABLE SP_CATEGORY_DEFAULT (
        CD_CATEGORY_ID       INT           NOT NULL,
        CD_CATEGORY_NAME     NVARCHAR(255) NOT NULL,
        CD_SUB_CATEGORY_ID   INT           NOT NULL,
        CD_SUB_CATEGORY_NAME NVARCHAR(255) NOT NULL
    );
END
GO

-- Request Main
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_REQUEST_MAIN')
BEGIN
    CREATE TABLE SP_REQUEST_MAIN (
        RM_REQUESTID    INT           IDENTITY(1,1) PRIMARY KEY,
        RM_REQUESTEDBY  NVARCHAR(255) NOT NULL,
        RM_REQUESTEDON  DATETIME      NOT NULL DEFAULT GETDATE(),
        RM_LOCATIONID   INT           NULL,
        RM_DEPT_ID      NVARCHAR(50)  NULL,
        RM_UNIT_CD      NVARCHAR(3)   NULL,
        RM_FINYEAR      INT           NULL
    );
END
GO

-- Request Sub
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_REQUEST_SUB')
BEGIN
    CREATE TABLE SP_REQUEST_SUB (
        RS_REQUESTSUB_ID INT           IDENTITY(1,1) PRIMARY KEY,
        RS_REQUESTID     INT           NOT NULL,
        RS_CATEGORYID    INT           NULL,
        RS_SUBCATEGORYID INT           NULL,
        RS_STATIONERYID  INT           NULL,
        RS_QUANTITY      INT           NULL,
        RS_APPROX_COST   BIGINT        NULL,
        RS_STATUS        NVARCHAR(1)   NULL DEFAULT 'P',
        RS_APPROVEDBY    NVARCHAR(255) NULL,
        RS_APPROVEDON    DATETIME      NULL,
        RS_INDENTORID    NVARCHAR(255) NULL,
        RS_INDENTEDON    DATETIME      NULL,
        RS_RECEIVEDON    DATETIME      NULL,
        CONSTRAINT FK_RequestSub_RequestMain FOREIGN KEY (RS_REQUESTID) REFERENCES SP_REQUEST_MAIN(RM_REQUESTID)
    );
END
GO

-- Order Main
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_ORDER_MAIN')
BEGIN
    CREATE TABLE SP_ORDER_MAIN (
        OM_ORDERMAIN_ID INT           IDENTITY(1,1) PRIMARY KEY,
        OM_VENDORID     NVARCHAR(255) NOT NULL,
        OM_ORDEREDON    DATETIME      NOT NULL DEFAULT GETDATE(),
        OM_DELIVERYDATE DATETIME      NULL,
        OM_LOCATIONID   INT           NULL
    );
END
GO

-- Order Sub
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_ORDER_SUB')
BEGIN
    CREATE TABLE SP_ORDER_SUB (
        OS_ORDERSUB_ID  INT           IDENTITY(1,1) PRIMARY KEY,
        OS_ORDERMAIN_ID INT           NOT NULL,
        OS_REQUESTSUB_ID INT          NULL,
        OS_ORDERED_QTY  INT           NULL,
        OS_UNIT_PRICE   BIGINT        NULL,
        OS_RECEIVEDON   DATETIME      NULL,
        OS_ACTUAL_PRICE BIGINT        NULL,
        CONSTRAINT FK_OrderSub_OrderMain FOREIGN KEY (OS_ORDERMAIN_ID) REFERENCES SP_ORDER_MAIN(OM_ORDERMAIN_ID)
    );
END
GO

-- Dept Budget
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_DEPT_BUDGET')
BEGIN
    CREATE TABLE SP_DEPT_BUDGET (
        DB_DEPT_ID       NVARCHAR(50) NOT NULL,
        DB_BUDGET_AMOUNT BIGINT       NULL,
        DB_FINYEAR       INT          NULL,
        DB_UNIT_CD       NVARCHAR(3)  NULL
    );
END
GO

-- Unit Budget
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_UNIT_BUDGET')
BEGIN
    CREATE TABLE SP_UNIT_BUDGET (
        UB_UNIT_CD       NVARCHAR(3)  NOT NULL,
        UB_DEPT_ID       NVARCHAR(50) NOT NULL,
        UB_BUDGET_AMOUNT BIGINT       NULL,
        UB_FINYEAR       INT          NULL
    );
END
GO

-- Dept Approver
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_DEPT_APPROVER')
BEGIN
    CREATE TABLE SP_DEPT_APPROVER (
        DA_DEPT_ID       NVARCHAR(50)  NOT NULL,
        DA_APPROVERID    NVARCHAR(255) NOT NULL,
        DA_APPROVER_TYPE NVARCHAR(1)   NULL
    );
END
GO

-- Unit Approver
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_UNIT_APPROVER')
BEGIN
    CREATE TABLE SP_UNIT_APPROVER (
        UA_UNIT_CD     NVARCHAR(3)   NOT NULL,
        UA_DEPT_ID     NVARCHAR(50)  NOT NULL,
        UA_APPROVERID  NVARCHAR(255) NOT NULL,
        UA_CLOSURE_DATE NVARCHAR(255) NULL
    );
END
GO

PRINT 'Transaction database tables created successfully.';


GO

