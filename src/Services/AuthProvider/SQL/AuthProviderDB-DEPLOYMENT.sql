-- ==========================================
-- MODULE : AuthProvider
-- Component : Full Database Deployment
-- Version   : 1.0.0
-- Generated : 2026-03-09
-- Connection: Data Source=(localdb)\MSSQLLocalDB;
--             Integrated Security=True;
--             TrustServerCertificate=True;
--             Application Name="SQL Server Management Studio"
-- ==========================================

USE MASTER;
GO

PRINT '=== AuthProviderDB Deployment ===';
PRINT '';
GO

-- ──────────────────────────────────────────
-- Step 1: Create Database
-- ──────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'AuthProviderDB')
BEGIN
    CREATE DATABASE [AuthProviderDB];
    PRINT '✓ AuthProviderDB database created';
END
ELSE
BEGIN
    PRINT '  AuthProviderDB already exists – skipping create';
END
GO

USE [AuthProviderDB];
GO

-- ──────────────────────────────────────────
-- Step 2: Tables
-- ──────────────────────────────────────────
PRINT 'Creating tables...';
GO

-- Users
IF OBJECT_ID(N'[dbo].[Users]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id]               UNIQUEIDENTIFIER  NOT NULL,
        [Username]         NVARCHAR(50)      NOT NULL,
        [Email]            NVARCHAR(320)     NOT NULL,
        [PasswordHash]     NVARCHAR(256)     NOT NULL,
        [FirstName]        NVARCHAR(100)     NOT NULL,
        [LastName]         NVARCHAR(100)     NOT NULL,
        [IsActive]         BIT               NOT NULL DEFAULT (1),
        [IsEmailVerified]  BIT               NOT NULL DEFAULT (0),
        [CreatedAt]        DATETIME2(7)      NOT NULL DEFAULT (SYSUTCDATETIME()),
        [UpdatedAt]        DATETIME2(7)      NULL,
        [LastLoginAt]      DATETIME2(7)      NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username] ON [dbo].[Users]([Username]);
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email]    ON [dbo].[Users]([Email]);
    PRINT '  ✓ Users';
END
GO

-- Roles
IF OBJECT_ID(N'[dbo].[Roles]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Roles] (
        [Id]          UNIQUEIDENTIFIER NOT NULL,
        [Name]        NVARCHAR(100)    NOT NULL,
        [Description] NVARCHAR(500)    NOT NULL DEFAULT (''),
        [IsActive]    BIT              NOT NULL DEFAULT (1),
        [CreatedAt]   DATETIME2(7)     NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles_Name] ON [dbo].[Roles]([Name]);
    PRINT '  ✓ Roles';
END
GO

-- Permissions
IF OBJECT_ID(N'[dbo].[Permissions]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Permissions] (
        [Id]        UNIQUEIDENTIFIER NOT NULL,
        [Name]      NVARCHAR(200)    NOT NULL,
        [Resource]  NVARCHAR(100)    NOT NULL,
        [Action]    NVARCHAR(50)     NOT NULL,
        [CreatedAt] DATETIME2(7)     NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '  ✓ Permissions';
END
GO

-- UserRoles
IF OBJECT_ID(N'[dbo].[UserRoles]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[UserRoles] (
        [UserId]     UNIQUEIDENTIFIER NOT NULL,
        [RoleId]     UNIQUEIDENTIFIER NOT NULL,
        [AssignedAt] DATETIME2(7)     NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
        CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id]) ON DELETE CASCADE
    );
    PRINT '  ✓ UserRoles';
END
GO

-- RolePermissions
IF OBJECT_ID(N'[dbo].[RolePermissions]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[RolePermissions] (
        [RoleId]       UNIQUEIDENTIFIER NOT NULL,
        [PermissionId] UNIQUEIDENTIFIER NOT NULL,
        [AssignedAt]   DATETIME2(7)     NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED ([RoleId] ASC, [PermissionId] ASC),
        CONSTRAINT [FK_RolePermissions_Roles]       FOREIGN KEY ([RoleId])       REFERENCES [dbo].[Roles]([Id])       ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permissions]([Id]) ON DELETE CASCADE
    );
    PRINT '  ✓ RolePermissions';
END
GO

-- RefreshTokens
IF OBJECT_ID(N'[dbo].[RefreshTokens]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[RefreshTokens] (
        [Id]            UNIQUEIDENTIFIER NOT NULL,
        [UserId]        UNIQUEIDENTIFIER NOT NULL,
        [Token]         NVARCHAR(500)    NOT NULL,
        [ExpiresAt]     DATETIME2(7)     NOT NULL,
        [CreatedAt]     DATETIME2(7)     NOT NULL DEFAULT (SYSUTCDATETIME()),
        [CreatedByIp]   NVARCHAR(50)     NOT NULL DEFAULT (''),
        [RevokedAt]     DATETIME2(7)     NULL,
        [RevokedByIp]   NVARCHAR(50)     NULL,
        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE
    );
    CREATE NONCLUSTERED INDEX [IX_RefreshTokens_Token]  ON [dbo].[RefreshTokens]([Token]);
    CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId] ON [dbo].[RefreshTokens]([UserId]);
    PRINT '  ✓ RefreshTokens';
END
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
        [IpAddress] NVARCHAR(50)     NOT NULL DEFAULT (''),
        [Timestamp] DATETIME2(7)     NOT NULL DEFAULT (SYSUTCDATETIME()),
        [IsSuccess] BIT              NOT NULL DEFAULT (1),
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE NONCLUSTERED INDEX [IX_AuditLogs_UserId]    ON [dbo].[AuditLogs]([UserId]);
    CREATE NONCLUSTERED INDEX [IX_AuditLogs_Timestamp] ON [dbo].[AuditLogs]([Timestamp] DESC);
    PRINT '  ✓ AuditLogs';
END
GO

-- EF Migrations tracking table
IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
    PRINT '  ✓ __EFMigrationsHistory';
END
GO

-- ──────────────────────────────────────────
-- Step 3: Stored Procedures
-- ──────────────────────────────────────────
PRINT '';
PRINT 'Creating stored procedures...';
GO

EXEC sp_executesql N'
CREATE OR ALTER PROCEDURE [dbo].[usp_GetUserSummary]
    @Page INT = 1, @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@Page - 1) * @PageSize;
    SELECT u.[Id], u.[Username], u.[Email], u.[IsActive], u.[CreatedAt],
           u.[FirstName] + N'' '' + u.[LastName] AS FullName
    FROM [dbo].[Users] u
    ORDER BY u.[CreatedAt] DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END';
PRINT '  ✓ usp_GetUserSummary';
GO

EXEC sp_executesql N'
CREATE OR ALTER PROCEDURE [dbo].[usp_GetUserWithRoles]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.[Id], u.[Username], u.[Email], u.[IsActive], r.[Name] AS RoleName
    FROM [dbo].[Users] u
    LEFT JOIN [dbo].[UserRoles] ur ON ur.[UserId] = u.[Id]
    LEFT JOIN [dbo].[Roles]     r  ON r.[Id]      = ur.[RoleId]
    WHERE u.[Id] = @UserId;
END';
PRINT '  ✓ usp_GetUserWithRoles';
GO

EXEC sp_executesql N'
CREATE OR ALTER PROCEDURE [dbo].[usp_GetUserAuditLog]
    @UserId UNIQUEIDENTIFIER, @TopN INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@TopN) al.[Id], al.[Action], al.[Resource], al.[Timestamp], al.[IsSuccess]
    FROM [dbo].[AuditLogs] al
    WHERE al.[UserId] = @UserId
    ORDER BY al.[Timestamp] DESC;
END';
PRINT '  ✓ usp_GetUserAuditLog';
GO

EXEC sp_executesql N'
CREATE OR ALTER PROCEDURE [dbo].[usp_CleanupExpiredTokens]
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[RefreshTokens] WHERE [ExpiresAt] < SYSUTCDATETIME() OR [RevokedAt] IS NOT NULL;
    SELECT @@ROWCOUNT AS DeletedCount;
END';
PRINT '  ✓ usp_CleanupExpiredTokens';
GO

EXEC sp_executesql N'
CREATE OR ALTER PROCEDURE [dbo].[usp_GetDashboardStats]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        (SELECT COUNT(*) FROM [dbo].[Users] WHERE [IsActive]=1)  AS ActiveUsers,
        (SELECT COUNT(*) FROM [dbo].[Users] WHERE [IsActive]=0)  AS InactiveUsers,
        (SELECT COUNT(*) FROM [dbo].[Roles])                     AS TotalRoles,
        (SELECT COUNT(*) FROM [dbo].[RefreshTokens]
            WHERE [ExpiresAt] > SYSUTCDATETIME() AND [RevokedAt] IS NULL) AS ActiveSessions;
END';
PRINT '  ✓ usp_GetDashboardStats';
GO

-- ──────────────────────────────────────────
-- Step 4: Seed / Sample Data
-- ──────────────────────────────────────────
PRINT '';
PRINT 'Seeding reference data...';
GO

-- Roles
IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [Name] = N'ADMIN')
BEGIN
    INSERT INTO [dbo].[Roles] ([Id],[Name],[Description]) VALUES
    ('22222222-0001-0001-0001-000000000001','ADMIN',  'Full system administrator'),
    ('22222222-0002-0001-0001-000000000001','USER',   'Standard end user'),
    ('22222222-0003-0001-0001-000000000001','AUDITOR','Read-only audit access');
    PRINT '  ✓ Roles seeded';
END

-- Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [Resource] = N'users' AND [Action] = N'read')
BEGIN
    INSERT INTO [dbo].[Permissions] ([Id],[Name],[Resource],[Action]) VALUES
    ('11111111-0001-0001-0001-000000000001','View Users',      'users','read'),
    ('11111111-0001-0001-0001-000000000002','Create Users',    'users','create'),
    ('11111111-0001-0001-0001-000000000003','Update Users',    'users','update'),
    ('11111111-0001-0001-0001-000000000004','Delete Users',    'users','delete'),
    ('11111111-0002-0001-0001-000000000001','View Roles',      'roles','read'),
    ('11111111-0002-0001-0001-000000000002','Manage Roles',    'roles','manage'),
    ('11111111-0003-0001-0001-000000000001','View Audit Logs', 'audit','read');
    PRINT '  ✓ Permissions seeded';
END

-- RolePermissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[RolePermissions])
BEGIN
    INSERT INTO [dbo].[RolePermissions] ([RoleId],[PermissionId]) VALUES
    ('22222222-0001-0001-0001-000000000001','11111111-0001-0001-0001-000000000001'),
    ('22222222-0001-0001-0001-000000000001','11111111-0001-0001-0001-000000000002'),
    ('22222222-0001-0001-0001-000000000001','11111111-0001-0001-0001-000000000003'),
    ('22222222-0001-0001-0001-000000000001','11111111-0001-0001-0001-000000000004'),
    ('22222222-0001-0001-0001-000000000001','11111111-0002-0001-0001-000000000001'),
    ('22222222-0001-0001-0001-000000000001','11111111-0002-0001-0001-000000000002'),
    ('22222222-0001-0001-0001-000000000001','11111111-0003-0001-0001-000000000001'),
    ('22222222-0002-0001-0001-000000000001','11111111-0001-0001-0001-000000000001'),
    ('22222222-0002-0001-0001-000000000001','11111111-0001-0001-0001-000000000003'),
    ('22222222-0003-0001-0001-000000000001','11111111-0003-0001-0001-000000000001'),
    ('22222222-0003-0001-0001-000000000001','11111111-0001-0001-0001-000000000001');
    PRINT '  ✓ RolePermissions seeded';
END
GO

-- EF Migrations registration (matches InitialCreate migration)
IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20260309000000_InitialCreate')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] VALUES
    ('20260309000000_InitialCreate', '8.0.0');
    PRINT '  ✓ EF Migration history seeded';
END
GO

-- ──────────────────────────────────────────
-- Final Summary
-- ──────────────────────────────────────────
PRINT '';
PRINT '========================================';
PRINT 'AuthProviderDB Deployment Complete';
PRINT '========================================';
PRINT '';
PRINT 'Database  : AuthProviderDB';
PRINT 'Server    : (localdb)\MSSQLLocalDB';
PRINT '';
PRINT 'Objects Created:';
PRINT '  ✓ 7 Tables';
PRINT '  ✓ 5 Stored Procedures';
PRINT '  ✓ Seed data (Roles, Permissions, RolePermissions)';
PRINT '  ✓ EF Migration history entry';
PRINT '';
PRINT 'Patterns implemented in application layer:';
PRINT '  • API Gateway (ocelot.json)';
PRINT '  • GraphQL (HotChocolate)';
PRINT '  • CORS';
PRINT '  • Minimal APIs';
PRINT '  • API Versioning (v1/v2)';
PRINT '  • CQRS (MediatR)';
PRINT '  • RabbitMQ Message Queue';
PRINT '  • Circuit Breaker + Retry (Polly)';
PRINT '  • Swagger at /swagger/index.html';
PRINT '  • ILogger + Serilog';
PRINT '  • Error Handling Middleware';
PRINT '  • Custom Middleware (CorrelationId, Logging)';
PRINT '  • Application Insights';
PRINT '  • Azure Functions';
PRINT '  • Blob Storage';
PRINT '  • DDD (Entities, Value Objects, Aggregates)';
PRINT '  • Entity Framework Core + Migrations';
PRINT '  • Unit of Work';
PRINT '  • Dapper';
PRINT '  • Repository Pattern';
PRINT '  • Adapter Pattern';
PRINT '  • JWT Authentication + Authorization Policies';
PRINT '========================================';
GO
-- ==========================================
-- END OF AuthProviderDB-DEPLOYMENT.sql
-- ==========================================
