-- ==========================================
-- MIGRATION: Apply Stored Procedures & Sample Data
-- ==========================================

USE [FinyearDB];
GO

-- =====================================================
-- SECTION 1: CREATE STORED PROCEDURES
-- =====================================================

-- Drop existing stored procedures if they exist
IF OBJECT_ID(N'[dbo].[sp_GetFinancialYearByDateRange]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetFinancialYearByDateRange];
GO

IF OBJECT_ID(N'[dbo].[sp_IsFinancialYearActive]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_IsFinancialYearActive];
GO

IF OBJECT_ID(N'[dbo].[sp_DeleteFinancialYear]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_DeleteFinancialYear];
GO

IF OBJECT_ID(N'[dbo].[sp_UpdateFinancialYear]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateFinancialYear];
GO

IF OBJECT_ID(N'[dbo].[sp_CreateFinancialYear]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateFinancialYear];
GO

IF OBJECT_ID(N'[dbo].[sp_GetAllFinancialYears]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetAllFinancialYears];
GO

IF OBJECT_ID(N'[dbo].[sp_GetFinancialYearById]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_GetFinancialYearById];
GO

-- =====================================================
-- Create Stored Procedures
-- =====================================================

-- 1. Get Financial Year by ID
CREATE PROCEDURE [dbo].[sp_GetFinancialYearById]
    @FY_ID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        FY_ID,
        FY_NAME,
        FY_STARTDATE,
        FY_CLOSEDATE,
        FY_UPDATED_BY,
        FY_UPDATED_ON
    FROM [FINYEAR_MASTER]
    WHERE FY_ID = @FY_ID;
END
GO

-- 2. Get All Financial Years
CREATE PROCEDURE [dbo].[sp_GetAllFinancialYears]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        FY_ID,
        FY_NAME,
        FY_STARTDATE,
        FY_CLOSEDATE,
        FY_UPDATED_BY,
        FY_UPDATED_ON
    FROM [FINYEAR_MASTER]
    ORDER BY FY_STARTDATE DESC;
END
GO

-- 3. Create Financial Year
CREATE PROCEDURE [dbo].[sp_CreateFinancialYear]
    @FY_NAME VARCHAR(27),
    @FY_STARTDATE DATETIME2(3),
    @FY_CLOSEDATE DATETIME2(3),
    @FY_UPDATED_BY BIGINT,
    @NewFY_ID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    SET @NewFY_ID = (SELECT ISNULL(MAX(FY_ID), 0) + 1 FROM [FINYEAR_MASTER]);
    
    INSERT INTO [FINYEAR_MASTER] 
    (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
    VALUES 
    (@NewFY_ID, @FY_NAME, @FY_STARTDATE, @FY_CLOSEDATE, @FY_UPDATED_BY, GETUTCDATE());
END
GO

-- 4. Update Financial Year
CREATE PROCEDURE [dbo].[sp_UpdateFinancialYear]
    @FY_ID BIGINT,
    @FY_NAME VARCHAR(27),
    @FY_STARTDATE DATETIME2(3),
    @FY_CLOSEDATE DATETIME2(3),
    @FY_UPDATED_BY BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [FINYEAR_MASTER]
    SET 
        FY_NAME = @FY_NAME,
        FY_STARTDATE = @FY_STARTDATE,
        FY_CLOSEDATE = @FY_CLOSEDATE,
        FY_UPDATED_BY = @FY_UPDATED_BY,
        FY_UPDATED_ON = GETUTCDATE()
    WHERE FY_ID = @FY_ID;
END
GO

-- 5. Delete Financial Year
CREATE PROCEDURE [dbo].[sp_DeleteFinancialYear]
    @FY_ID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM [FINYEAR_MASTER]
    WHERE FY_ID = @FY_ID;
END
GO

-- 6. Check if Financial Year is Active
CREATE PROCEDURE [dbo].[sp_IsFinancialYearActive]
    @FY_ID BIGINT,
    @IsActive BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (
        SELECT 1 FROM [FINYEAR_MASTER]
        WHERE FY_ID = @FY_ID
        AND GETUTCDATE() BETWEEN FY_STARTDATE AND FY_CLOSEDATE
    )
    BEGIN
        SET @IsActive = 1;
    END
    ELSE
    BEGIN
        SET @IsActive = 0;
    END
END
GO

-- 7. Get Financial Year by Date Range
CREATE PROCEDURE [dbo].[sp_GetFinancialYearByDateRange]
    @StartDate DATETIME2(3),
    @EndDate DATETIME2(3)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        FY_ID,
        FY_NAME,
        FY_STARTDATE,
        FY_CLOSEDATE,
        FY_UPDATED_BY,
        FY_UPDATED_ON
    FROM [FINYEAR_MASTER]
    WHERE FY_STARTDATE >= @StartDate 
    AND FY_CLOSEDATE <= @EndDate
    ORDER BY FY_STARTDATE DESC;
END
GO

PRINT 'All stored procedures created successfully.';
GO

-- =====================================================
-- SECTION 2: INSERT SAMPLE DATA
-- =====================================================

-- Check if data already exists before inserting
IF NOT EXISTS (SELECT 1 FROM [FINYEAR_MASTER] WHERE FY_ID = 1)
BEGIN
    INSERT INTO [FINYEAR_MASTER] 
    (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
    VALUES 
    (1, 'FY 2024-2025', '2024-04-01', '2025-03-31', 1, GETUTCDATE());
    
    PRINT 'Sample data: FY 2024-2025 inserted (FY_ID = 1)';
END

IF NOT EXISTS (SELECT 1 FROM [FINYEAR_MASTER] WHERE FY_ID = 2)
BEGIN
    INSERT INTO [FINYEAR_MASTER] 
    (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
    VALUES 
    (2, 'FY 2025-2026', '2025-04-01', '2026-03-31', 1, GETUTCDATE());
    
    PRINT 'Sample data: FY 2025-2026 inserted (FY_ID = 2)';
END

IF NOT EXISTS (SELECT 1 FROM [FINYEAR_MASTER] WHERE FY_ID = 3)
BEGIN
    INSERT INTO [FINYEAR_MASTER] 
    (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
    VALUES 
    (3, 'FY 2026-2027', '2026-04-01', '2027-03-31', 1, GETUTCDATE());
    
    PRINT 'Sample data: FY 2026-2027 inserted (FY_ID = 3)';
END

IF NOT EXISTS (SELECT 1 FROM [FINYEAR_MASTER] WHERE FY_ID = 4)
BEGIN
    INSERT INTO [FINYEAR_MASTER] 
    (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
    VALUES 
    (4, 'FY 2023-2024', '2023-04-01', '2024-03-31', 1, GETUTCDATE());
    
    PRINT 'Sample data: FY 2023-2024 inserted (FY_ID = 4)';
END

PRINT 'Sample data insertion completed.';
GO

-- =====================================================
-- VERIFY INSTALLATION
-- =====================================================

-- Show final data
SELECT COUNT(*) AS 'Total Financial Years' FROM [FINYEAR_MASTER];
GO

SELECT 
    FY_ID,
    FY_NAME,
    FY_STARTDATE,
    FY_CLOSEDATE,
    FY_UPDATED_BY,
    FY_UPDATED_ON
FROM [FINYEAR_MASTER]
ORDER BY FY_ID;
GO

-- Show created procedures
SELECT 
    ROUTINE_NAME,
    ROUTINE_TYPE
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_TYPE = 'PROCEDURE'
AND ROUTINE_NAME LIKE 'sp_%'
AND ROUTINE_SCHEMA = 'dbo'
ORDER BY ROUTINE_NAME;
GO

PRINT 'Migration script completed successfully!';
PRINT 'Tables: 1 (FINYEAR_MASTER)';
PRINT 'Stored Procedures: 7';
PRINT 'Sample Records: 4';
GO
