-- ==========================================
-- Module: EmailNotification
-- Purpose: Email Configuration and Notification Management
-- Created: March 9, 2026
-- Database: CASHDB
-- ==========================================

USE CASHDB;
GO

-- =====================================================
-- CREATE TABLES FOR EMAIL NOTIFICATION MODULE
-- =====================================================

-- Table: EMAIL_TYPEMAST - Email Type Master
CREATE TABLE [EMAIL_TYPEMAST] (
    [EMAIL_TYPEID] BIGINT NOT NULL  -- EMail Type ID,
    [EMAIL_NAME] VARCHAR(500) NOT NULL  -- EMail Name,
    [EMAIL_TYPE] CHAR(1) NOT NULL  -- D - Daily Alert / E - Event Alert,
    [EMAIL_PRCNAME] VARCHAR(100) NOT NULL  -- Email Procedure Name,
    [EMAIL_MODIFIEDBY] DECIMAL(19,0) NOT NULL  -- Modified by,
    [EMAIL_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified on,
    CONSTRAINT [PK_EMAIL_TYPEMAST] PRIMARY KEY ([EMAIL_TYPEID])
);

-- Table: MAIL_ACCESS - Mail Access Control
CREATE TABLE [MAIL_ACCESS] (
    [MAIL_ACCESSID] BIGINT NOT NULL  -- Mail Access ID,
    [MAIL_TYPEID] BIGINT NOT NULL  -- Email Type ID,
    [MAIL_ORGID] BIGINT NULL  -- 0 - All / Specific Org ID,
    [MAIL_BUSINESSID] BIGINT NULL  -- 0 – All / Specific Business ID,
    [MAIL_EMPSYSID] BIGINT NULL  -- Employee SysID,
    [MAIL_EMAILID] VARCHAR(200) NOT NULL  -- Email ID,
    [MAIL_MODIFIEDBY] DECIMAL(19,0) NOT NULL  -- Last Modified by,
    [MAIL_MODIFIEDON] DATETIME2(3) NOT NULL  -- Last Modified On,
    [MAIL_NAME] VARCHAR(100) NULL  -- Name of Non Employee,
    CONSTRAINT [PK_MAIL_ACCESS] PRIMARY KEY ([MAIL_ACCESSID]),
    CONSTRAINT [FK_MAIL_ACCESS_TYPEMAST] FOREIGN KEY ([MAIL_TYPEID]) REFERENCES [EMAIL_TYPEMAST]([EMAIL_TYPEID])
);

-- =====================================================
-- CREATE INDEXES
-- =====================================================

CREATE INDEX [IX_EMAIL_TYPEMAST_TYPE] ON [EMAIL_TYPEMAST] ([EMAIL_TYPE]);
CREATE INDEX [IX_MAIL_ACCESS_TYPEID] ON [MAIL_ACCESS] ([MAIL_TYPEID]);
CREATE INDEX [IX_MAIL_ACCESS_EMAILID] ON [MAIL_ACCESS] ([MAIL_EMAILID]);
CREATE INDEX [IX_MAIL_ACCESS_ORGID] ON [MAIL_ACCESS] ([MAIL_ORGID]);
CREATE INDEX [IX_MAIL_ACCESS_EMPSYSID] ON [MAIL_ACCESS] ([MAIL_EMPSYSID]);

-- =====================================================
-- VERIFICATION
-- =====================================================

PRINT 'EmailNotification Module Schema created successfully.';
GO

-- Verify table creation
IF OBJECT_ID('EMAIL_TYPEMAST', 'U') IS NOT NULL
    PRINT 'Table EMAIL_TYPEMAST: OK'
ELSE
    PRINT 'Table EMAIL_TYPEMAST: FAILED'
GO

IF OBJECT_ID('MAIL_ACCESS', 'U') IS NOT NULL
    PRINT 'Table MAIL_ACCESS: OK'
ELSE
    PRINT 'Table MAIL_ACCESS: FAILED'
GO
