-- =========================================================================
-- MEMBER MODULE - Master Setup Script
-- Database: PFDB
-- Module: Member Management
-- Description: Execute this script to create all Member module objects
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

PRINT '========================================';
PRINT 'MEMBER MODULE - SETUP STARTED';
PRINT '========================================';
GO

-- Execute table creation
PRINT 'Creating Member module tables...';
GO

-- Execute procedures creation
PRINT 'Creating Member module procedures...';
GO

-- Create additional indexes for optimization
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_MEMBER_AUDIT_LOG_MEMBER')
BEGIN
    CREATE NONCLUSTERED INDEX [IDX_MEMBER_AUDIT_LOG_MEMBER]
    ON [MEMBER_AUDIT_LOG] ([MEMBER_NO], [AUDIT_TIMESTAMP])
    INCLUDE ([AUDIT_ACTION], [AUDIT_USER_ID]);
    PRINT 'Created: IDX_MEMBER_AUDIT_LOG_MEMBER index';
END
GO

-- Create reporting views
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_MemberWithNominee' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_MemberWithNominee AS
    SELECT 
        mm.MEMBER_NO,
        mm.MEMBER_NAME,
        mm.MEMBER_TRUST_CODE,
        mm.MEMBER_DOJ,
        mm.MEMBER_STATUS,
        mn.NOMINEE_NAME,
        mn.NOMINEE_RELATIONSHIP_CODE,
        mn.NOMINEE_PERCENTAGE,
        mn.NOMINEE_FUND_TYPE,
        mn.NOMINEE_STATUS
    FROM dbo.MEMBER_MASTER mm
    LEFT JOIN dbo.MEMBER_NOMINEE mn ON mm.MEMBER_NO = mn.NOMINEE_MEMBER_NO
    WHERE mm.MEMBER_STATUS = 'A';
    PRINT 'Created: vw_MemberWithNominee view';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_MemberGuardianInfo' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_MemberGuardianInfo AS
    SELECT 
        mm.MEMBER_NO,
        mm.MEMBER_NAME,
        mn.NOMINEE_SERIAL_NO,
        mn.NOMINEE_NAME,
        mn.NOMINEE_MINOR_FLAG,
        ng.GAURDIAN_NAME,
        ng.GAURDIAN_RELATIONSHIP,
        ng.GN_ADDRESS_LINE1
    FROM dbo.MEMBER_MASTER mm
    INNER JOIN dbo.MEMBER_NOMINEE mn ON mm.MEMBER_NO = mn.NOMINEE_MEMBER_NO
    LEFT JOIN dbo.NOMINEE_GAURDIAN ng ON mn.NOMINEE_MEMBER_NO = ng.GN_NOMINEE_MEMBER_NO
        AND mn.NOMINEE_SERIAL_NO = ng.GN_NOMINEE_SERIAL_NO
    WHERE mn.NOMINEE_MINOR_FLAG = 'Y';
    PRINT 'Created: vw_MemberGuardianInfo view';
END
GO

PRINT '========================================';
PRINT 'MEMBER MODULE - SETUP COMPLETED';
PRINT '========================================';
GO
