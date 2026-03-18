-- ==========================================
-- Module: FILING AND ARCHIVE MODULE
-- Description: File management, archiving, and document dispatch tracking
-- Created: March 9, 2026
-- Database: SSCDB
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- FILE_MASTER - File Master
-- ==========================================
IF OBJECT_ID('[FILE_MASTER]', 'U') IS NOT NULL DROP TABLE [FILE_MASTER];
GO
CREATE TABLE [FILE_MASTER] (
    [FILE_ID] BIGINT NOT NULL,
    [FILE_ORGID] VARCHAR(25) NOT NULL,
    [FILE_YEAR] BIGINT NOT NULL,
    [FILE_NO] VARCHAR(25) NOT NULL,
    [FILE_STATUS] CHAR(1) NOT NULL,
    [FILE_REMARKS] VARCHAR(200) NULL,
    [FILE_PODNO] VARCHAR(50) NULL,
    [FILE_COURIERNAME] VARCHAR(200) NULL,
    [FILE_CREATEDON] DATETIME2(3) NOT NULL,
    [FILE_CREATEDBY] BIGINT NOT NULL,
    [FILE_UPDATEDON] DATETIME2(3) NOT NULL,
    [FILE_UPDATEDBY] BIGINT NOT NULL,
    [FILE_DISPATCHEDON] DATETIME2(3) NULL,
    [FILE_DISPATCHEDBY] BIGINT NULL,
    CONSTRAINT [PK_FILE_MASTER] PRIMARY KEY ([FILE_ID])
);
GO

-- ==========================================
-- FILING_COUNTER - Filing Counter
-- ==========================================
IF OBJECT_ID('[FILING_COUNTER]', 'U') IS NOT NULL DROP TABLE [FILING_COUNTER];
GO
CREATE TABLE [FILING_COUNTER] (
    [FILING_BUID] VARCHAR(25) NOT NULL,
    [FILE_COUNT] BIGINT NOT NULL
);
GO

-- ==========================================
-- FILING_DOC_PRINT - Filing Document Print Tracking
-- ==========================================
IF OBJECT_ID('[FILING_DOC_PRINT]', 'U') IS NOT NULL DROP TABLE [FILING_DOC_PRINT];
GO
CREATE TABLE [FILING_DOC_PRINT] (
    [DOC_SEQ] BIGINT NOT NULL,
    [DOC_KEY] VARCHAR(50) NOT NULL,
    [DOC_FILENO] BIGINT NOT NULL
);
GO

-- ==========================================
-- FILINGDOC_ERROR_LIST - Filing Document Error List
-- ==========================================
IF OBJECT_ID('[FILINGDOC_ERROR_LIST]', 'U') IS NOT NULL DROP TABLE [FILINGDOC_ERROR_LIST];
GO
CREATE TABLE [FILINGDOC_ERROR_LIST] (
    [DOC_KEY] VARCHAR(50) NULL,
    [REMARKS] VARCHAR(4000) NULL,
    [SYS_ID] BIGINT NULL,
    [ACCOUNTING_DATE] DATETIME2(3) NULL,
    [FLAG] VARCHAR(10) NULL,
    [STATUS] VARCHAR(100) NULL,
    [SNO] BIGINT NULL
);
GO

PRINT 'FILING_AND_ARCHIVE_MODULE Schema created successfully.';
GO
