-- ==========================================
-- Seed Data: ERROR LOGGING MODULE
-- Run AFTER InitialCreate migration
-- ==========================================

USE SCIDB;
GO

SET IDENTITY_INSERT dbo.ERRSP ON;

INSERT INTO dbo.ERRSP (Id, ERR_MESS, ERR_SP, ERR_REF, ERR_DATE)
VALUES
    (1, 'Seed: Division by zero encountered.', 'usp_ProcessOrder', 1001, GETDATE()),
    (2, 'Seed: Foreign key violation on Orders table.', 'usp_InsertOrderLine', 1002, GETDATE()),
    (3, 'Seed: Timeout expired during batch processing.', 'usp_BatchProcess', 1003, GETDATE());

SET IDENTITY_INSERT dbo.ERRSP OFF;
GO

PRINT 'Seed data inserted successfully.';
GO
