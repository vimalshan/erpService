-- Extended schema columns required by settings service SPs and insert scripts
-- 2026-04-15

-- Add FirstName, LastName to Users (needed by Admin/Member list SPs)
IF COL_LENGTH('dbo.Users', 'FirstName') IS NULL
    ALTER TABLE [dbo].[Users] ADD [FirstName] NVARCHAR(100) NULL;

IF COL_LENGTH('dbo.Users', 'LastName') IS NULL
    ALTER TABLE [dbo].[Users] ADD [LastName] NVARCHAR(100) NULL;

-- Add Address, CityId, CountryId, PostalCode to Companies (needed by Company Details SP)
IF COL_LENGTH('dbo.Companies', 'Address') IS NULL
    ALTER TABLE [dbo].[Companies] ADD [Address] NVARCHAR(500) NULL;

IF COL_LENGTH('dbo.Companies', 'CityId') IS NULL
    ALTER TABLE [dbo].[Companies] ADD [CityId] INT NULL;

IF COL_LENGTH('dbo.Companies', 'CountryId') IS NULL
    ALTER TABLE [dbo].[Companies] ADD [CountryId] INT NULL;

IF COL_LENGTH('dbo.Companies', 'PostalCode') IS NULL
    ALTER TABLE [dbo].[Companies] ADD [PostalCode] NVARCHAR(20) NULL;

-- Add CityId to Sites (needed by Member List SP)
IF COL_LENGTH('dbo.Sites', 'CityId') IS NULL
    ALTER TABLE [dbo].[Sites] ADD [CityId] INT NULL;

PRINT 'Schema columns migration 2026-04-15 applied successfully.';
