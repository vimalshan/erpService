-- ============================================================
-- Email Notification Database Migration Script
-- Generated: March 12, 2026
-- Purpose: Creates EmailNotificationDb database and schema
-- ============================================================

USE master;
GO

-- Drop database if it exists (use with caution in production)
-- EXECUTE msdb.dbo.sp_dropdb 'EmailNotificationDb';

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EmailNotificationDb')
BEGIN
    CREATE DATABASE EmailNotificationDb;
    PRINT 'Database [EmailNotificationDb] created successfully.';
END
GO

USE EmailNotificationDb;
GO

-- ============================================================
-- Create Tables
-- ============================================================

-- Table: EMAIL_TYPEMAST (Email Type Master)
-- Purpose: Stores email alert type definitions
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EMAIL_TYPEMAST')
BEGIN
    CREATE TABLE EMAIL_TYPEMAST
    (
        EMAIL_TYPEID        BIGINT          PRIMARY KEY IDENTITY(1,1) NOT NULL,
        EMAIL_NAME          VARCHAR(500)    NOT NULL,
        EMAIL_TYPE          CHAR(1)         NOT NULL,  -- 'D'=Daily, 'E'=Event
        EMAIL_PRCNAME       VARCHAR(100)    NOT NULL,
        EMAIL_MODIFIEDBY    DECIMAL(19,0)   NOT NULL,
        EMAIL_MODIFIEDON    DATETIME2(3)    NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT CK_EMAIL_TYPE CHECK (EMAIL_TYPE IN ('D', 'E'))
    );
    PRINT 'Table [EMAIL_TYPEMAST] created successfully.';
END
GO

-- Table: MAIL_ACCESS (Mail Access/Recipients)
-- Purpose: Stores email recipient information with org/business filtering
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'MAIL_ACCESS')
BEGIN
    CREATE TABLE MAIL_ACCESS
    (
        MAIL_ACCESSID       BIGINT          PRIMARY KEY IDENTITY(1,1) NOT NULL,
        MAIL_TYPEID         BIGINT          NOT NULL,
        MAIL_ORGID          BIGINT          NULL,           -- NULL or 0 = All orgs
        MAIL_BUSINESSID     BIGINT          NULL,           -- NULL or 0 = All business units
        MAIL_EMPSYSID       BIGINT          NULL,           -- Employee system ID
        MAIL_EMAILID        VARCHAR(200)    NOT NULL,
        MAIL_NAME           VARCHAR(100)    NULL,           -- For non-employees
        MAIL_MODIFIEDBY     DECIMAL(19,0)   NOT NULL,
        MAIL_MODIFIEDON     DATETIME2(3)    NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_MAIL_ACCESS_EMAIL_TYPE FOREIGN KEY (MAIL_TYPEID) 
            REFERENCES EMAIL_TYPEMAST(EMAIL_TYPEID) ON DELETE CASCADE
    );
    PRINT 'Table [MAIL_ACCESS] created successfully.';
END
GO

-- ============================================================
-- Create Indexes
-- ============================================================

-- Index for EMAIL_TYPEMAST - Search by type
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_EMAIL_TYPEMAST_TYPE')
BEGIN
    CREATE INDEX IX_EMAIL_TYPEMAST_TYPE ON EMAIL_TYPEMAST (EMAIL_TYPE);
    PRINT 'Index [IX_EMAIL_TYPEMAST_TYPE] created successfully.';
END
GO

-- Index for MAIL_ACCESS - Search by type
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MAIL_ACCESS_TYPEID')
BEGIN
    CREATE INDEX IX_MAIL_ACCESS_TYPEID ON MAIL_ACCESS (MAIL_TYPEID);
    PRINT 'Index [IX_MAIL_ACCESS_TYPEID] created successfully.';
END
GO

-- Index for MAIL_ACCESS - Search by email
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MAIL_ACCESS_EMAIL')
BEGIN
    CREATE INDEX IX_MAIL_ACCESS_EMAIL ON MAIL_ACCESS (MAIL_EMAILID);
    PRINT 'Index [IX_MAIL_ACCESS_EMAIL] created successfully.';
END
GO

-- Index for MAIL_ACCESS - Search by org/business
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MAIL_ACCESS_ORGBUS')
BEGIN
    CREATE INDEX IX_MAIL_ACCESS_ORGBUS ON MAIL_ACCESS (MAIL_ORGID, MAIL_BUSINESSID);
    PRINT 'Index [IX_MAIL_ACCESS_ORGBUS] created successfully.';
END
GO

-- Index for MAIL_ACCESS - Search by employee
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MAIL_ACCESS_EMPID')
BEGIN
    CREATE INDEX IX_MAIL_ACCESS_EMPID ON MAIL_ACCESS (MAIL_EMPSYSID);
    PRINT 'Index [IX_MAIL_ACCESS_EMPID] created successfully.';
END
GO

-- ============================================================
-- Create Stored Procedures
-- ============================================================

-- Stored Procedure: Get email configuration for org and business
CREATE OR ALTER PROCEDURE usp_GetEmailConfig
    @EmailTypeId BIGINT,
    @OrgId BIGINT = NULL,
    @BusinessId BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        et.EMAIL_TYPEID,
        et.EMAIL_NAME,
        et.EMAIL_TYPE,
        et.EMAIL_PRCNAME,
        ma.MAIL_EMAILID AS RecipientEmail,
        ma.MAIL_NAME AS RecipientName
    FROM EMAIL_TYPEMAST et
    LEFT JOIN MAIL_ACCESS ma ON et.EMAIL_TYPEID = ma.MAIL_TYPEID
    WHERE et.EMAIL_TYPEID = @EmailTypeId
        AND (ma.MAIL_ORGID IS NULL OR ma.MAIL_ORGID = 0 OR ma.MAIL_ORGID = @OrgId)
        AND (ma.MAIL_BUSINESSID IS NULL OR ma.MAIL_BUSINESSID = 0 OR @BusinessId IS NULL OR ma.MAIL_BUSINESSID = @BusinessId)
    ORDER BY ma.MAIL_EMAILID;
END
GO

-- Stored Procedure: Get all daily emails (for scheduled processing)
CREATE OR ALTER PROCEDURE usp_GetDailyEmailConfigs
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT DISTINCT
        EMAIL_TYPEID,
        EMAIL_NAME,
        EMAIL_PRCNAME
    FROM EMAIL_TYPEMAST
    WHERE EMAIL_TYPE = 'D'
    ORDER BY EMAIL_TYPEID;
END
GO

-- Stored Procedure: Get all event-triggered emails
CREATE OR ALTER PROCEDURE usp_GetEventEmailConfigs
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT DISTINCT
        EMAIL_TYPEID,
        EMAIL_NAME,
        EMAIL_PRCNAME
    FROM EMAIL_TYPEMAST
    WHERE EMAIL_TYPE = 'E'
    ORDER BY EMAIL_TYPEID;
END
GO

-- ============================================================
-- Insert Sample Data (Optional)
-- ============================================================

-- Uncomment and modify for your environment
/*
INSERT INTO EMAIL_TYPEMAST (EMAIL_NAME, EMAIL_TYPE, EMAIL_PRCNAME, EMAIL_MODIFIEDBY)
VALUES 
    ('Daily Treasury Report', 'D', 'usp_GenerateTreasuryReport', 1),
    ('Trade Confirmation', 'E', 'usp_SendTradeConfirmation', 1),
    ('Daily Settlement Report', 'D', 'usp_GenerateSettlementReport', 1),
    ('Event Alert', 'E', 'usp_GenerateEventAlert', 1);

-- Sample recipients
-- For EmailTypeId = 1, all organizations and business units
INSERT INTO MAIL_ACCESS (MAIL_TYPEID, MAIL_ORGID, MAIL_BUSINESSID, MAIL_EMAILID, MAIL_NAME, MAIL_MODIFIEDBY)
VALUES 
    (1, NULL, NULL, 'treasury@bank.com', 'Treasury Team', 1),
    (1, 1, NULL, 'manager@bank.com', 'Branch Manager', 1),
    (1, 1, 1, 'supervisor@bank.com', 'Supervisor', 1);
*/

-- ============================================================
-- Database Initialization Complete
-- ============================================================

PRINT '====================================================';
PRINT 'Email Notification Database Setup Complete!';
PRINT '====================================================';
PRINT 'Tables Created:';
PRINT '  - EMAIL_TYPEMAST';
PRINT '  - MAIL_ACCESS';
PRINT '';
PRINT 'Indexes Created:';
PRINT '  - IX_EMAIL_TYPEMAST_TYPE';
PRINT '  - IX_MAIL_ACCESS_TYPEID';
PRINT '  - IX_MAIL_ACCESS_EMAIL';
PRINT '  - IX_MAIL_ACCESS_ORGBUS';
PRINT '  - IX_MAIL_ACCESS_EMPID';
PRINT '';
PRINT 'Stored Procedures Created:';
PRINT '  - usp_GetEmailConfig';
PRINT '  - usp_GetDailyEmailConfigs';
PRINT '  - usp_GetEventEmailConfigs';
PRINT '====================================================';
GO
