-- ==========================================
-- MAM ALLOCATION MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Material Allocation Management
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_GetAllocationSummary', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetAllocationSummary;
GO
CREATE PROCEDURE dbo.usp_GetAllocationSummary
    @p_AllocationDate DATETIME2,
    @p_RawMaterialCode INT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT mad.ALL_DATE, mad.ALL_RM, mad.ALL_PROD, mad.ALL_CONS, mad.ALL_SALE
        FROM dbo.MAM_ALLOCATION_DET mad
        WHERE mad.ALL_DATE = @p_AllocationDate
        AND mad.ALL_RM = @p_RawMaterialCode;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'MAM_ALLOCATION_MODULE Procedures created successfully.';
GO
