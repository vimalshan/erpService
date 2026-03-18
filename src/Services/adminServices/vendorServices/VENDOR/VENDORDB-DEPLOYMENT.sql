-- ==========================================
-- VENDOR MODULE - STANDALONE DATABASE
-- Complete Deployment Script
-- Version: 1.0
-- Generated: 2026-03-09
-- ==========================================

USE MASTER;
GO

PRINT '=== Creating VENDORDB ===';
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'VENDORDB')
BEGIN
    CREATE DATABASE VENDORDB
    ON PRIMARY (
        NAME = 'VENDORDB_data',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\VENDORDB.mdf',
        SIZE = 50MB,
        MAXSIZE = 500MB,
        FILEGROWTH = 10%
    )
    LOG ON (
        NAME = 'VENDORDB_log',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\VENDORDB.ldf',
        SIZE = 25MB,
        MAXSIZE = 250MB,
        FILEGROWTH = 10%
    );
    PRINT '✓ VENDORDB created';
END
GO

USE [VENDORDB];
GO

PRINT '';
PRINT '=== Deploying VENDOR Tables ===';
GO

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
PRINT '✓ VENDOR_MASTER table created';
GO

CREATE INDEX [IDX_VENDOR_MASTER_LOCID] ON [VENDOR_MASTER]([VM_LOC_ID]);
CREATE INDEX [IDX_VENDOR_MASTER_STATUS] ON [VENDOR_MASTER]([VM_LIVESTATUS]);
PRINT '✓ Indexes created';
GO

PRINT '';
PRINT '=== Deploying VENDOR Procedures ===';
GO

CREATE OR ALTER PROCEDURE dbo.usp_AddUpdateVendor
(
    @p_VM_ID BIGINT = NULL,
    @p_VM_CATID BIGINT,
    @p_VM_LOC_ID BIGINT,
    @p_VM_NAME VARCHAR(100),
    @p_VM_EMAIL VARCHAR(50) = NULL,
    @p_VM_ADDRESS VARCHAR(200),
    @p_UpdatedBy BIGINT,
    @p_VM_LIVESTATUS CHAR(1) = 'A'
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @p_VM_ID IS NULL
        BEGIN
            SELECT @p_VM_ID = ISNULL(MAX(VM_ID), 0) + 1 FROM VENDOR_MASTER;
            INSERT INTO VENDOR_MASTER
            (VM_ID, VM_CATID, VM_LOC_ID, VM_NAME, VM_EMAIL, VM_ADDRESS, VM_UPDATED_BY, VM_UPDATED_ON, VM_LIVESTATUS)
            VALUES (@p_VM_ID, @p_VM_CATID, @p_VM_LOC_ID, @p_VM_NAME, @p_VM_EMAIL, @p_VM_ADDRESS, @p_UpdatedBy, GETDATE(), @p_VM_LIVESTATUS);
        END
        ELSE
        BEGIN
            UPDATE VENDOR_MASTER
            SET VM_CATID = @p_VM_CATID, VM_LOC_ID = @p_VM_LOC_ID, VM_NAME = @p_VM_NAME,
                VM_EMAIL = @p_VM_EMAIL, VM_ADDRESS = @p_VM_ADDRESS, VM_UPDATED_BY = @p_UpdatedBy,
                VM_UPDATED_ON = GETDATE(), VM_LIVESTATUS = @p_VM_LIVESTATUS
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
PRINT '✓ usp_AddUpdateVendor procedure created';
GO

PRINT '';
PRINT '=== Deploying VENDOR Triggers ===';
GO

CREATE OR ALTER TRIGGER dbo.trg_VendorMaster_UpdateAudit
ON dbo.VENDOR_MASTER
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
PRINT '✓ trg_VendorMaster_UpdateAudit trigger created';
GO

PRINT '';
PRINT '========================================';
PRINT 'VENDORDB DEPLOYMENT COMPLETE';
PRINT '========================================';
PRINT '';
PRINT 'Database: VENDORDB';
PRINT 'Status: ✓ Successfully Deployed';
PRINT '';
PRINT 'Objects Created:';
PRINT '  ✓ 1 Table (VENDOR_MASTER)';
PRINT '  ✓ 2 Indexes';
PRINT '  ✓ 1 Procedure (usp_AddUpdateVendor)';
PRINT '  ✓ 1 Trigger (trg_VendorMaster_UpdateAudit)';
PRINT '';
PRINT '========================================';
GO

-- ==========================================
-- END OF VENDORDB DEPLOYMENT
-- ==========================================
