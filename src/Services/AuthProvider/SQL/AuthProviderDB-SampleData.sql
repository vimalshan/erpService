-- ==========================================
-- MODULE : AuthProvider
-- Component : Sample Data
-- Database : AuthProviderDB
-- Generated : 2026-03-09
-- NOTE: Passwords are BCrypt hashes of 'Admin@1234' and 'User@1234'
-- ==========================================

USE [AuthProviderDB];
GO

-- ──────────────────────────────────────────
-- Permissions
-- ──────────────────────────────────────────
DELETE FROM [dbo].[RolePermissions];
DELETE FROM [dbo].[Permissions];
GO

INSERT INTO [dbo].[Permissions] ([Id], [Name], [Resource], [Action]) VALUES
-- User permissions
('11111111-0001-0001-0001-000000000001', 'View Users',   'users', 'read'),
('11111111-0001-0001-0001-000000000002', 'Create Users', 'users', 'create'),
('11111111-0001-0001-0001-000000000003', 'Update Users', 'users', 'update'),
('11111111-0001-0001-0001-000000000004', 'Delete Users', 'users', 'delete'),
-- Role permissions
('11111111-0002-0001-0001-000000000001', 'View Roles',   'roles', 'read'),
('11111111-0002-0001-0001-000000000002', 'Manage Roles', 'roles', 'manage'),
-- Audit permissions
('11111111-0003-0001-0001-000000000001', 'View Audit Logs', 'audit', 'read');
GO
PRINT '✓ Permissions inserted';
GO

-- ──────────────────────────────────────────
-- Roles
-- ──────────────────────────────────────────
DELETE FROM [dbo].[UserRoles];
DELETE FROM [dbo].[Roles];
GO

INSERT INTO [dbo].[Roles] ([Id], [Name], [Description]) VALUES
('22222222-0001-0001-0001-000000000001', 'ADMIN',  'Full system administrator – all permissions'),
('22222222-0002-0001-0001-000000000001', 'USER',   'Standard user – can view and update own profile'),
('22222222-0003-0001-0001-000000000001', 'AUDITOR','Read-only access to audit logs');
GO
PRINT '✓ Roles inserted';
GO

-- ──────────────────────────────────────────
-- Role → Permission mapping
-- ──────────────────────────────────────────
-- ADMIN gets all permissions
INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES
('22222222-0001-0001-0001-000000000001', '11111111-0001-0001-0001-000000000001'),
('22222222-0001-0001-0001-000000000001', '11111111-0001-0001-0001-000000000002'),
('22222222-0001-0001-0001-000000000001', '11111111-0001-0001-0001-000000000003'),
('22222222-0001-0001-0001-000000000001', '11111111-0001-0001-0001-000000000004'),
('22222222-0001-0001-0001-000000000001', '11111111-0002-0001-0001-000000000001'),
('22222222-0001-0001-0001-000000000001', '11111111-0002-0001-0001-000000000002'),
('22222222-0001-0001-0001-000000000001', '11111111-0003-0001-0001-000000000001');

-- USER gets view + update own
INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES
('22222222-0002-0001-0001-000000000001', '11111111-0001-0001-0001-000000000001'),
('22222222-0002-0001-0001-000000000001', '11111111-0001-0001-0001-000000000003');

-- AUDITOR gets audit log view
INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES
('22222222-0003-0001-0001-000000000001', '11111111-0003-0001-0001-000000000001'),
('22222222-0003-0001-0001-000000000001', '11111111-0001-0001-0001-000000000001');
GO
PRINT '✓ RolePermissions inserted';
GO

-- ──────────────────────────────────────────
-- Users
-- ──────────────────────────────────────────
DELETE FROM [dbo].[UserRoles];
DELETE FROM [dbo].[RefreshTokens];
DELETE FROM [dbo].[Users];
GO

-- BCrypt hash of 'Admin@1234' (cost=11)
DECLARE @AdminHash NVARCHAR(256) = '$2a$11$xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx';
-- BCrypt hash of 'User@1234'  (cost=11)
DECLARE @UserHash  NVARCHAR(256) = '$2a$11$yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy';

INSERT INTO [dbo].[Users]
    ([Id], [Username], [Email], [PasswordHash], [FirstName], [LastName], [IsActive], [IsEmailVerified], [CreatedAt])
VALUES
-- Admin user
('33333333-0001-0001-0001-000000000001',
 'admin', 'admin@authprovider.local', @AdminHash, 'System', 'Administrator', 1, 1, SYSUTCDATETIME()),

-- Standard users
('33333333-0002-0001-0001-000000000001',
 'john.doe', 'john.doe@example.com', @UserHash, 'John', 'Doe', 1, 1, SYSUTCDATETIME()),

('33333333-0003-0001-0001-000000000001',
 'jane.smith', 'jane.smith@example.com', @UserHash, 'Jane', 'Smith', 1, 1, SYSUTCDATETIME()),

('33333333-0004-0001-0001-000000000001',
 'bob.auditor', 'bob.auditor@example.com', @UserHash, 'Bob', 'Auditor', 1, 1, SYSUTCDATETIME()),

-- Inactive user
('33333333-0005-0001-0001-000000000001',
 'inactive.user', 'inactive@example.com', @UserHash, 'Inactive', 'User', 0, 0, SYSUTCDATETIME());
GO
PRINT '✓ Users inserted';
GO

-- ──────────────────────────────────────────
-- User → Role assignments
-- ──────────────────────────────────────────
INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId]) VALUES
-- admin  → ADMIN
('33333333-0001-0001-0001-000000000001', '22222222-0001-0001-0001-000000000001'),
-- john   → USER
('33333333-0002-0001-0001-000000000001', '22222222-0002-0001-0001-000000000001'),
-- jane   → USER
('33333333-0003-0001-0001-000000000001', '22222222-0002-0001-0001-000000000001'),
-- bob    → AUDITOR
('33333333-0004-0001-0001-000000000001', '22222222-0003-0001-0001-000000000001');
GO
PRINT '✓ UserRoles inserted';
GO

-- ──────────────────────────────────────────
-- Sample AuditLogs
-- ──────────────────────────────────────────
INSERT INTO [dbo].[AuditLogs] ([Id], [UserId], [Action], [Resource], [IpAddress], [IsSuccess]) VALUES
(NEWID(), '33333333-0001-0001-0001-000000000001', 'LOGIN',        'auth',  '127.0.0.1', 1),
(NEWID(), '33333333-0002-0001-0001-000000000001', 'LOGIN',        'auth',  '192.168.1.10', 1),
(NEWID(), NULL,                                   'FAILED_LOGIN', 'auth',  '10.0.0.1', 0),
(NEWID(), '33333333-0001-0001-0001-000000000001', 'CREATE_USER',  'users', '127.0.0.1', 1);
GO
PRINT '✓ AuditLogs inserted';
GO

PRINT '';
PRINT '========================================';
PRINT 'AuthProviderDB Sample Data Loaded';
PRINT '========================================';
GO
-- ==========================================
-- END OF AuthProviderDB-SampleData.sql
-- ==========================================
