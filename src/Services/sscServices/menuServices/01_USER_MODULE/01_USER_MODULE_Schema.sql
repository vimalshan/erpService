-- ==========================================
-- Module: USER MANAGEMENT MODULE
-- Description: User authentication, roles, and organization mapping
-- Created: March 9, 2026
-- Database: SSCDB
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- USER_MAST - User Master Data
-- ==========================================
IF OBJECT_ID('[USER_MAST]', 'U') IS NOT NULL DROP TABLE [USER_MAST];
GO
CREATE TABLE [USER_MAST] (
    [USER_ID] BIGINT NOT NULL  -- User ID,
    [USER_HREMPSYSID] BIGINT NULL  -- HR Employee System ID,
    [USER_NAME] VARCHAR(100) NOT NULL  -- User Name,
    [USER_PASSWORD] VARCHAR(50) NOT NULL  -- Password,
    [USER_SPARSHUSERID] VARCHAR(50) NULL  -- Sparch User ID,
    [USER_EMAILID] VARCHAR(50) NULL  -- Email ID,
    [USER_EFFECTIVE_DATE] DATETIME2(3) NOT NULL  -- User id created on,
    [USER_CLOSURE_DATE] DATETIME2(3) NOT NULL  -- User id created Closed On,
    [USER_ENTEREDBY] BIGINT NOT NULL  -- Access given by,
    CONSTRAINT [PK_USER_MAST] PRIMARY KEY ([USER_ID])
);
GO

-- ==========================================
-- USER_ROLEMAP - User Role Mapping
-- ==========================================
IF OBJECT_ID('[USER_ROLEMAP]', 'U') IS NOT NULL DROP TABLE [USER_ROLEMAP];
GO
CREATE TABLE [USER_ROLEMAP] (
    [ROLE_MAPID] BIGINT NOT NULL  -- Role Map ID,
    [ROLE_USERID] BIGINT NOT NULL  -- User ID,
    [ROLE_ID] BIGINT NOT NULL  -- End User / Special Approver / Unit Mail Room / SSC Mai Room / AP Processor / AP Validator/Admin,
    [ROLE_DEFFLAG] VARCHAR(255) NOT NULL  -- Defaul Yes/No,
    [ROLE_CREATEDBY] DATETIME2(3) NOT NULL  -- Createdon,
    [ROLE_CREATEDON] BIGINT NOT NULL  -- Created By,
    CONSTRAINT [PK_USER_ROLEMAP] PRIMARY KEY ([ROLE_MAPID])
);
GO

-- ==========================================
-- USER_ORGMAP - User Organization Mapping
-- ==========================================
IF OBJECT_ID('[USER_ORGMAP]', 'U') IS NOT NULL DROP TABLE [USER_ORGMAP];
GO
CREATE TABLE [USER_ORGMAP] (
    [ORG_MAPID] BIGINT NOT NULL  -- Org Map ID,
    [ORG_USERID] BIGINT NOT NULL  -- User ID,
    [ORG_BUID] VARCHAR(25) NOT NULL  -- R12 BU ID - Unit ID,
    [ORG_CREATEDBY] BIGINT NOT NULL  -- Created By,
    [ORG_CREATEDON] DATETIME2(3) NOT NULL  -- Created On,
    CONSTRAINT [PK_USER_ORGMAP] PRIMARY KEY ([ORG_MAPID])
);
GO

-- ==========================================
-- USER_LOCATIONMAP - User Location Mapping
-- ==========================================
IF OBJECT_ID('[USER_LOCATIONMAP]', 'U') IS NOT NULL DROP TABLE [USER_LOCATIONMAP];
GO
CREATE TABLE [USER_LOCATIONMAP] (
    [LOC_MAPID] BIGINT NOT NULL  -- User Location Sequence ID,
    [LOC_USERID] BIGINT NOT NULL  -- User ID,
    [LOC_ID] INT NOT NULL  -- Admin Location ID,
    [LOC_CREATEDON] DATETIME2(3) NOT NULL  -- Createdon,
    [LOC_CREATEDBY] BIGINT NOT NULL  -- Created By,
    CONSTRAINT [PK_USER_LOCATIONMAP] PRIMARY KEY ([LOC_MAPID])
);
GO

PRINT 'USER_MODULE Schema created successfully.';
GO
