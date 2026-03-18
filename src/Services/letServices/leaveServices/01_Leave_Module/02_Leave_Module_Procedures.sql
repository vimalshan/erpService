-- ==========================================
-- LEAVE MODULE - Stored Procedures & Functions
-- Database: LETDB
-- Purpose: Leave Management procedures
-- Created: March 9, 2026
-- ==========================================

USE LETDB;
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- Function: fn_CalculateLeaveEncashment
-- Purpose: Calculate encashment amount based on employee's basic salary
IF OBJECT_ID('dbo.fn_Leave_CalculateEncashment', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_Leave_CalculateEncashment;
GO
CREATE FUNCTION dbo.fn_Leave_CalculateEncashment (
    @p_EmpSysID BIGINT, 
    @p_LeaveType VARCHAR(20), 
    @p_Days INT
)
RETURNS DECIMAL(19,2)
AS BEGIN
    DECLARE @Amount DECIMAL(19,2) = 0, 
            @DailyWage DECIMAL(19,2), 
            @BasicSalary DECIMAL(19,2);
    
    BEGIN TRY
        -- Fetch basic salary from PAYDB if available
        SELECT TOP 1 @BasicSalary = PAY_BASIC 
        FROM PAYDB.dbo.PAY_SALARY_MASTER 
        WHERE EMP_SYS_ID = @p_EmpSysID 
        ORDER BY PAY_PERIOD_ENDING DESC;
        
        -- Calculate daily wage (Basic * 12 months / 365 days)
        SET @DailyWage = CAST((ISNULL(@BasicSalary, 0) * 12) / 365 AS DECIMAL(19,2));
        
        -- Calculate encashment as 50% of daily wage * days
        SET @Amount = CAST(@DailyWage * @p_Days * 0.5 AS DECIMAL(19,2));
    END TRY 
    BEGIN CATCH 
        SET @Amount = 0;
    END CATCH
    
    RETURN @Amount;
END;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- Procedure: usp_Leave_ApplyEncashment
-- Purpose: Record a leave encashment request
IF OBJECT_ID('dbo.usp_Leave_ApplyEncashment', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Leave_ApplyEncashment;
GO
CREATE PROCEDURE dbo.usp_Leave_ApplyEncashment
    @p_EmpSysID BIGINT, 
    @p_LeaveType VARCHAR(20), 
    @p_EncashmentDays INT, 
    @p_RequestDate DATE, 
    @p_RequestedBy BIGINT, 
    @p_EncashmentID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Calculate encashment amount
        DECLARE @Amount DECIMAL(19,2) = dbo.fn_Leave_CalculateEncashment(
            @p_EmpSysID, 
            @p_LeaveType, 
            @p_EncashmentDays
        );
        
        -- Insert encashment record
        INSERT INTO dbo.LEAVE_ENCASHMENT (
            EMP_SYS_ID, 
            LEAVE_TYPE, 
            ENCASHMENT_DAYS, 
            ENCASHMENT_AMOUNT, 
            REQUEST_DATE, 
            ENCASHMENT_STATUS, 
            CREATED_BY, 
            CREATED_ON
        )
        VALUES (
            @p_EmpSysID, 
            @p_LeaveType, 
            @p_EncashmentDays, 
            @Amount, 
            @p_RequestDate, 
            'P',  -- Pending status
            @p_RequestedBy, 
            GETDATE()
        );
        
        SET @p_EncashmentID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Leave_RecordLossOfPay
-- Purpose: Record loss of pay for an employee
IF OBJECT_ID('dbo.usp_Leave_RecordLossOfPay', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Leave_RecordLossOfPay;
GO
CREATE PROCEDURE dbo.usp_Leave_RecordLossOfPay
    @p_EmpSysID BIGINT, 
    @p_LopDays INT, 
    @p_LopMonth DATE, 
    @p_Remarks VARCHAR(500), 
    @p_RecordedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.LOSS_OF_PAY (
            EMP_SYS_ID, 
            LOP_DAYS, 
            LOP_MONTH, 
            LOP_REMARKS, 
            CREATED_BY, 
            CREATED_ON
        )
        VALUES (
            @p_EmpSysID, 
            @p_LopDays, 
            @p_LopMonth, 
            @p_Remarks, 
            @p_RecordedBy, 
            GETDATE()
        );
        
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Leave_CreateLeaveRequest
-- Purpose: Create a new leave request
IF OBJECT_ID('dbo.usp_Leave_CreateLeaveRequest', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Leave_CreateLeaveRequest;
GO
CREATE PROCEDURE dbo.usp_Leave_CreateLeaveRequest
    @p_ReqNum BIGINT,
    @p_FinyearSrlno INT,
    @p_EmpUserid VARCHAR(25),
    @p_SupUserid VARCHAR(25),
    @p_ReqDate DATETIME2(3)
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO dbo.LET_MAIN (
            REQ_NUM,
            FINYEAR_SRLNO,
            EMP_USERID,
            SUP_USERID,
            REQ_DATE
        )
        VALUES (
            @p_ReqNum,
            @p_FinyearSrlno,
            @p_EmpUserid,
            @p_SupUserid,
            @p_ReqDate
        );
        
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Leave_UpdateEncashmentStatus
-- Purpose: Update the status of an encashment request
IF OBJECT_ID('dbo.usp_Leave_UpdateEncashmentStatus', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Leave_UpdateEncashmentStatus;
GO
CREATE PROCEDURE dbo.usp_Leave_UpdateEncashmentStatus
    @p_EncashmentID BIGINT,
    @p_Status CHAR(1),
    @p_ModifiedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE dbo.LEAVE_ENCASHMENT
        SET ENCASHMENT_STATUS = @p_Status,
            MODIFIED_ON = GETDATE(),
            MODIFIED_BY = @p_ModifiedBy
        WHERE ENCASHMENT_ID = @p_EncashmentID;
        
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_Leave_GetEncashmentDetails
-- Purpose: Retrieve encashment details for an employee
IF OBJECT_ID('dbo.usp_Leave_GetEncashmentDetails', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Leave_GetEncashmentDetails;
GO
CREATE PROCEDURE dbo.usp_Leave_GetEncashmentDetails
    @p_EmpSysID BIGINT,
    @p_Status CHAR(1) = NULL
AS BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ENCASHMENT_ID,
        EMP_SYS_ID,
        LEAVE_TYPE,
        ENCASHMENT_DAYS,
        ENCASHMENT_AMOUNT,
        REQUEST_DATE,
        ENCASHMENT_STATUS,
        CREATED_ON
    FROM dbo.LEAVE_ENCASHMENT
    WHERE EMP_SYS_ID = @p_EmpSysID
        AND (@p_Status IS NULL OR ENCASHMENT_STATUS = @p_Status)
    ORDER BY REQUEST_DATE DESC;
END;
GO

PRINT 'Leave Module Procedures created successfully.';
GO
