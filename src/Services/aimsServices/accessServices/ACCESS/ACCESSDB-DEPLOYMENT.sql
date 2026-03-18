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
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'ACCESSDB')
    DROP DATABASE ACCESSDB;
GO

CREATE DATABASE ACCESSDB
ON PRIMARY (
    NAME = 'ACCESSDB_Data',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\ACCESSDB_Data.mdf',
    SIZE = 100MB,
    MAXSIZE = 500MB,
    FILEGROWTH = 10MB
)
LOG ON (
    NAME = 'ACCESSDB_Log',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\ACCESSDB_Log.ldf',
    SIZE = 50MB,
    MAXSIZE = 200MB,
    FILEGROWTH = 5MB
);
GO

PRINT '=== PHASE 1 COMPLETE: Database Created ===';
GO

-- ==========================================
-- PHASE 2: Table Definitions
-- ==========================================
USE ACCESSDB;
GO

-- Table: AIMS_USERMAP - AIMS User Map Master
CREATE TABLE [AIMS_USERMAP] (
    [USER_EMPSYSID] BIGINT NOT NULL,
    [USER_EFFDATE] DATETIME2(3) NULL,
    [USER_CLSDATE] DATETIME2(3) NULL,
    [USER_MODIFIEDBY] BIGINT NULL,
    [USER_MODIFIEDON] DATETIME2(3) NULL,
    CONSTRAINT [PK_AIMS_USERMAP] PRIMARY KEY ([USER_EMPSYSID])
);

-- Table: AIMS_USERROLE - AIMS User Access Role Map
CREATE TABLE [AIMS_USERROLE] (
    [ROLE_ID] INT NOT NULL,
    [ROLE_EMPSYSID] BIGINT NULL,
    [ROLE_TYPE] CHAR(1) NULL,  -- S-SuperUser; U-Unit Access; C- Calendar Access
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

-- Table: MENU_MASTER - AIMS Menu Master
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

-- Table: AIMS_USERMENUMAP - User Role Menu Map
CREATE TABLE [AIMS_USERMENUMAP] (
    [USER_ROLEID] INT NULL,
    [USER_MENUID] INT NULL,
    [USER_MODIFIEDBY] BIGINT NULL,
    [USER_MODIFIEDON] DATETIME2(3) NULL
);

-- Table: SPARSHMENU_MASTER - SPARSH Menu Master
CREATE TABLE [SPARSHMENU_MASTER] (
    [SPARSHMENU_ID] BIGINT NOT NULL,
    [SPARSHMENU_NAME] VARCHAR(200) NOT NULL,
    [SPARSHMENU_PAGENAME] VARCHAR(250) NOT NULL,
    [SPARSHMENU_LASTMODIFIEDBY] BIGINT NOT NULL,
    [SPARSHMENU_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_SPARSHMENU_MASTER] PRIMARY KEY ([SPARSHMENU_ID])
);

-- Table: SPARSHMENU_ACCESS - SPARSH Menu Access
CREATE TABLE [SPARSHMENU_ACCESS] (
    [ACCESS_ID] BIGINT NOT NULL,
    [ACCESS_UNIT] BIGINT NOT NULL,
    [ACCESS_CALENDAR] BIGINT NOT NULL,
    [ACCESS_GRADECATEGORY] CHAR(3) NOT NULL,
    [ACCESS_SPARSHMENUID] BIGINT NOT NULL,
    CONSTRAINT [PK_SPARSHMENU_ACCESS] PRIMARY KEY ([ACCESS_ID])
);

-- Create Indexes
CREATE INDEX [IX_AIMS_USERROLE_EMPSYSID] ON [AIMS_USERROLE] ([ROLE_EMPSYSID]);
CREATE INDEX [IX_MENU_MASTER_PARENT] ON [MENU_MASTER] ([MENU_PARENTID]);
CREATE INDEX [IX_SPARSHMENU_ACCESS_UNIT] ON [SPARSHMENU_ACCESS] ([ACCESS_UNIT]);

PRINT '=== PHASE 2 COMPLETE: Tables Created ===';
GO

-- ==========================================
-- PHASE 3: Data Validation Views
-- ==========================================
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
