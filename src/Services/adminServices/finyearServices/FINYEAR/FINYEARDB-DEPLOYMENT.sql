-- ==========================================
-- FINYEAR MODULE - STANDALONE DATABASE
-- Complete Deployment Script
-- Version: 1.0
-- Generated: 2026-03-09
-- ==========================================

USE MASTER;
GO

PRINT '=== Creating FINYEARDB ===';
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FINYEARDB')
BEGIN
    CREATE DATABASE FINYEARDB
    ON PRIMARY (
        NAME = 'FINYEARDB_data',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\FINYEARDB.mdf',
        SIZE = 50MB,
        MAXSIZE = 500MB,
        FILEGROWTH = 10%
    )
    LOG ON (
        NAME = 'FINYEARDB_log',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\FINYEARDB.ldf',
        SIZE = 25MB,
        MAXSIZE = 250MB,
        FILEGROWTH = 10%
    );
    PRINT '✓ FINYEARDB created';
END
GO

USE [FINYEARDB];
GO

PRINT '';
PRINT '=== Deploying FINYEAR Tables ===';
GO

CREATE TABLE [FINYEAR_MASTER] (
    [FY_ID] BIGINT NOT NULL,
    [FY_NAME] VARCHAR(27) NOT NULL,
    [FY_STARTDATE] DATETIME2(3) NOT NULL,
    [FY_CLOSEDATE] DATETIME2(3) NOT NULL,
    [FY_UPDATED_BY] BIGINT NOT NULL,
    [FY_UPDATED_ON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_FINYEAR_MASTER] PRIMARY KEY ([FY_ID])
);
PRINT '✓ FINYEAR_MASTER table created';
GO

CREATE INDEX [IDX_FINYEAR_STARTDATE] ON [FINYEAR_MASTER]([FY_STARTDATE]);
PRINT '✓ Index created';
GO

PRINT '';
PRINT '=== Deploying FINYEAR Procedures ===';
GO

CREATE OR ALTER PROCEDURE dbo.usp_GetCurrentFinancialYear
(
    @p_FinYearID BIGINT OUTPUT,
    @p_FinYearName VARCHAR(27) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT TOP 1
            @p_FinYearID = FY_ID,
            @p_FinYearName = FY_NAME
        FROM FINYEAR_MASTER
        WHERE GETDATE() BETWEEN FY_STARTDATE AND FY_CLOSEDATE;

        IF @p_FinYearID IS NULL
        BEGIN
            RAISERROR('No active financial year found.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
PRINT '✓ usp_GetCurrentFinancialYear procedure created';
GO

CREATE OR ALTER PROCEDURE dbo.usp_AddFinancialYear
(
    @p_FY_ID BIGINT,
    @p_FY_NAME VARCHAR(27),
    @p_FY_STARTDATE DATETIME2(3),
    @p_FY_CLOSEDATE DATETIME2(3),
    @p_UpdatedBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS (SELECT 1 FROM FINYEAR_MASTER WHERE FY_ID = @p_FY_ID)
        BEGIN
            RAISERROR('Financial Year with ID %d already exists.', 16, 1, @p_FY_ID);
            RETURN;
        END

        INSERT INTO FINYEAR_MASTER
        (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
        VALUES (@p_FY_ID, @p_FY_NAME, @p_FY_STARTDATE, @p_FY_CLOSEDATE, @p_UpdatedBy, GETDATE());

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '✓ usp_AddFinancialYear procedure created';
GO

PRINT '';
PRINT '========================================';
PRINT 'FINYEARDB DEPLOYMENT COMPLETE';
PRINT '========================================';
PRINT '';
PRINT 'Database: FINYEARDB';
PRINT 'Status: ✓ Successfully Deployed';
PRINT '';
PRINT 'Objects Created:';
PRINT '  ✓ 1 Table (FINYEAR_MASTER)';
PRINT '  ✓ 1 Index';
PRINT '  ✓ 2 Procedures';
PRINT '';
PRINT '========================================';
GO

-- ==========================================
-- END OF FINYEARDB DEPLOYMENT
-- ==========================================
