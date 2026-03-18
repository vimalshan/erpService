-- ============================================================================
-- Module: Mobile App Management
-- Purpose: Manage mobile application device registration and login tracking
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

-- Set database context
USE [SPARSHDB];
GO

-- ============================================================================
-- TABLE: MOB_APPDEVICE_DETAILS
-- Description: Stores mobile device details for registered applications
-- ============================================================================
IF OBJECT_ID('[dbo].[MOB_APPDEVICE_DETAILS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MOB_APPDEVICE_DETAILS];
GO

CREATE TABLE [dbo].[MOB_APPDEVICE_DETAILS] (
    [MD_EMPSYSID]       DECIMAL(38) NOT NULL,           -- Employee System ID
    [MD_DEVICEID]       VARCHAR(200) NULL,              -- Device ID
    [MD_ACTIVE]         CHAR(1) NOT NULL,               -- Y = Active, N = Inactive
    [MD_DEVICETYPE]     CHAR(1) NULL,                   -- A = Android, I = iOS
    [MD_IMEINO]         VARCHAR(200) NULL,              -- Device IMEI Number
    [MD_CREATEDON]      DATETIME2(3) NOT NULL,          -- Creation Timestamp
    [MD_UPDATEDBY]      DECIMAL(38) NOT NULL,           -- Updated By (Employee System ID)
    [MD_UPDATEDON]      DATETIME2(3) NOT NULL,          -- Last Updated Timestamp
    CONSTRAINT [PK_MOB_APPDEVICE_DETAILS] PRIMARY KEY ([MD_EMPSYSID], [MD_DEVICEID])
);

CREATE INDEX [IX_MOB_APPDEVICE_ACTIVE] ON [dbo].[MOB_APPDEVICE_DETAILS]([MD_ACTIVE]);
CREATE INDEX [IX_MOB_APPDEVICE_DEVICE] ON [dbo].[MOB_APPDEVICE_DETAILS]([MD_DEVICEID]);
GO

-- ============================================================================
-- TABLE: MOB_LOGINDET
-- Description: Tracks mobile application login history
-- ============================================================================
IF OBJECT_ID('[dbo].[MOB_LOGINDET]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MOB_LOGINDET];
GO

CREATE TABLE [dbo].[MOB_LOGINDET] (
    [LD_LOGINID]        DECIMAL(38) NOT NULL,           -- Login ID (Primary Key)
    [LD_USERSYSID]      DECIMAL(38) NOT NULL,           -- User System ID
    [LD_DEVICEID]       VARCHAR(200) NULL,              -- Device ID
    [LD_LOGON]          DATETIME2(3) NOT NULL,          -- Login DateTime
    [LD_GUID]           VARCHAR(255) NOT NULL,          -- Unique Sequential GUID
    [LD_IMEINO]         VARCHAR(200) NULL,              -- Device IMEI Number
    [LD_DEVICETYPE]     CHAR(1) NULL,                   -- A = Android, I = iOS
    CONSTRAINT [PK_MOB_LOGINDET] PRIMARY KEY ([LD_LOGINID])
);

CREATE INDEX [IX_MOB_LOGIN_USERID] ON [dbo].[MOB_LOGINDET]([LD_USERSYSID]);
CREATE INDEX [IX_MOB_LOGIN_DEVICE] ON [dbo].[MOB_LOGINDET]([LD_DEVICEID]);
CREATE INDEX [IX_MOB_LOGIN_LOGON] ON [dbo].[MOB_LOGINDET]([LD_LOGON]);
GO

-- ============================================================================
-- TABLE: MOBAPP_REGISTER
-- Description: Mobile application user registration management
-- ============================================================================
IF OBJECT_ID('[dbo].[MOBAPP_REGISTER]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MOBAPP_REGISTER];
GO

CREATE TABLE [dbo].[MOBAPP_REGISTER] (
    [REGISTER_ID]           BIGINT NOT NULL,             -- Registration ID (Primary Key)
    [REGISTER_EMPSYSID]     BIGINT NULL,                 -- Employee System ID
    [REGISTER_USERID]       VARCHAR(255) NULL,           -- User ID
    [REGISTER_USERSYSID]    BIGINT NULL,                 -- User System ID
    [REGISTER_USERTYPE]     CHAR(1) NULL,                -- User Type
    [REGISTER_PINNO]        BIGINT NULL,                 -- Registration PIN
    [REGISTER_PINGENERATEDON] DATETIME2(3) NULL,         -- PIN Generated Timestamp
    [REGISTER_UPDATEDON]    DATETIME2(3) NULL,           -- Last Updated Timestamp
    [REGISTER_STATUS]       CHAR(1) NULL,                -- P = Pending, R = Registered, C = Closed
    [REGISTER_MOBILENO]     VARCHAR(255) NULL,           -- Mobile Number
    [REGISTER_IMEINO]       VARCHAR(255) NULL,           -- Device IMEI Number
    [REGISTER_GUID]         CHAR(1) NULL,                -- GUID
    [REGISTER_DEVICEID]     VARCHAR(255) NULL,           -- Device ID
    [REGISTER_DTYPE]        CHAR(1) NULL,                -- A = Android, I = iOS
    CONSTRAINT [PK_MOBAPP_REGISTER] PRIMARY KEY ([REGISTER_ID])
);

CREATE INDEX [IX_MOBAPP_REG_STATUS] ON [dbo].[MOBAPP_REGISTER]([REGISTER_STATUS]);
CREATE INDEX [IX_MOBAPP_REG_USERID] ON [dbo].[MOBAPP_REGISTER]([REGISTER_USERID]);
GO

PRINT 'Mobile App Management Tables created successfully.';
GO
