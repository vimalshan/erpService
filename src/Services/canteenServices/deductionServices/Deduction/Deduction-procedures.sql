-- ==========================================
-- Module: Deduction
-- Stored Procedures and Functions
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_GetCanteenDeductionAmount
-- Purpose:  Calculate employee share and employer contribution
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetCanteenDeductionAmount
(
    @p_EmpSysID BIGINT,
    @p_ItemCode BIGINT,
    @p_DateTaken DATETIME2(3)
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        ISNULL(CN_EMP_CON, 0) AS [EmployeeShare],
        ISNULL(CN_EPR_CON, 0) AS [EmployerShare]
    FROM CANTEEN_ITEM_PRICE_MASTER
    WHERE CN_ITM_COD = @p_ItemCode
      AND CN_EFF_DAT <= @p_DateTaken
      AND (CN_CLS_DAT IS NULL OR CN_CLS_DAT >= @p_DateTaken)
);
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ProcessMonthlyCanteenDeduction
-- Purpose:  Calculate and process monthly canteen deductions for payroll
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ProcessMonthlyCanteenDeduction
(
    @p_MonthYear VARCHAR(7),  -- YYYY-MM
    @p_ProcessedBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @EmpSysID BIGINT;
        DECLARE @TotalDeduction BIGINT;
        DECLARE cur_emp CURSOR FOR
            SELECT DISTINCT CN_SYS_ID FROM CANTEEDN_DACON
            WHERE SUBSTRING(CN_SWP_DAT, 1, 7) = @p_MonthYear;
        
        OPEN cur_emp;
        FETCH NEXT FROM cur_emp INTO @EmpSysID;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Calculate total deduction for employee
            SELECT @TotalDeduction = ISNULL(SUM(CN_EE_CON), 0)
            FROM CANTEEDN_DACON
            WHERE CN_SYS_ID = @EmpSysID
              AND SUBSTRING(CN_SWP_DAT, 1, 7) = @p_MonthYear;
            
            IF @TotalDeduction > 0
            BEGIN
                -- Record in payroll
                INSERT INTO ADHOC_PAY_DED
                (
                    PY_SYS_ID, PY_CAN_UNT, PY_BAT_NUM, PY_TRN_DAT,
                    PY_ED_COD, PY_PAY_AMT, PY_ENT_DAT, PY_ENT_USR
                )
                VALUES
                (
                    @EmpSysID, 1, 0, GETDATE(),
                    'CANTEN', @TotalDeduction, GETDATE(), @p_ProcessedBy
                );
            END
            
            FETCH NEXT FROM cur_emp INTO @EmpSysID;
        END
        
        CLOSE cur_emp;
        DEALLOCATE cur_emp;
        
        COMMIT TRANSACTION;
        PRINT 'Monthly canteen deduction processed for: ' + @p_MonthYear;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Canteen deduction processing failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO
