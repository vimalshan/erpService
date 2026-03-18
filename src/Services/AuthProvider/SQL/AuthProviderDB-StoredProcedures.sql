-- ==========================================
-- MODULE : AuthProvider
-- Component : Stored Procedures
-- Database : AuthProviderDB
-- Generated : 2026-03-09
-- ==========================================

USE [AuthProviderDB];
GO

-- ──────────────────────────────────────────
-- usp_GetUserSummary  (used by Dapper layer)
-- Paged list of users with computed full name
-- ──────────────────────────────────────────
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
PRINT '✓ usp_GetUserSummary created';
GO

-- ──────────────────────────────────────────
-- usp_GetUserWithRoles  (used by Dapper layer)
-- Returns user with one row per role (multi-map)
-- ──────────────────────────────────────────
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
PRINT '✓ usp_GetUserWithRoles created';
GO

-- ──────────────────────────────────────────
-- usp_GetUserAuditLog  (used by Dapper layer)
-- ──────────────────────────────────────────
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
PRINT '✓ usp_GetUserAuditLog created';
GO

-- ──────────────────────────────────────────
-- usp_CreateUser  (EF alternative for bulk imports)
-- ──────────────────────────────────────────
CREATE OR ALTER PROCEDURE [dbo].[usp_CreateUser]
    @Id               UNIQUEIDENTIFIER,
    @Username         NVARCHAR(50),
    @Email            NVARCHAR(320),
    @PasswordHash     NVARCHAR(256),
    @FirstName        NVARCHAR(100),
    @LastName         NVARCHAR(100)
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
PRINT '✓ usp_CreateUser created';
GO

-- ──────────────────────────────────────────
-- usp_AssignRole
-- ──────────────────────────────────────────
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
PRINT '✓ usp_AssignRole created';
GO

-- ──────────────────────────────────────────
-- usp_CleanupExpiredTokens  (called by Azure Function)
-- ──────────────────────────────────────────
CREATE OR ALTER PROCEDURE [dbo].[usp_CleanupExpiredTokens]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Deleted INT = 0;

    DELETE FROM [dbo].[RefreshTokens]
    WHERE [ExpiresAt] < SYSUTCDATETIME()
       OR [RevokedAt] IS NOT NULL;

    SET @Deleted = @@ROWCOUNT;
    SELECT @Deleted AS DeletedCount;
END
GO
PRINT '✓ usp_CleanupExpiredTokens created';
GO

-- ──────────────────────────────────────────
-- usp_GetDashboardStats  (admin reporting)
-- ──────────────────────────────────────────
CREATE OR ALTER PROCEDURE [dbo].[usp_GetDashboardStats]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        (SELECT COUNT(*)  FROM [dbo].[Users]         WHERE [IsActive] = 1)    AS ActiveUsers,
        (SELECT COUNT(*)  FROM [dbo].[Users]         WHERE [IsActive] = 0)    AS InactiveUsers,
        (SELECT COUNT(*)  FROM [dbo].[Roles])                                 AS TotalRoles,
        (SELECT COUNT(*)  FROM [dbo].[RefreshTokens] WHERE [ExpiresAt] > SYSUTCDATETIME()
                                                       AND [RevokedAt] IS NULL) AS ActiveSessions,
        (SELECT COUNT(*)  FROM [dbo].[AuditLogs]     WHERE [Timestamp] >= DATEADD(DAY, -1, SYSUTCDATETIME())) AS LogsLast24h;
END
GO
PRINT '✓ usp_GetDashboardStats created';
GO

PRINT '';
PRINT '========================================';
PRINT 'All AuthProvider Stored Procedures Created';
PRINT '========================================';
GO
-- ==========================================
-- END OF AuthProviderDB-StoredProcedures.sql
-- ==========================================
