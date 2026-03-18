-- ==========================================
-- MODULE: STATIONERY
-- Component: Triggers
-- Description: Triggers for stationery workflow automations and alerts
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- Table: STATIONERY_REORDER_ALERT
-- Purpose: Tracks stationery items that need reordering
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

-- Trigger: trg_StationeryRequestSub_StatusChange
-- Purpose: Update request main when sub items status changes to completed
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
              AND NOT EXISTS (
                  SELECT 1
                  FROM dbo.SP_REQUEST_SUB RS
                  WHERE RS.RS_REQUESTID = RM.RM_REQUESTID
                    AND RS.RS_STATUS NOT IN ('C', 'X')
              )
        );
    END
END;
GO

-- Trigger: trg_StationeryMaster_ReorderAlert
-- Purpose: Create alert when stock falls below reorder level
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

-- ==========================================
-- END OF STATIONERY TRIGGERS
-- ==========================================
