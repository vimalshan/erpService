-- ==========================================
-- ItemMasterService - Database Initialization
-- ==========================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ItemMasterDb')
BEGIN
    CREATE DATABASE [ItemMasterDb];
    PRINT 'Database ItemMasterDb created.'
END
ELSE
    PRINT 'Database ItemMasterDb already exists.'
GO

USE [ItemMasterDb];
GO

PRINT 'Applying ItemMaster table definitions...'
GO
:r /sql/ItemMaster-tables.sql
GO

PRINT 'Applying ItemMaster stored procedures...'
GO
:r /sql/ItemMaster-procedures.sql
GO

PRINT 'ItemMasterDb initialization complete.'
GO
