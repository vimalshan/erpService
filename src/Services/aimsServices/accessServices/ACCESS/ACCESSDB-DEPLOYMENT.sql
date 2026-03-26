-- ==========================================
-- Database: ACCESSDB
-- Module: User Access Management
-- Generated Module Script
-- ==========================================

USE master;
GO

-- ==========================================
-- PHASE 1: Database Creation
-- ==========================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ACCESSDB')
BEGIN
    CREATE DATABASE [ACCESSDB];
    PRINT '+ ACCESSDB created';
END
ELSE
    PRINT '= ACCESSDB already exists';
GO

ALTER DATABASE [ACCESSDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ==========================================
-- PHASE 2: Table Definitions
-- ==========================================
USE ACCESSDB;
GO

-- Table: AIMS_USERMAP - AIMS User Map Master
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AIMS_USERMAP]') AND type = 'U')
BEGIN
    CREATE TABLE [AIMS_USERMAP] (
        [USER_EMPSYSID] BIGINT NOT NULL,
        [USER_EFFDATE] DATETIME2(3) NULL,
        [USER_CLSDATE] DATETIME2(3) NULL,
        [USER_MODIFIEDBY] BIGINT NULL,
        [USER_MODIFIEDON] DATETIME2(3) NULL,
        CONSTRAINT [PK_AIMS_USERMAP] PRIMARY KEY ([USER_EMPSYSID])
    );
    PRINT '+ AIMS_USERMAP created';
END
ELSE
    PRINT '= AIMS_USERMAP already exists';
GO

-- Table: AIMS_USERROLE - AIMS User Access Role Map
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AIMS_USERROLE]') AND type = 'U')
BEGIN
    CREATE TABLE [AIMS_USERROLE] (
        [ROLE_ID] INT NOT NULL,
        [ROLE_EMPSYSID] BIGINT NULL,
        [ROLE_TYPE] CHAR(1) NULL,  -- S-SuperUser; U-Unit Access; C-Calendar Access
        [ROLE_MENUACCESS] CHAR(1) NULL,  -- All Menus / View Menus only / Specific Menus
        [ROLE_ORGID] INT NULL,
        [ROLE_UNITID] INT NULL,
        [ROLE_CALENDARID] BIGINT NULL,
        [ROLE_EFFDATE] DATETIME2(3) NULL,
        [ROLE_CLSDATE] DATETIME2(3) NULL,
        [ROLE_MODIFIEDBY] BIGINT NULL,
        [ROLE_MODIFIEDON] DATETIME2(3) NULL,
        CONSTRAINT [PK_AIMS_USERROLE] PRIMARY KEY ([ROLE_ID])
    );
    PRINT '+ AIMS_USERROLE created';
END
ELSE
    PRINT '= AIMS_USERROLE already exists';
GO

-- Table: MENU_MASTER - AIMS Menu Master
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MENU_MASTER]') AND type = 'U')
BEGIN
    CREATE TABLE [MENU_MASTER] (
        [MENU_ID] INT NOT NULL,
        [Menu_NAME] VARCHAR(100) NULL,
        [MENU_PARENTID] INT NULL,
        [Menu_PATH] VARCHAR(150) NULL,
        [MENU_CALENDARROLE] CHAR(1) NULL,
        [MENU_TYPE] CHAR(1) NULL,
        [MENU_DISPLAYORDER] INT NULL,
        [MENU_MODIFIEDBY] BIGINT NULL,
        [MENU_MODIFIEDON] DATETIME2(3) NULL,
        CONSTRAINT [PK_MENU_MASTER] PRIMARY KEY ([MENU_ID])
    );
    PRINT '+ MENU_MASTER created';
END
ELSE
    PRINT '= MENU_MASTER already exists';
GO

-- Table: AIMS_USERMENUMAP - User Role Menu Map
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AIMS_USERMENUMAP]') AND type = 'U')
BEGIN
    CREATE TABLE [AIMS_USERMENUMAP] (
        [USER_ROLEID] INT NULL,
        [USER_MENUID] INT NULL,
        [USER_MODIFIEDBY] BIGINT NULL,
        [USER_MODIFIEDON] DATETIME2(3) NULL
    );
    PRINT '+ AIMS_USERMENUMAP created';
END
ELSE
    PRINT '= AIMS_USERMENUMAP already exists';
GO

-- Table: SPARSHMENU_MASTER - SPARSH Menu Master
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SPARSHMENU_MASTER]') AND type = 'U')
BEGIN
    CREATE TABLE [SPARSHMENU_MASTER] (
        [SPARSHMENU_ID] BIGINT NOT NULL,
        [SPARSHMENU_NAME] VARCHAR(200) NOT NULL,
        [SPARSHMENU_PAGENAME] VARCHAR(250) NOT NULL,
        [SPARSHMENU_LASTMODIFIEDBY] BIGINT NOT NULL,
        [SPARSHMENU_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_SPARSHMENU_MASTER] PRIMARY KEY ([SPARSHMENU_ID])
    );
    PRINT '+ SPARSHMENU_MASTER created';
END
ELSE
    PRINT '= SPARSHMENU_MASTER already exists';
GO

-- Table: SPARSHMENU_ACCESS - SPARSH Menu Access
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SPARSHMENU_ACCESS]') AND type = 'U')
BEGIN
    CREATE TABLE [SPARSHMENU_ACCESS] (
        [ACCESS_ID] BIGINT NOT NULL,
        [ACCESS_UNIT] BIGINT NOT NULL,
        [ACCESS_CALENDAR] BIGINT NOT NULL,
        [ACCESS_GRADECATEGORY] CHAR(3) NOT NULL,
        [ACCESS_SPARSHMENUID] BIGINT NOT NULL,
        CONSTRAINT [PK_SPARSHMENU_ACCESS] PRIMARY KEY ([ACCESS_ID])
    );
    PRINT '+ SPARSHMENU_ACCESS created';
END
ELSE
    PRINT '= SPARSHMENU_ACCESS already exists';
GO

-- Create Indexes (idempotent)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AIMS_USERROLE_EMPSYSID' AND object_id = OBJECT_ID('AIMS_USERROLE'))
    CREATE INDEX [IX_AIMS_USERROLE_EMPSYSID] ON [AIMS_USERROLE] ([ROLE_EMPSYSID]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_MENU_MASTER_PARENT' AND object_id = OBJECT_ID('MENU_MASTER'))
    CREATE INDEX [IX_MENU_MASTER_PARENT] ON [MENU_MASTER] ([MENU_PARENTID]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SPARSHMENU_ACCESS_UNIT' AND object_id = OBJECT_ID('SPARSHMENU_ACCESS'))
    CREATE INDEX [IX_SPARSHMENU_ACCESS_UNIT] ON [SPARSHMENU_ACCESS] ([ACCESS_UNIT]);
GO

PRINT '=== PHASE 2 COMPLETE: Tables Created ===';
GO

-- ==========================================
-- PHASE 3: Data Validation Views
-- ==========================================
IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'vw_AccessDB_Status')
    DROP VIEW vw_AccessDB_Status;
GO

CREATE VIEW vw_AccessDB_Status AS
SELECT 
    'ACCESSDB' AS DatabaseName,
    'Access Management Module' AS ModuleDescription,
    (SELECT COUNT(*) FROM AIMS_USERMAP) AS UserMappings,
    (SELECT COUNT(*) FROM AIMS_USERROLE) AS UserRoles,
    (SELECT COUNT(*) FROM MENU_MASTER) AS MenuItems,
    (SELECT COUNT(*) FROM SPARSHMENU_MASTER) AS SPARSHMenuItems,
    GETDATE() AS LastChecked;
GO

-- ==========================================
-- PHASE 4: Stored Procedures & Functions
-- ==========================================
-- No procedures defined for ACCESS module at this stage
-- Can be added later based on business requirements

PRINT '=== PHASE 4 COMPLETE: Views Created ===';
GO

-- ==========================================
-- PHASE 5: Final Verification
-- ==========================================
PRINT '=== PHASE 5: FINAL VERIFICATION ===';
SELECT 'System Tables' AS Object_Type, COUNT(*) AS Count FROM sys.tables;
SELECT 'Indexes' AS Object_Type, COUNT(*) AS Count FROM sys.indexes WHERE object_id > 0;
SELECT 'Views' AS Object_Type, COUNT(*) AS Count FROM sys.views;

PRINT '======================================';
PRINT 'ACCESSDB DEPLOYMENT COMPLETE';
PRINT '======================================';
PRINT 'Database: ACCESSDB';
PRINT 'Tables: 6';
PRINT 'Indexes: 3';
PRINT 'Views: 1';
PRINT 'Status: READY FOR DEPLOYMENT';
PRINT '======================================';
GO
