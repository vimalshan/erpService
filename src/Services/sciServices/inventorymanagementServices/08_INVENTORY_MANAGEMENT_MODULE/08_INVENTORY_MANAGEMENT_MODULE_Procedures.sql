-- ==========================================
-- INVENTORY MANAGEMENT MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Product & Item Management
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_RegisterProduct', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterProduct;
GO
CREATE PROCEDURE dbo.usp_RegisterProduct
    @p_ProductName VARCHAR(20),
    @p_ProductDescription VARCHAR(100),
    @p_UnitID INT,
    @p_ProductTypeID INT,
    @p_CompanyUnitID INT,
    @p_CreatedBy INT,
    @p_ProductID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.MAIN_PRODUCT_MASTER (PRODUCT_NAME, PRODUCT_DESCRIPTION, UNIT_ID, PRODUCT_TYPE_ID, COMPANY_UNIT_ID, SCI_USER_ID_CREATED, CREATION_DATE)
            VALUES (@p_ProductName, @p_ProductDescription, @p_UnitID, @p_ProductTypeID, @p_CompanyUnitID, @p_CreatedBy, GETDATE());
            SET @p_ProductID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_RegisterItem', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RegisterItem;
GO
CREATE PROCEDURE dbo.usp_RegisterItem
    @p_OracleCode VARCHAR(20),
    @p_ItemName VARCHAR(100),
    @p_MainProductID INT,
    @p_ItemType VARCHAR(20),
    @p_UnitID INT,
    @p_ConversionFactor DECIMAL(38),
    @p_HierarchyLevel INT,
    @p_ItemID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.ITEM_MASTER (ORACLE_CODE, ORACLE_ITEM_ID, ITEM_NAME, MAIN_PRODUCT_ID, ITEM_TYPE, ITEM_UOM_ID, MAIN_PRODUCT_UOM_CONFACTOR, ISBULK_SOURCE, ISBULK_ITEM)
            VALUES (@p_OracleCode, 0, @p_ItemName, @p_MainProductID, @p_ItemType, @p_UnitID, @p_ConversionFactor, 'N', 'N');
            SET @p_ItemID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

PRINT 'INVENTORY_MANAGEMENT_MODULE Procedures created successfully.';
GO
