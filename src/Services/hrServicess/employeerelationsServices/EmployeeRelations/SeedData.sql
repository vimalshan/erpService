-- =============================================
-- EWS Seed Data
-- =============================================
USE [HRDB];
GO

-- EWS Categories
IF NOT EXISTS (SELECT 1 FROM EWS_CATEGORY WHERE EWS_CATID = 1)
BEGIN
    INSERT INTO EWS_CATEGORY (EWS_CATID, EWS_CATCODE, EWS_CATDESC) VALUES
    (1, 'R', 'Red - High Risk'),
    (2, 'G', 'Green - Low Risk'),
    (3, 'A', 'Amber - Medium Risk');
END

-- EWS Periods
IF NOT EXISTS (SELECT 1 FROM EWS_PERIOD WHERE PERIOD_ID = 1)
BEGIN
    INSERT INTO EWS_PERIOD (PERIOD_ID, PERIOD_YEAR, PERIOD_QUARTER, PERIOD_FROMDATE, PERIOD_TODATE, PERIOD_LIVEFLAG, PERIOD_RELEASEDATE, PERIOD_LEAVESTART, PERIOD_CLOSEDATE, PERIOD_STATUS)
    VALUES
    (1, 2026, 1, '2026-01-01', '2026-03-31', 'Y', '2026-01-05', '2026-01-01', '2026-04-15', 'A'),
    (2, 2026, 2, '2026-04-01', '2026-06-30', 'N', '2026-04-05', '2026-04-01', '2026-07-15', 'N'),
    (3, 2026, 3, '2026-07-01', '2026-09-30', 'N', '2026-07-05', '2026-07-01', '2026-10-15', 'N'),
    (4, 2026, 4, '2026-10-01', '2026-12-31', 'N', '2026-10-05', '2026-10-01', '2027-01-15', 'N');
END

-- EWS Menu
IF NOT EXISTS (SELECT 1 FROM EWS_MENU WHERE MENUID = 1)
BEGIN
    INSERT INTO EWS_MENU (MENUID, MENUCODE, MENUDESC, ACTIVEFLAG, REMARKS) VALUES
    (1, 'EWS', 'Early Warning System', 'Y', NULL),
    (2, 'DIS', 'Disciplinary Management', 'Y', NULL),
    (3, 'SUR', 'Survey Management', 'Y', NULL);
END

PRINT 'Seed data applied successfully.';
GO
