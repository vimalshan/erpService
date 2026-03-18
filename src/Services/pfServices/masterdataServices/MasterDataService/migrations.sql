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
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [COMP_FINYEAR] (
        [AC_SRL_NUM] bigint NOT NULL,
        [AC_STR_DAT] datetime2 NOT NULL,
        [AC_END_DAT] datetime2 NOT NULL,
        [AC_CLS_FLG] nchar(1) NOT NULL,
        [AC_REMARKS] nvarchar(4000) NULL,
        [AC_INT_FLG] nchar(1) NULL,
        [AC_EMP_NAME] nvarchar(65) NULL,
        [AC_EMP_DESG] nvarchar(65) NULL,
        [AC_BAT_NUM] bigint NULL,
        [Version] int NOT NULL,
        CONSTRAINT [PK_COMP_FINYEAR] PRIMARY KEY ([AC_SRL_NUM])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [COMP_MONTH] (
        [AC_SRL_NUM] bigint NOT NULL,
        [AC_MNT_NAM] nvarchar(15) NULL,
        CONSTRAINT [PK_COMP_MONTH] PRIMARY KEY ([AC_SRL_NUM])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [CONFIGURATION] (
        [CONFIG_ID] int NOT NULL IDENTITY,
        [CONFIG_KEY] nvarchar(100) NOT NULL,
        [CONFIG_VALUE] nvarchar(500) NOT NULL,
        [CONFIG_TYPE] nvarchar(50) NOT NULL,
        [CONFIG_DESCRIPTION] nvarchar(200) NULL,
        [CREATED_DATE] datetime2 NOT NULL,
        [UPDATED_DATE] datetime2 NULL,
        [CREATED_BY] bigint NOT NULL,
        [Version] int NOT NULL,
        CONSTRAINT [PK_CONFIGURATION] PRIMARY KEY ([CONFIG_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [FUND_TYPE_MASTER] (
        [FUND_TYPECODE] nchar(3) NOT NULL,
        [FUND_TYPENAME] nvarchar(25) NOT NULL,
        CONSTRAINT [PK_FUND_TYPE_MASTER] PRIMARY KEY ([FUND_TYPECODE])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [INVCAT_LIMIT] (
        [INVCAT_LIMITID] int NOT NULL,
        [INVCAT_ID] int NOT NULL,
        [INVCAT_MAXPER] int NOT NULL,
        [INVCAT_EFFDATE] datetime2 NOT NULL,
        [INVCAT_CLSDATE] datetime2 NULL,
        CONSTRAINT [PK_INVCAT_LIMIT] PRIMARY KEY ([INVCAT_LIMITID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [INVCATGRP_MAST] (
        [INVGRP_ID] int NOT NULL,
        [INVGRP_SHTNAME] nvarchar(20) NULL,
        [INVGRP_NAME] nvarchar(50) NULL,
        CONSTRAINT [PK_INVCATGRP_MAST] PRIMARY KEY ([INVGRP_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [LOV_MASTER] (
        [LOV_ID] decimal(38,0) NOT NULL,
        [LOV_CODE] nvarchar(10) NOT NULL,
        [LOV_DESC] nvarchar(100) NOT NULL,
        [LOV_VALUE] nvarchar(20) NOT NULL,
        [LOV_CATEGORY] nvarchar(50) NOT NULL,
        [LOV_STATUS] nvarchar(1) NOT NULL DEFAULT N'A',
        [Version] int NOT NULL,
        CONSTRAINT [PK_LOV_MASTER] PRIMARY KEY ([LOV_ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [PF_FINYEARRULES] (
        [PF_FINYEAR_CODE] bigint NOT NULL,
        [PF_FINYEAR_RULES] nvarchar(4000) NULL,
        CONSTRAINT [PK_PF_FINYEARRULES] PRIMARY KEY ([PF_FINYEAR_CODE])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [PF_HRIS] (
        [COM_COD] nchar(3) NOT NULL,
        [EMP_NUM] decimal(38,0) NOT NULL,
        [PIN_NUM] decimal(38,0) NOT NULL,
        CONSTRAINT [PK_PF_HRIS] PRIMARY KEY ([COM_COD], [EMP_NUM])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [PF_MAIN_ACCOUNT] (
        [MAIN_ACC_COD] decimal(38,0) NOT NULL,
        [MAIN_ACC_NAM] nvarchar(60) NOT NULL,
        CONSTRAINT [PK_PF_MAIN_ACCOUNT] PRIMARY KEY ([MAIN_ACC_COD])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [RATE_TYPE_MASTER] (
        [RATE_TYPE_CODE] nchar(3) NOT NULL,
        [RATE_TYPE_NAME] nvarchar(25) NULL,
        CONSTRAINT [PK_RATE_TYPE_MASTER] PRIMARY KEY ([RATE_TYPE_CODE])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [ROLE_MASTER] (
        [ROLE_CODE] bigint NOT NULL,
        [ROLE_NAME] nvarchar(65) NOT NULL,
        [ROLE_DESCRIPTION] nvarchar(200) NULL,
        [ROLE_STATUS] nvarchar(1) NOT NULL DEFAULT N'A',
        [Version] int NOT NULL,
        CONSTRAINT [PK_ROLE_MASTER] PRIMARY KEY ([ROLE_CODE])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [STATUS_MASTER] (
        [STATUS_TYPE] nchar(2) NOT NULL,
        [STATUS_CODE] nchar(2) NOT NULL,
        [STATUS_NAME] nvarchar(65) NULL,
        CONSTRAINT [PK_STATUS_MASTER] PRIMARY KEY ([STATUS_TYPE], [STATUS_CODE])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [INVGRP_LIMIT] (
        [INVGRP_LIMITID] int NOT NULL,
        [INVGRP_ID] int NOT NULL,
        [INVGRP_MAXPER] int NOT NULL,
        [INVGRP_EFFDATE] datetime2 NOT NULL,
        [INVGRP_CLSDATE] datetime2 NULL,
        [INVGRP_RANGE] nvarchar(20) NULL,
        CONSTRAINT [PK_INVGRP_LIMIT] PRIMARY KEY ([INVGRP_LIMITID]),
        CONSTRAINT [FK_INVGRP_LIMIT_INVCATGRP_MAST_INVGRP_ID] FOREIGN KEY ([INVGRP_ID]) REFERENCES [INVCATGRP_MAST] ([INVGRP_ID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [PF_MAIN_SUB] (
        [MAIN_ACC_COD] decimal(38,0) NOT NULL,
        [SUB_ACC_COD] decimal(38,0) NOT NULL,
        CONSTRAINT [PK_PF_MAIN_SUB] PRIMARY KEY ([MAIN_ACC_COD], [SUB_ACC_COD]),
        CONSTRAINT [FK_PF_MAIN_SUB_PF_MAIN_ACCOUNT_MAIN_ACC_COD] FOREIGN KEY ([MAIN_ACC_COD]) REFERENCES [PF_MAIN_ACCOUNT] ([MAIN_ACC_COD]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE TABLE [RATE_MASTER] (
        [RT_TRUST_CODE] nchar(3) NOT NULL,
        [RATE_ID] int NOT NULL,
        [RT_RATE_TYPE_CODE] nchar(3) NULL,
        [RATE_EFF_DATE] nvarchar(255) NULL,
        [RATE_CLS_DATE] nvarchar(255) NULL,
        [RATE_VALUE] decimal(19,0) NULL,
        [RATE_DEL_FLAG] nchar(1) NULL,
        [RT_REWRK_STS] nchar(1) NULL,
        [Version] int NOT NULL,
        CONSTRAINT [PK_RATE_MASTER] PRIMARY KEY ([RT_TRUST_CODE], [RATE_ID]),
        CONSTRAINT [FK_RATE_MASTER_RATE_TYPE_MASTER_RT_RATE_TYPE_CODE] FOREIGN KEY ([RT_RATE_TYPE_CODE]) REFERENCES [RATE_TYPE_MASTER] ([RATE_TYPE_CODE])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IDX_CONFIG_KEY] ON [CONFIGURATION] ([CONFIG_KEY]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_INVGRP_LIMIT_INVGRP_ID] ON [INVGRP_LIMIT] ([INVGRP_ID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE INDEX [IDX_LOV_MASTER_CODE] ON [LOV_MASTER] ([LOV_CATEGORY], [LOV_CODE]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    CREATE INDEX [IDX_RATE_MASTER_TYPE] ON [RATE_MASTER] ([RT_RATE_TYPE_CODE], [RATE_EFF_DATE]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317120229_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317120229_InitialCreate', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317131100_InitialCreate_v2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317131100_InitialCreate_v2', N'10.0.5');
END;

COMMIT;
GO

