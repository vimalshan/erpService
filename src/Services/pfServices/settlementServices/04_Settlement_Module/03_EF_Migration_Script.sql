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
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    CREATE TABLE [SET_MAIN] (
        [ST_SET_NUM] bigint NOT NULL,
        [ST_TRUST_CODE] nchar(3) NULL,
        [ST_MEMBER_NO] bigint NULL,
        [ST_SET_TYPE] nchar(1) NULL,
        [ST_SET_DATE] datetime2(3) NULL,
        [ST_DOL_DAT] datetime2(3) NULL,
        [ST_REASON] nvarchar(200) NULL,
        [ST_UPDON] datetime2(3) NULL,
        [ST_UPDBY_EMP_SYSID] bigint NULL,
        [ST_ACC_DATE] datetime2(3) NULL,
        [ST_FINYEAR] bigint NULL,
        [ST_JV_VOUCHER_TYPE] nchar(3) NULL,
        [ST_JV_NO] bigint NULL,
        [ST_SET_INT_FLG] nchar(1) NULL,
        [ST_TAXSTS] nvarchar(200) NULL,
        [ST_TAXRATE] bigint NULL,
        [ST_SETTLEMENT_AMOUNT] decimal(19,0) NULL,
        [ST_STATUS] nvarchar(1) NOT NULL DEFAULT N'P',
        CONSTRAINT [PK_SET_MAIN] PRIMARY KEY ([ST_SET_NUM])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    CREATE TABLE [SET_APPROVAL] (
        [APR_ID] bigint NOT NULL IDENTITY,
        [SET_NUM] bigint NOT NULL,
        [APR_LEVEL] int NOT NULL,
        [APR_BY_SYSID] bigint NOT NULL,
        [APR_STATUS] nvarchar(1) NOT NULL,
        [APR_REMARKS] nvarchar(200) NULL,
        [APR_DATE] datetime2(3) NOT NULL,
        CONSTRAINT [PK_SET_APPROVAL] PRIMARY KEY ([APR_ID]),
        CONSTRAINT [FK_SET_APPROVAL_SET_MAIN_SET_NUM] FOREIGN KEY ([SET_NUM]) REFERENCES [SET_MAIN] ([ST_SET_NUM]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    CREATE TABLE [SET_DEDUCTION] (
        [SET_DED_ID] bigint NOT NULL IDENTITY,
        [SET_NUM] bigint NOT NULL,
        [DED_TYPE] nvarchar(50) NOT NULL,
        [DED_AMOUNT] decimal(19,0) NOT NULL,
        [CREATED_ON] datetime2(3) NOT NULL,
        CONSTRAINT [PK_SET_DEDUCTION] PRIMARY KEY ([SET_DED_ID]),
        CONSTRAINT [FK_SET_DEDUCTION_SET_MAIN_SET_NUM] FOREIGN KEY ([SET_NUM]) REFERENCES [SET_MAIN] ([ST_SET_NUM]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    CREATE TABLE [SET_PAYMENT] (
        [PAY_ID] bigint NOT NULL IDENTITY,
        [SET_NUM] bigint NOT NULL,
        [PAY_MODE] nvarchar(20) NOT NULL,
        [PAY_AMOUNT] decimal(19,0) NOT NULL,
        [PAY_DATE] datetime2(3) NOT NULL,
        [PAY_REF_NO] nvarchar(50) NULL,
        [PAY_STATUS] nvarchar(1) NOT NULL DEFAULT N'P',
        CONSTRAINT [PK_SET_PAYMENT] PRIMARY KEY ([PAY_ID]),
        CONSTRAINT [FK_SET_PAYMENT_SET_MAIN_SET_NUM] FOREIGN KEY ([SET_NUM]) REFERENCES [SET_MAIN] ([ST_SET_NUM]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SET_APPROVAL_SET_NUM] ON [SET_APPROVAL] ([SET_NUM]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SET_DEDUCTION_SET_NUM] ON [SET_DEDUCTION] ([SET_NUM]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    CREATE INDEX [IDX_SET_MAIN_MEMBER] ON [SET_MAIN] ([ST_MEMBER_NO]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    CREATE INDEX [IDX_SET_MAIN_STATUS] ON [SET_MAIN] ([ST_STATUS], [ST_SET_DATE]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SET_PAYMENT_SET_NUM] ON [SET_PAYMENT] ([SET_NUM]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317211735_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317211735_InitialCreate', N'10.0.5');
END;

COMMIT;
GO

