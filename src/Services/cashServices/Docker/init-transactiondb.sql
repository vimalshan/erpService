-- ============================================================================
-- TransactionProcessingDb Initialization Script
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TransactionProcessingDb')
BEGIN
    CREATE DATABASE TransactionProcessingDb;
END
GO

USE TransactionProcessingDb;
GO

-- Tables are auto-created by EF Core migrations
-- This script serves as fallback for manual DB setup

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TransactionBatches')
BEGIN
    CREATE TABLE [dbo].[TransactionBatches] (
        [BatchId] BIGINT PRIMARY KEY IDENTITY(1,1),
        [BatchType] NVARCHAR(50) NOT NULL,
        [BatchDate] DATETIME2(7) NOT NULL,
        [BatchStatus] NVARCHAR(20) NOT NULL DEFAULT 'OPEN',
        [BatchTotalCount] INT NULL,
        [BatchSuccessCount] INT NULL,
        [BatchFailureCount] INT NULL,
        [BatchTotalAmount] DECIMAL(18,4) NULL,
        [CreatedBy] BIGINT NOT NULL,
        [CreatedOn] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] BIGINT NULL,
        [UpdatedOn] DATETIME2(7) NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
BEGIN
    CREATE TABLE [dbo].[FinancialTransactions] (
        [TxnId] BIGINT PRIMARY KEY IDENTITY(1,1),
        [TxnBatchId] BIGINT NULL,
        [TxnType] NVARCHAR(50) NOT NULL,
        [TxnSubType] NVARCHAR(50) NULL,
        [TxnAmount] DECIMAL(18,4) NOT NULL,
        [TxnCurrencyId] BIGINT NULL,
        [TxnExchangeRate] DECIMAL(18,8) NOT NULL DEFAULT 1.0,
        [TxnBaseAmount] DECIMAL(18,4) NOT NULL,
        [TxnReference] NVARCHAR(200) NULL,
        [TxnSourceService] NVARCHAR(100) NOT NULL,
        [TxnSourceId] BIGINT NULL,
        [TxnStatus] NVARCHAR(20) NOT NULL DEFAULT 'PENDING',
        [TxnRemarks] NVARCHAR(500) NULL,
        [CreatedBy] BIGINT NOT NULL,
        [CreatedOn] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] BIGINT NULL,
        [UpdatedOn] DATETIME2(7) NULL,
        CONSTRAINT FK_FT_Batch FOREIGN KEY ([TxnBatchId]) REFERENCES [TransactionBatches]([BatchId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DealSettlements')
BEGIN
    CREATE TABLE [dbo].[DealSettlements] (
        [SettlementId] BIGINT PRIMARY KEY IDENTITY(1,1),
        [TxnId] BIGINT NOT NULL,
        [DealId] BIGINT NOT NULL,
        [SetId] BIGINT NOT NULL,
        [SettlementType] CHAR(1) NOT NULL DEFAULT 'U',
        [SpotRate] DECIMAL(18,8) NULL,
        [ExchangeRate] DECIMAL(18,8) NULL,
        [SettlementAmount] DECIMAL(18,4) NOT NULL,
        [GainLossAmount] DECIMAL(18,4) NULL,
        [PremiumAmount] DECIMAL(18,4) NULL,
        [WindingFee] DECIMAL(18,4) NULL,
        [NetAmount] DECIMAL(18,4) NOT NULL,
        [BankAccountId] BIGINT NULL,
        [ProcessingStatus] NVARCHAR(20) NOT NULL DEFAULT 'PENDING',
        [CreatedBy] BIGINT NOT NULL,
        [CreatedOn] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] BIGINT NULL,
        [UpdatedOn] DATETIME2(7) NULL,
        CONSTRAINT FK_DS_Txn FOREIGN KEY ([TxnId]) REFERENCES [FinancialTransactions]([TxnId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LoanDisbursements')
BEGIN
    CREATE TABLE [dbo].[LoanDisbursements] (
        [DisbProcId] BIGINT PRIMARY KEY IDENTITY(1,1),
        [TxnId] BIGINT NOT NULL,
        [LoanId] BIGINT NOT NULL,
        [DisbId] BIGINT NOT NULL,
        [DisbAmount] DECIMAL(18,4) NOT NULL,
        [ExchangeRate] DECIMAL(18,8) NOT NULL DEFAULT 1.0,
        [ConvertedAmount] DECIMAL(18,4) NOT NULL,
        [BankAccountId] BIGINT NULL,
        [ProcessingStatus] NVARCHAR(20) NOT NULL DEFAULT 'PENDING',
        [CreatedBy] BIGINT NOT NULL,
        [CreatedOn] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] BIGINT NULL,
        [UpdatedOn] DATETIME2(7) NULL,
        CONSTRAINT FK_LD_Txn FOREIGN KEY ([TxnId]) REFERENCES [FinancialTransactions]([TxnId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LoanRepayments')
BEGIN
    CREATE TABLE [dbo].[LoanRepayments] (
        [RepayProcId] BIGINT PRIMARY KEY IDENTITY(1,1),
        [TxnId] BIGINT NOT NULL,
        [LoanId] BIGINT NOT NULL,
        [RepayId] BIGINT NOT NULL,
        [RepayAmount] DECIMAL(18,4) NOT NULL,
        [ExchangeRate] DECIMAL(18,8) NOT NULL DEFAULT 1.0,
        [ConvertedAmount] DECIMAL(18,4) NOT NULL,
        [BankAccountId] BIGINT NULL,
        [ProcessingStatus] NVARCHAR(20) NOT NULL DEFAULT 'PENDING',
        [CreatedBy] BIGINT NOT NULL,
        [CreatedOn] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] BIGINT NULL,
        [UpdatedOn] DATETIME2(7) NULL,
        CONSTRAINT FK_LR_Txn FOREIGN KEY ([TxnId]) REFERENCES [FinancialTransactions]([TxnId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TransactionAudits')
BEGIN
    CREATE TABLE [dbo].[TransactionAudits] (
        [AuditId] BIGINT PRIMARY KEY IDENTITY(1,1),
        [TxnId] BIGINT NOT NULL,
        [PreviousStatus] NVARCHAR(20) NOT NULL,
        [NewStatus] NVARCHAR(20) NOT NULL,
        [AuditAction] NVARCHAR(100) NULL,
        [AuditRemarks] NVARCHAR(500) NULL,
        [AuditBy] BIGINT NOT NULL,
        [AuditOn] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_TA_Txn FOREIGN KEY ([TxnId]) REFERENCES [FinancialTransactions]([TxnId])
    );
END
GO

PRINT 'TransactionProcessingDb initialization completed successfully.'
GO
