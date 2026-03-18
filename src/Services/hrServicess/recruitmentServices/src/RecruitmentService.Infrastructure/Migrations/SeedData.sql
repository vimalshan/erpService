-- ============================================================
-- Seed Data Script for HRDB Recruitment Module
-- Run after InitialCreate migration
-- ============================================================
USE [HRDB];
GO

-- ── Seed Prospects ───────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM WEBPROSPECT_MAST WHERE WEBUSER_ID = 1001)
BEGIN
    INSERT INTO WEBPROSPECT_MAST
        (WEBUSER_ID, WEBUSER_PWD, WEBUSER_FRS_NAME, WEBUSER_MID_NAME, WEBUSER_LST_NAME,
         WEBUSER_EMAILID, WEBUSER_STATUS, WEBUSER_DATEOFBIRTH, WEBUSER_CREATEDON, WEBUSER_TYPE)
    VALUES
        (1001, 'Pass@1234', 'John', 'A', 'Doe', 'john.doe@example.com', 'L', '1990-05-15', GETDATE(), 'R'),
        (1002, 'Pass@1234', 'Jane', NULL, 'Smith', 'jane.smith@example.com', 'L', '1992-08-20', GETDATE(), 'R');
END
GO

-- ── Seed Vacancies ───────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM VACANCY_MAIN WHERE VACANCY_ID = 1)
BEGIN
    INSERT INTO VACANCY_MAIN
        (VACANCY_ID, VACANCY_UNIT, VACANCY_GRADE, VACANCY_POSITIONID, VACANCY_NAME,
         VACANCY_REPORTING, VACANCY_LOCATION, VACANCY_PROCESS, VACANCY_AGE,
         VACANCY_EXPERIENCE, VACANCY_QUALIFICATION, VACANCY_NARRATION1,
         VACANCY_LASTDATE, VACANCY_ADINTRAFLAG, VACANCY_ADINTERFLAG,
         VACANCY_POSTBY, VACANCY_POSTDATE, VACANCY_LIVESTATUS, VACANCY_UNITID,
         VACANCY_TYPE, VACANCY_NOS, VACANCY_CTCFROM, VACANCY_CTCTO,
         VACANCY_DESIGNATION, VACANCY_UPLOADRESUME)
    VALUES
        (1, 'HRD', 5, 101, 'Senior Software Engineer',
         'Engineering Manager', 100, 200, '25-35 Years',
         'Minimum 5 years in .NET development',
         'BE/BTech in Computer Science or equivalent',
         'Lead development of microservices and cloud-native applications.',
         DATEADD(DAY, 30, GETDATE()), 'Y', 'Y',
         1001, GETDATE(), 'Y', 10,
         'MS', 2, 800000, 1500000,
         'Senior Software Engineer', 'Y'),
        (2, 'FIN', 3, 102, 'Junior Accountant',
         'Finance Manager', 100, 300, '22-28 Years',
         'Minimum 1 year in accounting or finance',
         'B.Com / MBA Finance',
         'Handle day-to-day accounting entries and reconciliations.',
         DATEADD(DAY, 20, GETDATE()), 'Y', 'Y',
         1001, GETDATE(), 'Y', 10,
         'NMS', 3, 300000, 600000,
         'Junior Accountant', 'Y');
END
GO

-- ── Seed Application Histories ───────────────────────────────
IF NOT EXISTS (SELECT 1 FROM APPLICATION_HISTORY WHERE APP_ID = 5001)
BEGIN
    INSERT INTO APPLICATION_HISTORY
        (APP_ID, APP_SL, APP_UNIT, APP_VACANCYID, APP_STATUS, APP_REMARKS, APP_UPDATEDBY, APP_UPDATEDON)
    VALUES
        (5001, 1, 'HRD', 1, '01', 'Application received via portal.', 1002, GETDATE()),
        (5002, 1, 'FIN', 2, '04', 'Shortlisted for interview.', 1001, GETDATE());
END
GO

PRINT 'Seed data inserted successfully.';
GO
