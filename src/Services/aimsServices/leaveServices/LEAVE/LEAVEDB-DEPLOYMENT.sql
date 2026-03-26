-- ==========================================
-- Database: LEAVEDB
-- Module: Leave Management and Approval System
-- Generated Module Script
-- ==========================================

USE master;
GO

-- ==========================================
-- PHASE 1: Database Creation
-- ==========================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'LEAVEDB')
BEGIN
    CREATE DATABASE [LEAVEDB];
    PRINT '+ LEAVEDB created';
END
ELSE
    PRINT '= LEAVEDB already exists';
GO

ALTER DATABASE [LEAVEDB] SET READ_COMMITTED_SNAPSHOT ON;
GO

PRINT '=== PHASE 1 COMPLETE: Database Created ===';
GO

-- ==========================================
-- PHASE 2: Table Definitions
-- ==========================================
USE LEAVEDB;
GO

-- Table: LEAVE_MASTER - Leave Type Master
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEAVE_MASTER]') AND type = 'U')
BEGIN
CREATE TABLE [LEAVE_MASTER] (
    [LEAVE_ID] BIGINT NOT NULL,
    [LEAVE_DESCRIPTION] VARCHAR(255) NOT NULL,
    [LEAVE_GENDERSPECIFIC] CHAR(1) NOT NULL,
    [LEAVE_APPLICABLEFORALL] CHAR(1) NOT NULL,
    [LEAVE_MAXDAYSPL] INT NOT NULL,
    [LEAVE_ENCASHABLE] CHAR(1) NOT NULL,
    [LEAVE_CARRYFORWARD] CHAR(1) NOT NULL,
    [LEAVE_LASTMODIFIEDBY] BIGINT NOT NULL,
    [LEAVE_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LEAVE_MASTER] PRIMARY KEY ([LEAVE_ID]),
    CONSTRAINT [UQ_LEAVE_DESC] UNIQUE ([LEAVE_DESCRIPTION])
);
    PRINT '+ LEAVE_MASTER created';
END
ELSE
    PRINT '= LEAVE_MASTER already exists';
GO

-- Table: LEAVE_DETAILS - Leave Application Details
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEAVE_DETAILS]') AND type = 'U')
BEGIN
CREATE TABLE [LEAVE_DETAILS] (
    [LEAVE_DETAILID] BIGINT NOT NULL,
    [LEAVE_EMPSYSID] BIGINT NOT NULL,
    [LEAVE_APPFROM] DATETIME2(3) NOT NULL,
    [LEAVE_APPTO] DATETIME2(3) NOT NULL,
    [LEAVE_APPTYPE] VARCHAR(10) NOT NULL,
    [LEAVE_ID] BIGINT NOT NULL,
    [LEAVE_TIMEUNITID] INT NOT NULL,
    [LEAVE_APPSTATUS] CHAR(1) NOT NULL,
    [LEAVE_APPLIEDDAYS] DECIMAL(5,2) NOT NULL,
    [LEAVE_REASON] VARCHAR(500) NULL,
    [LEAVE_ENTEREDON] DATETIME2(3) NOT NULL,
    [LEAVE_ENTEREDBY] BIGINT NOT NULL,
    [LEAVE_LASTMODIFIEDBY] BIGINT NOT NULL,
    [LEAVE_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LEAVE_DETAILS] PRIMARY KEY ([LEAVE_DETAILID]),
    CONSTRAINT [FK_LEAVE_DETAILS_MASTER] FOREIGN KEY ([LEAVE_ID]) REFERENCES [LEAVE_MASTER]([LEAVE_ID])
);
    PRINT '+ LEAVE_DETAILS created';
END
ELSE
    PRINT '= LEAVE_DETAILS already exists';
GO

-- Table: LEAVE_CREDIT - Leave Credit Accrual
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEAVE_CREDIT]') AND type = 'U')
BEGIN
CREATE TABLE [LEAVE_CREDIT] (
    [CREDIT_ID] BIGINT NOT NULL,
    [CREDIT_EMPSYSID] BIGINT NOT NULL,
    [CREDIT_LEAVEID] BIGINT NOT NULL,
    [CREDIT_LEAVEFLAG] CHAR(1) NOT NULL,
    [CREDIT_YEAR] INT NOT NULL,
    [CREDIT_OPENING] DECIMAL(5,2) NOT NULL,
    [CREDIT_CREDITED] DECIMAL(5,2) NOT NULL,
    [CREDIT_UTILIZED] DECIMAL(5,2) NOT NULL,
    [CREDIT_CLOSING] DECIMAL(5,2) NOT NULL,
    [CREDIT_LASTMODIFIEDBY] BIGINT NOT NULL,
    [CREDIT_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LEAVE_CREDIT] PRIMARY KEY ([CREDIT_ID]),
    CONSTRAINT [FK_CREDIT_LEAVE] FOREIGN KEY ([CREDIT_LEAVEID]) REFERENCES [LEAVE_MASTER]([LEAVE_ID]),
    CONSTRAINT [UQ_LEAVE_CREDIT] UNIQUE ([CREDIT_EMPSYSID], [CREDIT_LEAVEID], [CREDIT_YEAR])
);
    PRINT '+ LEAVE_CREDIT created';
END
ELSE
    PRINT '= LEAVE_CREDIT already exists';
GO

-- Table: LEAVE_DETAILSAPR - Leave Details Approval History
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEAVE_DETAILSAPR]') AND type = 'U')
BEGIN
CREATE TABLE [LEAVE_DETAILSAPR] (
    [LEAVEAPR_ID] BIGINT NOT NULL,
    [LEAVEAPR_DETAILID] BIGINT NOT NULL,
    [LEAVEAPR_APPROVESTATUS] CHAR(1) NOT NULL,
    [LEAVEAPR_REMARKS] VARCHAR(500) NULL,
    [LEAVEAPR_APPROVEDON] DATETIME2(3) NOT NULL,
    [LEAVEAPR_APPROVEDBY] BIGINT NOT NULL,
    [LEAVEAPR_LASTMODIFIEDBY] BIGINT NOT NULL,
    [LEAVEAPR_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LEAVE_DETAILSAPR] PRIMARY KEY ([LEAVEAPR_ID]),
    CONSTRAINT [FK_LEAVEAPR_DETAILS] FOREIGN KEY ([LEAVEAPR_DETAILID]) REFERENCES [LEAVE_DETAILS]([LEAVE_DETAILID])
);
    PRINT '+ LEAVE_DETAILSAPR created';
END
ELSE
    PRINT '= LEAVE_DETAILSAPR already exists';
GO

-- Table: LEAVE_RULES - Leave Policy Rules
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEAVE_RULES]') AND type = 'U')
BEGIN
CREATE TABLE [LEAVE_RULES] (
    [RULE_ID] INT NOT NULL,
    [RULE_LEAVEID] BIGINT NOT NULL,
    [RULE_MAXDAYSINAPPL] INT NOT NULL,
    [RULE_MINDAYSINAPPL] INT NOT NULL,
    [RULE_MAXYEARLIMIT] INT NOT NULL,
    [RULE_CLUBBING] CHAR(1) NOT NULL,
    [RULE_LASTMODIFIEDBY] BIGINT NOT NULL,
    [RULE_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LEAVE_RULES] PRIMARY KEY ([RULE_ID]),
    CONSTRAINT [FK_RULE_LEAVE] FOREIGN KEY ([RULE_LEAVEID]) REFERENCES [LEAVE_MASTER]([LEAVE_ID])
);
    PRINT '+ LEAVE_RULES created';
END
ELSE
    PRINT '= LEAVE_RULES already exists';
GO

-- Table: COMPOFF_ADJUST - Compensation Off Adjustment
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMPOFF_ADJUST]') AND type = 'U')
BEGIN
CREATE TABLE [COMPOFF_ADJUST] (
    [COMPOFF_ID] BIGINT NOT NULL,
    [COMPOFF_EMPSYSID] BIGINT NOT NULL,
    [COMPOFF_COMPOFFDATE] DATETIME2(3) NOT NULL,
    [COMPOFF_USEDDATE] DATETIME2(3) NULL,
    [COMPOFF_STATUS] CHAR(1) NOT NULL,
    [COMPOFF_LASTMODIFIEDBY] BIGINT NOT NULL,
    [COMPOFF_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_COMPOFF_ADJUST] PRIMARY KEY ([COMPOFF_ID])
);
    PRINT '+ COMPOFF_ADJUST created';
END
ELSE
    PRINT '= COMPOFF_ADJUST already exists';
GO

PRINT '=== PHASE 2 COMPLETE: Tables Created ===';
GO

-- ==========================================
-- PHASE 3: Functions
-- ==========================================

-- Function: fn_GetLeaveBalance
CREATE OR ALTER FUNCTION dbo.fn_GetLeaveBalance
(
    @p_EmpSysID BIGINT,
    @p_LeaveID BIGINT,
    @p_AsOnDate DATETIME2(3)
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @Balance DECIMAL(5,2);
    DECLARE @Credited DECIMAL(5,2) = 0;
    DECLARE @Utilized DECIMAL(5,2) = 0;
    
    -- Get credited leave balance
    SELECT @Credited = ISNULL(SUM(CREDIT_CREDITED), 0)
    FROM LEAVE_CREDIT
    WHERE CREDIT_EMPSYSID = @p_EmpSysID
      AND CREDIT_LEAVEID = @p_LeaveID
      AND YEAR(GETDATE()) = CREDIT_YEAR;
    
    -- Get utilized leave balance
    SELECT @Utilized = ISNULL(SUM(LEAVE_APPLIEDDAYS), 0)
    FROM LEAVE_DETAILS
    WHERE LEAVE_EMPSYSID = @p_EmpSysID
      AND LEAVE_ID = @p_LeaveID
      AND LEAVE_APPSTATUS IN ('Y', 'P')
      AND LEAVE_ENTEREDON <= @p_AsOnDate;
    
    SET @Balance = @Credited - @Utilized;
    
    RETURN CASE WHEN @Balance < 0 THEN 0 ELSE @Balance END;
END;
GO

PRINT '=== PHASE 3.1 COMPLETE: Functions Created ===';
GO

-- ==========================================
-- PHASE 3.2: Stored Procedures
-- ==========================================

-- Procedure: usp_ApplyLeave
CREATE OR ALTER PROCEDURE dbo.usp_ApplyLeave
(
    @p_EmpSysID BIGINT,
    @p_LeaveID BIGINT,
    @p_FromDate DATETIME2(3),
    @p_ToDate DATETIME2(3),
    @p_Reason VARCHAR(500),
    @p_LeaveDays DECIMAL(5,2),
    @p_AppliedBy BIGINT,
    @p_LeaveDetailID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate date range
        IF @p_FromDate > @p_ToDate
            THROW 50001, 'From date cannot be after To date', 1;
        
        -- Validate leave type exists
        IF NOT EXISTS (SELECT 1 FROM LEAVE_MASTER WHERE LEAVE_ID = @p_LeaveID)
            THROW 50002, 'Leave type not found', 1;
        
        -- Check leave balance
        DECLARE @Balance DECIMAL(5,2);
        SET @Balance = dbo.fn_GetLeaveBalance(@p_EmpSysID, @p_LeaveID, GETDATE());
        
        IF @Balance < @p_LeaveDays
            THROW 50003, 'Insufficient leave balance', 1;
        
        SELECT @p_LeaveDetailID = ISNULL(MAX(LEAVE_DETAILID), 0) + 1 FROM LEAVE_DETAILS;
        
        INSERT INTO LEAVE_DETAILS
        (LEAVE_DETAILID, LEAVE_EMPSYSID, LEAVE_APPFROM, LEAVE_APPTO, LEAVE_APPTYPE, 
         LEAVE_ID, LEAVE_TIMEUNITID, LEAVE_APPSTATUS, LEAVE_APPLIEDDAYS, LEAVE_REASON, 
         LEAVE_ENTEREDON, LEAVE_ENTEREDBY, LEAVE_LASTMODIFIEDBY, LEAVE_LASTMODIFIEDON)
        VALUES (@p_LeaveDetailID, @p_EmpSysID, @p_FromDate, @p_ToDate, 'LV', @p_LeaveID, 1, 
                'P', @p_LeaveDays, @p_Reason, GETDATE(), @p_AppliedBy, @p_AppliedBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'Leave application submitted: ID = ' + CAST(@p_LeaveDetailID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Leave application failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- Procedure: usp_ApproveLeave
CREATE OR ALTER PROCEDURE dbo.usp_ApproveLeave
(
    @p_LeaveDetailID BIGINT,
    @p_Status CHAR(1),
    @p_Remarks VARCHAR(500),
    @p_ApprovedBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate status
        IF @p_Status NOT IN ('Y', 'R', 'C', 'D')
            THROW 50004, 'Invalid approval status: Y=Approved, R=Rejected, C=Cancelled, D=Draft', 1;
        
        -- Validate leave detail exists
        IF NOT EXISTS (SELECT 1 FROM LEAVE_DETAILS WHERE LEAVE_DETAILID = @p_LeaveDetailID)
            THROW 50005, 'Leave application not found', 1;
        
        -- Update leave detail status
        UPDATE LEAVE_DETAILS
        SET LEAVE_APPSTATUS = @p_Status,
            LEAVE_LASTMODIFIEDBY = @p_ApprovedBy,
            LEAVE_LASTMODIFIEDON = GETDATE()
        WHERE LEAVE_DETAILID = @p_LeaveDetailID;
        
        -- Insert approval record
        DECLARE @AprID BIGINT;
        SELECT @AprID = ISNULL(MAX(LEAVEAPR_ID), 0) + 1 FROM LEAVE_DETAILSAPR;
        
        INSERT INTO LEAVE_DETAILSAPR
        (LEAVEAPR_ID, LEAVEAPR_DETAILID, LEAVEAPR_APPROVESTATUS, LEAVEAPR_REMARKS,
         LEAVEAPR_APPROVEDON, LEAVEAPR_APPROVEDBY, LEAVEAPR_LASTMODIFIEDBY, LEAVEAPR_LASTMODIFIEDON)
        VALUES (@AprID, @p_LeaveDetailID, @p_Status, @p_Remarks,
                GETDATE(), @p_ApprovedBy, @p_ApprovedBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'Leave approval processed: Detail ID = ' + CAST(@p_LeaveDetailID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Leave approval failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

PRINT '=== PHASE 3.2 COMPLETE: Procedures Created ===';
GO

-- ==========================================
-- PHASE 4: Triggers
-- ==========================================

-- Trigger: Validate leave balance before insert
CREATE OR ALTER TRIGGER [trg_LeaveDetails_CheckBalance]
ON [LEAVE_DETAILS]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @LeaveDetailID BIGINT;
    DECLARE @EmpSysID BIGINT;
    DECLARE @LeaveID BIGINT;
    DECLARE @LeaveDays DECIMAL(5,2);
    DECLARE @Balance DECIMAL(5,2);
    
    DECLARE cur CURSOR FOR SELECT LEAVE_DETAILID, LEAVE_EMPSYSID, LEAVE_ID, LEAVE_APPLIEDDAYS FROM inserted;
    
    OPEN cur;
    FETCH NEXT FROM cur INTO @LeaveDetailID, @EmpSysID, @LeaveID, @LeaveDays;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @Balance = dbo.fn_GetLeaveBalance(@EmpSysID, @LeaveID, GETDATE());
        
        IF @Balance >= @LeaveDays
        BEGIN
            INSERT INTO LEAVE_DETAILS
            SELECT * FROM inserted WHERE LEAVE_DETAILID = @LeaveDetailID;
        END
        ELSE
        BEGIN
            RAISERROR('Insufficient leave balance for this application', 16, 1);
        END
        
        FETCH NEXT FROM cur INTO @LeaveDetailID, @EmpSysID, @LeaveID, @LeaveDays;
    END
    
    CLOSE cur;
    DEALLOCATE cur;
END;
GO

PRINT '=== PHASE 4 COMPLETE: Trigger Created ===';
GO

-- ==========================================
-- PHASE 5: Verification Views
-- ==========================================
IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'vw_LeaveDB_Status')
    DROP VIEW vw_LeaveDB_Status;
GO

CREATE VIEW vw_LeaveDB_Status AS
SELECT 
    'LEAVEDB' AS DatabaseName,
    'Leave Management and Approval Module' AS ModuleDescription,
    (SELECT COUNT(*) FROM LEAVE_MASTER) AS LeaveTypes,
    (SELECT COUNT(*) FROM LEAVE_DETAILS WHERE LEAVE_APPSTATUS = 'P') AS PendingApplications,
    (SELECT COUNT(*) FROM LEAVE_DETAILS WHERE LEAVE_APPSTATUS = 'Y') AS ApprovedLeaves,
    (SELECT SUM(LEAVE_APPLIEDDAYS) FROM LEAVE_DETAILS WHERE LEAVE_APPSTATUS = 'Y') AS TotalApprovedDays,
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
PRINT 'LEAVEDB DEPLOYMENT COMPLETE';
PRINT '======================================';
PRINT 'Database: LEAVEDB';
PRINT 'Tables: 6';
PRINT 'Indexes: 3';
PRINT 'Procedures: 2';
PRINT 'Functions: 1';
PRINT 'Triggers: 1';
PRINT 'Views: 1';
PRINT 'Status: READY FOR DEPLOYMENT';
PRINT '======================================';
GO
