-- ==========================================
-- GroupManagementModule
-- Database: SRFSPARSHDB
-- Module Purpose: User Group and Menu Mapping Management
-- Created: March 09, 2026
-- ==========================================

USE SRFSPARSHDB;
GO

-- Drop tables if they exist (reverse order for dependencies)
IF OBJECT_ID('[GROUP_MENUMAP]', 'U') IS NOT NULL DROP TABLE [GROUP_MENUMAP];
GO
IF OBJECT_ID('[GROUP_MAST]', 'U') IS NOT NULL DROP TABLE [GROUP_MAST];
GO

-- ==========================================
-- Table: GROUP_MAST - User Group Master
-- Description: Master table for user groups/roles with access control
-- ==========================================
CREATE TABLE [GROUP_MAST] (
    [GROUP_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [GROUP_CODE] VARCHAR(50) NOT NULL UNIQUE,
    [GROUP_NAME] VARCHAR(255) NOT NULL,
    [GROUP_DESC] NVARCHAR(MAX),
    [GROUP_STATUS] CHAR(1) DEFAULT 'A', -- A=Active, I=Inactive
    [IS_ADMIN] CHAR(1) DEFAULT 'N', -- Y=Admin Group, N=Regular Group
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [UPDATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3)
);
GO

-- ==========================================
-- Table: GROUP_MENUMAP - Group Menu Mapping
-- Description: Maps menus/features to user groups for access control
-- Relationship: References GROUP_MAST
-- ==========================================
CREATE TABLE [GROUP_MENUMAP] (
    [MENUMAP_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [GROUP_ID] BIGINT NOT NULL,
    [MENU_CODE] VARCHAR(50) NOT NULL,
    [MENU_NAME] VARCHAR(255) NOT NULL,
    [CAN_VIEW] CHAR(1) DEFAULT 'Y',
    [CAN_CREATE] CHAR(1) DEFAULT 'N',
    [CAN_EDIT] CHAR(1) DEFAULT 'N',
    [CAN_DELETE] CHAR(1) DEFAULT 'N',
    [CAN_APPROVE] CHAR(1) DEFAULT 'N',
    [MENU_SEQUENCE] INT,
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [UPDATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3),
    CONSTRAINT [FK_GROUP_MENUMAP_GROUP] FOREIGN KEY ([GROUP_ID]) REFERENCES [GROUP_MAST]([GROUP_ID]),
    CONSTRAINT [UC_GROUP_MENU] UNIQUE ([GROUP_ID], [MENU_CODE])
);
GO

-- Create Indexes
CREATE INDEX [IX_GROUP_MAST_CODE] ON [GROUP_MAST]([GROUP_CODE]);
CREATE INDEX [IX_GROUP_MAST_STATUS] ON [GROUP_MAST]([GROUP_STATUS]);
CREATE INDEX [IX_GROUP_MAST_IS_ADMIN] ON [GROUP_MAST]([IS_ADMIN]);
CREATE INDEX [IX_GROUP_MENUMAP_GROUP_ID] ON [GROUP_MENUMAP]([GROUP_ID]);
CREATE INDEX [IX_GROUP_MENUMAP_MENU] ON [GROUP_MENUMAP]([MENU_CODE]);
GO

PRINT 'GroupManagementModule_Schema created successfully.';
GO
