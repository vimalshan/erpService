-- ==========================================
-- MODULE: VENDOR
-- Component: Triggers
-- Description: Triggers for vendor data integrity and audits
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- Trigger: trg_VendorMaster_UpdateAudit
-- Purpose: Automatically update VM_UPDATED_ON on vendor master changes
CREATE OR ALTER TRIGGER dbo.trg_VendorMaster_UpdateAudit
ON dbo.VENDOR_MASTER
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE VM
    SET VM_UPDATED_ON = GETDATE()
    FROM dbo.VENDOR_MASTER VM
    INNER JOIN inserted I ON VM.VM_ID = I.VM_ID;
END;
GO

-- ==========================================
-- END OF VENDOR TRIGGERS
-- ==========================================
