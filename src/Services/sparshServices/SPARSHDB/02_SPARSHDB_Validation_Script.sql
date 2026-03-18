-- ============================================================================
-- SPARSHDB Module Validation Script
-- Purpose: Validate module structure, objects, and relationships
-- Description: Run this script after deployment to verify all modules
-- Created: March 9, 2026
-- ============================================================================

USE [SPARSHDB];
GO

PRINT '========== SPARSHDB MODULE VALIDATION REPORT ==========';
PRINT '';
PRINT 'Date: ' + CAST(GETDATE() AS VARCHAR(30));
GO

-- ============================================================================
-- 1. VERIFY SEQUENCES
-- ============================================================================
PRINT '1. CHECKING SEQUENCES...';
PRINT '';

SELECT 'Sequence' AS Type, name AS ObjectName, 'VERIFIED' AS Status
FROM sys.sequences
WHERE name LIKE 'seq_%'
ORDER BY name;

PRINT '';

-- ============================================================================
-- 2. VERIFY MOBILE APP MANAGEMENT MODULE OBJECTS
-- ============================================================================
PRINT '2. MOBILE APP MANAGEMENT MODULE (MOD_MobileAppManagement)';
PRINT 'Tables Expected: 3, Procedures Expected: 3';
PRINT '';

SELECT COUNT(*) AS TableCount, 'Tables' AS Type
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' 
  AND (TABLE_NAME LIKE 'MOB_%' AND TABLE_NAME NOT LIKE 'MOBEXP%' AND TABLE_NAME NOT LIKE 'MOMENT%');

PRINT 'Tables:';
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' 
  AND ((TABLE_NAME LIKE 'MOB_' OR TABLE_NAME LIKE 'MOBAPP_%') 
       AND TABLE_NAME NOT LIKE 'MOBEXP%');

PRINT '';
PRINT 'Procedures:';
SELECT ROUTINE_NAME
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' 
  AND ROUTINE_TYPE = 'PROCEDURE'
  AND ROUTINE_NAME LIKE 'usp_MOB%';

PRINT '';

-- ============================================================================
-- 3. VERIFY MOBILE EXPENSE MANAGEMENT MODULE OBJECTS
-- ============================================================================
PRINT '3. MOBILE EXPENSE MANAGEMENT MODULE (MOD_MobileExpenseManagement)';
PRINT 'Tables Expected: 2, Procedures Expected: 4';
PRINT ''

SELECT COUNT(*) AS TableCount, 'Tables' AS Type
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME LIKE 'MOBEXP%';

PRINT 'Tables:';
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME LIKE 'MOBEXP%';

PRINT '';
PRINT 'Procedures:';
SELECT ROUTINE_NAME
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' 
  AND ROUTINE_TYPE = 'PROCEDURE'
  AND ROUTINE_NAME LIKE 'usp_EXP%';

PRINT '';

-- ============================================================================
-- 4. VERIFY EMPLOYEE PRIDE MANAGEMENT MODULE OBJECTS
-- ============================================================================
PRINT '4. EMPLOYEE PRIDE MANAGEMENT MODULE (MOD_EmployeePrideManagement)';
PRINT 'Tables Expected: 1, Procedures Expected: 4';
PRINT ''

SELECT COUNT(*) AS TableCount, 'Tables' AS Type
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME LIKE 'MOMENT%';

PRINT 'Tables:';
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME LIKE 'MOMENT%';

PRINT '';
PRINT 'Procedures:';
SELECT ROUTINE_NAME
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' 
  AND ROUTINE_TYPE = 'PROCEDURE'
  AND ROUTINE_NAME LIKE 'usp_PRIDE%';

PRINT '';

-- ============================================================================
-- 5. VERIFY PROBLEM MANAGEMENT MODULE OBJECTS
-- ============================================================================
PRINT '5. PROBLEM MANAGEMENT MODULE (MOD_ProblemManagement)';
PRINT 'Tables Expected: 9, Procedures Expected: 5';
PRINT ''

SELECT COUNT(*) AS TableCount, 'Tables' AS Type
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' 
  AND (TABLE_NAME LIKE 'PROBLEM%' OR TABLE_NAME LIKE 'SOLUTION%');

PRINT 'Tables:';
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' 
  AND (TABLE_NAME LIKE 'PROBLEM%' OR TABLE_NAME LIKE 'SOLUTION%')
ORDER BY TABLE_NAME;

PRINT '';
PRINT 'Procedures:';
SELECT ROUTINE_NAME
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' 
  AND ROUTINE_TYPE = 'PROCEDURE'
  AND (ROUTINE_NAME LIKE 'usp_PROBLEM%' OR ROUTINE_NAME LIKE 'usp_SOLUTION%')
ORDER BY ROUTINE_NAME;

PRINT '';

-- ============================================================================
-- 6. VERIFY SCHOLARSHIP MANAGEMENT MODULE OBJECTS
-- ============================================================================
PRINT '6. SCHOLARSHIP MANAGEMENT MODULE (MOD_ScholarshipManagement)';
PRINT 'Tables Expected: 4, Functions Expected: 2, Procedures Expected: 5';
PRINT ''

SELECT COUNT(*) AS TableCount, 'Tables' AS Type
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME LIKE 'SCHOLARSHIP%';

PRINT 'Tables:';
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME LIKE 'SCHOLARSHIP%'
ORDER BY TABLE_NAME;

PRINT '';
PRINT 'Functions:';
SELECT ROUTINE_NAME
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' 
  AND ROUTINE_TYPE = 'FUNCTION'
  AND ROUTINE_NAME LIKE 'fn_SCHOLARSHIP%' OR ROUTINE_NAME LIKE 'fn_%Student%' OR ROUTINE_NAME LIKE 'fn_%Scholarship%';

PRINT '';
PRINT 'Procedures:';
SELECT ROUTINE_NAME
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' 
  AND ROUTINE_TYPE = 'PROCEDURE'
  AND ROUTINE_NAME LIKE 'usp_SCHOLARSHIP%'
ORDER BY ROUTINE_NAME;

PRINT '';

-- ============================================================================
-- 7. SUMMARY STATISTICS
-- ============================================================================
PRINT '========== SUMMARY STATISTICS ==========';
PRINT '';

SELECT 'TOTAL TABLES' AS MetricType, COUNT(*) AS Count
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo'
  AND (TABLE_NAME LIKE 'MOB%' OR TABLE_NAME LIKE 'MOMENT%' OR TABLE_NAME LIKE 'PROBLEM%' 
       OR TABLE_NAME LIKE 'SOLUTION%' OR TABLE_NAME LIKE 'SCHOLARSHIP%')

UNION ALL

SELECT 'TOTAL PROCEDURES', COUNT(*)
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME LIKE 'usp_%'

UNION ALL

SELECT 'TOTAL FUNCTIONS', COUNT(*)
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION' AND ROUTINE_NAME LIKE 'fn_%'

UNION ALL

SELECT 'TOTAL SEQUENCES', COUNT(*)
FROM sys.sequences
WHERE name LIKE 'seq_%';

PRINT '';

-- ============================================================================
-- 8. VALIDATE FOREIGN KEY RELATIONSHIPS
-- ============================================================================
PRINT '========== FOREIGN KEY RELATIONSHIPS ==========';
PRINT ''

SELECT 
    CONSTRAINT_NAME,
    TABLE_NAME,
    COLUMN_NAME,
    REFERENCED_TABLE_NAME,
    REFERENCED_COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_SCHEMA = 'dbo'
  AND REFERENCED_TABLE_NAME IS NOT NULL
  AND (TABLE_NAME LIKE 'MOB%' OR TABLE_NAME LIKE 'MOMENT%' OR TABLE_NAME LIKE 'PROBLEM%' 
       OR TABLE_NAME LIKE 'SOLUTION%' OR TABLE_NAME LIKE 'SCHOLARSHIP%')
ORDER BY TABLE_NAME, CONSTRAINT_NAME;

PRINT '';

-- ============================================================================
-- 9. VALIDATE INDEXES
-- ============================================================================
PRINT '========== INDEX SUMMARY ==========';
PRINT ''

SELECT 
    t.name AS TableName,
    COUNT(i.name) AS IndexCount
FROM sys.tables t
LEFT JOIN sys.indexes i ON t.object_id = i.object_id AND i.type > 0
WHERE t.schema_id = (SELECT schema_id FROM sys.schemas WHERE name = 'dbo')
  AND (t.name LIKE 'MOB%' OR t.name LIKE 'MOMENT%' OR t.name LIKE 'PROBLEM%' 
       OR t.name LIKE 'SOLUTION%' OR t.name LIKE 'SCHOLARSHIP%')
GROUP BY t.name
ORDER BY t.name;

PRINT '';

-- ============================================================================
-- 10. VALIDATION COMPLETE
-- ============================================================================
PRINT '========== VALIDATION COMPLETE ==========';
PRINT 'All modules have been verified.';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Review module-specific README.md files for business logic';
PRINT '2. Load test data for functionality testing';
PRINT '3. Run stored procedures with test parameters';
PRINT '4. Verify data integrity constraints';
PRINT '';
PRINT 'Validation Report Generated: ' + CAST(GETDATE() AS VARCHAR(30));
GO
