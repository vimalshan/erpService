-- ==========================================
-- BATCH Module - Table Scripts
-- Database: MYWORKDB
-- Module: BATCH
-- Description: Batch Processing and Monthly Management Module
-- Created: March 9, 2026
-- ==========================================

USE MYWORKDB;
GO

-- =====================================================
-- BATCH Core Tables
-- =====================================================

-- Table: BATCH_MASTER - batch master
CREATE TABLE [BATCH_MASTER] (
    [BATCH_ID] BIGINT NOT NULL,
    [BATCH_MONTHNO] INT NOT NULL,
    [BATCH_STATUS] CHAR(1) NOT NULL,
    [BATCH_LASTMODIFIEDBY] BIGINT NOT NULL,
    [BATCH_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_BATCH_MASTER] PRIMARY KEY ([BATCH_ID])
);

PRINT 'BATCH Module - Tables created successfully.';
GO
