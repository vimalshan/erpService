-- ==========================================
-- LeaveServices Seed Data Script
-- Run AFTER InitialCreate migration
-- ==========================================
USE LEAVEDB;
GO

-- ── Leave Master (leave types) ────────────────────────────────────────────────
SET IDENTITY_INSERT [LEAVE_MASTER] OFF;

INSERT INTO [LEAVE_MASTER]
    (LEAVE_ID, LEAVE_DESCRIPTION, LEAVE_GENDERSPECIFIC, LEAVE_APPLICABLEFORALL,
     LEAVE_MAXDAYSPL, LEAVE_ENCASHABLE, LEAVE_CARRYFORWARD,
     LEAVE_LASTMODIFIEDBY, LEAVE_LASTMODIFIEDON)
VALUES
    (1,  'Casual Leave',           'B', 'Y', 12, 'N', 'N', 1, GETDATE()),
    (2,  'Sick / Medical Leave',   'B', 'Y', 12, 'N', 'N', 1, GETDATE()),
    (3,  'Privilege Leave',        'B', 'Y', 30, 'Y', 'Y', 1, GETDATE()),
    (4,  'Maternity Leave',        'F', 'N', 84, 'N', 'N', 1, GETDATE()),
    (5,  'Paternity Leave',        'M', 'N', 15, 'N', 'N', 1, GETDATE()),
    (6,  'Compensatory Off',       'B', 'Y',  0, 'N', 'N', 1, GETDATE()),
    (7,  'Loss of Pay',            'B', 'Y',  0, 'N', 'N', 1, GETDATE()),
    (8,  'Study / Exam Leave',     'B', 'Y',  5, 'N', 'N', 1, GETDATE());
GO

-- ── Leave Rules ────────────────────────────────────────────────────────────────
INSERT INTO [LEAVE_RULES]
    (RULE_ID, RULE_LEAVEID, RULE_MAXDAYSINAPPL, RULE_MINDAYSINAPPL,
     RULE_MAXYEARLIMIT, RULE_CLUBBING, RULE_LASTMODIFIEDBY, RULE_LASTMODIFIEDON)
VALUES
    (1, 1, 3, 1, 12, 'N', 1, GETDATE()),   -- Casual Leave
    (2, 2, 3, 1, 12, 'N', 1, GETDATE()),   -- Sick Leave
    (3, 3, 15, 1, 30, 'Y', 1, GETDATE()),  -- Privilege Leave
    (4, 6, 1,  1,  0, 'N', 1, GETDATE()); -- Comp-Off
GO

-- ── Sample Leave Credits for demo employee (EmpSysID = 1001) ─────────────────
INSERT INTO [LEAVE_CREDIT]
    (CREDIT_ID, CREDIT_EMPSYSID, CREDIT_LEAVEID, CREDIT_LEAVEFLAG, CREDIT_YEAR,
     CREDIT_OPENING, CREDIT_CREDITED, CREDIT_UTILIZED, CREDIT_CLOSING,
     CREDIT_LASTMODIFIEDBY, CREDIT_LASTMODIFIEDON)
VALUES
    (1, 1001, 1, 'A', YEAR(GETDATE()), 0, 12, 0, 12, 1, GETDATE()),
    (2, 1001, 2, 'A', YEAR(GETDATE()), 0, 12, 0, 12, 1, GETDATE()),
    (3, 1001, 3, 'A', YEAR(GETDATE()), 5, 18, 0, 23, 1, GETDATE());
GO

PRINT '=== Seed data inserted successfully ===';
GO
