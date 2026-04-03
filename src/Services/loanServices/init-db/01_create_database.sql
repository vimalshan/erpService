-- =============================================================================
--  01_create_database.sql
--  Creates the LOANDB database and login for the application.
--  Run once on container first start.
-- =============================================================================

-- Create the database if it does not exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'LOANDB')
BEGIN
    CREATE DATABASE LOANDB
        COLLATE Latin1_General_CI_AS;
    PRINT 'Database LOANDB created.';
END
ELSE
BEGIN
    PRINT 'Database LOANDB already exists — skipping.';
END
GO

USE LOANDB;
GO

-- Create application login (password overridden at runtime via env var)
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'loan_app')
BEGIN
    CREATE LOGIN loan_app
        WITH PASSWORD = 'LoanERP_AppUser!2025',
             CHECK_EXPIRATION = OFF,
             CHECK_POLICY = OFF;
    PRINT 'Login loan_app created.';
END

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'loan_app')
BEGIN
    CREATE USER loan_app FOR LOGIN loan_app;
    ALTER ROLE db_datareader  ADD MEMBER loan_app;
    ALTER ROLE db_datawriter  ADD MEMBER loan_app;
    ALTER ROLE db_ddladmin    ADD MEMBER loan_app;
    PRINT 'User loan_app created and granted roles.';
END
GO
