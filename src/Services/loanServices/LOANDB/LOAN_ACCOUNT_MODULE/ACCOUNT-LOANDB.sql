-- ==========================================
-- Module: LOAN_ACCOUNT
-- Database: LOANDB
-- Description: Active Loans, Installments, Ledger, and Settlements
-- ==========================================

USE [LOANDB];
GO

-- Table: LOAN_MAIN - Employee Loan Main
CREATE TABLE [LOAN_MAIN] (
    [LOAN_NO] BIGINT NOT NULL  -- Loan No,
    [LOAN_APPID] BIGINT NOT NULL  -- Loan Application ID,
    [LOAN_EMPSYSID] BIGINT NOT NULL  -- Employee System ID,
    [LOAN_ID] BIGINT NOT NULL  -- Loan ID,
    [LOAN_DISBTYPE] CHAR(3) NOT NULL  -- NEW/ADJ ( New Loan / Adjust Loan Amount against old loan),
    [LOAN_GRADEID] BIGINT NOT NULL  -- Employee Grade ID - While applying the loan,
    [LOAN_PRNAMT] DECIMAL(19,0) NOT NULL  -- Loan Principal Amount,
    [LOAN_OLDPRNADJ] DECIMAL(19,0) NOT NULL  -- Principal Adjustment against Old Loan,
    [LOAN_PAID] DECIMAL(19,0) NOT NULL  -- Loan Amount Disbursed,
    [LOAN_FIRSTINSDATE] DATETIME2(3) NOT NULL  -- Loan first installment date,
    [LOAN_PRNOUT] DECIMAL(19,0) NOT NULL  -- Princial Outstanding,
    [LOAN_DATE] DATETIME2(3) NOT NULL  -- Loan Effective Date,
    [LOAN_CLSDATE] DATETIME2(3) NULL  -- Loan Closure Date,
    [LOAN_UNITID] BIGINT NOT NULL  -- Unit ID - Where Loan applied,
    [LOAN_SUBCLASSID] BIGINT NOT NULL  -- Loan Sub class ID,
    [LOAN_REASON] VARCHAR(200) NOT NULL  -- Reason for Loan,
    [LOAN_GUARANTOR] BIGINT NOT NULL  -- Co Worker Employee System ID,
    [LOAN_APRREMARKS] VARCHAR(200) NULL  -- Approver Remarks,
    [LOAN_CLOSURETYPE] CHAR(3) NOT NULL  -- (SET/WOF/ADJ/LIV ) Settled ; Written Off ; Adjusted against new loan,
    [LOAN_NEWLOANNO] BIGINT NOT NULL  -- New Loan  No  In case of principal adjustment against new loan,
    [LOAN_EMPINTRATE] CHAR(1) NOT NULL  -- Employee Wise interest rates applicable (Y/N),
    [LOAN_COMFACTOR] CHAR(1) NOT NULL  -- Compounding Factor (Only for Employee Specific Loans),
    [LOAN_INTFREQUENCY] CHAR(1) NOT NULL  -- Interest Frequency  (Only for Employee Specific Loans),
    [LOAN_RECTYPE] CHAR(3) NOT NULL  -- Recovery Method (RBM, EM1,EMA,FPI) (Only for Employee Specific Loans),
    [LOAN_CREATEDBY] BIGINT NOT NULL  -- Created By,
    [LOAN_CREATEDON] DATETIME2(3) NOT NULL  -- Created On,
    [LOAN_MODIFIEDBY] BIGINT NOT NULL  -- Modified By,
    [LOAN_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    [LOAN_LASTINSDATE] DATETIME2(3) NOT NULL  -- Loan Last Instalment Date,
    [LOAN_AMTEDID] BIGINT NOT NULL  -- Loan Disbursement ED ID,
    [LOAN_PRNEDID] BIGINT NOT NULL  -- Loan Principal Recovery ED ID,
    [LOAN_INTEDID] BIGINT NOT NULL  -- Loan Interest Recovery ED ID,
    [LOAN_EMPINSNOS] INT NULL  -- Loan Specific Reduced Tenure Nos,
    [LOAN_EMPINSAMT] DECIMAL(19,0) NULL  -- Loan Specific Increased Installment,
    CONSTRAINT [PK_LOAN_MAIN] PRIMARY KEY ([LOAN_NO])
);
GO

-- Table: LOAN_EMPINTRATEMAST - Employee Wise Loan Interest Rate Master
CREATE TABLE [LOAN_EMPINTRATEMAST] (
    [LOANINT_RATEID] BIGINT NOT NULL  -- Interest Rate ID,
    [LOANINT_LOANNO] BIGINT NOT NULL  -- Loan No,
    [LOANINT_EFFDATE] DATETIME2(3) NOT NULL  -- Effective Date,
    [LOANINT_CLSDATE] DATETIME2(3) NULL  -- Closure Date,
    [LOANINT_RATE] INT NOT NULL  -- Interest Rate (%),
    [LOANINT_LASTMODIFIEDBY] BIGINT NOT NULL  -- Last Modified By,
    [LOANINT_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Last Modified On,
    [LOANINT_EMIAMT] BIGINT NOT NULL  -- EMI Amount/Instalment Amount,
    [LOANINT_INSNOS] INT NOT NULL  -- No of Installments,
    CONSTRAINT [PK_LOAN_EMPINTRATEMAST] PRIMARY KEY ([LOANINT_RATEID])
);
GO

-- Table: LOAN_INS - Loan Instalment
CREATE TABLE [LOAN_INS] (
    [LOANINS_ID] BIGINT NOT NULL  -- Loan Instalment ID,
    [LOANINS_UNITID] BIGINT NOT NULL  -- Payroll Unit ID,
    [LOANINS_LOANNO] BIGINT NOT NULL  -- Loan No,
    [LOANINS_INSDATE] DATETIME2(3) NOT NULL  -- Loan Installment Date,
    [LOANINS_INSNO] BIGINT NOT NULL  -- Loan Installment No,
    [LOANINS_INSAMT] BIGINT NOT NULL  -- Installment Amount,
    [LOANINS_PRNOUT] BIGINT NOT NULL  -- Principal Outstanding,
    [LOANINS_PRNADJ] BIGINT NOT NULL  -- Principal to be Adjusted,
    [LOANINS_INTADJ] BIGINT NOT NULL  -- Interest to be Adusted,
    [LOANINS_FRODATE] DATETIME2(3) NULL  -- Interest From Date,
    [LOANINS_INTACC] BIGINT NOT NULL  -- Interest Accrued,
    [LOANINS_UPDATEDBY] BIGINT NOT NULL  -- Updated By,
    [LOANINS_UPDATEDON] DATETIME2(3) NOT NULL  -- Updated On,
    [LOANINS_INTREC] BIGINT NOT NULL  -- Interest Recovered Amount,
    [LOANINS_PRNREC] BIGINT NOT NULL  -- Principal Recovered Amount,
    [LOANINS_INTRATE] INT NOT NULL  -- Interest Rate %,
    [LOANINS_REMARKS] VARCHAR(200) NOT NULL  -- Installment Remarks,
    CONSTRAINT [PK_LOAN_INS] PRIMARY KEY ([LOANINS_ID])
);
GO

-- Table: LOAN_LEDGER - Loan Ledger
CREATE TABLE [LOAN_LEDGER] (
    [LOAN_LEDGERID] BIGINT NOT NULL  -- Loan Ledger ID,
    [LOAN_NO] BIGINT NOT NULL  -- Loan No,
    [LOAN_EMPSYSID] BIGINT NOT NULL  -- Employee System ID,
    [LOAN_UNITID] BIGINT NOT NULL  -- Unit ID,
    [LOAN_EMPNO] BIGINT NOT NULL  -- Employee No,
    [LOAN_TRNDATE] DATETIME2(3) NOT NULL  -- Transaction Date,
    [LOAN_DCFLAG] CHAR(1) NOT NULL  -- DC Flag (D-Debit ; C - Credit),
    [LOAN_DESCRIPTION] VARCHAR(200) NOT NULL  -- Loan Description,
    [LOAN_TRNAMT] BIGINT NOT NULL  -- Transaction Amount,
    [LOAN_TRNTYPE] CHAR(3) NOT NULL  -- Loan Transaction Type,
    [LOAN_TRNREFNUM] BIGINT NOT NULL  -- Loan Transaction Reference No,
    [LOAN_SCHEDULEID] BIGINT NOT NULL  -- Loan Schedule ID,
    [LOAN_UPDATEDBY] BIGINT NOT NULL  -- Updated By,
    [LOAN_UPDATEDON] DATETIME2(3) NOT NULL  -- Updated On,
    CONSTRAINT [PK_LOAN_LEDGER] PRIMARY KEY ([LOAN_LEDGERID])
);
GO

-- Table: LOAN_SET - Loan Settlement
CREATE TABLE [LOAN_SET] (
    [LOANSET_ID] BIGINT NOT NULL  -- Loan Settlement ID,
    [LOANSET_UNITID] BIGINT NOT NULL  -- Payroll Unit ID,
    [LOANSET_LOANNO] BIGINT NOT NULL  -- Loan No,
    [LOANSET_TYPE] CHAR(3) NOT NULL  -- Settlement Type (SET/INS) - Settlement / Installment Recovery,
    [LOANSET_INSNO] BIGINT NOT NULL  -- Loan Installment No,
    [LOANSET_INSDATE] DATETIME2(3) NOT NULL  -- Loan Installment Date,
    [LOANSET_RECDATE] DATETIME2(3) NOT NULL  -- Loan Recovery Date,
    [LOANSET_RECTYPE] CHAR(3) NOT NULL  -- Recovery Type (PRN/INT) - Principal / Interest,
    [LOANSET_INSAMT] BIGINT NOT NULL  -- Installment Amount,
    [LOANSET_PAYTYPE] CHAR(3) NOT NULL  -- Pay type (DIR/PAY/ADJ),
    [LOANSET_PAYBATCHID] BIGINT NOT NULL  -- Payroll Batch ID,
    [LOANSET_PAYID] INT NOT NULL  -- Direct Payment ID,
    [LOANSET_ADJLOANNO] BIGINT NOT NULL  -- Adjustment against loan No,
    [LOANSET_CANCELDATE] DATETIME2(3) NULL  -- Cancelled On,
    [LOANSET_CANCELBY] BIGINT NULL  -- Cancelled By,
    [LOANSET_UPDATEDBY] BIGINT NOT NULL  -- Updated By,
    [LOANSET_UPDATEDON] DATETIME2(3) NOT NULL  -- Updated On,
    CONSTRAINT [PK_LOAN_SET] PRIMARY KEY ([LOANSET_ID])
);
GO

-- Table: LOAN_ADJUSTMENT - Loan Disbursement Adjustment
CREATE TABLE [LOAN_ADJUSTMENT] (
    [LOAN_ADJID] BIGINT NOT NULL  -- Loan Adjustment ID,
    [LOAN_NO] BIGINT NOT NULL  -- Loan No,
    [LOAN_ADJLOANNO] BIGINT NOT NULL  -- Loan Adjustment Loan No,
    [LOAN_ADJPRNAMT] BIGINT NOT NULL  -- Loan Adjustment Principal Amount,
    [LOAN_ADJINTAMT] BIGINT NOT NULL  -- Loan Adjustment Interest Amount,
    [LOAN_UPDATEDBY] BIGINT NOT NULL  -- Last Updated By,
    [LOAN_UPDATEDON] DATETIME2(3) NOT NULL  -- Last Updated On,
    CONSTRAINT [PK_LOAN_ADJUSTMENT] PRIMARY KEY ([LOAN_ADJID])
);
GO

-- Table: LOAN_IUTAJV - Loan IUTA JV Details
CREATE TABLE [LOAN_IUTAJV] (
    [IUTA_ID] BIGINT NOT NULL  -- IUTA ID,
    [IUTA_LOANNO] BIGINT NOT NULL  -- Loan No,
    [IUTA_LOANCURUNIT] BIGINT NOT NULL  -- Employee Cur Unit,
    [IUTA_LOANJVNO] BIGINT NOT NULL  -- IUTA JV No,
    CONSTRAINT [PK_LOAN_IUTAJV] PRIMARY KEY ([IUTA_ID])
);
GO

-- Table: TEMPLOAN_IUTAJV - Loan IUTA JV Details
CREATE TABLE [TEMPLOAN_IUTAJV] (
    [IUTA_BATCHID] BIGINT NOT NULL  -- Batch ID,
    [IUTA_LOANNO] BIGINT NOT NULL  -- Loan No,
    [IUTA_LOANOLDUNIT] BIGINT NOT NULL  -- Employee Old Unit,
    [IUTA_LOANCURUNIT] BIGINT NOT NULL  -- Employee Cur Unit,
    CONSTRAINT [PK_TEMPLOAN_IUTAJV] PRIMARY KEY ([IUTA_BATCHID])
);
GO

-- Table: LOAN_UPD - loan update
CREATE TABLE [LOAN_UPD] (
    [LOAN_NO] BIGINT NOT NULL,
    [LOAN_PRNAMT] DECIMAL(19,0) NOT NULL
);
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

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

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

-- Indexes on Foreign Key Columns
CREATE INDEX [IDX_LOAN_EMPINTRATEMAST_LOANINT_LOANNO] ON [LOAN_EMPINTRATEMAST]([LOANINT_LOANNO]);
GO
CREATE INDEX [IDX_LOAN_LEDGER_LOAN_NO] ON [LOAN_LEDGER]([LOAN_NO]);
GO
