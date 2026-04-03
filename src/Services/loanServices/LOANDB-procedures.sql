-- ==========================================
-- Database: LOANDB
-- Stored Procedures, Functions, Triggers
-- Loan & Advance Management
-- ==========================================

USE [LOANDB];
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_GetEMIAmount
-- Purpose:  Calculate EMI (Equated Monthly Installment)
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_GetEMIAmount
(
    @p_PrincipalAmount BIGINT,
    @p_RatePerAnnum INT,  -- Interest rate percentage
    @p_TenureMonths INT
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @MonthlyRate DECIMAL(8,6);
    DECLARE @EMI BIGINT;
    
    -- Convert annual rate to monthly rate
    SET @MonthlyRate = CAST(@p_RatePerAnnum AS DECIMAL(8,6)) / 12 / 100;
    
    -- EMI = P * [r(1+r)^n]/[(1+r)^n-1]
    IF @MonthlyRate > 0
    BEGIN
        SET @EMI = CAST(
            @p_PrincipalAmount * (@MonthlyRate * POWER(1 + @MonthlyRate, @p_TenureMonths)) / 
            (POWER(1 + @MonthlyRate, @p_TenureMonths) - 1)
        AS BIGINT);
    END
    ELSE
    BEGIN
        -- Simple division if rate is 0
        SET @EMI = CAST(@p_PrincipalAmount / @p_TenureMonths AS BIGINT);
    END
    
    RETURN ISNULL(@EMI, 0);
END;
GO

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
            LOAN_GUARANTOR, LOAN_TENURE, LOAN_MODIFIEDBY, LOAN_MODIFIEDON
        )
        VALUES
        (
            @p_LoanAppID, @p_EmpSysID, @p_LoanID, @p_AppliedBy, GETDATE(),
            'SLF', @p_LoanAmount, @p_LoanReason, 'C',  -- C = Created, SLF = Self Loan
            @p_GuarantorEmpSysID, @p_TenureMonths, @p_AppliedBy, GETDATE()
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

-- ------------------------------------------------------------------
-- Procedure: usp_ApproveLoanApplication
-- Purpose:  Approve loan and create EMI schedule
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ApproveLoanApplication
(
    @p_LoanAppID BIGINT,
    @p_InterestRate INT,
    @p_ApprovedBy BIGINT,
    @p_ApprovalRemarks VARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @LoanID BIGINT;
        DECLARE @Amount BIGINT;
        DECLARE @EmpSysID BIGINT;
        DECLARE @Tenure INT;
        DECLARE @EMIAmount BIGINT;
        DECLARE @Counter INT = 1;
        DECLARE @RateID BIGINT;
        
        -- Get loan details
        SELECT @LoanID = LOAN_ID, @Amount = LOAN_AMOUNT, @EmpSysID = LOAN_EMPSYSID, @Tenure = LOAN_TENURE
        FROM LOAN_APPLICATION
        WHERE LOAN_APPID = @p_LoanAppID;
        
        IF @LoanID IS NULL
            THROW 50001, 'Loan application not found', 1;
        
        -- Calculate EMI
        SET @EMIAmount = dbo.fn_GetEMIAmount(@Amount, @p_InterestRate, @Tenure);
        
        -- Set interest rate master
        SELECT @RateID = ISNULL(MAX(LOANINT_RATEID), 0) + 1 FROM LOAN_EMPINTRATEMAST;
        
        INSERT INTO LOAN_EMPINTRATEMAST
        (
            LOANINT_RATEID, LOANINT_LOANNO, LOANINT_EFFDATE, LOANINT_RATE,
            LOANINT_EMIAMT, LOANINT_INSNOS, LOANINT_LASTMODIFIEDBY, LOANINT_LASTMODIFIEDON
        )
        VALUES
        (
            @RateID, @LoanID, GETDATE(), @p_InterestRate,
            @EMIAmount, @Tenure, @p_ApprovedBy, GETDATE()
        );
        
        -- Create EMI schedule
        WHILE @Counter <= @Tenure
        BEGIN
            DECLARE @InstalmentID BIGINT;
            SELECT @InstalmentID = ISNULL(MAX(LOANINS_ID), 0) + 1 FROM LOAN_INS;
            
            INSERT INTO LOAN_INS
            (
                LOANINS_ID, LOANINS_UNITID, LOANINS_LOANNO, LOANINS_INSDATE,
                LOANINS_INSNO, LOANINS_INSAMT, LOANINS_PRNOUT, LOANINS_PRNADJ,
                LOANINS_INTADJ, LOANINS_INTACC, LOANINS_INTRATE, LOANINS_INTREC, LOANINS_PRNREC,
                LOANINS_UPDATEDBY, LOANINS_UPDATEDON
            )
            VALUES
            (
                @InstalmentID, 1, @LoanID, DATEADD(MONTH, @Counter, GETDATE()),
                @Counter, @EMIAmount, (@Amount - (@Counter * @EMIAmount)), 0,
                0, 0, @p_InterestRate, 0, 0,
                @p_ApprovedBy, GETDATE()
            );
            
            SET @Counter = @Counter + 1;
        END
        
        -- Update application status
        UPDATE LOAN_APPLICATION
        SET LOAN_APPSTATUS = 'A',  -- A = Approved
            LOAN_APPROVEDBY = @p_ApprovedBy,
            LOAN_APPROVEDON = GETDATE(),
            LOAN_APRREMARKS = @p_ApprovalRemarks
        WHERE LOAN_APPID = @p_LoanAppID;
        
        COMMIT TRANSACTION;
        PRINT 'Loan approved: Amount = ₹' + CAST(@Amount AS VARCHAR) + ', EMI = ₹' + CAST(@EMIAmount AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Loan approval failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_RecordEMIPayment
-- Purpose:  Record EMI payment and update loan status
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_RecordEMIPayment
(
    @p_InstalmentID BIGINT,
    @p_PrincipalPaid BIGINT,
    @p_InterestPaid BIGINT,
    @p_PaymentDate DATETIME2(3) = NULL,
    @p_PaidBy BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Update instalment
        UPDATE LOAN_INS
        SET LOANINS_PRNADJ = @p_PrincipalPaid,
            LOANINS_INTADJ = @p_InterestPaid,
            LOANINS_INTREC = @p_InterestPaid,
            LOANINS_PRNREC = @p_PrincipalPaid,
            LOANINS_UPDATEDBY = @p_PaidBy,
            LOANINS_UPDATEDON = ISNULL(@p_PaymentDate, GETDATE())
        WHERE LOANINS_ID = @p_InstalmentID;
        
        COMMIT TRANSACTION;
        PRINT 'EMI payment recorded: Principal = ₹' + CAST(@p_PrincipalPaid AS VARCHAR) + 
              ', Interest = ₹' + CAST(@p_InterestPaid AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('EMI payment recording failed: %s', 16, 1, ERROR_MESSAGE());
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

-- ==========================================
-- END OF SCRIPT
-- ==========================================
