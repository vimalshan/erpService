-- ==========================================
-- EligibilityService - Database Initialization
-- ==========================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EligibilityServiceDb')
BEGIN
    CREATE DATABASE [EligibilityServiceDb];
    PRINT 'Database EligibilityServiceDb created.'
END
ELSE
    PRINT 'Database EligibilityServiceDb already exists.'
GO

USE [EligibilityServiceDb];
GO

PRINT 'Applying Eligibility table definitions...'
GO
:r /sql/Eligibility-tables.sql
GO

PRINT 'Applying Eligibility stored procedures...'
GO
:r /sql/Eligibility-procedures.sql
GO

PRINT 'EligibilityServiceDb initialization complete.'
GO
