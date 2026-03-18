-- ==========================================
-- ApprovalModule
-- Database: SRFSPARSHDB
-- Module Purpose: Approval and Module Master Management
-- Created: March 09, 2026
-- ==========================================

USE SRFSPARSHDB;
GO

-- Drop tables if they exist (reverse order for dependencies)
IF OBJECT_ID('[APPROVER_EMP]', 'U') IS NOT NULL DROP TABLE [APPROVER_EMP];
GO
IF OBJECT_ID('[APPR_MAST]', 'U') IS NOT NULL DROP TABLE [APPR_MAST];
GO

-- ==========================================
-- Table: APPR_MAST - MODULE MASTER - PER,DDP,LET
-- Description: Approval Module Master containing approval process definitions
-- ==========================================
CREATE TABLE [APPR_MAST] (
    [APPR_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [APPR_CODE] VARCHAR(50) NOT NULL UNIQUE,
    [APPR_NAME] VARCHAR(255) NOT NULL,
    [APPR_MODULE] VARCHAR(100) NOT NULL, -- PER, DDP, LET
    [APPR_STATUS] CHAR(1) DEFAULT 'A', -- A=Active, I=Inactive
    [APPR_LEVEL] INT NOT NULL DEFAULT 1,
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [UPDATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3)
);
GO

-- ==========================================
-- Table: APPROVER_EMP - Approver Employee Mapping
-- Description: Maps approvers to approval processes with employee references
-- Relationship: References APPR_MAST (AP_MAN_MOD)
-- ==========================================
CREATE TABLE [APPROVER_EMP] (
    [APPROVER_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [APPR_ID] BIGINT NOT NULL,
    [EMP_SYSID] BIGINT NOT NULL,
    [APPROVER_LEVEL] INT NOT NULL,
    [APPROVER_STATUS] CHAR(1) DEFAULT 'A', -- A=Active, I=Inactive
    [EFFECTIVE_FROM] DATE NOT NULL,
    [EFFECTIVE_TO] DATE,
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [UPDATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3),
    CONSTRAINT [FK_APPROVER_EMP_APPR_MAST] FOREIGN KEY ([APPR_ID]) REFERENCES [APPR_MAST]([APPR_ID])
);
GO

-- Create Indexes
CREATE INDEX [IX_APPROVER_EMP_APPR_ID] ON [APPROVER_EMP]([APPR_ID]);
CREATE INDEX [IX_APPROVER_EMP_EMP_SYSID] ON [APPROVER_EMP]([EMP_SYSID]);
CREATE INDEX [IX_APPR_MAST_MODULE] ON [APPR_MAST]([APPR_MODULE]);
GO

PRINT 'ApprovalModule_Schema created successfully.';
GO
