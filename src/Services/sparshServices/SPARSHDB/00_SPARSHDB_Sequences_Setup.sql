-- ============================================================================
-- SPARSHDB Module Initialization Script
-- Purpose: Create all sequences and setup database for modular deployment
-- Created: March 9, 2026
-- ============================================================================

USE [SPARSHDB];
GO

-- ============================================================================
-- SEQUENCES FOR IDENTITY GENERATION
-- ============================================================================

-- Mobile App Management Sequences
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOB_LoginId')
BEGIN
    CREATE SEQUENCE dbo.seq_MOB_LoginId
        AS DECIMAL(38)
        START WITH 1
        INCREMENT BY 1
        CACHE 100;
END
GO

-- Mobile Expense Management Sequences
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOBEXP_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_MOBEXP_Id
        AS DECIMAL(38)
        START WITH 1
        INCREMENT BY 1
        CACHE 100;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOBEXP_File_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_MOBEXP_File_Id
        AS DECIMAL(38)
        START WITH 1
        INCREMENT BY 1
        CACHE 100;
END
GO

-- Employee Pride Management Sequences
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOMENT_PRIDE_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_MOMENT_PRIDE_Id
        AS DECIMAL(38)
        START WITH 1
        INCREMENT BY 1
        CACHE 100;
END
GO

-- Problem Management Sequences
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_PROBLEM_MAIN_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_PROBLEM_MAIN_Id
        AS BIGINT
        START WITH 1
        INCREMENT BY 1
        CACHE 100;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_PROBLEM_SOLUTION_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_PROBLEM_SOLUTION_Id
        AS BIGINT
        START WITH 1
        INCREMENT BY 1
        CACHE 100;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_PROBLEM_APP_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_PROBLEM_APP_Id
        AS BIGINT
        START WITH 1
        INCREMENT BY 1
        CACHE 100;
END
GO

-- Scholarship Management Sequences
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_SCHOLARSHIP_APPLICATION_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_SCHOLARSHIP_APPLICATION_Id
        AS BIGINT
        START WITH 1
        INCREMENT BY 1
        CACHE 100;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_SCHOLARSHIP_DISBURSEMENT_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_SCHOLARSHIP_DISBURSEMENT_Id
        AS BIGINT
        START WITH 1
        INCREMENT BY 1
        CACHE 100;
END
GO

PRINT 'All sequences created successfully.';
GO
