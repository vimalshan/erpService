-- ==========================================
-- Module: EXPENSE & SETTLEMENT
-- Description: Expense management procedures and settlement logic
-- ==========================================

USE [TRAVELDB];
GO

-- ------------------------------------------------------------------
-- Procedure: usp_RecordTravelExpense
-- Purpose: Record a travel expense for settlement
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RecordTravelExpense
(
    @p_RequestNum BIGINT,
    @p_ExpenseCode BIGINT,
    @p_BudgetAmount DECIMAL(19,0),
    @p_ActualAmount DECIMAL(19,0),
    @p_CompanyAmount DECIMAL(19,0),
    @p_SelfAmount DECIMAL(19,0),
    @p_ExpenseRemarks VARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Generate serial number
        DECLARE @SerialNum BIGINT;
        SELECT @SerialNum = ISNULL(MAX(TR_SRL_NUM), 0) + 1
        FROM TRAVEL_EXPENSE
        WHERE TR_REQ_NUM = @p_RequestNum;
        
        -- Validate amounts
        IF (@p_CompanyAmount + @p_SelfAmount) != @p_ActualAmount
            THROW 50001, 'Company and Self amounts do not equal actual amount', 1;
        
        -- Calculate variance
        DECLARE @VarianceAmount DECIMAL(19,0) = @p_ActualAmount - @p_BudgetAmount;
        
        -- Insert expense record
        INSERT INTO TRAVEL_EXPENSE
        (
            TR_REQ_NUM, TR_SRL_NUM, TR_EXP_COD, TR_BUD_AMT,
            TR_ELG_AMT, TR_ACT_SLF, TR_VAR_AMT, TR_EXP_REM
        )
        VALUES
        (
            @p_RequestNum, @SerialNum, @p_ExpenseCode, @p_BudgetAmount,
            @p_ActualAmount, @p_SelfAmount, @VarianceAmount, @p_ExpenseRemarks
        );
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Expense recorded successfully' AS [Message],
               @SerialNum AS [ExpenseSerialNum],
               @VarianceAmount AS [VarianceAmount];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [ExpenseSerialNum],
               NULL AS [VarianceAmount];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_CalculateDAAmount
-- Purpose: Calculate dearness allowance for travel period
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CalculateDAAmount
(
    @p_RequestNum BIGINT,
    @p_FromDate DATETIME2(3),
    @p_ToDate DATETIME2(3),
    @p_ArrangementType CHAR(1),  -- A=Admin, S=Self
    @p_GradeCode CHAR(3),
    @p_DAAmount DECIMAL(19,0) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @NoOfDays INT;
        DECLARE @DailyRate DECIMAL(19,0);
        
        -- Calculate number of days
        SET @NoOfDays = DATEDIFF(DAY, @p_FromDate, @p_ToDate) + 1;
        
        -- Get daily rate from rules
        SELECT TOP 1 @DailyRate = RL_BUD_AMT
        FROM RULE_DA
        WHERE RL_BND_COD = @p_GradeCode
          AND RL_ADM_SLF = @p_ArrangementType
          AND RL_EFF_DAT <= @p_FromDate
          AND (RL_CLS_DAT IS NULL OR RL_CLS_DAT >= @p_ToDate);
        
        -- If no rate found, throw error
        IF @DailyRate IS NULL
            THROW 50001, 'No DA rate found for specified criteria', 1;
        
        -- Calculate total DA amount
        SET @p_DAAmount = @NoOfDays * @DailyRate;
        
        -- Insert into DA_SUMMARY
        INSERT INTO DA_SUMMARY
        (
            DA_REQID, DA_ADMHRS, DA_ADMDYS, DA_ADMRAT, DA_ADMAMT,
            DA_SLFHRS, DA_SLFDYS, DA_SLFRAT, DA_SLFAMT
        )
        VALUES
        (
            @p_RequestNum,
            CASE WHEN @p_ArrangementType = 'A' THEN @NoOfDays * 24 ELSE 0 END,
            CASE WHEN @p_ArrangementType = 'A' THEN @NoOfDays ELSE 0 END,
            CASE WHEN @p_ArrangementType = 'A' THEN @DailyRate ELSE 0 END,
            CASE WHEN @p_ArrangementType = 'A' THEN @p_DAAmount ELSE 0 END,
            CASE WHEN @p_ArrangementType = 'S' THEN @NoOfDays * 24 ELSE 0 END,
            CASE WHEN @p_ArrangementType = 'S' THEN @NoOfDays ELSE 0 END,
            CASE WHEN @p_ArrangementType = 'S' THEN @DailyRate ELSE 0 END,
            CASE WHEN @p_ArrangementType = 'S' THEN @p_DAAmount ELSE 0 END
        );
        
        SELECT 'SUCCESS' AS [Result],
               @NoOfDays AS [NoOfDays],
               @DailyRate AS [DailyRate],
               @p_DAAmount AS [TotalDAAmount];
        
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_SettleExpenses
-- Purpose: Settle traveled expenses and generate settlement
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_SettleExpenses
(
    @p_RequestNum BIGINT,
    @p_SettlementAmount DECIMAL(19,0) OUTPUT,
    @p_RefundAmount DECIMAL(19,0) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @TotalBudget DECIMAL(19,0);
        DECLARE @TotalActual DECIMAL(19,0);
        
        -- Get budget and actual amounts
        SELECT @TotalBudget = TR_BUD_AMT FROM TRAVEL_MAIN WHERE TR_REQ_ID = @p_RequestNum;
        
        SELECT @TotalActual = ISNULL(SUM(TR_ELG_AMT), 0)
        FROM TRAVEL_EXPENSE
        WHERE TR_REQ_NUM = @p_RequestNum;
        
        -- Calculate settlement and refund
        SET @p_SettlementAmount = CASE 
            WHEN @TotalActual >= @TotalBudget THEN @TotalBudget
            ELSE @TotalActual
        END;
        
        SET @p_RefundAmount = CASE 
            WHEN @TotalActual < @TotalBudget THEN @TotalBudget - @TotalActual
            ELSE 0
        END;
        
        -- Update travel main status
        UPDATE TRAVEL_MAIN
        SET TR_PLS_FLG = 'S',  -- S = Settled
            TR_ACT_AMT = @TotalActual
        WHERE TR_REQ_ID = @p_RequestNum;
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               @p_SettlementAmount AS [SettlementAmount],
               @p_RefundAmount AS [RefundAmount];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_GetExpenseSummary
-- Purpose: Get expense summary for a travel request
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_GetExpenseSummary
(
    @p_RequestNum BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        tm.TR_PLN_NUM AS [TravelPlanNo],
        tm.TR_BUD_AMT AS [BudgetAmount],
        ISNULL(SUM(te.TR_ELG_AMT), 0) AS [TotalExpenses],
        ISNULL(SUM(te.TR_VAR_AMT), 0) AS [TotalVariance],
        ISNULL(SUM(te.TR_ACT_SLF), 0) AS [EmployeeShare],
        ISNULL(SUM(te.TR_ELG_AMT) - SUM(te.TR_ACT_SLF), 0) AS [CompanyShare],
        tm.TR_PLS_FLG AS [Status],
        ISNULL(ds.DA_ADMAMT, 0) + ISNULL(ds.DA_SLFAMT, 0) AS [TotalDAAmount]
    FROM TRAVEL_MAIN tm
    LEFT JOIN TRAVEL_EXPENSE te ON tm.TR_REQ_ID = te.TR_REQ_NUM
    LEFT JOIN DA_SUMMARY ds ON tm.TR_REQ_ID = ds.DA_REQID
    WHERE tm.TR_REQ_ID = @p_RequestNum
    GROUP BY tm.TR_PLN_NUM, tm.TR_BUD_AMT, tm.TR_PLS_FLG, ds.DA_ADMAMT, ds.DA_SLFAMT;
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
