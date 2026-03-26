-- ==========================================
-- CanteenTransactionService - Database Initialization
-- ==========================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CanteenTransactionDb')
BEGIN
    CREATE DATABASE [CanteenTransactionDb];
    PRINT 'Database CanteenTransactionDb created.'
END
ELSE
    PRINT 'Database CanteenTransactionDb already exists.'
GO

USE [CanteenTransactionDb];
GO

PRINT 'Applying CanteenTransaction table definitions...'
GO
:r /sql/CanteenTransaction-tables.sql
GO

PRINT 'Applying CanteenTransaction stored procedures...'
GO
:r /sql/CanteenTransaction-procedures.sql
GO

PRINT 'CanteenTransactionDb initialization complete.'
GO
