-- =====================================================
-- Docker init script for LOVDB
-- Runs on first container startup via sqlcmd
-- =====================================================

USE MASTER;
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 1. Create database
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'LOVDB')
BEGIN
    CREATE DATABASE [LOVDB];
    PRINT '+ LOVDB created';
END
ELSE
    PRINT '+ LOVDB already exists';
GO

USE [LOVDB];
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 2. Tables
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬

-- LOV_TYPE
IF OBJECT_ID(N'[dbo].[LOV_TYPE]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[LOV_TYPE] (
        [LOV_TYPE_ID]   BIGINT        NOT NULL,
        [LOV_TYPE_NAME] NVARCHAR(30)  NOT NULL,
        CONSTRAINT [PK_LOV_TYPE] PRIMARY KEY ([LOV_TYPE_ID])
    );
    PRINT '+ Table LOV_TYPE created';
END
ELSE
    PRINT '+ Table LOV_TYPE already exists';
GO

-- LOV_MASTER
IF OBJECT_ID(N'[dbo].[LOV_MASTER]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[LOV_MASTER] (
        [LOV_ID]         BIGINT        NOT NULL,
        [LOV_TYPE_ID]    BIGINT        NOT NULL,
        [LOV_NAME]       NVARCHAR(30)  NOT NULL,
        [LOV_UPDATED_BY] BIGINT        NOT NULL,
        [LOV_UPDATED_ON] DATETIME2(3)  NOT NULL,
        CONSTRAINT [PK_LOV_MASTER]      PRIMARY KEY ([LOV_ID]),
        CONSTRAINT [FK_LOV_MASTER_TYPE] FOREIGN KEY ([LOV_TYPE_ID]) REFERENCES [dbo].[LOV_TYPE]([LOV_TYPE_ID])
    );
    CREATE INDEX [IDX_LOV_MASTER_TYPEID] ON [dbo].[LOV_MASTER]([LOV_TYPE_ID]);
    PRINT '+ Table LOV_MASTER created';
END
ELSE
    PRINT '+ Table LOV_MASTER already exists';
GO

-- ITEMDATA
IF OBJECT_ID(N'[dbo].[ITEMDATA]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ITEMDATA] (
        [ID]       INT IDENTITY(1,1) NOT NULL,
        [CATNAME]  NVARCHAR(40) NULL,
        [ITEMNAME] NVARCHAR(60) NULL,
        [MAKE]     NVARCHAR(30) NULL,
        [UOM]      NVARCHAR(20) NULL,
        [PRICE]    INT          NULL,
        CONSTRAINT [PK_ITEMDATA] PRIMARY KEY ([ID])
    );
    PRINT '+ Table ITEMDATA created';
END
ELSE
    PRINT '+ Table ITEMDATA already exists';
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 3. Stored Procedures
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬

-- usp_GetAllLovTypes
IF OBJECT_ID(N'[dbo].[usp_GetAllLovTypes]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_GetAllLovTypes];
GO
CREATE PROCEDURE [dbo].[usp_GetAllLovTypes]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT LOV_TYPE_ID AS LovTypeId, LOV_TYPE_NAME AS LovTypeName
    FROM [dbo].[LOV_TYPE] WITH (NOLOCK)
    ORDER BY LOV_TYPE_NAME;
END
GO
PRINT '+ usp_GetAllLovTypes created';
GO

-- usp_GetLovMastersByType
IF OBJECT_ID(N'[dbo].[usp_GetLovMastersByType]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_GetLovMastersByType];
GO
CREATE PROCEDURE [dbo].[usp_GetLovMastersByType]
    @LovTypeId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        LOV_ID         AS LovId,
        LOV_TYPE_ID    AS LovTypeId,
        LOV_NAME       AS LovName,
        LOV_UPDATED_BY AS LovUpdatedBy,
        LOV_UPDATED_ON AS LovUpdatedOn
    FROM [dbo].[LOV_MASTER] WITH (NOLOCK)
    WHERE LOV_TYPE_ID = @LovTypeId
    ORDER BY LOV_NAME;
END
GO
PRINT '+ usp_GetLovMastersByType created';
GO

-- usp_UpsertLovType
IF OBJECT_ID(N'[dbo].[usp_UpsertLovType]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_UpsertLovType];
GO
CREATE PROCEDURE [dbo].[usp_UpsertLovType]
    @LovTypeId   BIGINT,
    @LovTypeName VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM [dbo].[LOV_TYPE] WHERE LOV_TYPE_ID = @LovTypeId)
        UPDATE [dbo].[LOV_TYPE] SET LOV_TYPE_NAME = @LovTypeName WHERE LOV_TYPE_ID = @LovTypeId;
    ELSE
        INSERT INTO [dbo].[LOV_TYPE] (LOV_TYPE_ID, LOV_TYPE_NAME) VALUES (@LovTypeId, @LovTypeName);
END
GO
PRINT '+ usp_UpsertLovType created';
GO

-- usp_UpsertLovMaster
IF OBJECT_ID(N'[dbo].[usp_UpsertLovMaster]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_UpsertLovMaster];
GO
CREATE PROCEDURE [dbo].[usp_UpsertLovMaster]
    @LovId        BIGINT,
    @LovTypeId    BIGINT,
    @LovName      VARCHAR(30),
    @LovUpdatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME2(3) = SYSDATETIME();
    IF EXISTS (SELECT 1 FROM [dbo].[LOV_MASTER] WHERE LOV_ID = @LovId)
        UPDATE [dbo].[LOV_MASTER]
        SET LOV_NAME = @LovName, LOV_UPDATED_BY = @LovUpdatedBy, LOV_UPDATED_ON = @Now
        WHERE LOV_ID = @LovId;
    ELSE
        INSERT INTO [dbo].[LOV_MASTER] (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
        VALUES (@LovId, @LovTypeId, @LovName, @LovUpdatedBy, @Now);
END
GO
PRINT '+ usp_UpsertLovMaster created';
GO

-- usp_DeleteLovType
IF OBJECT_ID(N'[dbo].[usp_DeleteLovType]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_DeleteLovType];
GO
CREATE PROCEDURE [dbo].[usp_DeleteLovType]
    @LovTypeId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[LOV_MASTER] WHERE LOV_TYPE_ID = @LovTypeId;
    DELETE FROM [dbo].[LOV_TYPE]   WHERE LOV_TYPE_ID = @LovTypeId;
END
GO
PRINT '+ usp_DeleteLovType created';
GO

-- usp_SearchItemData
IF OBJECT_ID(N'[dbo].[usp_SearchItemData]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[usp_SearchItemData];
GO
CREATE PROCEDURE [dbo].[usp_SearchItemData]
    @CatName  VARCHAR(40) = NULL,
    @ItemName VARCHAR(60) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, CATNAME, ITEMNAME, MAKE, UOM, PRICE
    FROM [dbo].[ITEMDATA] WITH (NOLOCK)
    WHERE (@CatName  IS NULL OR CATNAME  LIKE '%' + @CatName  + '%')
      AND (@ItemName IS NULL OR ITEMNAME LIKE '%' + @ItemName + '%')
    ORDER BY CATNAME, ITEMNAME;
END
GO
PRINT '+ usp_SearchItemData created';
GO

-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬
-- 4. Sample Data
-- Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬Ã¢â€â‚¬

-- LOV_TYPE
MERGE INTO [dbo].[LOV_TYPE] AS target
USING (VALUES
    (1, 'CATEGORY'),
    (2, 'STATUS'),
    (3, 'PRIORITY'),
    (4, 'DEPARTMENT'),
    (5, 'UOM'),
    (6, 'PAYMENT_MODE'),
    (7, 'TAX_TYPE'),
    (8, 'CURRENCY')
) AS source (LOV_TYPE_ID, LOV_TYPE_NAME)
ON target.LOV_TYPE_ID = source.LOV_TYPE_ID
WHEN MATCHED THEN
    UPDATE SET LOV_TYPE_NAME = source.LOV_TYPE_NAME
WHEN NOT MATCHED THEN
    INSERT (LOV_TYPE_ID, LOV_TYPE_NAME) VALUES (source.LOV_TYPE_ID, source.LOV_TYPE_NAME);
PRINT '+ LOV_TYPE sample data inserted';
GO

-- LOV_MASTER
DECLARE @Now DATETIME2(3) = SYSDATETIME();
DECLARE @UserId BIGINT = 1;

MERGE INTO [dbo].[LOV_MASTER] AS target
USING (VALUES
    -- CATEGORY (Type 1)
    (101, 1, 'Electronics',   @UserId, @Now),
    (102, 1, 'Furniture',     @UserId, @Now),
    (103, 1, 'Stationery',    @UserId, @Now),
    (104, 1, 'Consumables',   @UserId, @Now),
    -- STATUS (Type 2)
    (201, 2, 'Active',        @UserId, @Now),
    (202, 2, 'Inactive',      @UserId, @Now),
    (203, 2, 'Pending',       @UserId, @Now),
    (204, 2, 'Cancelled',     @UserId, @Now),
    -- PRIORITY (Type 3)
    (301, 3, 'High',          @UserId, @Now),
    (302, 3, 'Medium',        @UserId, @Now),
    (303, 3, 'Low',           @UserId, @Now),
    -- DEPARTMENT (Type 4)
    (401, 4, 'IT',            @UserId, @Now),
    (402, 4, 'HR',            @UserId, @Now),
    (403, 4, 'Finance',       @UserId, @Now),
    (404, 4, 'Operations',    @UserId, @Now),
    -- UOM (Type 5)
    (501, 5, 'Nos',           @UserId, @Now),
    (502, 5, 'Kg',            @UserId, @Now),
    (503, 5, 'Ltr',           @UserId, @Now),
    (504, 5, 'Box',           @UserId, @Now),
    -- PAYMENT_MODE (Type 6)
    (601, 6, 'Cash',          @UserId, @Now),
    (602, 6, 'Credit Card',   @UserId, @Now),
    (603, 6, 'Bank Transfer', @UserId, @Now),
    -- TAX_TYPE (Type 7)
    (701, 7, 'GST',           @UserId, @Now),
    (702, 7, 'VAT',           @UserId, @Now),
    (703, 7, 'Exempt',        @UserId, @Now),
    -- CURRENCY (Type 8)
    (801, 8, 'INR',           @UserId, @Now),
    (802, 8, 'USD',           @UserId, @Now),
    (803, 8, 'EUR',           @UserId, @Now)
) AS source (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
ON target.LOV_ID = source.LOV_ID
WHEN MATCHED THEN
    UPDATE SET LOV_NAME = source.LOV_NAME, LOV_UPDATED_BY = source.LOV_UPDATED_BY, LOV_UPDATED_ON = source.LOV_UPDATED_ON
WHEN NOT MATCHED THEN
    INSERT (LOV_ID, LOV_TYPE_ID, LOV_NAME, LOV_UPDATED_BY, LOV_UPDATED_ON)
    VALUES (source.LOV_ID, source.LOV_TYPE_ID, source.LOV_NAME, source.LOV_UPDATED_BY, source.LOV_UPDATED_ON);
PRINT '+ LOV_MASTER sample data inserted';
GO

-- ITEMDATA
MERGE INTO [dbo].[ITEMDATA] AS target
USING (VALUES
    ('Electronics', 'Laptop Dell XPS 15',     'Dell',     'Nos',  85000),
    ('Electronics', 'Monitor 27 inch',         'LG',       'Nos',  22000),
    ('Electronics', 'Wireless Keyboard',       'Logitech', 'Nos',   3500),
    ('Electronics', 'Wireless Mouse',          'Logitech', 'Nos',   1800),
    ('Electronics', 'USB-C Hub 7-in-1',        'Anker',    'Nos',   4200),
    ('Furniture',   'Office Chair Ergonomic',  'Herman',   'Nos',  45000),
    ('Furniture',   'Standing Desk 180cm',     'IKEA',     'Nos',  32000),
    ('Furniture',   'Bookshelf 5-tier',        'IKEA',     'Nos',   8500),
    ('Stationery',  'A4 Paper 500 sheets',     'ITC',      'Box',    550),
    ('Stationery',  'Ballpoint Pen Box',       'Cello',    'Box',    250),
    ('Stationery',  'Sticky Notes Pack',       '3M',       'Box',    320),
    ('Consumables', 'Hand Sanitizer 500ml',    'Dettol',   'Ltr',    250),
    ('Consumables', 'Printer Ink Cartridge',   'HP',       'Nos',   1200),
    ('Consumables', 'Coffee Beans 1kg',        'Lavazza',  'Kg',    1800)
) AS source (CATNAME, ITEMNAME, MAKE, UOM, PRICE)
ON target.ITEMNAME = source.ITEMNAME AND target.CATNAME = source.CATNAME
WHEN MATCHED THEN
    UPDATE SET MAKE = source.MAKE, UOM = source.UOM, PRICE = source.PRICE
WHEN NOT MATCHED THEN
    INSERT (CATNAME, ITEMNAME, MAKE, UOM, PRICE)
    VALUES (source.CATNAME, source.ITEMNAME, source.MAKE, source.UOM, source.PRICE);
PRINT '+ ITEMDATA sample data inserted';
GO

-- ──────────────────────────────────────────
-- 5. EF Core migration history
-- ──────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
    PRINT '+ Table __EFMigrationsHistory created';
END
GO

-- Mark EF Core InitialCreate migration as already applied
IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = '20260309184431_InitialCreate')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260309184431_InitialCreate', '10.0.3');
    PRINT '+ EF Core migration history seeded';
END
GO

-- Verify counts
SELECT 'LOV_TYPE'   AS TableName, COUNT(*) AS RecordCount FROM [dbo].[LOV_TYPE]
UNION ALL
SELECT 'LOV_MASTER' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[LOV_MASTER]
UNION ALL
SELECT 'ITEMDATA'   AS TableName, COUNT(*) AS RecordCount FROM [dbo].[ITEMDATA];
GO

PRINT '';
PRINT '======================================';
PRINT 'LOVDB initialisation complete';
PRINT '======================================';
GO
-- =====================================================
-- END OF init-database.sql
-- ====================