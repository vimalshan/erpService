-- ==========================================
-- CanteenUnit Service - Database Initialization
-- ==========================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CanteenUnitDb')
BEGIN
    CREATE DATABASE [CanteenUnitDb];
    PRINT 'Database CanteenUnitDb created.'
END
ELSE
    PRINT 'Database CanteenUnitDb already exists.'
GO

USE [CanteenUnitDb];
GO

PRINT 'Applying CanteenUnit table definitions...'
GO
:r /sql/CanteenUnit-tables.sql
GO

PRINT 'Applying CanteenUnit stored procedures...'
GO
:r /sql/CanteenUnit-procedures.sql
GO

PRINT 'CanteenUnitDb initialization complete.'
GO
