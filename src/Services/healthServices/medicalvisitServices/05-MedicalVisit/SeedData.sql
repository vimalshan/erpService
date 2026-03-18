-- ==========================================
-- Module: MedicalVisit
-- Purpose: Seed Data for Medical Clinic Visit & Consultation
-- Generated: 2026-03-09
-- ==========================================

USE HEALTHDB;
GO

-- =====================================================
-- Seed: VISIT_MAIN
-- Company: 001
-- =====================================================

SET IDENTITY_INSERT dbo.VISIT_MAIN OFF;
GO

-- Clear existing seed data (for idempotency)
DELETE FROM [dbo].[VISIT_SUB]   WHERE [VS_COM_COD] = '001' AND [VS_VIS_NUM] IN (1001,1002,1003,1004,1005);
DELETE FROM [dbo].[VISIT_MAIN]  WHERE [VM_COM_COD] = '001' AND [VM_VIS_NUM] IN (1001,1002,1003,1004,1005);
GO

-- =====================================================
-- Visit 1001 - Routine checkup (Active)
-- =====================================================
INSERT INTO [dbo].[VISIT_MAIN] (
    [VM_COM_COD], [VM_VIS_NUM], [VM_USR_ID], [VM_PIN_NUM],
    [VM_WRK_NAM], [VM_CONTRCT_ID], [VM_CONTRCT_NAM],
    [VM_VIS_DAT], [VM_OTH_HOSP],
    [VM_VIS_SHIFT], [VM_VIS_TYP],
    [VM_ATT_COD], [VM_DOC_COD],
    [VM_PAT_DIA], [VM_TRT_REM], [VM_TST_ADV],
    [VM_MED_GIV], [VM_NXT_REV],
    [VM_ENT_USR], [VM_ENT_NUM], [VM_ENT_DAT],
    [DV_MOD_USR], [VM_MOD_NUM], [VM_MOD_DAT],
    [VM_CAN_FLG], [VM_DIA_CAT], [VM_DIA_SUBCAT], [VM_DOC_REMARKS]
) VALUES (
    '001', 1001, 'EMP001', 100001,
    'John Smith', 'CONT001', 'Contract A',
    '2026-01-10 09:00:00', NULL,
    'M', 'R',
    'ATT001', 'DOC001',
    'Mild hypertension, occasional headaches', 'Prescribed antihypertensive medication', 'Blood pressure monitoring weekly',
    'YES', '2026-02-10 09:00:00',
    'ADMIN', 500001, '2026-01-10 09:05:00',
    NULL, NULL, NULL,
    'N', 'CVS', 10001, 'Patient advised lifestyle modifications: reduce salt intake, exercise 30 min daily.'
);
GO

-- =====================================================
-- Visit 1002 - Emergency visit (Active)
-- =====================================================
INSERT INTO [dbo].[VISIT_MAIN] (
    [VM_COM_COD], [VM_VIS_NUM], [VM_USR_ID], [VM_PIN_NUM],
    [VM_WRK_NAM], [VM_CONTRCT_ID], [VM_CONTRCT_NAM],
    [VM_VIS_DAT], [VM_OTH_HOSP],
    [VM_VIS_SHIFT], [VM_VIS_TYP],
    [VM_ATT_COD], [VM_DOC_COD],
    [VM_PAT_DIA], [VM_TRT_REM], [VM_TST_ADV],
    [VM_MED_GIV], [VM_NXT_REV],
    [VM_ENT_USR], [VM_ENT_NUM], [VM_ENT_DAT],
    [DV_MOD_USR], [VM_MOD_NUM], [VM_MOD_DAT],
    [VM_CAN_FLG], [VM_DIA_CAT], [VM_DIA_SUBCAT], [VM_DOC_REMARKS]
) VALUES (
    '001', 1002, 'EMP002', 100002,
    'Jane Doe', 'CONT001', 'Contract A',
    '2026-01-15 14:30:00', NULL,
    'A', 'E',
    'ATT002', 'DOC002',
    'Acute gastroenteritis with dehydration', 'IV fluids administered, antiemetics given', 'Stool culture if no improvement',
    'YES', '2026-01-22 09:00:00',
    'ADMIN', 500002, '2026-01-15 14:35:00',
    'ADMIN', 500002, '2026-01-15 16:00:00',
    'N', 'GAS', 10002, 'Patient responded well to IV fluids. Clear liquid diet advised for 24 hours.'
);
GO

-- =====================================================
-- Visit 1003 - Follow-up visit (Active)
-- =====================================================
INSERT INTO [dbo].[VISIT_MAIN] (
    [VM_COM_COD], [VM_VIS_NUM], [VM_USR_ID], [VM_PIN_NUM],
    [VM_WRK_NAM], [VM_CONTRCT_ID], [VM_CONTRCT_NAM],
    [VM_VIS_DAT], [VM_OTH_HOSP],
    [VM_VIS_SHIFT], [VM_VIS_TYP],
    [VM_ATT_COD], [VM_DOC_COD],
    [VM_PAT_DIA], [VM_TRT_REM], [VM_TST_ADV],
    [VM_MED_GIV], [VM_NXT_REV],
    [VM_ENT_USR], [VM_ENT_NUM], [VM_ENT_DAT],
    [DV_MOD_USR], [VM_MOD_NUM], [VM_MOD_DAT],
    [VM_CAN_FLG], [VM_DIA_CAT], [VM_DIA_SUBCAT], [VM_DOC_REMARKS]
) VALUES (
    '001', 1003, 'EMP001', 100001,
    'John Smith', 'CONT001', 'Contract A',
    '2026-02-10 09:15:00', NULL,
    'M', 'F',
    'ATT001', 'DOC001',
    'Hypertension - follow up, BP improved', 'Continue antihypertensive medication', NULL,
    'YES', '2026-03-10 09:00:00',
    'ADMIN', 500003, '2026-02-10 09:20:00',
    NULL, NULL, NULL,
    'N', 'CVS', 10001, 'BP 128/82 - improved. Continue current medication plan.'
);
GO

-- =====================================================
-- Visit 1004 - Consultation visit (Cancelled)
-- =====================================================
INSERT INTO [dbo].[VISIT_MAIN] (
    [VM_COM_COD], [VM_VIS_NUM], [VM_USR_ID], [VM_PIN_NUM],
    [VM_WRK_NAM], [VM_CONTRCT_ID], [VM_CONTRCT_NAM],
    [VM_VIS_DAT], [VM_OTH_HOSP],
    [VM_VIS_SHIFT], [VM_VIS_TYP],
    [VM_ATT_COD], [VM_DOC_COD],
    [VM_PAT_DIA], [VM_TRT_REM], [VM_TST_ADV],
    [VM_MED_GIV], [VM_NXT_REV],
    [VM_ENT_USR], [VM_ENT_NUM], [VM_ENT_DAT],
    [DV_MOD_USR], [VM_MOD_NUM], [VM_MOD_DAT],
    [VM_CAN_FLG], [VM_DIA_CAT], [VM_DIA_SUBCAT], [VM_DOC_REMARKS]
) VALUES (
    '001', 1004, 'EMP003', 100003,
    'Robert Johnson', 'CONT002', 'Contract B',
    '2026-02-20 10:00:00', NULL,
    'M', 'C',
    'ATT003', 'DOC001',
    'Dermatology consultation - skin rash', 'Topical corticosteroid prescribed', 'Allergy patch test recommended',
    'NO',  NULL,
    'ADMIN', 500004, '2026-02-20 10:05:00',
    'ADMIN', 500004, '2026-02-20 11:00:00',
    'Y', 'DRM', 10003, 'Visit cancelled - patient rescheduled to specialist clinic.'
);
GO

-- =====================================================
-- Visit 1005 - Evening shift Routine (Active, different shift)
-- =====================================================
INSERT INTO [dbo].[VISIT_MAIN] (
    [VM_COM_COD], [VM_VIS_NUM], [VM_USR_ID], [VM_PIN_NUM],
    [VM_WRK_NAM], [VM_CONTRCT_ID], [VM_CONTRCT_NAM],
    [VM_VIS_DAT], [VM_OTH_HOSP],
    [VM_VIS_SHIFT], [VM_VIS_TYP],
    [VM_ATT_COD], [VM_DOC_COD],
    [VM_PAT_DIA], [VM_TRT_REM], [VM_TST_ADV],
    [VM_MED_GIV], [VM_NXT_REV],
    [VM_ENT_USR], [VM_ENT_NUM], [VM_ENT_DAT],
    [DV_MOD_USR], [VM_MOD_NUM], [VM_MOD_DAT],
    [VM_CAN_FLG], [VM_DIA_CAT], [VM_DIA_SUBCAT], [VM_DOC_REMARKS]
) VALUES (
    '001', 1005, 'EMP004', 100004,
    'Alice Williams', 'CONT002', 'Contract B',
    '2026-03-05 20:00:00', NULL,
    'E', 'R',
    'ATT001', 'DOC003',
    'Upper respiratory tract infection', 'Antibiotics and decongestants prescribed', 'Chest X-ray if fever persists beyond 3 days',
    'YES', '2026-03-19 09:00:00',
    'ADMIN', 500005, '2026-03-05 20:10:00',
    NULL, NULL, NULL,
    'N', 'ENT', 10004, 'Patient advised rest and increased fluid intake. Reviewed in 2 weeks.'
);
GO

-- =====================================================
-- Seed: VISIT_SUB (Vitals / Test records)
-- =====================================================

-- Visit 1001 sub records (vitals)
INSERT INTO [dbo].[VISIT_SUB] ([VS_COM_COD], [VS_VIS_NUM], [VS_TST_TYP], [VS_TST_VAL], [VS_SRL_NUM])
VALUES
    ('001', 1001, 'BLOOD_PRESSURE',  '148/92',  1),
    ('001', 1001, 'PULSE',           '82',      2),
    ('001', 1001, 'TEMPERATURE',     '37.0',    3),
    ('001', 1001, 'SPO2',            '98',      4),
    ('001', 1001, 'WEIGHT_KG',       '78.5',    5);
GO

-- Visit 1002 sub records (vitals + tests)
INSERT INTO [dbo].[VISIT_SUB] ([VS_COM_COD], [VS_VIS_NUM], [VS_TST_TYP], [VS_TST_VAL], [VS_SRL_NUM])
VALUES
    ('001', 1002, 'BLOOD_PRESSURE',  '90/60',   1),
    ('001', 1002, 'PULSE',           '105',     2),
    ('001', 1002, 'TEMPERATURE',     '38.5',    3),
    ('001', 1002, 'SPO2',            '97',      4),
    ('001', 1002, 'WEIGHT_KG',       '62.0',    5),
    ('001', 1002, 'BLOOD_GLUCOSE',   '4.2',     6);
GO

-- Visit 1003 sub records (follow-up vitals)
INSERT INTO [dbo].[VISIT_SUB] ([VS_COM_COD], [VS_VIS_NUM], [VS_TST_TYP], [VS_TST_VAL], [VS_SRL_NUM])
VALUES
    ('001', 1003, 'BLOOD_PRESSURE',  '128/82',  1),
    ('001', 1003, 'PULSE',           '76',      2),
    ('001', 1003, 'TEMPERATURE',     '36.8',    3),
    ('001', 1003, 'SPO2',            '99',      4),
    ('001', 1003, 'WEIGHT_KG',       '78.0',    5);
GO

-- Visit 1004 sub records (minimal – cancelled visit)
INSERT INTO [dbo].[VISIT_SUB] ([VS_COM_COD], [VS_VIS_NUM], [VS_TST_TYP], [VS_TST_VAL], [VS_SRL_NUM])
VALUES
    ('001', 1004, 'BLOOD_PRESSURE',  '120/78',  1),
    ('001', 1004, 'TEMPERATURE',     '37.2',    2);
GO

-- Visit 1005 sub records (vitals + additional test)
INSERT INTO [dbo].[VISIT_SUB] ([VS_COM_COD], [VS_VIS_NUM], [VS_TST_TYP], [VS_TST_VAL], [VS_SRL_NUM])
VALUES
    ('001', 1005, 'BLOOD_PRESSURE',  '118/76',  1),
    ('001', 1005, 'PULSE',           '88',      2),
    ('001', 1005, 'TEMPERATURE',     '38.1',    3),
    ('001', 1005, 'SPO2',            '96',      4),
    ('001', 1005, 'WEIGHT_KG',       '65.3',    5),
    ('001', 1005, 'THROAT_SWAB',     'POSITIVE',6);
GO

PRINT 'MedicalVisit: Seed data inserted successfully.';
GO
