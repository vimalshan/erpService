-- ==========================================
-- Database: AIMSDB
-- Stored Procedures, Functions, Triggers
-- Attendance & Leave Management System
-- ==========================================

USE [AIMSDB];
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_GetEmployeeAttendancePercentage
-- Purpose:  Calculate attendance percentage for an employee in a month
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetEmployeeAttendancePercentage
(
    @p_EmpSysID BIGINT,
    @p_MonthStart DATETIME2(3),
    @p_MonthEnd DATETIME2(3)
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @TotalWorkingDays INT;
    DECLARE @PresentDays INT;
    DECLARE @Percentage DECIMAL(5,2);
    
    -- Get total working days (excluding weekends/holidays)
    SELECT @TotalWorkingDays = COUNT(*)
    FROM ATTENDANCE_BATCHCALENDAR
    WHERE BATCH_CALDATE BETWEEN @p_MonthStart AND @p_MonthEnd
      AND BATCH_CALDAY NOT IN ('SA', 'SU');  -- Exclude Saturday, Sunday
    
    -- Get present days (swipes recorded)
    SELECT @PresentDays = COUNT(DISTINCT CAST(SWIPE_PUNCHTIME AS DATE))
    FROM SWIPE_RAWPUNCH
    WHERE SWIPE_EMPSYSID = @p_EmpSysID
      AND SWIPE_PUNCHTIME BETWEEN @p_MonthStart AND @p_MonthEnd;
    
    -- Calculate percentage
    IF @TotalWorkingDays > 0
        SET @Percentage = (@PresentDays * 100.0) / @TotalWorkingDays;
    ELSE
        SET @Percentage = 0;
    
    RETURN ISNULL(@Percentage, 0);
END;
GO

-- ------------------------------------------------------------------
-- Function: fn_GetLOPDays
-- Purpose:  Calculate Loss of Pay days for an employee in a period
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetLOPDays
(
    @p_EmpSysID BIGINT,
    @p_MonthStart DATETIME2(3),
    @p_MonthEnd DATETIME2(3)
)
RETURNS INT
AS
BEGIN
    DECLARE @LOPDays INT;
    DECLARE @WorkingDays INT;
    DECLARE @PresentDays INT;
    
    -- Calculate working days
    SELECT @WorkingDays = COUNT(*)
    FROM ATTENDANCE_BATCHCALENDAR
    WHERE BATCH_CALDATE BETWEEN @p_MonthStart AND @p_MonthEnd
      AND BATCH_CALDAY NOT IN ('SA', 'SU', 'HO');
    
    -- Get present days
    SELECT @PresentDays = COUNT(DISTINCT CAST(SWIPE_PUNCHTIME AS DATE))
    FROM SWIPE_RAWPUNCH
    WHERE SWIPE_EMPSYSID = @p_EmpSysID
      AND SWIPE_PUNCHTIME BETWEEN @p_MonthStart AND @p_MonthEnd;
    
    -- LOP = Working days - Present days (excluding leaves)
    SET @LOPDays = @WorkingDays - @PresentDays;
    
    RETURN ISNULL(@LOPDays, 0);
END;
GO

-- ------------------------------------------------------------------
-- Function: fn_GetLeaveBalance
-- Purpose:  Get remaining leave balance for an employee
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetLeaveBalance
(
    @p_EmpSysID BIGINT,
    @p_LeaveID BIGINT,
    @p_AsOnDate DATETIME2(3)
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @Entitled DECIMAL(5,2);
    DECLARE @Taken DECIMAL(5,2);
    DECLARE @Balance DECIMAL(5,2);
    
    -- Get leave entitlement
    SELECT TOP 1 @Entitled = LEAVE_DAYS
    FROM LEAVE_MASTER
    WHERE LEAVE_ID = @p_LeaveID;
    
    -- Get leave taken
    SELECT @Taken = ISNULL(SUM(LEAVE_DAYS), 0)
    FROM LEAVE_DETAILS
    WHERE LD_EMPSYSID = @p_EmpSysID
      AND LD_LEAVE_ID = @p_LeaveID
      AND LD_FROMDATE <= @p_AsOnDate
      AND LD_STATUS = 'A';  -- Approved
    
    SET @Balance = ISNULL(@Entitled, 0) - ISNULL(@Taken, 0);
    
    RETURN ISNULL(@Balance, 0);
END;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_RecordSwipePunch
-- Purpose:  Record biometric punch data with validation
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RecordSwipePunch
(
    @p_EmpSysID BIGINT,
    @p_GateNo INT,
    @p_PunchTime DATETIME2(3),
    @p_PunchStatus CHAR(1),  -- I = In, O = Out
    @p_MachineNo INT = NULL,
    @p_ReferenceNo VARCHAR(30) = NULL,
    @p_SwipeID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Validate employee
        -- Validate punch status
        IF @p_PunchStatus NOT IN ('I', 'O')
            THROW 50001, 'Invalid punch status. Use I for In, O for Out', 1;
        
        -- Generate Swipe ID
        SELECT @p_SwipeID = ISNULL(MAX(SWIPE_ID), 0) + 1 FROM SWIPE_RAWPUNCH;
        
        -- Insert punch record
        INSERT INTO SWIPE_RAWPUNCH
        (
            SWIPE_ID, SWIPE_EMPSYSID, SWIPE_GATENO, SWIPE_PUNCHTIME,
            SWIPE_PUNCHSTATUS, SWIPE_MACHINENO, SWIPE_REFERENCENO,
            SWIPE_UPDATEDBY, SWIPE_UPDATEDON, SWIPE_PULLSTATUS
        )
        VALUES
        (
            @p_SwipeID, @p_EmpSysID, @p_GateNo, @p_PunchTime,
            @p_PunchStatus, @p_MachineNo, @p_ReferenceNo,
            1, GETDATE(), 'A'  -- Auto pull (A = Automatic)
        );
        
        PRINT 'Swipe punch recorded: ID = ' + CAST(@p_SwipeID AS VARCHAR);
    END TRY
    BEGIN CATCH
        RAISERROR('Swipe recording failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ProcessMonthlyAttendance
-- Purpose:  Process attendance for a month and calculate LOPs
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ProcessMonthlyAttendance
(
    @p_MonthStart DATETIME2(3),
    @p_MonthEnd DATETIME2(3),
    @p_ProcessedBy BIGINT,
    @p_BatchID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Create attendance batch
        SELECT @p_BatchID = ISNULL(MAX(BATCH_ID), 0) + 1 FROM ATTENDANCE_BATCH;
        
        INSERT INTO ATTENDANCE_BATCH
        (BATCH_ID, BATCH_MONTHSTART, BATCH_MONTHEND, BATCH_STATUS, BATCH_CREATEDBY, BATCH_CREATEDON)
        VALUES (@p_BatchID, @p_MonthStart, @p_MonthEnd, 'N', @p_ProcessedBy, GETDATE());
        
        -- Process each employee
        DECLARE @EmpSysID BIGINT;
        DECLARE @LOPDays INT;
        DECLARE cur_employees CURSOR FOR
            SELECT DISTINCT SWIPE_EMPSYSID
            FROM SWIPE_RAWPUNCH
            WHERE SWIPE_PUNCHTIME BETWEEN @p_MonthStart AND @p_MonthEnd;
        
        OPEN cur_employees;
        FETCH NEXT FROM cur_employees INTO @EmpSysID;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Calculate LOP
            SET @LOPDays = dbo.fn_GetLOPDays(@EmpSysID, @p_MonthStart, @p_MonthEnd);
            
            -- Create LOP main record if LOP > 0
            IF @LOPDays > 0
            BEGIN
                INSERT INTO ATTENDANCE_LOPMAIN
                (
                    LOP_EMPSYSID, LOP_MONTHSTART, LOP_MONTHEND, 
                    LOP_CALDAYS, LOP_CREATEDBY, LOP_CREATEDON
                )
                VALUES
                (
                    @EmpSysID, @p_MonthStart, @p_MonthEnd, 
                    @LOPDays, @p_ProcessedBy, GETDATE()
                );
            END
            
            FETCH NEXT FROM cur_employees INTO @EmpSysID;
        END
        
        CLOSE cur_employees;
        DEALLOCATE cur_employees;
        
        -- Update batch status
        UPDATE ATTENDANCE_BATCH
        SET BATCH_STATUS = 'Y'
        WHERE BATCH_ID = @p_BatchID;
        
        COMMIT TRANSACTION;
        PRINT 'Attendance processing completed for batch: ' + CAST(@p_BatchID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Attendance processing failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ApplyLeave
-- Purpose:  Submit leave application with validation
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ApplyLeave
(
    @p_EmpSysID BIGINT,
    @p_LeaveID BIGINT,
    @p_FromDate DATETIME2(3),
    @p_ToDate DATETIME2(3),
    @p_Reason VARCHAR(500),
    @p_LeaveDays DECIMAL(5,2),
    @p_AppliedBy BIGINT,
    @p_LeaveDetailID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate leave balance
        DECLARE @AvailableBalance DECIMAL(5,2);
        SET @AvailableBalance = dbo.fn_GetLeaveBalance(@p_EmpSysID, @p_LeaveID, @p_FromDate);
        
        IF @AvailableBalance < @p_LeaveDays
            THROW 50002, 'Insufficient leave balance. Available: ' + CAST(@AvailableBalance AS VARCHAR), 1;
        
        -- Validate dates
        IF @p_FromDate > @p_ToDate
            THROW 50003, 'From date cannot be after To date', 1;
        
        -- Generate leave detail ID
        SELECT @p_LeaveDetailID = ISNULL(MAX(LD_ID), 0) + 1 FROM LEAVE_DETAILS;
        
        -- Create leave application
        INSERT INTO LEAVE_DETAILS
        (
            LD_ID, LD_EMPSYSID, LD_LEAVE_ID, LD_FROMDATE, LD_TODATE,
            LD_DAYS, LD_REASON, LD_STATUS, LD_APPLIEDON, LD_APPLIEDBY
        )
        VALUES
        (
            @p_LeaveDetailID, @p_EmpSysID, @p_LeaveID, @p_FromDate, @p_ToDate,
            @p_LeaveDays, @p_Reason, 'P', GETDATE(), @p_AppliedBy  -- P = Pending
        );
        
        COMMIT TRANSACTION;
        PRINT 'Leave application submitted: ID = ' + CAST(@p_LeaveDetailID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Leave application failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ApproveLeave
-- Purpose:  Approve or reject leave application
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ApproveLeave
(
    @p_LeaveDetailID BIGINT,
    @p_Status CHAR(1),  -- A = Approved, R = Rejected
    @p_Remarks VARCHAR(500) = NULL,
    @p_ApprovedBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF @p_Status NOT IN ('A', 'R')
            THROW 50001, 'Invalid status. Use A for Approved, R for Rejected', 1;
        
        -- Update leave status
        UPDATE LEAVE_DETAILS
        SET LD_STATUS = @p_Status,
            LD_APPOVEDON = GETDATE(),
            LD_APPROVEDBY = @p_ApprovedBy,
            LD_REMARKS = @p_Remarks
        WHERE LD_ID = @p_LeaveDetailID;
        
        -- If approved, create leave details approval record
        IF @p_Status = 'A'
        BEGIN
            DECLARE @LeaveID BIGINT;
            SELECT @LeaveID = ISNULL(MAX(LDA_ID), 0) + 1 FROM LEAVE_DETAILSAPR;
            
            INSERT INTO LEAVE_DETAILSAPR
            (LDA_ID, LDA_LEAVE_DETAILID, LDA_APPROVEDBY, LDA_APPROVEDON)
            SELECT @LeaveID, @p_LeaveDetailID, @p_ApprovedBy, GETDATE();
        END
        
        COMMIT TRANSACTION;
        PRINT 'Leave ' + CASE @p_Status WHEN 'A' THEN 'approved' ELSE 'rejected' END + ' for detail ID: ' + CAST(@p_LeaveDetailID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Leave approval failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_RequestCompOff
-- Purpose:  Request compensation off after completing extra hours
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RequestCompOff
(
    @p_EmpSysID BIGINT,
    @p_CompMainID BIGINT,
    @p_RequestedHours DECIMAL(5,2),
    @p_RequestedBy BIGINT,
    @p_CompRequestID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate extra hours worked
        DECLARE @ExtraHoursWorked DECIMAL(5,2);
        SELECT @ExtraHoursWorked = COMP_HOURS_WORKED
        FROM COMPOFF_MAIN
        WHERE COMP_MAINID = @p_CompMainID
          AND COMP_EMPSYSID = @p_EmpSysID;
        
        IF @ExtraHoursWorked IS NULL OR @ExtraHoursWorked < @p_RequestedHours
            THROW 50001, 'Insufficient extra hours available', 1;
        
        -- Create comp-off request
        SELECT @p_CompRequestID = ISNULL(MAX(COMP_REQUESTID), 0) + 1 FROM COMPOFF_REQUEST;
        
        INSERT INTO COMPOFF_REQUEST
        (
            COMP_REQUESTID, COMP_EMPSYSID, COMP_MAINID, COMP_HOURSREQUESTED,
            COMP_STATUS, COMP_REQUESTEDON, COMP_REQUESTEDBY
        )
        VALUES
        (
            @p_CompRequestID, @p_EmpSysID, @p_CompMainID, @p_RequestedHours,
            'P', GETDATE(), @p_RequestedBy  -- P = Pending
        );
        
        COMMIT TRANSACTION;
        PRINT 'Comp-off request submitted: ID = ' + CAST(@p_CompRequestID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Comp-off request failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- TRIGGERS
-- ==========================================

-- ------------------------------------------------------------------
-- Trigger: trg_SwipeRawPunch_ValidateEntry
-- Purpose:  Validate swipe entry and update pull status
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_SwipeRawPunch_ValidateEntry
ON dbo.SWIPE_RAWPUNCH
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validate punch status
    UPDATE SR
    SET SWIPE_PULLSTATUS = CASE 
        WHEN I.SWIPE_PUNCHSTATUS IN ('I', 'O') THEN 'A'  -- Valid
        ELSE 'M'  -- Manual review needed
    END
    FROM SWIPE_RAWPUNCH SR
    INNER JOIN inserted I ON SR.SWIPE_ID = I.SWIPE_ID;
END;
GO

-- ------------------------------------------------------------------
-- Trigger: trg_LeaveDetails_CheckBalance
-- Purpose:  Prevent leave application if balance insufficient
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_LeaveDetails_CheckBalance
ON dbo.LEAVE_DETAILS
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Balance DECIMAL(5,2);
    DECLARE @EmpSysID BIGINT;
    DECLARE @LeaveID BIGINT;
    DECLARE @Days DECIMAL(5,2);
    
    SELECT TOP 1 
        @EmpSysID = LD_EMPSYSID,
        @LeaveID = LD_LEAVE_ID,
        @Days = LD_DAYS
    FROM inserted;
    
    SET @Balance = dbo.fn_GetLeaveBalance(@EmpSysID, @LeaveID, GETDATE());
    
    IF @Balance >= @Days
    BEGIN
        -- Proceed with insert
        INSERT INTO LEAVE_DETAILS
        SELECT * FROM inserted;
    END
    ELSE
    BEGIN
        RAISERROR('Insufficient leave balance for this leave type', 16, 1);
    END
END;
GO

-- ------------------------------------------------------------------
-- Trigger: trg_AttendanceBatch_AutoUpdateStatus
-- Purpose:  Mark batch as processed after all employees processed
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_AttendanceBatch_AutoUpdateStatus
ON dbo.ATTENDANCE_LOPMAIN
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get batch ID from inserted records
    DECLARE @BatchID BIGINT;
    SELECT TOP 1 @BatchID = BATCH_ID 
    FROM ATTENDANCE_BATCH 
    WHERE BATCH_MONTHSTART <= (SELECT MIN(LOP_MONTHSTART) FROM inserted);
    
    -- Check if all employees processed
    DECLARE @TotalEmps INT;
    DECLARE @ProcessedEmps INT;
    
    SELECT @TotalEmps = COUNT(DISTINCT SWIPE_EMPSYSID)
    FROM SWIPE_RAWPUNCH
    WHERE BATCH_ID = @BatchID;
    
    SELECT @ProcessedEmps = COUNT(DISTINCT LOP_EMPSYSID)
    FROM ATTENDANCE_LOPMAIN
    WHERE BATCH_ID = @BatchID;
    
    IF @TotalEmps = @ProcessedEmps
    BEGIN
        UPDATE ATTENDANCE_BATCH
        SET BATCH_STATUS = 'Y'  -- Complete
        WHERE BATCH_ID = @BatchID;
    END
END;
GO

-- ------------------------------------------------------------------
-- Trigger: trg_LeaveDetails_Audit
-- Purpose:  Track leave application changes in audit log
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_LeaveDetails_Audit
ON dbo.LEAVE_DETAILS
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO LEAVE_DETAILS_AUDIT
    (
        LD_ID, LD_EMPSYSID, LD_LEAVE_ID, LD_STATUS,
        LD_FROMDATE, LD_TODATE, AUDIT_ACTION, AUDIT_TIMESTAMP
    )
    SELECT 
        I.LD_ID, I.LD_EMPSYSID, I.LD_LEAVE_ID, I.LD_STATUS,
        I.LD_FROMDATE, I.LD_TODATE,
        CASE 
            WHEN NOT EXISTS (SELECT 1 FROM deleted D WHERE D.LD_ID = I.LD_ID) THEN 'INSERT'
            ELSE 'UPDATE'
        END,
        GETDATE()
    FROM inserted I;
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
