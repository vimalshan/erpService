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
    WHERE [MigrationId] = N'20260317214933_InitialCreate'
)
BEGIN
    CREATE TABLE [ERRSP] (
        [Id] int NOT NULL IDENTITY,
        [ERR_MESS] nvarchar(4000) NULL,
        [ERR_SP] nvarchar(100) NULL,
        [ERR_REF] int NULL,
        [ERR_DATE] datetime2(3) NULL,
        CONSTRAINT [PK_ERRSP] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317214933_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317214933_InitialCreate', N'10.0.5');
END;

COMMIT;
GO

