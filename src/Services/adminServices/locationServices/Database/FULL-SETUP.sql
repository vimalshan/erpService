-- =====================================================
-- LOCATIONDB - FULL DATABASE SETUP SCRIPT
-- Run this ONCE to create everything from scratch:
--   Database → Tables → Indexes → Audit tables →
--   Stored Procedures → Sample Data
--
-- Usage:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i Database\FULL-SETUP.sql
-- =====================================================

-- =====================================================
-- STEP 1 : Create LOCATIONDB
-- =====================================================
USE MASTER;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LOCATIONDB')
BEGIN
    CREATE DATABASE [LOCATIONDB];
    PRINT '✓ LOCATIONDB created';
END
ELSE
    PRINT '✓ LOCATIONDB already exists';
GO

USE [LOCATIONDB];
GO

PRINT '';
PRINT '=== STEP 2: Base Table ===';
GO

-- =====================================================
-- STEP 2 : LOCATION_APP_MAP table
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOCATION_APP_MAP]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[LOCATION_APP_MAP]
    (
        [LOCATION_ID]        DECIMAL(22,0)  NOT NULL,
        [APP_NAME]           VARCHAR(255)   NOT NULL,
        [SITE_CATEGORY_CODE] BIGINT         NULL,
        [SELF_ACCESS]        VARCHAR(255)   NULL,
        [DEEMED_APPROVAL]    CHAR(1)        NULL,
        [CREATED_DATE]       DATETIME       NOT NULL DEFAULT GETUTCDATE(),
        [CREATED_BY]         VARCHAR(100)   NULL,
        [MODIFIED_DATE]      DATETIME       NULL,
        [MODIFIED_BY]        VARCHAR(100)   NULL,
        [IS_ACTIVE]          BIT            NOT NULL DEFAULT 1,
        CONSTRAINT [PK_LOCATION_APP_MAP] PRIMARY KEY ([LOCATION_ID], [APP_NAME])
    );
    PRINT '✓ LOCATION_APP_MAP created';
END
ELSE
    PRINT '✓ LOCATION_APP_MAP already exists';
GO

-- Base indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_APPNAME' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_APPNAME] ON [dbo].[LOCATION_APP_MAP]([APP_NAME]);
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_LOCATIONID' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_LOCATIONID] ON [dbo].[LOCATION_APP_MAP]([LOCATION_ID]);
GO

-- Filtered indexes: require QUOTED_IDENTIFIER ON
SET QUOTED_IDENTIFIER ON;
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_ACTIVE' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_ACTIVE] ON [dbo].[LOCATION_APP_MAP]([IS_ACTIVE], [LOCATION_ID])
    WHERE [IS_ACTIVE] = 1;
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_CATEGORY' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_CATEGORY] ON [dbo].[LOCATION_APP_MAP]([SITE_CATEGORY_CODE])
    WHERE [SITE_CATEGORY_CODE] IS NOT NULL;
GO
PRINT '';
PRINT '✓ Indexes created';
GO

PRINT '';
PRINT '=== STEP 3: Audit / History Tables ===';
GO

-- =====================================================
-- STEP 3 : AUDIT_LOG table
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AUDIT_LOG]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[AUDIT_LOG]
    (
        [AUDIT_ID]       BIGINT        IDENTITY(1,1) PRIMARY KEY,
        [TABLE_NAME]     VARCHAR(128)  NOT NULL,
        [RECORD_ID]      VARCHAR(500)  NOT NULL,
        [OPERATION_TYPE] VARCHAR(10)   NOT NULL,
        [OLD_VALUES]     NVARCHAR(MAX) NULL,
        [NEW_VALUES]     NVARCHAR(MAX) NULL,
        [CHANGED_BY]     VARCHAR(100)  NOT NULL,
        [CHANGED_DATE]   DATETIME      NOT NULL DEFAULT GETUTCDATE(),
        [IP_ADDRESS]     VARCHAR(50)   NULL
    );
    CREATE INDEX [IDX_AUDIT_TABLE_NAME]   ON [dbo].[AUDIT_LOG]([TABLE_NAME]);
    CREATE INDEX [IDX_AUDIT_CHANGED_DATE] ON [dbo].[AUDIT_LOG]([CHANGED_DATE]);
    PRINT '✓ AUDIT_LOG created';
END
ELSE
    PRINT '✓ AUDIT_LOG already exists';
GO

-- =====================================================
-- LOCATION_APP_MAP_HISTORY table
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOCATION_APP_MAP_HISTORY]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[LOCATION_APP_MAP_HISTORY]
    (
        [HISTORY_ID]         BIGINT        IDENTITY(1,1) PRIMARY KEY,
        [LOCATION_ID]        DECIMAL(22,0) NOT NULL,
        [APP_NAME]           VARCHAR(255)  NOT NULL,
        [SITE_CATEGORY_CODE] BIGINT        NULL,
        [SELF_ACCESS]        VARCHAR(255)  NULL,
        [DEEMED_APPROVAL]    CHAR(1)       NULL,
        [IS_ACTIVE]          BIT           NOT NULL,
        [CREATED_DATE]       DATETIME      NULL,
        [CREATED_BY]         VARCHAR(100)  NULL,
        [MODIFIED_DATE]      DATETIME      NULL,
        [MODIFIED_BY]        VARCHAR(100)  NULL,
        [CHANGE_DATE]        DATETIME      NOT NULL DEFAULT GETUTCDATE(),
        [CHANGE_TYPE]        VARCHAR(10)   NOT NULL
    );
    CREATE INDEX [IDX_HISTORY_LOCATION]    ON [dbo].[LOCATION_APP_MAP_HISTORY]([LOCATION_ID]);
    CREATE INDEX [IDX_HISTORY_APPNAME]     ON [dbo].[LOCATION_APP_MAP_HISTORY]([APP_NAME]);
    CREATE INDEX [IDX_HISTORY_CHANGE_DATE] ON [dbo].[LOCATION_APP_MAP_HISTORY]([CHANGE_DATE]);
    PRINT '✓ LOCATION_APP_MAP_HISTORY created';
END
ELSE
    PRINT '✓ LOCATION_APP_MAP_HISTORY already exists';
GO

PRINT '';
PRINT '=== STEP 4: Stored Procedures ===';
GO

-- =====================================================
-- STEP 4 : Stored Procedures
-- =====================================================

-- usp_LocationAppMap_GetAll
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_GetAll]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_GetAll];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],
           [CREATED_DATE],[CREATED_BY],[MODIFIED_DATE],[MODIFIED_BY],[IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    ORDER BY [LOCATION_ID], [APP_NAME];
END;
GO
PRINT '✓ usp_LocationAppMap_GetAll';
GO

-- usp_LocationAppMap_GetByLocationId
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_GetByLocationId]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_GetByLocationId];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetByLocationId]
    @LocationId DECIMAL(22,0)
AS
BEGIN
    SET NOCOUNT ON;
    IF @LocationId <= 0 BEGIN RAISERROR('Location ID must be > 0',16,1); RETURN; END;
    SELECT [LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],
           [CREATED_DATE],[CREATED_BY],[MODIFIED_DATE],[MODIFIED_BY],[IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    WHERE [LOCATION_ID] = @LocationId
    ORDER BY [APP_NAME];
END;
GO
PRINT '✓ usp_LocationAppMap_GetByLocationId';
GO

-- usp_LocationAppMap_GetByAppName
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_GetByAppName]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_GetByAppName];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetByAppName]
    @AppName VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    IF @AppName IS NULL OR LEN(LTRIM(@AppName)) = 0 BEGIN RAISERROR('AppName cannot be empty',16,1); RETURN; END;
    SELECT [LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],
           [CREATED_DATE],[CREATED_BY],[MODIFIED_DATE],[MODIFIED_BY],[IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    WHERE [APP_NAME] = @AppName
    ORDER BY [LOCATION_ID];
END;
GO
PRINT '✓ usp_LocationAppMap_GetByAppName';
GO

-- usp_LocationAppMap_GetMapping
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_GetMapping]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_GetMapping];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetMapping]
    @LocationId DECIMAL(22,0),
    @AppName    VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    IF @LocationId <= 0 BEGIN RAISERROR('Location ID must be > 0',16,1); RETURN; END;
    IF @AppName IS NULL OR LEN(LTRIM(@AppName)) = 0 BEGIN RAISERROR('AppName cannot be empty',16,1); RETURN; END;
    SELECT [LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],
           [CREATED_DATE],[CREATED_BY],[MODIFIED_DATE],[MODIFIED_BY],[IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName;
END;
GO
PRINT '✓ usp_LocationAppMap_GetMapping';
GO

-- usp_LocationAppMap_GetActive
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_GetActive]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_GetActive];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetActive]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],
           [CREATED_DATE],[CREATED_BY],[MODIFIED_DATE],[MODIFIED_BY],[IS_ACTIVE]
    FROM [dbo].[LOCATION_APP_MAP]
    WHERE [IS_ACTIVE] = 1
    ORDER BY [LOCATION_ID],[APP_NAME];
END;
GO
PRINT '✓ usp_LocationAppMap_GetActive';
GO

-- usp_LocationAppMap_Insert
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_Insert]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_Insert];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_Insert]
    @LocationId       DECIMAL(22,0),
    @AppName          VARCHAR(255),
    @SiteCategoryCode BIGINT      = NULL,
    @SelfAccess       VARCHAR(255)= NULL,
    @DeemedApproval   CHAR(1)     = NULL,
    @CreatedBy        VARCHAR(100)= 'System'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @LocationId <= 0 BEGIN RAISERROR('Location ID must be > 0',16,1); RETURN; END;
        IF @AppName IS NULL OR LEN(LTRIM(@AppName)) = 0 BEGIN RAISERROR('AppName cannot be empty',16,1); RETURN; END;
        IF EXISTS(SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=@LocationId AND [APP_NAME]=@AppName)
            BEGIN RAISERROR('Mapping already exists',16,1); RETURN; END;
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_DATE],[CREATED_BY],[IS_ACTIVE])
        VALUES(@LocationId,@AppName,@SiteCategoryCode,@SelfAccess,@DeemedApproval,GETUTCDATE(),@CreatedBy,1);
        SELECT 'Mapping inserted successfully' AS [Message];
    END TRY
    BEGIN CATCH THROW; END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_Insert';
GO

-- usp_LocationAppMap_Update
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_Update]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_Update];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_Update]
    @LocationId       DECIMAL(22,0),
    @AppName          VARCHAR(255),
    @SiteCategoryCode BIGINT      = NULL,
    @SelfAccess       VARCHAR(255)= NULL,
    @DeemedApproval   CHAR(1)     = NULL,
    @IsActive         BIT         = 1,
    @ModifiedBy       VARCHAR(100)= 'System'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @LocationId <= 0 BEGIN RAISERROR('Location ID must be > 0',16,1); RETURN; END;
        IF @AppName IS NULL OR LEN(LTRIM(@AppName)) = 0 BEGIN RAISERROR('AppName cannot be empty',16,1); RETURN; END;
        IF NOT EXISTS(SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=@LocationId AND [APP_NAME]=@AppName)
            BEGIN RAISERROR('Mapping not found',16,1); RETURN; END;
        UPDATE [dbo].[LOCATION_APP_MAP]
        SET [SITE_CATEGORY_CODE]=@SiteCategoryCode, [SELF_ACCESS]=@SelfAccess,
            [DEEMED_APPROVAL]=@DeemedApproval, [MODIFIED_DATE]=GETUTCDATE(),
            [MODIFIED_BY]=@ModifiedBy, [IS_ACTIVE]=@IsActive
        WHERE [LOCATION_ID]=@LocationId AND [APP_NAME]=@AppName;
        SELECT 'Mapping updated successfully' AS [Message];
    END TRY
    BEGIN CATCH THROW; END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_Update';
GO

-- usp_LocationAppMap_Delete
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_Delete]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_Delete];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_Delete]
    @LocationId DECIMAL(22,0),
    @AppName    VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @LocationId <= 0 BEGIN RAISERROR('Location ID must be > 0',16,1); RETURN; END;
        IF @AppName IS NULL OR LEN(LTRIM(@AppName)) = 0 BEGIN RAISERROR('AppName cannot be empty',16,1); RETURN; END;
        DELETE FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=@LocationId AND [APP_NAME]=@AppName;
        SELECT 'Mapping deleted successfully' AS [Message];
    END TRY
    BEGIN CATCH THROW; END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_Delete';
GO

-- usp_LocationAppMap_DeleteByLocationId
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_DeleteByLocationId]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_DeleteByLocationId];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_DeleteByLocationId]
    @LocationId DECIMAL(22,0)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @LocationId <= 0 BEGIN RAISERROR('Location ID must be > 0',16,1); RETURN; END;
        DELETE FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=@LocationId;
        SELECT 'All mappings for location deleted' AS [Message];
    END TRY
    BEGIN CATCH THROW; END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_DeleteByLocationId';
GO

-- usp_LocationAppMap_GetCount
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_GetCount]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_GetCount];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetCount]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS [TotalMappings] FROM [dbo].[LOCATION_APP_MAP];
END;
GO
PRINT '✓ usp_LocationAppMap_GetCount';
GO

-- usp_LocationAppMap_GetCountActive
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_GetCountActive]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_GetCountActive];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_GetCountActive]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS [ActiveMappings] FROM [dbo].[LOCATION_APP_MAP] WHERE [IS_ACTIVE]=1;
END;
GO
PRINT '✓ usp_LocationAppMap_GetCountActive';
GO

-- usp_AuditLog_Insert
IF OBJECT_ID(N'[dbo].[usp_AuditLog_Insert]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_AuditLog_Insert];
GO
CREATE PROCEDURE [dbo].[usp_AuditLog_Insert]
    @TableName     VARCHAR(128),
    @RecordId      VARCHAR(500),
    @OperationType VARCHAR(10),
    @OldValues     NVARCHAR(MAX) = NULL,
    @NewValues     NVARCHAR(MAX) = NULL,
    @ChangedBy     VARCHAR(100),
    @IpAddress     VARCHAR(50)   = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO [dbo].[AUDIT_LOG]([TABLE_NAME],[RECORD_ID],[OPERATION_TYPE],[OLD_VALUES],[NEW_VALUES],[CHANGED_BY],[IP_ADDRESS])
    VALUES(@TableName,@RecordId,@OperationType,@OldValues,@NewValues,@ChangedBy,@IpAddress);
    SELECT SCOPE_IDENTITY() AS [AUDIT_ID];
END;
GO
PRINT '✓ usp_AuditLog_Insert';
GO

-- usp_LocationAppMap_InsertWithAudit
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_InsertWithAudit]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_InsertWithAudit];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_InsertWithAudit]
    @LocationId       DECIMAL(22,0),
    @AppName          VARCHAR(255),
    @SiteCategoryCode BIGINT       = NULL,
    @SelfAccess       VARCHAR(255) = NULL,
    @DeemedApproval   CHAR(1)      = NULL,
    @CreatedBy        VARCHAR(100) = 'System',
    @IpAddress        VARCHAR(50)  = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @LocationId <= 0 BEGIN RAISERROR('Location ID must be > 0',16,1); ROLLBACK; RETURN; END;
        IF @AppName IS NULL OR LEN(LTRIM(@AppName)) = 0 BEGIN RAISERROR('AppName cannot be empty',16,1); ROLLBACK; RETURN; END;
        IF EXISTS(SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=@LocationId AND [APP_NAME]=@AppName)
            BEGIN RAISERROR('Mapping already exists',16,1); ROLLBACK; RETURN; END;

        -- Insert main record
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_DATE],[CREATED_BY],[IS_ACTIVE])
        VALUES(@LocationId,@AppName,@SiteCategoryCode,@SelfAccess,@DeemedApproval,GETUTCDATE(),@CreatedBy,1);

        -- Insert history
        INSERT INTO [dbo].[LOCATION_APP_MAP_HISTORY]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[IS_ACTIVE],[CREATED_DATE],[CREATED_BY],[CHANGE_TYPE])
        VALUES(@LocationId,@AppName,@SiteCategoryCode,@SelfAccess,@DeemedApproval,1,GETUTCDATE(),@CreatedBy,'INSERT');

        -- Audit log (use variables — EXEC named params cannot be expressions)
        DECLARE @AuditRecordId  VARCHAR(500)   = CAST(@LocationId AS VARCHAR(50)) + '|' + @AppName;
        DECLARE @AuditNewValues NVARCHAR(MAX)  = 'LocationId=' + CAST(@LocationId AS VARCHAR(50))
                                               + ', AppName=' + @AppName
                                               + ', SiteCategoryCode=' + ISNULL(CAST(@SiteCategoryCode AS VARCHAR(20)), 'NULL');
        EXEC [dbo].[usp_AuditLog_Insert]
            @TableName     = 'LOCATION_APP_MAP',
            @RecordId      = @AuditRecordId,
            @OperationType = 'INSERT',
            @NewValues     = @AuditNewValues,
            @ChangedBy     = @CreatedBy,
            @IpAddress     = @IpAddress;

        COMMIT TRANSACTION;
        SELECT 'Mapping inserted with audit' AS [Message];
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_InsertWithAudit';
GO

-- usp_LocationAppMap_UpdateWithAudit
IF OBJECT_ID(N'[dbo].[usp_LocationAppMap_UpdateWithAudit]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_LocationAppMap_UpdateWithAudit];
GO
CREATE PROCEDURE [dbo].[usp_LocationAppMap_UpdateWithAudit]
    @LocationId       DECIMAL(22,0),
    @AppName          VARCHAR(255),
    @SiteCategoryCode BIGINT       = NULL,
    @SelfAccess       VARCHAR(255) = NULL,
    @DeemedApproval   CHAR(1)      = NULL,
    @IsActive         BIT          = 1,
    @ModifiedBy       VARCHAR(100) = 'System',
    @IpAddress        VARCHAR(50)  = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OldValues NVARCHAR(MAX);
        DECLARE @NewValues NVARCHAR(MAX);

        IF @LocationId <= 0 BEGIN RAISERROR('Location ID must be > 0',16,1); ROLLBACK; RETURN; END;
        IF @AppName IS NULL OR LEN(LTRIM(@AppName)) = 0 BEGIN RAISERROR('AppName cannot be empty',16,1); ROLLBACK; RETURN; END;
        IF NOT EXISTS(SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=@LocationId AND [APP_NAME]=@AppName)
            BEGIN RAISERROR('Mapping not found',16,1); ROLLBACK; RETURN; END;

        -- Capture old values
        SELECT @OldValues = 'SiteCategoryCode=' + ISNULL(CAST([SITE_CATEGORY_CODE] AS VARCHAR(20)), 'NULL')
                          + ', SelfAccess='    + ISNULL([SELF_ACCESS], 'NULL')
                          + ', DeemedApproval='+ ISNULL([DEEMED_APPROVAL], 'NULL')
                          + ', IsActive='      + CAST([IS_ACTIVE] AS VARCHAR(1))
        FROM [dbo].[LOCATION_APP_MAP]
        WHERE [LOCATION_ID]=@LocationId AND [APP_NAME]=@AppName;

        -- Update
        UPDATE [dbo].[LOCATION_APP_MAP]
        SET [SITE_CATEGORY_CODE]=@SiteCategoryCode, [SELF_ACCESS]=@SelfAccess,
            [DEEMED_APPROVAL]=@DeemedApproval, [MODIFIED_DATE]=GETUTCDATE(),
            [MODIFIED_BY]=@ModifiedBy, [IS_ACTIVE]=@IsActive
        WHERE [LOCATION_ID]=@LocationId AND [APP_NAME]=@AppName;

        -- Insert history
        INSERT INTO [dbo].[LOCATION_APP_MAP_HISTORY]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[IS_ACTIVE],[MODIFIED_DATE],[MODIFIED_BY],[CHANGE_TYPE])
        VALUES(@LocationId,@AppName,@SiteCategoryCode,@SelfAccess,@DeemedApproval,@IsActive,GETUTCDATE(),@ModifiedBy,'UPDATE');

        -- New values string
        SET @NewValues = 'SiteCategoryCode=' + ISNULL(CAST(@SiteCategoryCode AS VARCHAR(20)), 'NULL')
                       + ', SelfAccess='    + ISNULL(@SelfAccess, 'NULL')
                       + ', DeemedApproval='+ ISNULL(@DeemedApproval, 'NULL')
                       + ', IsActive='      + CAST(@IsActive AS VARCHAR(1));

        -- Audit log
        DECLARE @AuditRecordId2 VARCHAR(500) = CAST(@LocationId AS VARCHAR(50)) + '|' + @AppName;
        EXEC [dbo].[usp_AuditLog_Insert]
            @TableName     = 'LOCATION_APP_MAP',
            @RecordId      = @AuditRecordId2,
            @OperationType = 'UPDATE',
            @OldValues     = @OldValues,
            @NewValues     = @NewValues,
            @ChangedBy     = @ModifiedBy,
            @IpAddress     = @IpAddress;

        COMMIT TRANSACTION;
        SELECT 'Mapping updated with audit' AS [Message];
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_UpdateWithAudit';
GO

PRINT '';
PRINT '=== STEP 5: Sample Data ===';
GO

-- =====================================================
-- STEP 5 : Sample Data (idempotent)
-- =====================================================
BEGIN TRY
    BEGIN TRANSACTION;

    -- Location 1001 - Warehouse
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=1001 AND [APP_NAME]='WarehouseApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(1001,'WarehouseApp',100,'Y','Y','Admin',1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=1001 AND [APP_NAME]='InventoryApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(1001,'InventoryApp',101,'Y','N','Admin',1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=1001 AND [APP_NAME]='ReportingApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(1001,'ReportingApp',102,'N','Y','Admin',1);

    -- Location 2001 - Manufacturing
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=2001 AND [APP_NAME]='ManufacturingApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(2001,'ManufacturingApp',200,'Y','Y','Admin',1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=2001 AND [APP_NAME]='QualityApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(2001,'QualityApp',201,'Y','Y','Admin',1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=2001 AND [APP_NAME]='MaintenanceApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(2001,'MaintenanceApp',202,'Y','N','Admin',1);

    -- Location 3001 - Distribution
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=3001 AND [APP_NAME]='DistributionApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(3001,'DistributionApp',300,'Y','Y','Admin',1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=3001 AND [APP_NAME]='ShippingApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(3001,'ShippingApp',301,'Y','Y','Admin',1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=3001 AND [APP_NAME]='ReportingApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(3001,'ReportingApp',102,'N','Y','Admin',1);

    -- Location 4001 - Regional Office
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=4001 AND [APP_NAME]='HRApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(4001,'HRApp',400,'N','Y','Admin',1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=4001 AND [APP_NAME]='FinanceApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(4001,'FinanceApp',401,'N','Y','Admin',1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=4001 AND [APP_NAME]='ReportingApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(4001,'ReportingApp',102,'Y','Y','Admin',1);

    -- Location 5001 - Sales
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=5001 AND [APP_NAME]='SalesApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(5001,'SalesApp',500,'Y','N','Admin',1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=5001 AND [APP_NAME]='CRMApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[IS_ACTIVE])
        VALUES(5001,'CRMApp',501,'Y','Y','Admin',1);

    -- Location 6001 - Legacy (inactive)
    IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP] WHERE [LOCATION_ID]=6001 AND [APP_NAME]='LegacyApp')
        INSERT INTO [dbo].[LOCATION_APP_MAP]([LOCATION_ID],[APP_NAME],[SITE_CATEGORY_CODE],[SELF_ACCESS],[DEEMED_APPROVAL],[CREATED_BY],[MODIFIED_BY],[IS_ACTIVE])
        VALUES(6001,'LegacyApp',600,'N','N','Admin','Admin',0);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    THROW;
END CATCH;
GO

PRINT '';
PRINT '========================================';
PRINT '  LOCATIONDB SETUP COMPLETE';
PRINT '========================================';
GO

-- Verification summary
SELECT
    (SELECT COUNT(*) FROM [dbo].[LOCATION_APP_MAP])          AS TotalMappings,
    (SELECT COUNT(*) FROM [dbo].[LOCATION_APP_MAP] WHERE [IS_ACTIVE]=1) AS ActiveMappings,
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE='PROCEDURE') AS StoredProcedures,
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE') AS Tables;
GO
