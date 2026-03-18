-- ==========================================
-- Module: MASTER DATA & REFERENCE
-- Description: Master data procedures and reference management
-- Procedures for managing reference data
-- ==========================================

USE [TRAVELDB];
GO

-- ------------------------------------------------------------------
-- Procedure: usp_CreateGuestHouse
-- Purpose: Create a guest house master record
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_CreateGuestHouse
(
    @p_AdminCode BIGINT,
    @p_GuestHouseName VARCHAR(50),
    @p_DailyAmount BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Insert guest house
        INSERT INTO TRAVEL_GUESTHOUSE
        (AD_ADM_COD, AD_ADM_NAM, AD_ADM_TYP, AD_ADM_AMOUNT)
        VALUES
        (@p_AdminCode, @p_GuestHouseName, 'S', @p_DailyAmount);
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Guest house created successfully' AS [Message];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_RegisterArea
-- Purpose: Register a geographic area
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RegisterArea
(
    @p_AreaID INT,
    @p_AreaName VARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Insert area
        INSERT INTO AREA_MASTER
        (AREA_ID, AREA_NAME)
        VALUES
        (@p_AreaID, @p_AreaName);
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Area registered successfully' AS [Message],
               @p_AreaID AS [AreaID];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [AreaID];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_RegisterRoute
-- Purpose: Register a route
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RegisterRoute
(
    @p_RouteID INT,
    @p_RouteName VARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Insert route
        INSERT INTO ROUTE_MASTER
        (ROUTE_ID, ROUTE_NAME)
        VALUES
        (@p_RouteID, @p_RouteName);
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Route registered successfully' AS [Message],
               @p_RouteID AS [RouteID];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [RouteID];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_GetGuestHouseList
-- Purpose: Retrieve guest house details
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_GetGuestHouseList
(
    @p_AdminCode BIGINT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        AD_ADM_COD AS [AdminCode],
        AD_ADM_NAM AS [GuestHouseName],
        AD_ADM_AMOUNT AS [DailyAmount]
    FROM TRAVEL_GUESTHOUSE
    WHERE (@p_AdminCode IS NULL OR AD_ADM_COD = @p_AdminCode)
    ORDER BY AD_ADM_NAM;
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
