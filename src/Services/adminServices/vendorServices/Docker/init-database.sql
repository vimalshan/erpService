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