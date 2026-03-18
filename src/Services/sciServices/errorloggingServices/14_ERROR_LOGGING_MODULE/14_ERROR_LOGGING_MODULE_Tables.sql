-- ==========================================
-- ERROR LOGGING MODULE - Table Definitions
-- Database: SCIDB
-- Module: Error Handling & Logging
-- Created: March 9, 2026
-- ==========================================

USE SCIDB;
GO

-- Table: ERRSP (Error in Stored Procedure)
CREATE TABLE [ERRSP] (
    [ERR_MESS] VARCHAR(4000) NULL,
    [ERR_SP] VARCHAR(100) NULL,
    [ERR_REF] INT NULL,
    [ERR_DATE] DATETIME2(3) NULL
);

PRINT 'ERROR_LOGGING_MODULE Tables created successfully.';
GO
