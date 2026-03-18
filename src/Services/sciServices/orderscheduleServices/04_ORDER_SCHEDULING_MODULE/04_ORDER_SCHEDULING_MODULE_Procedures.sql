-- ==========================================
-- ORDER SCHEDULING MODULE - Stored Procedures
-- Database: SCIDB
-- Module: Order Management & Scheduling
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

IF OBJECT_ID('dbo.usp_CreateOrder', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_CreateOrder;
GO
CREATE PROCEDURE dbo.usp_CreateOrder
    @p_CustomerCode VARCHAR(100),
    @p_OrderedDate DATETIME2,
    @p_CompanyUnitID DECIMAL(38),
    @p_ModifiedBy VARCHAR(255),
    @p_OrderID DECIMAL(38) OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.OS_TIED_ORDER_HEADER (CUSTOMER_CODE, ORDERED_DATE, COMPANY_UNIT_ID, MODIFIED_SCI_USER_ID, MODIFIED_DATE, RECORD_STATUS)
            VALUES (@p_CustomerCode, @p_OrderedDate, @p_CompanyUnitID, @p_ModifiedBy, GETDATE(), 'N');
            SET @p_OrderID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_AddOrderDetail', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_AddOrderDetail;
GO
CREATE PROCEDURE dbo.usp_AddOrderDetail
    @p_OrderID DECIMAL(38),
    @p_ItemID DECIMAL(38),
    @p_OrderQty BIGINT,
    @p_DispatchDate DATETIME2,
    @p_DetailID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO dbo.OS_TIED_ORDER_DETAILS (TIED_ORDER_ID, SCI_ITEM_ID, ORDER_QTY, DISPATCH_DATE, CANCEL_FLAG, MODIFIED_DATE)
            VALUES (@p_OrderID, @p_ItemID, @p_OrderQty, @p_DispatchDate, 'N', GETDATE());
            SET @p_DetailID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_GetOrderDetails', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetOrderDetails;
GO
CREATE PROCEDURE dbo.usp_GetOrderDetails
    @p_OrderID DECIMAL(38)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT otd.TIED_ORDER_DETAIL_ID, otd.SCI_ITEM_ID, otd.ORDER_QTY, otd.DISPATCH_DATE, otd.CANCEL_FLAG, otd.MODIFIED_DATE
        FROM dbo.OS_TIED_ORDER_DETAILS otd
        WHERE otd.TIED_ORDER_ID = @p_OrderID
        ORDER BY otd.TIED_ORDER_DETAIL_ID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'ORDER_SCHEDULING_MODULE Procedures created successfully.';
GO
