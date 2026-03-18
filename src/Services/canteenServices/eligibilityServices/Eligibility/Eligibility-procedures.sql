-- ==========================================
-- Module: Eligibility
-- Stored Procedures and Functions
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
