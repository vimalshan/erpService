-- ==========================================
-- CardManagement Service - Database Initialization
-- ==========================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CardManagementDb')
BEGIN
    CREATE DATABASE [CardManagementDb];
    PRINT 'Database CardManagementDb created.'
END
ELSE
    PRINT 'Database CardManagementDb already exists.'
GO

USE [CardManagementDb];
GO

PRINT 'Applying CardManagement table definitions...'
GO
:r /sql/CardManagement-tables.sql
GO

PRINT 'Applying CardManagement stored procedures...'
GO
:r /sql/CardManagement-procedures.sql
GO

PRINT 'CardManagementDb initialization complete.'
GO
