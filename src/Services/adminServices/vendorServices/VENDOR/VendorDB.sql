-- ==========================================
-- Database: VendorDB
-- Microservice: Vendor Service
-- Description: Vendor master, TDS vendors, TDS file details
-- ==========================================

CREATE DATABASE VendorDB;
GO

USE VendorDB;
GO

-- Table: VENDOR_MASTER
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

-- Table: TDS_VENDORS
CREATE TABLE [TDS_VENDORS] (
    [VENDOR_ID] BIGINT NULL,
    [VENDOR_NAME] VARCHAR(240) NULL,
    [EMAIL_ADDRESS] VARCHAR(3000) NULL,
    [PAN_NO] VARCHAR(30) NULL
);

-- Table: TDSFILE_DETAILS
CREATE TABLE [TDSFILE_DETAILS] (
    [FILE_ID] BIGINT NOT NULL,
    [FILE_NAME] VARCHAR(100) NULL,
    [PAN_NO] VARCHAR(15) NULL,
    [EMAIL_STATUS] VARCHAR(1) NULL,
    [FILE_TYPE] VARCHAR(3) NULL,
    CONSTRAINT [PK_TDSFILE_DETAILS] PRIMARY KEY ([FILE_ID])
);

-- Procedure: usp_AddUpdateVendor
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
            UPDATE VENDOR_MASTER
            SET VM_CATID = @p_VM_CATID,
                VM_LOC_ID = @p_VM_LOC_ID,
                VM_NAME = @p_VM_NAME,
                VM_EMAIL = @p_VM_EMAIL,
                VM_ADDRESS = @p_VM_ADDRESS,
                VM_UPDATED_BY = @p_UpdatedBy,
                VM_UPDATED_ON = GETDATE(),
                VM_LIVESTATUS = @p_VM_LIVESTATUS
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

-- Trigger: trg_VendorMaster_UpdateAudit
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