-- ==========================================
-- Database: StationeryDB
-- Microservice: Stationery (Procurement & Inventory) Service
-- Description: Stationery master, requests, orders, budgets, approvers
-- ==========================================

CREATE DATABASE StationeryDB;
GO

USE StationeryDB;
GO

-- Table: STATIONARY_MASTER
CREATE TABLE [STATIONARY_MASTER] (
    [SM_STATIONARYID] BIGINT NOT NULL,
    [SM_CATID] BIGINT NOT NULL,
    [SM_LOC_ID] BIGINT NOT NULL,
    [SM_DESC] VARCHAR(200) NOT NULL,
    [SM_UOMID] BIGINT NOT NULL,
    [SM_MAKE] VARCHAR(10) NOT NULL,
    [SM_PRICE_PERUNIT] BIGINT NULL,
    [SM_REORDER_LEVEL] BIGINT NULL,
    [SM_UPDATED_BY] BIGINT NOT NULL,
    [SM_UPDATED_ON] DATETIME2(3) NOT NULL,
    [SM_VMID] BIGINT NOT NULL,
    [SM_CLOSED] CHAR(1) NOT NULL,
    [SM_OPENINGSTOCK] BIGINT NOT NULL,
    CONSTRAINT [PK_STATIONARY_MASTER] PRIMARY KEY ([SM_STATIONARYID])
);

-- Table: ITEMDATA (if used)
CREATE TABLE [ITEMDATA] (
    [CATNAME] VARCHAR(40) NULL,
    [ITEMNAME] VARCHAR(60) NULL,
    [MAKE] VARCHAR(30) NULL,
    [UOM] VARCHAR(20) NULL,
    [PRICE] INT NULL
);

-- Table: SP_CATEGORY_DEFAULT
CREATE TABLE [SP_CATEGORY_DEFAULT] (
    [CD_STATIONERYID] BIGINT NOT NULL,
    [CD_CATEGORYID] BIGINT NOT NULL,
    [CD_LOCATIONID] BIGINT NOT NULL,
    [CD_MODIFIEDBY] BIGINT NOT NULL,
    [CD_MODIFIEDON] DATETIME2(3) NOT NULL
);

-- Table: SP_DEPT_APPROVER
CREATE TABLE [SP_DEPT_APPROVER] (
    [DA_LOCATION_ID] BIGINT NOT NULL,
    [DA_UNIT_CODE] CHAR(3) NOT NULL,
    [DA_DEPT_ID] BIGINT NOT NULL,
    [DA_EMP_SYSID] BIGINT NOT NULL,
    [DA_TYPE] CHAR(1) NOT NULL,
    [DA_EFFECTIVE_DATE] DATETIME2(3) NOT NULL,
    [DA_CLOSURE_DATE] DATETIME2(3) NULL,
    [DA_UPDATED_BY] BIGINT NOT NULL,
    [DA_UPDATED_ON] DATETIME2(3) NOT NULL
);

-- Table: SP_DEPT_BUDGET
CREATE TABLE [SP_DEPT_BUDGET] (
    [DB_LOCATION_ID] BIGINT NOT NULL,
    [DB_UNIT_CODE] CHAR(3) NOT NULL,
    [DB_DEPT_ID] BIGINT NOT NULL,
    [DB_FINYEAR_ID] BIGINT NOT NULL,
    [DB_BUDGETAMOUNT] BIGINT NOT NULL,
    [DB_UPDATED_BY] BIGINT NOT NULL,
    [DB_UPDATED_ON] DATETIME2(3) NOT NULL
);

-- Table: SP_LOCATION_ADMIN
CREATE TABLE [SP_LOCATION_ADMIN] (
    [LA_LOCATION_ID] BIGINT NOT NULL,
    [LA_EMP_SYSID] BIGINT NOT NULL,
    [LA_EFFECTIVE_DATE] DATETIME2(3) NOT NULL,
    [LA_CLOSURE_DATE] DATETIME2(3) NULL,
    [LA_UPDATED_BY] BIGINT NOT NULL,
    [LA_UPDATED_ON] DATETIME2(3) NOT NULL
);

-- Table: SP_ORDER_MAIN
CREATE TABLE [SP_ORDER_MAIN] (
    [OM_ORDERMAIN_ID] BIGINT NOT NULL,
    [OM_LOCATION_ID] BIGINT NOT NULL,
    [OM_VENDORID] BIGINT NOT NULL,
    [OM_DELIVERYDATE] DATETIME2(3) NOT NULL,
    [OM_ORDEREDDATE] DATETIME2(3) NOT NULL,
    [OM_ORDEREDBY] BIGINT NOT NULL,
    CONSTRAINT [PK_SP_ORDER_MAIN] PRIMARY KEY ([OM_ORDERMAIN_ID])
);

-- Table: SP_ORDER_SUB
CREATE TABLE [SP_ORDER_SUB] (
    [OS_ORDERSUB_ID] BIGINT NOT NULL,
    [OS_ORDERMAIN_ID] BIGINT NOT NULL,
    [OS_REQUESTSUB_ID] BIGINT NOT NULL,
    [OS_ORDERED_QTY] BIGINT NOT NULL,
    [OS_RECEIVEDON] DATETIME2(3) NULL,
    [OS_RECEIVED_BY] BIGINT NOT NULL,
    [OS_ORDERPRICE] BIGINT NOT NULL,
    [OS_ACTUALPRICE] BIGINT NOT NULL,
    [OS_RECEIVEDDATE] DATETIME2(3) NOT NULL,
    [OS_DELIVERYDATE] DATETIME2(3) NOT NULL,
    [OS_RECEIPTENTRYBY] BIGINT NULL,
    [OS_RECEIPTENTRYON] DATETIME2(3) NULL,
    CONSTRAINT [PK_SP_ORDER_SUB] PRIMARY KEY ([OS_ORDERSUB_ID])
);

-- Table: SP_REQUEST_MAIN
CREATE TABLE [SP_REQUEST_MAIN] (
    [RM_REQUESTID] BIGINT NOT NULL,
    [RM_REQUESTEDBY] BIGINT NOT NULL,
    [RM_REQUESTEDON] DATETIME2(3) NOT NULL,
    [RM_LOCATIONID] BIGINT NULL,
    [RM_UNITCODE] CHAR(3) NULL,
    CONSTRAINT [PK_SP_REQUEST_MAIN] PRIMARY KEY ([RM_REQUESTID])
);

-- Table: SP_REQUEST_SUB
CREATE TABLE [SP_REQUEST_SUB] (
    [RS_REQUESTSUB_ID] BIGINT NOT NULL,
    [RS_REQUESTID] BIGINT NOT NULL,
    [RS_STATIONARYID] BIGINT NOT NULL,
    [RS_DEPTID] BIGINT NOT NULL,
    [RS_EXPECTED_DATE] DATETIME2(3) NOT NULL,
    [RS_USER_SYSID] BIGINT NULL,
    [RS_REQUESTEDQTY] BIGINT NOT NULL,
    [RS_INDENTEDQTY] BIGINT NULL,
    [RS_APPROVEDQTY] BIGINT NULL,
    [RS_APPROVER_SYSID] BIGINT NULL,
    [RS_APPROVER_RAMARKS] VARCHAR(255) NULL,
    [RS_RECEIVED_DATE] DATETIME2(3) NULL,
    [RS_STATUS] VARCHAR(1) NOT NULL,
    [RS_UPDATED_BY] BIGINT NOT NULL,
    [RS_UPDATED_ON] DATETIME2(3) NOT NULL,
    [RS_APPROVED_ON] DATETIME2(3) NULL,
    CONSTRAINT [PK_SP_REQUEST_SUB] PRIMARY KEY ([RS_REQUESTSUB_ID])
);

-- Table: SP_UNIT_APPROVER
CREATE TABLE [SP_UNIT_APPROVER] (
    [UA_LOCATION_ID] BIGINT NOT NULL,
    [UA_UNIT_CODE] CHAR(3) NOT NULL,
    [UA_EMP_SYSID] BIGINT NOT NULL,
    [UA_TYPE] CHAR(1) NOT NULL,
    [UA_EFFECTIVE_DATE] DATETIME2(3) NOT NULL,
    [UA_CLOSURE_DATE] VARCHAR(255) NULL,
    [UA_UPDATED_BY] BIGINT NOT NULL,
    [UA_UPDATED_ON] DATETIME2(3) NOT NULL
);

-- Table: SP_UNIT_BUDGET
CREATE TABLE [SP_UNIT_BUDGET] (
    [UB_LOCATION_ID] BIGINT NOT NULL,
    [UB_UNIT_CODE] CHAR(3) NOT NULL,
    [UB_FINYEAR_ID] BIGINT NOT NULL,
    [UB_BUDGETAMOUNT] BIGINT NOT NULL,
    [UB_UPDATED_BY] BIGINT NOT NULL,
    [UB_UPDATED_ON] DATETIME2(3) NOT NULL
);

-- Table: STATIONERY_REORDER_ALERT (created by trigger)
CREATE TABLE [STATIONERY_REORDER_ALERT] (
    [AlertID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [StationaryID] BIGINT NOT NULL,
    [AlertDate] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [CurrentStock] BIGINT NOT NULL,
    [ReorderLevel] BIGINT NOT NULL,
    [Resolved] CHAR(1) DEFAULT 'N'
);

-- User-defined table type for request items
CREATE TYPE dbo.RequestItemType AS TABLE
(
    StationaryID BIGINT,
    DeptID BIGINT,
    ExpectedDate DATETIME2(3),
    RequestedQty BIGINT,
    Remarks VARCHAR(255)
);
GO

-- Function: fn_GetDeptRemainingBudget
-- TODO: This function originally joined FINYEAR_MASTER (now in ReferenceDB).
-- In a microservice architecture, obtain the current financial year ID via API call
-- and pass it as a parameter. The function below is kept as a stub – modify as needed.
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

    SELECT @TotalBudget = DB_BUDGETAMOUNT
    FROM SP_DEPT_BUDGET
    WHERE DB_LOCATION_ID = @p_LocationID
      AND DB_DEPT_ID = @p_DeptID
      AND DB_FINYEAR_ID = @p_FinYearID;

    -- Sum of approved quantities * price per unit for this department and year
    -- Note: This assumes the financial year start date is known; in practice you'd compare with the request date or use FY_ID.
    SELECT @ApprovedAmount = SUM(RS_APPROVEDQTY * SM_PRICE_PERUNIT)
    FROM SP_REQUEST_SUB RS
    INNER JOIN STATIONARY_MASTER SM ON RS.RS_STATIONARYID = SM.SM_STATIONARYID
    WHERE RS.RS_DEPTID = @p_DeptID
      AND RS.RS_STATUS IN ('A', 'P')
      -- AND YEAR(RS.RS_UPDATED_ON) = (SELECT FY_STARTDATE FROM FINYEAR_MASTER WHERE FY_ID = @p_FinYearID); -- This line must be replaced with proper logic

    RETURN ISNULL(@TotalBudget, 0) - ISNULL(@ApprovedAmount, 0);
END;
GO

-- Procedure: usp_StationeryRequestSubmit
-- TODO: Needs current financial year ID from Reference Service; currently uses hardcoded logic.
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
        SELECT @p_NewRequestID = ISNULL(MAX(RM_REQUESTID), 0) + 1 FROM SP_REQUEST_MAIN;
        INSERT INTO SP_REQUEST_MAIN (RM_REQUESTID, RM_REQUESTEDBY, RM_REQUESTEDON, RM_LOCATIONID, RM_UNITCODE)
        VALUES (@p_NewRequestID, @p_RequestedBy, GETDATE(), @p_LocationID, @p_UnitCode);

        DECLARE @SubID BIGINT;
        DECLARE @StationaryID BIGINT, @DeptID BIGINT, @ExpectedDate DATETIME2(3), @Qty BIGINT, @Remarks VARCHAR(255);

        DECLARE cur CURSOR FOR
            SELECT StationaryID, DeptID, ExpectedDate, RequestedQty, Remarks FROM @p_RequestItems;
        OPEN cur;
        FETCH NEXT FROM cur INTO @StationaryID, @DeptID, @ExpectedDate, @Qty, @Remarks;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- TODO: Replace with actual current FY ID obtained from Reference Service API
            DECLARE @FinYearID BIGINT = 1; -- Placeholder

            DECLARE @RemainingBudget BIGINT = dbo.fn_GetDeptRemainingBudget(@p_LocationID, @DeptID, @FinYearID);
            DECLARE @ItemPrice BIGINT = (SELECT SM_PRICE_PERUNIT FROM STATIONARY_MASTER WHERE SM_STATIONARYID = @StationaryID);
            IF @RemainingBudget < (@Qty * ISNULL(@ItemPrice, 0))
            BEGIN
                ROLLBACK TRANSACTION;
                RAISERROR('Insufficient budget for department %d.', 16, 1, @DeptID);
                RETURN;
            END

            SELECT @SubID = ISNULL(MAX(RS_REQUESTSUB_ID), 0) + 1 FROM SP_REQUEST_SUB;
            INSERT INTO SP_REQUEST_SUB (...)
            VALUES (...); -- (full insert omitted)

            FETCH NEXT FROM cur INTO @StationaryID, @DeptID, @ExpectedDate, @Qty, @Remarks;
        END

        CLOSE cur;
        DEALLOCATE cur;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_StationeryRequestApprove
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
        DECLARE @LocationID BIGINT, @DeptID BIGINT, @StationaryID BIGINT;
        SELECT @LocationID = RM_LOCATIONID, @DeptID = RS_DEPTID, @StationaryID = RS_STATIONARYID
        FROM SP_REQUEST_SUB RS
        INNER JOIN SP_REQUEST_MAIN RM ON RS.RS_REQUESTID = RM.RM_REQUESTID
        WHERE RS.RS_REQUESTSUB_ID = @p_RequestSubID;

        -- TODO: Replace with actual current FY ID
        DECLARE @FinYearID BIGINT = 1;
        DECLARE @RemainingBudget BIGINT = dbo.fn_GetDeptRemainingBudget(@LocationID, @DeptID, @FinYearID);
        DECLARE @ItemPrice BIGINT = (SELECT SM_PRICE_PERUNIT FROM STATIONARY_MASTER WHERE SM_STATIONARYID = @StationaryID);
        IF @RemainingBudget < (@p_ApprovedQty * ISNULL(@ItemPrice, 0))
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Insufficient budget for department %d.', 16, 1, @DeptID);
            RETURN;
        END

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
        DECLARE @OrderMainID BIGINT, @RequestSubID BIGINT, @StationaryID BIGINT;
        SELECT @OrderMainID = OS_ORDERMAIN_ID, @RequestSubID = OS_REQUESTSUB_ID
        FROM SP_ORDER_SUB
        WHERE OS_ORDERSUB_ID = @p_OrderSubID;

        SELECT @StationaryID = RS_STATIONARYID
        FROM SP_REQUEST_SUB
        WHERE RS_REQUESTSUB_ID = @RequestSubID;

        UPDATE SP_ORDER_SUB
        SET OS_RECEIVEDON = ISNULL(@p_ReceiptDate, GETDATE()),
            OS_RECEIVED_BY = @p_ReceivedBy,
            OS_RECEIVEDDATE = ISNULL(@p_ReceiptDate, GETDATE()),
            OS_RECEIPTENTRYBY = @p_ReceivedBy,
            OS_RECEIPTENTRYON = GETDATE()
        WHERE OS_ORDERSUB_ID = @p_OrderSubID;

        UPDATE STATIONARY_MASTER
        SET SM_OPENINGSTOCK = SM_OPENINGSTOCK + @p_ReceivedQty,
            SM_UPDATED_BY = @p_ReceivedBy,
            SM_UPDATED_ON = GETDATE()
        WHERE SM_STATIONARYID = @StationaryID;

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

-- Trigger: trg_StationeryRequestSub_StatusChange
CREATE OR ALTER TRIGGER dbo.trg_StationeryRequestSub_StatusChange
ON dbo.SP_REQUEST_SUB
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(RS_STATUS)
    BEGIN
        UPDATE RM
        SET RM.RM_REQUESTEDON = RM.RM_REQUESTEDON  -- dummy update
        FROM dbo.SP_REQUEST_MAIN RM
        WHERE EXISTS (
            SELECT 1
            FROM inserted I
            INNER JOIN deleted D ON I.RS_REQUESTSUB_ID = D.RS_REQUESTSUB_ID
            WHERE I.RS_STATUS <> D.RS_STATUS
              AND I.RS_REQUESTID = RM.RM_REQUESTID
              AND NOT EXISTS (
                  SELECT 1
                  FROM dbo.SP_REQUEST_SUB RS
                  WHERE RS.RS_REQUESTID = RM.RM_REQUESTID
                    AND RS.RS_STATUS NOT IN ('C', 'X')
              )
        );
    END
END;
GO

-- Trigger: trg_StationeryMaster_ReorderAlert
CREATE OR ALTER TRIGGER dbo.trg_StationeryMaster_ReorderAlert
ON dbo.STATIONARY_MASTER
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(SM_OPENINGSTOCK)
    BEGIN
        INSERT INTO dbo.STATIONERY_REORDER_ALERT (StationaryID, CurrentStock, ReorderLevel)
        SELECT I.SM_STATIONARYID, I.SM_OPENINGSTOCK, I.SM_REORDER_LEVEL
        FROM inserted I
        INNER JOIN deleted D ON I.SM_STATIONARYID = D.SM_STATIONARYID
        WHERE I.SM_OPENINGSTOCK < I.SM_REORDER_LEVEL
          AND (D.SM_OPENINGSTOCK >= D.SM_REORDER_LEVEL OR D.SM_OPENINGSTOCK IS NULL);
    END
END;
GO