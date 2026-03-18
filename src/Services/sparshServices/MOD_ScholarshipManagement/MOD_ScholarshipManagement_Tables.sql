-- ============================================================================
-- Module: Scholarship Management
-- Purpose: Manage scholarship schemes, applications, approvals, and disbursements
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

-- Set database context
USE [SPARSHDB];
GO

-- ============================================================================
-- TABLE: SCHOLARSHIP_MASTER
-- Description: Master data for scholarship schemes
-- ============================================================================
IF OBJECT_ID('[dbo].[SCHOLARSHIP_MASTER]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SCHOLARSHIP_MASTER];
GO

CREATE TABLE [dbo].[SCHOLARSHIP_MASTER] (
    [SCHOLARSHIP_ID]            BIGINT NOT NULL,        -- Scholarship ID (Primary Key)
    [SCHOLARSHIP_NAME]          VARCHAR(255) NOT NULL,  -- Scholarship Name
    [SCHOLARSHIP_CODE]          VARCHAR(50) NOT NULL,   -- Scholarship Code (Unique)
    [SCHOLARSHIP_DESCRIPTION]   NVARCHAR(MAX) NULL,     -- Description
    [SCHOLARSHIP_COVERAGE_PERCENT] DECIMAL(5,2) NULL,   -- Coverage Percentage (e.g., 50.00 for 50%)
    [SCHOLARSHIP_STATUS]        CHAR(1) NOT NULL,       -- A = Active, I = Inactive
    [SCHOLARSHIP_CREATEDBY]     BIGINT NOT NULL,        -- Created By
    [SCHOLARSHIP_CREATEDON]     DATETIME2(3) NOT NULL,  -- Created On
    [SCHOLARSHIP_MODIFIEDBY]    BIGINT NULL,            -- Modified By
    [SCHOLARSHIP_MODIFIEDON]    DATETIME2(3) NULL,      -- Modified On
    CONSTRAINT [PK_SCHOLARSHIP_MASTER] PRIMARY KEY ([SCHOLARSHIP_ID]),
    CONSTRAINT [UQ_SCHOLARSHIP_CODE] UNIQUE ([SCHOLARSHIP_CODE])
);

CREATE INDEX [IX_SCHOLARSHIP_MASTER_STATUS] ON [dbo].[SCHOLARSHIP_MASTER]([SCHOLARSHIP_STATUS]);
CREATE INDEX [IX_SCHOLARSHIP_MASTER_CODE] ON [dbo].[SCHOLARSHIP_MASTER]([SCHOLARSHIP_CODE]);
GO

-- ============================================================================
-- TABLE: SCHOLARSHIP_ELIGIBILITY_CRITERIA
-- Description: Eligibility criteria for scholarships
-- ============================================================================
IF OBJECT_ID('[dbo].[SCHOLARSHIP_ELIGIBILITY_CRITERIA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SCHOLARSHIP_ELIGIBILITY_CRITERIA];
GO

CREATE TABLE [dbo].[SCHOLARSHIP_ELIGIBILITY_CRITERIA] (
    [ELIGIBILITY_ID]        BIGINT NOT NULL,            -- Eligibility Criteria ID (Primary Key)
    [SCHOLARSHIP_ID]        BIGINT NOT NULL,            -- Scholarship ID (FK)
    [ELIGIBILITY_CRITERIA]  VARCHAR(500) NOT NULL,      -- Criteria Description
    [ELIGIBILITY_STATUS]    CHAR(1) NOT NULL,           -- A = Active, I = Inactive
    [ELIGIBILITY_CREATEDBY] BIGINT NOT NULL,            -- Created By
    [ELIGIBILITY_CREATEDON] DATETIME2(3) NOT NULL,      -- Created On
    CONSTRAINT [PK_SCHOLARSHIP_ELIGIBILITY] PRIMARY KEY ([ELIGIBILITY_ID]),
    CONSTRAINT [FK_SCHOLARSHIP_ELIGIBILITY_SCHID] FOREIGN KEY ([SCHOLARSHIP_ID]) 
        REFERENCES [dbo].[SCHOLARSHIP_MASTER]([SCHOLARSHIP_ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_ELIGIBILITY_SCHID] ON [dbo].[SCHOLARSHIP_ELIGIBILITY_CRITERIA]([SCHOLARSHIP_ID]);
CREATE INDEX [IX_ELIGIBILITY_STATUS] ON [dbo].[SCHOLARSHIP_ELIGIBILITY_CRITERIA]([ELIGIBILITY_STATUS]);
GO

-- ============================================================================
-- TABLE: SCHOLARSHIP_APPLICATION
-- Description: Student scholarship applications
-- ============================================================================
IF OBJECT_ID('[dbo].[SCHOLARSHIP_APPLICATION]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SCHOLARSHIP_APPLICATION];
GO

CREATE TABLE [dbo].[SCHOLARSHIP_APPLICATION] (
    [APPLICATION_ID]        BIGINT NOT NULL,            -- Application ID (Primary Key)
    [EMP_STUDENT_ID]        BIGINT NOT NULL,            -- Student/Employee ID
    [SCHOLARSHIP_ID]        BIGINT NOT NULL,            -- Scholarship ID (FK)
    [APPLICATION_DATE]      DATE NOT NULL,              -- Application Date
    [FAMILY_INCOME]         DECIMAL(19,0) NULL,         -- Family Annual Income
    [APPLICATION_STATUS]    CHAR(1) NOT NULL,           -- S = Submitted, A = Approved, R = Rejected, C = Closed
    [APPROVED_AMOUNT]       DECIMAL(19,0) NULL,         -- Approved/Sanctioned Amount
    [APPROVED_BY]           BIGINT NULL,                -- Approved By (Employee ID)
    [APPROVAL_DATE]         DATETIME2(3) NULL,          -- Approval Date
    [REMARKS]               VARCHAR(500) NULL,          -- Remarks
    [CREATED_BY]            BIGINT NOT NULL,            -- Created By
    [CREATED_ON]            DATETIME2(3) NOT NULL,      -- Created On
    [UPDATED_BY]            BIGINT NULL,                -- Updated By
    [UPDATED_ON]            DATETIME2(3) NULL,          -- Updated On
    CONSTRAINT [PK_SCHOLARSHIP_APPLICATION] PRIMARY KEY ([APPLICATION_ID]),
    CONSTRAINT [FK_APPLICATION_SCHID] FOREIGN KEY ([SCHOLARSHIP_ID]) 
        REFERENCES [dbo].[SCHOLARSHIP_MASTER]([SCHOLARSHIP_ID]) ON DELETE RESTRICT
);

CREATE INDEX [IX_APPLICATION_STUDENTID] ON [dbo].[SCHOLARSHIP_APPLICATION]([EMP_STUDENT_ID]);
CREATE INDEX [IX_APPLICATION_SCHID] ON [dbo].[SCHOLARSHIP_APPLICATION]([SCHOLARSHIP_ID]);
CREATE INDEX [IX_APPLICATION_STATUS] ON [dbo].[SCHOLARSHIP_APPLICATION]([APPLICATION_STATUS]);
CREATE INDEX [IX_APPLICATION_DATE] ON [dbo].[SCHOLARSHIP_APPLICATION]([APPLICATION_DATE]);
GO

-- ============================================================================
-- TABLE: SCHOLARSHIP_DISBURSEMENT
-- Description: Scholarship disbursement transactions
-- ============================================================================
IF OBJECT_ID('[dbo].[SCHOLARSHIP_DISBURSEMENT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SCHOLARSHIP_DISBURSEMENT];
GO

CREATE TABLE [dbo].[SCHOLARSHIP_DISBURSEMENT] (
    [DISBURSEMENT_ID]       BIGINT NOT NULL,            -- Disbursement ID (Primary Key)
    [APPLICATION_ID]        BIGINT NOT NULL,            -- Application ID (FK)
    [STUDENT_ID]            BIGINT NOT NULL,            -- Student/Employee ID
    [SCHOLARSHIP_ID]        BIGINT NOT NULL,            -- Scholarship ID (FK)
    [DISBURSEMENT_AMOUNT]   DECIMAL(19,0) NOT NULL,     -- Amount Disbursed
    [DISBURSEMENT_DATE]     DATETIME2(3) NULL,          -- Disbursement Date
    [DISBURSEMENT_STATUS]   CHAR(1) NOT NULL,           -- P = Pending, D = Disbursed, C = Cancelled
    [REFERENCE_NUMBER]      VARCHAR(100) NULL,          -- Payment Reference Number
    [BANK_DETAILS]          VARCHAR(500) NULL,          -- Bank Account Details
    [CREATED_BY]            BIGINT NOT NULL,            -- Created By
    [CREATED_ON]            DATETIME2(3) NOT NULL,      -- Created On
    [UPDATED_BY]            BIGINT NULL,                -- Updated By
    [UPDATED_ON]            DATETIME2(3) NULL,          -- Updated On
    CONSTRAINT [PK_SCHOLARSHIP_DISBURSEMENT] PRIMARY KEY ([DISBURSEMENT_ID]),
    CONSTRAINT [FK_DISBURSEMENT_APPID] FOREIGN KEY ([APPLICATION_ID]) 
        REFERENCES [dbo].[SCHOLARSHIP_APPLICATION]([APPLICATION_ID]) ON DELETE RESTRICT,
    CONSTRAINT [FK_DISBURSEMENT_SCHID] FOREIGN KEY ([SCHOLARSHIP_ID]) 
        REFERENCES [dbo].[SCHOLARSHIP_MASTER]([SCHOLARSHIP_ID]) ON DELETE RESTRICT
);

CREATE INDEX [IX_DISBURSEMENT_APPID] ON [dbo].[SCHOLARSHIP_DISBURSEMENT]([APPLICATION_ID]);
CREATE INDEX [IX_DISBURSEMENT_STUDENTID] ON [dbo].[SCHOLARSHIP_DISBURSEMENT]([STUDENT_ID]);
CREATE INDEX [IX_DISBURSEMENT_STATUS] ON [dbo].[SCHOLARSHIP_DISBURSEMENT]([DISBURSEMENT_STATUS]);
CREATE INDEX [IX_DISBURSEMENT_DATE] ON [dbo].[SCHOLARSHIP_DISBURSEMENT]([DISBURSEMENT_DATE]);
GO

PRINT 'Scholarship Management Tables created successfully.';
GO
