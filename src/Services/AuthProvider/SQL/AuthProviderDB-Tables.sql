-- ==========================================
-- MODULE : AuthProvider
-- Component : Tables
-- Database : AuthProviderDB
-- Connection: Data Source=(localdb)\MSSQLLocalDB;
--             Initial Catalog=AuthProviderDB;
--             Integrated Security=True;
--             TrustServerCertificate=True
-- Generated : 2026-03-09
-- ==========================================

USE [AuthProviderDB];
GO

-- ──────────────────────────────────────────
-- Table: Users  (DDD Aggregate Root)
-- ──────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[Users]', N'U') IS NOT NULL DROP TABLE [dbo].[Users];
GO

CREATE TABLE [dbo].[Users] (
    [Id]               UNIQUEIDENTIFIER  NOT NULL,
    [Username]         NVARCHAR(50)      NOT NULL,
    [Email]            NVARCHAR(320)     NOT NULL,
    [PasswordHash]     NVARCHAR(256)     NOT NULL,
    [FirstName]        NVARCHAR(100)     NOT NULL,
    [LastName]         NVARCHAR(100)     NOT NULL,
    [IsActive]         BIT               NOT NULL CONSTRAINT [DF_Users_IsActive]         DEFAULT (1),
    [IsEmailVerified]  BIT               NOT NULL CONSTRAINT [DF_Users_IsEmailVerified]  DEFAULT (0),
    [CreatedAt]        DATETIME2(7)      NOT NULL CONSTRAINT [DF_Users_CreatedAt]         DEFAULT (SYSUTCDATETIME()),
    [UpdatedAt]        DATETIME2(7)      NULL,
    [LastLoginAt]      DATETIME2(7)      NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username] ON [dbo].[Users]([Username]);
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email]    ON [dbo].[Users]([Email]);
GO

-- ──────────────────────────────────────────
-- Table: Roles
-- ──────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[Roles]', N'U') IS NOT NULL DROP TABLE [dbo].[Roles];
GO

CREATE TABLE [dbo].[Roles] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR(100)    NOT NULL,
    [Description] NVARCHAR(500)    NOT NULL CONSTRAINT [DF_Roles_Description] DEFAULT (''),
    [IsActive]    BIT              NOT NULL CONSTRAINT [DF_Roles_IsActive]    DEFAULT (1),
    [CreatedAt]   DATETIME2(7)     NOT NULL CONSTRAINT [DF_Roles_CreatedAt]   DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles_Name] ON [dbo].[Roles]([Name]);
GO

-- ──────────────────────────────────────────
-- Table: Permissions
-- ──────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[Permissions]', N'U') IS NOT NULL DROP TABLE [dbo].[Permissions];
GO

CREATE TABLE [dbo].[Permissions] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Name]      NVARCHAR(200)    NOT NULL,
    [Resource]  NVARCHAR(100)    NOT NULL,
    [Action]    NVARCHAR(50)     NOT NULL,
    [CreatedAt] DATETIME2(7)     NOT NULL CONSTRAINT [DF_Permissions_CreatedAt] DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- ──────────────────────────────────────────
-- Table: UserRoles  (many-to-many)
-- ──────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[UserRoles]', N'U') IS NOT NULL DROP TABLE [dbo].[UserRoles];
GO

CREATE TABLE [dbo].[UserRoles] (
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [RoleId]     UNIQUEIDENTIFIER NOT NULL,
    [AssignedAt] DATETIME2(7)     NOT NULL CONSTRAINT [DF_UserRoles_AssignedAt] DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id]) ON DELETE CASCADE
);
GO

-- ──────────────────────────────────────────
-- Table: RolePermissions  (many-to-many)
-- ──────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[RolePermissions]', N'U') IS NOT NULL DROP TABLE [dbo].[RolePermissions];
GO

CREATE TABLE [dbo].[RolePermissions] (
    [RoleId]       UNIQUEIDENTIFIER NOT NULL,
    [PermissionId] UNIQUEIDENTIFIER NOT NULL,
    [AssignedAt]   DATETIME2(7)     NOT NULL CONSTRAINT [DF_RolePermissions_AssignedAt] DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED ([RoleId] ASC, [PermissionId] ASC),
    CONSTRAINT [FK_RolePermissions_Roles]       FOREIGN KEY ([RoleId])       REFERENCES [dbo].[Roles]([Id])       ON DELETE CASCADE,
    CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permissions]([Id]) ON DELETE CASCADE
);
GO

-- ──────────────────────────────────────────
-- Table: RefreshTokens
-- ──────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[RefreshTokens]', N'U') IS NOT NULL DROP TABLE [dbo].[RefreshTokens];
GO

CREATE TABLE [dbo].[RefreshTokens] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [Token]         NVARCHAR(500)    NOT NULL,
    [ExpiresAt]     DATETIME2(7)     NOT NULL,
    [CreatedAt]     DATETIME2(7)     NOT NULL CONSTRAINT [DF_RefreshTokens_CreatedAt]  DEFAULT (SYSUTCDATETIME()),
    [CreatedByIp]   NVARCHAR(50)     NOT NULL CONSTRAINT [DF_RefreshTokens_CreatedByIp] DEFAULT (''),
    [RevokedAt]     DATETIME2(7)     NULL,
    [RevokedByIp]   NVARCHAR(50)     NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_RefreshTokens_Token]  ON [dbo].[RefreshTokens]([Token]);
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId] ON [dbo].[RefreshTokens]([UserId]);
GO

-- ──────────────────────────────────────────
-- Table: AuditLogs
-- ──────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[AuditLogs]', N'U') IS NOT NULL DROP TABLE [dbo].[AuditLogs];
GO

CREATE TABLE [dbo].[AuditLogs] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NULL,
    [Action]    NVARCHAR(100)    NOT NULL,
    [Resource]  NVARCHAR(200)    NOT NULL,
    [Details]   NVARCHAR(2000)   NULL,
    [IpAddress] NVARCHAR(50)     NOT NULL CONSTRAINT [DF_AuditLogs_IpAddress]  DEFAULT (''),
    [Timestamp] DATETIME2(7)     NOT NULL CONSTRAINT [DF_AuditLogs_Timestamp]  DEFAULT (SYSUTCDATETIME()),
    [IsSuccess] BIT              NOT NULL CONSTRAINT [DF_AuditLogs_IsSuccess]  DEFAULT (1),
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_AuditLogs_UserId]    ON [dbo].[AuditLogs]([UserId]);
CREATE NONCLUSTERED INDEX [IX_AuditLogs_Timestamp] ON [dbo].[AuditLogs]([Timestamp] DESC);
GO

-- ═════════════════════════════════════════
-- EF Core __EFMigrationsHistory tracking
-- ═════════════════════════════════════════
IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END
GO

PRINT '✓ AuthProviderDB Tables created successfully';
GO
-- ==========================================
-- END OF AuthProvider-Tables.sql
-- ==========================================
