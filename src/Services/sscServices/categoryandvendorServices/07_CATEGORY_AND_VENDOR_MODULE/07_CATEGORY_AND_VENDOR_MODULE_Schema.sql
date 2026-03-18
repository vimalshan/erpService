-- ==========================================
-- Module: CATEGORY AND VENDOR MODULE
-- Description: Product categories, vendor management, and support documents
-- Created: March 9, 2026
-- Database: SSCDB
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- MAINCAT_MAST - Main Category Master
-- ==========================================
IF OBJECT_ID('[MAINCAT_MAST]', 'U') IS NOT NULL DROP TABLE [MAINCAT_MAST];
GO
CREATE TABLE [MAINCAT_MAST] (
    [MAINCAT_ID] BIGINT NOT NULL  -- Main Category ID,
    [MAINCAT_NAME] VARCHAR(200) NOT NULL  -- Main Category Name,
    [MAINCAT_PRIORITY] BIGINT NOT NULL  -- Priority ID,
    [MAINCAT_MODIFIEDBY] BIGINT NOT NULL,
    [MAINCAT_MODIFIEDON] DATETIME2(3) NOT NULL,
    [MAINCAT_DEFSUBCATID] BIGINT NULL,
    [MAINCAT_AVGRESTIME] BIGINT NULL,
    CONSTRAINT [PK_MAINCAT_MAST] PRIMARY KEY ([MAINCAT_ID])
);
GO

-- ==========================================
-- SUBCAT_MAST - Sub Category Master
-- ==========================================
IF OBJECT_ID('[SUBCAT_MAST]', 'U') IS NOT NULL DROP TABLE [SUBCAT_MAST];
GO
CREATE TABLE [SUBCAT_MAST] (
    [SUBCAT_ID] BIGINT NOT NULL  -- Sub Category ID,
    [SUBCAT_MAINID] BIGINT NOT NULL  -- Main Category ID,
    [SUBCAT_NAME] VARCHAR(200) NOT NULL  -- Sub Category Name,
    [SUBCAT_MODIFIEDBY] BIGINT NOT NULL,
    [SUBCAT_MODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_SUBCAT_MAST] PRIMARY KEY ([SUBCAT_ID])
);
GO

-- ==========================================
-- VENDOR_DOCDET - Vendor Document Details
-- ==========================================
IF OBJECT_ID('[VENDOR_DOCDET]', 'U') IS NOT NULL DROP TABLE [VENDOR_DOCDET];
GO
CREATE TABLE [VENDOR_DOCDET] (
    [VNDDOC_ID] BIGINT NOT NULL  -- Document ID,
    [VNDDOC_VENDORID] BIGINT NOT NULL  -- Vendor ID,
    [VNDDOC_SITEID] BIGINT NOT NULL  -- Vendor Site ID - 0 for All,
    [VNDDOC_BUID] BIGINT NOT NULL  -- BU ID - 0 for All,
    [VNDDOC_INFCAT] BIGINT NOT NULL  -- Information Category,
    [VNDDOC_REMARKS] VARCHAR(2000) NOT NULL  -- Information Remarks,
    [VNDDOC_DOCFLAG] CHAR(1) NOT NULL  -- Document Attached,
    [VNDDOC_DOCTYPE] BIGINT NULL  -- Document Type,
    [VNDDOC_DOCREFNO] VARCHAR(100) NULL  -- Document Reference No,
    [VNDDOC_VALIDFROM] DATETIME2(3) NOT NULL  -- Valid From,
    [VNDDOC_VALIDTO] DATETIME2(3) NULL  -- Valid To,
    [VNDDOC_ACTIVESTATUS] CHAR(1) NOT NULL  -- Active Status,
    [VNDDOC_MODIFIEDBY] BIGINT NOT NULL  -- Last Updated by,
    [VNDDOC_MODIFIEDON] DATETIME2(3) NOT NULL  -- Last Updated on,
    [VNDDOC_APPSTATUS] CHAR(1) NOT NULL  -- (N - Pending for Submission / P - Pending for Approval / A - Apporved / R - Rejected),
    [VNDDOC_APPREMARKS] VARCHAR(500) NULL  -- Approver Remarks,
    [VNDDOC_APPROVEDBY] BIGINT NULL  -- Approved by,
    [VNDDOC_APPROVEDON] DATETIME2(3) NULL  -- Approved on,
    CONSTRAINT [PK_VENDOR_DOCDET] PRIMARY KEY ([VNDDOC_ID])
);
GO

-- ==========================================
-- VENDOR_DOCFILE - Vendor Document Files
-- ==========================================
IF OBJECT_ID('[VENDOR_DOCFILE]', 'U') IS NOT NULL DROP TABLE [VENDOR_DOCFILE];
GO
CREATE TABLE [VENDOR_DOCFILE] (
    [VNDFILE_ID] BIGINT NOT NULL  -- File ID,
    [VNDFILE_DOCID] BIGINT NOT NULL  -- Document ID,
    [VNDFILE_NAME] VARCHAR(100) NOT NULL  -- Document File Name,
    [VNDFILE_PATH] VARCHAR(100) NULL,
    CONSTRAINT [PK_VENDOR_DOCFILE] PRIMARY KEY ([VNDFILE_ID])
);
GO

-- ==========================================
-- SUPDOC_DET - Support Document Details
-- ==========================================
IF OBJECT_ID('[SUPDOC_DET]', 'U') IS NOT NULL DROP TABLE [SUPDOC_DET];
GO
CREATE TABLE [SUPDOC_DET] (
    [SUP_DOCID] BIGINT NOT NULL,
    [SUP_DOCCAT] BIGINT NOT NULL,
    [SUP_INVDOCID] BIGINT NOT NULL,
    [SUP_DOCKEY] VARCHAR(50) NULL,
    [SUP_DOCSTATUS] CHAR(2) NOT NULL,
    [SUP_PBGNO] VARCHAR(50) NULL,
    [SUP_PBGSTART] DATETIME2(3) NULL,
    [SUP_PBGEXPDATE] DATETIME2(3) NULL,
    [SUP_AMOUNT] BIGINT NULL,
    [SUP_RECDUE] BIGINT NULL,
    CONSTRAINT [PK_SUPDOC_DET] PRIMARY KEY ([SUP_DOCID])
);
GO

-- ==========================================
-- SUPDOC_ATT - Support Document Attachments
-- ==========================================
IF OBJECT_ID('[SUPDOC_ATT]', 'U') IS NOT NULL DROP TABLE [SUPDOC_ATT];
GO
CREATE TABLE [SUPDOC_ATT] (
    [SUPDOC_ATTID] BIGINT NOT NULL,
    [SUPDOC_DOCID] BIGINT NOT NULL,
    [SUPDOC_INVDOCID] BIGINT NOT NULL,
    [SUPDOC_REFFLAG] CHAR(1) NOT NULL,
    CONSTRAINT [PK_SUPDOC_ATT] PRIMARY KEY ([SUPDOC_ATTID])
);
GO

-- ==========================================
-- SUPDOC_COUNTER - Support Document Counter
-- ==========================================
IF OBJECT_ID('[SUPDOC_COUNTER]', 'U') IS NOT NULL DROP TABLE [SUPDOC_COUNTER];
GO
CREATE TABLE [SUPDOC_COUNTER] (
    [SUPDOC_BUID] VARCHAR(25) NOT NULL,
    [SUPDOC_NO] BIGINT NOT NULL
);
GO

PRINT 'CATEGORY_AND_VENDOR_MODULE Schema created successfully.';
GO
