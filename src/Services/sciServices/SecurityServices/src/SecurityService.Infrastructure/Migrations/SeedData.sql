-- ==========================================
-- SECURITY MODULE - Seed Data
-- Database: SCIDB
-- Created: March 18, 2026
-- ==========================================

USE SCIDB;
GO

-- ── Seed Roles ──────────────────────────────────────────────
INSERT INTO [ROLE_MAST] ([RL_ROL_COD], [RL_ROL_NAM], [RL_UPD_USR], [RL_UPD_DAT])
SELECT 1, 'System Administrator', 'SEED', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM [ROLE_MAST] WHERE [RL_ROL_COD] = 1);

INSERT INTO [ROLE_MAST] ([RL_ROL_COD], [RL_ROL_NAM], [RL_UPD_USR], [RL_UPD_DAT])
SELECT 2, 'Security Manager', 'SEED', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM [ROLE_MAST] WHERE [RL_ROL_COD] = 2);

INSERT INTO [ROLE_MAST] ([RL_ROL_COD], [RL_ROL_NAM], [RL_UPD_USR], [RL_UPD_DAT])
SELECT 3, 'Read Only User', 'SEED', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM [ROLE_MAST] WHERE [RL_ROL_COD] = 3);

-- ── Seed Menu Master ─────────────────────────────────────────
INSERT INTO [MENUMASTER] ([MENU_ID], [MENU_NAME], [URL], [PARENT_MENU_ID], [DISPLAYORDER])
SELECT 1, 'Security', '/security', NULL, 1
WHERE NOT EXISTS (SELECT 1 FROM [MENUMASTER] WHERE [MENU_ID] = 1);

INSERT INTO [MENUMASTER] ([MENU_ID], [MENU_NAME], [URL], [PARENT_MENU_ID], [DISPLAYORDER])
SELECT 2, 'Users', '/security/users', 1, 1
WHERE NOT EXISTS (SELECT 1 FROM [MENUMASTER] WHERE [MENU_ID] = 2);

INSERT INTO [MENUMASTER] ([MENU_ID], [MENU_NAME], [URL], [PARENT_MENU_ID], [DISPLAYORDER])
SELECT 3, 'Roles', '/security/roles', 1, 2
WHERE NOT EXISTS (SELECT 1 FROM [MENUMASTER] WHERE [MENU_ID] = 3);

INSERT INTO [MENUMASTER] ([MENU_ID], [MENU_NAME], [URL], [PARENT_MENU_ID], [DISPLAYORDER])
SELECT 4, 'Menus', '/security/menus', 1, 3
WHERE NOT EXISTS (SELECT 1 FROM [MENUMASTER] WHERE [MENU_ID] = 4);

-- ── Seed Access Role Master ──────────────────────────────────
INSERT INTO [ACCESS_ROLE_MASTER] ([AR_ROL_COD], [AR_ROL_NAM], [AR_UPD_USR], [AR_UPD_DAT])
VALUES (1, 'System Administrator', 'SEED', GETDATE()),
       (2, 'Security Manager', 'SEED', GETDATE()),
       (3, 'Read Only User', 'SEED', GETDATE());

-- ── Seed AccessRole Menu ─────────────────────────────────────
-- Admin gets all menus
INSERT INTO [ACCESSROLE_MENU] ([ARM_ROL_COD], [ARM_MEN_COD], [ARM_UPD_USR], [ARM_UPD_DAT])
VALUES (1, 1, 'SEED', GETDATE()), (1, 2, 'SEED', GETDATE()),
       (1, 3, 'SEED', GETDATE()), (1, 4, 'SEED', GETDATE());

-- Security Manager gets users and roles menus
INSERT INTO [ACCESSROLE_MENU] ([ARM_ROL_COD], [ARM_MEN_COD], [ARM_UPD_USR], [ARM_UPD_DAT])
VALUES (2, 1, 'SEED', GETDATE()), (2, 2, 'SEED', GETDATE()), (2, 3, 'SEED', GETDATE());

-- Read Only gets only parent security menu
INSERT INTO [ACCESSROLE_MENU] ([ARM_ROL_COD], [ARM_MEN_COD], [ARM_UPD_USR], [ARM_UPD_DAT])
VALUES (3, 1, 'SEED', GETDATE());

-- ── Seed Admin User ──────────────────────────────────────────
INSERT INTO [USER_MASTER]
    ([UM_USR_NUM], [UM_USR_COD], [UM_USR_NAM], [UM_USR_MAI], [UM_STR_DAT], [UM_USR_TYP], [UM_UPD_USR], [UM_UPD_DAT])
SELECT 1, 'ADMIN', 'System Administrator', 'admin@scidb.local', GETDATE(), 'A', 'SEED', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM [USER_MASTER] WHERE [UM_USR_NUM] = 1);

-- ── Assign Admin Role to Admin User ──────────────────────────
INSERT INTO [USER_ROLE] ([UR_USR_NUM], [UR_ROL_COD], [UR_STR_DAT], [UR_UPD_USR], [UR_UPD_NUM], [UR_UPD_DAT])
SELECT 1, 1, GETDATE(), 'SEED', 1, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM [USER_ROLE] WHERE [UR_USR_NUM] = 1 AND [UR_ROL_COD] = 1);

PRINT 'Seed data inserted successfully.';
GO
