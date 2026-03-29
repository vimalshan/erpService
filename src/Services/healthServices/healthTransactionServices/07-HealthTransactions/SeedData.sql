-- ============================================================
-- HealthTransaction Service — Seed Data
-- Database: HEALTHDB_HealthTransactions
-- ============================================================

USE HEALTHDB_HealthTransactions;
GO

-- Seed CHKUP_PRE_MAIN
INSERT INTO CHKUP_PRE_MAIN (CPM_EMP_NUM, CPM_COM_COD, CPM_HLTH_NUM, CPM_PHYS_HAND, CPM_PROP_EMP, CPM_IDENT_MARKS, CPM_FINAL_RMKS, CPM_FIT_PH, CPM_FIT_FINAL, CPM_CHK_DAT)
VALUES
(1001, 'A01', 10001, 'N', 'Software Engineer', 'Scar on left hand', 'Fit for employment', 'Y  ', 'Y', '2024-06-01'),
(1002, 'A01', 10002, 'N', 'Accountant',        NULL,                'Requires follow-up',  'N  ', 'N', '2024-06-05'),
(1003, 'B02', 10003, 'Y', 'Manager',            'Tattoo on arm',    'Fit with conditions', 'Y  ', 'Y', '2024-06-10');
GO

-- Seed HLTH_CHKUP_CARD
INSERT INTO HLTH_CHKUP_CARD (HCC_HLTH_NUM, HCC_EMP_NUM, HCC_EMP_DATE, HCC_COM_COD, HCC_PER_DET, HCC_COMPL_DET, HCC_ADV_RMK1, HCC_DOC_DATE1, HCC_ADV_FOLLOW1)
VALUES
(10001, 1001, '2024-06-01', 'A01', 'No known allergies', 'General annual checkup', 'Maintain healthy diet', '2024-06-01', 'Review in 12 months'),
(10002, 1002, '2024-06-05', 'A01', 'History of hypertension', 'Elevated blood pressure', 'Start medication', '2024-06-05', 'Review in 3 months'),
(10003, 1003, '2024-06-10', 'B02', 'Mild diabetes', 'Blood sugar elevated', 'Diet control', '2024-06-10', 'Review in 6 months');
GO

-- Seed HLTH_CHKCARD_SUB
INSERT INTO HLTH_CHKCARD_SUB (HCS_HLTH_NUM, HCS_SYMP_ID, HCS_FLAG_YN, HCS_SYMP_VAL, HCS_EMP_NUM)
VALUES
(10001, 1, 'Y', 'Normal', 1001),
(10001, 2, 'N', 'Absent', 1001),
(10002, 1, 'Y', 'High BP - 150/100', 1002),
(10003, 1, 'Y', 'FBS 7.2', 1003);
GO

-- Seed CHKUP_PFI_HIST
INSERT INTO CHKUP_PFI_HIST (CPH_HLTH_NUM, CPH_EMP_NUM, CPH_SYMP_ID, CPH_YN_FLAG, CPH_IMM_DAT, CPH_TEST_VAL)
VALUES
(10001, 1001, 10, 'Y', '2024-01-15', 'Hepatitis B positive'),
(10001, 1001, 11, 'N', NULL,         NULL),
(10002, 1002, 10, 'Y', '2023-12-01', 'Tetanus boost given'),
(10003, 1003, 12, 'Y', '2024-03-20', 'COVID booster done');
GO

-- Seed HEALTH_DYN_DET
INSERT INTO HEALTH_DYN_DET (CDD_HLTH_NUM, CDD_CHKUP_COD, CDD_COM_COD, CDD_CTRLSRC_ID, CDD_DYN_VAL, CDD_EMP_NUM, CDD_SYS_DAT)
VALUES
(10001, 'BLOOD',  'A01', 1, '120/80',   1001, GETDATE()),
(10001, 'WEIGHT', 'A01', 2, '72 kg',    1001, GETDATE()),
(10002, 'BLOOD',  'A01', 1, '150/100',  1002, GETDATE()),
(10003, 'SUGAR',  'B02', 1, '7.2 FBS',  1003, GETDATE());
GO
