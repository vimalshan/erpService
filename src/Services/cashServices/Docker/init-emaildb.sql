-- ============================================================================
-- EmailNotificationDb Initialization Script
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EmailNotificationDb')
BEGIN
    CREATE DATABASE EmailNotificationDb;
END
GO

USE EmailNotificationDb;
GO

-- Tables are auto-created by EF Core migrations
-- This script serves as fallback for manual DB setup

PRINT 'EmailNotificationDb initialization completed successfully.'
GO
