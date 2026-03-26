-- ==========================================
-- ReferenceDataService - Database Initialization
-- ==========================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ReferenceDataDb')
BEGIN
    CREATE DATABASE [ReferenceDataDb];
    PRINT 'Database ReferenceDataDb created.'
END
ELSE
    PRINT 'Database ReferenceDataDb already exists.'
GO

USE [ReferenceDataDb];
GO

PRINT 'Applying ReferenceData table definitions...'
GO
:r /sql/ReferenceData-tables.sql
GO

PRINT 'Applying ReferenceData stored procedures...'
GO
:r /sql/ReferenceData-procedures.sql
GO

PRINT 'ReferenceDataDb initialization complete.'
GO
