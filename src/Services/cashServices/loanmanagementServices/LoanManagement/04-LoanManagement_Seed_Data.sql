-- =============================================================================
-- LoanManagement Seed Data
-- Database : CASHDB
-- Server   : (localdb)\MSSQLLocalDB
-- Run after: 03-LoanManagement_Create_Schema.sql  OR  dotnet ef database update
-- =============================================================================
USE CASHDB;
GO

-- Idempotent: skip if data already present
IF EXISTS (SELECT 1 FROM LOAN_MAIN WHERE LOAN_ID IN (1, 2))
BEGIN
    RAISERROR('Seed data already present – nothing inserted.', 0, 1) WITH NOWAIT;
    GOTO DONE;
END

-- ─────────────────────────────────────────────────────────────────────────────
-- 1. LOAN_MAIN
-- ─────────────────────────────────────────────────────────────────────────────
INSERT INTO LOAN_MAIN (
    LOAN_ID, LOAN_KEY, LOAN_ORGID, LOAN_ORGCURR, LOAN_CURR,
    LOAN_DATE, LOAN_TYPEID, LOAN_BANKID,
    LOAN_CREATEDBY, LOAN_CREATEDON,
    LOAN_AMOUNT, LOAN_STATUS
) VALUES
-- Term loan – 3-year, ₹50 lakh
(1, 'L2026-001', 100, 1, 1,
 '2026-01-15', 10, 200,
 1, '2026-01-15T09:00:00',
 5000000, 'A'),
-- Working capital – 1-year, ₹12 lakh
(2, 'L2026-002', 101, 1, 1,
 '2026-03-01', 11, 201,
 1, '2026-03-01T10:30:00',
 1200000, 'A');

-- ─────────────────────────────────────────────────────────────────────────────
-- 2. LOAN_DISBSCH  (3 tranches for Loan 1 + 1 tranche for Loan 2)
--    DISB_ID is IDENTITY — omit it and let SQL Server generate it
-- ─────────────────────────────────────────────────────────────────────────────
INSERT INTO LOAN_DISBSCH (DISB_LOANID, DISB_DATE, DISB_AMOUNT, DISB_EXCRATE, DISB_EXCAMT)
VALUES
(1, '2026-02-01', 2000000, 1, 2000000),
(1, '2026-05-01', 2000000, 1, 2000000),
(1, '2026-09-01', 1000000, 1, 1000000),
(2, '2026-03-15', 1200000, 1, 1200000);

-- ─────────────────────────────────────────────────────────────────────────────
-- 3. LOAN_INTEREST  (fixed + floating for Loan 1; fixed for Loan 2)
--    INT_ID is IDENTITY — omit it
-- ─────────────────────────────────────────────────────────────────────────────
INSERT INTO LOAN_INTEREST (INT_LOANID, INT_RATETYPE, INT_PER, INT_FLOATTYPEID, INT_EFFDATE)
VALUES
(1, 'FX', 8.50, NULL,   '2026-01-15'),   -- Fixed  8.50 % from origination
(1, 'FL', 1.00, 301,    '2026-07-01'),   -- Floating MIBOR+1 % from Jul-26
(2, 'FX', 9.25, NULL,   '2026-03-01');   -- Fixed  9.25 % for working-capital

-- ─────────────────────────────────────────────────────────────────────────────
-- 4a. LOAN_REPAYSCH for Loan 1  (36 monthly EMIs of ₹152,777)
--    REPAY_ID is IDENTITY — omit it
-- ─────────────────────────────────────────────────────────────────────────────
DECLARE @startDate1 DATE    = '2026-02-01';
DECLARE @emi1       DECIMAL = 152777;
DECLARE @m          INT     = 0;

WHILE @m < 36
BEGIN
    INSERT INTO LOAN_REPAYSCH (REPAY_LOANID, REPAY_DATE, REPAY_AMT, REPAY_FLAG)
    VALUES (1, DATEADD(MONTH, @m, @startDate1), @emi1, 'N');
    SET @m += 1;
END

-- ─────────────────────────────────────────────────────────────────────────────
-- 4b. LOAN_REPAYSCH for Loan 2  (12 monthly EMIs of ₹104,167)
-- ─────────────────────────────────────────────────────────────────────────────
DECLARE @startDate2 DATE    = '2026-04-01';
DECLARE @emi2       DECIMAL = 104167;
SET @m = 0;

WHILE @m < 12
BEGIN
    INSERT INTO LOAN_REPAYSCH (REPAY_LOANID, REPAY_DATE, REPAY_AMT, REPAY_FLAG)
    VALUES (2, DATEADD(MONTH, @m, @startDate2), @emi2, 'N');
    SET @m += 1;
END

DONE:
GO

-- Quick verification
SELECT 'LOAN_MAIN'     AS [Table], COUNT(*) AS [Rows] FROM LOAN_MAIN
UNION ALL
SELECT 'LOAN_DISBSCH'  ,           COUNT(*)            FROM LOAN_DISBSCH
UNION ALL
SELECT 'LOAN_INTEREST' ,           COUNT(*)            FROM LOAN_INTEREST
UNION ALL
SELECT 'LOAN_REPAYSCH' ,           COUNT(*)            FROM LOAN_REPAYSCH;
GO
