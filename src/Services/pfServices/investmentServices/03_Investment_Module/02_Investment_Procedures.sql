-- =========================================================================
-- INVESTMENT MODULE - Stored Procedures and Functions
-- Database: PFDB
-- Module: Investment Portfolio Management
-- Description: Procedures for investment operations
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- PROCEDURE: Record Investment Purchase
IF OBJECT_ID('dbo.usp_RecordInvestmentPurchase', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RecordInvestmentPurchase;
GO

CREATE PROCEDURE dbo.usp_RecordInvestmentPurchase
    @p_InvNo BIGINT,
    @p_CategoryID INT,
    @p_Units DECIMAL(19,0),
    @p_PurchaseRate DECIMAL(19,0),
    @p_PurchaseDate DATETIME2(3),
    @p_MaturityDate DATETIME2(3),
    @p_InterestRate DECIMAL(19,0),
    @p_EnteredBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.INV_MAIN (
            INV_NO, INV_CATID, INV_UNITS, INV_PURRATE, INV_PURDATE, INV_MATDATE,
            INV_ISSINTRATE, INV_PURVALUE, INV_STATUS, INV_ENTEREDBY, INV_ENTEREDON
        ) VALUES (
            @p_InvNo, @p_CategoryID, @p_Units, @p_PurchaseRate, @p_PurchaseDate, @p_MaturityDate,
            @p_InterestRate, (@p_Units * @p_PurchaseRate), 'A', @p_EnteredBy, GETDATE()
        );
        
        COMMIT TRANSACTION;
        PRINT 'Investment recorded successfully';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- PROCEDURE: Record Investment Redemption
IF OBJECT_ID('dbo.usp_RecordInvestmentRedemption', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RecordInvestmentRedemption;
GO

CREATE PROCEDURE dbo.usp_RecordInvestmentRedemption
    @p_SaleNo BIGINT,
    @p_InvNo BIGINT,
    @p_SaleType CHAR(1),
    @p_SaleDate DATETIME2(3),
    @p_SaleValue DECIMAL(19,0),
    @p_EnteredBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.INV_SALEDET (
            INV_SALENO, INV_NO, INV_SALETYPE, INV_SALEDATE, INV_INTADJUSTED,
            INV_SALPREMIUM, INV_SALVALUE, INV_SALTRANID, INV_ENTEREDBY, INV_ENTEREDON,
            INV_LASTMODBY, INV_LASTMODON
        ) VALUES (
            @p_SaleNo, @p_InvNo, @p_SaleType, @p_SaleDate, 0, 0, @p_SaleValue, 0,
            @p_EnteredBy, GETDATE(), @p_EnteredBy, GETDATE()
        );
        
        -- Update investment status
        UPDATE dbo.INV_MAIN SET INV_STATUS = 'R' WHERE INV_NO = @p_InvNo;
        
        COMMIT TRANSACTION;
        PRINT 'Redemption recorded successfully';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- VIEW: Investment Portfolio Summary
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_InvestmentPortfolio' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_InvestmentPortfolio AS
    SELECT 
        im.INV_NO,
        ic.INVCAT_NAME,
        im.INV_UNITS,
        im.INV_PURRATE,
        im.INV_PURVALUE,
        im.INV_ISSINTRATE,
        im.INV_PURDATE,
        im.INV_MATDATE,
        im.INV_STATUS
    FROM dbo.INV_MAIN im
    LEFT JOIN dbo.INVCAT_MAST ic ON im.INV_CATID = ic.INVCAT_CODE
    WHERE im.INV_STATUS IN ('A', 'M');
END
GO

PRINT 'Investment Module Procedures created successfully!';
GO
