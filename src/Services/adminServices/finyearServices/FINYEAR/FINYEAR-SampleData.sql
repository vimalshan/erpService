-- ==========================================
-- MODULE: FINYEAR
-- Component: Sample Data Script
-- Description: Insert sample financial year data for testing
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

SET NOCOUNT ON;
GO

-- Insert sample financial years
IF NOT EXISTS (SELECT * FROM FINYEAR_MASTER WHERE FY_ID = 1)
BEGIN
    INSERT INTO FINYEAR_MASTER (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
    VALUES 
    (
        1,
        '2024-2025',
        '2024-04-01 00:00:00.000',
        '2025-03-31 23:59:59.999',
        1,
        GETDATE()
    );
    PRINT 'Sample financial year 2024-2025 inserted successfully.';
END

IF NOT EXISTS (SELECT * FROM FINYEAR_MASTER WHERE FY_ID = 2)
BEGIN
    INSERT INTO FINYEAR_MASTER (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
    VALUES 
    (
        2,
        '2025-2026',
        '2025-04-01 00:00:00.000',
        '2026-03-31 23:59:59.999',
        1,
        GETDATE()
    );
    PRINT 'Sample financial year 2025-2026 inserted successfully.';
END

IF NOT EXISTS (SELECT * FROM FINYEAR_MASTER WHERE FY_ID = 3)
BEGIN
    INSERT INTO FINYEAR_MASTER (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
    VALUES 
    (
        3,
        '2023-2024',
        '2023-04-01 00:00:00.000',
        '2024-03-31 23:59:59.999',
        1,
        GETDATE()
    );
    PRINT 'Sample financial year 2023-2024 inserted successfully.';
END

SET NOCOUNT OFF;
GO

-- Verify inserted data
SELECT COUNT(*) AS TotalRecords FROM FINYEAR_MASTER;
GO

PRINT 'Sample data insertion completed.';
GO
