-- ==========================================
-- Module: HR DOCUMENT MODULE
-- Description: HR employee payroll documents and file management
-- Created: March 9, 2026
-- Database: SSCDB
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- HRDOC_DET - HR Document Details
-- ==========================================
IF OBJECT_ID('[HRDOC_DET]', 'U') IS NOT NULL DROP TABLE [HRDOC_DET];
GO
CREATE TABLE [HRDOC_DET] (
    [DOC_ID] BIGINT NOT NULL,
    [DOC_NO] BIGINT NOT NULL,
    [DOC_TYPE] CHAR(3) NOT NULL,
    [DOC_PAYREFNO] BIGINT NOT NULL,
    [DOC_LOCID] BIGINT NOT NULL,
    [DOC_UNITID] BIGINT NOT NULL,
    [DOC_REMARKS] VARCHAR(100) NOT NULL,
    [DOC_USERID] BIGINT NOT NULL,
    [DOC_REFNO] VARCHAR(50) NULL,
    [DOC_REFNAME] VARCHAR(200) NULL,
    [DOC_CREATEDON] DATETIME2(3) NOT NULL,
    [DOC_DOCSTATUS] CHAR(2) NOT NULL,
    [DOC_SOURCE] CHAR(3) NOT NULL,
    [DOC_ACTIONSTATUS] CHAR(1) NULL,
    [DOC_ACTIONTAKENON] DATETIME2(3) NULL,
    [DOC_ACTIONTAKENBY] DECIMAL(38) NULL,
    [DOC_FILEPATH] VARCHAR(200) NULL,
    [DOC_CANCELFLAG] CHAR(1) NULL,
    [DOC_CANCELBY] DECIMAL(38) NULL,
    [DOC_CANCELON] DATETIME2(3) NULL,
    [DOC_PAYBY] DECIMAL(38) NULL,
    [DOC_REJECTREMARKS] VARCHAR(200) NULL,
    CONSTRAINT [PK_HRDOC_DET] PRIMARY KEY ([DOC_ID])
);
GO

-- ==========================================
-- HRDOC_SSFILELIST - HR Document SSC File List
-- ==========================================
IF OBJECT_ID('[HRDOC_SSFILELIST]', 'U') IS NOT NULL DROP TABLE [HRDOC_SSFILELIST];
GO
CREATE TABLE [HRDOC_SSFILELIST] (
    [FILE_ID] BIGINT NOT NULL,
    [FILE_DOCID] BIGINT NOT NULL,
    [FILE_PATH] VARCHAR(25) NOT NULL,
    [FILE_NAME] VARCHAR(200) NOT NULL,
    CONSTRAINT [PK_HRDOC_SSFILELIST] PRIMARY KEY ([FILE_ID])
);
GO

-- ==========================================
-- HRDOC_RECDET - HR Document Receipt Details
-- ==========================================
IF OBJECT_ID('[HRDOC_RECDET]', 'U') IS NOT NULL DROP TABLE [HRDOC_RECDET];
GO
CREATE TABLE [HRDOC_RECDET] (
    [HRREC_ID] BIGINT NOT NULL,
    [HRREC_ENVID] BIGINT NOT NULL,
    [HRREC_HRDOCID] BIGINT NOT NULL,
    [HRREC_UPDATEDBY] BIGINT NOT NULL,
    [HRREC_UPDATEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_HRDOC_RECDET] PRIMARY KEY ([HRREC_ID])
);
GO

-- ==========================================
-- HRDOC_COUNTER - HR Document Counter
-- ==========================================
IF OBJECT_ID('[HRDOC_COUNTER]', 'U') IS NOT NULL DROP TABLE [HRDOC_COUNTER];
GO
CREATE TABLE [HRDOC_COUNTER] (
    [DOC_NO] BIGINT NULL
);
GO

PRINT 'HR_DOCUMENT_MODULE Schema created successfully.';
GO
