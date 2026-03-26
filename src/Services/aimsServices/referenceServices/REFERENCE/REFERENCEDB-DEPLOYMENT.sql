-- ==========================================
-- Database: REFERENCEDB
-- Module: Reference Data Management System
-- Generated Module Script
-- ==========================================

USE master;
GO

-- ==========================================
-- PHASE 1: Database Creation
-- ==========================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'REFERENCEDB')
BEGIN
    CREATE DATABASE [REFERENCEDB];
END
GO

PRINT '=== PHASE 1 COMPLETE: Database Created ===';
GO

-- ==========================================
-- PHASE 2: Table Definitions
-- ==========================================
USE REFERENCEDB;
GO

-- Table: LOV_TYPEMAST - List of Values Type Master
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOV_TYPEMAST]') AND type = 'U')
BEGIN
CREATE TABLE [LOV_TYPEMAST] (
    [LOV_TYPEID] INT NOT NULL,
    [LOV_TYPENAME] VARCHAR(255) NOT NULL,
    [LOV_DESCRIPTION] VARCHAR(500) NULL,
    [LOV_TYPESEQ] INT NOT NULL,
    [LOV_STATUS] CHAR(1) NOT NULL,
    [LOV_LASTMODIFIEDBY] BIGINT NOT NULL,
    [LOV_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LOV_TYPEMAST] PRIMARY KEY ([LOV_TYPEID]),
    CONSTRAINT [UQ_LOV_TYPENAME] UNIQUE ([LOV_TYPENAME])
);
END

-- Table: LOV_MAST - List of Values Master
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOV_MAST]') AND type = 'U')
BEGIN
CREATE TABLE [LOV_MAST] (
    [LOV_ID] INT NOT NULL,
    [LOV_TYPEID] INT NOT NULL,
    [LOV_CODE] VARCHAR(50) NOT NULL,
    [LOV_DESCRIPTION] VARCHAR(255) NOT NULL,
    [LOV_LONGDESCRIPTION] VARCHAR(500) NULL,
    [LOV_SEQUENCE] INT NOT NULL,
    [LOV_STATUS] CHAR(1) NOT NULL,
    [LOV_LASTMODIFIEDBY] BIGINT NOT NULL,
    [LOV_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LOV_MAST] PRIMARY KEY ([LOV_ID]),
    CONSTRAINT [FK_LOV_TYPEID] FOREIGN KEY ([LOV_TYPEID]) REFERENCES [LOV_TYPEMAST]([LOV_TYPEID]),
    CONSTRAINT [UQ_LOV_CODE] UNIQUE ([LOV_TYPEID], [LOV_CODE])
);
END

-- Table: PROGRAMLOV_MAST - Program-Specific List of Values
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PROGRAMLOV_MAST]') AND type = 'U')
BEGIN
CREATE TABLE [PROGRAMLOV_MAST] (
    [PROGLOV_ID] INT NOT NULL,
    [PROGLOV_LOVID] INT NOT NULL,
    [PROGLOV_PROGRAMID] INT NOT NULL,
    [PROGLOV_ACTIVE] CHAR(1) NOT NULL,
    [PROGLOV_LASTMODIFIEDBY] BIGINT NOT NULL,
    [PROGLOV_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_PROGRAMLOV_MAST] PRIMARY KEY ([PROGLOV_ID]),
    CONSTRAINT [FK_PROGLOV_LOVID] FOREIGN KEY ([PROGLOV_LOVID]) REFERENCES [LOV_MAST]([LOV_ID])
);
END

-- Table: PERMISSION_RULES - Permission Rules Reference
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PERMISSION_RULES]') AND type = 'U')
BEGIN
CREATE TABLE [PERMISSION_RULES] (
    [PERM_ID] INT NOT NULL,
    [PERM_RESOURCEID] VARCHAR(100) NOT NULL,
    [PERM_ACTION] VARCHAR(100) NOT NULL,
    [PERM_DESCRIPTION] VARCHAR(255) NULL,
    [PERM_APPCODE] VARCHAR(50) NOT NULL,
    [PERM_STATUS] CHAR(1) NOT NULL,
    [PERM_LASTMODIFIEDBY] BIGINT NOT NULL,
    [PERM_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_PERMISSION_RULES] PRIMARY KEY ([PERM_ID]),
    CONSTRAINT [UQ_PERMISSION] UNIQUE ([PERM_RESOURCEID], [PERM_ACTION], [PERM_APPCODE])
);
END

-- Table: LEAVEFLAG - Leave Classification Reference
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEAVEFLAG]') AND type = 'U')
BEGIN
CREATE TABLE [LEAVEFLAG] (
    [LEAVEFLAG_ID] INT NOT NULL,
    [LEAVEFLAG_CODE] VARCHAR(10) NOT NULL,
    [LEAVEFLAG_DESCRIPTION] VARCHAR(255) NOT NULL,
    [LEAVEFLAG_TYPE] VARCHAR(50) NULL,
    [LEAVEFLAG_STATUS] CHAR(1) NOT NULL,
    [LEAVEFLAG_LASTMODIFIEDBY] BIGINT NOT NULL,
    [LEAVEFLAG_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LEAVEFLAG] PRIMARY KEY ([LEAVEFLAG_ID]),
    CONSTRAINT [UQ_LEAVEFLAG_CODE] UNIQUE ([LEAVEFLAG_CODE])
);
END

-- Create Indexes
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LOV_TYPEMAST_STATUS' AND object_id = OBJECT_ID('LOV_TYPEMAST'))
    CREATE INDEX [IX_LOV_TYPEMAST_STATUS] ON [LOV_TYPEMAST] ([LOV_STATUS]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LOV_MAST_TYPEID' AND object_id = OBJECT_ID('LOV_MAST'))
    CREATE INDEX [IX_LOV_MAST_TYPEID] ON [LOV_MAST] ([LOV_TYPEID]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LOV_MAST_CODE' AND object_id = OBJECT_ID('LOV_MAST'))
    CREATE INDEX [IX_LOV_MAST_CODE] ON [LOV_MAST] ([LOV_CODE]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PROGRAMLOV_LOVID' AND object_id = OBJECT_ID('PROGRAMLOV_MAST'))
    CREATE INDEX [IX_PROGRAMLOV_LOVID] ON [PROGRAMLOV_MAST] ([PROGLOV_LOVID]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LEAVEFLAG_CODE' AND object_id = OBJECT_ID('LEAVEFLAG'))
    CREATE INDEX [IX_LEAVEFLAG_CODE] ON [LEAVEFLAG] ([LEAVEFLAG_CODE]);

PRINT '=== PHASE 2 COMPLETE: Tables Created ===';
GO

-- ==========================================
-- PHASE 3: Stored Procedures & Functions
-- ==========================================

-- Function: fn_GetLOVDescription
CREATE OR ALTER FUNCTION dbo.fn_GetLOVDescription
(
    @p_LOVID INT
)
RETURNS VARCHAR(255)
AS
BEGIN
    DECLARE @Description VARCHAR(255);
    
    SELECT @Description = LOV_DESCRIPTION
    FROM LOV_MAST
    WHERE LOV_ID = @p_LOVID;
    
    RETURN ISNULL(@Description, 'Invalid LOV');
END;
GO

-- Procedure: usp_CreateLOVType
CREATE OR ALTER PROCEDURE dbo.usp_CreateLOVType
(
    @p_TypeName VARCHAR(255),
    @p_Description VARCHAR(500),
    @p_Sequence INT,
    @p_CreatedBy BIGINT,
    @p_TypeID INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF EXISTS (SELECT 1 FROM LOV_TYPEMAST WHERE LOV_TYPENAME = @p_TypeName)
            THROW 50001, 'LOV Type already exists', 1;
        
        SELECT @p_TypeID = ISNULL(MAX(LOV_TYPEID), 0) + 1 FROM LOV_TYPEMAST;
        
        INSERT INTO LOV_TYPEMAST
        (LOV_TYPEID, LOV_TYPENAME, LOV_DESCRIPTION, LOV_TYPESEQ, LOV_STATUS, LOV_LASTMODIFIEDBY, LOV_LASTMODIFIEDON)
        VALUES (@p_TypeID, @p_TypeName, @p_Description, @p_Sequence, 'Y', @p_CreatedBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'LOV Type created: ID = ' + CAST(@p_TypeID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('LOV Type creation failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- Procedure: usp_CreateLOVValue
CREATE OR ALTER PROCEDURE dbo.usp_CreateLOVValue
(
    @p_TypeID INT,
    @p_Code VARCHAR(50),
    @p_Description VARCHAR(255),
    @p_Sequence INT,
    @p_CreatedBy BIGINT,
    @p_LOVID INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM LOV_TYPEMAST WHERE LOV_TYPEID = @p_TypeID)
            THROW 50002, 'LOV Type not found', 1;
        
        IF EXISTS (SELECT 1 FROM LOV_MAST WHERE LOV_TYPEID = @p_TypeID AND LOV_CODE = @p_Code)
            THROW 50003, 'LOV value already exists for this type', 1;
        
        SELECT @p_LOVID = ISNULL(MAX(LOV_ID), 0) + 1 FROM LOV_MAST;
        
        INSERT INTO LOV_MAST
        (LOV_ID, LOV_TYPEID, LOV_CODE, LOV_DESCRIPTION, LOV_SEQUENCE, LOV_STATUS, LOV_LASTMODIFIEDBY, LOV_LASTMODIFIEDON)
        VALUES (@p_LOVID, @p_TypeID, @p_Code, @p_Description, @p_Sequence, 'Y', @p_CreatedBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'LOV value created: ID = ' + CAST(@p_LOVID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('LOV value creation failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

PRINT '=== PHASE 3 COMPLETE: Procedures and Functions Created ===';
GO

-- ==========================================
-- PHASE 4: Verification Views
-- ==========================================
IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'vw_ReferenceDB_Status')
    DROP VIEW vw_ReferenceDB_Status;
GO
CREATE VIEW vw_ReferenceDB_Status AS
SELECT 
    'REFERENCEDB' AS DatabaseName,
    'Reference Data Management Module' AS ModuleDescription,
    (SELECT COUNT(*) FROM LOV_TYPEMAST) AS LOVTypes,
    (SELECT COUNT(*) FROM LOV_MAST) AS LOVValues,
    (SELECT COUNT(*) FROM PERMISSION_RULES) AS PermissionRules,
    (SELECT COUNT(*) FROM LEAVEFLAG) AS LeaveFlags,
    GETDATE() AS LastChecked;
GO

PRINT '=== PHASE 4 COMPLETE: Views Created ===';
GO

-- ==========================================
-- PHASE 5: Final Verification
-- ==========================================
PRINT '=== PHASE 5: FINAL VERIFICATION ===';
SELECT 'System Tables' AS Object_Type, COUNT(*) AS Count FROM sys.tables;
SELECT 'Indexes' AS Object_Type, COUNT(*) AS Count FROM sys.indexes WHERE object_id > 0;
SELECT 'Procedures' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'P';
SELECT 'Functions' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'FN';
SELECT 'Views' AS Object_Type, COUNT(*) AS Count FROM sys.views;

PRINT '======================================';
PRINT 'REFERENCEDB DEPLOYMENT COMPLETE';
PRINT '======================================';
PRINT 'Database: REFERENCEDB';
PRINT 'Tables: 5';
PRINT 'Indexes: 5';
PRINT 'Procedures: 2';
PRINT 'Functions: 1';
PRINT 'Views: 1';
PRINT 'Status: READY FOR DEPLOYMENT';
PRINT '======================================';
GO
