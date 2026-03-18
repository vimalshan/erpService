-- =========================================================================
-- MEMBER MODULE - Stored Procedures and Functions
-- Database: PFDB
-- Module: Member Management
-- Description: Procedures and functions for member profile management
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- =========================================================================
-- PROCEDURE: Create New Member
-- Description: Enrolls a new member in the provident fund
-- Parameters:
--   @p_MemberName: Member Name
--   @p_TrustCode: Trust Code
--   @p_DOJ: Date of Joining
--   @p_EmployeeType: N-New/S-Transfer within SRF/O-Transfer from Outside
--   @p_EmployeeSysID: Employee System ID
--   @p_UnitCode: Payroll Unit Code
--   @p_EmployeeNo: Payroll Employee Number
--   @p_CreatedBy: User creating record
--   @p_MemberNo: New Member Number (OUTPUT)
-- =========================================================================
IF OBJECT_ID('dbo.usp_CreateNewMember', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_CreateNewMember;
GO

CREATE PROCEDURE dbo.usp_CreateNewMember
    @p_MemberName VARCHAR(65),
    @p_TrustCode CHAR(3),
    @p_DOJ DATETIME2(3),
    @p_DOB DATETIME2(3),
    @p_EmployeeType CHAR(2),
    @p_EmployeeSysID BIGINT,
    @p_UnitCode CHAR(3),
    @p_EmployeeNo BIGINT,
    @p_CreatedBy BIGINT,
    @p_MemberNo BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @CurrentMaxMemberNo BIGINT;
        
        -- Validate inputs
        IF @p_MemberName IS NULL OR LEN(RTRIM(@p_MemberName)) = 0
        BEGIN
            THROW 50001, 'Member name cannot be empty', 1;
        END
        
        IF @p_DOJ IS NULL
        BEGIN
            THROW 50002, 'Date of joining is required', 1;
        END
        
        -- Check if member already exists for this employee
        IF EXISTS (SELECT 1 FROM dbo.MEMBER_MASTER WHERE MEMBER_EMP_SYSID = @p_EmployeeSysID AND MEMBER_STATUS = 'A')
        BEGIN
            THROW 50003, 'Member already exists for this employee', 1;
        END
        
        -- Generate new member number
        SELECT @CurrentMaxMemberNo = ISNULL(MAX(MEMBER_NO), 0) FROM dbo.MEMBER_MASTER;
        SET @p_MemberNo = @CurrentMaxMemberNo + 1;
        
        -- Insert new member record
        INSERT INTO dbo.MEMBER_MASTER (
            MEMBER_NO,
            MEMBER_TRUST_CODE,
            MEMBER_FPSTRUST_CODE,
            MEMBER_OPF_NO,
            MEMBER_PENSION_NO,
            MEMBER_NAME,
            MEMBER_ENR_DATE,
            MEMBER_DOJ,
            MEMBER_EMPLOYEE_TYPE,
            MEMBER_ENROLL_USER_ID,
            MEMBER_ENROLL_SYSID,
            MEMBER_ENROLL_DATE,
            MEMBER_UNIT_CODE,
            MEMBER_EMP_NUM,
            MEMBER_EMP_SYSID,
            MEMBER_DOB,
            MEMBER_STATUS,
            MEMBER_UPDATED_BY,
            MEMBER_UPDATED_ON
        ) VALUES (
            @p_MemberNo,
            @p_TrustCode,
            @p_TrustCode,
            CAST(@p_MemberNo AS INT),  -- OPF number same as member no for simplicity
            CAST(@p_MemberNo AS INT),  -- Pension number same as member no for simplicity
            @p_MemberName,
            @ProcessDate,
            @p_DOJ,
            @p_EmployeeType,
            CAST(@p_CreatedBy AS VARCHAR(25)),
            @p_CreatedBy,
            @ProcessDate,
            @p_UnitCode,
            @p_EmployeeNo,
            @p_EmployeeSysID,
            @p_DOB,
            'A',  -- Active status
            @p_CreatedBy,
            @ProcessDate
        );
        
        -- Insert member payroll integration
        INSERT INTO dbo.MEMBER_PAYROLL (
            PAYROLL_MEMBER_NO,
            PAYROLL_UNT_COD,
            PAYROLL_EMP_NUM,
            PAYROLL_EFF_DATE,
            PAYROLL_STATUS
        ) VALUES (
            @p_MemberNo,
            @p_UnitCode,
            @p_EmployeeNo,
            @p_DOJ,
            'A'
        );
        
        -- Audit log
        INSERT INTO dbo.MEMBER_AUDIT_LOG (
            MEMBER_NO,
            AUDIT_ACTION,
            AUDIT_TIMESTAMP,
            AUDIT_USER_ID,
            AUDIT_NEW_VALUES
        ) VALUES (
            @p_MemberNo,
            'INSERT',
            @ProcessDate,
            @p_CreatedBy,
            'New member created: ' + @p_MemberName
        );
        
        COMMIT TRANSACTION;
        
        PRINT 'Member created successfully. Member No: ' + CAST(@p_MemberNo AS VARCHAR);
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =========================================================================
-- PROCEDURE: Add Member Nominee
-- Description: Adds a nominee for a member
-- Parameters:
--   @p_MemberNo: Member Number
--   @p_NomineeSerialNo: Nominee Serial Number
--   @p_NomineeeName: Nominee Name
--   @p_Relationship: Relationship Code
--   @p_Percentage: Nominee percentage
--   @p_FundType: Fund Type
-- =========================================================================
IF OBJECT_ID('dbo.usp_AddMemberNominee', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_AddMemberNominee;
GO

CREATE PROCEDURE dbo.usp_AddMemberNominee
    @p_MemberNo INT,
    @p_SerialNo INT,
    @p_NomineeName VARCHAR(65),
    @p_RelationshipCode CHAR(3),
    @p_Percentage BIGINT,
    @p_DOB DATETIME2(3),
    @p_FundType CHAR(3),
    @p_MinorFlag CHAR(1),
    @p_CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @TrustCode CHAR(3);
        
        -- Get trust code from member
        SELECT @TrustCode = MEMBER_TRUST_CODE FROM dbo.MEMBER_MASTER WHERE MEMBER_NO = @p_MemberNo;
        
        IF @TrustCode IS NULL
        BEGIN
            THROW 50004, 'Member not found', 1;
        END
        
        -- Validate percentage
        IF @p_Percentage < 0 OR @p_Percentage > 100
        BEGIN
            THROW 50005, 'Nominee percentage must be between 0 and 100', 1;
        END
        
        -- Check if nominee already exists
        IF EXISTS (SELECT 1 FROM dbo.MEMBER_NOMINEE 
                   WHERE NOMINEE_MEMBER_NO = @p_MemberNo 
                   AND NOMINEE_SERIAL_NO = @p_SerialNo 
                   AND NOMINEE_FUND_TYPE = @p_FundType)
        BEGIN
            THROW 50006, 'Nominee already exists', 1;
        END
        
        -- Insert nominee record
        INSERT INTO dbo.MEMBER_NOMINEE (
            NOMINEE_MEMBER_NO,
            NOMINEE_SERIAL_NO,
            NOMINEE_FUND_TYPE,
            NOMINEE_NAME,
            NOMINEE_RELATIONSHIP_CODE,
            NOMINEE_PERCENTAGE,
            NOMINEE_DOB,
            NOMINEE_EFF_DATE,
            NOMINEE_MINOR_FLAG,
            NOMINEE_TRUST_CODE,
            NOMINEE_STATUS
        ) VALUES (
            @p_MemberNo,
            @p_SerialNo,
            @p_FundType,
            @p_NomineeName,
            @p_RelationshipCode,
            @p_Percentage,
            @p_DOB,
            @ProcessDate,
            @p_MinorFlag,
            @TrustCode,
            'A'
        );
        
        -- Audit log
        INSERT INTO dbo.MEMBER_AUDIT_LOG (
            MEMBER_NO,
            AUDIT_ACTION,
            AUDIT_TIMESTAMP,
            AUDIT_USER_ID,
            AUDIT_NEW_VALUES
        ) VALUES (
            @p_MemberNo,
            'UPDATE',
            @ProcessDate,
            @p_CreatedBy,
            'Nominee added: ' + @p_NomineeName + ' (' + CAST(@p_Percentage AS VARCHAR) + '%)'
        );
        
        COMMIT TRANSACTION;
        
        PRINT 'Nominee added successfully.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =========================================================================
-- PROCEDURE: Close Member Account
-- Description: Closes a member's PF account
-- Parameters:
--   @p_MemberNo: Member Number
--   @p_ReasonCode: Reason for closure
--   @p_ClosureDate: Date of closure
--   @p_ApprovedBy: User approving closure
-- =========================================================================
IF OBJECT_ID('dbo.usp_CloseMemberAccount', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_CloseMemberAccount;
GO

CREATE PROCEDURE dbo.usp_CloseMemberAccount
    @p_MemberNo BIGINT,
    @p_LeaveReason VARCHAR(200),
    @p_LeaveDate DATETIME2(3),
    @p_ApprovedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        
        -- Validate member exists
        IF NOT EXISTS (SELECT 1 FROM dbo.MEMBER_MASTER WHERE MEMBER_NO = @p_MemberNo)
        BEGIN
            THROW 50007, 'Member not found', 1;
        END
        
        -- Check if already closed
        IF EXISTS (SELECT 1 FROM dbo.MEMBER_MASTER WHERE MEMBER_NO = @p_MemberNo AND MEMBER_STATUS = 'C')
        BEGIN
            THROW 50008, 'Member account already closed', 1;
        END
        
        -- Update member status
        UPDATE dbo.MEMBER_MASTER
        SET 
            MEMBER_CLOSURE_DATE = @ProcessDate,
            MEMBER_LEAVE_DATE = @p_LeaveDate,
            MEMBER_LEAVE_REASON = @p_LeaveReason,
            MEMBER_STATUS = 'C',
            MEMBER_UPDATED_BY = @p_ApprovedBy,
            MEMBER_UPDATED_ON = @ProcessDate
        WHERE MEMBER_NO = @p_MemberNo;
        
        -- Close active payroll records
        UPDATE dbo.MEMBER_PAYROLL
        SET 
            PAYROLL_CLS_DATE = @p_LeaveDate,
            PAYROLL_STATUS = 'C'
        WHERE PAYROLL_MEMBER_NO = @p_MemberNo AND PAYROLL_STATUS = 'A';
        
        -- Audit log
        INSERT INTO dbo.MEMBER_AUDIT_LOG (
            MEMBER_NO,
            AUDIT_ACTION,
            AUDIT_TIMESTAMP,
            AUDIT_USER_ID,
            AUDIT_NEW_VALUES
        ) VALUES (
            @p_MemberNo,
            'UPDATE',
            @ProcessDate,
            @p_ApprovedBy,
            'Member account closed. Reason: ' + @p_LeaveReason
        );
        
        COMMIT TRANSACTION;
        
        PRINT 'Member account closed successfully.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =========================================================================
-- PROCEDURE: Get Member Profile
-- Description: Retrieves complete member profile with nominees
-- Parameters:
--   @p_MemberNo: Member Number
-- =========================================================================
IF OBJECT_ID('dbo.usp_GetMemberProfile', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_GetMemberProfile;
GO

CREATE PROCEDURE dbo.usp_GetMemberProfile
    @p_MemberNo BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Member basic info
    SELECT 
        'MEMBER_PROFILE' AS RecordType,
        MEMBER_NO,
        MEMBER_NAME,
        MEMBER_TRUST_CODE,
        MEMBER_DOJ,
        MEMBER_DOB,
        MEMBER_STATUS,
        MEMBER_EMP_SYSID,
        MEMBER_UNIT_CODE,
        MEMBER_EMP_NUM
    FROM dbo.MEMBER_MASTER
    WHERE MEMBER_NO = @p_MemberNo;
    
    -- Nominees info
    SELECT 
        'NOMINEES' AS RecordType,
        NOMINEE_SERIAL_NO AS SerialNo,
        NOMINEE_NAME,
        NOMINEE_RELATIONSHIP_CODE,
        NOMINEE_PERCENTAGE,
        NOMINEE_FUND_TYPE,
        NOMINEE_STATUS
    FROM dbo.MEMBER_NOMINEE
    WHERE NOMINEE_MEMBER_NO = @p_MemberNo
    AND NOMINEE_STATUS = 'A'
    ORDER BY NOMINEE_SERIAL_NO;
    
    -- Contact information
    SELECT 
        'CONTACT' AS RecordType,
        CONTACT_TYPE,
        ADDRESS_LINE_1,
        EMAIL,
        PHONE_NO,
        EFF_DATE
    FROM dbo.MEMBER_CONTACT
    WHERE MEMBER_NO = @p_MemberNo
    AND (CLS_DATE IS NULL OR CLS_DATE > GETDATE())
    ORDER BY CONTACT_TYPE;
END;
GO

-- =========================================================================
-- VIEW: Active Members
-- =========================================================================
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_ActiveMembers' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_ActiveMembers AS
    SELECT 
        mm.MEMBER_NO,
        mm.MEMBER_NAME,
        mm.MEMBER_TRUST_CODE,
        mm.MEMBER_DOJ,
        mm.MEMBER_UNIT_CODE,
        mm.MEMBER_STATUS,
        mp.PAYROLL_EMP_NUM,
        mp.PAYROLL_STATUS,
        COUNT(mn.NOMINEE_SERIAL_NO) AS NomineeCount
    FROM dbo.MEMBER_MASTER mm
    LEFT JOIN dbo.MEMBER_PAYROLL mp ON mm.MEMBER_NO = mp.PAYROLL_MEMBER_NO
    LEFT JOIN dbo.MEMBER_NOMINEE mn ON mm.MEMBER_NO = mn.NOMINEE_MEMBER_NO
    WHERE mm.MEMBER_STATUS = 'A'
    GROUP BY 
        mm.MEMBER_NO,
        mm.MEMBER_NAME,
        mm.MEMBER_TRUST_CODE,
        mm.MEMBER_DOJ,
        mm.MEMBER_UNIT_CODE,
        mm.MEMBER_STATUS,
        mp.PAYROLL_EMP_NUM,
        mp.PAYROLL_STATUS;
END
GO

PRINT 'Member Module Procedures created successfully!';
GO
