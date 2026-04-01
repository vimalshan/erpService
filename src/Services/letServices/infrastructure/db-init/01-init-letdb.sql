-- ═══════════════════════════════════════════════════════════════════════
-- LET ERP Database Initialization Script
-- Step 1: Create the LETDB database if it does not exist
-- Module schemas and procedures are loaded separately via sqlcmd
-- ═══════════════════════════════════════════════════════════════════════

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'LETDB')
BEGIN
    CREATE DATABASE [LETDB];
    PRINT 'Database LETDB created.';
END
ELSE
BEGIN
    PRINT 'Database LETDB already exists.';
END
GO

PRINT 'Database initialization step completed.';
GO
