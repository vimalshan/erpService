-- ==========================================
-- EXIM MANAGEMENT MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Export-Import Management
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_RegisterEximProduct', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterEximProduct;
GO
CREATE PROCEDURE dbo.usp_RegisterEximProduct
    @p_ProductName VARCHAR(100),
    @p_OracleCode VARCHAR(50) = NULL,
    @p_UpdatedBy BIGINT,
    @p_ProductID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.EXIM_PRODUCT (PRODUCT_NAME, PRODUCT_ORACLE_CODE, LAST_UPDATED_BY, LAST_UPDATED_ON, STATUS)
            VALUES (@p_ProductName, @p_OracleCode, @p_UpdatedBy, GETDATE(), 'Y');
            SET @p_ProductID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_GetEximData', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetEximData;
GO
CREATE PROCEDURE dbo.usp_GetEximData
    @p_StartDate DATETIME2,
    @p_EndDate DATETIME2,
    @p_FileType VARCHAR(10)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @p_FileType = 'EXPORT'
            SELECT TOP 1000 ede.* FROM dbo.EXIM_DATA_EXPORT ede
            WHERE ede.EXIM_DATE BETWEEN @p_StartDate AND @p_EndDate
            ORDER BY ede.EXIM_DATE DESC;
        ELSE IF @p_FileType = 'IMPORT'
            SELECT TOP 1000 edi.* FROM dbo.EXIM_DATA_IMPORT edi
            WHERE edi.EXIM_DATE BETWEEN @p_StartDate AND @p_EndDate
            ORDER BY edi.EXIM_DATE DESC;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'EXIM_MANAGEMENT_MODULE Procedures created successfully.';
GO
