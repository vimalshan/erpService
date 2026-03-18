-- ==========================================
-- Module: ADMINISTRATION & CONFIGURATION
-- Description: Admin procedures and configuration management
-- Procedures for admin unit and access management
-- ==========================================

USE [TRAVELDB];
GO

-- ------------------------------------------------------------------
-- Procedure: usp_CreateAdminUnit
-- Purpose: Create a new admin travel unit
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CreateAdminUnit
(
    @p_AdminCode BIGINT,
    @p_AdminName VARCHAR(50),
    @p_AdminType VARCHAR(1),  -- T=Travel, S=Stay, M=Meeting
    @p_UnitCode CHAR(3) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Insert admin unit
        INSERT INTO TRAVEL_ADMIN_UNITS
        (AD_ADM_COD, AD_ADM_NAM, AD_ADM_TYP, AD_ADM_UNT)
        VALUES
        (@p_AdminCode, @p_AdminName, @p_AdminType, @p_UnitCode);
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Admin unit created successfully' AS [Message],
               @p_AdminCode AS [AdminCode];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [AdminCode];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_CreateFinanceUnit
-- Purpose: Create a finance processing unit
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CreateFinanceUnit
(
    @p_UnitID BIGINT,
    @p_UnitCode CHAR(3),
    @p_UnitName VARCHAR(50),
    @p_OracleCode BIGINT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Insert finance unit
        INSERT INTO TRAVEL_FINANCE_UNITS
        (TR_UNT_ID, TR_UNT_COD, TR_UNT_NAM, TR_ORA_COD, TR_LOC_OPTION)
        VALUES
        (@p_UnitID, @p_UnitCode, @p_UnitName, @p_OracleCode, 'N');
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Finance unit created successfully' AS [Message],
               @p_UnitID AS [UnitID];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [UnitID];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_GetAdminUnitDetails
-- Purpose: Retrieve admin unit details
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_GetAdminUnitDetails
(
    @p_AdminCode BIGINT = NULL,
    @p_AdminType VARCHAR(1) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        AD_ADM_COD AS [AdminCode],
        AD_ADM_NAM AS [AdminName],
        AD_ADM_TYP AS [AdminType],
        AD_ADM_UNT AS [UnitCode]
    FROM TRAVEL_ADMIN_UNITS
    WHERE (@p_AdminCode IS NULL OR AD_ADM_COD = @p_AdminCode)
      AND (@p_AdminType IS NULL OR AD_ADM_TYP = @p_AdminType)
    ORDER BY AD_ADM_NAM;
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
