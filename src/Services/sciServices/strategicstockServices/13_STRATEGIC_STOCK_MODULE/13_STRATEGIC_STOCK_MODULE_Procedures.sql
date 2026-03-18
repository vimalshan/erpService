-- ==========================================
-- STRATEGIC STOCK MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Strategic Stock Management
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_GetStrategicStockInfo', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetStrategicStockInfo;
GO
CREATE PROCEDURE dbo.usp_GetStrategicStockInfo
    @p_ItemID INT,
    @p_CompanyUnitID INT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT ss.STRATEGIC_STOCK_ID, ss.STRATEGIC_STOCK_TYPE, ss.MAX_QTY, ss.FILLED_QTY, ss.EFFECTIVE_DATE
        FROM dbo.STRATEGIC_STOCK ss
        WHERE ss.SCI_ITEM_ID = @p_ItemID
        AND ss.COMPANY_UNIT_ID = @p_CompanyUnitID
        AND (ss.CLOSURE_DATE IS NULL OR ss.CLOSURE_DATE >= CAST(GETDATE() AS DATE));
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'STRATEGIC_STOCK_MODULE Procedures created successfully.';
GO
