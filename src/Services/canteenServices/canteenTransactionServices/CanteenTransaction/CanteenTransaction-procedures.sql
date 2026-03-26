-- ==========================================
-- Module: CanteenTransaction
-- Stored Procedures and Functions
-- ==========================================

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
        DECLARE @ShiftCode CHAR(1) = 'M';

        IF dbo.fn_IsEmployeeEligibleForMeal(@p_EmpSysID, @p_ItemCode, @ShiftCode, @p_CanteenUnit) = 0
            THROW 50001, 'Employee not eligible for this meal/item', 1;

        SELECT TOP 1 @EmpShare = EmployeeShare, @ErShare = EmployerShare
        FROM dbo.fn_GetCanteenDeductionAmount(@p_EmpSysID, @p_ItemCode, @DateTaken);

        DECLARE @TransID BIGINT;
        SELECT @TransID = ISNULL(MAX(CN_SRL_NUM), 0) + 1 FROM CANTEEDN_DACON;

        INSERT INTO CANTEEDN_DACON
        (CN_SRL_NUM, CN_COM_COD, CN_SYS_ID, CN_EMP_TYP, CN_SWP_DAT,
         CN_ITM_COD, CN_ITM_TYP, CN_EE_CON, CN_ER_CON)
        VALUES
        (@TransID, @p_CanteenUnit, @p_EmpSysID, 'R', CAST(@DateTaken AS VARCHAR),
         @p_ItemCode, 'M', @EmpShare, @ErShare);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Canteen transaction failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ProcessDailyAvailed
-- Purpose:  Process daily availed items from DACON to DAYWISE_AVAILED
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ProcessDailyAvailed
(
    @p_ProcessDate DATE,
    @p_CanteenUnit BIGINT,
    @p_ProcessedBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO CANTEEN_DAYWISE_AVAILED
        (CN_SRL_NUM, CN_COM_COD, CN_SYS_ID, CN_EMP_TYP, CN_SWP_DAT,
         CN_ITM_COD, CN_ITM_TYP, CN_EE_CON, CN_ER_CON, CN_CAN_NUM,
         CN_ITM_QTY, CN_ENT_USR, CN_ENT_DAT, CN_FLEX1, CN_GRD_CAT)
        SELECT
            CN_SRL_NUM, CN_COM_COD, CN_SYS_ID, CN_EMP_TYP, CN_SWP_DAT,
            CN_ITM_COD, CN_ITM_TYP, CN_EE_CON, CN_ER_CON, CN_CAN_NUM,
            CN_ITM_QTY, @p_ProcessedBy, CAST(GETDATE() AS VARCHAR), CN_FLEX1, CN_GRD_CAT
        FROM CANTEEDN_DACON
        WHERE CN_COM_COD = @p_CanteenUnit
          AND CAST(CN_SWP_DAT AS DATE) = @p_ProcessDate;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Daily availed processing failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO
