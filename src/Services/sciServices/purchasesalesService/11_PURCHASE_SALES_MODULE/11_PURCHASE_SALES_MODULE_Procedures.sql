-- ==========================================
-- PURCHASE SALES MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Purchase & Sales Management
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_RecordPurchase', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RecordPurchase;
GO
CREATE PROCEDURE dbo.usp_RecordPurchase
    @p_TrackingNum BIGINT,
    @p_SupplierCode VARCHAR(25),
    @p_PurposeCode BIGINT,
    @p_StageCode BIGINT,
    @p_EnteredBy VARCHAR(25),
    @p_EnteredByNum BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.PURCHASE_DETAILS (PD_TRC_NUM, PD_TRN_NUM, PD_PUR_COD, PD_STG_COD, PD_SUP_COD, PD_USR_ID, PD_USR_NUM, PD_UPD_DAT)
            VALUES (@p_TrackingNum, 1, @p_PurposeCode, @p_StageCode, @p_SupplierCode, @p_EnteredBy, @p_EnteredByNum, GETDATE());
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_RecordSale', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RecordSale;
GO
CREATE PROCEDURE dbo.usp_RecordSale
    @p_TrackingNum BIGINT,
    @p_PurposeCode BIGINT,
    @p_StageCode BIGINT,
    @p_ProductCode VARCHAR(25),
    @p_EnteredBy VARCHAR(25),
    @p_EnteredByNum BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.SALE_MAIN (SL_TRC_NUM, SL_TRN_NUM, SL_PUR_COD, SL_STG_COD, SL_USR_ID, SL_USR_NUM, SL_UPD_DAT)
            VALUES (@p_TrackingNum, 1, @p_PurposeCode, @p_StageCode, @p_EnteredBy, @p_EnteredByNum, GETDATE());
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

PRINT 'PURCHASE_SALES_MODULE Procedures created successfully.';
GO
