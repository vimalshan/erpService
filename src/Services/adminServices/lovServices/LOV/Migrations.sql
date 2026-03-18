-- ==========================================
-- LOV SERVICE - EF MIGRATION SCRIPT
-- Run AFTER: dotnet ef migrations add InitialCreate
--            dotnet ef database update
-- Target DB: LOVDB (LocalDB / SQL Server)
-- Connection: Server=(localdb)\MSSQLLocalDB;Database=LOVDB;
--             Integrated Security=True;TrustServerCertificate=True
-- ==========================================

-- Step 1: Ensure LOVDB exists
USE MASTER;
GO
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LOVDB')
BEGIN
    CREATE DATABASE LOVDB;
    PRINT '✓ LOVDB created';
END
ELSE
    PRINT '  LOVDB already exists';
GO

USE [LOVDB];
GO

-- Step 2: EF __EFMigrationsHistory table (created automatically by EF)
-- dotnet tool install --global dotnet-ef
-- dotnet ef migrations add InitialCreate --project src/LovService.Infrastructure --startup-project src/LovService.API
-- dotnet ef database update             --project src/LovService.Infrastructure --startup-project src/LovService.API

-- Step 3: Manual schema (matches EF model exactly)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('LOV_TYPE') AND type = 'U')
BEGIN
    CREATE TABLE [LOV_TYPE] (
        [LOV_TYPE_ID]   BIGINT       NOT NULL,
        [LOV_TYPE_NAME] VARCHAR(30)  NOT NULL,
        CONSTRAINT [PK_LOV_TYPE] PRIMARY KEY ([LOV_TYPE_ID])
    );
    PRINT '✓ LOV_TYPE created';
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('LOV_MASTER') AND type = 'U')
BEGIN
    CREATE TABLE [LOV_MASTER] (
        [LOV_ID]         BIGINT          NOT NULL,
        [LOV_TYPE_ID]    BIGINT          NOT NULL,
        [LOV_NAME]       VARCHAR(30)     NOT NULL,
        [LOV_UPDATED_BY] BIGINT          NOT NULL,
        [LOV_UPDATED_ON] DATETIME2(3)    NOT NULL,
        CONSTRAINT [PK_LOV_MASTER]     PRIMARY KEY  ([LOV_ID]),
        CONSTRAINT [FK_LOV_MASTER_TYPE] FOREIGN KEY ([LOV_TYPE_ID])
            REFERENCES [LOV_TYPE]([LOV_TYPE_ID]) ON DELETE CASCADE
    );

    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_LOV_MASTER_TYPEID')
        CREATE INDEX [IDX_LOV_MASTER_TYPEID] ON [LOV_MASTER]([LOV_TYPE_ID]);

    PRINT '✓ LOV_MASTER created';
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('ITEMDATA') AND type = 'U')
BEGIN
    CREATE TABLE [ITEMDATA] (
        [ID]       INT          NOT NULL IDENTITY(1,1),
        [CATNAME]  VARCHAR(40)  NULL,
        [ITEMNAME] VARCHAR(60)  NULL,
        [MAKE]     VARCHAR(30)  NULL,
        [UOM]      VARCHAR(20)  NULL,
        [PRICE]    INT          NULL,
        CONSTRAINT [PK_ITEMDATA] PRIMARY KEY ([ID])
    );
    PRINT '✓ ITEMDATA created';
END
GO

PRINT '';
PRINT '====================================================';
PRINT ' LOVDB Schema ready for EF Core migrations';
PRINT '====================================================';
PRINT '';
PRINT 'EF Migration Commands:';
PRINT '  dotnet ef migrations add InitialCreate \';
PRINT '    --project src/LovService.Infrastructure \';
PRINT '    --startup-project src/LovService.API';
PRINT '';
PRINT '  dotnet ef database update \';
PRINT '    --project src/LovService.Infrastructure \';
PRINT '    --startup-project src/LovService.API';
PRINT '';
PRINT 'Then run StoredProcedures.sql and SampleData.sql';
GO
