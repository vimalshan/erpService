-- CASHDB Stored Procedures, Functions & Triggers
-- Cash Management & Bank Reconciliation System
-- Created: February 13, 2026

USE CASHDB;
GO

-- =====================================================
-- FUNCTIONS
-- =====================================================

-- Function: Get Total Cash in Hand
IF OBJECT_ID('dbo.fn_GetCashInHand', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_GetCashInHand;
GO

CREATE FUNCTION dbo.fn_GetCashInHand (
    @p_CashUnitID BIGINT,
    @p_AsOfDate DATETIME2(3)
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    DECLARE @CashInHand DECIMAL(19,0) = 0;
    
    BEGIN TRY
        SELECT @CashInHand = ISNULL(SUM(
            CASE 
                WHEN CASH_TXN_TYPE = 'R' THEN CASH_TXN_AMOUNT  -- Receipt
                WHEN CASH_TXN_TYPE = 'D' THEN -CASH_TXN_AMOUNT  -- Disbursement
                ELSE 0
            END
        ), 0)
        FROM dbo.CASH_TRANSACTION
        WHERE CASH_UNIT_ID = @p_CashUnitID
          AND CASH_TXN_DATE <= @p_AsOfDate
          AND CASH_TXN_STATUS = 'P';  -- Posted
    END TRY
    BEGIN CATCH
        SET @CashInHand = 0;
    END CATCH
    
    RETURN @CashInHand;
END;
GO

-- Function: Get Bank Balance (Ledger)
IF OBJECT_ID('dbo.fn_GetBankBalance', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_GetBankBalance;
GO

CREATE FUNCTION dbo.fn_GetBankBalance (
    @p_BankAccountID BIGINT,
    @p_AsOfDate DATETIME2(3)
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    DECLARE @BankBalance DECIMAL(19,0) = 0;
    
    BEGIN TRY
        SELECT @BankBalance = ISNULL(SUM(
            CASE 
                WHEN BANK_TXN_TYPE = 'D' THEN BANK_TXN_AMOUNT  -- Deposit
                WHEN BANK_TXN_TYPE = 'W' THEN -BANK_TXN_AMOUNT  -- Withdrawal
                ELSE 0
            END
        ), 0)
        FROM dbo.BANK_TRANSACTION
        WHERE BANK_ACCOUNT_ID = @p_BankAccountID
          AND BANK_TXN_DATE <= @p_AsOfDate
          AND BANK_TXN_STATUS = 'P';  -- Posted
    END TRY
    BEGIN CATCH
        SET @BankBalance = 0;
    END CATCH
    
    RETURN @BankBalance;
END;
GO

-- Function: Get Uncleared Cheques Total
IF OBJECT_ID('dbo.fn_GetUnclearedChequesTotal', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_GetUnclearedChequesTotal;
GO

CREATE FUNCTION dbo.fn_GetUnclearedChequesTotal (
    @p_BankAccountID BIGINT,
    @p_AsOfDate DATETIME2(3)
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    DECLARE @UnclearedAmount DECIMAL(19,0) = 0;
    
    BEGIN TRY
        SELECT @UnclearedAmount = ISNULL(SUM(CHEQUE_AMOUNT), 0)
        FROM dbo.CHEQUE_REGISTER
        WHERE BANK_ACCOUNT_ID = @p_BankAccountID
          AND CHEQUE_ISSUE_DATE <= @p_AsOfDate
          AND CHEQUE_STATUS IN ('I', 'B');  -- Issued, Bounced
    END TRY
    BEGIN CATCH
        SET @UnclearedAmount = 0;
    END CATCH
    
    RETURN @UnclearedAmount;
END;
GO

-- =====================================================
-- STORED PROCEDURES
-- =====================================================

-- Procedure: Record Cash Receipt
IF OBJECT_ID('dbo.usp_RecordCashReceipt', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RecordCashReceipt;
GO

CREATE PROCEDURE dbo.usp_RecordCashReceipt
    @p_CashUnitID BIGINT,
    @p_ReceiptAmount DECIMAL(19,0),
    @p_ReceiptSource VARCHAR(100),  -- 'EXPENSE', 'ADVANCE', 'REFUND', etc.
    @p_ReferenceNumber VARCHAR(50),
    @p_Remarks VARCHAR(500),
    @p_RecordedBy BIGINT,
    @p_TransactionID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        
        -- Validate amount
        IF @p_ReceiptAmount <= 0
        BEGIN
            THROW 50001, 'Receipt amount must be greater than zero', 1;
        END
        
        -- Record transaction
        INSERT INTO dbo.CASH_TRANSACTION (
            CASH_UNIT_ID,
            CASH_TXN_TYPE,
            CASH_TXN_AMOUNT,
            CASH_TXN_SOURCE,
            CASH_TXN_REF_NO,
            CASH_TXN_DATE,
            CASH_TXN_REMARKS,
            CASH_TXN_STATUS,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_CashUnitID,
            'R',  -- Receipt
            @p_ReceiptAmount,
            @p_ReceiptSource,
            @p_ReferenceNumber,
            @ProcessDate,
            @p_Remarks,
            'P',  -- Posted
            @p_RecordedBy,
            @ProcessDate
        );
        
        SET @p_TransactionID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW 50002, 'Error recording cash receipt', 1;
    END CATCH
END;
GO

-- Procedure: Record Cash Disbursement
IF OBJECT_ID('dbo.usp_RecordCashDisbursement', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RecordCashDisbursement;
GO

CREATE PROCEDURE dbo.usp_RecordCashDisbursement
    @p_CashUnitID BIGINT,
    @p_DisbursementAmount DECIMAL(19,0),
    @p_DisbursementType VARCHAR(100),  -- 'ADVANCE', 'ALLOWANCE', 'PETTY_CASH', etc.
    @p_PayeeID BIGINT,
    @p_ReferenceNumber VARCHAR(50),
    @p_Remarks VARCHAR(500),
    @p_AuthorizedBy BIGINT,
    @p_RecordedBy BIGINT,
    @p_TransactionID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @CurrentCashInHand DECIMAL(19,0);
        
        -- Get current cash in hand
        SET @CurrentCashInHand = dbo.fn_GetCashInHand(@p_CashUnitID, @ProcessDate);
        
        -- Validate amount
        IF @p_DisbursementAmount <= 0
        BEGIN
            THROW 50003, 'Disbursement amount must be greater than zero', 1;
        END
        
        -- Validate sufficient cash
        IF @p_DisbursementAmount > @CurrentCashInHand
        BEGIN
            THROW 50004, 'Insufficient cash in hand. Available: ' + CAST(@CurrentCashInHand AS VARCHAR), 1;
        END
        
        -- Record transaction
        INSERT INTO dbo.CASH_TRANSACTION (
            CASH_UNIT_ID,
            CASH_TXN_TYPE,
            CASH_TXN_AMOUNT,
            CASH_TXN_SOURCE,
            CASH_TXN_PAYEE_ID,
            CASH_TXN_REF_NO,
            CASH_TXN_DATE,
            CASH_TXN_REMARKS,
            CASH_TXN_STATUS,
            AUTHORIZED_BY,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_CashUnitID,
            'D',  -- Disbursement
            @p_DisbursementAmount,
            @p_DisbursementType,
            @p_PayeeID,
            @p_ReferenceNumber,
            @ProcessDate,
            @p_Remarks,
            'P',  -- Posted
            @p_AuthorizedBy,
            @p_RecordedBy,
            @ProcessDate
        );
        
        SET @p_TransactionID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Perform Bank Reconciliation
IF OBJECT_ID('dbo.usp_PerformBankReconciliation', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PerformBankReconciliation;
GO

CREATE PROCEDURE dbo.usp_PerformBankReconciliation
    @p_BankAccountID BIGINT,
    @p_BankStatementBalance DECIMAL(19,0),
    @p_ReconciliationDate DATE,
    @p_ReconciliationBy BIGINT,
    @p_ReconciliationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @LedgerBalance DECIMAL(19,0);
        DECLARE @UnclearedCheques DECIMAL(19,0);
        DECLARE @ComputedBalance DECIMAL(19,0);
        DECLARE @Difference DECIMAL(19,0);
        DECLARE @ReconciliationStatus VARCHAR(10);
        DECLARE @DepositInTransit DECIMAL(19,0) = 0;
        
        -- Get ledger balance
        SET @LedgerBalance = dbo.fn_GetBankBalance(@p_BankAccountID, @p_ReconciliationDate);
        
        -- Get uncleared cheques
        SET @UnclearedCheques = dbo.fn_GetUnclearedChequesTotal(@p_BankAccountID, @p_ReconciliationDate);
        
        -- Compute expected balance
        SET @ComputedBalance = @LedgerBalance - @UnclearedCheques + @DepositInTransit;
        
        -- Calculate difference
        SET @Difference = @p_BankStatementBalance - @ComputedBalance;
        
        -- Determine reconciliation status
        IF @Difference = 0
            SET @ReconciliationStatus = 'R'  -- Reconciled
        ELSE
            SET @ReconciliationStatus = 'D'  -- Difference found
        
        -- Record reconciliation
        INSERT INTO dbo.BANK_RECONCILIATION (
            BANK_ACCOUNT_ID,
            BANK_STATEMENT_BALANCE,
            LEDGER_BALANCE,
            UNCLEARED_CHEQUES,
            DIFFERENCE_AMOUNT,
            RECONCILIATION_STATUS,
            RECONCILIATION_DATE,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_BankAccountID,
            @p_BankStatementBalance,
            @LedgerBalance,
            @UnclearedCheques,
            @Difference,
            @ReconciliationStatus,
            @p_ReconciliationDate,
            @p_ReconciliationBy,
            @ProcessDate
        );
        
        SET @p_ReconciliationID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Issue Cheque
IF OBJECT_ID('dbo.usp_IssueCheque', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_IssueCheque;
GO

CREATE PROCEDURE dbo.usp_IssueCheque
    @p_BankAccountID BIGINT,
    @p_ChequeNumber VARCHAR(20),
    @p_PayeeName VARCHAR(100),
    @p_ChequeAmount DECIMAL(19,0),
    @p_ChequeDate DATE,
    @p_Reference VARCHAR(100),
    @p_IssuedBy BIGINT,
    @p_ChequeID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        
        -- Validate cheque is not duplicate
        IF EXISTS (
            SELECT 1 FROM dbo.CHEQUE_REGISTER
            WHERE BANK_ACCOUNT_ID = @p_BankAccountID
              AND CHEQUE_NUMBER = @p_ChequeNumber
              AND CHEQUE_STATUS <> 'C'  -- Not cancelled
        )
        BEGIN
            THROW 50005, 'Cheque number already exists', 1;
        END
        
        -- Record cheque
        INSERT INTO dbo.CHEQUE_REGISTER (
            BANK_ACCOUNT_ID,
            CHEQUE_NUMBER,
            PAYEE_NAME,
            CHEQUE_AMOUNT,
            CHEQUE_ISSUE_DATE,
            CHEQUE_DATE,
            CHEQUE_REFERENCE,
            CHEQUE_STATUS,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_BankAccountID,
            @p_ChequeNumber,
            @p_PayeeName,
            @p_ChequeAmount,
            CAST(@ProcessDate AS DATE),
            @p_ChequeDate,
            @p_Reference,
            'I',  -- Issued
            @p_IssuedBy,
            @ProcessDate
        );
        
        SET @p_ChequeID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Mark Cheque as Cleared
IF OBJECT_ID('dbo.usp_MarkChequeBounced', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_MarkChequeBounced;
GO

CREATE PROCEDURE dbo.usp_MarkChequeBounced
    @p_ChequeID BIGINT,
    @p_BouncedReason VARCHAR(200),
    @p_ProcessedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        
        UPDATE dbo.CHEQUE_REGISTER
        SET 
            CHEQUE_STATUS = 'B',  -- Bounced
            CHEQUE_BOUNCE_REASON = @p_BouncedReason,
            UPDATED_BY = @p_ProcessedBy,
            UPDATED_ON = @ProcessDate
        WHERE CHEQUE_ID = @p_ChequeID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW 50006, 'Error updating cheque status', 1;
    END CATCH
END;
GO

-- =====================================================
-- TRIGGERS
-- =====================================================

-- Trigger: Validate Cash Transaction
IF OBJECT_ID('dbo.trg_CashTransaction_Validate', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_CashTransaction_Validate;
GO

CREATE TRIGGER dbo.trg_CashTransaction_Validate
ON dbo.CASH_TRANSACTION
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validate transaction type
        IF EXISTS (SELECT 1 FROM inserted WHERE CASH_TXN_TYPE NOT IN ('R', 'D'))
        BEGIN
            THROW 50007, 'Invalid cash transaction type', 1;
        END
        
        -- Validate amount
        IF EXISTS (SELECT 1 FROM inserted WHERE CASH_TXN_AMOUNT <= 0)
        BEGIN
            THROW 50008, 'Transaction amount must be greater than zero', 1;
        END
        
        INSERT INTO dbo.CASH_TRANSACTION (
            CASH_UNIT_ID,
            CASH_TXN_TYPE,
            CASH_TXN_AMOUNT,
            CASH_TXN_SOURCE,
            CASH_TXN_PAYEE_ID,
            CASH_TXN_REF_NO,
            CASH_TXN_DATE,
            CASH_TXN_REMARKS,
            CASH_TXN_STATUS,
            AUTHORIZED_BY,
            CREATED_BY,
            CREATED_ON
        )
        SELECT 
            CASH_UNIT_ID,
            CASH_TXN_TYPE,
            CASH_TXN_AMOUNT,
            CASH_TXN_SOURCE,
            CASH_TXN_PAYEE_ID,
            CASH_TXN_REF_NO,
            CASH_TXN_DATE,
            CASH_TXN_REMARKS,
            CASH_TXN_STATUS,
            AUTHORIZED_BY,
            CREATED_BY,
            CREATED_ON
        FROM inserted;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Trigger: Validate Bank Transaction
IF OBJECT_ID('dbo.trg_BankTransaction_Validate', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_BankTransaction_Validate;
GO

CREATE TRIGGER dbo.trg_BankTransaction_Validate
ON dbo.BANK_TRANSACTION
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validate transaction type
        IF EXISTS (SELECT 1 FROM inserted WHERE BANK_TXN_TYPE NOT IN ('D', 'W'))
        BEGIN
            THROW 50009, 'Invalid bank transaction type', 1;
        END
        
        -- Validate amount
        IF EXISTS (SELECT 1 FROM inserted WHERE BANK_TXN_AMOUNT <= 0)
        BEGIN
            THROW 50010, 'Transaction amount must be greater than zero', 1;
        END
        
        INSERT INTO dbo.BANK_TRANSACTION (
            BANK_ACCOUNT_ID,
            BANK_TXN_TYPE,
            BANK_TXN_AMOUNT,
            BANK_TXN_DATE,
            BANK_TXN_REFERENCE,
            BANK_TXN_REMARKS,
            BANK_TXN_STATUS,
            CREATED_BY,
            CREATED_ON
        )
        SELECT 
            BANK_ACCOUNT_ID,
            BANK_TXN_TYPE,
            BANK_TXN_AMOUNT,
            BANK_TXN_DATE,
            BANK_TXN_REFERENCE,
            BANK_TXN_REMARKS,
            BANK_TXN_STATUS,
            CREATED_BY,
            CREATED_ON
        FROM inserted;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Trigger: Audit Cheque Register Changes
IF OBJECT_ID('dbo.trg_ChequeRegister_Audit', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_ChequeRegister_Audit;
GO

CREATE TRIGGER dbo.trg_ChequeRegister_Audit
ON dbo.CHEQUE_REGISTER
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO dbo.CHEQUE_REGISTER_AUDIT (
        CHEQUE_ID,
        BANK_ACCOUNT_ID,
        CHEQUE_NUMBER,
        PREVIOUS_STATUS,
        NEW_STATUS,
        AUDIT_ACTION,
        AUDIT_DATE
    )
    SELECT 
        i.CHEQUE_ID,
        i.BANK_ACCOUNT_ID,
        i.CHEQUE_NUMBER,
        ISNULL(d.CHEQUE_STATUS, 'NEW'),
        i.CHEQUE_STATUS,
        CASE WHEN d.CHEQUE_ID IS NULL THEN 'INSERT' ELSE 'UPDATE' END,
        GETDATE()
    FROM inserted i
    LEFT JOIN deleted d ON i.CHEQUE_ID = d.CHEQUE_ID;
END;
GO

PRINT 'CASHDB Procedures, Functions & Triggers created successfully.';
GO
