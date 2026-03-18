-- ==========================================
-- Module: LOAN_APPLICATION
-- Database: LOANDB
-- Description: Loan Applications and Approvals
-- ==========================================

USE [LOANDB];
GO

-- Table: LOAN_APPLICATION - LOAN_APPLICATION
CREATE TABLE [LOAN_APPLICATION] (
    [LOAN_APPID] BIGINT NOT NULL  -- Loan Application ID,
    [LOAN_EMPSYSID] BIGINT NOT NULL  -- Employee System ID,
    [LOAN_ID] BIGINT NOT NULL  -- Loan ID,
    [LOAN_APPLIEDBY] BIGINT NOT NULL  -- Loan Applied By,
    [LOAN_APPLIEDON] DATETIME2(3) NOT NULL  -- Loan Applied On,
    [LOAN_SOURCE] CHAR(3) NOT NULL  -- DIR/SLF,
    [LOAN_AMOUNT] BIGINT NOT NULL  -- Loan Application Amount,
    [LOAN_SUBCLASSID] BIGINT NULL  -- Loan Sub class ID,
    [LOAN_REASON] VARCHAR(200) NOT NULL  -- Reason for Loan,
    [LOAN_APPSTATUS] CHAR(1) NOT NULL  -- P - Applied ; A - Approved ; R - Rejected ; C - Created,
    [LOAN_GUARANTOR] BIGINT NOT NULL  -- Co Worker Employee System ID,
    [LOAN_APRREMARKS] VARCHAR(200) NULL  -- Approver Remarks,
    [LOAN_REQUIREDBY] BIGINT NOT NULL  -- Loan required by,
    [LOAN_APPROVEDBY] BIGINT NULL  -- Loan Approved By,
    [LOAN_APPROVEDON] DATETIME2(3) NULL  -- Loan Approved On,
    [LOAN_MODIFIEDBY] BIGINT NOT NULL  -- Modified By,
    [LOAN_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    [LOAN_TENURE] BIGINT NULL,
    [LOAN_GUARANTOR2] BIGINT NULL,
    [LOAN_SPLSANCTION] CHAR(1) NULL,
    CONSTRAINT [PK_LOAN_APPLICATION] PRIMARY KEY ([LOAN_APPID])
);
GO

-- Table: LOAN_ADDITIONAL - loan additional
CREATE TABLE [LOAN_ADDITIONAL] (
    [LOAN_EMPSYSID] BIGINT NOT NULL  -- Employee System ID,
    [ADDL_LOANNO] BIGINT NOT NULL  -- Additional LoanNo,
    [ADDL_LOANID] BIGINT NOT NULL  -- Additional LoanID
);
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_GetLoanEligibility
-- Purpose:  Check if employee is eligible for loan
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetLoanEligibility
(
    @p_EmpSysID BIGINT,
    @p_LoanTypeID BIGINT
)
RETURNS BIT
AS
BEGIN
    DECLARE @IsEligible BIT = 0;
    DECLARE @ActiveLoans INT;
    DECLARE @ServiceYears INT;
    
    -- Check service years (minimum 1 year)
    SELECT @ServiceYears = DATEDIFF(YEAR, EMP_DOJ, GETDATE())
    FROM HRDB.dbo.EMPLOYEE_MASTER
    WHERE EMP_SYSID = @p_EmpSysID;
    
    -- Check active loans (max 2)
    SELECT @ActiveLoans = COUNT(*)
    FROM LOAN_APPLICATION
    WHERE LOAN_EMPSYSID = @p_EmpSysID
      AND LOAN_APPSTATUS IN ('A', 'D');  -- Approved or Disbursed
    
    -- Eligibility check
    IF @ServiceYears >= 1 AND @ActiveLoans < 2
        SET @IsEligible = 1;
    
    RETURN @IsEligible;
END;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_ApplyForLoan
-- Purpose:  Create new loan application
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ApplyForLoan
(
    @p_EmpSysID BIGINT,
    @p_LoanID BIGINT,
    @p_LoanAmount BIGINT,
    @p_LoanReason VARCHAR(200),
    @p_GuarantorEmpSysID BIGINT,
    @p_TenureMonths INT,
    @p_AppliedBy BIGINT,
    @p_LoanAppID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check eligibility
        IF dbo.fn_GetLoanEligibility(@p_EmpSysID, @p_LoanID) = 0
            THROW 50001, 'Employee not eligible for this loan', 1;
        
        -- Generate application ID
        SELECT @p_LoanAppID = ISNULL(MAX(LOAN_APPID), 0) + 1 FROM LOAN_APPLICATION;
        
        -- Create application
        INSERT INTO LOAN_APPLICATION
        (
            LOAN_APPID, LOAN_EMPSYSID, LOAN_ID, LOAN_APPLIEDBY, LOAN_APPLIEDON,
            LOAN_SOURCE, LOAN_AMOUNT, LOAN_REASON, LOAN_APPSTATUS,
            LOAN_GUARANTOR, LOAN_TENURE, LOAN_MODIFIEDBY, LOAN_MODIFIEDON, LOAN_REQUIREDBY
        )
        VALUES
        (
            @p_LoanAppID, @p_EmpSysID, @p_LoanID, @p_AppliedBy, GETDATE(),
            'SLF', @p_LoanAmount, @p_LoanReason, 'C',  -- C = Created, SLF = Self Loan
            @p_GuarantorEmpSysID, @p_TenureMonths, @p_AppliedBy, GETDATE(), @p_EmpSysID
        );
        
        COMMIT TRANSACTION;
        PRINT 'Loan application created: ID = ' + CAST(@p_LoanAppID AS VARCHAR) + ', Amount = ₹' + CAST(@p_LoanAmount AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Loan application failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- TRIGGERS
-- ==========================================

-- ------------------------------------------------------------------
-- Trigger: trg_LoanApplication_ValidateGuarantor
-- Purpose:  Validate guarantor exists and is different from applicant
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_LoanApplication_ValidateGuarantor
ON dbo.LOAN_APPLICATION
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @EmpSysID BIGINT;
    DECLARE @GuarantorEmpSysID BIGINT;
    
    SELECT TOP 1 @EmpSysID = LOAN_EMPSYSID, @GuarantorEmpSysID = LOAN_GUARANTOR FROM inserted;
    
    -- Validate guarantor is different from applicant
    IF @EmpSysID = @GuarantorEmpSysID
    BEGIN
        RAISERROR('Guarantor cannot be the same as applicant', 16, 1);
        RETURN;
    END
    
    -- Proceed with insert
    INSERT INTO LOAN_APPLICATION
    SELECT * FROM inserted;
END;
GO
