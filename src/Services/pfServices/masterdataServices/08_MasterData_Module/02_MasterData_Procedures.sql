-- =========================================================================
-- MASTER DATA MODULE - Procedures and Functions
-- Database: PFDB
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- PROCEDURE: Add LOV Value
IF OBJECT_ID('dbo.usp_AddLOVValue', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_AddLOVValue;
GO

CREATE PROCEDURE dbo.usp_AddLOVValue
    @p_LOVCode VARCHAR(10),
    @p_LOVDesc VARCHAR(100),
    @p_LOVValue VARCHAR(20),
    @p_Category VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        DECLARE @NewID DECIMAL(38) = (SELECT ISNULL(MAX(LOV_ID), 0) + 1 FROM dbo.LOV_MASTER);
        
        INSERT INTO dbo.LOV_MASTER (
            LOV_ID, LOV_CODE, LOV_DESC, LOV_VALUE, LOV_CATEGORY, LOV_STATUS
        ) VALUES (
            @NewID, @p_LOVCode, @p_LOVDesc, @p_LOVValue, @p_Category, 'A'
        );
        
        PRINT 'LOV value added successfully';
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- PROCEDURE: Update Configuration
IF OBJECT_ID('dbo.usp_UpdateConfiguration', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_UpdateConfiguration;
GO

CREATE PROCEDURE dbo.usp_UpdateConfiguration
    @p_ConfigKey VARCHAR(100),
    @p_ConfigValue VARCHAR(500),
    @p_ConfigType VARCHAR(50),
    @p_UpdatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.CONFIGURATION WHERE CONFIG_KEY = @p_ConfigKey)
        BEGIN
            UPDATE dbo.CONFIGURATION
            SET CONFIG_VALUE = @p_ConfigValue,
                CONFIG_TYPE = @p_ConfigType,
                UPDATED_DATE = GETDATE()
            WHERE CONFIG_KEY = @p_ConfigKey;
        END
        ELSE
        BEGIN
            INSERT INTO dbo.CONFIGURATION (
                CONFIG_KEY, CONFIG_VALUE, CONFIG_TYPE, CREATED_DATE, CREATED_BY
            ) VALUES (
                @p_ConfigKey, @p_ConfigValue, @p_ConfigType, GETDATE(), @p_UpdatedBy
            );
        END
        
        PRINT 'Configuration updated successfully';
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- VIEW: LOV Values by Category
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_LOVByCategory' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_LOVByCategory AS
    SELECT 
        LOV_CATEGORY,
        LOV_CODE,
        LOV_DESC,
        LOV_VALUE,
        LOV_STATUS
    FROM dbo.LOV_MASTER
    WHERE LOV_STATUS = 'A'
    ORDER BY LOV_CATEGORY, LOV_CODE;
END
GO

-- VIEW: Active Configuration
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_ActiveConfiguration' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_ActiveConfiguration AS
    SELECT 
        CONFIG_KEY,
        CONFIG_VALUE,
        CONFIG_TYPE,
        CONFIG_CATEGORY,
        CREATED_DATE,
        UPDATED_DATE
    FROM dbo.CONFIGURATION
    ORDER BY CONFIG_CATEGORY, CONFIG_KEY;
END
GO

PRINT 'Master Data Module Procedures created successfully!';
GO
