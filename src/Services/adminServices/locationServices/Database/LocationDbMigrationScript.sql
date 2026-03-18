-- ==========================================
-- LOCATION MODULE - EXTENDED MIGRATION SCRIPT
-- Database: LOCATIONDB
-- Version: 1.0
-- Purpose: Additional tables and enhancements
-- ==========================================

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

-- ==========================================
-- Base Table: LOCATION_APP_MAP (if not exists)
-- ==========================================
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
    PRINT '✓ LOCATION_APP_MAP table created';
END
ELSE
    PRINT '✓ LOCATION_APP_MAP already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_APPNAME' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_APPNAME] ON [dbo].[LOCATION_APP_MAP]([APP_NAME]);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_LOCATIONID' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_LOCATIONID] ON [dbo].[LOCATION_APP_MAP]([LOCATION_ID]);
GO

PRINT '=== Creating Additional Audit and Audit Trail Tables ===';
GO

-- ==========================================
-- Table: AUDIT_LOG
-- Purpose: Track all changes made to location app mappings
-- ==========================================
CREATE TABLE [dbo].[AUDIT_LOG]
(
    [AUDIT_ID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [TABLE_NAME] VARCHAR(128) NOT NULL,
    [RECORD_ID] VARCHAR(500) NOT NULL,
    [OPERATION_TYPE] VARCHAR(10) NOT NULL, -- INSERT, UPDATE, DELETE
    [OLD_VALUES] NVARCHAR(MAX) NULL,
    [NEW_VALUES] NVARCHAR(MAX) NULL,
    [CHANGED_BY] VARCHAR(100) NOT NULL,
    [CHANGED_DATE] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [IP_ADDRESS] VARCHAR(50) NULL,
    INDEX [IDX_AUDIT_TABLE_NAME] ([TABLE_NAME]),
    INDEX [IDX_AUDIT_CHANGED_DATE] ([CHANGED_DATE])
);
GO
PRINT '✓ AUDIT_LOG table created';
GO

-- ==========================================
-- Table: LOCATION_APP_MAP_HISTORY
-- Purpose: Maintain historical records of location app mappings
-- ==========================================
CREATE TABLE [dbo].[LOCATION_APP_MAP_HISTORY]
(
    [HISTORY_ID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [LOCATION_ID] DECIMAL(22,0) NOT NULL,
    [APP_NAME] VARCHAR(255) NOT NULL,
    [SITE_CATEGORY_CODE] BIGINT NULL,
    [SELF_ACCESS] VARCHAR(255) NULL,
    [DEEMED_APPROVAL] CHAR(1) NULL,
    [IS_ACTIVE] BIT NOT NULL,
    [CREATED_DATE] DATETIME NOT NULL,
    [CREATED_BY] VARCHAR(100) NULL,
    [MODIFIED_DATE] DATETIME NULL,
    [MODIFIED_BY] VARCHAR(100) NULL,
    [CHANGE_DATE] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CHANGE_TYPE] VARCHAR(10) NOT NULL, -- INSERT, UPDATE, DELETE
    INDEX [IDX_HISTORY_LOCATION] ([LOCATION_ID]),
    INDEX [IDX_HISTORY_APPNAME] ([APP_NAME]),
    INDEX [IDX_HISTORY_CHANGE_DATE] ([CHANGE_DATE])
);
GO
PRINT '✓ LOCATION_APP_MAP_HISTORY table created';
GO

-- ==========================================
-- Index: Extended Indexes for Performance
-- ==========================================
CREATE INDEX [IDX_LOCATION_APP_MAP_ACTIVE] 
    ON [dbo].[LOCATION_APP_MAP]([IS_ACTIVE], [LOCATION_ID])
    WHERE [IS_ACTIVE] = 1;
GO
PRINT '✓ Composite index for active mappings created';
GO

CREATE INDEX [IDX_LOCATION_APP_MAP_CATEGORY] 
    ON [dbo].[LOCATION_APP_MAP]([SITE_CATEGORY_CODE])
    WHERE [SITE_CATEGORY_CODE] IS NOT NULL;
GO
PRINT '✓ Index for site category code created';
GO

-- ==========================================
-- Stored Procedure: usp_AuditLog_Insert
-- Purpose: Insert audit log entry
-- ==========================================
CREATE PROCEDURE [dbo].[usp_AuditLog_Insert]
    @TableName VARCHAR(128),
    @RecordId VARCHAR(500),
    @OperationType VARCHAR(10),
    @OldValues NVARCHAR(MAX) = NULL,
    @NewValues NVARCHAR(MAX) = NULL,
    @ChangedBy VARCHAR(100),
    @IpAddress VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO [dbo].[AUDIT_LOG] 
    ([TABLE_NAME], [RECORD_ID], [OPERATION_TYPE], [OLD_VALUES], [NEW_VALUES], [CHANGED_BY], [IP_ADDRESS])
    VALUES 
    (@TableName, @RecordId, @OperationType, @OldValues, @NewValues, @ChangedBy, @IpAddress);
    
    SELECT SCOPE_IDENTITY() AS [AUDIT_ID];
END;
GO
PRINT '✓ usp_AuditLog_Insert created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_InsertWithAudit
-- Purpose: Insert location app mapping with audit trail
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_InsertWithAudit]
    @LocationId DECIMAL(22,0),
    @AppName VARCHAR(255),
    @SiteCategoryCode BIGINT = NULL,
    @SelfAccess VARCHAR(255) = NULL,
    @DeemedApproval CHAR(1) = NULL,
    @CreatedBy VARCHAR(100) = 'System',
    @IpAddress VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF @LocationId <= 0
        BEGIN
            RAISERROR('Location ID must be greater than zero', 16, 1);
            ROLLBACK;
            RETURN;
        END;
        
        IF @AppName IS NULL OR LEN(@AppName) = 0
        BEGIN
            RAISERROR('Application name cannot be empty', 16, 1);
            ROLLBACK;
            RETURN;
        END;
        
        IF EXISTS(SELECT 1 FROM [dbo].[LOCATION_APP_MAP] 
                  WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName)
        BEGIN
            RAISERROR('Mapping already exists', 16, 1);
            ROLLBACK;
            RETURN;
        END;
        
        -- Insert into main table
        INSERT INTO [dbo].[LOCATION_APP_MAP] 
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
        VALUES 
        (@LocationId, @AppName, @SiteCategoryCode, @SelfAccess, @DeemedApproval, GETUTCDATE(), @CreatedBy, 1);
        
        -- Insert into history table
        INSERT INTO [dbo].[LOCATION_APP_MAP_HISTORY]
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [IS_ACTIVE], [CREATED_DATE], [CREATED_BY], [CHANGE_TYPE])
        VALUES
        (@LocationId, @AppName, @SiteCategoryCode, @SelfAccess, @DeemedApproval, 1, GETUTCDATE(), @CreatedBy, 'INSERT');
        
        -- Log to audit trail
        DECLARE @AuditRecordId   VARCHAR(500)   = CAST(@LocationId AS VARCHAR(50)) + '|' + @AppName;
        DECLARE @AuditNewValues  NVARCHAR(MAX)  = 'LocationId=' + CAST(@LocationId AS VARCHAR(50))
                                                + ', AppName=' + @AppName
                                                + ', SiteCategoryCode=' + ISNULL(CAST(@SiteCategoryCode AS VARCHAR(20)), 'NULL');
        EXEC [dbo].[usp_AuditLog_Insert]
            @TableName    = 'LOCATION_APP_MAP',
            @RecordId     = @AuditRecordId,
            @OperationType = 'INSERT',
            @NewValues    = @AuditNewValues,
            @ChangedBy    = @CreatedBy,
            @IpAddress    = @IpAddress;
        
        COMMIT TRANSACTION;
        SELECT 'Mapping inserted successfully' AS [Message];
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_InsertWithAudit created';
GO

-- ==========================================
-- Stored Procedure: usp_LocationAppMap_UpdateWithAudit
-- Purpose: Update location app mapping with audit trail
-- ==========================================
CREATE PROCEDURE [dbo].[usp_LocationAppMap_UpdateWithAudit]
    @LocationId DECIMAL(22,0),
    @AppName VARCHAR(255),
    @SiteCategoryCode BIGINT = NULL,
    @SelfAccess VARCHAR(255) = NULL,
    @DeemedApproval CHAR(1) = NULL,
    @IsActive BIT = 1,
    @ModifiedBy VARCHAR(100) = 'System',
    @IpAddress VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @OldValues NVARCHAR(MAX);
        DECLARE @NewValues NVARCHAR(MAX);
        
        IF @LocationId <= 0
        BEGIN
            RAISERROR('Location ID must be greater than zero', 16, 1);
            ROLLBACK;
            RETURN;
        END;
        
        IF @AppName IS NULL OR LEN(@AppName) = 0
        BEGIN
            RAISERROR('Application name cannot be empty', 16, 1);
            ROLLBACK;
            RETURN;
        END;
        
        IF NOT EXISTS(SELECT 1 FROM [dbo].[LOCATION_APP_MAP] 
                      WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName)
        BEGIN
            RAISERROR('Mapping not found', 16, 1);
            ROLLBACK;
            RETURN;
        END;
        
        -- Capture old values
        SELECT @OldValues = 'SiteCategoryCode=' + ISNULL(CAST([SITE_CATEGORY_CODE] AS VARCHAR(20)), 'NULL')
                          + ', SelfAccess=' + ISNULL([SELF_ACCESS], 'NULL')
                          + ', DeemedApproval=' + ISNULL([DEEMED_APPROVAL], 'NULL')
                          + ', IsActive=' + CAST([IS_ACTIVE] AS VARCHAR(1))
        FROM [dbo].[LOCATION_APP_MAP]
        WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName;
        
        UPDATE [dbo].[LOCATION_APP_MAP]
        SET 
            [SITE_CATEGORY_CODE] = @SiteCategoryCode,
            [SELF_ACCESS] = @SelfAccess,
            [DEEMED_APPROVAL] = @DeemedApproval,
            [MODIFIED_DATE] = GETUTCDATE(),
            [MODIFIED_BY] = @ModifiedBy,
            [IS_ACTIVE] = @IsActive
        WHERE [LOCATION_ID] = @LocationId AND [APP_NAME] = @AppName;
        
        -- Insert into history table
        INSERT INTO [dbo].[LOCATION_APP_MAP_HISTORY]
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [IS_ACTIVE], [MODIFIED_DATE], [MODIFIED_BY], [CHANGE_TYPE])
        VALUES
        (@LocationId, @AppName, @SiteCategoryCode, @SelfAccess, @DeemedApproval, @IsActive, GETUTCDATE(), @ModifiedBy, 'UPDATE');
        
        -- Set new values
        SET @NewValues = 'SiteCategoryCode=' + ISNULL(CAST(@SiteCategoryCode AS VARCHAR(20)), 'NULL')
                       + ', SelfAccess=' + ISNULL(@SelfAccess, 'NULL')
                       + ', DeemedApproval=' + ISNULL(@DeemedApproval, 'NULL')
                       + ', IsActive=' + CAST(@IsActive AS VARCHAR(1));
        
        -- Log to audit trail
        DECLARE @AuditRecordId2  VARCHAR(500) = CAST(@LocationId AS VARCHAR(50)) + '|' + @AppName;
        EXEC [dbo].[usp_AuditLog_Insert]
            @TableName     = 'LOCATION_APP_MAP',
            @RecordId      = @AuditRecordId2,
            @OperationType = 'UPDATE',
            @OldValues     = @OldValues,
            @NewValues     = @NewValues,
            @ChangedBy     = @ModifiedBy,
            @IpAddress     = @IpAddress;
        
        COMMIT TRANSACTION;
        SELECT 'Mapping updated successfully' AS [Message];
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH;
END;
GO
PRINT '✓ usp_LocationAppMap_UpdateWithAudit created';
GO

PRINT '';
PRINT '========================================';
PRINT 'MIGRATION AND AUDIT TABLES CREATED';
PRINT '========================================';
GO
