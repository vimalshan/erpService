-- Migration: Add columns required by SettingsDomainDbContext User/UserRole entity mappings
-- 2026-04-15

-- Users table: add all columns present in the domain entity but missing from DB
IF COL_LENGTH('dbo.Users', 'CreatedBy') IS NULL
    ALTER TABLE [dbo].[Users] ADD [CreatedBy] INT NULL;

IF COL_LENGTH('dbo.Users', 'ModifiedBy') IS NULL
    ALTER TABLE [dbo].[Users] ADD [ModifiedBy] INT NULL;

IF COL_LENGTH('dbo.Users', 'CreatedDate') IS NULL
    ALTER TABLE [dbo].[Users] ADD [CreatedDate] DATETIME NOT NULL CONSTRAINT [DF_Users_CreatedDate] DEFAULT GETDATE();

IF COL_LENGTH('dbo.Users', 'ModifiedDate') IS NULL
    ALTER TABLE [dbo].[Users] ADD [ModifiedDate] DATETIME NOT NULL CONSTRAINT [DF_Users_ModifiedDate] DEFAULT GETDATE();

IF COL_LENGTH('dbo.Users', 'Department') IS NULL
    ALTER TABLE [dbo].[Users] ADD [Department] NVARCHAR(100) NULL;

IF COL_LENGTH('dbo.Users', 'EmailVerificationToken') IS NULL
    ALTER TABLE [dbo].[Users] ADD [EmailVerificationToken] NVARCHAR(255) NULL;

IF COL_LENGTH('dbo.Users', 'IsEmailVerified') IS NULL
    ALTER TABLE [dbo].[Users] ADD [IsEmailVerified] BIT NOT NULL CONSTRAINT [DF_Users_IsEmailVerified] DEFAULT 0;

IF COL_LENGTH('dbo.Users', 'Language') IS NULL
    ALTER TABLE [dbo].[Users] ADD [Language] NVARCHAR(10) NULL CONSTRAINT [DF_Users_Language] DEFAULT 'EN';

IF COL_LENGTH('dbo.Users', 'LastLoginDate') IS NULL
    ALTER TABLE [dbo].[Users] ADD [LastLoginDate] DATETIME NULL;

IF COL_LENGTH('dbo.Users', 'PasswordResetExpiry') IS NULL
    ALTER TABLE [dbo].[Users] ADD [PasswordResetExpiry] DATETIME NULL;

IF COL_LENGTH('dbo.Users', 'PasswordResetToken') IS NULL
    ALTER TABLE [dbo].[Users] ADD [PasswordResetToken] NVARCHAR(255) NULL;

IF COL_LENGTH('dbo.Users', 'Phone') IS NULL
    ALTER TABLE [dbo].[Users] ADD [Phone] NVARCHAR(20) NULL;

IF COL_LENGTH('dbo.Users', 'Position') IS NULL
    ALTER TABLE [dbo].[Users] ADD [Position] NVARCHAR(100) NULL;

IF COL_LENGTH('dbo.Users', 'TimeZone') IS NULL
    ALTER TABLE [dbo].[Users] ADD [TimeZone] NVARCHAR(50) NULL CONSTRAINT [DF_Users_TimeZone] DEFAULT 'UTC';

IF COL_LENGTH('dbo.Users', 'TwoFactorEnabled') IS NULL
    ALTER TABLE [dbo].[Users] ADD [TwoFactorEnabled] BIT NOT NULL CONSTRAINT [DF_Users_TwoFactorEnabled] DEFAULT 0;

IF COL_LENGTH('dbo.Users', 'TwoFactorSecret') IS NULL
    ALTER TABLE [dbo].[Users] ADD [TwoFactorSecret] NVARCHAR(100) NULL;

-- UserRoles table: add CreatedBy and ModifiedBy (DB has AssignedBy/AssignedDate but entity uses CreatedBy/ModifiedBy)
IF COL_LENGTH('dbo.UserRoles', 'CreatedBy') IS NULL
    ALTER TABLE [dbo].[UserRoles] ADD [CreatedBy] INT NULL;

IF COL_LENGTH('dbo.UserRoles', 'ModifiedBy') IS NULL
    ALTER TABLE [dbo].[UserRoles] ADD [ModifiedBy] INT NULL;

PRINT 'Users/UserRoles columns migration 2026-04-15 applied successfully.';

-- Show updated column list
SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' ORDER BY ORDINAL_POSITION;
