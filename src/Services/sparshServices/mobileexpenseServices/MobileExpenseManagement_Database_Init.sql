-- ============================================================================
-- Database Initialization Script for Mobile Expense Management
-- Purpose: Create sequences and initial data
-- ============================================================================

USE [SPARSHDB];
GO

-- ============================================================================
-- CREATE SEQUENCES for ID generation
-- ============================================================================

-- Check if sequence exists, if not create it
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOBEXP_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_MOBEXP_Id START WITH 1000 INCREMENT BY 1;
    PRINT 'Sequence seq_MOBEXP_Id created successfully';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOBEXP_File_Id')
BEGIN
    CREATE SEQUENCE dbo.seq_MOBEXP_File_Id START WITH 5000 INCREMENT BY 1;
    PRINT 'Sequence seq_MOBEXP_File_Id created successfully';
END
GO

-- ============================================================================
-- SEED DATA - Sample Categories (if master table exists)
-- ============================================================================

-- This assumes you have a category master table
-- Uncomment and modify based on your actual schema

/*
IF NOT EXISTS (SELECT 1 FROM [dbo].[EXPENSE_CATEGORY] WHERE EXPCAT_ID = 1)
BEGIN
    INSERT INTO [dbo].[EXPENSE_CATEGORY] (EXPCAT_ID, EXPCAT_NAME, EXPCAT_DESC, EXPCAT_MAXLIMIT)
    VALUES 
        (1, 'Travel', 'Travel related expenses', 5000),
        (2, 'Meals', 'Food and meal expenses', 1000),
        (3, 'Accommodation', 'Hotel and lodging expenses', 10000),
        (4, 'Transport', 'Vehicle and transportation expenses', 3000),
        (5, 'Communication', 'Phone and communication expenses', 500),
        (6, 'Supplies', 'Office supplies and materials', 2000),
        (7, 'Entertainment', 'Client entertainment expenses', 5000),
        (8, 'Other', 'Other miscellaneous expenses', 1000);
    
    PRINT 'Sample expense categories inserted';
END
GO
*/

-- ============================================================================
-- VERIFY TABLES and COLUMNS
-- ============================================================================

PRINT '=== Verification Report ===';

IF OBJECT_ID('[dbo].[MOBEXP_DET]', 'U') IS NOT NULL
    PRINT 'Table MOBEXP_DET: OK';
ELSE
    PRINT 'Table MOBEXP_DET: MISSING';

IF OBJECT_ID('[dbo].[MOBEXP_FILE]', 'U') IS NOT NULL
    PRINT 'Table MOBEXP_FILE: OK';
ELSE
    PRINT 'Table MOBEXP_FILE: MISSING';

IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOBEXP_Id')
    PRINT 'Sequence seq_MOBEXP_Id: OK';
ELSE
    PRINT 'Sequence seq_MOBEXP_Id: MISSING';

IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MOBEXP_File_Id')
    PRINT 'Sequence seq_MOBEXP_File_Id: OK';
ELSE
    PRINT 'Sequence seq_MOBEXP_File_Id: MISSING';

PRINT '';
PRINT 'Database initialization completed successfully!';
GO
