-- ==========================================
-- WORKORDER Module - Table Scripts
-- Database: MYWORKDB
-- Module: WORKORDER
-- Description: Work Order and Task Management System
-- Created: March 9, 2026
-- ==========================================

USE MYWORKDB;
GO

-- =====================================================
-- WORKORDER Core Tables
-- Note: WORK_ORDER and WORK_TASK tables are referenced
-- in procedures but definitions need to be created
-- =====================================================

-- Table: WORK_ORDER - Work Order Master
CREATE TABLE [WORK_ORDER] (
    [WORK_ORDER_ID] BIGINT NOT NULL  -- Work Order ID,
    [WORK_ORDER_NAME] VARCHAR(200) NOT NULL  -- Work Order Name,
    [WORK_ORDER_DESCRIPTION] VARCHAR(500) NOT NULL  -- Work Order Description,
    [DUE_DATE] DATE NOT NULL  -- Due Date,
    [ASSIGNED_TO] BIGINT NOT NULL  -- Assigned To Employee System ID,
    [WORK_ORDER_STATUS] CHAR(1) NOT NULL  -- Status (O - Open, C - Closed, A - Archived),
    [CREATED_BY] BIGINT NOT NULL  -- Created By Employee System ID,
    [CREATED_ON] DATETIME2(3) NOT NULL  -- Created On,
    [UPDATED_BY] BIGINT NULL  -- Updated By Employee System ID,
    [UPDATED_ON] DATETIME2(3) NULL  -- Updated On,
    CONSTRAINT [PK_WORK_ORDER] PRIMARY KEY ([WORK_ORDER_ID])
);

-- Table: WORK_TASK - Work Task Details  
CREATE TABLE [WORK_TASK] (
    [TASK_ID] BIGINT NOT NULL  -- Task ID,
    [WORK_ORDER_ID] BIGINT NOT NULL  -- Work Order ID (Foreign Key),
    [TASK_NAME] VARCHAR(100) NOT NULL  -- Task Name,
    [ASSIGNED_TO] BIGINT NOT NULL  -- Assigned To Employee System ID,
    [ESTIMATED_HOURS] INT NOT NULL  -- Estimated Hours,
    [ACTUAL_HOURS] INT NULL  -- Actual Hours,
    [TASK_STATUS] CHAR(1) NOT NULL  -- Status (O - Open, C - Completed, A - Archived, P - Paused),
    [COMPLETION_REMARKS] VARCHAR(500) NULL  -- Completion Remarks,
    [COMPLETED_BY] BIGINT NULL  -- Completed By Employee System ID,
    [COMPLETED_ON] DATETIME2(3) NULL  -- Completed On,
    [CREATED_BY] BIGINT NOT NULL  -- Created By Employee System ID,
    [CREATED_ON] DATETIME2(3) NOT NULL  -- Created On,
    [UPDATED_BY] BIGINT NULL  -- Updated By Employee System ID,
    [UPDATED_ON] DATETIME2(3) NULL  -- Updated On,
    CONSTRAINT [PK_WORK_TASK] PRIMARY KEY ([TASK_ID]),
    CONSTRAINT [FK_WORK_TASK_WORK_ORDER] FOREIGN KEY ([WORK_ORDER_ID]) REFERENCES [WORK_ORDER]([WORK_ORDER_ID])
);

-- =====================================================
-- WORKORDER Indexes
-- =====================================================
CREATE INDEX [IDX_WORK_TASK_WORK_ORDER_ID] ON [WORK_TASK]([WORK_ORDER_ID]);
CREATE INDEX [IDX_WORK_TASK_STATUS] ON [WORK_TASK]([TASK_STATUS]);
CREATE INDEX [IDX_WORK_ORDER_STATUS] ON [WORK_ORDER]([WORK_ORDER_STATUS]);

PRINT 'WORKORDER Module - Tables created successfully.';
GO
