-- =============================================================================
-- LOAN_TRANSACTION_MODULE — DDL & Supporting Objects
-- Database : LOANDB
-- Schema   : dbo
-- =============================================================================

USE LOANDB;
GO

-- =============================================================================
-- 1. LOAN_MAIN  (Aggregate root – disbursed loans)
-- =============================================================================
IF OBJECT_ID('dbo.LOAN_MAIN', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LOAN_MAIN
    (
        LOAN_NO             BIGINT          NOT NULL IDENTITY(1,1),
        LOAN_APPID          BIGINT          NOT NULL,           -- FK → LOAN_APPLICATION
        LOAN_EMPSYSID       BIGINT          NOT NULL,           -- Employee system ID
        LOAN_ID             BIGINT          NOT NULL,           -- FK → loan definition
        LOAN_GRADEID        BIGINT          NOT NULL,
        LOAN_UNITID         BIGINT          NOT NULL,
        LOAN_SUBCLASSID     BIGINT          NOT NULL,
        LOAN_GUARANTOR      BIGINT          NOT NULL,

        -- Disbursement
        LOAN_DISBTYPE       NVARCHAR(3)     NOT NULL,           -- NEW / ADJ
        LOAN_PRNAMT         DECIMAL(19,0)   NOT NULL,           -- principal amount
        LOAN_OLDPRNADJ      DECIMAL(19,0)   NOT NULL DEFAULT 0, -- old principal adj
        LOAN_PAID           DECIMAL(19,0)   NOT NULL DEFAULT 0, -- amount paid
        LOAN_PRNOUT         DECIMAL(19,0)   NOT NULL,           -- principal outstanding
        LOAN_DATE           DATETIME2(3)    NOT NULL,           -- effective date
        LOAN_FIRSTINSDATE   DATETIME2(3)    NOT NULL,           -- first instalment date
        LOAN_LASTINSDATE    DATETIME2(3)    NOT NULL,           -- last  instalment date
        LOAN_CLSDATE        DATETIME2(3)    NULL,               -- closure date

        -- Terms
        LOAN_REASON         NVARCHAR(200)   NOT NULL,
        LOAN_APRREMARKS     NVARCHAR(200)   NULL,
        LOAN_CLOSURETYPE    NVARCHAR(3)     NOT NULL DEFAULT 'SET', -- SET/WOF/ADJ/LIV
        LOAN_NEWLOANNO      BIGINT          NOT NULL DEFAULT 0,
        LOAN_EMPINTRATE     CHAR(1)         NOT NULL DEFAULT 'N',   -- Y/N
        LOAN_COMFACTOR      CHAR(1)         NOT NULL DEFAULT 'S',   -- S=Simple
        LOAN_INTFREQUENCY   CHAR(1)         NOT NULL DEFAULT 'M',   -- M=Monthly
        LOAN_RECTYPE        NVARCHAR(3)     NOT NULL,               -- RBM/EM1/EMA/FPI

        -- Earning-Deduction IDs
        LOAN_AMTEDID        BIGINT          NOT NULL DEFAULT 0,
        LOAN_PRNEDID        BIGINT          NOT NULL DEFAULT 0,
        LOAN_INTEDID        BIGINT          NOT NULL DEFAULT 0,

        -- Employee overrides
        LOAN_EMPINSNOS      INT             NULL,
        LOAN_EMPINSAMT      DECIMAL(19,0)   NULL,

        -- Audit
        LOAN_CREATEDBY      BIGINT          NOT NULL,
        LOAN_CREATEDON      DATETIME2(3)    NOT NULL DEFAULT SYSUTCDATETIME(),
        LOAN_MODIFIEDBY     BIGINT          NOT NULL,
        LOAN_MODIFIEDON     DATETIME2(3)    NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_LOAN_MAIN PRIMARY KEY (LOAN_NO)
    );

    CREATE INDEX IDX_LOAN_MAIN_APPID    ON dbo.LOAN_MAIN (LOAN_APPID);
    CREATE INDEX IDX_LOAN_MAIN_EMPSYSID ON dbo.LOAN_MAIN (LOAN_EMPSYSID);
    CREATE INDEX IDX_LOAN_MAIN_UNITID   ON dbo.LOAN_MAIN (LOAN_UNITID);
END
GO

-- =============================================================================
-- 2. LOAN_INS  (Instalment schedule)
-- =============================================================================
IF OBJECT_ID('dbo.LOAN_INS', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LOAN_INS
    (
        LOANINS_ID          BIGINT          NOT NULL IDENTITY(1,1),
        LOANINS_LOANNO      BIGINT          NOT NULL,
        LOANINS_UNITID      BIGINT          NOT NULL,
        LOANINS_INSDATE     DATETIME2(3)    NOT NULL,
        LOANINS_INSNO       BIGINT          NOT NULL,
        LOANINS_INSAMT      DECIMAL(19,2)   NOT NULL,
        LOANINS_PRNOUT      DECIMAL(19,2)   NOT NULL DEFAULT 0,
        LOANINS_PRNADJ      DECIMAL(19,2)   NOT NULL DEFAULT 0,
        LOANINS_INTADJ      DECIMAL(19,2)   NOT NULL DEFAULT 0,
        LOANINS_FRODATE     DATETIME2(3)    NULL,
        LOANINS_INTACC      DECIMAL(19,2)   NOT NULL DEFAULT 0,
        LOANINS_INTREC      DECIMAL(19,2)   NOT NULL DEFAULT 0,
        LOANINS_PRNREC      DECIMAL(19,2)   NOT NULL DEFAULT 0,
        LOANINS_INTRATE     INT             NOT NULL DEFAULT 0,
        LOANINS_REMARKS     NVARCHAR(200)   NOT NULL DEFAULT '',
        LOANINS_UPDATEDBY   BIGINT          NOT NULL DEFAULT 0,
        LOANINS_UPDATEDON   DATETIME2(3)    NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_LOAN_INS PRIMARY KEY (LOANINS_ID),
        CONSTRAINT FK_LOAN_INS_MAIN FOREIGN KEY (LOANINS_LOANNO)
            REFERENCES dbo.LOAN_MAIN (LOAN_NO)
    );

    CREATE INDEX IDX_LOAN_INS_LOANNO ON dbo.LOAN_INS (LOANINS_LOANNO);
    CREATE UNIQUE INDEX UQ_LOAN_INS_NO ON dbo.LOAN_INS (LOANINS_LOANNO, LOANINS_INSNO);
END
GO

-- =============================================================================
-- 3. LOAN_SET  (Settlement / recovery records)
-- =============================================================================
IF OBJECT_ID('dbo.LOAN_SET', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LOAN_SET
    (
        LOANSET_ID          BIGINT          NOT NULL IDENTITY(1,1),
        LOANSET_LOANNO      BIGINT          NOT NULL,
        LOANSET_UNITID      BIGINT          NOT NULL,
        LOANSET_TYPE        NVARCHAR(3)     NOT NULL,           -- SET/WOF/ADJ
        LOANSET_INSNO       BIGINT          NOT NULL,
        LOANSET_INSDATE     DATETIME2(3)    NOT NULL,
        LOANSET_RECDATE     DATETIME2(3)    NOT NULL,
        LOANSET_RECTYPE     NVARCHAR(3)     NOT NULL,           -- RBM/EM1/EMA/FPI
        LOANSET_INSAMT      DECIMAL(19,2)   NOT NULL,
        LOANSET_PAYTYPE     NVARCHAR(3)     NOT NULL DEFAULT '',
        LOANSET_PAYBATCHID  BIGINT          NOT NULL DEFAULT 0,
        LOANSET_PAYID       BIGINT          NOT NULL DEFAULT 0,
        LOANSET_ADJLOANNO   BIGINT          NOT NULL DEFAULT 0,
        LOANSET_CANCELDATE  DATETIME2(3)    NULL,
        LOANSET_CANCELBY    BIGINT          NULL,
        LOANSET_UPDATEDBY   BIGINT          NOT NULL DEFAULT 0,
        LOANSET_UPDATEDON   DATETIME2(3)    NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_LOAN_SET PRIMARY KEY (LOANSET_ID),
        CONSTRAINT FK_LOAN_SET_MAIN FOREIGN KEY (LOANSET_LOANNO)
            REFERENCES dbo.LOAN_MAIN (LOAN_NO)
    );

    CREATE INDEX IDX_LOAN_SET_LOANNO ON dbo.LOAN_SET (LOANSET_LOANNO);
END
GO

-- =============================================================================
-- 4. LOAN_LEDGER  (Double-entry accounting ledger)
-- =============================================================================
IF OBJECT_ID('dbo.LOAN_LEDGER', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LOAN_LEDGER
    (
        LOAN_LEDGERID       BIGINT          NOT NULL IDENTITY(1,1),
        LOAN_NO             BIGINT          NOT NULL,
        LOAN_EMPSYSID       BIGINT          NOT NULL,
        LOAN_UNITID         BIGINT          NOT NULL,
        LOAN_EMPNO          BIGINT          NOT NULL DEFAULT 0,
        LOAN_TRNDATE        DATETIME2(3)    NOT NULL,
        LOAN_DCFLAG         CHAR(1)         NOT NULL,           -- D=Debit / C=Credit
        LOAN_DESCRIPTION    NVARCHAR(200)   NOT NULL,
        LOAN_TRNAMT         DECIMAL(19,2)   NOT NULL,
        LOAN_TRNTYPE        NVARCHAR(3)     NOT NULL,
        LOAN_TRNREFNUM      BIGINT          NOT NULL DEFAULT 0,
        LOAN_SCHEDULEID     BIGINT          NOT NULL DEFAULT 0,
        LOAN_UPDATEDBY      BIGINT          NOT NULL DEFAULT 0,
        LOAN_UPDATEDON      DATETIME2(3)    NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_LOAN_LEDGER PRIMARY KEY (LOAN_LEDGERID),
        CONSTRAINT CK_LOAN_LEDGER_DC CHECK (LOAN_DCFLAG IN ('D','C'))
    );

    CREATE INDEX IDX_LOAN_LEDGER_NO       ON dbo.LOAN_LEDGER (LOAN_NO);
    CREATE INDEX IDX_LOAN_LEDGER_EMPSYSID ON dbo.LOAN_LEDGER (LOAN_EMPSYSID);
    CREATE INDEX IDX_LOAN_LEDGER_TRNDATE  ON dbo.LOAN_LEDGER (LOAN_TRNDATE);
END
GO

-- =============================================================================
-- 5. LOAN_EMPINTRATEMAST  (Employee-specific interest rate overrides)
-- =============================================================================
IF OBJECT_ID('dbo.LOAN_EMPINTRATEMAST', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LOAN_EMPINTRATEMAST
    (
        LOANINT_RATEID          BIGINT          NOT NULL IDENTITY(1,1),
        LOANINT_LOANNO          BIGINT          NOT NULL,
        LOANINT_EFFDATE         DATETIME2(3)    NOT NULL,
        LOANINT_CLSDATE         DATETIME2(3)    NULL,
        LOANINT_RATE            INT             NOT NULL,
        LOANINT_EMIAMT          DECIMAL(19,2)   NOT NULL,
        LOANINT_INSNOS          INT             NOT NULL,
        LOANINT_LASTMODIFIEDBY  BIGINT          NOT NULL,

        CONSTRAINT PK_LOAN_EMPINTRATEMAST PRIMARY KEY (LOANINT_RATEID),
        CONSTRAINT FK_LOANINT_MAIN FOREIGN KEY (LOANINT_LOANNO)
            REFERENCES dbo.LOAN_MAIN (LOAN_NO)
    );

    CREATE INDEX IDX_LOANINT_LOANNO ON dbo.LOAN_EMPINTRATEMAST (LOANINT_LOANNO);
END
GO

-- =============================================================================
-- 6. LOAN_ADJUSTMENT  (Principal / interest adjustments)
-- =============================================================================
IF OBJECT_ID('dbo.LOAN_ADJUSTMENT', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LOAN_ADJUSTMENT
    (
        LOAN_ADJID          BIGINT          NOT NULL IDENTITY(1,1),
        LOAN_NO             BIGINT          NOT NULL,
        LOAN_ADJLOANNO      BIGINT          NOT NULL DEFAULT 0,
        LOAN_ADJPRNAMT      DECIMAL(19,0)   NOT NULL DEFAULT 0,
        LOAN_ADJINTAMT      DECIMAL(19,0)   NOT NULL DEFAULT 0,
        LOAN_UPDATEDBY      BIGINT          NOT NULL DEFAULT 0,
        LOAN_UPDATEDON      DATETIME2(3)    NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_LOAN_ADJUSTMENT PRIMARY KEY (LOAN_ADJID),
        CONSTRAINT FK_LOAN_ADJ_MAIN FOREIGN KEY (LOAN_NO)
            REFERENCES dbo.LOAN_MAIN (LOAN_NO)
    );

    CREATE INDEX IDX_LOAN_ADJUSTMENT_NO ON dbo.LOAN_ADJUSTMENT (LOAN_NO);
END
GO

-- =============================================================================
-- 7. VIEWS
-- =============================================================================

-- v_ActiveLoans — active (non-closed) loans with employee context
CREATE OR ALTER VIEW dbo.v_ActiveLoans
AS
SELECT
    m.LOAN_NO,
    m.LOAN_APPID,
    m.LOAN_EMPSYSID,
    m.LOAN_ID             AS LOAN_DEFINITION_ID,
    m.LOAN_UNITID,
    m.LOAN_DISBTYPE,
    m.LOAN_PRNAMT,
    m.LOAN_PRNOUT,
    m.LOAN_PAID,
    m.LOAN_DATE,
    m.LOAN_FIRSTINSDATE,
    m.LOAN_LASTINSDATE,
    m.LOAN_RECTYPE,
    m.LOAN_CREATEDBY,
    m.LOAN_CREATEDON
FROM dbo.LOAN_MAIN m
WHERE m.LOAN_CLSDATE IS NULL;
GO

-- v_OverdueInstallments — unpaid instalments past their due date
CREATE OR ALTER VIEW dbo.v_OverdueInstallments
AS
SELECT
    i.LOANINS_ID,
    i.LOANINS_LOANNO,
    i.LOANINS_UNITID,
    i.LOANINS_INSNO,
    i.LOANINS_INSDATE,
    i.LOANINS_INSAMT,
    i.LOANINS_PRNREC,
    i.LOANINS_INTREC,
    (i.LOANINS_INSAMT - i.LOANINS_PRNREC - i.LOANINS_INTREC) AS REMAINING_AMOUNT,
    m.LOAN_EMPSYSID,
    m.LOAN_UNITID  AS LOAN_UNITID
FROM dbo.LOAN_INS i
INNER JOIN dbo.LOAN_MAIN m ON m.LOAN_NO = i.LOANINS_LOANNO
WHERE i.LOANINS_INSDATE < CAST(SYSUTCDATETIME() AS DATE)
  AND (i.LOANINS_PRNREC + i.LOANINS_INTREC) < i.LOANINS_INSAMT
  AND m.LOAN_CLSDATE IS NULL;
GO

-- v_LoanLedgerSummary — running balance per loan
CREATE OR ALTER VIEW dbo.v_LoanLedgerSummary
AS
SELECT
    l.LOAN_NO,
    l.LOAN_EMPSYSID,
    COUNT(*)                                                        AS ENTRY_COUNT,
    SUM(CASE WHEN l.LOAN_DCFLAG = 'D' THEN l.LOAN_TRNAMT ELSE 0 END) AS TOTAL_DEBITS,
    SUM(CASE WHEN l.LOAN_DCFLAG = 'C' THEN l.LOAN_TRNAMT ELSE 0 END) AS TOTAL_CREDITS
FROM dbo.LOAN_LEDGER l
GROUP BY l.LOAN_NO, l.LOAN_EMPSYSID;
GO

-- =============================================================================
-- 8. SCALAR FUNCTIONS
-- =============================================================================

-- fn_GetEMIAmount  — monthly EMI using reducing-balance formula
CREATE OR ALTER FUNCTION dbo.fn_GetEMIAmount
(
    @PrincipalAmount    DECIMAL(19,2),
    @AnnualInterestRate INT,
    @TenureMonths       INT
)
RETURNS DECIMAL(19,2)
AS
BEGIN
    IF @TenureMonths <= 0 OR @PrincipalAmount <= 0
        RETURN 0;

    IF @AnnualInterestRate = 0
        RETURN ROUND(@PrincipalAmount / @TenureMonths, 2);

    DECLARE @r      FLOAT = CAST(@AnnualInterestRate AS FLOAT) / 12.0 / 100.0;
    DECLARE @factor FLOAT = POWER(1.0 + @r, @TenureMonths);
    RETURN ROUND(CAST(@PrincipalAmount * @r * @factor / (@factor - 1.0) AS DECIMAL(19,2)), 2);
END;
GO

-- fn_GetLoanEligibility  — maximum eligible principal for an employee
-- (placeholder: overridden by business rules in the definition module)
CREATE OR ALTER FUNCTION dbo.fn_GetLoanEligibility
(
    @EmployeeId         BIGINT,
    @LoanDefinitionId   BIGINT,
    @GradeId            BIGINT
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    -- Returns the max amount the employee is eligible to borrow.
    -- The actual limit is enforced by the loan-definition rule tables;
    -- this function provides a quick scalar check for API validation.
    DECLARE @MaxEligible DECIMAL(19,0) = 0;

    SELECT @MaxEligible = ISNULL(SUM(m.LOAN_PRNOUT), 0)
    FROM dbo.LOAN_MAIN m
    WHERE m.LOAN_EMPSYSID = @EmployeeId
      AND m.LOAN_ID        = @LoanDefinitionId
      AND m.LOAN_CLSDATE  IS NULL;

    -- Return remaining (positive means employee still has headroom)
    RETURN CASE WHEN @MaxEligible = 0 THEN 999999999 ELSE @MaxEligible END;
END;
GO

-- =============================================================================
-- 9. STORED PROCEDURES
-- =============================================================================

-- sp_ApplyForLoan — records a new disbursement transaction
CREATE OR ALTER PROCEDURE dbo.sp_ApplyForLoan
(
    @ApplicationId          BIGINT,
    @EmployeeId             BIGINT,
    @LoanDefinitionId       BIGINT,
    @GradeId                BIGINT,
    @UnitId                 BIGINT,
    @SubclassId             BIGINT,
    @GuarantorId            BIGINT,
    @DisbursementType       NVARCHAR(3),
    @PrincipalAmount        DECIMAL(19,0),
    @OldPrincipalAdj        DECIMAL(19,0),
    @RecoveryMethod         NVARCHAR(3),
    @EffectiveDate          DATETIME2(3),
    @FirstInstallmentDate   DATETIME2(3),
    @LastInstallmentDate    DATETIME2(3),
    @Reason                 NVARCHAR(200),
    @ClosureType            NVARCHAR(3),
    @CompoundingFactor      CHAR(1),
    @InterestFrequency      CHAR(1),
    @HasEmployeeInterestRate CHAR(1),
    @AmountEdId             BIGINT,
    @PrnEdId                BIGINT,
    @IntEdId                BIGINT,
    @CreatedBy              BIGINT,
    @NewLoanNo              BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LOAN_MAIN
    (
        LOAN_APPID, LOAN_EMPSYSID, LOAN_ID, LOAN_GRADEID,
        LOAN_UNITID, LOAN_SUBCLASSID, LOAN_GUARANTOR,
        LOAN_DISBTYPE, LOAN_PRNAMT, LOAN_OLDPRNADJ, LOAN_PAID, LOAN_PRNOUT,
        LOAN_DATE, LOAN_FIRSTINSDATE, LOAN_LASTINSDATE,
        LOAN_REASON, LOAN_CLOSURETYPE, LOAN_NEWLOANNO,
        LOAN_EMPINTRATE, LOAN_COMFACTOR, LOAN_INTFREQUENCY, LOAN_RECTYPE,
        LOAN_AMTEDID, LOAN_PRNEDID, LOAN_INTEDID,
        LOAN_CREATEDBY, LOAN_CREATEDON, LOAN_MODIFIEDBY, LOAN_MODIFIEDON
    )
    VALUES
    (
        @ApplicationId, @EmployeeId, @LoanDefinitionId, @GradeId,
        @UnitId, @SubclassId, @GuarantorId,
        @DisbursementType, @PrincipalAmount, @OldPrincipalAdj, 0, @PrincipalAmount,
        @EffectiveDate, @FirstInstallmentDate, @LastInstallmentDate,
        @Reason, @ClosureType, 0,
        @HasEmployeeInterestRate, @CompoundingFactor, @InterestFrequency, @RecoveryMethod,
        @AmountEdId, @PrnEdId, @IntEdId,
        @CreatedBy, SYSUTCDATETIME(), @CreatedBy, SYSUTCDATETIME()
    );

    SET @NewLoanNo = SCOPE_IDENTITY();
END;
GO

-- sp_ApproveLoanApplication — updates application status to APPROVED
-- (mirrors the loan-application service procedure for cross-check)
CREATE OR ALTER PROCEDURE dbo.sp_ApproveLoanApplication
(
    @LoanApplicationId  BIGINT,
    @ApprovedBy         BIGINT,
    @Remarks            NVARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    -- If this database holds a mirror of the application status, update it here.
    -- In the split-service topology this is a no-op placeholder that confirms
    -- the disbursement transaction has been linked to the application.
    SELECT
        @LoanApplicationId  AS LoanApplicationId,
        @ApprovedBy         AS ApprovedBy,
        SYSUTCDATETIME()    AS ApprovedAt,
        @Remarks            AS Remarks;
END;
GO

-- sp_RecordEmiPayment — marks an instalment as paid and updates LOAN_MAIN
CREATE OR ALTER PROCEDURE dbo.sp_RecordEmiPayment
(
    @LoanNo         BIGINT,
    @InstallmentId  BIGINT,
    @PrincipalPaid  DECIMAL(19,2),
    @InterestPaid   DECIMAL(19,2),
    @PaidBy         BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        UPDATE dbo.LOAN_INS
        SET LOANINS_PRNREC   = LOANINS_PRNREC + @PrincipalPaid,
            LOANINS_INTREC   = LOANINS_INTREC + @InterestPaid,
            LOANINS_UPDATEDBY = @PaidBy,
            LOANINS_UPDATEDON = SYSUTCDATETIME()
        WHERE LOANINS_ID     = @InstallmentId
          AND LOANINS_LOANNO = @LoanNo;

        UPDATE dbo.LOAN_MAIN
        SET LOAN_PAID       = LOAN_PAID + @PrincipalPaid + @InterestPaid,
            LOAN_PRNOUT     = LOAN_PRNOUT - @PrincipalPaid,
            LOAN_MODIFIEDBY = @PaidBy,
            LOAN_MODIFIEDON = SYSUTCDATETIME()
        WHERE LOAN_NO = @LoanNo;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- sp_CloseLoan — stamps the closure date and type on LOAN_MAIN
CREATE OR ALTER PROCEDURE dbo.sp_CloseLoan
(
    @LoanNo       BIGINT,
    @ClosureType  NVARCHAR(3),
    @ClosedBy     BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LOAN_MAIN
    SET LOAN_CLSDATE    = SYSUTCDATETIME(),
        LOAN_CLOSURETYPE = @ClosureType,
        LOAN_MODIFIEDBY  = @ClosedBy,
        LOAN_MODIFIEDON  = SYSUTCDATETIME()
    WHERE LOAN_NO = @LoanNo
      AND LOAN_CLSDATE IS NULL;   -- idempotent guard

    IF @@ROWCOUNT = 0
        THROW 50001, 'Loan not found or already closed.', 1;
END;
GO

-- =============================================================================
-- 10. SAMPLE / SEED DATA (development only – wrapped in conditional)
-- =============================================================================
/*
IF DB_NAME() = 'LOANDB_DEV'
BEGIN
    -- Insert a test loan for verification
    INSERT INTO dbo.LOAN_MAIN (
        LOAN_APPID, LOAN_EMPSYSID, LOAN_ID, LOAN_GRADEID, LOAN_UNITID,
        LOAN_SUBCLASSID, LOAN_GUARANTOR, LOAN_DISBTYPE, LOAN_PRNAMT,
        LOAN_OLDPRNADJ, LOAN_PAID, LOAN_PRNOUT, LOAN_DATE,
        LOAN_FIRSTINSDATE, LOAN_LASTINSDATE, LOAN_REASON, LOAN_CLOSURETYPE,
        LOAN_NEWLOANNO, LOAN_EMPINTRATE, LOAN_COMFACTOR, LOAN_INTFREQUENCY,
        LOAN_RECTYPE, LOAN_AMTEDID, LOAN_PRNEDID, LOAN_INTEDID,
        LOAN_CREATEDBY, LOAN_CREATEDON, LOAN_MODIFIEDBY, LOAN_MODIFIEDON
    ) VALUES (
        1001, 2001, 3001, 4001, 5001,
        6001, 7001, 'NEW', 500000,
        0, 0, 500000, '2026-01-01',
        '2026-02-01', '2028-01-01', 'House repair', 'SET',
        0, 'N', 'S', 'M',
        'EMA', 0, 0, 0,
        1, SYSUTCDATETIME(), 1, SYSUTCDATETIME()
    );
END
*/
GO
