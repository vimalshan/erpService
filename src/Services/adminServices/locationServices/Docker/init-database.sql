-- =====================================================
-- Docker init script for LOCATIONDB
-- Runs automatically on first container startup
-- =====================================================

USE MASTER;
GO

-- Create LOCATIONDB database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LOCATIONDB')
BEGIN
    CREATE DATABASE [LOCATIONDB];
    PRINT '+ LOCATIONDB created';
END
ELSE
    PRINT '+ LOCATIONDB already exists';
GO

USE [LOCATIONDB];
GO

-- =====================================================
-- LOCATION_APP_MAP  (base table)
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
    PRINT '+ LOCATION_APP_MAP created';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_APPNAME' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_APPNAME]    ON [dbo].[LOCATION_APP_MAP]([APP_NAME]);
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_LOCATIONID' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_LOCATIONID] ON [dbo].[LOCATION_APP_MAP]([LOCATION_ID]);
GO
SET QUOTED_IDENTIFIER ON;
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_ACTIVE' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_ACTIVE]    ON [dbo].[LOCATION_APP_MAP]([IS_ACTIVE], [LOCATION_ID]) WHERE [IS_ACTIVE] = 1;
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOCATION_APP_MAP_CATEGORY' AND object_id = OBJECT_ID('LOCATION_APP_MAP'))
    CREATE INDEX [IDX_LOCATION_APP_MAP_CATEGORY]  ON [dbo].[LOCATION_APP_MAP]([SITE_CATEGORY_CODE]) WHERE [SITE_CATEGORY_CODE] IS NOT NULL;
GO

-- =====================================================
-- AUDIT_LOG
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
    PRINT '+ AUDIT_LOG created';
END
GO

-- =====================================================
-- LOCATION_APP_MAP_HISTORY
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
        [IS_ACTIVE]          BIT           NOT NULL DEFAULT 1,
        [CHANGED_BY]         VARCHAR(100)  NOT NULL,
        [CHANGED_DATE]       DATETIME      NOT NULL DEFAULT GETUTCDATE(),
        [OPERATION_TYPE]     VARCHAR(10)   NOT NULL
    );
    CREATE INDEX [IDX_HISTORY_LOCATION] ON [dbo].[LOCATION_APP_MAP_HISTORY]([LOCATION_ID]);
    CREATE INDEX [IDX_HISTORY_DATE]     ON [dbo].[LOCATION_APP_MAP_HISTORY]([CHANGED_DATE]);
    PRINT '+ LOCATION_APP_MAP_HISTORY created';
END
GO

-- =====================================================
-- Seed data
-- =====================================================
IF NOT EXISTS (SELECT 1 FROM [dbo].[LOCATION_APP_MAP])
BEGIN
    INSERT INTO [dbo].[LOCATION_APP_MAP]
        ([LOCATION_ID], [APP_NAME], [SITE_CATEGORY_CODE], [SELF_ACCESS], [DEEMED_APPROVAL], [CREATED_DATE], [CREATED_BY], [IS_ACTIVE])
    VALUES
        (1001, 'WarehouseApp',      100, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (1001, 'InventoryApp',      101, 'Y', 'N', GETUTCDATE(), 'Admin', 1),
        (1001, 'ReportingApp',      102, 'N', 'Y', GETUTCDATE(), 'Admin', 1),
        (2001, 'ManufacturingApp',  200, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (2001, 'QualityApp',        201, 'Y', 'N', GETUTCDATE(), 'Admin', 1),
        (3001, 'RetailApp',         300, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (3001, 'POSApp',            301, 'N', 'N', GETUTCDATE(), 'Admin', 1),
        (4001, 'OfficeApp',         400, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (4001, 'HRApp',             401, 'Y', 'Y', GETUTCDATE(), 'Admin', 1),
        (5001, 'DistributionApp',   500, 'Y', 'N', GETUTCDATE(), 'Admin', 1);
    PRINT '+ Seed data inserted (10 rows)';
END
ELSE
    PRINT '+ Seed data already present, skipping';
GO

PRINT '';
PRINT '=== LOCATIONDB initialisation complete ===';
GO
