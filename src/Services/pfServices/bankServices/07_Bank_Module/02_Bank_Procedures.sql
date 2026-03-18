-- =========================================================================
-- BANK MODULE - Stored Procedures
-- Database: PFDB
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- PROCEDURE: Record Cheque Issue
IF OBJECT_ID('dbo.usp_IssueCheque', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_IssueCheque;
GO

CREATE PROCEDURE dbo.usp_IssueCheque
    @p_ChequeID BIGINT,
    @p_ChequeNo DECIMAL(20,0),
    @p_Amount DECIMAL(19,0),
    @p_ChequeDate DATETIME2(3),
    @p_Payee VARCHAR(100),
    @p_AccountID BIGINT,
    @p_IssuedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF @p_Amount <= 0
        BEGIN
            THROW 50001, 'Cheque amount must be greater than zero', 1;
        END
        
        INSERT INTO dbo.CHEQUE_DET (
            CHEQUE_ID, CHEQUE_NO, CHEQUE_AMOUNT, CHEQUE_DATE, 
            CHEQUE_PAYEE, CHEQUE_STATUS
        ) VALUES (
            @p_ChequeID, @p_ChequeNo, @p_Amount, @p_ChequeDate,
            @p_Payee, 'I'
        );
        
        COMMIT TRANSACTION;
        PRINT 'Cheque issued successfully';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- VIEW: Cheque Status Report
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_ChequeStatusReport' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_ChequeStatusReport AS
    SELECT 
        cd.CHEQUE_ID,
        cd.CHEQUE_NO,
        cd.CHEQUE_AMOUNT,
        cd.CHEQUE_DATE,
        cd.CHEQUE_PAYEE,
        cd.CHEQUE_STATUS,
        cd.CHEQUE_CLEARED_DATE,
        ba.ACCOUNT_NUMBER,
        ba.ACCOUNT_TITLE
    FROM dbo.CHEQUE_DET cd
    LEFT JOIN dbo.BANK_ACCOUNT ba ON cd.CHEQUE_BANK = ba.ACCOUNT_ID
    WHERE cd.CHEQUE_STATUS IN ('I', 'O', 'C');
END
GO

-- VIEW: Bank Account Summary
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_BankAccountSummary' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_BankAccountSummary AS
    SELECT 
        ba.ACCOUNT_ID,
        ba.ACCOUNT_NUMBER,
        ba.ACCOUNT_TITLE,
        ba.TRUST_CODE,
        ba.ACCOUNT_TYPE,
        ba.ACCOUNT_BALANCE,
        ba.ACCOUNT_STATUS,
        bm.BANK_NAME,
        bm.BRANCH_NAME
    FROM dbo.BANK_ACCOUNT ba
    LEFT JOIN dbo.BANK_MASTER bm ON ba.BANK_CODE = bm.BANK_CODE
    WHERE ba.ACCOUNT_STATUS = 'A';
END
GO

PRINT 'Bank Module Procedures created successfully!';
GO
