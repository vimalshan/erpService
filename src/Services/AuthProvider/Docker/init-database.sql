-- =====================================================
-- Docker init script for AuthProviderDB
-- Runs on first container startup via sqlcmd
-- =====================================================

USE MASTER;
GO

-- ──────────────────────────────────────────
-- 1. Create database
-- ──────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'AuthProviderDB')
BEGIN
    CREATE DATABASE [AuthProviderDB];
    PRINT '+ AuthProviderDB created';
END
ELSE
    PRINT '+ AuthProviderDB already exists';
GO

USE [AuthProviderDB];
GO

-- ──────────────────────────────────────────
-- 2. Tables
-- ──────────────────────────────────────────

-- Users (DDD Aggregate Root)
IF OBJECT_ID(N'[dbo].[Users]', N'U') IS NULL
BEGIN
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
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username] ON [dbo].[Users]([Username]);
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email]    ON [dbo].[Users]([Email]);
    PRINT '+ Table Users created';
END
ELSE
    PRINT '+ Table Users already exists';
GO

-- Roles
IF OBJECT_ID(N'[dbo].[Roles]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Roles] (
        [Id]          UNIQUEIDENTIFIER NOT NULL,
        [Name]        NVARCHAR(100)    NOT NULL,
        [Description] NVARCHAR(500)    NOT NULL CONSTRAINT [DF_Roles_Description] DEFAULT (''),
        [IsActive]    BIT              NOT NULL CONSTRAINT [DF_Roles_IsActive]    DEFAULT (1),
        [CreatedAt]   DATETIME2(7)     NOT NULL CONSTRAINT [DF_Roles_CreatedAt]   DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles_Name] ON [dbo].[Roles]([Name]);
    PRINT '+ Table Roles created';
END
ELSE
    PRINT '+ Table Roles already exists';
GO

-- Permissions
IF OBJECT_ID(N'[dbo].[Permissions]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Permissions] (
        [Id]        UNIQUEIDENTIFIER NOT NULL,
        [Name]      NVARCHAR(200)    NOT NULL,
        [Resource]  NVARCHAR(100)    NOT NULL,
        [Action]    NVARCHAR(50)     NOT NULL,
        [CreatedAt] DATETIME2(7)     NOT NULL CONSTRAINT [DF_Permissions_CreatedAt] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '+ Table Permissions created';
END
ELSE
    PRINT '+ Table Permissions already exists';
GO

-- UserRoles  (many-to-many)
IF OBJECT_ID(N'[dbo].[UserRoles]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[UserRoles] (
        [UserId]     UNIQUEIDENTIFIER NOT NULL,
        [RoleId]     UNIQUEIDENTIFIER NOT NULL,
        [AssignedAt] DATETIME2(7)     NOT NULL CONSTRAINT [DF_UserRoles_AssignedAt] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
        CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id]) ON DELETE CASCADE
    );
    PRINT '+ Table UserRoles created';
END
ELSE
    PRINT '+ Table UserRoles already exists';
GO

-- RolePermissions  (many-to-many)
IF OBJECT_ID(N'[dbo].[RolePermissions]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[RolePermissions] (
        [RoleId]       UNIQUEIDENTIFIER NOT NULL,
        [PermissionId] UNIQUEIDENTIFIER NOT NULL,
        [AssignedAt]   DATETIME2(7)     NOT NULL CONSTRAINT [DF_RolePermissions_AssignedAt] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED ([RoleId] ASC, [PermissionId] ASC),
        CONSTRAINT [FK_RolePermissions_Roles]       FOREIGN KEY ([RoleId])       REFERENCES [dbo].[Roles]([Id])       ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permissions]([Id]) ON DELETE CASCADE
    );
    PRINT '+ Table RolePermissions created';
END
ELSE
    PRINT '+ Table RolePermissions already exists';
GO

-- RefreshTokens
IF OBJECT_ID(N'[dbo].[RefreshTokens]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[RefreshTokens] (
        [Id]          UNIQUEIDENTIFIER NOT NULL,
        [UserId]      UNIQUEIDENTIFIER NOT NULL,
        [Token]       NVARCHAR(500)    NOT NULL,
        [ExpiresAt]   DATETIME2(7)     NOT NULL,
        [CreatedAt]   DATETIME2(7)     NOT NULL CONSTRAINT [DF_RefreshTokens_CreatedAt]   DEFAULT (SYSUTCDATETIME()),
        [CreatedByIp] NVARCHAR(50)     NOT NULL CONSTRAINT [DF_RefreshTokens_CreatedByIp] DEFAULT (''),
        [RevokedAt]   DATETIME2(7)     NULL,
        [RevokedByIp] NVARCHAR(50)     NULL,
        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE
    );
    CREATE NONCLUSTERED INDEX [IX_RefreshTokens_Token]  ON [dbo].[RefreshTokens]([Token]);
    CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId] ON [dbo].[RefreshTokens]([UserId]);
    PRINT '+ Table RefreshTokens created';
END
ELSE
    PRINT '+ Table RefreshTokens already exists';
GO

-- AuditLogs
IF OBJECT_ID(N'[dbo].[AuditLogs]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[AuditLogs] (
        [Id]        UNIQUEIDENTIFIER NOT NULL,
        [UserId]    UNIQUEIDENTIFIER NULL,
        [Action]    NVARCHAR(100)    NOT NULL,
        [Resource]  NVARCHAR(200)    NOT NULL,
        [Details]   NVARCHAR(2000)   NULL,
        [IpAddress] NVARCHAR(50)     NOT NULL CONSTRAINT [DF_AuditLogs_IpAddress] DEFAULT (''),
        [Timestamp] DATETIME2(7)     NOT NULL CONSTRAINT [DF_AuditLogs_Timestamp] DEFAULT (SYSUTCDATETIME()),
        [IsSuccess] BIT              NOT NULL CONSTRAINT [DF_AuditLogs_IsSuccess] DEFAULT (1),
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '+ Table AuditLogs created';
END
ELSE
    PRINT '+ Table AuditLogs already exists';
GO

-- ──────────────────────────────────────────
-- 3. Stored Procedures
-- ──────────────────────────────────────────

-- usp_GetUserSummary: paged list of users with computed full name
CREATE OR ALTER PROCEDURE [dbo].[usp_GetUserSummary]
    @Page     INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    SELECT
        u.[Id],
        u.[Username],
        u.[Email],
        u.[IsActive],
        u.[CreatedAt],
        u.[FirstName] + N' ' + u.[LastName] AS FullName
    FROM [dbo].[Users] u
    ORDER BY u.[CreatedAt] DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
PRINT '+ usp_GetUserSummary created';
GO

-- usp_GetUserWithRoles: returns user with one row per role
CREATE OR ALTER PROCEDURE [dbo].[usp_GetUserWithRoles]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.[Id],
        u.[Username],
        u.[Email],
        u.[IsActive],
        r.[Name] AS RoleName
    FROM [dbo].[Users] u
    LEFT JOIN [dbo].[UserRoles] ur ON ur.[UserId] = u.[Id]
    LEFT JOIN [dbo].[Roles]     r  ON r.[Id]      = ur.[RoleId]
    WHERE u.[Id] = @UserId;
END
GO
PRINT '+ usp_GetUserWithRoles created';
GO

-- usp_GetUserAuditLog: recent audit entries for a user
CREATE OR ALTER PROCEDURE [dbo].[usp_GetUserAuditLog]
    @UserId UNIQUEIDENTIFIER,
    @TopN   INT = 50
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TopN)
        al.[Id],
        al.[Action],
        al.[Resource],
        al.[Timestamp],
        al.[IsSuccess],
        al.[IpAddress],
        al.[Details]
    FROM [dbo].[AuditLogs] al
    WHERE al.[UserId] = @UserId
    ORDER BY al.[Timestamp] DESC;
END
GO
PRINT '+ usp_GetUserAuditLog created';
GO

-- usp_CreateUser: insert a new user (duplicate-email guard)
CREATE OR ALTER PROCEDURE [dbo].[usp_CreateUser]
    @Id           UNIQUEIDENTIFIER,
    @Username     NVARCHAR(50),
    @Email        NVARCHAR(320),
    @PasswordHash NVARCHAR(256),
    @FirstName    NVARCHAR(100),
    @LastName     NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Email] = @Email)
    BEGIN
        RAISERROR(N'Email already registered.', 16, 1);
        RETURN;
    END

    INSERT INTO [dbo].[Users]
        ([Id], [Username], [Email], [PasswordHash], [FirstName], [LastName],
         [IsActive], [IsEmailVerified], [CreatedAt])
    VALUES
        (@Id, @Username, @Email, @PasswordHash, @FirstName, @LastName,
         1, 0, SYSUTCDATETIME());

    SELECT * FROM [dbo].[Users] WHERE [Id] = @Id;
END
GO
PRINT '+ usp_CreateUser created';
GO

-- usp_AssignRole: idempotent role assignment
CREATE OR ALTER PROCEDURE [dbo].[usp_AssignRole]
    @UserId UNIQUEIDENTIFIER,
    @RoleId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [dbo].[UserRoles] WHERE [UserId] = @UserId AND [RoleId] = @RoleId)
    BEGIN
        INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId]) VALUES (@UserId, @RoleId);
    END
END
GO
PRINT '+ usp_AssignRole created';
GO

-- usp_CleanupExpiredTokens: called by Azure Function timer trigger
CREATE OR ALTER PROCEDURE [dbo].[usp_CleanupExpiredTokens]
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[RefreshTokens]
    WHERE [ExpiresAt] < SYSUTCDATETIME()
       OR [RevokedAt] IS NOT NULL;

    SELECT @@ROWCOUNT AS DeletedCount;
END
GO
PRINT '+ usp_CleanupExpiredTokens created';
GO

-- usp_GetDashboardStats: admin summary counters
CREATE OR ALTER PROCEDURE [dbo].[usp_GetDashboardStats]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        (SELECT COUNT(*) FROM [dbo].[Users]         WHERE [IsActive] = 1)                                              AS ActiveUsers,
        (SELECT COUNT(*) FROM [dbo].[Users]         WHERE [IsActive] = 0)                                              AS InactiveUsers,
        (SELECT COUNT(*) FROM [dbo].[Roles])                                                                            AS TotalRoles,
        (SELECT COUNT(*) FROM [dbo].[RefreshTokens] WHERE [ExpiresAt] > SYSUTCDATETIME() AND [RevokedAt] IS NULL)      AS ActiveSessions,
        (SELECT COUNT(*) FROM [dbo].[AuditLogs]     WHERE [Timestamp] >= DATEADD(DAY, -1, SYSUTCDATETIME()))           AS LogsLast24h;
END
GO
PRINT '+ usp_GetDashboardStats created';
GO

PRINT '';
PRINT '======================================';
PRINT 'AuthProviderDB initialisation complete';
PRINT '======================================';
GO
-- =====================================================
-- END OF init-database.sql
-- =====================================================
