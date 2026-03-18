-- ==========================================
-- MODULE: SCHOLARSHIP
-- Component: Triggers
-- Description: Triggers for scholarship-related automations and audits
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- Trigger: trg_ScholarshipDetail_UpdateAudit
-- Purpose: Automatically update audit columns on any change to SCHOLARSHIP_DETAIL
CREATE OR ALTER TRIGGER dbo.trg_ScholarshipDetail_UpdateAudit
ON dbo.SCHOLARSHIP_DETAIL
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE SD
    SET SCHDET_UPDATEDON = GETDATE()
    FROM dbo.SCHOLARSHIP_DETAIL SD
    INNER JOIN inserted I ON SD.SCHDET_ID = I.SCHDET_ID;
END;
GO

-- ==========================================
-- END OF SCHOLARSHIP TRIGGERS
-- ==========================================
