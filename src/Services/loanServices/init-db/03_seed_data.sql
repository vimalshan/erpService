-- =============================================================================
--  03_seed_data.sql
--  Seeds reference / LOV data and a sample loan master record.
--  Idempotent — uses MERGE so it can be re-run safely.
-- =============================================================================
USE LOANDB;
GO

SET NOCOUNT ON;
GO

-- ── LOV Categories ────────────────────────────────────────────────────────────
MERGE [LOV_CATEGORY] AS target
USING (VALUES
    (1, 'L', 'Loan Type',           1, GETDATE()),
    (2, 'S', 'Loan Status',         1, GETDATE()),
    (3, 'D', 'Document Type',       1, GETDATE()),
    (4, 'F', 'Interest Frequency',  1, GETDATE()),
    (5, 'C', 'Compounding Factor',  1, GETDATE())
) AS source (LOVC_CATID, LOVC_CATCODE, LOVC_CATDESC, LOVC_MODIFIEDBY, LOVC_MODIFIEDON)
ON target.LOVC_CATID = source.LOVC_CATID
WHEN NOT MATCHED THEN
    INSERT (LOVC_CATID, LOVC_CATCODE, LOVC_CATDESC, LOVC_MODIFIEDBY, LOVC_MODIFIEDON)
    VALUES (source.LOVC_CATID, source.LOVC_CATCODE, source.LOVC_CATDESC, source.LOVC_MODIFIEDBY, source.LOVC_MODIFIEDON);

-- ── LOV Details ───────────────────────────────────────────────────────────────
MERGE [LOV_DETAILS] AS target
USING (VALUES
    -- Loan Types (Category L)
    (101, 1, 'PERSONAL',  'Personal Loan',        'Y', 1, GETDATE()),
    (102, 1, 'HOME',      'Home Loan',             'Y', 1, GETDATE()),
    (103, 1, 'VEHICLE',   'Vehicle Loan',          'Y', 1, GETDATE()),
    (104, 1, 'FESTIVAL',  'Festival Advance',      'Y', 1, GETDATE()),
    (105, 1, 'EDUCATION', 'Education Loan',        'Y', 1, GETDATE()),
    -- Loan Status (Category S)
    (201, 2, 'A', 'Active',              'Y', 1, GETDATE()),
    (202, 2, 'C', 'Closed',              'Y', 1, GETDATE()),
    (203, 2, 'D', 'Disbursed',           'Y', 1, GETDATE()),
    (204, 2, 'P', 'Pending Approval',    'Y', 1, GETDATE()),
    (205, 2, 'R', 'Rejected',            'Y', 1, GETDATE()),
    -- Document Types (Category D)
    (301, 3, 'AADHAR',  'Aadhar Card',    'Y', 1, GETDATE()),
    (302, 3, 'PAN',     'PAN Card',       'Y', 1, GETDATE()),
    (303, 3, 'SALARY',  'Salary Slip',    'Y', 1, GETDATE()),
    (304, 3, 'BANK',    'Bank Statement', 'Y', 1, GETDATE()),
    -- Interest Frequency (Category F)
    (401, 4, 'M', 'Monthly',   'Y', 1, GETDATE()),
    (402, 4, 'Q', 'Quarterly', 'Y', 1, GETDATE()),
    (403, 4, 'Y', 'Yearly',    'Y', 1, GETDATE()),
    -- Compounding Factor (Category C)
    (501, 5, 'S', 'Simple',   'Y', 1, GETDATE()),
    (502, 5, 'C', 'Compound', 'Y', 1, GETDATE())
) AS source (LOVD_DETID, LOVD_CATID, LOVD_DETCODE, LOVD_DETDESC, LOVD_ACTIVE, LOVD_MODIFIEDBY, LOVD_MODIFIEDON)
ON target.LOVD_DETID = source.LOVD_DETID
WHEN NOT MATCHED THEN
    INSERT (LOVD_DETID, LOVD_CATID, LOVD_DETCODE, LOVD_DETDESC, LOVD_ACTIVE, LOVD_MODIFIEDBY, LOVD_MODIFIEDON)
    VALUES (source.LOVD_DETID, source.LOVD_CATID, source.LOVD_DETCODE, source.LOVD_DETDESC, source.LOVD_ACTIVE, source.LOVD_MODIFIEDBY, source.LOVD_MODIFIEDON);

-- ── Sample Loan Master Records ────────────────────────────────────────────────
MERGE [LOAN_MASTER] AS target
USING (VALUES
    (1, 'PERSONAL-01', 'Personal Loan Scheme',      'P', 'Y', 1000000, 10000,  60, 6,  12.0, 'S', 'M', 1, GETDATE()),
    (2, 'HOME-01',     'Home Loan Scheme',           'H', 'Y', 5000000, 100000, 240, 12, 8.5,  'C', 'M', 1, GETDATE()),
    (3, 'VEHICLE-01',  'Vehicle Loan Scheme',        'V', 'Y', 2000000, 50000,  84, 12, 10.5, 'S', 'M', 1, GETDATE()),
    (4, 'FESTIVAL-01', 'Festival Advance Scheme',    'F', 'Y', 100000,  5000,   12, 1,  0.0,  'S', 'M', 1, GETDATE()),
    (5, 'EDUCATION-01','Education Loan Scheme',      'E', 'Y', 2000000, 50000,  120, 12, 7.0, 'S', 'M', 1, GETDATE())
) AS source (LOAN_LOANID, LOAN_LOANCODE, LOAN_LOANDESC, LOAN_LOANTYPE, LOAN_ACTIVE,
             LOAN_MAXAMT, LOAN_MINAMT, LOAN_MAXTENURE, LOAN_MINTENURE,
             LOAN_INTRATE, LOAN_COMPFACTOR, LOAN_INTFREQ,
             LOAN_MODIFIEDBY, LOAN_MODIFIEDON)
ON target.LOAN_LOANID = source.LOAN_LOANID
WHEN NOT MATCHED THEN
    INSERT (LOAN_LOANID, LOAN_LOANCODE, LOAN_LOANDESC, LOAN_LOANTYPE, LOAN_ACTIVE,
            LOAN_MAXAMT, LOAN_MINAMT, LOAN_MAXTENURE, LOAN_MINTENURE,
            LOAN_INTRATE, LOAN_COMPFACTOR, LOAN_INTFREQ,
            LOAN_MODIFIEDBY, LOAN_MODIFIEDON)
    VALUES (source.LOAN_LOANID, source.LOAN_LOANCODE, source.LOAN_LOANDESC, source.LOAN_LOANTYPE, source.LOAN_ACTIVE,
            source.LOAN_MAXAMT, source.LOAN_MINAMT, source.LOAN_MAXTENURE, source.LOAN_MINTENURE,
            source.LOAN_INTRATE, source.LOAN_COMPFACTOR, source.LOAN_INTFREQ,
            source.LOAN_MODIFIEDBY, source.LOAN_MODIFIEDON);

PRINT 'Seed data applied.';
GO
