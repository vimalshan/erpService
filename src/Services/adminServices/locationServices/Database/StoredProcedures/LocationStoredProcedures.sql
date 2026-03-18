-- ==========================================
-- LOCATION MODULE STORED PROCEDURES
-- Database: LOCATIONDB
-- Version: 1.0
-- ==========================================

USE [LOCATIONDB];
GO

PRINT '=== Creating Stored Procedures ===';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_GetAll
-- Purpose: Get all location app mappings
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        [LOCATION_ID],
        [APP_NAME],
        [SITE_CATEGORY_CODE],
        [SELF_ACCESS],
        [DEEMED_APPROVAL],
        [CREATED_DATE],
        [CREATED_BY],
        [MODIFIED_DATE],
        [MODIFIED_BY],
        [IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    ORDER BY [LOCATION_ID], [APP_NAME];
END;
GO
PRINT '✓ usp_LocationAppMap_GetAll created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_GetByLocationId
-- Purpose: Get mappings by location ID
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetByLocationId]
    @LocationId DECIMAL(22,0)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @LocationId <= 0
    BEGIN
        RAISERROR('Location ID must be greater than zero', 16, 1);
        RETURN;
    END;
    
    SELECT 
        [LOCATION_ID],
        [APP_NAME],
        [SITE_CATEGORY_CODE],
        [SELF_ACCESS],
        [DEEMED_APPROVAL],
        [CREATED_DATE],
        [CREATED_BY],
        [MODIFIED_DATE],
        [MODIFIED_BY],
        [IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    WHERE [LOCATION_ID] = @LocationId
    ORDER BY [APP_NAME];
END;
GO
PRINT '✓ usp_LocationAppMap_GetByLocationId created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_GetByAppName
-- Purpose: Get mappings by application name
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetByAppName]
    @AppName VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @AppName IS NULL OR LEN(@AppName) = 0
    BEGIN
        RAISERROR('Application name cannot be empty', 16, 1);
        RETURN;
    END;
    
    SELECT 
        [LOCATION_ID],
        [APP_NAME],
        [SITE_CATEGORY_CODE],
        [SELF_ACCESS],
        [DEEMED_APPROVAL],
        [CREATED_DATE],
        [CREATED_BY],
        [MODIFIED_DATE],
        [MODIFIED_BY],
        [IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    WHERE [APP_NAME] = @AppName
    ORDER BY [LOCATION_ID];
END;
GO
PRINT '✓ usp_LocationAppMap_GetByAppName created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_GetMapping
-- Purpose: Get a single mapping by location ID and app name
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetMapping]
    @LocationId DECIMAL(22,0),
    @AppName VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @LocationId <= 0
    BEGIN
        RAISERROR('Location ID must be greater than zero', 16, 1);
        RETURN;
    END;
    
    IF @AppName IS NULL OR LEN(@AppName) = 0
    BEGIN
        RAISERROR('Application name cannot be empty', 16, 1);
        RETURN;
    END;
    
    SELECT 
        [LOCATION_ID],
        [APP_NAME],
        [SITE_CATEGORY_CODE],
        [SELF_ACCESS],
        [DEEMED_APPROVAL],
        [CREATED_DATE],
        [CREATED_BY],
        [MODIFIED_DATE],
        [MODIFIED_BY],
        [IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName;
END;
GO
PRINT '✓ usp_LocationAppMap_GetMapping created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_GetActive
-- Purpose: Get all active location app mappings
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetActive]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        [LOCATION_ID],
        [APP_NAME],
        [SITE_CATEGORY_CODE],
        [SELF_ACCESS],
        [DEEMED_APPROVAL],
        [CREATED_DATE],
        [CREATED_BY],
        [MODIFIED_DATE],
        [MODIFIED_BY],
        [IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    WHERE [IS_ACTIVE] = 1
    ORDER BY [LOCATION_ID], [APP_NAME];
END;
GO
PRINT '✓ usp_LocationAppMap_GetActive created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_Insert
-- Purpose: Insert a new location app mapping
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_Insert]
    @LocationId DECIMAL(22,0),
    @AppName VARCHAR(255),
    @SiteCategoryCode BIGINT = NULL,
    @SelfAccess VARCHAR(255) = NULL,
    @DeemedApproval CHAR(1) = NULL,
    @CreatedBy VARCHAR(100) = 'System'
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF @LocationId <= 0
        BEGIN
            RAISERROR('Location ID must be greater than zero', 16, 1);
            RETURN;
        END;
        
        IF @AppName IS NULL OR LEN(@AppName) = 0
        BEGIN
            RAISERROR('Application name cannot be empty', 16, 1);
            RETURN;
        END;
        
        -- Check if mapping already exists
        IF EXISTS(SELECT 1 FROM [dbo].[LOCATION_APP_MAP] 
                  WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName)
        BEGIN
            RAISERROR('Mapping already exists', 16, 1);
            RETURN;
        END;
        
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (@LocationId, @AppName, @SiteCategoryCode, @SelfAccess, @DeemedApproval, GETUTCDATE(), @CreatedBy, 1);
        
        SELECT 'Mapping inserted successfully' AS [Message];
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_Insert created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_Update
-- Purpose: Update an existing location app mapping
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_Update]
    @LocationId DECIMAL(22,0),
    @AppName VARCHAR(255),
    @SiteCategoryCode BIGINT = NULL,
    @SelfAccess VARCHAR(255) = NULL,
    @DeemedApproval CHAR(1) = NULL,
    @IsActive BIT = 1,
    @ModifiedBy VARCHAR(100) = 'System'
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF @LocationId <= 0
        BEGIN
            RAISERROR('Location ID must be greater than zero', 16, 1);
            RETURN;
        END;
        
        IF @AppName IS NULL OR LEN(@AppName) = 0
        BEGIN
            RAISERROR('Application name cannot be empty', 16, 1);
            RETURN;
        END;
        
        -- Check if mapping exists
        IF NOT EXISTS(SELECT 1 FROM [dbo].[LOCATION_APP_MAP] 
                      WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName)
        BEGIN
            RAISERROR('Mapping not found', 16, 1);
            RETURN;
        END;
        
        UPDATE [dbo].[LOCATION_APP_MAP]
        SET 
            [SITE_CATEGORY_CODE] = @SiteCategoryCode,
            [SELF_ACCESS] = @SelfAccess,
            [DEEMED_APPROVAL] = @DeemedApproval,
            [MODIFIED_DATE] = GETUTCDATE(),
            [MODIFIED_BY] = @ModifiedBy,
            [IS_ACTIVE] = @IsActive
        WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName;
        
        SELECT 'Mapping updated successfully' AS [Message];
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_Update created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_Delete
-- Purpose: Delete a location app mapping
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_Delete]
    @LocationId DECIMAL(22,0),
    @AppName VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF @LocationId <= 0
        BEGIN
            RAISERROR('Location ID must be greater than zero', 16, 1);
            RETURN;
        END;
        
        IF @AppName IS NULL OR LEN(@AppName) = 0
        BEGIN
            RAISERROR('Application name cannot be empty', 16, 1);
            RETURN;
        END;
        
        DELETE FROM [dbo].[LOCATION_APP_MAP]
        WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName;
        
        SELECT 'Mapping deleted successfully' AS [Message];
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_Delete created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_DeleteByLocationId
-- Purpose: Delete all mappings for a location
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_DeleteByLocationId]
    @LocationId DECIMAL(22,0)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF @LocationId <= 0
        BEGIN
            RAISERROR('Location ID must be greater than zero', 16, 1);
            RETURN;
        END;
        
        DELETE FROM [dbo].[LOCATION_APP_MAP]
        WHERE [LOCATION_ID] = @LocationId;
        
        SELECT 'All mappings for location deleted successfully' AS [Message];
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_DeleteByLocationId created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_GetCount
-- Purpose: Get total count of mappings
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetCount]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(*) AS [TotalMappings] FROM [dbo].[LOCATION_APP_MAP];
END;
GO
PRINT '✓ usp_LocationAppMap_GetCount created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_GetCountActive
-- Purpose: Get count of active mappings
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetCountActive]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(*) AS [ActiveMappings] FROM [dbo].[LOCATION_APP_MAP] WHERE [IS_ACTIVE] = 1;
END;
GO
PRINT '✓ usp_LocationAppMap_GetCountActive created';
GO

PRINT '';
PRINT '========================================';
PRINT 'STORED PROCEDURES CREATED SUCCESSFULLY';
PRINT '========================================';
GO
