-- ==========================================
-- Stored Procedures, Functions, Triggers for ADMINDB
-- ==========================================
-- This script adds procedural logic to the ADMINDB schema.
-- It includes functions, stored procedures with transaction handling,
-- and triggers to maintain data integrity and automate common tasks.
-- ==========================================

USE [ADMINDB];
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_GetScholarshipEligibleAmount
-- Purpose:  Returns the eligible scholarship amount for a given grade,
--           exam type, and financial year.
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetScholarshipEligibleAmount
(
    @p_GradeCat CHAR(3),
    @p_EligibleExam VARCHAR(2),
    @p_Year INT
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @Amount BIGINT;

    SELECT TOP 1 @Amount = SCH_ELGIBLEAMOUNT
    FROM SCHOLARSHIP_AMOUNT
    WHERE SCH_GRADECAT = @p_GradeCat
      AND SCH_ELGIBLEEXAM = @p_EligibleExam
      AND @p_Year BETWEEN SCH_FROMYEAR AND ISNULL(SCH_CLOSEYEAR, @p_Year)
    ORDER BY SCH_FROMYEAR DESC;

    RETURN ISNULL(@Amount, 0);
END;
GO

-- ------------------------------------------------------------------
-- Function: fn_GetDeptRemainingBudget
-- Purpose:  Calculates remaining budget for a department in a given
--           financial year after considering approved stationary requests.
-- ------------------------------------------------------------------
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
      AND RS.RS_STATUS IN ('A', 'P')  -- Approved or Partially processed
      AND YEAR(RS.RS_UPDATED_ON) = (SELECT FY_STARTDATE FROM FINYEAR_MASTER WHERE FY_ID = @p_FinYearID); -- Simplified year match

    RETURN ISNULL(@TotalBudget, 0) - ISNULL(@ApprovedAmount, 0);
END;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_ScholarshipApplication
-- Purpose:  Insert a new scholarship application (main record) and create
--           the first detail record. Uses transaction to ensure consistency.
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ScholarshipApplication
(
    @p_SCH_EMPSYSID INT,
    @p_SCH_GRADEID INT,
    @p_SCH_DEPENDID INT,
    @p_SCH_CHILDNAME VARCHAR(100),
    @p_SCH_LASTSCHOOL VARCHAR(100),
    @p_SCH_LASTYEAROFSCHOOL DECIMAL(38,0),
    @p_SCH_LASTEXAM CHAR(2),
    @p_SCH_CGPAFLAG CHAR(1),
    @p_SCH_MARKSPER DECIMAL(19,0),
    @p_SCH_MARKSGPA DECIMAL(19,0),
    @p_SCH_MARKSFILE VARCHAR(100),
    @p_SCH_COURSENAME VARCHAR(100),
    @p_SCH_COURSEJOINYEAR INT,
    @p_SCH_COURSEJOINMONTH DECIMAL(20,0),
    @p_SCH_COURSEDURATION BIGINT,
    @p_SCH_ADMRECPTFILE VARCHAR(100) = NULL,
    @p_SCH_PAYMODE CHAR(3) = NULL,
    @p_SCH_CHILDACCNO VARCHAR(20) = NULL,
    @p_SCH_CHILLDBANKIFSC VARCHAR(12) = NULL,
    @p_SCH_CHILLDBANKMICR VARCHAR(12) = NULL,
    @p_SCH_ENTRYSTATUS CHAR(1) = 'E',  -- Default Entered
    @p_SCH_SOURCE CHAR(1),
    @p_SCH_DISBAMOUNT DECIMAL(19,0),
    @p_SCH_DISBFREQ CHAR(1),
    @p_SCH_LIVESTATUS CHAR(1) = 'A',
    @p_CreatedBy INT,
    @p_SCH_OFFLINE CHAR(1) = 'N',
    @p_SCH_OFFLINEYEAR INT = NULL,
    @p_NewSchID INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Generate new SCH_ID (simplified - in production use sequence or identity)
        SELECT @p_NewSchID = ISNULL(MAX(SCH_ID), 0) + 1 FROM SCHOLARSHIP_MAIN;

        -- Insert main record
        INSERT INTO SCHOLARSHIP_MAIN
        (
            SCH_ID, SCH_EMPSYSID, SCH_GRADEID, SCH_DEPENDID, SCH_CHILDNAME,
            SCH_LASTSCHOOL, SCH_LASTYEAROFSCHOOL, SCH_LASTEXAM, SCH_CGPAFLAG,
            SCH_MARKSPER, SCH_MARKSGPA, SCH_MARKSFILE, SCH_COURSENAME,
            SCH_COURSEJOINYEAR, SCH_COURSEJOINMONTH, SCH_COURSEDURATION,
            SCH_ADMRECPTFILE, SCH_PAYMODE, SCH_CHILDACCNO, SCH_CHILLDBANKIFSC,
            SCH_CHILLDBANKMICR, SCH_ENTRYSTATUS, SCH_SOURCE, SCH_DISBAMOUNT,
            SCH_DISBFREQ, SCH_LIVESTATUS, SCH_CREATEDON, SCH_CREATEDBY,
            SCH_UPDATEDON, SCH_UPDATEDBY, SCH_APPROVALBY, SCH_APPROVALON,
            SCH_APPREMARKS, SCH_STOPREASON, SCH_STOPDATE, SCH_STOPENTEREDON,
            SCH_STOPENTEREDBY, SCH_OFFLINE, SCH_OFFLINEYEAR
        )
        VALUES
        (
            @p_NewSchID, @p_SCH_EMPSYSID, @p_SCH_GRADEID, @p_SCH_DEPENDID, @p_SCH_CHILDNAME,
            @p_SCH_LASTSCHOOL, @p_SCH_LASTYEAROFSCHOOL, @p_SCH_LASTEXAM, @p_SCH_CGPAFLAG,
            @p_SCH_MARKSPER, @p_SCH_MARKSGPA, @p_SCH_MARKSFILE, @p_SCH_COURSENAME,
            @p_SCH_COURSEJOINYEAR, @p_SCH_COURSEJOINMONTH, @p_SCH_COURSEDURATION,
            @p_SCH_ADMRECPTFILE, @p_SCH_PAYMODE, @p_SCH_CHILDACCNO, @p_SCH_CHILLDBANKIFSC,
            @p_SCH_CHILLDBANKMICR, @p_SCH_ENTRYSTATUS, @p_SCH_SOURCE, @p_SCH_DISBAMOUNT,
            @p_SCH_DISBFREQ, @p_SCH_LIVESTATUS, GETDATE(), @p_CreatedBy,
            GETDATE(), @p_CreatedBy, 0, GETDATE(),  -- dummy approval values
            '', '', GETDATE(), GETDATE(), 0,
            @p_SCH_OFFLINE, @p_SCH_OFFLINEYEAR
        );

        -- Create first scholarship detail record (for current year)
        DECLARE @NewDetID BIGINT;
        SELECT @NewDetID = ISNULL(MAX(SCHDET_ID), 0) + 1 FROM SCHOLARSHIP_DETAIL;

        INSERT INTO SCHOLARSHIP_DETAIL
        (
            SCHDET_ID, SCHDET_MAINID, SCHDET_YEAR, SCHDET_MARKSFILE,
            SCHDET_MARKSTATUS, SCHDET_PAYSTATUS, SCHDET_CREATEDON, SCHDET_CREATEDBY,
            SCHDET_UPDATEDON, SCHDET_UPDATEDBY, SCHDET_APPROVEDON, SCHDET_APPROVEDBY,
            SCHDET_PAYAPPROVEDON, SCHDET_PAYAPPROVEDBY, SCHDET_PAYDATE,
            SCHDET_PAYAMOUNT, SCHDET_PAYUPDATEDON, SCHDET_PAYUPDATEDBY
        )
        VALUES
        (
            @NewDetID, @p_NewSchID, @p_SCH_COURSEJOINYEAR, @p_SCH_MARKSFILE,
            'P',  -- Pending
            'S',  -- Scheduled
            GETDATE(), @p_CreatedBy,
            GETDATE(), @p_CreatedBy,
            NULL, NULL,
            NULL, NULL, NULL,
            NULL, NULL, NULL
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ScholarshipApprove
-- Purpose:  Approve a scholarship main record, update status and
--           set disbursement amount based on rules.
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ScholarshipApprove
(
    @p_SCH_ID INT,
    @p_ApprovedBy INT,
    @p_AppRemarks VARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Update main record
        UPDATE SCHOLARSHIP_MAIN
        SET SCH_ENTRYSTATUS = 'A',  -- Approved
            SCH_APPROVALBY = @p_ApprovedBy,
            SCH_APPROVALON = GETDATE(),
            SCH_APPREMARKS = ISNULL(@p_AppRemarks, ''),
            SCH_UPDATEDON = GETDATE(),
            SCH_UPDATEDBY = @p_ApprovedBy
        WHERE SCH_ID = @p_SCH_ID;

        -- Optionally update detail records to reflect approval
        UPDATE SCHOLARSHIP_DETAIL
        SET SCHDET_MARKSTATUS = 'A',  -- Approved
            SCHDET_APPROVEDON = GETDATE(),
            SCHDET_APPROVEDBY = @p_ApprovedBy,
            SCHDET_UPDATEDON = GETDATE(),
            SCHDET_UPDATEDBY = @p_ApprovedBy
        WHERE SCHDET_MAINID = @p_SCH_ID
          AND SCHDET_MARKSTATUS = 'P';  -- Only pending details

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_StationeryRequestSubmit
-- Purpose:  Submit a new stationery request with multiple items.
--           Checks department budget before allowing submission.
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_StationeryRequestSubmit
(
    @p_RequestedBy BIGINT,
    @p_LocationID BIGINT,
    @p_UnitCode CHAR(3),
    @p_RequestItems dbo.RequestItemType READONLY,  -- User-defined table type (see below)
    @p_NewRequestID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    -- Define table type if not exists (run separately)
    /*
    CREATE TYPE dbo.RequestItemType AS TABLE
    (
        StationaryID BIGINT,
        DeptID BIGINT,
        ExpectedDate DATETIME2(3),
        RequestedQty BIGINT,
        Remarks VARCHAR(255)
    );
    */
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
                NULL, 'P', @p_RequestedBy, GETDATE(), NULL  -- Pending
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

-- ------------------------------------------------------------------
-- Procedure: ussp_StationeryRequestApprove
-- Purpose:  Approve individual request sub items, update approved quantity,
--           and check budget again.
-- ------------------------------------------------------------------
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
            RS_STATUS = 'A',  -- Approved
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

-- ------------------------------------------------------------------
-- Procedure: usp_ReceiveOrder
-- Purpose:  Record receipt of ordered items, update stock, and mark
--           request sub as received.
-- ------------------------------------------------------------------
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

        -- Update request sub received date and status (optional)
        UPDATE SP_REQUEST_SUB
        SET RS_RECEIVED_DATE = ISNULL(@p_ReceiptDate, GETDATE()),
            RS_STATUS = 'C',  -- Completed
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

-- ------------------------------------------------------------------
-- Procedure: usp_AddUpdateVendor
-- Purpose:  Insert or update a vendor master record with transaction.
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_AddUpdateVendor
(
    @p_VM_ID BIGINT = NULL,  -- NULL for insert
    @p_VM_CATID BIGINT,
    @p_VM_LOC_ID BIGINT,
    @p_VM_NAME VARCHAR(100),
    @p_VM_EMAIL VARCHAR(50) = NULL,
    @p_VM_ADDRESS VARCHAR(200),
    @p_UpdatedBy BIGINT,
    @p_VM_LIVESTATUS CHAR(1) = 'A'
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @p_VM_ID IS NULL
        BEGIN
            -- Insert
            SELECT @p_VM_ID = ISNULL(MAX(VM_ID), 0) + 1 FROM VENDOR_MASTER;
            INSERT INTO VENDOR_MASTER
            (
                VM_ID, VM_CATID, VM_LOC_ID, VM_NAME, VM_EMAIL,
                VM_ADDRESS, VM_UPDATED_BY, VM_UPDATED_ON, VM_LIVESTATUS
            )
            VALUES
            (
                @p_VM_ID, @p_VM_CATID, @p_VM_LOC_ID, @p_VM_NAME, @p_VM_EMAIL,
                @p_VM_ADDRESS, @p_UpdatedBy, GETDATE(), @p_VM_LIVESTATUS
            );
        END
        ELSE
        BEGIN
            -- Update
            UPDATE VENDOR_MASTER
            SET VM_CATID = @p_VM_CATID,
                VM_LOC_ID = @p_VM_LOC_ID,
                VM_NAME = @p_VM_NAME,
                VM_EMAIL = @p_VM_EMAIL,
                VM_ADDRESS = @p_VM_ADDRESS,
                VM_UPDATED_BY = @p_UpdatedBy,
                VM_UPDATED_ON = GETDATE(),
                VM_LIVESTATUS = @p_VM_LIVESTATUS
            WHERE VM_ID = @p_VM_ID;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ==========================================
-- TRIGGERS
-- ==========================================

-- ------------------------------------------------------------------
-- Trigger: trg_ScholarshipDetail_UpdateAudit
-- Purpose:  Automatically update audit columns on any change to SCHOLARSHIP_DETAIL.
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_ScholarshipDetail_UpdateAudit
ON dbo.SCHOLARSHIP_DETAIL
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE SD
    SET SCHDET_UPDATEDON = GETDATE()
    FROM dbo.SCHOLARSHIP_DETAIL SD
    INNER JOIN inserted I ON SD.SCHDET_ID = I.SCHDET_ID;
    -- Note: SCHDET_UPDATEDBY should be set by application; trigger does not override it.
END;
GO

-- ------------------------------------------------------------------
-- Trigger: trg_StationeryRequestSub_StatusChange
-- Purpose:  When a request sub status changes to 'C' (completed), update the main request
--           if all subs are completed.
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_StationeryRequestSub_StatusChange
ON dbo.SP_REQUEST_SUB
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(RS_STATUS)
    BEGIN
        -- For each request main whose sub status changed, check if all subs are completed
        UPDATE RM
        SET -- Optionally set a flag or just log; here we do nothing but could update a summary.
            -- For example, we could set a last updated timestamp.
            RM.RM_REQUESTEDON = RM.RM_REQUESTEDON  -- dummy update to illustrate
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
                    AND RS.RS_STATUS NOT IN ('C', 'X')  -- Not completed or cancelled
              )
        );
    END
END;
GO

-- ------------------------------------------------------------------
-- Trigger: trg_StationeryMaster_ReorderAlert
-- Purpose:  When stock is updated, if it falls below reorder level,
--           insert a record into an alert table (create if needed).
-- ------------------------------------------------------------------
-- First, create an alert table if not exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'STATIONERY_REORDER_ALERT')
CREATE TABLE dbo.STATIONERY_REORDER_ALERT
(
    AlertID BIGINT IDENTITY(1,1) PRIMARY KEY,
    StationaryID BIGINT NOT NULL,
    AlertDate DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    CurrentStock BIGINT NOT NULL,
    ReorderLevel BIGINT NOT NULL,
    Resolved CHAR(1) DEFAULT 'N'
);
GO

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
          AND (D.SM_OPENINGSTOCK >= D.SM_REORDER_LEVEL OR D.SM_OPENINGSTOCK IS NULL); -- newly crossed threshold
    END
END;
GO

-- ------------------------------------------------------------------
-- Trigger: trg_SetUpdatedOn
-- Purpose:  Automatically set UPDATED_ON columns for tables that have them
--           but are not handled by application. This is a generic template.
--           We'll create one for a key table as example.
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_VendorMaster_UpdateAudit
ON dbo.VENDOR_MASTER
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE VM
    SET VM_UPDATED_ON = GETDATE()
    FROM dbo.VENDOR_MASTER VM
    INNER JOIN inserted I ON VM.VM_ID = I.VM_ID;
    -- Note: VM_UPDATED_BY must be set by application; trigger does not override.
END;
GO

-- ==========================================
-- EXAMPLE EXECUTION SCRIPTS
-- ==========================================

-- **FUNCTION: fn_GetScholarshipEligibleAmount**
-- Gets eligible scholarship amount for a grade, exam type, and year
DECLARE @ScholarshipAmount BIGINT;
SET @ScholarshipAmount = dbo.fn_GetScholarshipEligibleAmount('UG', '10', 2026);
PRINT 'Eligible Scholarship Amount for UG Grade, Exam 10, Year 2026: ' + CAST(@ScholarshipAmount AS VARCHAR);
GO

-- **FUNCTION: fn_GetDeptRemainingBudget**
-- Calculates remaining budget for a department in a financial year
DECLARE @RemainingBudget BIGINT;
SET @RemainingBudget = dbo.fn_GetDeptRemainingBudget(1, 5, 1);
PRINT 'Remaining Budget for Location 1, Dept 5, FY 1: ' + CAST(@RemainingBudget AS VARCHAR);
GO

-- **PROCEDURE: usp_ScholarshipApplication**
-- Insert a new scholarship application (if SCHOLARSHIP_MAIN and SCHOLARSHIP_DETAIL tables exist)
/*
DECLARE @NewSchID INT;
EXEC dbo.usp_ScholarshipApplication
    @p_SCH_EMPSYSID = 1001,
    @p_SCH_GRADEID = 1,
    @p_SCH_DEPENDID = 1,
    @p_SCH_CHILDNAME = 'John Doe',
    @p_SCH_LASTSCHOOL = 'ABC School',
    @p_SCH_LASTYEAROFSCHOOL = 2024,
    @p_SCH_LASTEXAM = 'S',
    @p_SCH_CGPAFLAG = 'Y',
    @p_SCH_MARKSPER = 85,
    @p_SCH_MARKSGPA = 3.8,
    @p_SCH_MARKSFILE = '/files/marks_2024.pdf',
    @p_SCH_COURSENAME = 'Engineering',
    @p_SCH_COURSEJOINYEAR = 2024,
    @p_SCH_COURSEJOINMONTH = 8,
    @p_SCH_COURSEDURATION = 4,
    @p_SCH_ADMRECPTFILE = NULL,
    @p_SCH_PAYMODE = 'TRF',
    @p_SCH_CHILDACCNO = '123456789',
    @p_SCH_CHILLDBANKIFSC = 'SBIN0001234',
    @p_SCH_CHILLDBANKMICR = '123456789',
    @p_SCH_SOURCE = 'E',
    @p_SCH_DISBAMOUNT = 50000,
    @p_SCH_DISBFREQ = 'Q',
    @p_CreatedBy = 100,
    @p_NewSchID = @NewSchID OUTPUT;
PRINT 'New Scholarship ID Created: ' + CAST(@NewSchID AS VARCHAR);
*/
GO

-- **PROCEDURE: usp_ScholarshipApprove**
-- Approve a scholarship application
/*
EXEC dbo.usp_ScholarshipApprove
    @p_SCH_ID = 1,
    @p_ApprovedBy = 200,
    @p_AppRemarks = 'Approved as per guidelines';
PRINT 'Scholarship Application Approved Successfully';
*/
GO

-- **PROCEDURE: usp_StationeryRequestSubmit**
-- Submit a new stationery request with multiple items
-- Note: Requires table type dbo.RequestItemType to be created first
/*
DECLARE @NewRequestID BIGINT;
DECLARE @RequestItems dbo.RequestItemType;

INSERT INTO @RequestItems VALUES 
    (10, 5, '2026-03-15', 100, 'A4 Paper needed'),
    (20, 5, '2026-03-20', 50, 'Pens required');

EXEC dbo.usp_StationeryRequestSubmit
    @p_RequestedBy = 1001,
    @p_LocationID = 1,
    @p_UnitCode = 'HR',
    @p_RequestItems = @RequestItems,
    @p_NewRequestID = @NewRequestID OUTPUT;
PRINT 'New Stationery Request ID: ' + CAST(@NewRequestID AS VARCHAR);
*/
GO

-- **PROCEDURE: usp_StationeryRequestApprove**
-- Approve a stationery request sub-item
/*
EXEC dbo.usp_StationeryRequestApprove
    @p_RequestSubID = 1,
    @p_ApprovedQty = 100,
    @p_ApproverSysID = 200,
    @p_Remarks = 'Approved for procurement';
PRINT 'Stationery Request Approved';
*/
GO

-- **PROCEDURE: usp_ReceiveOrder**
-- Record receipt of ordered items and update stock
/*
EXEC dbo.usp_ReceiveOrder
    @p_OrderSubID = 1,
    @p_ReceivedQty = 100,
    @p_ReceivedBy = 300,
    @p_ReceiptDate = '2026-03-18';
PRINT 'Order Receipt Recorded Successfully';
*/
GO

-- **PROCEDURE: usp_AddUpdateVendor**
-- Insert or update vendor master record
/*
DECLARE @VendorID BIGINT;
EXEC dbo.usp_AddUpdateVendor
    @p_VM_ID = NULL,  -- NULL for insert
    @p_VM_CATID = 1,
    @p_VM_LOC_ID = 1,
    @p_VM_NAME = 'ABC Supplies pvt ltd',
    @p_VM_EMAIL = 'vendor@abcsupplies.com',
    @p_VM_ADDRESS = '123 Business Street, City',
    @p_UpdatedBy = 100,
    @p_VM_LIVESTATUS = 'A';
PRINT 'Vendor Record Added/Updated Successfully';

-- Update existing vendor
EXEC dbo.usp_AddUpdateVendor
    @p_VM_ID = 1,  -- Existing vendor ID
    @p_VM_CATID = 1,
    @p_VM_LOC_ID = 1,
    @p_VM_NAME = 'ABC Supplies pvt ltd - Updated',
    @p_VM_EMAIL = 'newemail@abcsupplies.com',
    @p_VM_ADDRESS = '456 New Street, City',
    @p_UpdatedBy = 100,
    @p_VM_LIVESTATUS = 'A';
PRINT 'Vendor Record Updated Successfully';
*/
GO

-- **TRIGGER EXAMPLES**
-- The following triggers execute automatically:
--   1. trg_ScholarshipDetail_UpdateAudit 
--      - Auto-updates SCHDET_UPDATEDON on SCHOLARSHIP_DETAIL changes
--   2. trg_StationeryRequestSub_StatusChange 
--      - Auto-updates SP_REQUEST_MAIN when sub-item statuses change
--   3. trg_StationeryMaster_ReorderAlert 
--      - Auto-creates alert when stock falls below reorder level
--   4. trg_VendorMaster_UpdateAudit 
--      - Auto-updates VM_UPDATED_ON on VENDOR_MASTER changes

-- Example: Check reorder alerts
-- SELECT * FROM dbo.STATIONERY_REORDER_ALERT WHERE Resolved = 'N';
GO

-- **QUERY EXAMPLES**
-- Below are some useful query examples:

-- 1. Get all active scholarship applications
-- SELECT * FROM SCHOLARSHIP_MAIN WHERE SCH_LIVESTATUS = 'A' ORDER BY SCH_CREATEDON DESC;

-- 2. Get pending stationery requests
-- SELECT * FROM SP_REQUEST_SUB WHERE RS_STATUS = 'P' ORDER BY RS_EXPECTED_DATE;

-- 3. Get all vendors for a location
-- SELECT * FROM VENDOR_MASTER WHERE VM_LOC_ID = 1 AND VM_LIVESTATUS = 'A';

-- 4. Get financial years
-- SELECT * FROM FINYEAR_MASTER ORDER BY FY_STARTDATE DESC;

-- 5. Get LOV (List of Values) for a type
-- SELECT * FROM LOV_MASTER WHERE LOV_TYPE_ID = 1 ORDER BY LOV_NAME;

-- ==========================================
-- END OF SCRIPT
-- ==========================================