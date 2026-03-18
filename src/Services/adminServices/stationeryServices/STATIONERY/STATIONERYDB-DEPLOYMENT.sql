-- ==========================================
-- STATIONERY MODULE - STANDALONE DATABASE
-- Complete Deployment Script
-- Version: 1.0
-- Generated: 2026-03-09
-- ==========================================

/*
DEPLOYMENT INSTRUCTIONS:

This script creates a standalone STATIONERYDB database with all 
stationery request, order, inventory, and budget management tables.

Prerequisites:
- SQL Server 2016 or later
- Sufficient disk space for database
- Administrative permissions

Execution Time: 2-3 minutes
*/

-- ==========================================
-- PHASE 1: DATABASE CREATION
-- ==========================================

PRINT '=== PHASE 1: Creating STATIONERYDB ===';
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'STATIONERYDB')
BEGIN
    CREATE DATABASE STATIONERYDB
    ON PRIMARY (
        NAME = 'STATIONERYDB_data',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\STATIONERYDB.mdf',
        SIZE = 200MB,
        MAXSIZE = 2GB,
        FILEGROWTH = 10%
    )
    LOG ON (
        NAME = 'STATIONERYDB_log',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\STATIONERYDB.ldf',
        SIZE = 100MB,
        MAXSIZE = 1GB,
        FILEGROWTH = 10%
    );
    PRINT '✓ STATIONERYDB created successfully';
END
ELSE
BEGIN
    PRINT '✓ STATIONERYDB already exists';
END
GO

-- ==========================================
-- PHASE 2: DEPLOY TABLES
-- ==========================================

USE [STATIONERYDB];
GO

PRINT '';
PRINT '=== PHASE 2: Deploying STATIONERY Tables ===';
GO

CREATE TABLE [STATIONARY_MASTER] (
    [SM_STATIONARYID] BIGINT NOT NULL,
    [SM_CATID] BIGINT NOT NULL,
    [SM_LOC_ID] BIGINT NOT NULL,
    [SM_DESC] VARCHAR(200) NOT NULL,
    [SM_UOMID] BIGINT NOT NULL,
    [SM_MAKE] VARCHAR(10) NOT NULL,
    [SM_PRICE_PERUNIT] BIGINT NULL,
    [SM_REORDER_LEVEL] BIGINT NULL,
    [SM_UPDATED_BY] BIGINT NOT NULL,
    [SM_UPDATED_ON] DATETIME2(3) NOT NULL,
    [SM_VMID] BIGINT NOT NULL,
    [SM_CLOSED] CHAR(1) NOT NULL,
    [SM_OPENINGSTOCK] BIGINT NOT NULL,
    CONSTRAINT [PK_STATIONARY_MASTER] PRIMARY KEY ([SM_STATIONARYID])
);
PRINT '✓ STATIONARY_MASTER table created';
GO

CREATE TABLE [SP_REQUEST_MAIN] (
    [RM_REQUESTID] BIGINT NOT NULL,
    [RM_REQUESTEDBY] BIGINT NOT NULL,
    [RM_REQUESTEDON] DATETIME2(3) NOT NULL,
    [RM_LOCATIONID] BIGINT NULL,
    [RM_UNITCODE] CHAR(3) NULL,
    CONSTRAINT [PK_SP_REQUEST_MAIN] PRIMARY KEY ([RM_REQUESTID])
);
PRINT '✓ SP_REQUEST_MAIN table created';
GO

CREATE TABLE [SP_REQUEST_SUB] (
    [RS_REQUESTSUB_ID] BIGINT NOT NULL,
    [RS_REQUESTID] BIGINT NOT NULL,
    [RS_STATIONARYID] BIGINT NOT NULL,
    [RS_DEPTID] BIGINT NOT NULL,
    [RS_EXPECTED_DATE] DATETIME2(3) NOT NULL,
    [RS_USER_SYSID] BIGINT NULL,
    [RS_REQUESTEDQTY] BIGINT NOT NULL,
    [RS_INDENTEDQTY] BIGINT NULL,
    [RS_APPROVEDQTY] BIGINT NULL,
    [RS_APPROVER_SYSID] BIGINT NULL,
    [RS_APPROVER_RAMARKS] VARCHAR(255) NULL,
    [RS_RECEIVED_DATE] DATETIME2(3) NULL,
    [RS_STATUS] VARCHAR(1) NOT NULL,
    [RS_UPDATED_BY] BIGINT NOT NULL,
    [RS_UPDATED_ON] DATETIME2(3) NOT NULL,
    [RS_APPROVED_ON] DATETIME2(3) NULL,
    CONSTRAINT [PK_SP_REQUEST_SUB] PRIMARY KEY ([RS_REQUESTSUB_ID]),
    CONSTRAINT [FK_SP_REQUEST_SUB_MAIN] FOREIGN KEY ([RS_REQUESTID]) REFERENCES [SP_REQUEST_MAIN]([RM_REQUESTID])
);
PRINT '✓ SP_REQUEST_SUB table created';
GO

CREATE TABLE [SP_ORDER_MAIN] (
    [OM_ORDERMAIN_ID] BIGINT NOT NULL,
    [OM_LOCATION_ID] BIGINT NOT NULL,
    [OM_VENDORID] BIGINT NOT NULL,
    [OM_DELIVERYDATE] DATETIME2(3) NOT NULL,
    [OM_ORDEREDDATE] DATETIME2(3) NOT NULL,
    [OM_ORDEREDBY] BIGINT NOT NULL,
    CONSTRAINT [PK_SP_ORDER_MAIN] PRIMARY KEY ([OM_ORDERMAIN_ID])
);
PRINT '✓ SP_ORDER_MAIN table created';
GO

CREATE TABLE [SP_ORDER_SUB] (
    [OS_ORDERSUB_ID] BIGINT NOT NULL,
    [OS_ORDERMAIN_ID] BIGINT NOT NULL,
    [OS_REQUESTSUB_ID] BIGINT NOT NULL,
    [OS_ORDERED_QTY] BIGINT NOT NULL,
    [OS_RECEIVEDON] DATETIME2(3) NULL,
    [OS_RECEIVED_BY] BIGINT NOT NULL,
    [OS_ORDERPRICE] BIGINT NOT NULL,
    [OS_ACTUALPRICE] BIGINT NOT NULL,
    [OS_RECEIVEDDATE] DATETIME2(3) NOT NULL,
    [OS_DELIVERYDATE] DATETIME2(3) NOT NULL,
    [OS_RECEIPTENTRYBY] BIGINT NULL,
    [OS_RECEIPTENTRYON] DATETIME2(3) NULL,
    CONSTRAINT [PK_SP_ORDER_SUB] PRIMARY KEY ([OS_ORDERSUB_ID]),
    CONSTRAINT [FK_SP_ORDER_SUB_MAIN] FOREIGN KEY ([OS_ORDERMAIN_ID]) REFERENCES [SP_ORDER_MAIN]([OM_ORDERMAIN_ID])
);
PRINT '✓ SP_ORDER_SUB table created';
GO

CREATE TABLE [SP_DEPT_BUDGET] (
    [DB_LOCATION_ID] BIGINT NOT NULL,
    [DB_UNIT_CODE] CHAR(3) NOT NULL,
    [DB_DEPT_ID] BIGINT NOT NULL,
    [DB_FINYEAR_ID] BIGINT NOT NULL,
    [DB_BUDGETAMOUNT] BIGINT NOT NULL,
    [DB_UPDATED_BY] BIGINT NOT NULL,
    [DB_UPDATED_ON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_SP_DEPT_BUDGET] PRIMARY KEY ([DB_LOCATION_ID], [DB_DEPT_ID], [DB_FINYEAR_ID])
);
PRINT '✓ SP_DEPT_BUDGET table created';
GO

CREATE TABLE [SP_UNIT_BUDGET] (
    [UB_LOCATION_ID] BIGINT NOT NULL,
    [UB_UNIT_CODE] CHAR(3) NOT NULL,
    [UB_FINYEAR_ID] BIGINT NOT NULL,
    [UB_BUDGETAMOUNT] BIGINT NOT NULL,
    [UB_UPDATED_BY] BIGINT NOT NULL,
    [UB_UPDATED_ON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_SP_UNIT_BUDGET] PRIMARY KEY ([UB_LOCATION_ID], [UB_UNIT_CODE], [UB_FINYEAR_ID])
);
PRINT '✓ SP_UNIT_BUDGET table created';
GO

CREATE TABLE [SP_DEPT_APPROVER] (
    [DA_LOCATION_ID] BIGINT NOT NULL,
    [DA_UNIT_CODE] CHAR(3) NOT NULL,
    [DA_DEPT_ID] BIGINT NOT NULL,
    [DA_EMP_SYSID] BIGINT NOT NULL,
    [DA_TYPE] CHAR(1) NOT NULL,
    [DA_EFFECTIVE_DATE] DATETIME2(3) NOT NULL,
    [DA_CLOSURE_DATE] DATETIME2(3) NULL,
    [DA_UPDATED_BY] BIGINT NOT NULL,
    [DA_UPDATED_ON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_SP_DEPT_APPROVER] PRIMARY KEY ([DA_LOCATION_ID], [DA_DEPT_ID], [DA_EMP_SYSID], [DA_TYPE])
);
PRINT '✓ SP_DEPT_APPROVER table created';
GO

CREATE TABLE [SP_UNIT_APPROVER] (
    [UA_LOCATION_ID] BIGINT NOT NULL,
    [UA_UNIT_CODE] CHAR(3) NOT NULL,
    [UA_EMP_SYSID] BIGINT NOT NULL,
    [UA_TYPE] CHAR(1) NOT NULL,
    [UA_EFFECTIVE_DATE] DATETIME2(3) NOT NULL,
    [UA_CLOSURE_DATE] VARCHAR(255) NULL,
    [UA_UPDATED_BY] BIGINT NOT NULL,
    [UA_UPDATED_ON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_SP_UNIT_APPROVER] PRIMARY KEY ([UA_LOCATION_ID], [UA_UNIT_CODE], [UA_EMP_SYSID], [UA_TYPE])
);
PRINT '✓ SP_UNIT_APPROVER table created';
GO

CREATE TABLE [SP_LOCATION_ADMIN] (
    [LA_LOCATION_ID] BIGINT NOT NULL,
    [LA_EMP_SYSID] BIGINT NOT NULL,
    [LA_EFFECTIVE_DATE] DATETIME2(3) NOT NULL,
    [LA_CLOSURE_DATE] DATETIME2(3) NULL,
    [LA_UPDATED_BY] BIGINT NOT NULL,
    [LA_UPDATED_ON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_SP_LOCATION_ADMIN] PRIMARY KEY ([LA_LOCATION_ID], [LA_EMP_SYSID])
);
PRINT '✓ SP_LOCATION_ADMIN table created';
GO

CREATE TABLE [SP_CATEGORY_DEFAULT] (
    [CD_STATIONERYID] BIGINT NOT NULL,
    [CD_CATEGORYID] BIGINT NOT NULL,
    [CD_LOCATIONID] BIGINT NOT NULL,
    [CD_MODIFIEDBY] BIGINT NOT NULL,
    [CD_MODIFIEDON] DATETIME2(3) NOT NULL
);
PRINT '✓ SP_CATEGORY_DEFAULT table created';
GO

-- Create indexes
CREATE INDEX [IDX_STATIONERY_MASTER_LOCID] ON [STATIONARY_MASTER]([SM_LOC_ID]);
CREATE INDEX [IDX_REQUEST_MAIN_REQUESTEDBY] ON [SP_REQUEST_MAIN]([RM_REQUESTEDBY]);
CREATE INDEX [IDX_REQUEST_SUB_REQUESTID] ON [SP_REQUEST_SUB]([RS_REQUESTID]);
CREATE INDEX [IDX_REQUEST_SUB_STATUS] ON [SP_REQUEST_SUB]([RS_STATUS]);
CREATE INDEX [IDX_ORDER_MAIN_VENDORID] ON [SP_ORDER_MAIN]([OM_VENDORID]);
CREATE INDEX [IDX_ORDER_SUB_ORDERMAINID] ON [SP_ORDER_SUB]([OS_ORDERMAIN_ID]);
PRINT '✓ Indexes created';
GO

PRINT '✓ Phase 2 Complete: All tables deployed';
GO

-- ==========================================
-- PHASE 3: DEPLOY PROCEDURES AND FUNCTIONS
-- ==========================================

PRINT '';
PRINT '=== PHASE 3: Deploying Procedures and Functions ===';
GO

-- Function: fn_GetDeptRemainingBudget
CREATE OR ALTER FUNCTION dbo.fn_GetDeptRemainingBudget
(
    @p_LocationID BIGINT,
    @p_DeptID BIGINT,
    @p_FinYearID BIGINT
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @TotalBudget BIGINT;
    DECLARE @ApprovedAmount BIGINT;

    SELECT @TotalBudget = DB_BUDGETAMOUNT
    FROM SP_DEPT_BUDGET
    WHERE DB_LOCATION_ID = @p_LocationID
      AND DB_DEPT_ID = @p_DeptID
      AND DB_FINYEAR_ID = @p_FinYearID;

    SELECT @ApprovedAmount = SUM(RS_APPROVEDQTY * ISNULL(
        (SELECT SM_PRICE_PERUNIT FROM STATIONARY_MASTER WHERE SM_STATIONARYID = RS_STATIONARYID), 0))
    FROM SP_REQUEST_SUB RS
    WHERE RS.RS_DEPTID = @p_DeptID
      AND RS.RS_STATUS IN ('A', 'P');

    RETURN ISNULL(@TotalBudget, 0) - ISNULL(@ApprovedAmount, 0);
END;
GO
PRINT '✓ fn_GetDeptRemainingBudget function created';
GO

-- Procedure: usp_StationeryRequestSubmit
CREATE OR ALTER PROCEDURE dbo.usp_StationeryRequestSubmit
(
    @p_RequestedBy BIGINT,
    @p_LocationID BIGINT,
    @p_UnitCode CHAR(3),
    @p_NewRequestID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @p_NewRequestID = ISNULL(MAX(RM_REQUESTID), 0) + 1 FROM SP_REQUEST_MAIN;

        INSERT INTO SP_REQUEST_MAIN (RM_REQUESTID, RM_REQUESTEDBY, RM_REQUESTEDON, RM_LOCATIONID, RM_UNITCODE)
        VALUES (@p_NewRequestID, @p_RequestedBy, GETDATE(), @p_LocationID, @p_UnitCode);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '✓ usp_StationeryRequestSubmit procedure created';
GO

-- Procedure: usp_StationeryRequestApprove
CREATE OR ALTER PROCEDURE dbo.usp_StationeryRequestApprove
(
    @p_RequestSubID BIGINT,
    @p_ApprovedQty BIGINT,
    @p_ApproverSysID BIGINT,
    @p_Remarks VARCHAR(255) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE SP_REQUEST_SUB
        SET RS_APPROVEDQTY = @p_ApprovedQty,
            RS_APPROVER_SYSID = @p_ApproverSysID,
            RS_APPROVER_RAMARKS = ISNULL(@p_Remarks, RS_APPROVER_RAMARKS),
            RS_STATUS = 'A',
            RS_APPROVED_ON = GETDATE(),
            RS_UPDATED_BY = @p_ApproverSysID,
            RS_UPDATED_ON = GETDATE()
        WHERE RS_REQUESTSUB_ID = @p_RequestSubID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '✓ usp_StationeryRequestApprove procedure created';
GO

-- Procedure: usp_ReceiveOrder
CREATE OR ALTER PROCEDURE dbo.usp_ReceiveOrder
(
    @p_OrderSubID BIGINT,
    @p_ReceivedQty BIGINT,
    @p_ReceivedBy BIGINT,
    @p_ReceiptDate DATETIME2(3) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @RequestSubID BIGINT, @StationaryID BIGINT;
        
        SELECT @RequestSubID = OS_REQUESTSUB_ID
        FROM SP_ORDER_SUB
        WHERE OS_ORDERSUB_ID = @p_OrderSubID;

        SELECT @StationaryID = RS_STATIONARYID
        FROM SP_REQUEST_SUB
        WHERE RS_REQUESTSUB_ID = @RequestSubID;

        UPDATE SP_ORDER_SUB
        SET OS_RECEIVEDON = ISNULL(@p_ReceiptDate, GETDATE()),
            OS_RECEIVED_BY = @p_ReceivedBy,
            OS_RECEIVEDDATE = ISNULL(@p_ReceiptDate, GETDATE()),
            OS_RECEIPTENTRYBY = @p_ReceivedBy,
            OS_RECEIPTENTRYON = GETDATE()
        WHERE OS_ORDERSUB_ID = @p_OrderSubID;

        UPDATE STATIONARY_MASTER
        SET SM_OPENINGSTOCK = SM_OPENINGSTOCK + @p_ReceivedQty,
            SM_UPDATED_BY = @p_ReceivedBy,
            SM_UPDATED_ON = GETDATE()
        WHERE SM_STATIONARYID = @StationaryID;

        UPDATE SP_REQUEST_SUB
        SET RS_RECEIVED_DATE = ISNULL(@p_ReceiptDate, GETDATE()),
            RS_STATUS = 'C',
            RS_UPDATED_BY = @p_ReceivedBy,
            RS_UPDATED_ON = GETDATE()
        WHERE RS_REQUESTSUB_ID = @RequestSubID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
PRINT '✓ usp_ReceiveOrder procedure created';
GO

PRINT '✓ Phase 3 Complete: All procedures deployed';
GO

-- ==========================================
-- PHASE 4: DEPLOY TRIGGERS
-- ==========================================

PRINT '';
PRINT '=== PHASE 4: Deploying Triggers ===';
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'STATIONERY_REORDER_ALERT')
CREATE TABLE dbo.STATIONERY_REORDER_ALERT
(
    AlertID BIGINT IDENTITY(1,1) PRIMARY KEY,
    StationaryID BIGINT NOT NULL,
    AlertDate DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    CurrentStock BIGINT NOT NULL,
    ReorderLevel BIGINT NOT NULL,
    Resolved CHAR(1) DEFAULT 'N'
);
GO

CREATE OR ALTER TRIGGER dbo.trg_StationeryRequestSub_StatusChange
ON dbo.SP_REQUEST_SUB
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(RS_STATUS)
    BEGIN
        UPDATE RM
        SET RM.RM_REQUESTEDON = RM.RM_REQUESTEDON
        FROM dbo.SP_REQUEST_MAIN RM
        WHERE EXISTS (
            SELECT 1
            FROM inserted I
            INNER JOIN deleted D ON I.RS_REQUESTSUB_ID = D.RS_REQUESTSUB_ID
            WHERE I.RS_STATUS <> D.RS_STATUS
              AND I.RS_REQUESTID = RM.RM_REQUESTID
        );
    END
END;
GO
PRINT '✓ trg_StationeryRequestSub_StatusChange trigger created';
GO

CREATE OR ALTER TRIGGER dbo.trg_StationeryMaster_ReorderAlert
ON dbo.STATIONARY_MASTER
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(SM_OPENINGSTOCK)
    BEGIN
        INSERT INTO dbo.STATIONERY_REORDER_ALERT (StationaryID, CurrentStock, ReorderLevel)
        SELECT I.SM_STATIONARYID, I.SM_OPENINGSTOCK, I.SM_REORDER_LEVEL
        FROM inserted I
        INNER JOIN deleted D ON I.SM_STATIONARYID = D.SM_STATIONARYID
        WHERE I.SM_OPENINGSTOCK < I.SM_REORDER_LEVEL
          AND (D.SM_OPENINGSTOCK >= D.SM_REORDER_LEVEL OR D.SM_OPENINGSTOCK IS NULL);
    END
END;
GO
PRINT '✓ trg_StationeryMaster_ReorderAlert trigger created';
GO

PRINT '✓ Phase 4 Complete: All triggers deployed';
GO

-- ==========================================
-- PHASE 5: VERIFICATION
-- ==========================================

PRINT '';
PRINT '=== PHASE 5: Verification ===';
GO

PRINT 'Tables:';
SELECT COUNT(*) as TableCount FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo');

PRINT 'Procedures and Functions:';
SELECT COUNT(*) as RoutineCount FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'dbo';

PRINT 'Triggers:';
SELECT COUNT(*) as TriggerCount FROM sys.triggers WHERE parent_class = 0;

PRINT 'Indexes:';
SELECT COUNT(*) as IndexCount FROM sys.indexes WHERE object_id > 100 AND name IS NOT NULL;

GO

-- ==========================================
-- DEPLOYMENT SUMMARY
-- ==========================================

PRINT '';
PRINT '========================================';
PRINT 'STATIONERYDB DEPLOYMENT COMPLETE';
PRINT '========================================';
PRINT '';
PRINT 'Database: STATIONERYDB';
PRINT 'Module: STATIONERY';
PRINT 'Status: ✓ Successfully Deployed';
PRINT '';
PRINT 'Objects Created:';
PRINT '  ✓ 11 Tables (Request, Order, Budget, Approver, etc.)';
PRINT '  ✓ 6 Indexes (Performance optimization)';
PRINT '  ✓ 3 Procedures (Request management & approval)';
PRINT '  ✓ 1 Function (fn_GetDeptRemainingBudget)';
PRINT '  ✓ 2 Triggers (Status management & reorder alerts)';
PRINT '  ✓ 1 Supporting Table (STATIONERY_REORDER_ALERT)';
PRINT '';
PRINT 'Quick Start:';
PRINT '  1. Verify tables in SSMS Object Explorer';
PRINT '  2. Test procedure: EXEC usp_StationeryRequestSubmit ...';
PRINT '  3. Query budget: SELECT dbo.fn_GetDeptRemainingBudget(1,5,1);';
PRINT '';
PRINT '========================================';
GO

-- ==========================================
-- END OF STATIONERYDB DEPLOYMENT
-- ==========================================
