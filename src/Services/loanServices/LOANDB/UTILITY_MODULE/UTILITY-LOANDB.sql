-- ==========================================
-- Module: UTILITY
-- Database: LOANDB
-- Description: Utilities and Temporary Tables
-- ==========================================

USE [LOANDB];
GO

-- Table: TOAD_PLAN_SQL - toad plan
CREATE TABLE [TOAD_PLAN_SQL] (
    [USERNAME] VARCHAR(30) NULL,
    [STATEMENT_ID] VARCHAR(32) NULL,
    [TIMESTAMP] DATETIME2(3) NULL,
    [STATEMENT] VARCHAR(2000) NULL
);
GO
