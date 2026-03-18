-- ============================================================================
-- Module: Mobile Expense Management
-- Purpose: Manage mobile expenses and expense file attachments
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

-- Set database context
USE [SPARSHDB];
GO

-- ============================================================================
-- TABLE: MOBEXP_DET
-- Description: Mobile expense details and transaction records
-- ============================================================================
IF OBJECT_ID('[dbo].[MOBEXP_DET]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MOBEXP_DET];
GO

CREATE TABLE [dbo].[MOBEXP_DET] (
    [MOBEXP_ID]         DECIMAL(38) NOT NULL,           -- Expense ID (Primary Key)
    [MOBEXP_TPID]       DECIMAL(38) NOT NULL,           -- Trip/Project ID
    [MOBEXP_CATID]      DECIMAL(38) NOT NULL,           -- Expense Category ID
    [MOBEXP_DATE]       DATETIME2(3) NULL,              -- Expense Date
    [MOBEXP_COMMENT]    VARCHAR(500) NOT NULL,          -- Expense Description/Comment
    [MOBEXP_AMOUNT]     DECIMAL(19,2) NULL,             -- Expense Amount
    [MOBEXP_CURRID]     DECIMAL(38) NULL,               -- Currency ID
    [MOBEXP_ENTEREDBY]  DECIMAL(38) NOT NULL,           -- Entered By (Employee ID)
    [MOBEXP_ENTEREDON]  DATETIME2(3) NOT NULL,          -- Entry Timestamp
    CONSTRAINT [PK_MOBEXP_DET] PRIMARY KEY ([MOBEXP_ID])
);

CREATE INDEX [IX_MOBEXP_TPID] ON [dbo].[MOBEXP_DET]([MOBEXP_TPID]);
CREATE INDEX [IX_MOBEXP_CATID] ON [dbo].[MOBEXP_DET]([MOBEXP_CATID]);
CREATE INDEX [IX_MOBEXP_DATE] ON [dbo].[MOBEXP_DET]([MOBEXP_DATE]);
CREATE INDEX [IX_MOBEXP_ENTEREDBY] ON [dbo].[MOBEXP_DET]([MOBEXP_ENTEREDBY]);
GO

-- ============================================================================
-- TABLE: MOBEXP_FILE
-- Description: Mobile expense file attachments (photos, receipts, etc.)
-- ============================================================================
IF OBJECT_ID('[dbo].[MOBEXP_FILE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MOBEXP_FILE];
GO

CREATE TABLE [dbo].[MOBEXP_FILE] (
    [MOBEXPPHT_ID]      DECIMAL(38) NOT NULL,           -- File ID (Primary Key)
    [MOBEXPPHT_EXPID]   DECIMAL(38) NOT NULL,           -- Expense ID (FK to MOBEXP_DET)
    [MOBEXPPHT_FILENAME] VARCHAR(500) NOT NULL,         -- File Name
    [MOBEXPPHT_FILEDATA] NVARCHAR(MAX) NOT NULL,        -- File Data (Base64 or file path)
    CONSTRAINT [PK_MOBEXP_FILE] PRIMARY KEY ([MOBEXPPHT_ID]),
    CONSTRAINT [FK_MOBEXP_FILE_EXPID] FOREIGN KEY ([MOBEXPPHT_EXPID]) 
        REFERENCES [dbo].[MOBEXP_DET]([MOBEXP_ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_MOBEXP_FILE_EXPID] ON [dbo].[MOBEXP_FILE]([MOBEXPPHT_EXPID]);
GO

PRINT 'Mobile Expense Management Tables created successfully.';
GO
