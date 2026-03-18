-- ==========================================
-- DEVELOPMENT MODULE - Seed Data Script
-- Database: LETDB
-- Purpose: Initial seed data for Development Module
-- ==========================================

USE LETDB;
GO

-- Seed competency indicators
IF NOT EXISTS (SELECT 1 FROM DD_COMPETENCY_IND WHERE COMP_NUM = 1)
BEGIN
    INSERT INTO DD_COMPETENCY_IND (SRL_NO, BAND, COMP_NUM, IND_FLAG, IND_DEFN) VALUES
    (1, 'A', 1, 'Y', 'Demonstrates strategic thinking and long-term vision'),
    (2, 'A', 1, 'Y', 'Leads cross-functional teams with measurable results'),
    (3, 'B', 2, 'Y', 'Applies analytical skills to complex business problems'),
    (4, 'B', 2, 'Y', 'Builds strong internal and external relationships'),
    (5, 'C', 3, 'Y', 'Shows initiative in learning and self-development'),
    (6, 'C', 3, 'Y', 'Communicates effectively across all levels');
END
GO

-- Seed a sample learning plan
IF NOT EXISTS (SELECT 1 FROM DD_LETPLAN WHERE DD_REQNUM = 1001)
BEGIN
    INSERT INTO DD_LETPLAN (
        DD_REQNUM, DD_SNO, DD_USERID, DD_PINNUM,
        DD_DEVSOURCE, DD_DEVNEED, DD_PRIORITY,
        DD_ENTDATE, DD_APPSTATUS
    ) VALUES (
        1001, 1, 'SYSTEM', 0,
        'APPRAISAL', 'Leadership Development', 1,
        GETDATE(), 'F'
    );
END
GO

PRINT 'Seed data applied successfully.';
GO
