-- =========================================================================
-- MEMBER MODULE - Database Tables
-- Database: PFDB
-- Module: Member Management
-- Description: Manages member profiles, nominees, and payroll integration
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- =========================================================================
-- 1. MEMBER_MASTER - Member Profile Records
-- =========================================================================
CREATE TABLE [MEMBER_MASTER] (
    [MEMBER_NO] BIGINT NOT NULL  -- Membership no,
    [MEMBER_TRUST_CODE] CHAR(3) NOT NULL  -- Trust code,
    [MEMBER_OPF_NO] INT NOT NULL  -- OPF A/C No,
    [MEMBER_PENSION_NO] INT NOT NULL  -- Pension A/C No,
    [MEMBER_FPSTRUST_CODE] CHAR(3) NOT NULL  -- FPS Trust Code,
    [MEMBER_NAME] VARCHAR(65) NOT NULL  -- Member Name,
    [MEMBER_FATHERNAME] VARCHAR(65) NULL  -- Member Father Name,
    [MEMBER_ENR_DATE] DATETIME2(3) NOT NULL  -- Date of Enrollment,
    [MEMBER_DOJ] DATETIME2(3) NOT NULL  -- Date of joining SRF,
    [MEMBER_EMPLOYEE_TYPE] CHAR(2) NOT NULL  -- Employee Type (N-New/S-Transfer within SRF/O-Transfer from Outside),
    [MEMBER_CLOSURE_DATE] DATETIME2(3) NULL  -- Date of Closure of PF A/C,
    [MEMBER_LEAVE_DATE] DATETIME2(3) NULL  -- Date of Leaving SRF,
    [MEMBER_LEAVE_REASON] VARCHAR(200) NULL  -- Reason for Leaving SRF,
    [MEMBER_ENROLL_USER_ID] VARCHAR(25) NOT NULL  -- Enrollment done by - User ID,
    [MEMBER_ENROLL_SYSID] BIGINT NOT NULL  -- Enrollment done by - SysID,
    [MEMBER_ENROLL_DATE] DATETIME2(3) NOT NULL  -- Enrollment done on,
    [MEMBER_UNIT_CODE] CHAR(3) NOT NULL  -- Payroll Processing Unit code,
    [MEMBER_EMP_NUM] BIGINT NOT NULL  -- Payroll Processing Employee No,
    [MEMBER_EMP_SYSID] BIGINT NOT NULL  -- Employee Sys ID,
    [MEMBER_DOB] DATETIME2(3) NULL  -- Member Date of Birth,
    [MEMBER_STATUS] CHAR(1) DEFAULT 'A'  -- Member Status (A-Active/I-Inactive/C-Closed),
    [MEMBER_UPDATED_BY] BIGINT NULL,
    [MEMBER_UPDATED_ON] DATETIME2(3) NULL,
    CONSTRAINT [PK_MEMBER_MASTER] PRIMARY KEY ([MEMBER_NO])
);
GO

-- =========================================================================
-- 2. MEMBER_NOMINEE - Member Nominee Details
-- =========================================================================
CREATE TABLE [MEMBER_NOMINEE] (
    [NOMINEE_MEMBER_NO] INT NOT NULL  -- Membership No,
    [NOMINEE_SERIAL_NO] INT NOT NULL  -- Serial No,
    [NOMINEE_FUND_TYPE] CHAR(3) NOT NULL  -- Fund type,
    [NOMINEE_NAME] VARCHAR(65) NOT NULL  -- Nominee name,
    [NOMINEE_RELATIONSHIP_CODE] CHAR(3) NOT NULL  -- Relationship of Nominee WITH Employee,
    [NOMINEE_PERCENTAGE] BIGINT NOT NULL  -- Nominee's % of share,
    [NOMINEE_DOB] DATETIME2(3) NOT NULL  -- Nominee Date of Birth,
    [NOMINEE_ADDRESS_LINE_1] VARCHAR(200) NULL  -- Nominee's Address Line - 1,
    [NOMINEE_ADDRESS_LINE_2] VARCHAR(200) NULL  -- Nominee's Address Line - 2,
    [NOMINEE_ADDRESS_LINE_3] VARCHAR(200) NULL  -- Nominee's Address Line - 3,
    [NOMINEE_PHONE_NO] VARCHAR(20) NULL  -- Nominee's Phone No,
    [NOMINEE_EMAIL] VARCHAR(100) NULL  -- Nominee's Email,
    [NOMINEE_EFF_DATE] DATETIME2(3) NOT NULL  -- Effective Date,
    [NOMINEE_CLS_DATE] DATETIME2(3) NULL  -- CLOSURE Date,
    [NOMINEE_MINOR_FLAG] CHAR(1) NOT NULL  -- Minor Flag,
    [NOMINEE_TRUST_CODE] CHAR(3) NOT NULL  -- Nominee Trust Code,
    [NOMINEE_STATUS] CHAR(1) DEFAULT 'A'  -- Status (A-Active/I-Inactive),
    CONSTRAINT [PK_MEMBER_NOMINEE] PRIMARY KEY ([NOMINEE_MEMBER_NO], [NOMINEE_SERIAL_NO], [NOMINEE_FUND_TYPE])
);
GO

-- =========================================================================
-- 3. MEMBER_PAYROLL - Member Payroll Integration
-- =========================================================================
CREATE TABLE [MEMBER_PAYROLL] (
    [PAYROLL_MEMBER_NO] BIGINT NOT NULL  -- Member No,
    [PAYROLL_UNT_COD] CHAR(3) NOT NULL  -- Payroll Unit Code,
    [PAYROLL_EMP_NUM] BIGINT NOT NULL  -- Payroll Employee No,
    [PAYROLL_EFF_DATE] DATETIME2(3) NOT NULL  -- Payroll Effective Date,
    [PAYROLL_CLS_DATE] DATETIME2(3) NULL  -- Payroll CLOSURE Date,
    [PAYROLL_STATUS] CHAR(1) DEFAULT 'A',  -- Status (A-Active/C-Closed),
    CONSTRAINT [PK_MEMBER_PAYROLL] PRIMARY KEY ([PAYROLL_MEMBER_NO], [PAYROLL_UNT_COD])
);
GO

-- =========================================================================
-- 4. NOMINEE_GAURDIAN - Guardian Details for Minor Nominees
-- =========================================================================
CREATE TABLE [NOMINEE_GAURDIAN] (
    [GN_TRUST_CODE] CHAR(3) NOT NULL  -- Trust Code,
    [GN_NOMINEE_MEMBER_NO] BIGINT NOT NULL  -- Nominee Member No,
    [GN_NOMINEE_SERIAL_NO] BIGINT NOT NULL  -- Nominee Serial No,
    [GAURDIAN_NAME] VARCHAR(65) NOT NULL  -- Gaurdian Name,
    [GN_ADDRESS_LINE1] VARCHAR(200) NULL  -- Gaurdian Address Line - 1,
    [GN_ADDRESS_LINE2] VARCHAR(200) NULL  -- Gaurdian Address Line - 2,
    [GN_ADDRESS_LINE3] VARCHAR(200) NULL  -- Gaurdian Address Line - 3,
    [GN_ADDRESS_LINE4] VARCHAR(200) NULL  -- Gaurdian Address Line - 4,
    [GN_PHONE_NO] VARCHAR(20) NULL  -- Guardian Phone No,
    [GN_EMAIL] VARCHAR(100) NULL  -- Guardian Email,
    [GN_EFF_DATE] DATETIME2(3) NULL  -- Effective Date,
    [GN_CLS_DATE] DATETIME2(3) NULL  -- Closure Date,
    [GAURDIAN_RELATIONSHIP] CHAR(3) NOT NULL  -- Gaurdian Relationship with Minor,
    [GN_STATUS] CHAR(1) DEFAULT 'A',  -- Status,
    CONSTRAINT [PK_NOMINEE_GAURDIAN] PRIMARY KEY ([GN_TRUST_CODE], [GN_NOMINEE_MEMBER_NO], [GN_NOMINEE_SERIAL_NO])
);
GO

-- =========================================================================
-- 5. MEMBER_AUDIT_LOG - Member Record Audit Trail
-- =========================================================================
CREATE TABLE [MEMBER_AUDIT_LOG] (
    [AUDIT_ID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [MEMBER_NO] BIGINT NOT NULL,
    [AUDIT_ACTION] VARCHAR(50) NOT NULL,  -- INSERT, UPDATE, DELETE
    [AUDIT_TIMESTAMP] DATETIME2(3) NOT NULL,
    [AUDIT_USER_ID] BIGINT NOT NULL,
    [AUDIT_OLD_VALUES] VARCHAR(MAX) NULL,
    [AUDIT_NEW_VALUES] VARCHAR(MAX) NULL
);
GO

-- =========================================================================
-- 6. MEMBER_CONTACT - Member Contact Information
-- =========================================================================
CREATE TABLE [MEMBER_CONTACT] (
    [CONTACT_ID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [MEMBER_NO] BIGINT NOT NULL,
    [CONTACT_TYPE] CHAR(1) NOT NULL,  -- P-Personal/O-Official/E-Emergency
    [ADDRESS_LINE_1] VARCHAR(200) NOT NULL,
    [ADDRESS_LINE_2] VARCHAR(200) NULL,
    [ADDRESS_LINE_3] VARCHAR(200) NULL,
    [CITY] VARCHAR(50) NOT NULL,
    [STATE] VARCHAR(50) NOT NULL,
    [PIN_CODE] VARCHAR(10) NOT NULL,
    [COUNTRY] VARCHAR(50) NOT NULL,
    [PHONE_NO] VARCHAR(20) NULL,
    [EMAIL] VARCHAR(100) NULL,
    [EFF_DATE] DATETIME2(3) NOT NULL,
    [CLS_DATE] DATETIME2(3) NULL,
    CONSTRAINT [FK_MEMBER_CONTACT_MEMBER] FOREIGN KEY ([MEMBER_NO]) REFERENCES [MEMBER_MASTER]([MEMBER_NO])
);
GO

-- =========================================================================
-- Indexes for Performance Optimization
-- =========================================================================

-- Index on MEMBER_MASTER for trust and status queries
CREATE NONCLUSTERED INDEX [IDX_MEMBER_MASTER_TRUST_STATUS]
ON [MEMBER_MASTER] ([MEMBER_TRUST_CODE], [MEMBER_STATUS])
INCLUDE ([MEMBER_NO], [MEMBER_NAME], [MEMBER_EMP_SYSID]);
GO

-- Index on MEMBER_MASTER for employee system ID lookup
CREATE NONCLUSTERED INDEX [IDX_MEMBER_MASTER_EMP_SYSID]
ON [MEMBER_MASTER] ([MEMBER_EMP_SYSID])
INCLUDE ([MEMBER_NO], [MEMBER_NAME]);
GO

-- Index on MEMBER_NOMINEE for member and effective date queries
CREATE NONCLUSTERED INDEX [IDX_MEMBER_NOMINEE_MEMBER]
ON [MEMBER_NOMINEE] ([NOMINEE_MEMBER_NO], [NOMINEE_EFF_DATE], [NOMINEE_STATUS])
INCLUDE ([NOMINEE_NAME], [NOMINEE_PERCENTAGE]);
GO

-- Index on MEMBER_PAYROLL for active employee tracking
CREATE NONCLUSTERED INDEX [IDX_MEMBER_PAYROLL_STATUS]
ON [MEMBER_PAYROLL] ([PAYROLL_MEMBER_NO], [PAYROLL_STATUS])
INCLUDE ([PAYROLL_EMP_NUM], [PAYROLL_UNT_COD]);
GO

-- Index on MEMBER_CONTACT for lookups
CREATE NONCLUSTERED INDEX [IDX_MEMBER_CONTACT_MEMBER]
ON [MEMBER_CONTACT] ([MEMBER_NO], [CONTACT_TYPE])
INCLUDE ([EMAIL], [PHONE_NO]);
GO

PRINT 'Member Module Tables created successfully!';
GO
