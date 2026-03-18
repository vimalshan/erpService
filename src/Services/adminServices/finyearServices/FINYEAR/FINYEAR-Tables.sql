-- ==========================================
-- MODULE: FINYEAR
-- Component: Tables
-- Description: Financial year master tables
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- Table: FINYEAR_MASTER
-- Purpose: Defines financial year boundaries and status
CREATE TABLE [FINYEAR_MASTER] (
    [FY_ID] BIGINT NOT NULL,
    [FY_NAME] VARCHAR(27) NOT NULL,
    [FY_STARTDATE] DATETIME2(3) NOT NULL,
    [FY_CLOSEDATE] DATETIME2(3) NOT NULL,
    [FY_UPDATED_BY] BIGINT NOT NULL,
    [FY_UPDATED_ON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_FINYEAR_MASTER] PRIMARY KEY ([FY_ID])
);
GO

-- Create indexes for performance
CREATE INDEX [IDX_FINYEAR_STARTDATE] ON [FINYEAR_MASTER]([FY_STARTDATE]);
GO

-- ==========================================
-- END OF FINYEAR TABLES
-- ==========================================
