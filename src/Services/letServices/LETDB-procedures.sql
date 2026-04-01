-- LETDB Stored Procedures, Functions & Triggers
-- Leave Encashment & Loss of Pay Tracking System
-- Created: February 13, 2026

USE LETDB;
GO

IF OBJECT_ID('dbo.fn_CalculateLeaveEncashment', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateLeaveEncashment;
GO
CREATE FUNCTION dbo.fn_CalculateLeaveEncashment (@p_EmpSysID BIGINT, @p_LeaveType VARCHAR(20), @p_Days INT)
RETURNS DECIMAL(19,0)
AS BEGIN
    DECLARE @Amount DECIMAL(19,0) = 0, @DailyWage DECIMAL(19,0), @BasicSalary DECIMAL(19,0);
    BEGIN TRY
        SELECT TOP 1 @BasicSalary = PAY_BASIC FROM PAYDB.dbo.PAY_SALARY_MASTER 
        WHERE EMP_SYS_ID = @p_EmpSysID ORDER BY PAY_PERIOD_ENDING DESC;
        SET @DailyWage = CAST((ISNULL(@BasicSalary, 0) * 12) / 365 AS DECIMAL(19,0));
        SET @Amount = CAST(@DailyWage * @p_Days * 0.5 AS DECIMAL(19,0));
    END TRY BEGIN CATCH SET @Amount = 0; END CATCH
    RETURN @Amount;
END;
GO

IF OBJECT_ID('dbo.usp_ApplyLeaveEncashment', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_ApplyLeaveEncashment;
GO
CREATE PROCEDURE dbo.usp_ApplyLeaveEncashment
    @p_EmpSysID BIGINT, @p_LeaveType VARCHAR(20), @p_EncashmentDays INT, @p_RequestDate DATE, 
    @p_RequestedBy BIGINT, @p_EncashmentID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @Amount DECIMAL(19,0) = dbo.fn_CalculateLeaveEncashment(@p_EmpSysID, @p_LeaveType, @p_EncashmentDays);
        INSERT INTO dbo.LEAVE_ENCASHMENT (EMP_SYS_ID, LEAVE_TYPE, ENCASHMENT_DAYS, ENCASHMENT_AMOUNT, 
            REQUEST_DATE, ENCASHMENT_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_EmpSysID, @p_LeaveType, @p_EncashmentDays, @Amount, @p_RequestDate, 'P', @p_RequestedBy, GETDATE());
        SET @p_EncashmentID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_RecordLossOfPay', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_RecordLossOfPay;
GO
CREATE PROCEDURE dbo.usp_RecordLossOfPay
    @p_EmpSysID BIGINT, @p_LopDays INT, @p_LopMonth DATE, @p_Remarks VARCHAR(500), @p_RecordedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO dbo.LOSS_OF_PAY (EMP_SYS_ID, LOP_DAYS, LOP_MONTH, LOP_REMARKS, CREATED_BY, CREATED_ON)
        VALUES (@p_EmpSysID, @p_LopDays, @p_LopMonth, @p_Remarks, @p_RecordedBy, GETDATE());
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

PRINT 'LETDB Procedures created successfully.';
GO
