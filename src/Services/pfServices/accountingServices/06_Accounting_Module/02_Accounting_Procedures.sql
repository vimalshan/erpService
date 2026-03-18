-- =========================================================================
-- ACCOUNTING & GL MODULE - Stored Procedures
-- Database: PFDB
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- PROCEDURE: Post GL Entry
IF OBJECT_ID('dbo.usp_PostGLEntry', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PostGLEntry;
GO

CREATE PROCEDURE dbo.usp_PostGLEntry
    @p_AccountCode VARCHAR(10),
    @p_DebitAmount DECIMAL(19,0),
    @p_CreditAmount DECIMAL(19,0),
    @p_ReferenceID BIGINT,
    @p_PostingDate DATETIME2(3),
    @p_Remarks VARCHAR(200),
    @p_PostedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF @p_DebitAmount <= 0 AND @p_CreditAmount <= 0
        BEGIN
            THROW 50001, 'Either debit or credit amount must be greater than zero', 1;
        END
        
        INSERT INTO dbo.GL_POSTING (
            ACCOUNT_CODE,
            POSTING_DATE,
            DEBIT_AMOUNT,
            CREDIT_AMOUNT,
            REFERENCE_ID,
            POSTING_REMARKS
        ) VALUES (
            @p_AccountCode,
            @p_PostingDate,
            @p_DebitAmount,
            @p_CreditAmount,
            @p_ReferenceID,
            @p_Remarks
        );
        
        COMMIT TRANSACTION;
        PRINT 'GL entry posted successfully';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- VIEW: GL Trial Balance
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_GLTrialBalance' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_GLTrialBalance AS
    SELECT 
        gp.ACCOUNT_CODE,
        ma.MAIN_ACCOUNT_NAME,
        SUM(ISNULL(gp.DEBIT_AMOUNT, 0)) AS TotalDebit,
        SUM(ISNULL(gp.CREDIT_AMOUNT, 0)) AS TotalCredit,
        SUM(ISNULL(gp.DEBIT_AMOUNT, 0)) - SUM(ISNULL(gp.CREDIT_AMOUNT, 0)) AS Balance
    FROM dbo.GL_POSTING gp
    LEFT JOIN dbo.MAINACCOUNT_MASTER ma ON gp.ACCOUNT_CODE = ma.MAIN_ACCOUNT_CODE
    GROUP BY gp.ACCOUNT_CODE, ma.MAIN_ACCOUNT_NAME;
END
GO

-- VIEW: Transaction Journal
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE name = 'vw_TransactionJournal' AND type = 'V')
BEGIN
    CREATE VIEW dbo.vw_TransactionJournal AS
    SELECT 
        t.TRANSACTION_ID,
        t.TD_TRANSACTION_CODE,
        tm.TRANSACTION_NAME,
        t.TD_TRANSACTION_DATE,
        t.TD_MEMBER_NO,
        t.TD_AMOUNT,
        t.TD_TYPE_CODE,
        t.TD_REMARKS
    FROM dbo.TRAN_DET t
    LEFT JOIN dbo.TRANSACTION_MASTER tm ON t.TD_TRANSACTION_CODE = tm.TRANSACTION_CODE
    WHERE t.TD_CANCEL_STATUS IS NULL;
END
GO

PRINT 'Accounting Module Procedures created successfully!';
GO
