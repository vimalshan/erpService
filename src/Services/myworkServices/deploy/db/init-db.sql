-- ============================================================
-- ERP Microservices – Database Initialization Script
-- Runs inside the sqlserver-init container on first startup.
-- Creates both databases, then the full schema is applied
-- by MYWORKDB.sql and MYWORKDB-procedures.sql.
-- ============================================================

-- Create main application database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'MYWORKDB')
BEGIN
    CREATE DATABASE [MYWORKDB];
    PRINT 'Database MYWORKDB created.';
END
ELSE
    PRINT 'Database MYWORKDB already exists – skipping.';
GO

-- Grant full access to sa (already default, but explicit)
USE [MYWORKDB];
GO

-- ── Per-module USE statements follow in MYWORKDB.sql ──────────────────────────
-- The root MYWORKDB.sql contains all table DDL for every module.
-- The root MYWORKDB-procedures.sql contains all stored procedures.
-- Individual module SQL files (01_AUDIT, 02_CSA, …) are included for reference.
