-- ==========================================
-- MODULE: VENDOR
-- Component: Procedures
-- Description: Vendor management stored procedures
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- Procedure: usp_AddUpdateVendor
-- Purpose: Insert or update vendor master records with transaction support
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

-- ==========================================
-- END OF VENDOR PROCEDURES
-- ==========================================
