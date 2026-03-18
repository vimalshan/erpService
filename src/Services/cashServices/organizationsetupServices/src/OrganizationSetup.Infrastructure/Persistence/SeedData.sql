-- Organization Setup Service - Seed Data Script
-- For CASHDB database
-- Run after applying migrations

USE CASHDB;
GO

-- =====================================================
-- SEED DATA: DEAL_ROLE
-- =====================================================
SET IDENTITY_INSERT [DEAL_ROLE] ON;

INSERT INTO [DEAL_ROLE] ([ROLE_ID], [ROLE_NAME], [ROLE_LEVEL], [ROLE_MODIFIEDBY], [ROLE_MODIFIEDON])
VALUES
    (1, 'Treasury Manager', 1, 1, GETDATE()),
    (2, 'Dealer', 2, 1, GETDATE()),
    (3, 'Approver', 2, 1, GETDATE()),
    (4, 'Accountant', 3, 1, GETDATE()),
    (5, 'Data Entry', 4, 1, GETDATE()),
    (6, 'Compliance Officer', 2, 1, GETDATE()),
    (7, 'Operations Manager', 3, 1, GETDATE());

SET IDENTITY_INSERT [DEAL_ROLE] OFF;
PRINT 'DEAL_ROLE seeded: 7 roles';

-- =====================================================
-- SEED DATA: DEAL_USERMAP
-- =====================================================
SET IDENTITY_INSERT [DEAL_USERMAP] ON;

INSERT INTO [DEAL_USERMAP] ([ROLE_MAPID], [ROLE_ID], [ROLE_EMPSYSID], [ROLE_ORGID], [ROLE_BUSINESS])
VALUES
    (1, 1, 1001, 100, NULL),        -- User 1001 = Treasury Manager in Org 100 (Head Office)
    (2, 2, 1002, 100, 10),          -- User 1002 = Dealer in Org 100, Business Unit 10
    (3, 3, 1003, 100, 10),          -- User 1003 = Approver in Org 100, Business Unit 10
    (4, 4, 1004, 100, NULL),        -- User 1004 = Accountant in Org 100
    (5, 5, 1005, 100, 20),          -- User 1005 = Data Entry in Org 100, Business Unit 20
    (6, 2, 1006, 200, 30),          -- User 1006 = Dealer in Org 200, Business Unit 30
    (7, 1, 1007, 200, NULL),        -- User 1007 = Treasury Manager in Org 200
    (8, 6, 1008, 100, NULL),        -- User 1008 = Compliance Officer in Org 100
    (9, 7, 1009, 100, NULL);        -- User 1009 = Operations Manager in Org 100

SET IDENTITY_INSERT [DEAL_USERMAP] OFF;
PRINT 'DEAL_USERMAP seeded: 9 user mappings';

-- =====================================================
-- SEED DATA: DEAL_ORGPARAMS
-- =====================================================
SET IDENTITY_INSERT [DEAL_ORGPARAMS] ON;

INSERT INTO [DEAL_ORGPARAMS] ([ORG_PARAMID], [ORG_PARAMTYPE], [ORG_PARAMVALUE], [ORG_ID], [ORG_MODIFIEDBY], [ORG_MODIFIEDON])
VALUES
    -- Organization 100 Parameters
    (1, 'MAXDEAL', 10000000, 100, 1, GETDATE()),     -- Max 10 Crores per deal
    (2, 'MAXEXP', 50000000, 100, 1, GETDATE()),      -- Max 50 Crores exposure
    (3, 'MINAPP', 1000000, 100, 1, GETDATE()),       -- Min 1 Crore for approval
    (4, 'REPFRQ', 7, 100, 1, GETDATE()),             -- Weekly reporting
    (5, 'FISYEAR', 4, 100, 1, GETDATE()),            -- FY starts April
    (6, 'BASECUR', 1, 100, 1, GETDATE()),            -- USD base currency
    
    -- Organization 200 Parameters
    (7, 'MAXDEAL', 15000000, 200, 1, GETDATE()),     -- Max 15 Crores per deal
    (8, 'MAXEXP', 75000000, 200, 1, GETDATE()),      -- Max 75 Crores exposure
    (9, 'MINAPP', 2000000, 200, 1, GETDATE()),       -- Min 2 Crores for approval
    (10, 'REPFRQ', 7, 200, 1, GETDATE()),            -- Weekly reporting
    (11, 'FISYEAR', 4, 200, 1, GETDATE()),           -- FY starts April
    (12, 'BASECUR', 1, 200, 1, GETDATE());           -- USD base currency

SET IDENTITY_INSERT [DEAL_ORGPARAMS] OFF;
PRINT 'DEAL_ORGPARAMS seeded: 12 parameters';

-- =====================================================
-- SEED DATA: DEAL_PPLIMIT
-- =====================================================
SET IDENTITY_INSERT [DEAL_PPLIMIT] ON;

INSERT INTO [DEAL_PPLIMIT] ([PP_LIMITID], [PP_ORGID], [PP_TRANTYPE], [PP_BASCURR], [PP_LIMITAMT], [PP_FINYEAR], [PP_LIMITACT], [PP_CERTIFICATEUPLOAD], [PP_MODIFIEDBY], [PP_MODIFIEDON])
VALUES
    -- Organization 100, FY 2026
    (1, 100, 'I', 1, 100000000, 2026, 25000000, NULL, 1, GETDATE()),    -- Import: 100 Cr limit, 25 Cr utilized
    (2, 100, 'E', 1, 150000000, 2026, 75000000, NULL, 1, GETDATE()),    -- Export: 150 Cr limit, 75 Cr utilized
    
    -- Organization 100, FY 2025
    (3, 100, 'I', 1, 100000000, 2025, 95000000, NULL, 1, GETDATE()),    -- Import FY25: 100 Cr limit, 95 Cr utilized
    (4, 100, 'E', 1, 150000000, 2025, 140000000, NULL, 1, GETDATE()),   -- Export FY25: 150 Cr limit, 140 Cr utilized
    
    -- Organization 200, FY 2026
    (5, 200, 'I', 1, 200000000, 2026, 50000000, NULL, 1, GETDATE()),    -- Import: 200 Cr limit, 50 Cr utilized
    (6, 200, 'E', 1, 250000000, 2026, 100000000, NULL, 1, GETDATE());   -- Export: 250 Cr limit, 100 Cr utilized

SET IDENTITY_INSERT [DEAL_PPLIMIT] OFF;
PRINT 'DEAL_PPLIMIT seeded: 6 PP limits';

-- =====================================================
-- VERIFY DATA
-- =====================================================
PRINT '';
PRINT '===== VERIFICATION =====';
PRINT '';

SELECT 'DEAL_ROLE' as TableName, COUNT(*) as RecordCount FROM [DEAL_ROLE]
UNION ALL
SELECT 'DEAL_USERMAP', COUNT(*) FROM [DEAL_USERMAP]
UNION ALL
SELECT 'DEAL_ORGPARAMS', COUNT(*) FROM [DEAL_ORGPARAMS]
UNION ALL
SELECT 'DEAL_PPLIMIT', COUNT(*) FROM [DEAL_PPLIMIT];

PRINT '';
PRINT 'Seed data inserted successfully!';
GO
