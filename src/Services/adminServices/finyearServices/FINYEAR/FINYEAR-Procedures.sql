-- ==========================================
-- MODULE: FINYEAR
-- Component: Procedures
-- Description: Financial year management procedures
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- Procedure: usp_GetCurrentFinancialYear
-- Purpose: Retrieve the current active financial year
CREATE OR ALTER PROCEDURE dbo.usp_GetCurrentFinancialYear
(
    @p_FinYearID BIGINT OUTPUT,
    @p_FinYearName VARCHAR(27) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT TOP 1
            @p_FinYearID = FY_ID,
            @p_FinYearName = FY_NAME
        FROM FINYEAR_MASTER
        WHERE GETDATE() BETWEEN FY_STARTDATE AND FY_CLOSEDATE;

        IF @p_FinYearID IS NULL
        BEGIN
            RAISERROR('No active financial year found.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedure: usp_AddFinancialYear
-- Purpose: Add a new financial year
CREATE OR ALTER PROCEDURE dbo.usp_AddFinancialYear
(
    @p_FY_ID BIGINT,
    @p_FY_NAME VARCHAR(27),
    @p_FY_STARTDATE DATETIME2(3),
    @p_FY_CLOSEDATE DATETIME2(3),
    @p_UpdatedBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS (SELECT 1 FROM FINYEAR_MASTER WHERE FY_ID = @p_FY_ID)
        BEGIN
            RAISERROR('Financial Year with ID %d already exists.', 16, 1, @p_FY_ID);
            RETURN;
        END

        INSERT INTO FINYEAR_MASTER
        (
            FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON
        )
        VALUES
        (
            @p_FY_ID, @p_FY_NAME, @p_FY_STARTDATE, @p_FY_CLOSEDATE, @p_UpdatedBy, GETDATE()
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ==========================================
-- END OF FINYEAR PROCEDURES
-- ==========================================
