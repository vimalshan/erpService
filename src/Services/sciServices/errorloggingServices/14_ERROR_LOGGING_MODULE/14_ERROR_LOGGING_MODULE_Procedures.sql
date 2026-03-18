-- ==========================================
-- ERROR LOGGING MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Error Handling & Logging
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_LogError', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_LogError;
GO
CREATE PROCEDURE dbo.usp_LogError
    @p_ErrorMessage VARCHAR(4000),
    @p_StoredProcedureName VARCHAR(100),
    @p_ErrorReference INT = NULL
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.ERRSP (ERR_MESS, ERR_SP, ERR_REF, ERR_DATE)
        VALUES (@p_ErrorMessage, @p_StoredProcedureName, @p_ErrorReference, GETDATE());
    END TRY
    BEGIN CATCH
        PRINT 'Error logging failed: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_GetErrorLog', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetErrorLog;
GO
CREATE PROCEDURE dbo.usp_GetErrorLog
    @p_StartDate DATETIME2,
    @p_EndDate DATETIME2
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT TOP 1000 ERR_MESS, ERR_SP, ERR_REF, ERR_DATE
        FROM dbo.ERRSP
        WHERE ERR_DATE BETWEEN @p_StartDate AND @p_EndDate
        ORDER BY ERR_DATE DESC;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'ERROR_LOGGING_MODULE Procedures created successfully.';
GO
