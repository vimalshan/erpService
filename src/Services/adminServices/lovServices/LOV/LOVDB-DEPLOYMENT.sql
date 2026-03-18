-- ==========================================
-- LOV MODULE - STANDALONE DATABASE
-- Complete Deployment Script
-- Version: 1.0
-- Generated: 2026-03-09
-- ==========================================

USE MASTER;
GO

PRINT '=== Creating LOVDB ===';
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LOVDB')
BEGIN
    CREATE DATABASE LOVDB
    ON PRIMARY (
        NAME = 'LOVDB_data',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\LOVDB.mdf',
        SIZE = 50MB,
        MAXSIZE = 500MB,
        FILEGROWTH = 10%
    )
    LOG ON (
        NAME = 'LOVDB_log',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\LOVDB.ldf',
        SIZE = 25MB,
        MAXSIZE = 250MB,
        FILEGROWTH = 10%
    );
    PRINT '✓ LOVDB created';
END
GO

USE [LOVDB];
GO

PRINT '';
PRINT '=== Deploying LOV Tables ===';
GO

CREATE TABLE [LOV_TYPE] (
    [LOV_TYPE_ID] BIGINT NOT NULL,
    [LOV_TYPE_NAME] VARCHAR(30) NOT NULL,
    CONSTRAINT [PK_LOV_TYPE] PRIMARY KEY ([LOV_TYPE_ID])
);
PRINT '✓ LOV_TYPE table created';
GO

CREATE TABLE [LOV_MASTER] (
    [LOV_ID] BIGINT NOT NULL,
    [LOV_TYPE_ID] BIGINT NOT NULL,
    [LOV_NAME] VARCHAR(30) NOT NULL,
    [LOV_UPDATED_BY] BIGINT NOT NULL,
    [LOV_UPDATED_ON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LOV_MASTER] PRIMARY KEY ([LOV_ID]),
    CONSTRAINT [FK_LOV_MASTER_TYPE] FOREIGN KEY ([LOV_TYPE_ID]) REFERENCES [LOV_TYPE]([LOV_TYPE_ID])
);
PRINT '✓ LOV_MASTER table created';
GO

CREATE TABLE [ITEMDATA] (
    [CATNAME] VARCHAR(40) NULL,
    [ITEMNAME] VARCHAR(60) NULL,
    [MAKE] VARCHAR(30) NULL,
    [UOM] VARCHAR(20) NULL,
    [PRICE] INT NULL
);
PRINT '✓ ITEMDATA table created';
GO

CREATE INDEX [IDX_LOV_MASTER_TYPEID] ON [LOV_MASTER]([LOV_TYPE_ID]);
PRINT '✓ Index created';
GO

PRINT '';
PRINT '========================================';
PRINT 'LOVDB DEPLOYMENT COMPLETE';
PRINT '========================================';
PRINT '';
PRINT 'Database: LOVDB';
PRINT 'Status: ✓ Successfully Deployed';
PRINT '';
PRINT 'Objects Created:';
PRINT '  ✓ 3 Tables (LOV_TYPE, LOV_MASTER, ITEMDATA)';
PRINT '  ✓ 1 Index';
PRINT '  ✓ 1 Foreign Key';
PRINT '';
PRINT '========================================';
GO

-- ==========================================
-- END OF LOVDB DEPLOYMENT
-- ==========================================
