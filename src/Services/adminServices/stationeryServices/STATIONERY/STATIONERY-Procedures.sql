-- ==========================================
-- MODULE: STATIONERY
-- Component: Procedures and Functions
-- Description: Stationery request, approval, order, and receipt procedures
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- Function: fn_GetDeptRemainingBudget
-- Purpose: Calculates remaining budget for a department after considering approved requests
CREATE OR ALTER FUNCTION dbo.fn_GetDeptRemainingBudget
(
    @p_LocationID BIGINT,
    @p_DeptID BIGINT,
    @p_FinYearID BIGINT
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @TotalBudget BIGINT;
    DECLARE @ApprovedAmount BIGINT;

    -- Get total budget allocated
    SELECT @TotalBudget = DB_BUDGETAMOUNT
    FROM SP_DEPT_BUDGET
    WHERE DB_LOCATION_ID = @p_LocationID
      AND DB_DEPT_ID = @p_DeptID
      AND DB_FINYEAR_ID = @p_FinYearID;

    -- Sum of approved quantities * price per unit for this department and year
    SELECT @ApprovedAmount = SUM(RS_APPROVEDQTY * SM_PRICE_PERUNIT)
    FROM SP_REQUEST_SUB RS
    INNER JOIN STATIONARY_MASTER SM ON RS.RS_STATIONARYID = SM.SM_STATIONARYID
    WHERE RS.RS_DEPTID = @p_DeptID
      AND RS.RS_STATUS IN ('A', 'P');

    RETURN ISNULL(@TotalBudget, 0) - ISNULL(@ApprovedAmount, 0);
END;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- Procedure: usp_StationeryRequestSubmit
-- Purpose: Submit a new stationery request with multiple items and verify budget
CREATE OR ALTER PROCEDURE dbo.usp_StationeryRequestSubmit
(
    @p_RequestedBy BIGINT,
    @p_LocationID BIGINT,
    @p_UnitCode CHAR(3),
    @p_RequestItems dbo.RequestItemType READONLY,
    @p_NewRequestID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Generate new request main ID
        SELECT @p_NewRequestID = ISNULL(MAX(RM_REQUESTID), 0) + 1 FROM SP_REQUEST_MAIN;

        -- Insert main request
        INSERT INTO SP_REQUEST_MAIN (RM_REQUESTID, RM_REQUESTEDBY, RM_REQUESTEDON, RM_LOCATIONID, RM_UNITCODE)
        VALUES (@p_NewRequestID, @p_RequestedBy, GETDATE(), @p_LocationID, @p_UnitCode);

        -- Insert sub items
        DECLARE @SubID BIGINT;
        DECLARE @Cur CURSOR;
        DECLARE @StationaryID BIGINT, @DeptID BIGINT, @ExpectedDate DATETIME2(3), @Qty BIGINT, @Remarks VARCHAR(255);

        SET @Cur = CURSOR FOR
            SELECT StationaryID, DeptID, ExpectedDate, RequestedQty, Remarks FROM @p_RequestItems;
        OPEN @Cur;
        FETCH NEXT FROM @Cur INTO @StationaryID, @DeptID, @ExpectedDate, @Qty, @Remarks;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Check budget for each department
            DECLARE @FinYearID BIGINT;
            SELECT TOP 1 @FinYearID = FY_ID FROM FINYEAR_MASTER WHERE GETDATE() BETWEEN FY_STARTDATE AND FY_CLOSEDATE;
            DECLARE @RemainingBudget BIGINT = dbo.fn_GetDeptRemainingBudget(@p_LocationID, @DeptID, @FinYearID);
            DECLARE @ItemPrice BIGINT = (SELECT SM_PRICE_PERUNIT FROM STATIONARY_MASTER WHERE SM_STATIONARYID = @StationaryID);
            
            IF @RemainingBudget < (@Qty * ISNULL(@ItemPrice, 0))
            BEGIN
                ROLLBACK TRANSACTION;
                RAISERROR('Insufficient budget for department %d.', 16, 1, @DeptID);
                RETURN;
            END

            -- Generate new sub ID
            SELECT @SubID = ISNULL(MAX(RS_REQUESTSUB_ID), 0) + 1 FROM SP_REQUEST_SUB;

            INSERT INTO SP_REQUEST_SUB
            (
                RS_REQUESTSUB_ID, RS_REQUESTID, RS_STATIONARYID, RS_DEPTID,
                RS_EXPECTED_DATE, RS_USER_SYSID, RS_REQUESTEDQTY, RS_INDENTEDQTY,
                RS_APPROVEDQTY, RS_APPROVER_SYSID, RS_APPROVER_RAMARKS,
                RS_RECEIVED_DATE, RS_STATUS, RS_UPDATED_BY, RS_UPDATED_ON, RS_APPROVED_ON
            )
            VALUES
            (
                @SubID, @p_NewRequestID, @StationaryID, @DeptID,
                @ExpectedDate, @p_RequestedBy, @Qty, NULL,
                NULL, NULL, @Remarks,
                NULL, 'P', @p_RequestedBy, GETDATE(), NULL
            );

            FETCH NEXT FROM @Cur INTO @StationaryID, @DeptID, @ExpectedDate, @Qty, @Remarks;
        END

        CLOSE @Cur;
        DEALLOCATE @Cur;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_StationeryRequestApprove
-- Purpose: Approve individual request sub items with budget verification
CREATE OR ALTER PROCEDURE dbo.usp_StationeryRequestApprove
(
    @p_RequestSubID BIGINT,
    @p_ApprovedQty BIGINT,
    @p_ApproverSysID BIGINT,
    @p_Remarks VARCHAR(255) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Get request details
        DECLARE @LocationID BIGINT, @DeptID BIGINT, @StationaryID BIGINT, @RequestedQty BIGINT;
        SELECT @LocationID = RM_LOCATIONID, @DeptID = RS_DEPTID, @StationaryID = RS_STATIONARYID, @RequestedQty = RS_REQUESTEDQTY
        FROM SP_REQUEST_SUB RS
        INNER JOIN SP_REQUEST_MAIN RM ON RS.RS_REQUESTID = RM.RM_REQUESTID
        WHERE RS.RS_REQUESTSUB_ID = @p_RequestSubID;

        -- Check budget
        DECLARE @FinYearID BIGINT;
        SELECT TOP 1 @FinYearID = FY_ID FROM FINYEAR_MASTER WHERE GETDATE() BETWEEN FY_STARTDATE AND FY_CLOSEDATE;
        DECLARE @RemainingBudget BIGINT = dbo.fn_GetDeptRemainingBudget(@LocationID, @DeptID, @FinYearID);
        DECLARE @ItemPrice BIGINT = (SELECT SM_PRICE_PERUNIT FROM STATIONARY_MASTER WHERE SM_STATIONARYID = @StationaryID);
        
        IF @RemainingBudget < (@p_ApprovedQty * ISNULL(@ItemPrice, 0))
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Insufficient budget for department %d.', 16, 1, @DeptID);
            RETURN;
        END

        -- Update the request sub
        UPDATE SP_REQUEST_SUB
        SET RS_APPROVEDQTY = @p_ApprovedQty,
            RS_APPROVER_SYSID = @p_ApproverSysID,
            RS_APPROVER_RAMARKS = ISNULL(@p_Remarks, RS_APPROVER_RAMARKS),
            RS_STATUS = 'A',
            RS_APPROVED_ON = GETDATE(),
            RS_UPDATED_BY = @p_ApproverSysID,
            RS_UPDATED_ON = GETDATE()
        WHERE RS_REQUESTSUB_ID = @p_RequestSubID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_ReceiveOrder
-- Purpose: Record receipt of ordered items, update stock, and mark request as received
CREATE OR ALTER PROCEDURE dbo.usp_ReceiveOrder
(
    @p_OrderSubID BIGINT,
    @p_ReceivedQty BIGINT,
    @p_ReceivedBy BIGINT,
    @p_ReceiptDate DATETIME2(3) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Get order details
        DECLARE @OrderMainID BIGINT, @RequestSubID BIGINT, @StationaryID BIGINT, @OrderedQty BIGINT;
        SELECT @OrderMainID = OS_ORDERMAIN_ID, @RequestSubID = OS_REQUESTSUB_ID, @OrderedQty = OS_ORDERED_QTY
        FROM SP_ORDER_SUB
        WHERE OS_ORDERSUB_ID = @p_OrderSubID;

        -- Get Stationary ID from request sub
        SELECT @StationaryID = RS_STATIONARYID
        FROM SP_REQUEST_SUB
        WHERE RS_REQUESTSUB_ID = @RequestSubID;

        -- Update order sub with receipt info
        UPDATE SP_ORDER_SUB
        SET OS_RECEIVEDON = ISNULL(@p_ReceiptDate, GETDATE()),
            OS_RECEIVED_BY = @p_ReceivedBy,
            OS_RECEIVEDDATE = ISNULL(@p_ReceiptDate, GETDATE()),
            OS_RECEIPTENTRYBY = @p_ReceivedBy,
            OS_RECEIPTENTRYON = GETDATE()
        WHERE OS_ORDERSUB_ID = @p_OrderSubID;

        -- Update stock in stationary master
        UPDATE STATIONARY_MASTER
        SET SM_OPENINGSTOCK = SM_OPENINGSTOCK + @p_ReceivedQty,
            SM_UPDATED_BY = @p_ReceivedBy,
            SM_UPDATED_ON = GETDATE()
        WHERE SM_STATIONARYID = @StationaryID;

        -- Update request sub received date and status
        UPDATE SP_REQUEST_SUB
        SET RS_RECEIVED_DATE = ISNULL(@p_ReceiptDate, GETDATE()),
            RS_STATUS = 'C',
            RS_UPDATED_BY = @p_ReceivedBy,
            RS_UPDATED_ON = GETDATE()
        WHERE RS_REQUESTSUB_ID = @RequestSubID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ==========================================
-- END OF STATIONERY PROCEDURES
-- ==========================================
