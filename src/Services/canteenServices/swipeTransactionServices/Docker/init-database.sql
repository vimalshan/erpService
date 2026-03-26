-- ==========================================
-- SwipeTransactionService - Database Initialization
-- ==========================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SwipeTransactionDb')
BEGIN
    CREATE DATABASE [SwipeTransactionDb];
    PRINT 'Database SwipeTransactionDb created.'
END
ELSE
    PRINT 'Database SwipeTransactionDb already exists.'
GO

USE [SwipeTransactionDb];
GO

PRINT 'Applying SwipeTransaction table definitions...'
GO
:r /sql/SwipeTransaction-tables.sql
GO

PRINT 'Applying SwipeTransaction stored procedures...'
GO
:r /sql/SwipeTransaction-procedures.sql
GO

PRINT 'SwipeTransactionDb initialization complete.'
GO
