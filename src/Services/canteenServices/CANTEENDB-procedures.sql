-- ==========================================
-- Database: CANTEENDB
-- Stored Procedures, Functions, Triggers
-- Canteen Management System
-- ==========================================

USE [CANTEENDB];
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_IsEmployeeEligibleForMeal
-- Purpose:  Check meal eligibility based on shift and item
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_IsEmployeeEligibleForMeal
(
    @p_EmpSysID BIGINT,
    @p_ItemCode BIGINT,
    @p_ShiftCode CHAR(1),
    @p_CanteenUnit BIGINT
)
RETURNS BIT
AS
BEGIN
    DECLARE @IsEligible BIT = 0;
    DECLARE @EligibleLimit INT;
    
    -- Get eligibility limit
    SELECT TOP 1 @EligibleLimit = CN_ELG_LMT
    FROM CAN_ELIGIBILITY_MASTER
    WHERE CN_COM_COD = @p_CanteenUnit
      AND CN_SFT_COD = @p_ShiftCode
      AND CN_ITM_COD = @p_ItemCode;
    
    -- Employee is eligible if limit exists (limit > 0)
    IF @EligibleLimit IS NOT NULL AND @EligibleLimit > 0
        SET @IsEligible = 1;
    
    RETURN @IsEligible;
END;
GO

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
        CAST(ISNULL(PR_ITEM_PRICE, 0) * 0.5 AS BIGINT) AS [EmployeeShare],
        CAST(ISNULL(PR_ITEM_PRICE, 0) * 0.5 AS BIGINT) AS [EmployerShare]
    FROM CANTEEN_PRICE_MASTER
    WHERE PR_ITEM_CODE = @p_ItemCode
      AND PR_EFFECTIVE_DATE <= @p_DateTaken
      AND (PR_CLOSURE_DATE IS NULL OR PR_CLOSURE_DATE >= @p_DateTaken)
);
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_RecordCanteenPunch
-- Purpose:  Record employee canteen punch (in/out)
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RecordCanteenPunch
(
    @p_EmpSysID BIGINT,
    @p_CanteenUnit BIGINT,
    @p_PunchType CHAR(1),  -- I = Check-in, O = Check-out
    @p_PunchTime DATETIME2(3) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @PunchDateTime DATETIME2(3) = ISNULL(@p_PunchTime, GETDATE());
        DECLARE @PunchDate DATE = CAST(@PunchDateTime AS DATE);
        DECLARE @TimeIn TIME;
        DECLARE @TimeOut TIME;
        DECLARE @WorkHours DECIMAL(5,2);
        
        -- Check if record exists for the day
        IF NOT EXISTS (SELECT 1 FROM CAN_DAYWISE_EMP_PUNCH 
                       WHERE CN_SYSID = @p_EmpSysID 
                       AND CN_PUN_DAT = @PunchDate)
        BEGIN
            -- Create new record
            DECLARE @SerialNum BIGINT;
            SELECT @SerialNum = ISNULL(MAX(CN_SRL_NUM), 0) + 1 FROM CAN_DAYWISE_EMP_PUNCH;
            
            INSERT INTO CAN_DAYWISE_EMP_PUNCH
            (CN_SRL_NUM, CN_COM_COD, CN_SYSID, CN_CAN_NUM, CN_PUN_DAT, CN_TIM_IN, CN_TIM_OUT)
            VALUES
            (
                @SerialNum, @p_CanteenUnit, @p_EmpSysID, @p_CanteenUnit, @PunchDate,
                CASE WHEN @p_PunchType = 'I' THEN CAST(@PunchDateTime AS TIME) ELSE NULL END,
                CASE WHEN @p_PunchType = 'O' THEN CAST(@PunchDateTime AS TIME) ELSE NULL END
            );
        END
        ELSE
        BEGIN
            -- Update existing record
            IF @p_PunchType = 'I'
            BEGIN
                UPDATE CAN_DAYWISE_EMP_PUNCH
                SET CN_TIM_IN = CAST(@PunchDateTime AS TIME)
                WHERE CN_SYSID = @p_EmpSysID AND CN_PUN_DAT = @PunchDate;
            END
            ELSE IF @p_PunchType = 'O'
            BEGIN
                -- Get check-in time
                SELECT @TimeIn = CN_TIM_IN FROM CAN_DAYWISE_EMP_PUNCH
                WHERE CN_SYSID = @p_EmpSysID AND CN_PUN_DAT = @PunchDate;
                
                -- Calculate work hours
                SET @TimeOut = CAST(@PunchDateTime AS TIME);
                SET @WorkHours = CAST(DATEDIFF(HOUR, @TimeIn, @TimeOut) AS DECIMAL(5,2)) +
                                 CAST(DATEDIFF(MINUTE, @TimeIn, @TimeOut) % 60 AS DECIMAL(5,2)) / 60;
                
                UPDATE CAN_DAYWISE_EMP_PUNCH
                SET CN_TIM_OUT = @TimeOut,
                    CN_WRK_HRS = @WorkHours
                WHERE CN_SYSID = @p_EmpSysID AND CN_PUN_DAT = @PunchDate;
            END
        END
        
        COMMIT TRANSACTION;
        PRINT 'Canteen punch recorded: ' + CASE @p_PunchType WHEN 'I' THEN 'Check-In' ELSE 'Check-Out' END;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Canteen punch recording failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_RecordCanteenTransaction
-- Purpose:  Record employee meal/item taken from canteen
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RecordCanteenTransaction
(
    @p_EmpSysID BIGINT,
    @p_ItemCode BIGINT,
    @p_CanteenUnit BIGINT,
    @p_DateTaken DATETIME2(3) = NULL,
    @p_RecordedBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @DateTaken DATETIME2(3) = ISNULL(@p_DateTaken, GETDATE());
        DECLARE @EmpShare BIGINT;
        DECLARE @ErShare BIGINT;
        DECLARE @ShiftCode CHAR(1) = 'M';  -- Default morning shift
        
        -- Check eligibility
        IF dbo.fn_IsEmployeeEligibleForMeal(@p_EmpSysID, @p_ItemCode, @ShiftCode, @p_CanteenUnit) = 0
            THROW 50001, 'Employee not eligible for this meal/item', 1;
        
        -- Get deduction amounts
        SELECT TOP 1 @EmpShare = EmployeeShare, @ErShare = EmployerShare
        FROM dbo.fn_GetCanteenDeductionAmount(@p_EmpSysID, @p_ItemCode, @DateTaken);
        
        -- Record transaction
        DECLARE @TransID BIGINT;
        SELECT @TransID = ISNULL(MAX(CN_SRL_NUM), 0) + 1 FROM CANTEEDN_DACON;
        
        INSERT INTO CANTEEDN_DACON
        (
            CN_SRL_NUM, CN_COM_COD, CN_SYS_ID, CN_EMP_TYP, CN_SWP_DAT,
            CN_ITM_COD, CN_ITM_TYP, CN_EE_CON, CN_ER_CON
        )
        VALUES
        (
            @TransID, @p_CanteenUnit, @p_EmpSysID, 'R', CAST(@DateTaken AS VARCHAR),
            @p_ItemCode, 'M', @EmpShare, @ErShare
        );
        
        COMMIT TRANSACTION;
        PRINT 'Canteen transaction recorded: Employee Share = ₹' + CAST(@EmpShare AS VARCHAR) +
              ', Employer Share = ₹' + CAST(@ErShare AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Canteen transaction failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
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

-- ==========================================
-- TRIGGERS
-- ==========================================

-- ------------------------------------------------------------------
-- Trigger: trg_CanteenTransaction_ValidateItem
-- Purpose:  Validate item exists before transaction
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_CanteenTransaction_ValidateItem
ON dbo.CANTEEDN_DACON
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ItemCode BIGINT;
    SELECT TOP 1 @ItemCode = CN_ITM_COD FROM inserted;
    
    -- Validate item exists
    IF NOT EXISTS (SELECT 1 FROM CANTEEN_ITEM_MASTER WHERE ITEM_CODE = @ItemCode)
    BEGIN
        RAISERROR('Invalid canteen item code: %s', 16, 1, CAST(@ItemCode AS VARCHAR));
        RETURN;
    END
    
    -- Proceed with insert
    INSERT INTO CANTEEDN_DACON
    SELECT * FROM inserted;
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
