-- ==========================================
-- DeductionService - Database Initialization
-- ==========================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DeductionServiceDb')
BEGIN
    CREATE DATABASE [DeductionServiceDb];
    PRINT 'Database DeductionServiceDb created.'
END
ELSE
    PRINT 'Database DeductionServiceDb already exists.'
GO

USE [DeductionServiceDb];
GO

PRINT 'Applying Deduction table definitions...'
GO
:r /sql/Deduction-tables.sql
GO

PRINT 'Applying Deduction stored procedures...'
GO
:r /sql/Deduction-procedures.sql
GO

PRINT 'DeductionServiceDb initialization complete.'
GO
