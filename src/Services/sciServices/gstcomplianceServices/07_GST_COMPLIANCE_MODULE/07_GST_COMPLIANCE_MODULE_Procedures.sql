-- ==========================================
-- GST COMPLIANCE MODULE - Stored Procedures
-- Database: SCIDB
-- Module: GST Management
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_RegisterGST', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterGST;
GO
CREATE PROCEDURE dbo.usp_RegisterGST
    @p_PANNo VARCHAR(20),
    @p_Type CHAR(1) = NULL,
    @p_Email VARCHAR(200) = NULL,
    @p_Mobile BIGINT = NULL,
    @p_RegisteredBy BIGINT,
    @p_GSTID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.GST_MAIN (GST_PANNO, GST_TYPE, GST_EMAILID, GST_MOBILENO, GST_CREATEDON, GST_ENTEREDBY, GST_STATUS, GST_DIGITALFLAG)
            VALUES (@p_PANNo, @p_Type, @p_Email, @p_Mobile, GETDATE(), @p_RegisteredBy, 'P', 'N');
            SET @p_GSTID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_GetGSTDetails', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetGSTDetails;
GO
CREATE PROCEDURE dbo.usp_GetGSTDetails
    @p_GSTID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT gm.GST_ID, gm.GST_PANNO, gm.GST_EMAILID, gm.GST_MOBILENO, gm.GST_STATUS, gm.GST_CREATEDON
        FROM dbo.GST_MAIN gm
        WHERE gm.GST_ID = @p_GSTID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'GST_COMPLIANCE_MODULE Procedures created successfully.';
GO
