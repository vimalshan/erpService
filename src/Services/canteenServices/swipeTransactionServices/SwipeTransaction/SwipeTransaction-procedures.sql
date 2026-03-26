-- ==========================================
-- Module: SwipeTransaction
-- Stored Procedures and Functions
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
        DECLARE @ErrMsg1 NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Canteen punch recording failed: %s', 16, 1, @ErrMsg1);
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
        DECLARE @ErrMsg2 NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Canteen transaction failed: %s', 16, 1, @ErrMsg2);
    END CATCH
END;
GO

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
    IF NOT EXISTS (SELECT 1 FROM ItemMasterDb.dbo.CANTEEN_ITEM_MASTER WHERE CN_ITM_COD = @ItemCode)
    BEGIN
        DECLARE @ItemStr VARCHAR(20) = CAST(@ItemCode AS VARCHAR(20));
        RAISERROR('Invalid canteen item code: %s', 16, 1, @ItemStr);
        RETURN;
    END
    
    -- Proceed with insert
    INSERT INTO CANTEEDN_DACON
    SELECT * FROM inserted;
END;
GO
