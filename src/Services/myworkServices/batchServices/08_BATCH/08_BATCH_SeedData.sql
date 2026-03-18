-- ==========================================
-- BATCH Module - Seed Data
-- Database: MYWORKDB
-- ==========================================

USE MYWORKDB;
GO

IF NOT EXISTS (SELECT 1 FROM BATCH_MASTER WHERE BATCH_ID = 1001)
BEGIN
    INSERT INTO BATCH_MASTER (BATCH_ID, BATCH_MONTHNO, BATCH_STATUS, BATCH_LASTMODIFIEDBY, BATCH_LASTMODIFIEDON)
    VALUES
        (1001, 1, 'O', 1, SYSDATETIME()),
        (1002, 2, 'O', 1, SYSDATETIME()),
        (1003, 3, 'O', 1, SYSDATETIME());
    PRINT 'BATCH seed data inserted.';
END
ELSE
    PRINT 'BATCH seed data already present — skipped.';
GO
