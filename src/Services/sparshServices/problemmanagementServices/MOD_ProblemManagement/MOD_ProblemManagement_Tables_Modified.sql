-- ============================================================================
-- Module: Problem Management
-- Purpose: Manage problem tracking, solutions, approvals, and communications
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

-- Set database context
USE [ProblemManagementDb];
GO

-- ============================================================================
-- TABLE: PROBLEM_FUNCTION
-- Description: Function categories for problem classification
-- ============================================================================
IF OBJECT_ID('[dbo].[PROBLEM_FUNCTION]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PROBLEM_FUNCTION];
GO

CREATE TABLE [dbo].[PROBLEM_FUNCTION] (
    [FUNCID]            BIGINT NOT NULL,                -- Function ID (Primary Key)
    [FUNCNAME]          VARCHAR(200) NOT NULL,          -- Function Name
    CONSTRAINT [PK_PROBLEM_FUNCTION] PRIMARY KEY ([FUNCID])
);
GO

-- ============================================================================
-- TABLE: PROBLEM_IMPACT
-- Description: Impact levels for problem severity classification
-- ============================================================================
IF OBJECT_ID('[dbo].[PROBLEM_IMPACT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PROBLEM_IMPACT];
GO

CREATE TABLE [dbo].[PROBLEM_IMPACT] (
    [IMPACT_ID]         BIGINT NOT NULL,                -- Impact ID (Primary Key)
    [IMPACT_DESC]       VARCHAR(200) NOT NULL,          -- Impact Description
    CONSTRAINT [PK_PROBLEM_IMPACT] PRIMARY KEY ([IMPACT_ID])
);
GO

-- ============================================================================
-- TABLE: PROBLEM_MAIN
-- Description: Main problem records and tracking information
-- ============================================================================
IF OBJECT_ID('[dbo].[PROBLEM_MAIN]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PROBLEM_MAIN];
GO

CREATE TABLE [dbo].[PROBLEM_MAIN] (
    [PR_ID]             BIGINT NOT NULL,                -- Problem ID (Primary Key)
    [PR_OWNER]          BIGINT NOT NULL,                -- Problem Owner (Posted By)
    [PR_ENTEREDBY]      BIGINT NOT NULL,                -- Problem Entered By
    [PR_DESCRIPTION]    VARCHAR(255) NOT NULL,          -- Problem Description
    [PR_RESPEXPBY]      DATETIME2(3) NULL,              -- Response Expected By
    [PR_CATEGORY]       CHAR(1) NULL,                   -- 01 = Function, 02 = General
    [PR_SPECIALIZATION] BIGINT NULL,                    -- Specialization ID
    [PR_IMPACT]         VARCHAR(255) NULL,              -- Impact Description
    [PR_EXPRESULT]      VARCHAR(255) NULL,              -- Expected Result
    [PR_ENTEREDON]      DATETIME2(3) NULL,              -- Problem Entered On
    [PR_STATUS]         CHAR(1) NOT NULL,               -- P = Posted, A = Accepted, R = Rejected
    [PR_APPID]          BIGINT NULL,                    -- Last Approval ID
    [PR_STATEMENT]      VARCHAR(255) NULL,              -- Problem Statement
    [PR_TYPE]           CHAR(1) NULL,                   -- Problem Type
    [PR_ATTACH]         VARCHAR(255) NULL,              -- Attachment Reference
    [PR_PRBFLAG]        CHAR(1) NULL,                   -- Problem Flag
    [PR_PRBDESCRIPTION] VARCHAR(255) NULL,              -- Additional Problem Description
    [PR_POSTFLAG]       CHAR(1) NULL,                   -- Post Flag
    [PR_QUESTION]       VARCHAR(255) NULL,              -- Question
    [PR_UNITID]         BIGINT NOT NULL,                -- Unit ID
    [PR_SITEID]         BIGINT NOT NULL,                -- Site ID
    [PR_SOURCEID]       BIGINT NULL,                    -- Source ID
    [PR_MODBY]          BIGINT NOT NULL,                -- Modified By
    [PR_MODON]          DATETIME2(3) NOT NULL,          -- Modified On
    CONSTRAINT [PK_PROBLEM_MAIN] PRIMARY KEY ([PR_ID])
);

CREATE INDEX [IX_PROBLEM_MAIN_STATUS] ON [dbo].[PROBLEM_MAIN]([PR_STATUS]);
CREATE INDEX [IX_PROBLEM_MAIN_OWNER] ON [dbo].[PROBLEM_MAIN]([PR_OWNER]);
CREATE INDEX [IX_PROBLEM_MAIN_CATEGORY] ON [dbo].[PROBLEM_MAIN]([PR_CATEGORY]);
GO

-- ============================================================================
-- TABLE: PROBLEM_ATTACHMENT
-- Description: File attachments for problem records
-- ============================================================================
IF OBJECT_ID('[dbo].[PROBLEM_ATTACHMENT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PROBLEM_ATTACHMENT];
GO

CREATE TABLE [dbo].[PROBLEM_ATTACHMENT] (
    [PRAT_ID]           BIGINT NOT NULL,                -- Attachment ID (Primary Key)
    [PRAT_PRID]         BIGINT NULL,                    -- Problem ID (FK)
    [PRAT_FILENAME]     VARCHAR(2000) NULL,             -- File Name
    [PRAT_ENTEREDON]    DATETIME2(3) NULL,              -- Entered On
    CONSTRAINT [PK_PROBLEM_ATTACHMENT] PRIMARY KEY ([PRAT_ID]),
    CONSTRAINT [FK_PROBLEM_ATTACHMENT_PRID] FOREIGN KEY ([PRAT_PRID]) 
        REFERENCES [dbo].[PROBLEM_MAIN]([PR_ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_PROBLEM_ATTACHMENT_PRID] ON [dbo].[PROBLEM_ATTACHMENT]([PRAT_PRID]);
GO

-- ============================================================================
-- TABLE: PROBLEM_SOLUTION
-- Description: Solutions proposed for problems
-- ============================================================================
IF OBJECT_ID('[dbo].[PROBLEM_SOLUTION]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PROBLEM_SOLUTION];
GO

CREATE TABLE [dbo].[PROBLEM_SOLUTION] (
    [SOL_ID]            BIGINT NOT NULL,                -- Solution ID (Primary Key)
    [SOL_PRID]          BIGINT NOT NULL,                -- Problem ID (FK)
    [SOL_DESCRIPTION]   VARCHAR(255) NULL,              -- Solution Description
    [SOL_IMPLEMENTATION] CHAR(1) NULL,                  -- Y = Yes, N = No
    [SOL_ENTEREDBY]     BIGINT NOT NULL,                -- Entered By
    [SOL_ENTEREDON]     DATETIME2(3) NOT NULL,          -- Entered On
    [SOL_ATTACH]        VARCHAR(255) NULL,              -- Attachment Reference
    CONSTRAINT [PK_PROBLEM_SOLUTION] PRIMARY KEY ([SOL_ID]),
    CONSTRAINT [FK_PROBLEM_SOLUTION_PRID] FOREIGN KEY ([SOL_PRID]) 
        REFERENCES [dbo].[PROBLEM_MAIN]([PR_ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_PROBLEM_SOLUTION_PRID] ON [dbo].[PROBLEM_SOLUTION]([SOL_PRID]);
CREATE INDEX [IX_PROBLEM_SOLUTION_ENTEREDBY] ON [dbo].[PROBLEM_SOLUTION]([SOL_ENTEREDBY]);
GO

-- ============================================================================
-- TABLE: PROBLEM_APP
-- Description: Approval records for problems
-- ============================================================================
IF OBJECT_ID('[dbo].[PROBLEM_APP]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PROBLEM_APP];
GO

CREATE TABLE [dbo].[PROBLEM_APP] (
    [PRAPP_ID]          BIGINT NOT NULL,                -- Approval ID (Primary Key)
    [PRAPP_PRID]        BIGINT NOT NULL,                -- Problem ID (FK)
    [PRAPP_BY]          BIGINT NOT NULL,                -- Approved By
    [PRAPP_ON]          DATETIME2(3) NOT NULL,          -- Approved On
    [PRAPP_STATUS]      CHAR(1) NOT NULL,               -- Approval Status
    [PRAPP_REASON]      VARCHAR(255) NULL,              -- Reason for Approval/Rejection
    [PRAPP_AUDFLAG]     CHAR(1) NOT NULL,               -- 0 = All/1 = Selected
    CONSTRAINT [PK_PROBLEM_APP] PRIMARY KEY ([PRAPP_ID]),
    CONSTRAINT [FK_PROBLEM_APP_PRID] FOREIGN KEY ([PRAPP_PRID]) 
        REFERENCES [dbo].[PROBLEM_MAIN]([PR_ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_PROBLEM_APP_PRID] ON [dbo].[PROBLEM_APP]([PRAPP_PRID]);
CREATE INDEX [IX_PROBLEM_APP_STATUS] ON [dbo].[PROBLEM_APP]([PRAPP_STATUS]);
GO

-- ============================================================================
-- TABLE: PROBLEM_APPAUDIENCE
-- Description: Audience scope for problem approvals
-- ============================================================================
IF OBJECT_ID('[dbo].[PROBLEM_APPAUDIENCE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PROBLEM_APPAUDIENCE];
GO

CREATE TABLE [dbo].[PROBLEM_APPAUDIENCE] (
    [PRAUD_ID]          BIGINT NOT NULL,                -- Audience ID (Primary Key)
    [PRAUD_PRID]        BIGINT NOT NULL,                -- Reporting Unit ID
    [PRAUD_UNITID]      INT NOT NULL,                   -- Problem ID
    CONSTRAINT [PK_PROBLEM_APPAUDIENCE] PRIMARY KEY ([PRAUD_ID]),
    CONSTRAINT [FK_PROBLEM_APPAUDIENCE_PRID] FOREIGN KEY ([PRAUD_PRID]) 
        REFERENCES [dbo].[PROBLEM_MAIN]([PR_ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_PROBLEM_APPAUDIENCE_PRID] ON [dbo].[PROBLEM_APPAUDIENCE]([PRAUD_PRID]);
GO

-- ============================================================================
-- TABLE: SOLUTION_APP
-- Description: Approval records for solutions
-- ============================================================================
IF OBJECT_ID('[dbo].[SOLUTION_APP]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SOLUTION_APP];
GO

CREATE TABLE [dbo].[SOLUTION_APP] (
    [SOLAPP_ID]         BIGINT NOT NULL,                -- Approval ID (Primary Key)
    [SOLAPP_SOLID]      BIGINT NOT NULL,                -- Solution ID (FK)
    [SOLAPP_BY]         BIGINT NOT NULL,                -- Approved By
    [SOLAPP_ON]         DATETIME2(3) NOT NULL,          -- Approved On
    [SOLAPP_STATUS]     CHAR(1) NOT NULL,               -- Approval Status
    [SOLAPP_REASON]     VARCHAR(255) NULL,              -- Reason for Approval/Rejection
    [SOLAPP_AUDFLAG]    CHAR(1) NULL,                   -- Audience Flag
    CONSTRAINT [PK_SOLUTION_APP] PRIMARY KEY ([SOLAPP_ID]),
    CONSTRAINT [FK_SOLUTION_APP_SOLID] FOREIGN KEY ([SOLAPP_SOLID]) 
        REFERENCES [dbo].[PROBLEM_SOLUTION]([SOL_ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_SOLUTION_APP_SOLID] ON [dbo].[SOLUTION_APP]([SOLAPP_SOLID]);
CREATE INDEX [IX_SOLUTION_APP_STATUS] ON [dbo].[SOLUTION_APP]([SOLAPP_STATUS]);
GO

-- ============================================================================
-- TABLE: SOLUTION_COMMENT
-- Description: Comments on solutions
-- ============================================================================
IF OBJECT_ID('[dbo].[SOLUTION_COMMENT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SOLUTION_COMMENT];
GO

CREATE TABLE [dbo].[SOLUTION_COMMENT] (
    [SOLCOMMENT_ID]     BIGINT NOT NULL,                -- Comment ID (Primary Key)
    [SOLCOMMENT_SOLID]  BIGINT NOT NULL,                -- Solution ID (FK)
    [SOLCOMMENT_TEXT]   VARCHAR(500) NOT NULL,          -- Comment Text
    [SOLCOMMENT_BY]     BIGINT NOT NULL,                -- Commented By
    [SOLCOMMENT_ON]     DATETIME2(3) NOT NULL,          -- Commented On
    CONSTRAINT [PK_SOLUTION_COMMENT] PRIMARY KEY ([SOLCOMMENT_ID]),
    CONSTRAINT [FK_SOLUTION_COMMENT_SOLID] FOREIGN KEY ([SOLCOMMENT_SOLID]) 
        REFERENCES [dbo].[PROBLEM_SOLUTION]([SOL_ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_SOLUTION_COMMENT_SOLID] ON [dbo].[SOLUTION_COMMENT]([SOLCOMMENT_SOLID]);
GO

PRINT 'Problem Management Tables created successfully.';
GO
