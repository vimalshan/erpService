-- Seed script: insert default admin user
-- Run after EF migration InitialCreate
USE [HRDB];
GO

IF NOT EXISTS (SELECT 1 FROM [USER_PROFILE_PFS] WHERE [EM_USR_ID] = 'admin')
BEGIN
    INSERT INTO [USER_PROFILE_PFS]
        ([EM_USR_ID],[EM_EMP_NUM],[EM_UNT_COD],[EM_NICK_NAM],[EM_USR_TYP],
         [EM_EML_FLG],[EM_EFF_DAT],[EM_USR_PASS],[EM_EMP_NAM],[EM_REGSTATUS])
    VALUES
        ('admin', 1, 'HQ', 'Administrator', 'A',
         'Y', GETUTCDATE(), 'CHANGE_ME_HASH', 'System Administrator', 'A');
END
GO
