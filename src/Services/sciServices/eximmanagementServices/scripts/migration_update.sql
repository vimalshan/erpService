IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    CREATE TABLE [EXIM_DATA_EXPORT] (
        [DATA_ID] bigint NOT NULL IDENTITY,
        [EXIM_DATE] datetime2 NULL,
        [HSCODE] bigint NULL,
        [PRODDESC] nvarchar(500) NULL,
        [PORTDEST] nvarchar(500) NULL,
        [COUNTRYDEST] nvarchar(500) NULL,
        [PORTORIGIN] nvarchar(200) NULL,
        [STDQTY] bigint NULL,
        [STDUNIT] nvarchar(18) NULL,
        [STDUNITRATE] decimal(38,6) NULL,
        [UnitRateDol] bigint NULL,
        [FOBINR] bigint NULL,
        [FOBDOL] bigint NULL,
        [MODESHIP] nvarchar(200) NULL,
        [RecordId] nvarchar(max) NULL,
        [EMONTH] nvarchar(200) NULL,
        [FILE_ID] bigint NULL,
        [EXP_NAME] nvarchar(255) NULL,
        [ExpAdd1] nvarchar(max) NULL,
        [ExpAdd2] nvarchar(max) NULL,
        [ExpCity] nvarchar(max) NULL,
        [ExpState] nvarchar(max) NULL,
        [IMP_NAME] nvarchar(255) NULL,
        [ImpAdd1] nvarchar(max) NULL,
        [ImpAdd2] nvarchar(max) NULL,
        [IMP_COUNTRY] nvarchar(255) NULL,
        [Qty] bigint NULL,
        [Unit] nvarchar(max) NULL,
        [UnitRateInr] nvarchar(max) NULL,
        [UnitRateFc] nvarchar(max) NULL,
        [ValueFc] nvarchar(max) NULL,
        [IEC] nvarchar(200) NULL,
        [SB_NO] nvarchar(255) NULL,
        [INV_NO] nvarchar(255) NULL,
        [ItemNo] nvarchar(max) NULL,
        [DrawBack] nvarchar(max) NULL,
        [CurrentQue] nvarchar(max) NULL,
        [HS2] nvarchar(200) NULL,
        [HS4] nvarchar(200) NULL,
        [InvSlNo] nvarchar(max) NULL,
        [ChallanNo] nvarchar(max) NULL,
        [HS_DESC] nvarchar(255) NULL,
        [ChaPanNo] nvarchar(max) NULL,
        [ChaName] nvarchar(max) NULL,
        [INV_DATE] datetime2 NULL,
        CONSTRAINT [PK_EXIM_DATA_EXPORT] PRIMARY KEY ([DATA_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    CREATE TABLE [EXIM_DATA_IMPORT] (
        [DATA_ID] bigint NOT NULL IDENTITY,
        [EXIM_DATE] datetime2 NULL,
        [HSCODE] bigint NULL,
        [PRODDESC] nvarchar(500) NULL,
        [PORTDEST] nvarchar(500) NULL,
        [COUNTRYORG] nvarchar(500) NULL,
        [STDQTY] decimal(38,6) NULL,
        [STDUNIT] nvarchar(18) NULL,
        [STDUNITRATE] decimal(38,6) NULL,
        [UNITRATEDOL] decimal(38,6) NULL,
        [FOBINR] decimal(38,6) NULL,
        [FOBDOL] decimal(38,6) NULL,
        [ApplicableDutyInr] nvarchar(max) NULL,
        [MODESHIP] nvarchar(200) NULL,
        [RecordId] nvarchar(max) NULL,
        [EMONTH] nvarchar(200) NULL,
        [FILE_ID] bigint NULL,
        [IMP_NAME] nvarchar(255) NULL,
        [ImpAdd1] nvarchar(max) NULL,
        [ImpAdd2] nvarchar(max) NULL,
        [ImpCity] nvarchar(max) NULL,
        [ImpPinCode] nvarchar(max) NULL,
        [ImpState] nvarchar(max) NULL,
        [ImpPhone] nvarchar(max) NULL,
        [ImpEmail] nvarchar(max) NULL,
        [ImpContactPer] nvarchar(max) NULL,
        [EXP_NAME] nvarchar(255) NULL,
        [ExpAdd1] nvarchar(max) NULL,
        [QTY] decimal(38,6) NULL,
        [Unit] nvarchar(max) NULL,
        [UnitRateInr] nvarchar(max) NULL,
        [UnitPriceFc] nvarchar(max) NULL,
        [ActualDutyInr] nvarchar(max) NULL,
        [AvadInr] nvarchar(max) NULL,
        [AvadUsd] nvarchar(max) NULL,
        [PortOrg] nvarchar(max) NULL,
        [ChaPanNo] nvarchar(max) NULL,
        [ChaName] nvarchar(max) NULL,
        [Ag] nvarchar(max) NULL,
        [IEC] nvarchar(200) NULL,
        [BE_NO] nvarchar(255) NULL,
        [InvNo] nvarchar(max) NULL,
        [ItemNo] nvarchar(max) NULL,
        [HS2] nvarchar(200) NULL,
        [HS4] nvarchar(200) NULL,
        [HS_DESC] nvarchar(255) NULL,
        [InvValue] nvarchar(max) NULL,
        [INV_DATE] datetime2 NULL,
        [PossibleDuplicate] nvarchar(max) NULL,
        CONSTRAINT [PK_EXIM_DATA_IMPORT] PRIMARY KEY ([DATA_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    CREATE TABLE [EXIM_DATAFILE] (
        [FILE_ID] bigint NOT NULL IDENTITY,
        [FILE_TYPE] nvarchar(10) NOT NULL,
        [FILE_NAME] nvarchar(200) NULL,
        [ORIGINALCOUNT] bigint NULL,
        [FINALCOUNT] bigint NULL,
        [FILE_UPLOADEDBY] bigint NULL,
        [FILE_UPLOADEDON] datetime2 NOT NULL,
        [REMARKS] nvarchar(1000) NULL,
        [FILE_SOURCE] nvarchar(10) NULL,
        [DEL_FLAG] nvarchar(1) NULL,
        [DELETED_DATE] nvarchar(255) NULL,
        [DELETED_BY] nvarchar(255) NULL,
        [DATATYPE_CODE] nvarchar(1) NULL,
        [DATATYPE_MONTH] nvarchar(255) NULL,
        [DATA_XML] nvarchar(max) NULL,
        CONSTRAINT [PK_EXIM_DATAFILE] PRIMARY KEY ([FILE_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    CREATE TABLE [EXIM_PRODUCT] (
        [PRODUCT_ID] bigint NOT NULL IDENTITY,
        [PRODUCT_NAME] nvarchar(100) NOT NULL,
        [PRODUCT_ORACLE_CODE] nvarchar(50) NULL,
        [LAST_UPDATED_BY] bigint NOT NULL,
        [LAST_UPDATED_ON] datetime2 NOT NULL,
        [STATUS] char(1) NOT NULL,
        CONSTRAINT [PK_EXIM_PRODUCT] PRIMARY KEY ([PRODUCT_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    CREATE TABLE [EXIM_PRODUCT_SEARCH] (
        [SEARCH_ID] bigint NOT NULL IDENTITY,
        [PRODUCT_ID] bigint NOT NULL,
        [SEARCH_ITC_CODE] nvarchar(10) NULL,
        [SEARCH_TEXT] nvarchar(50) NULL,
        [NOTIN_TEXT] nvarchar(50) NULL,
        [LAST_UPDATED_BY] bigint NULL,
        [LAST_UPDATED_ON] datetime2 NOT NULL,
        CONSTRAINT [PK_EXIM_PRODUCT_SEARCH] PRIMARY KEY ([SEARCH_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    CREATE TABLE [EXIM_PRODUCTGROUP] (
        [GROUP_ID] bigint NOT NULL IDENTITY,
        [GROUP_NAME] nvarchar(100) NOT NULL,
        [LAST_UPDATED_BY] bigint NOT NULL,
        [LAST_UPDATED_ON] datetime2 NOT NULL,
        [STATUS] char(1) NOT NULL,
        CONSTRAINT [PK_EXIM_PRODUCTGROUP] PRIMARY KEY ([GROUP_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    CREATE TABLE [EXIM_PRODUCTGROUP_MAP] (
        [MAP_ID] bigint NOT NULL IDENTITY,
        [GROUP_ID] bigint NOT NULL,
        [PRODUCT_ID] bigint NOT NULL,
        [LAST_UPDATED_BY] bigint NOT NULL,
        [LAST_UPDATED_ON] datetime2 NOT NULL,
        CONSTRAINT [PK_EXIM_PRODUCTGROUP_MAP] PRIMARY KEY ([MAP_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    CREATE TABLE [EXIM_USERMASTER] (
        [EXIM_USERID] bigint NOT NULL IDENTITY,
        [EXIM_EMPSYSID] bigint NULL,
        [EXIM_SPARSHID] nvarchar(50) NULL,
        [EXIM_USER_EFFECTIVEDATE] datetime2 NOT NULL,
        [EXIM_USER_CLOSUREDATE] datetime2 NULL,
        [EXIM_USER_ENTEREDBY] bigint NOT NULL,
        CONSTRAINT [PK_EXIM_USERMASTER] PRIMARY KEY ([EXIM_USERID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PRODUCT_ID', N'LAST_UPDATED_BY', N'LAST_UPDATED_ON', N'PRODUCT_NAME', N'PRODUCT_ORACLE_CODE', N'STATUS') AND [object_id] = OBJECT_ID(N'[EXIM_PRODUCT]'))
        SET IDENTITY_INSERT [EXIM_PRODUCT] ON;
    EXEC(N'INSERT INTO [EXIM_PRODUCT] ([PRODUCT_ID], [LAST_UPDATED_BY], [LAST_UPDATED_ON], [PRODUCT_NAME], [PRODUCT_ORACLE_CODE], [STATUS])
    VALUES (CAST(1001 AS bigint), CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', N''Cotton Yarn'', N''CY001'', ''A''),
    (CAST(1002 AS bigint), CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', N''Polyester Fabric'', N''PF001'', ''A''),
    (CAST(1003 AS bigint), CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', N''Denim Cloth'', N''DC001'', ''A''),
    (CAST(1004 AS bigint), CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', N''Silk Threads'', N''ST001'', ''A''),
    (CAST(1005 AS bigint), CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', N''Woollen Yarn'', N''WY001'', ''A'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PRODUCT_ID', N'LAST_UPDATED_BY', N'LAST_UPDATED_ON', N'PRODUCT_NAME', N'PRODUCT_ORACLE_CODE', N'STATUS') AND [object_id] = OBJECT_ID(N'[EXIM_PRODUCT]'))
        SET IDENTITY_INSERT [EXIM_PRODUCT] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'GROUP_ID', N'GROUP_NAME', N'LAST_UPDATED_BY', N'LAST_UPDATED_ON', N'STATUS') AND [object_id] = OBJECT_ID(N'[EXIM_PRODUCTGROUP]'))
        SET IDENTITY_INSERT [EXIM_PRODUCTGROUP] ON;
    EXEC(N'INSERT INTO [EXIM_PRODUCTGROUP] ([GROUP_ID], [GROUP_NAME], [LAST_UPDATED_BY], [LAST_UPDATED_ON], [STATUS])
    VALUES (CAST(101 AS bigint), N''Textile Yarns'', CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', ''A''),
    (CAST(102 AS bigint), N''Woven Fabrics'', CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', ''A''),
    (CAST(103 AS bigint), N''Denim Products'', CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', ''A''),
    (CAST(104 AS bigint), N''Silk Products'', CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', ''A''),
    (CAST(105 AS bigint), N''Woollen Products'', CAST(1 AS bigint), ''2024-01-01T00:00:00.0000000'', ''A'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'GROUP_ID', N'GROUP_NAME', N'LAST_UPDATED_BY', N'LAST_UPDATED_ON', N'STATUS') AND [object_id] = OBJECT_ID(N'[EXIM_PRODUCTGROUP]'))
        SET IDENTITY_INSERT [EXIM_PRODUCTGROUP] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317223812_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317223812_InitialCreate', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCT] SET [STATUS] = ''Y''
    WHERE [PRODUCT_ID] = CAST(1001 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCT] SET [STATUS] = ''Y''
    WHERE [PRODUCT_ID] = CAST(1002 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCT] SET [STATUS] = ''Y''
    WHERE [PRODUCT_ID] = CAST(1003 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCT] SET [STATUS] = ''Y''
    WHERE [PRODUCT_ID] = CAST(1004 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCT] SET [STATUS] = ''Y''
    WHERE [PRODUCT_ID] = CAST(1005 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCTGROUP] SET [STATUS] = ''Y''
    WHERE [GROUP_ID] = CAST(101 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCTGROUP] SET [STATUS] = ''Y''
    WHERE [GROUP_ID] = CAST(102 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCTGROUP] SET [STATUS] = ''Y''
    WHERE [GROUP_ID] = CAST(103 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCTGROUP] SET [STATUS] = ''Y''
    WHERE [GROUP_ID] = CAST(104 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    EXEC(N'UPDATE [EXIM_PRODUCTGROUP] SET [STATUS] = ''Y''
    WHERE [GROUP_ID] = CAST(105 AS bigint);
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317224647_FixSeedDataStatus'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317224647_FixSeedDataStatus', N'10.0.5');
END;

COMMIT;
GO

