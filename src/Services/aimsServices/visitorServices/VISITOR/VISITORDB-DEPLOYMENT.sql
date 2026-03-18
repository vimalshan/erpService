-- ==========================================
-- Database: VISITORDB
-- Module: Visitor Management System
-- Generated Module Script
-- ==========================================

USE master;
GO

-- ==========================================
-- PHASE 1: Database Creation
-- ==========================================
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'VISITORDB')
    DROP DATABASE VISITORDB;
GO

CREATE DATABASE VISITORDB
ON PRIMARY (
    NAME = 'VISITORDB_Data',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\VISITORDB_Data.mdf',
    SIZE = 80MB,
    MAXSIZE = 400MB,
    FILEGROWTH = 8MB
)
LOG ON (
    NAME = 'VISITORDB_Log',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\VISITORDB_Log.ldf',
    SIZE = 30MB,
    MAXSIZE = 100MB,
    FILEGROWTH = 4MB
);
GO

PRINT '=== PHASE 1 COMPLETE: Database Created ===';
GO

-- ==========================================
-- PHASE 2: Table Definitions
-- ==========================================
USE VISITORDB;
GO

-- Table: VISITOR_MAIN - Visitor Main Record
CREATE TABLE [VISITOR_MAIN] (
    [VISITOR_ID] BIGINT NOT NULL,
    [VISITOR_NAME] VARCHAR(255) NOT NULL,
    [VISITOR_IDTYPE] CHAR(1) NOT NULL,
    [VISITOR_IDNUMBER] VARCHAR(50) NULL,
    [VISITOR_PHONENUMBER] VARCHAR(20) NULL,
    [VISITOR_EMAIL] VARCHAR(255) NULL,
    [VISITOR_COMPANY] VARCHAR(255) NULL,
    [VISITOR_PURPOSE] VARCHAR(500) NULL,
    [VISITOR_CHECKINTIME] DATETIME2(3) NOT NULL,
    [VISITOR_CHECKOUTTIME] DATETIME2(3) NULL,
    [VISITOR_STATUS] CHAR(1) NOT NULL,
    [VISITOR_WHOMTOVISIT] BIGINT NOT NULL,
    [VISITOR_ENTEREDON] DATETIME2(3) NOT NULL,
    [VISITOR_ENTEREDBY] BIGINT NOT NULL,
    [VISITOR_LASTMODIFIEDBY] BIGINT NOT NULL,
    [VISITOR_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_VISITOR_MAIN] PRIMARY KEY ([VISITOR_ID])
);

-- Table: VISITOR_ITEM - Visitor Items/Articles Details
CREATE TABLE [VISITOR_ITEM] (
    [ITEM_ID] BIGINT NOT NULL,
    [ITEM_VISITORID] BIGINT NOT NULL,
    [ITEM_DESCRIPTION] VARCHAR(255) NOT NULL,
    [ITEM_QUANTITY] INT NOT NULL,
    [ITEM_MATERIALTYPE] VARCHAR(100) NULL,
    [ITEM_NOTES] VARCHAR(500) NULL,
    [ITEM_STATUS] CHAR(1) NOT NULL,
    [ITEM_ENTEREDON] DATETIME2(3) NOT NULL,
    [ITEM_ENTEREDBY] BIGINT NOT NULL,
    CONSTRAINT [PK_VISITOR_ITEM] PRIMARY KEY ([ITEM_ID]),
    CONSTRAINT [FK_VISITOR_ITEM_MAIN] FOREIGN KEY ([ITEM_VISITORID]) REFERENCES [VISITOR_MAIN]([VISITOR_ID])
);

-- Table: VISITOR_APPREQUEST - Visitor Approval Request
CREATE TABLE [VISITOR_APPREQUEST] (
    [VREQ_ID] BIGINT NOT NULL,
    [VREQ_VISITORID] BIGINT NOT NULL,
    [VREQ_REQUIREDAPPROVERID] BIGINT NOT NULL,
    [VREQ_APPROVALSTATUS] CHAR(1) NOT NULL,
    [VREQ_APPROVALDATE] DATETIME2(3) NULL,
    [VREQ_APPROVALREMARKS] VARCHAR(500) NULL,
    [VREQ_REQUESTEDON] DATETIME2(3) NOT NULL,
    [VREQ_REQUESTEDBY] BIGINT NOT NULL,
    [VREQ_LASTMODIFIEDBY] BIGINT NOT NULL,
    [VREQ_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_VISITOR_APPREQUEST] PRIMARY KEY ([VREQ_ID]),
    CONSTRAINT [FK_APPREQUEST_VISITOR] FOREIGN KEY ([VREQ_VISITORID]) REFERENCES [VISITOR_MAIN]([VISITOR_ID])
);

-- Create Indexes
CREATE INDEX [IX_VISITOR_MAIN_CHECKINTIME] ON [VISITOR_MAIN] ([VISITOR_CHECKINTIME]);
CREATE INDEX [IX_VISITOR_MAIN_STATUS] ON [VISITOR_MAIN] ([VISITOR_STATUS]);
CREATE INDEX [IX_VISITOR_ITEM_VISITORID] ON [VISITOR_ITEM] ([ITEM_VISITORID]);
CREATE INDEX [IX_VISITOR_APPREQUEST_VISITORID] ON [VISITOR_APPREQUEST] ([VREQ_VISITORID]);
CREATE INDEX [IX_VISITOR_APPREQUEST_STATUS] ON [VISITOR_APPREQUEST] ([VREQ_APPROVALSTATUS]);

PRINT '=== PHASE 2 COMPLETE: Tables Created ===';
GO

-- ==========================================
-- PHASE 3: Stored Procedures
-- ==========================================

-- Procedure: usp_RegisterVisitor
CREATE OR ALTER PROCEDURE dbo.usp_RegisterVisitor
(
    @p_VisitorName VARCHAR(255),
    @p_IDType CHAR(1),
    @p_IDNumber VARCHAR(50),
    @p_PhoneNumber VARCHAR(20),
    @p_Email VARCHAR(255),
    @p_Company VARCHAR(255),
    @p_Purpose VARCHAR(500),
    @p_WhomToVisit BIGINT,
    @p_EnteredBy BIGINT,
    @p_VisitorID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF LEN(LTRIM(RTRIM(@p_VisitorName))) = 0
            THROW 50001, 'Visitor name is required', 1;
        
        SELECT @p_VisitorID = ISNULL(MAX(VISITOR_ID), 0) + 1 FROM VISITOR_MAIN;
        
        INSERT INTO VISITOR_MAIN
        (VISITOR_ID, VISITOR_NAME, VISITOR_IDTYPE, VISITOR_IDNUMBER, VISITOR_PHONENUMBER, VISITOR_EMAIL,
         VISITOR_COMPANY, VISITOR_PURPOSE, VISITOR_CHECKINTIME, VISITOR_STATUS, VISITOR_WHOMTOVISIT,
         VISITOR_ENTEREDON, VISITOR_ENTEREDBY, VISITOR_LASTMODIFIEDBY, VISITOR_LASTMODIFIEDON)
        VALUES (@p_VisitorID, @p_VisitorName, @p_IDType, @p_IDNumber, @p_PhoneNumber, @p_Email,
                @p_Company, @p_Purpose, GETDATE(), 'I', @p_WhomToVisit,
                GETDATE(), @p_EnteredBy, @p_EnteredBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'Visitor registered: ID = ' + CAST(@p_VisitorID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Visitor registration failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- Procedure: usp_CheckoutVisitor
CREATE OR ALTER PROCEDURE dbo.usp_CheckoutVisitor
(
    @p_VisitorID BIGINT,
    @p_CheckedOutBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM VISITOR_MAIN WHERE VISITOR_ID = @p_VisitorID)
            THROW 50002, 'Visitor not found', 1;
        
        UPDATE VISITOR_MAIN
        SET VISITOR_CHECKOUTTIME = GETDATE(),
            VISITOR_STATUS = 'O',
            VISITOR_LASTMODIFIEDBY = @p_CheckedOutBy,
            VISITOR_LASTMODIFIEDON = GETDATE()
        WHERE VISITOR_ID = @p_VisitorID;
        
        COMMIT TRANSACTION;
        PRINT 'Visitor checked out: ID = ' + CAST(@p_VisitorID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Visitor checkout failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

PRINT '=== PHASE 3 COMPLETE: Procedures Created ===';
GO

-- ==========================================
-- PHASE 4: Verification Views
-- ==========================================
CREATE VIEW vw_VisitorDB_Status AS
SELECT 
    'VISITORDB' AS DatabaseName,
    'Visitor Management Module' AS ModuleDescription,
    (SELECT COUNT(*) FROM VISITOR_MAIN) AS TotalVisitors,
    (SELECT COUNT(*) FROM VISITOR_MAIN WHERE VISITOR_STATUS = 'I') AS CurrentlyInside,
    (SELECT COUNT(*) FROM VISITOR_ITEM) AS ItemsTracked,
    (SELECT COUNT(*) FROM VISITOR_APPREQUEST WHERE VREQ_APPROVALSTATUS = 'P') AS PendingApprovals,
    GETDATE() AS LastChecked;
GO

PRINT '=== PHASE 4 COMPLETE: Views Created ===';
GO

-- ==========================================
-- PHASE 5: Final Verification
-- ==========================================
PRINT '=== PHASE 5: FINAL VERIFICATION ===';
SELECT 'System Tables' AS Object_Type, COUNT(*) AS Count FROM sys.tables;
SELECT 'Indexes' AS Object_Type, COUNT(*) AS Count FROM sys.indexes WHERE object_id > 0;
SELECT 'Procedures' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'P';
SELECT 'Views' AS Object_Type, COUNT(*) AS Count FROM sys.views;

PRINT '======================================';
PRINT 'VISITORDB DEPLOYMENT COMPLETE';
PRINT '======================================';
PRINT 'Database: VISITORDB';
PRINT 'Tables: 3';
PRINT 'Indexes: 5';
PRINT 'Procedures: 2';
PRINT 'Views: 1';
PRINT 'Status: READY FOR DEPLOYMENT';
PRINT '======================================';
GO
