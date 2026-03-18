-- ==========================================
-- TimesheetModule Seed Data Script
-- Database: SRFSPARSHDB
-- Run after InitialCreate migration
-- Created: March 15, 2026
-- ==========================================

USE SRFSPARSHDB;
GO

-- Only insert seed data if table is empty
IF NOT EXISTS (SELECT 1 FROM [TSE_TIMESHEET])
BEGIN

    PRINT 'Inserting seed timesheet records...';

    -- ── Employee 1001 — APPROVED entries (week of March 2–6, 2026) ──────────
    INSERT INTO [TSE_TIMESHEET]
        (EMP_SYSID, TIMESHEET_DATE, WORK_DATE, START_TIME, END_TIME,
         TOTAL_HOURS, PROJECT_ID, TASK_ID, WORK_DESCRIPTION,
         RECORDED_DATE, TIMESHEET_STATUS, APPROVAL_STATUS,
         APPROVED_BY, APPROVED_ON, CREATED_BY, CREATED_ON)
    VALUES
    (1001, '2026-03-15', '2026-03-02', '09:00', '17:30', 8.5, 100, 201,
     'Project Alpha — development sprint day 1.', GETDATE(), 'APPROVED', 'APPROVED', 9001, GETDATE(), 1001, GETDATE()),
    (1001, '2026-03-15', '2026-03-03', '09:00', '17:30', 8.5, 100, 201,
     'Project Alpha — development sprint day 2.', GETDATE(), 'APPROVED', 'APPROVED', 9001, GETDATE(), 1001, GETDATE()),
    (1001, '2026-03-15', '2026-03-04', '09:00', '17:30', 8.5, 100, 201,
     'Project Alpha — development sprint day 3.', GETDATE(), 'APPROVED', 'APPROVED', 9001, GETDATE(), 1001, GETDATE()),
    (1001, '2026-03-15', '2026-03-05', '09:00', '17:30', 8.5, 100, 201,
     'Project Alpha — development sprint day 4.', GETDATE(), 'APPROVED', 'APPROVED', 9001, GETDATE(), 1001, GETDATE()),
    (1001, '2026-03-15', '2026-03-06', '09:00', '17:30', 8.5, 100, 201,
     'Project Alpha — development sprint day 5.', GETDATE(), 'APPROVED', 'APPROVED', 9001, GETDATE(), 1001, GETDATE());

    -- ── Employee 1001 — SUBMITTED entries (week of March 9–11, 2026) ─────────
    INSERT INTO [TSE_TIMESHEET]
        (EMP_SYSID, TIMESHEET_DATE, WORK_DATE, START_TIME, END_TIME,
         TOTAL_HOURS, PROJECT_ID, TASK_ID, WORK_DESCRIPTION,
         RECORDED_DATE, TIMESHEET_STATUS, APPROVAL_STATUS,
         CREATED_BY, CREATED_ON)
    VALUES
    (1001, '2026-03-15', '2026-03-09', '09:00', '17:00', 8.0, 100, 202,
     'Project Alpha — testing sprint day 1.', GETDATE(), 'SUBMITTED', 'PENDING', 1001, GETDATE()),
    (1001, '2026-03-15', '2026-03-10', '09:00', '17:00', 8.0, 100, 202,
     'Project Alpha — testing sprint day 2.', GETDATE(), 'SUBMITTED', 'PENDING', 1001, GETDATE()),
    (1001, '2026-03-15', '2026-03-11', '09:00', '17:00', 8.0, 100, 202,
     'Project Alpha — testing sprint day 3.', GETDATE(), 'SUBMITTED', 'PENDING', 1001, GETDATE());

    -- ── Employee 1002 — DRAFT entries (week of March 12–14, 2026) ───────────
    INSERT INTO [TSE_TIMESHEET]
        (EMP_SYSID, TIMESHEET_DATE, WORK_DATE, START_TIME, END_TIME,
         TOTAL_HOURS, PROJECT_ID, TASK_ID, WORK_DESCRIPTION,
         RECORDED_DATE, TIMESHEET_STATUS, APPROVAL_STATUS,
         CREATED_BY, CREATED_ON)
    VALUES
    (1002, '2026-03-15', '2026-03-12', '08:30', '16:30', 8.0, 101, 301,
     'Project Beta — UI design review.', GETDATE(), 'DRAFT', 'PENDING', 1002, GETDATE()),
    (1002, '2026-03-15', '2026-03-13', '08:30', '16:30', 8.0, 101, 301,
     'Project Beta — wireframe iteration.', GETDATE(), 'DRAFT', 'PENDING', 1002, GETDATE()),
    (1002, '2026-03-15', '2026-03-14', '08:30', '16:30', 8.0, 101, 301,
     'Project Beta — prototype build.', GETDATE(), 'DRAFT', 'PENDING', 1002, GETDATE());

    -- ── Employee 1003 — APPROVED entry ───────────────────────────────────────
    INSERT INTO [TSE_TIMESHEET]
        (EMP_SYSID, TIMESHEET_DATE, WORK_DATE, START_TIME, END_TIME,
         TOTAL_HOURS, PROJECT_ID, TASK_ID, WORK_DESCRIPTION,
         RECORDED_DATE, TIMESHEET_STATUS, APPROVAL_STATUS,
         APPROVED_BY, APPROVED_ON, CREATED_BY, CREATED_ON)
    VALUES
    (1003, '2026-03-15', '2026-03-09', '09:00', '18:00', 9.0, 101, 302,
     'Project Beta — client demo preparation.', GETDATE(), 'APPROVED', 'APPROVED', 9001, GETDATE(), 1003, GETDATE());

    -- ── Employee 1003 — REJECTED entry ───────────────────────────────────────
    INSERT INTO [TSE_TIMESHEET]
        (EMP_SYSID, TIMESHEET_DATE, WORK_DATE, START_TIME, END_TIME,
         TOTAL_HOURS, PROJECT_ID, TASK_ID, WORK_DESCRIPTION,
         RECORDED_DATE, TIMESHEET_STATUS, APPROVAL_STATUS,
         REJECTION_REASON, CREATED_BY, CREATED_ON)
    VALUES
    (1003, '2026-03-15', '2026-03-10', '09:00', '13:00', 4.0, 101, 302,
     'Project Beta — requirements review (half day).',
     GETDATE(), 'REJECTED', 'REJECTED',
     'Hours do not match the attendance log. Please correct and resubmit.',
     1003, GETDATE());

    PRINT 'Seed data inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Seed skipped — TSE_TIMESHEET table already contains data.';
END
GO

-- ── Verification query ────────────────────────────────────────────────────────
SELECT
    TIMESHEET_ID,
    EMP_SYSID,
    WORK_DATE,
    TOTAL_HOURS,
    TIMESHEET_STATUS,
    APPROVAL_STATUS
FROM [TSE_TIMESHEET]
ORDER BY EMP_SYSID, WORK_DATE;
GO
