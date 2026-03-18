-- ==========================================
-- Module: TASK MANAGEMENT
-- Database: TASKDB
-- Purpose: Task Mail & Notification Management
-- Tables for managing task assignments and email notifications
-- ==========================================

USE [TASKDB];
GO

-- ==========================================
-- TASK MANAGEMENT TABLES
-- ==========================================

-- Table: TASK_MAIL
-- Purpose: Task assignment and email notification tracking
CREATE TABLE [TASK_MAIL] (
    [MID] DECIMAL(38) NOT NULL,
    [SYSID] DECIMAL(38) NOT NULL,
    CONSTRAINT [PK_TASK_MAIL] PRIMARY KEY ([MID])
);

-- ==========================================
-- END OF SCRIPT - TASK MODULE TABLES
-- ==========================================
