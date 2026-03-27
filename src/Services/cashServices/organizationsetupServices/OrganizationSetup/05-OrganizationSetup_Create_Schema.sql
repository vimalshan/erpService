-- ==========================================
-- Module: OrganizationSetup
-- Purpose: Organization Configuration and Role Management
-- Created: March 9, 2026
-- Database: CASHDB
-- ==========================================

USE CASHDB;
GO

-- =====================================================
-- CREATE TABLES FOR ORGANIZATION SETUP MODULE
-- =====================================================

-- Table: DEAL_ROLE - Role Master
CREATE TABLE [DEAL_ROLE] (
    [ROLE_ID] BIGINT NOT NULL,          -- Role ID
    [ROLE_NAME] VARCHAR(50) NOT NULL,    -- Role Name
    [ROLE_LEVEL] BIGINT NOT NULL,        -- Role Level
    [ROLE_MODIFIEDBY] DECIMAL(38) NOT NULL, -- Modified By
    [ROLE_MODIFIEDON] DATETIME2(3) NOT NULL, -- Modified On
    CONSTRAINT [PK_DEAL_ROLE] PRIMARY KEY ([ROLE_ID])
);

-- Table: DEAL_USERMAP - User Role Mapping
CREATE TABLE [DEAL_USERMAP] (
    [ROLE_MAPID] BIGINT NOT NULL,      -- Role Map ID
    [ROLE_ID] BIGINT NOT NULL,          -- Role ID
    [ROLE_EMPSYSID] BIGINT NOT NULL,    -- Employee System ID
    [ROLE_ORGID] BIGINT NOT NULL,       -- Organization ID
    [ROLE_BUSINESS] BIGINT NULL,
    CONSTRAINT [PK_DEAL_USERMAP] PRIMARY KEY ([ROLE_MAPID]),
    CONSTRAINT [FK_DEAL_USERMAP_ROLE] FOREIGN KEY ([ROLE_ID]) REFERENCES [DEAL_ROLE]([ROLE_ID])
);

-- Table: DEAL_ORGPARAMS - Organization Parameters
CREATE TABLE [DEAL_ORGPARAMS] (
    [ORG_PARAMID] BIGINT NOT NULL,     -- Parameter ID
    [ORG_PARAMTYPE] CHAR(6) NOT NULL,   -- Parameter Type
    [ORG_PARAMVALUE] BIGINT NOT NULL,   -- Parameter Value
    [ORG_ID] BIGINT NOT NULL,           -- Organization ID
    [ORG_MODIFIEDBY] DECIMAL(38) NOT NULL,
    [ORG_MODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_DEAL_ORGPARAMS] PRIMARY KEY ([ORG_PARAMID])
);

-- Table: DEAL_PPLIMIT - PP Limit Management
CREATE TABLE [DEAL_PPLIMIT] (
    [PP_LIMITID] BIGINT NOT NULL,       -- PP Limit ID
    [PP_ORGID] BIGINT NOT NULL,         -- Organization ID
    [PP_TRANTYPE] CHAR(1) NOT NULL,     -- I/E - Imports/Exports
    [PP_BASCURR] BIGINT NOT NULL,       -- Base Currency ID
    [PP_LIMITAMT] DECIMAL(19,0) NULL,   -- PP Limit Amount
    [PP_FINYEAR] INT NOT NULL,          -- Financial Year
    [PP_LIMITACT] DECIMAL(19,0) NULL,   -- PP Limit Used
    [PP_CERTIFICATEUPLOAD] VARCHAR(500) NULL,
    [PP_MODIFIEDBY] DECIMAL(38) NULL,
    [PP_MODIFIEDON] DATETIME2(3) NULL,
    CONSTRAINT [PK_DEAL_PPLIMIT] PRIMARY KEY ([PP_LIMITID])
);

-- =====================================================
-- CREATE INDEXES
-- =====================================================

CREATE INDEX [IX_DEAL_ROLE_NAME] ON [DEAL_ROLE] ([ROLE_NAME]);
CREATE INDEX [IX_DEAL_USERMAP_EMPID] ON [DEAL_USERMAP] ([ROLE_EMPSYSID]);
CREATE INDEX [IX_DEAL_USERMAP_ORGID] ON [DEAL_USERMAP] ([ROLE_ORGID]);
CREATE INDEX [IX_DEAL_ORGPARAMS_ORGID] ON [DEAL_ORGPARAMS] ([ORG_ID]);
CREATE INDEX [IX_DEAL_ORGPARAMS_PARAMTYPE] ON [DEAL_ORGPARAMS] ([ORG_PARAMTYPE]);
CREATE INDEX [IX_DEAL_PPLIMIT_ORGID] ON [DEAL_PPLIMIT] ([PP_ORGID]);
CREATE INDEX [IX_DEAL_PPLIMIT_FINYEAR] ON [DEAL_PPLIMIT] ([PP_FINYEAR]);

-- =====================================================
-- VERIFICATION
-- =====================================================

PRINT 'OrganizationSetup Module Schema created successfully.';
GO

-- Verify table creation
IF OBJECT_ID('DEAL_ROLE', 'U') IS NOT NULL
    PRINT 'Table DEAL_ROLE: OK'
ELSE
    PRINT 'Table DEAL_ROLE: FAILED'
GO

IF OBJECT_ID('DEAL_USERMAP', 'U') IS NOT NULL
    PRINT 'Table DEAL_USERMAP: OK'
ELSE
    PRINT 'Table DEAL_USERMAP: FAILED'
GO

IF OBJECT_ID('DEAL_ORGPARAMS', 'U') IS NOT NULL
    PRINT 'Table DEAL_ORGPARAMS: OK'
ELSE
    PRINT 'Table DEAL_ORGPARAMS: FAILED'
GO

IF OBJECT_ID('DEAL_PPLIMIT', 'U') IS NOT NULL
    PRINT 'Table DEAL_PPLIMIT: OK'
ELSE
    PRINT 'Table DEAL_PPLIMIT: FAILED'
GO
