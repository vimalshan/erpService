-- ==========================================
-- CompensationModule
-- Database: SRFSPARSHDB
-- Module Purpose: Compensation and Grade Management
-- Created: March 09, 2026
-- ==========================================

USE SRFSPARSHDB;
GO

-- Drop table if exists
IF OBJECT_ID('[COMP_GRADE]', 'U') IS NOT NULL DROP TABLE [COMP_GRADE];
GO

-- ==========================================
-- Table: COMP_GRADE - Compensation Grade Master
-- Description: Master table for employee compensation grades
-- ==========================================
CREATE TABLE [COMP_GRADE] (
    [GRADE_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [GRADE_CODE] VARCHAR(50) NOT NULL UNIQUE,
    [GRADE_NAME] VARCHAR(255) NOT NULL,
    [GRADE_LEVEL] INT NOT NULL,
    [BASE_SALARY] DECIMAL(19,2) NOT NULL,
    [HRA_PERCENTAGE] DECIMAL(5,2),
    [DA_PERCENTAGE] DECIMAL(5,2),
    [GRADE_STATUS] CHAR(1) DEFAULT 'A', -- A=Active, I=Inactive
    [EFFECTIVE_FROM] DATE NOT NULL,
    [EFFECTIVE_TO] DATE,
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [UPDATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3)
);
GO

-- Create Indexes
CREATE INDEX [IX_COMP_GRADE_STATUS] ON [COMP_GRADE]([GRADE_STATUS]);
CREATE INDEX [IX_COMP_GRADE_LEVEL] ON [COMP_GRADE]([GRADE_LEVEL]);
CREATE INDEX [IX_COMP_GRADE_EFFECTIVE] ON [COMP_GRADE]([EFFECTIVE_FROM], [EFFECTIVE_TO]);
GO

PRINT 'CompensationModule_Schema created successfully.';
GO
