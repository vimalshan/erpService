-- ==========================================
-- Module: MENU AND SECURITY MODULE
-- Description: Menu management and role-based access control
-- Created: March 9, 2026
-- Database: SSCDB
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- MENU_MASTER - Menu Master Data
-- ==========================================
IF OBJECT_ID('[MENU_MASTER]', 'U') IS NOT NULL DROP TABLE [MENU_MASTER];
GO
CREATE TABLE [MENU_MASTER] (
    [MENU_ID] BIGINT NOT NULL  -- Menu ID,
    [MENU_NAME] VARCHAR(100) NOT NULL  -- Menu Name,
    [MENU_PAGENAME] VARCHAR(200) NOT NULL  -- Menu Page Name,
    [MENU_PARENTID] BIGINT NOT NULL  -- Parent Menu Menu ID,
    [MENU_DISPLAYORDER] INT NOT NULL  -- Display Order,
    [MENU_MODIFIEDBY] BIGINT NOT NULL  -- Modified By,
    [MENU_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    CONSTRAINT [PK_MENU_MASTER] PRIMARY KEY ([MENU_ID])
);
GO

-- ==========================================
-- ROLE_MENUACCESS - Role Menu Access Control
-- ==========================================
IF OBJECT_ID('[ROLE_MENUACCESS]', 'U') IS NOT NULL DROP TABLE [ROLE_MENUACCESS];
GO
CREATE TABLE [ROLE_MENUACCESS] (
    [MENU_ACCESSID] BIGINT NOT NULL  -- Access ID,
    [MENU_ID] INT NOT NULL  -- Menu ID,
    [MENU_ROLEID] BIGINT NOT NULL  -- Role ID,
    [ROLE_MODIFIEDBY] BIGINT NULL,
    [ROLE_MODIFIEDON] DATETIME2(3) NULL,
    CONSTRAINT [PK_ROLE_MENUACCESS] PRIMARY KEY ([MENU_ACCESSID])
);
GO

PRINT 'MENU_AND_SECURITY_MODULE Schema created successfully.';
GO
