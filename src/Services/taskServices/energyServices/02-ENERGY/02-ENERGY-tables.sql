-- ==========================================
-- Module: ENERGY MANAGEMENT
-- Database: TASKDB
-- Purpose: Energy/Utility Consumption & Process Management
-- Tables for tracking energy readings, process access, and mail configurations
-- ==========================================

USE [TASKDB];
GO

-- ==========================================
-- ENERGY MANAGEMENT TABLES
-- ==========================================

-- Table: EC_PROCESS
-- Purpose: Energy consumption process master
CREATE TABLE [EC_PROCESS] (
    [EC_PROCESS_ID] INT NOT NULL,
    [EC_PROCESS_DESC] VARCHAR(65) NOT NULL,
    [EC_UNIT_CODE] CHAR(3) NOT NULL,
    [EC_CLOSE_FLAG] CHAR(1) NOT NULL,
    [LAST_MODIFIED_BY] INT NOT NULL,
    [LAST_MODIFIED_ON] DATETIME2(3) NOT NULL
);

-- Table: EC_PROCESS_ACCESS
-- Purpose: Employee access control for energy processes
CREATE TABLE [EC_PROCESS_ACCESS] (
    [PA_ID] INT NULL,
    [PA_PROCESS_ID] INT NOT NULL,
    [PA_EMP_SYSID] INT NOT NULL,
    [PA_START_DATE] DATETIME2(3) NOT NULL,
    [PA_CLOSE_DATE] DATETIME2(3) NULL,
    [PA_LAST_MODIFIEDBY] INT NOT NULL,
    [PA_LAST_MODIFIEDON] VARCHAR(30) NOT NULL
);

-- Table: EC_PROCESS_MAILID
-- Purpose: Email notification configuration for energy processes
CREATE TABLE [EC_PROCESS_MAILID] (
    [PM_ID] INT NULL,
    [PM_PROCESS_ID] INT NOT NULL,
    [PM_MAIL_ID] VARCHAR(65) NOT NULL,
    [PM_DELIVERY_TYPE] CHAR(3) NOT NULL,
    [PM_START_DATE] DATETIME2(3) NOT NULL,
    [PM_CLOSE_DATE] DATETIME2(3) NULL,
    [PM_LAST_MODIFIEDBY] INT NOT NULL,
    [PM_LAST_MODIFIEDON] VARCHAR(20) NOT NULL
);

-- Table: EC_READING
-- Purpose: Energy consumption readings and usage tracking
CREATE TABLE [EC_READING] (
    [EB_ID] INT NULL,
    [EB_UNIT_CODE] CHAR(3) NOT NULL,
    [EB_PROCESS_ID] INT NOT NULL,
    [EB_DATE] DATETIME2(3) NOT NULL,
    [EB_TARGET] BIGINT NULL,
    [EB_READING] BIGINT NULL,
    [EB_RESET_READING] BIGINT NULL,
    [EB_ACTUAL_USAGE] BIGINT NULL,
    [EB_TODATE] BIGINT NULL,
    [EB_REMARKS] VARCHAR(100) NULL,
    [LAST_MODIFIED_BY] INT NOT NULL,
    [LAST_MODIFIED_ON] DATETIME2(3) NOT NULL
);

-- ==========================================
-- END OF SCRIPT - ENERGY MODULE TABLES
-- ==========================================
