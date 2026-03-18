-- ==========================================
-- Module: DOCUMENT
-- Database: LOANDB
-- Description: Loan Documents
-- ==========================================

USE [LOANDB];
GO

-- Table: LOAN_DOCUMENTS - Loan Documents
CREATE TABLE [LOAN_DOCUMENTS] (
    [LOANDOC_ID] BIGINT NOT NULL  -- Document ID,
    [LOANDOC_LOANID] BIGINT NOT NULL  -- Loan ID,
    [LOANDOC_TYPEID] BIGINT NOT NULL  -- Document Type,
    [LOANDOC_LASTMODIFIEDBY] BIGINT NOT NULL  -- Last Modified By,
    [LOANDOC_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Last Modified On,
    CONSTRAINT [PK_LOAN_DOCUMENTS] PRIMARY KEY ([LOANDOC_ID])
);
GO
