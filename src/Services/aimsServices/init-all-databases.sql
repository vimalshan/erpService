-- ================================================================
-- Combined Database Initialization Script for AIMS Services
-- ================================================================
-- This script pre-creates all AIMS microservice databases so that
-- EF Core migrations can run without needing CREATE DATABASE rights.
--
-- Services included:
--   1. Access Service          (ACCESSDB)
--   2. Attendance Service      (ATTENDANCEDB)
--   3. Bus Service             (BUSDB)
--   4. Calendar Service        (CALENDARDB)
--   5. Employee Service        (EMPLOYEEDB)
--   6. Group Incentive Service (GROUPINCENTIVEDB)
--   7. Leave Service           (LEAVEDB)
--   8. Reference Service       (REFERENCEDB)
--   9. Visitor Service         (VISITORDB)
--  10. AIMS Transaction Service (AIMSDB)
--
-- Note: Table creation is handled by EF Core migrations at startup.
--       This script only ensures the databases exist.
-- Generated on: 2026-03-23
-- ================================================================

USE master;
GO

-- ============================================================
-- 1. Access Service  (ACCESSDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ACCESSDB')
BEGIN
    CREATE DATABASE [ACCESSDB];
    PRINT '+ ACCESSDB created';
END
ELSE
    PRINT '= ACCESSDB already exists';
GO

ALTER DATABASE [ACCESSDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ============================================================
-- 2. Attendance Service  (ATTENDANCEDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ATTENDANCEDB')
BEGIN
    CREATE DATABASE [ATTENDANCEDB];
    PRINT '+ ATTENDANCEDB created';
END
ELSE
    PRINT '= ATTENDANCEDB already exists';
GO

ALTER DATABASE [ATTENDANCEDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ============================================================
-- 3. Bus Service  (BUSDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'BUSDB')
BEGIN
    CREATE DATABASE [BUSDB];
    PRINT '+ BUSDB created';
END
ELSE
    PRINT '= BUSDB already exists';
GO

ALTER DATABASE [BUSDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ============================================================
-- 4. Calendar Service  (CALENDARDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CALENDARDB')
BEGIN
    CREATE DATABASE [CALENDARDB];
    PRINT '+ CALENDARDB created';
END
ELSE
    PRINT '= CALENDARDB already exists';
GO

ALTER DATABASE [CALENDARDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ============================================================
-- 5. Employee Service  (EMPLOYEEDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'EMPLOYEEDB')
BEGIN
    CREATE DATABASE [EMPLOYEEDB];
    PRINT '+ EMPLOYEEDB created';
END
ELSE
    PRINT '= EMPLOYEEDB already exists';
GO

ALTER DATABASE [EMPLOYEEDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ============================================================
-- 6. Group Incentive Service  (GROUPINCENTIVEDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'GROUPINCENTIVEDB')
BEGIN
    CREATE DATABASE [GROUPINCENTIVEDB];
    PRINT '+ GROUPINCENTIVEDB created';
END
ELSE
    PRINT '= GROUPINCENTIVEDB already exists';
GO

ALTER DATABASE [GROUPINCENTIVEDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ============================================================
-- 7. Leave Service  (LEAVEDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'LEAVEDB')
BEGIN
    CREATE DATABASE [LEAVEDB];
    PRINT '+ LEAVEDB created';
END
ELSE
    PRINT '= LEAVEDB already exists';
GO

ALTER DATABASE [LEAVEDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ============================================================
-- 8. Reference Service  (REFERENCEDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'REFERENCEDB')
BEGIN
    CREATE DATABASE [REFERENCEDB];
    PRINT '+ REFERENCEDB created';
END
ELSE
    PRINT '= REFERENCEDB already exists';
GO

ALTER DATABASE [REFERENCEDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ============================================================
-- 9. Visitor Service  (VISITORDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'VISITORDB')
BEGIN
    CREATE DATABASE [VISITORDB];
    PRINT '+ VISITORDB created';
END
ELSE
    PRINT '= VISITORDB already exists';
GO

ALTER DATABASE [VISITORDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

-- ============================================================
-- 10. AIMS Transaction Service  (AIMSDB)
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'AIMSDB')
BEGIN
    CREATE DATABASE [AIMSDB];
    PRINT '+ AIMSDB created';
END
ELSE
    PRINT '= AIMSDB already exists';
GO

ALTER DATABASE [AIMSDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

PRINT '';
PRINT 'All AIMS databases initialized successfully.';
PRINT 'EF Core migrations will create tables on first service startup.';
GO
