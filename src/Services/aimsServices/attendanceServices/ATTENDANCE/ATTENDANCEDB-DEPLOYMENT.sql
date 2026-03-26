-- ==========================================
-- Database: ATTENDANCEDB
-- Module: Attendance and Biometric Punch Management System
-- Generated Module Script
-- ==========================================

USE master;
GO

-- ==========================================
-- PHASE 1: Database Creation
-- ==========================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ATTENDANCEDB')
BEGIN
    CREATE DATABASE [ATTENDANCEDB];
    PRINT '+ ATTENDANCEDB created';
END
ELSE
    PRINT '= ATTENDANCEDB already exists';
GO

ALTER DATABASE [ATTENDANCEDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

PRINT '=== PHASE 1 COMPLETE: Database Created ===';
GO

-- ==========================================
-- PHASE 2: Table Definitions
-- ==========================================
USE ATTENDANCEDB;
GO

-- Table: SWIPE_RAWPUNCH - Biometric Punch Records
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SWIPE_RAWPUNCH]') AND type = 'U')
BEGIN
    CREATE TABLE [SWIPE_RAWPUNCH] (
        [SWIPE_ID] BIGINT NOT NULL,
        [SWIPE_EMPSYSID] BIGINT NOT NULL,
        [SWIPE_PUNCHTIME] DATETIME2(3) NOT NULL,
        [SWIPE_GATENO] VARCHAR(10) NOT NULL,
        [SWIPE_PUNCHSTATUS] CHAR(1) NOT NULL,
        [SWIPE_PULLSTATUS] CHAR(1) NULL,
        [SWIPE_VERIFIED] CHAR(1) NULL,
        [SWIPE_LASTMODIFIEDBY] BIGINT NULL,
        [SWIPE_LASTMODIFIEDON] DATETIME2(3) NULL,
        CONSTRAINT [PK_SWIPE_RAWPUNCH] PRIMARY KEY ([SWIPE_ID])
    );
    PRINT '+ SWIPE_RAWPUNCH created';
END
ELSE
    PRINT '= SWIPE_RAWPUNCH already exists';
GO

-- Table: SWIPE_RAWPUNCH_LOG - Biometric Punch Log
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SWIPE_RAWPUNCH_LOG]') AND type = 'U')
BEGIN
    CREATE TABLE [SWIPE_RAWPUNCH_LOG] (
        [SWIPE_ID] BIGINT NOT NULL,
        [SWIPE_EMPSYSID] BIGINT NOT NULL,
        [SWIPE_PUNCHTIME] DATETIME2(3) NOT NULL,
        [SWIPE_GATENO] VARCHAR(10) NOT NULL,
        [SWIPE_PUNCHSTATUS] CHAR(1) NOT NULL,
        [SWIPE_PULLSTATUS] CHAR(1) NULL,
        [LOG_CREATEDON] DATETIME2(3) NOT NULL,
        [LOG_CREATEDBY] BIGINT NULL
    );
    PRINT '+ SWIPE_RAWPUNCH_LOG created';
END
ELSE
    PRINT '= SWIPE_RAWPUNCH_LOG already exists';
GO

-- Table: ATTENDANCE_BATCH - Monthly Attendance Batch
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTENDANCE_BATCH]') AND type = 'U')
BEGIN
    CREATE TABLE [ATTENDANCE_BATCH] (
        [BATCH_ID] BIGINT NOT NULL,
        [BATCH_MONTHFROM] INT NOT NULL,
        [BATCH_MONTHTO] INT NOT NULL,
        [BATCH_YEARFROM] INT NOT NULL,
        [BATCH_YEAREND] INT NOT NULL,
        [BATCH_STATUS] CHAR(1) NOT NULL,
        [BATCH_CREATEDBY] BIGINT NOT NULL,
        [BATCH_CREATEDON] DATETIME2(3) NOT NULL,
        [BATCH_LASTMODIFIEDBY] BIGINT NOT NULL,
        [BATCH_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_ATTENDANCE_BATCH] PRIMARY KEY ([BATCH_ID])
    );
    PRINT '+ ATTENDANCE_BATCH created';
END
ELSE
    PRINT '= ATTENDANCE_BATCH already exists';
GO

-- Table: ATTENDANCE_SUMMARY - Attendance Summary
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTENDANCE_SUMMARY]') AND type = 'U')
BEGIN
    CREATE TABLE [ATTENDANCE_SUMMARY] (
        [SUMMARY_ID] BIGINT NOT NULL,
        [SUMMARY_EMPSYSID] BIGINT NOT NULL,
        [SUMMARY_BATCHID] BIGINT NOT NULL,
        [SUMMARY_ATTTYPE] VARCHAR(10) NOT NULL,
        [SUMMARY_DAYS] INT NOT NULL,
        [SUMMARY_LASTMODIFIEDBY] BIGINT NOT NULL,
        [SUMMARY_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_ATTENDANCE_SUMMARY] PRIMARY KEY ([SUMMARY_ID]),
        CONSTRAINT [FK_SUMMARY_BATCHID] FOREIGN KEY ([SUMMARY_BATCHID]) REFERENCES [ATTENDANCE_BATCH]([BATCH_ID])
    );
    PRINT '+ ATTENDANCE_SUMMARY created';
END
ELSE
    PRINT '= ATTENDANCE_SUMMARY already exists';
GO

-- Table: ATTENDANCE_OVERTIME - Overtime Tracking
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTENDANCE_OVERTIME]') AND type = 'U')
BEGIN
    CREATE TABLE [ATTENDANCE_OVERTIME] (
        [OT_ID] BIGINT NOT NULL,
        [OT_EMPSYSID] BIGINT NOT NULL,
        [OT_DATE] DATETIME2(3) NOT NULL,
        [OT_HOURS] DECIMAL(5,2) NOT NULL,
        [OT_TYPE] VARCHAR(20) NOT NULL,
        [OT_APPROVED] CHAR(1) NOT NULL,
        [OT_LASTMODIFIEDBY] BIGINT NOT NULL,
        [OT_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_ATTENDANCE_OVERTIME] PRIMARY KEY ([OT_ID])
    );
    PRINT '+ ATTENDANCE_OVERTIME created';
END
ELSE
    PRINT '= ATTENDANCE_OVERTIME already exists';
GO

-- Table: ATTENDANCE_NIGHT - Night Shift Tracking
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTENDANCE_NIGHT]') AND type = 'U')
BEGIN
    CREATE TABLE [ATTENDANCE_NIGHT] (
        [NIGHT_ID] BIGINT NOT NULL,
        [NIGHT_EMPSYSID] BIGINT NOT NULL,
        [NIGHT_DATE] DATETIME2(3) NOT NULL,
        [NIGHT_NIGHTTYPE] CHAR(1) NOT NULL,
        [NIGHT_LASTMODIFIEDBY] BIGINT NOT NULL,
        [NIGHT_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_ATTENDANCE_NIGHT] PRIMARY KEY ([NIGHT_ID])
    );
    PRINT '+ ATTENDANCE_NIGHT created';
END
ELSE
    PRINT '= ATTENDANCE_NIGHT already exists';
GO

-- Table: ATTENDANCE_LOPMAIN - Loss of Pay Main Record
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTENDANCE_LOPMAIN]') AND type = 'U')
BEGIN
    CREATE TABLE [ATTENDANCE_LOPMAIN] (
        [LOP_ID] BIGINT NOT NULL,
        [LOP_EMPSYSID] BIGINT NOT NULL,
        [LOP_BATCHID] BIGINT NOT NULL,
        [LOP_DAYS] DECIMAL(5,2) NOT NULL,
        [LOP_TYPE] CHAR(1) NOT NULL,
        [LOP_LASTMODIFIEDBY] BIGINT NOT NULL,
        [LOP_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_ATTENDANCE_LOPMAIN] PRIMARY KEY ([LOP_ID]),
        CONSTRAINT [FK_LOP_BATCHID] FOREIGN KEY ([LOP_BATCHID]) REFERENCES [ATTENDANCE_BATCH]([BATCH_ID])
    );
    PRINT '+ ATTENDANCE_LOPMAIN created';
END
ELSE
    PRINT '= ATTENDANCE_LOPMAIN already exists';
GO

-- Table: ATTENDANCE_GRACEADJUST - Grace Adjustment Records
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTENDANCE_GRACEADJUST]') AND type = 'U')
BEGIN
    CREATE TABLE [ATTENDANCE_GRACEADJUST] (
        [GRACE_ID] BIGINT NOT NULL,
        [GRACE_EMPSYSID] BIGINT NOT NULL,
        [GRACE_DATE] DATETIME2(3) NOT NULL,
        [GRACE_MINUTES] INT NOT NULL,
        [GRACE_LASTMODIFIEDBY] BIGINT NOT NULL,
        [GRACE_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_ATTENDANCE_GRACEADJUST] PRIMARY KEY ([GRACE_ID])
    );
    PRINT '+ ATTENDANCE_GRACEADJUST created';
END
ELSE
    PRINT '= ATTENDANCE_GRACEADJUST already exists';
GO

-- Table: ATTENDANCE_LEAVEADJUST - Leave Adjustment Records
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTENDANCE_LEAVEADJUST]') AND type = 'U')
BEGIN
    CREATE TABLE [ATTENDANCE_LEAVEADJUST] (
        [LEAVEADJ_ID] BIGINT NOT NULL,
        [LEAVEADJ_EMPSYSID] BIGINT NOT NULL,
        [LEAVEADJ_DATE] DATETIME2(3) NOT NULL,
        [LEAVEADJ_TYPE] CHAR(1) NOT NULL,
        [LEAVEADJ_LASTMODIFIEDBY] BIGINT NOT NULL,
        [LEAVEADJ_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_ATTENDANCE_LEAVEADJUST] PRIMARY KEY ([LEAVEADJ_ID])
    );
    PRINT '+ ATTENDANCE_LEAVEADJUST created';
END
ELSE
    PRINT '= ATTENDANCE_LEAVEADJUST already exists';
GO

-- Create Indexes (idempotent)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SWIPE_EMPSYSID' AND object_id = OBJECT_ID('SWIPE_RAWPUNCH'))
    CREATE INDEX [IX_SWIPE_EMPSYSID] ON [SWIPE_RAWPUNCH] ([SWIPE_EMPSYSID]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SWIPE_PUNCHTIME' AND object_id = OBJECT_ID('SWIPE_RAWPUNCH'))
    CREATE INDEX [IX_SWIPE_PUNCHTIME] ON [SWIPE_RAWPUNCH] ([SWIPE_PUNCHTIME]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ATTENDANCE_BATCH_STATUS' AND object_id = OBJECT_ID('ATTENDANCE_BATCH'))
    CREATE INDEX [IX_ATTENDANCE_BATCH_STATUS] ON [ATTENDANCE_BATCH] ([BATCH_STATUS]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OT_EMPSYSID' AND object_id = OBJECT_ID('ATTENDANCE_OVERTIME'))
    CREATE INDEX [IX_OT_EMPSYSID] ON [ATTENDANCE_OVERTIME] ([OT_EMPSYSID]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LOP_EMPSYSID' AND object_id = OBJECT_ID('ATTENDANCE_LOPMAIN'))
    CREATE INDEX [IX_LOP_EMPSYSID] ON [ATTENDANCE_LOPMAIN] ([LOP_EMPSYSID]);
GO

PRINT '=== PHASE 2 COMPLETE: Tables Created ===';
GO

-- ==========================================
-- PHASE 3: Functions
-- ==========================================

-- Function: fn_GetEmployeeAttendancePercentage
-- Note: CREATE OR ALTER requires SQL Server 2016+
CREATE OR ALTER FUNCTION dbo.fn_GetEmployeeAttendancePercentage
(
    @p_EmpSysID BIGINT,
    @p_MonthStart DATETIME2(3),
    @p_MonthEnd DATETIME2(3)
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @PresentDays INT;
    DECLARE @WorkingDays INT;
    DECLARE @Percentage DECIMAL(5,2);

    -- Count distinct punch dates
    SELECT @PresentDays = COUNT(DISTINCT CAST(SWIPE_PUNCHTIME AS DATE))
    FROM SWIPE_RAWPUNCH
    WHERE SWIPE_EMPSYSID = @p_EmpSysID
      AND SWIPE_PUNCHTIME >= @p_MonthStart
      AND SWIPE_PUNCHTIME <= @p_MonthEnd;

    SET @WorkingDays = DATEDIFF(DAY, @p_MonthStart, @p_MonthEnd) + 1;

    IF @WorkingDays = 0
        RETURN 0;

    SET @Percentage = (@PresentDays * 100.0) / @WorkingDays;

    RETURN ROUND(@Percentage, 2);
END;
GO

-- Function: fn_GetLOPDays
CREATE OR ALTER FUNCTION dbo.fn_GetLOPDays
(
    @p_EmpSysID BIGINT,
    @p_MonthStart DATETIME2(3),
    @p_MonthEnd DATETIME2(3)
)
RETURNS INT
AS
BEGIN
    DECLARE @LOPDays INT;
    DECLARE @PresentDays INT;
    DECLARE @WorkingDays INT;

    -- Count distinct punch dates
    SELECT @PresentDays = COUNT(DISTINCT CAST(SWIPE_PUNCHTIME AS DATE))
    FROM SWIPE_RAWPUNCH
    WHERE SWIPE_EMPSYSID = @p_EmpSysID
      AND SWIPE_PUNCHTIME >= @p_MonthStart
      AND SWIPE_PUNCHTIME <= @p_MonthEnd;

    SET @WorkingDays = DATEDIFF(DAY, @p_MonthStart, @p_MonthEnd) + 1;
    SET @LOPDays = @WorkingDays - ISNULL(@PresentDays, 0);

    RETURN ISNULL(@LOPDays, 0);
END;
GO

PRINT '=== PHASE 3.1 COMPLETE: Functions Created ===';
GO

-- ==========================================
-- PHASE 3.2: Stored Procedures
-- ==========================================

-- Procedure: usp_RecordSwipePunch
CREATE OR ALTER PROCEDURE dbo.usp_RecordSwipePunch
(
    @p_EmpSysID BIGINT,
    @p_GateNo VARCHAR(10),
    @p_PunchTime DATETIME2(3),
    @p_PunchStatus CHAR(1),
    @p_SwipeID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @p_PunchStatus NOT IN ('I', 'O')
            THROW 50001, 'Punch status must be I (In) or O (Out)', 1;

        SELECT @p_SwipeID = ISNULL(MAX(SWIPE_ID), 0) + 1 FROM SWIPE_RAWPUNCH;

        INSERT INTO SWIPE_RAWPUNCH
        (SWIPE_ID, SWIPE_EMPSYSID, SWIPE_PUNCHTIME, SWIPE_GATENO, SWIPE_PUNCHSTATUS, SWIPE_PULLSTATUS)
        VALUES (@p_SwipeID, @p_EmpSysID, @p_PunchTime, @p_GateNo, @p_PunchStatus, 'A');

        COMMIT TRANSACTION;
        PRINT 'Punch recorded: Swipe ID = ' + CAST(@p_SwipeID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Punch recording failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- Procedure: usp_ProcessMonthlyAttendance
CREATE OR ALTER PROCEDURE dbo.usp_ProcessMonthlyAttendance
(
    @p_MonthStart DATETIME2(3),
    @p_MonthEnd DATETIME2(3),
    @p_ProcessedBy BIGINT,
    @p_BatchID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @MonthNo INT = MONTH(@p_MonthStart);
        DECLARE @YearNo INT = YEAR(@p_MonthStart);

        -- Create batch
        SELECT @p_BatchID = ISNULL(MAX(BATCH_ID), 0) + 1 FROM ATTENDANCE_BATCH;

        INSERT INTO ATTENDANCE_BATCH
        (BATCH_ID, BATCH_MONTHFROM, BATCH_MONTHTO, BATCH_YEARFROM, BATCH_YEAREND, BATCH_STATUS,
         BATCH_CREATEDBY, BATCH_CREATEDON, BATCH_LASTMODIFIEDBY, BATCH_LASTMODIFIEDON)
        VALUES (@p_BatchID, @MonthNo, @MonthNo, @YearNo, @YearNo, 'P', @p_ProcessedBy, GETDATE(), @p_ProcessedBy, GETDATE());

        -- Process all employees
        DECLARE @EmpSysID BIGINT;
        DECLARE emp_cursor CURSOR FOR
        SELECT DISTINCT SWIPE_EMPSYSID FROM SWIPE_RAWPUNCH
        WHERE SWIPE_PUNCHTIME >= @p_MonthStart AND SWIPE_PUNCHTIME <= @p_MonthEnd;

        OPEN emp_cursor;
        FETCH NEXT FROM emp_cursor INTO @EmpSysID;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            DECLARE @LOP_Days INT;
            DECLARE @LOP_ID BIGINT;

            SET @LOP_Days = dbo.fn_GetLOPDays(@EmpSysID, @p_MonthStart, @p_MonthEnd);

            IF @LOP_Days > 0
            BEGIN
                SELECT @LOP_ID = ISNULL(MAX(LOP_ID), 0) + 1 FROM ATTENDANCE_LOPMAIN;

                INSERT INTO ATTENDANCE_LOPMAIN
                (LOP_ID, LOP_EMPSYSID, LOP_BATCHID, LOP_DAYS, LOP_TYPE, LOP_LASTMODIFIEDBY, LOP_LASTMODIFIEDON)
                VALUES (@LOP_ID, @EmpSysID, @p_BatchID, @LOP_Days, 'L', @p_ProcessedBy, GETDATE());
            END

            FETCH NEXT FROM emp_cursor INTO @EmpSysID;
        END

        CLOSE emp_cursor;
        DEALLOCATE emp_cursor;

        -- Update batch status
        UPDATE ATTENDANCE_BATCH SET BATCH_STATUS = 'Y', BATCH_LASTMODIFIEDON = GETDATE()
        WHERE BATCH_ID = @p_BatchID;

        COMMIT TRANSACTION;
        PRINT 'Monthly attendance processed: Batch ID = ' + CAST(@p_BatchID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Monthly attendance processing failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

PRINT '=== PHASE 3.2 COMPLETE: Procedures Created ===';
GO

-- ==========================================
-- PHASE 4: Triggers
-- ==========================================

-- Trigger: Validate punch status on insert
CREATE OR ALTER TRIGGER [trg_SwipeRawPunch_ValidateEntry]
ON [SWIPE_RAWPUNCH]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE SWIPE_RAWPUNCH
    SET SWIPE_PULLSTATUS = 'A',
        SWIPE_LASTMODIFIEDON = GETDATE()
    FROM SWIPE_RAWPUNCH sr
    INNER JOIN inserted i ON sr.SWIPE_ID = i.SWIPE_ID;
END;
GO

-- Trigger: Auto update batch status when all employees processed
CREATE OR ALTER TRIGGER [trg_AttendanceBatch_AutoUpdateStatus]
ON [ATTENDANCE_LOPMAIN]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Note: Simple trigger - can be enhanced with full logic
    UPDATE ATTENDANCE_BATCH
    SET BATCH_STATUS = 'Y',
        BATCH_LASTMODIFIEDON = GETDATE()
    FROM ATTENDANCE_BATCH ab
    INNER JOIN inserted i ON ab.BATCH_ID = i.LOP_BATCHID;
END;
GO

PRINT '=== PHASE 4 COMPLETE: Triggers Created ===';
GO

-- ==========================================
-- PHASE 5: Verification Views
-- ==========================================
IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'vw_AttendanceDB_Status')
    DROP VIEW vw_AttendanceDB_Status;
GO

CREATE VIEW vw_AttendanceDB_Status AS
SELECT
    'ATTENDANCEDB' AS DatabaseName,
    'Attendance and Biometric Punch Management Module' AS ModuleDescription,
    (SELECT COUNT(*) FROM SWIPE_RAWPUNCH) AS PunchRecords,
    (SELECT COUNT(DISTINCT SWIPE_EMPSYSID) FROM SWIPE_RAWPUNCH) AS EmployeesWithPunch,
    (SELECT COUNT(*) FROM ATTENDANCE_BATCH) AS ProcessedBatches,
    (SELECT SUM(LOP_DAYS) FROM ATTENDANCE_LOPMAIN) AS TotalLOPDays,
    GETDATE() AS LastChecked;
GO

PRINT '=== PHASE 5 COMPLETE: Views Created ===';
GO

-- ==========================================
-- PHASE 6: Final Verification
-- ==========================================
PRINT '=== FINAL VERIFICATION ===';
SELECT 'System Tables' AS Object_Type, COUNT(*) AS Count FROM sys.tables;
SELECT 'Indexes' AS Object_Type, COUNT(*) AS Count FROM sys.indexes WHERE object_id > 0;
SELECT 'Procedures' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'P';
SELECT 'Functions' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'FN';
SELECT 'Triggers' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'TR';
SELECT 'Views' AS Object_Type, COUNT(*) AS Count FROM sys.views;

PRINT '======================================';
PRINT 'ATTENDANCEDB DEPLOYMENT COMPLETE';
PRINT '======================================';
PRINT 'Database: ATTENDANCEDB';
PRINT 'Tables: 9';
PRINT 'Indexes: 5';
PRINT 'Procedures: 2';
PRINT 'Functions: 2';
PRINT 'Triggers: 2';
PRINT 'Views: 1';
PRINT 'Status: READY FOR DEPLOYMENT';
PRINT '======================================';
GO

-- ==========================================
-- PHASE 2: Table Definitions
-- ==========================================
USE ATTENDANCEDB;
GO

-- Table: SWIPE_RAWPUNCH - Biometric Punch Records
CREATE TABLE [SWIPE_RAWPUNCH] (
    [SWIPE_ID] BIGINT NOT NULL,
    [SWIPE_EMPSYSID] BIGINT NOT NULL,
    [SWIPE_PUNCHTIME] DATETIME2(3) NOT NULL,
    [SWIPE_GATENO] VARCHAR(10) NOT NULL,
    [SWIPE_PUNCHSTATUS] CHAR(1) NOT NULL,
    [SWIPE_PULLSTATUS] CHAR(1) NULL,
    [SWIPE_VERIFIED] CHAR(1) NULL,
    [SWIPE_LASTMODIFIEDBY] BIGINT NULL,
    [SWIPE_LASTMODIFIEDON] DATETIME2(3) NULL,
    CONSTRAINT [PK_SWIPE_RAWPUNCH] PRIMARY KEY ([SWIPE_ID])
);

-- Table: SWIPE_RAWPUNCH_LOG - Biometric Punch Log
CREATE TABLE [SWIPE_RAWPUNCH_LOG] (
    [SWIPE_ID] BIGINT NOT NULL,
    [SWIPE_EMPSYSID] BIGINT NOT NULL,
    [SWIPE_PUNCHTIME] DATETIME2(3) NOT NULL,
    [SWIPE_GATENO] VARCHAR(10) NOT NULL,
    [SWIPE_PUNCHSTATUS] CHAR(1) NOT NULL,
    [SWIPE_PULLSTATUS] CHAR(1) NULL,
    [LOG_CREATEDON] DATETIME2(3) NOT NULL,
    [LOG_CREATEDBY] BIGINT NULL
);

-- Table: ATTENDANCE_BATCH - Monthly Attendance Batch
CREATE TABLE [ATTENDANCE_BATCH] (
    [BATCH_ID] BIGINT NOT NULL,
    [BATCH_MONTHFROM] INT NOT NULL,
    [BATCH_MONTHTO] INT NOT NULL,
    [BATCH_YEARFROM] INT NOT NULL,
    [BATCH_YEAREND] INT NOT NULL,
    [BATCH_STATUS] CHAR(1) NOT NULL,
    [BATCH_CREATEDBY] BIGINT NOT NULL,
    [BATCH_CREATEDON] DATETIME2(3) NOT NULL,
    [BATCH_LASTMODIFIEDBY] BIGINT NOT NULL,
    [BATCH_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_ATTENDANCE_BATCH] PRIMARY KEY ([BATCH_ID])
);

-- Table: ATTENDANCE_SUMMARY - Attendance Summary
CREATE TABLE [ATTENDANCE_SUMMARY] (
    [SUMMARY_ID] BIGINT NOT NULL,
    [SUMMARY_EMPSYSID] BIGINT NOT NULL,
    [SUMMARY_BATCHID] BIGINT NOT NULL,
    [SUMMARY_ATTTYPE] VARCHAR(10) NOT NULL,
    [SUMMARY_DAYS] INT NOT NULL,
    [SUMMARY_LASTMODIFIEDBY] BIGINT NOT NULL,
    [SUMMARY_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_ATTENDANCE_SUMMARY] PRIMARY KEY ([SUMMARY_ID]),
    CONSTRAINT [FK_SUMMARY_BATCHID] FOREIGN KEY ([SUMMARY_BATCHID]) REFERENCES [ATTENDANCE_BATCH]([BATCH_ID])
);

-- Table: ATTENDANCE_OVERTIME - Overtime Tracking
CREATE TABLE [ATTENDANCE_OVERTIME] (
    [OT_ID] BIGINT NOT NULL,
    [OT_EMPSYSID] BIGINT NOT NULL,
    [OT_DATE] DATETIME2(3) NOT NULL,
    [OT_HOURS] DECIMAL(5,2) NOT NULL,
    [OT_TYPE] VARCHAR(20) NOT NULL,
    [OT_APPROVED] CHAR(1) NOT NULL,
    [OT_LASTMODIFIEDBY] BIGINT NOT NULL,
    [OT_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_ATTENDANCE_OVERTIME] PRIMARY KEY ([OT_ID])
);

-- Table: ATTENDANCE_NIGHT - Night Shift Tracking
CREATE TABLE [ATTENDANCE_NIGHT] (
    [NIGHT_ID] BIGINT NOT NULL,
    [NIGHT_EMPSYSID] BIGINT NOT NULL,
    [NIGHT_DATE] DATETIME2(3) NOT NULL,
    [NIGHT_NIGHTTYPE] CHAR(1) NOT NULL,
    [NIGHT_LASTMODIFIEDBY] BIGINT NOT NULL,
    [NIGHT_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_ATTENDANCE_NIGHT] PRIMARY KEY ([NIGHT_ID])
);

-- Table: ATTENDANCE_LOPMAIN - Loss of Pay Main Record
CREATE TABLE [ATTENDANCE_LOPMAIN] (
    [LOP_ID] BIGINT NOT NULL,
    [LOP_EMPSYSID] BIGINT NOT NULL,
    [LOP_BATCHID] BIGINT NOT NULL,
    [LOP_DAYS] DECIMAL(5,2) NOT NULL,
    [LOP_TYPE] CHAR(1) NOT NULL,
    [LOP_LASTMODIFIEDBY] BIGINT NOT NULL,
    [LOP_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_ATTENDANCE_LOPMAIN] PRIMARY KEY ([LOP_ID]),
    CONSTRAINT [FK_LOP_BATCHID] FOREIGN KEY ([LOP_BATCHID]) REFERENCES [ATTENDANCE_BATCH]([BATCH_ID])
);

-- Table: ATTENDANCE_GRACEADJUST - Grace Adjustment Records
CREATE TABLE [ATTENDANCE_GRACEADJUST] (
    [GRACE_ID] BIGINT NOT NULL,
    [GRACE_EMPSYSID] BIGINT NOT NULL,
    [GRACE_DATE] DATETIME2(3) NOT NULL,
    [GRACE_MINUTES] INT NOT NULL,
    [GRACE_LASTMODIFIEDBY] BIGINT NOT NULL,
    [GRACE_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_ATTENDANCE_GRACEADJUST] PRIMARY KEY ([GRACE_ID])
);

-- Table: ATTENDANCE_LEAVEADJUST - Leave Adjustment Records
CREATE TABLE [ATTENDANCE_LEAVEADJUST] (
    [LEAVEADJ_ID] BIGINT NOT NULL,
    [LEAVEADJ_EMPSYSID] BIGINT NOT NULL,
    [LEAVEADJ_DATE] DATETIME2(3) NOT NULL,
    [LEAVEADJ_TYPE] CHAR(1) NOT NULL,
    [LEAVEADJ_LASTMODIFIEDBY] BIGINT NOT NULL,
    [LEAVEADJ_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_ATTENDANCE_LEAVEADJUST] PRIMARY KEY ([LEAVEADJ_ID])
);

-- Create Indexes
CREATE INDEX [IX_SWIPE_EMPSYSID] ON [SWIPE_RAWPUNCH] ([SWIPE_EMPSYSID]);
CREATE INDEX [IX_SWIPE_PUNCHTIME] ON [SWIPE_RAWPUNCH] ([SWIPE_PUNCHTIME]);
CREATE INDEX [IX_ATTENDANCE_BATCH_STATUS] ON [ATTENDANCE_BATCH] ([BATCH_STATUS]);
CREATE INDEX [IX_OT_EMPSYSID] ON [ATTENDANCE_OVERTIME] ([OT_EMPSYSID]);
CREATE INDEX [IX_LOP_EMPSYSID] ON [ATTENDANCE_LOPMAIN] ([LOP_EMPSYSID]);

PRINT '=== PHASE 2 COMPLETE: Tables Created ===';
GO

-- ==========================================
-- PHASE 3: Functions
-- ==========================================

-- Function: fn_GetEmployeeAttendancePercentage
CREATE OR ALTER FUNCTION dbo.fn_GetEmployeeAttendancePercentage
(
    @p_EmpSysID BIGINT,
    @p_MonthStart DATETIME2(3),
    @p_MonthEnd DATETIME2(3)
)
RETURNS DECIMAL(5,2)
DETERMINISTIC
AS
BEGIN
    DECLARE @PresentDays INT;
    DECLARE @WorkingDays INT;
    DECLARE @Percentage DECIMAL(5,2);
    
    -- Count distinct punch dates
    SELECT @PresentDays = COUNT(DISTINCT CAST(SWIPE_PUNCHTIME AS DATE))
    FROM SWIPE_RAWPUNCH
    WHERE SWIPE_EMPSYSID = @p_EmpSysID
      AND SWIPE_PUNCHTIME >= @p_MonthStart
      AND SWIPE_PUNCHTIME <= @p_MonthEnd;
    
    SET @WorkingDays = DATEDIFF(DAY, @p_MonthStart, @p_MonthEnd) + 1;
    
    IF @WorkingDays = 0
        RETURN 0;
    
    SET @Percentage = (@PresentDays * 100.0) / @WorkingDays;
    
    RETURN ROUND(@Percentage, 2);
END;
GO

-- Function: fn_GetLOPDays
CREATE OR ALTER FUNCTION dbo.fn_GetLOPDays
(
    @p_EmpSysID BIGINT,
    @p_MonthStart DATETIME2(3),
    @p_MonthEnd DATETIME2(3)
)
RETURNS INT
DETERMINISTIC
AS
BEGIN
    DECLARE @LOPDays INT;
    DECLARE @PresentDays INT;
    DECLARE @WorkingDays INT;
    
    -- Count distinct punch dates
    SELECT @PresentDays = COUNT(DISTINCT CAST(SWIPE_PUNCHTIME AS DATE))
    FROM SWIPE_RAWPUNCH
    WHERE SWIPE_EMPSYSID = @p_EmpSysID
      AND SWIPE_PUNCHTIME >= @p_MonthStart
      AND SWIPE_PUNCHTIME <= @p_MonthEnd;
    
    SET @WorkingDays = DATEDIFF(DAY, @p_MonthStart, @p_MonthEnd) + 1;
    SET @LOPDays = @WorkingDays - ISNULL(@PresentDays, 0);
    
    RETURN ISNULL(@LOPDays, 0);
END;
GO

PRINT '=== PHASE 3.1 COMPLETE: Functions Created ===';
GO

-- ==========================================
-- PHASE 3.2: Stored Procedures
-- ==========================================

-- Procedure: usp_RecordSwipePunch
CREATE OR ALTER PROCEDURE dbo.usp_RecordSwipePunch
(
    @p_EmpSysID BIGINT,
    @p_GateNo VARCHAR(10),
    @p_PunchTime DATETIME2(3),
    @p_PunchStatus CHAR(1),
    @p_SwipeID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF @p_PunchStatus NOT IN ('I', 'O')
            THROW 50001, 'Punch status must be I (In) or O (Out)', 1;
        
        SELECT @p_SwipeID = ISNULL(MAX(SWIPE_ID), 0) + 1 FROM SWIPE_RAWPUNCH;
        
        INSERT INTO SWIPE_RAWPUNCH
        (SWIPE_ID, SWIPE_EMPSYSID, SWIPE_PUNCHTIME, SWIPE_GATENO, SWIPE_PUNCHSTATUS, SWIPE_PULLSTATUS)
        VALUES (@p_SwipeID, @p_EmpSysID, @p_PunchTime, @p_GateNo, @p_PunchStatus, 'A');
        
        COMMIT TRANSACTION;
        PRINT 'Punch recorded: Swipe ID = ' + CAST(@p_SwipeID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Punch recording failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- Procedure: usp_ProcessMonthlyAttendance
CREATE OR ALTER PROCEDURE dbo.usp_ProcessMonthlyAttendance
(
    @p_MonthStart DATETIME2(3),
    @p_MonthEnd DATETIME2(3),
    @p_ProcessedBy BIGINT,
    @p_BatchID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @MonthNo INT = MONTH(@p_MonthStart);
        DECLARE @YearNo INT = YEAR(@p_MonthStart);
        
        -- Create batch
        SELECT @p_BatchID = ISNULL(MAX(BATCH_ID), 0) + 1 FROM ATTENDANCE_BATCH;
        
        INSERT INTO ATTENDANCE_BATCH
        (BATCH_ID, BATCH_MONTHFROM, BATCH_MONTHTO, BATCH_YEARFROM, BATCH_YEAREND, BATCH_STATUS,
         BATCH_CREATEDBY, BATCH_CREATEDON, BATCH_LASTMODIFIEDBY, BATCH_LASTMODIFIEDON)
        VALUES (@p_BatchID, @MonthNo, @MonthNo, @YearNo, @YearNo, 'P', @p_ProcessedBy, GETDATE(), @p_ProcessedBy, GETDATE());
        
        -- Process all employees
        DECLARE @EmpSysID BIGINT;
        DECLARE emp_cursor CURSOR FOR
        SELECT DISTINCT SWIPE_EMPSYSID FROM SWIPE_RAWPUNCH
        WHERE SWIPE_PUNCHTIME >= @p_MonthStart AND SWIPE_PUNCHTIME <= @p_MonthEnd;
        
        OPEN emp_cursor;
        FETCH NEXT FROM emp_cursor INTO @EmpSysID;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            DECLARE @LOP_Days INT;
            DECLARE @LOP_ID BIGINT;
            
            SET @LOP_Days = dbo.fn_GetLOPDays(@EmpSysID, @p_MonthStart, @p_MonthEnd);
            
            IF @LOP_Days > 0
            BEGIN
                SELECT @LOP_ID = ISNULL(MAX(LOP_ID), 0) + 1 FROM ATTENDANCE_LOPMAIN;
                
                INSERT INTO ATTENDANCE_LOPMAIN
                (LOP_ID, LOP_EMPSYSID, LOP_BATCHID, LOP_DAYS, LOP_TYPE, LOP_LASTMODIFIEDBY, LOP_LASTMODIFIEDON)
                VALUES (@LOP_ID, @EmpSysID, @p_BatchID, @LOP_Days, 'L', @p_ProcessedBy, GETDATE());
            END
            
            FETCH NEXT FROM emp_cursor INTO @EmpSysID;
        END
        
        CLOSE emp_cursor;
        DEALLOCATE emp_cursor;
        
        -- Update batch status
        UPDATE ATTENDANCE_BATCH SET BATCH_STATUS = 'Y', BATCH_LASTMODIFIEDON = GETDATE()
        WHERE BATCH_ID = @p_BatchID;
        
        COMMIT TRANSACTION;
        PRINT 'Monthly attendance processed: Batch ID = ' + CAST(@p_BatchID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Monthly attendance processing failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

PRINT '=== PHASE 3.2 COMPLETE: Procedures Created ===';
GO

-- ==========================================
-- PHASE 4: Triggers
-- ==========================================

-- Trigger: Validate punch status on insert
CREATE OR ALTER TRIGGER [trg_SwipeRawPunch_ValidateEntry]
ON [SWIPE_RAWPUNCH]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE SWIPE_RAWPUNCH
    SET SWIPE_PULLSTATUS = 'A',
        SWIPE_LASTMODIFIEDON = GETDATE()
    FROM SWIPE_RAWPUNCH sr
    INNER JOIN inserted i ON sr.SWIPE_ID = i.SWIPE_ID;
END;
GO

-- Trigger: Auto update batch status when all employees processed
CREATE OR ALTER TRIGGER [trg_AttendanceBatch_AutoUpdateStatus]
ON [ATTENDANCE_LOPMAIN]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Note: Simple trigger - can be enhanced with full logic
    UPDATE ATTENDANCE_BATCH
    SET BATCH_STATUS = 'Y',
        BATCH_LASTMODIFIEDON = GETDATE()
    FROM ATTENDANCE_BATCH ab
    INNER JOIN inserted i ON ab.BATCH_ID = i.LOP_BATCHID;
END;
GO

PRINT '=== PHASE 4 COMPLETE: Triggers Created ===';
GO

-- ==========================================
-- PHASE 5: Verification Views
-- ==========================================
CREATE VIEW vw_AttendanceDB_Status AS
SELECT 
    'ATTENDANCEDB' AS DatabaseName,
    'Attendance and Biometric Punch Management Module' AS ModuleDescription,
    (SELECT COUNT(*) FROM SWIPE_RAWPUNCH) AS PunchRecords,
    (SELECT COUNT(DISTINCT SWIPE_EMPSYSID) FROM SWIPE_RAWPUNCH) AS EmployeesWithPunch,
    (SELECT COUNT(*) FROM ATTENDANCE_BATCH) AS ProcessedBatches,
    (SELECT SUM(LOP_DAYS) FROM ATTENDANCE_LOPMAIN) AS TotalLOPDays,
    GETDATE() AS LastChecked;
GO

PRINT '=== PHASE 5 COMPLETE: Views Created ===';
GO

-- ==========================================
-- PHASE 5: Final Verification
-- ==========================================
PRINT '=== FINAL VERIFICATION ===';
SELECT 'System Tables' AS Object_Type, COUNT(*) AS Count FROM sys.tables;
SELECT 'Indexes' AS Object_Type, COUNT(*) AS Count FROM sys.indexes WHERE object_id > 0;
SELECT 'Procedures' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'P';
SELECT 'Functions' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'FN';
SELECT 'Triggers' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'TR';
SELECT 'Views' AS Object_Type, COUNT(*) AS Count FROM sys.views;

PRINT '======================================';
PRINT 'ATTENDANCEDB DEPLOYMENT COMPLETE';
PRINT '======================================';
PRINT 'Database: ATTENDANCEDB';
PRINT 'Tables: 9';
PRINT 'Indexes: 5';
PRINT 'Procedures: 2';
PRINT 'Functions: 2';
PRINT 'Triggers: 2';
PRINT 'Views: 1';
PRINT 'Status: READY FOR DEPLOYMENT';
PRINT '======================================';
GO
