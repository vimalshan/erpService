-- ============================================
-- PFDB Initial Setup Script
-- Run this on a fresh SQL Server instance
-- ============================================

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PFDB')
BEGIN
    CREATE DATABASE PFDB;
END
GO

USE PFDB;
GO

-- Create application login (for non-SA access in production)
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = N'pfapp')
BEGIN
    CREATE LOGIN pfapp WITH PASSWORD = N'PfApp!Str0ngPassword', DEFAULT_DATABASE = PFDB;
END
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = N'pfapp')
BEGIN
    CREATE USER pfapp FOR LOGIN pfapp;
    ALTER ROLE db_datareader ADD MEMBER pfapp;
    ALTER ROLE db_datawriter ADD MEMBER pfapp;
    GRANT EXECUTE TO pfapp;
END
GO

PRINT 'PFDB initial setup complete.';
GO
