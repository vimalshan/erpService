-- ==========================================
-- MODULE: FINYEAR
-- Component: Database Migration Script
-- Description: Database initialization and migration for Financial Year Management
-- Database: ADMINDB
-- Connection String: Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name="FinyearAPI";Command Timeout=0
-- ==========================================

USE master;
GO

-- Check if database exists and create if it doesn't
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ADMINDB')
BEGIN
    CREATE DATABASE [ADMINDB];
    PRINT 'Database ADMINDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database ADMINDB already exists.';
END
GO

USE [ADMINDB];
GO

-- ==========================================
-- MIGRATION: 20260309000000_InitialCreate
-- ==========================================

-- Create FINYEAR_MASTER table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FINYEAR_MASTER')
BEGIN
    CREATE TABLE [FINYEAR_MASTER] (
        [FY_ID] BIGINT NOT NULL,
        [FY_NAME] VARCHAR(27) NOT NULL,
        [FY_STARTDATE] DATETIME2(3) NOT NULL,
        [FY_CLOSEDATE] DATETIME2(3) NOT NULL,
        [FY_UPDATED_BY] BIGINT NOT NULL,
        [FY_UPDATED_ON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_FINYEAR_MASTER] PRIMARY KEY ([FY_ID])
    );
    PRINT 'Table FINYEAR_MASTER created successfully.';
END
ELSE
BEGIN
    PRINT 'Table FINYEAR_MASTER already exists.';
END
GO

-- Create indexes for performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_FINYEAR_STARTDATE' AND object_id = OBJECT_ID('FINYEAR_MASTER'))
BEGIN
    CREATE INDEX [IDX_FINYEAR_STARTDATE] ON [FINYEAR_MASTER]([FY_STARTDATE]);
    PRINT 'Index IDX_FINYEAR_STARTDATE created successfully.';
END
ELSE
BEGIN
    PRINT 'Index IDX_FINYEAR_STARTDATE already exists.';
END
GO

-- Create __EFMigrationsHistory table if it doesn't exist (for EF Core tracking)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
    PRINT 'Table __EFMigrationsHistory created successfully.';
END
GO

-- Record the migration if it doesn't exist
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20260309000000_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260309000000_InitialCreate', '8.0.0');
    PRINT 'Migration 20260309000000_InitialCreate recorded in __EFMigrationsHistory.';
END
GO

-- ==========================================
-- END OF MIGRATION SCRIPT
-- ==========================================

PRINT 'Database migration completed successfully.';
PRINT 'Database: ADMINDB';
PRINT 'Connection String: Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name="FinyearAPI";Command Timeout=0'
GO
