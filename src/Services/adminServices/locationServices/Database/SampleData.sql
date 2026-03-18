-- ==========================================
-- LOCATION MODULE - SAMPLE DATA SCRIPT
-- Database: LOCATIONDB
-- Version: 1.0
-- Generated: 2026-03-09
-- ==========================================

USE [LOCATIONDB];
GO

PRINT '=== Inserting Sample Data ===';
GO

BEGIN TRY
    BEGIN TRANSACTION;

    -- Clear existing data (optional)
    -- DELETE FROM [dbo].[LOCATION_APP_MAP];

    -- ==========================================
    -- Location 1001 - Warehouse Facility
    -- ==========================================
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 1001 AND [APP_NAME] = 'WarehouseApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (1001, 'WarehouseApp', 100, 'Y', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 1001 - WarehouseApp';
    END;
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 1001 AND [APP_NAME] = 'InventoryApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (1001, 'InventoryApp', 101, 'Y', 'N', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 1001 - InventoryApp';
    END;
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 1001 AND [APP_NAME] = 'ReportingApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (1001, 'ReportingApp', 102, 'N', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 1001 - ReportingApp';
    END;

    -- ==========================================
    -- Location 2001 - Manufacturing Facility
    -- ==========================================
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 2001 AND [APP_NAME] = 'ManufacturingApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (2001, 'ManufacturingApp', 200, 'Y', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 2001 - ManufacturingApp';
    END;
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 2001 AND [APP_NAME] = 'QualityApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (2001, 'QualityApp', 201, 'Y', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 2001 - QualityApp';
    END;
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 2001 AND [APP_NAME] = 'MaintenanceApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (2001, 'MaintenanceApp', 202, 'Y', 'N', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 2001 - MaintenanceApp';
    END;

    -- ==========================================
    -- Location 3001 - Distribution Center
    -- ==========================================
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 3001 AND [APP_NAME] = 'DistributionApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (3001, 'DistributionApp', 300, 'Y', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 3001 - DistributionApp';
    END;
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 3001 AND [APP_NAME] = 'ShippingApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (3001, 'ShippingApp', 301, 'Y', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 3001 - ShippingApp';
    END;
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 3001 AND [APP_NAME] = 'ReportingApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (3001, 'ReportingApp', 102, 'N', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 3001 - ReportingApp';
    END;

    -- ==========================================
    -- Location 4001 - Regional Office
    -- ==========================================
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 4001 AND [APP_NAME] = 'HRApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (4001, 'HRApp', 400, 'N', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 4001 - HRApp';
    END;
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 4001 AND [APP_NAME] = 'FinanceApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (4001, 'FinanceApp', 401, 'N', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 4001 - FinanceApp';
    END;
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 4001 AND [APP_NAME] = 'ReportingApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (4001, 'ReportingApp', 102, 'Y', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 4001 - ReportingApp';
    END;

    -- ==========================================
    -- Location 5001 - Sales Office
    -- ==========================================
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 5001 AND [APP_NAME] = 'SalesApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (5001, 'SalesApp', 500, 'Y', 'N', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 5001 - SalesApp';
    END;
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 5001 AND [APP_NAME] = 'CRMApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (5001, 'CRMApp', 501, 'Y', 'Y', GETUTCDATE(), 'Admin', 1);
        PRINT '  + Location 5001 - CRMApp';
    END;

    -- ==========================================
    -- Location 6001 - Legacy/Inactive
    -- ==========================================
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID] = 6001 AND [APP_NAME] = 'LegacyApp')
    BEGIN
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [MODIFIED_DATE], [MODIFIED_BY], [IS_ACTIVE])
        VALUES 
        (6001, 'LegacyApp', 600, 'N', 'N', DATEADD(MONTH, -6, GETUTCDATE()), 'Admin', DATEADD(MONTH, -3, GETUTCDATE()), 'Admin', 0);
        PRINT '  + Location 6001 - LegacyApp (inactive)';
    END;

    COMMIT TRANSACTION;

    PRINT '';
    PRINT '========================================';
    PRINT 'SAMPLE DATA INSERTION COMPLETE';
    PRINT '========================================';
    PRINT '';
    
    DECLARE @TotalCount INT;
    DECLARE @ActiveCount INT;
    DECLARE @InactiveCount INT;
    
    SELECT @TotalCount = COUNT(*) FROM [dbo].[LOCATION_APP_MAP];
    SELECT @ActiveCount = COUNT(*) FROM [dbo].[LOCATION_APP_MAP] WHERE [IS_ACTIVE] = 1;
    SELECT @InactiveCount = COUNT(*) FROM [dbo].[LOCATION_APP_MAP] WHERE [IS_ACTIVE] = 0;
    
    PRINT 'Summary:';
    PRINT '  Total Mappings : ' + CAST(@TotalCount AS VARCHAR(10));
    PRINT '  Active         : ' + CAST(@ActiveCount AS VARCHAR(10));
    PRINT '  Inactive       : ' + CAST(@InactiveCount AS VARCHAR(10));
    PRINT '';
    PRINT '========================================';

END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT '✗ Error inserting sample data: ' + ERROR_MESSAGE();
    THROW;
END CATCH;
GO

-- ==========================================
-- Sample Queries to Verify Data
-- ==========================================

PRINT '';
PRINT '=== Verification Queries ===';
GO

-- View all data
SELECT 
    [LOCATION_ID] as LocationId,
    [APP_NAME] as AppName,
    [SITE_CATEGORY_CODE] as SiteCategoryCode,
    [SELF_ACCESS] as SelfAccess,
    [DEEMED_APPROVAL] as DeemedApproval,
    [IS_ACTIVE] as IsActive,
    [CREATED_DATE] as CreatedDate
FROM [dbo].[LOCATION_APP_MAP]
ORDER BY [LOCATION_ID], [APP_NAME];
GO

-- Summary by location
SELECT 
    [LOCATION_ID],
    COUNT(*) as TotalApps,
    SUM(CASE WHEN [IS_ACTIVE] = 1 THEN 1 ELSE 0 END) as ActiveApps,
    SUM(CASE WHEN [IS_ACTIVE] = 0 THEN 1 ELSE 0 END) as InactiveApps
FROM [dbo].[LOCATION_APP_MAP]
GROUP BY [LOCATION_ID]
ORDER BY [LOCATION_ID];
GO
