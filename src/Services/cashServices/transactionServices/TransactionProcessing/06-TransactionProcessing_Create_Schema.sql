-- =============================================
-- TransactionProcessing Service - Database Schema
-- Schema: dbo
-- Database: TransactionProcessingDb
-- =============================================

USE [TransactionProcessingDb]
GO

-- =============================================
-- Table: TRANSACTION_BATCH
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TRANSACTION_BATCH]'))
BEGIN
    CREATE TABLE [dbo].[TRANSACTION_BATCH] (
        [BATCH_ID]              BIGINT          IDENTITY(1,1) NOT NULL,
        [BATCH_TYPE]            NVARCHAR(50)    NOT NULL,           -- DAILY_SETTLEMENT, LOAN_PROCESSING, MANUAL
        [BATCH_DATE]            DATETIME2       NOT NULL,
        [BATCH_STATUS]          NVARCHAR(20)    NOT NULL DEFAULT 'OPEN', -- OPEN, PROCESSING, COMPLETED, FAILED
        [BATCH_TOTAL_COUNT]     INT             NULL,
        [BATCH_SUCCESS_COUNT]   INT             NULL,
        [BATCH_FAILURE_COUNT]   INT             NULL,
        [BATCH_TOTAL_AMOUNT]    DECIMAL(18,4)   NULL,
        [CREATED_BY]            BIGINT          NOT NULL,
        [CREATED_ON]            DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
        [COMPLETED_ON]          DATETIME2       NULL,
        CONSTRAINT [PK_TRANSACTION_BATCH] PRIMARY KEY CLUSTERED ([BATCH_ID])
    )
END
GO

CREATE NONCLUSTERED INDEX [IX_TRANSACTION_BATCH_STATUS] ON [dbo].[TRANSACTION_BATCH] ([BATCH_STATUS])
GO

-- =============================================
-- Table: FINANCIAL_TRANSACTION
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FINANCIAL_TRANSACTION]'))
BEGIN
    CREATE TABLE [dbo].[FINANCIAL_TRANSACTION] (
        [TXN_ID]                BIGINT          IDENTITY(1,1) NOT NULL,
        [TXN_BATCH_ID]          BIGINT          NULL,
        [TXN_TYPE]              NVARCHAR(50)    NOT NULL,           -- SETTLEMENT, DISBURSEMENT, REPAYMENT, CASH_TRANSFER
        [TXN_SUB_TYPE]          NVARCHAR(50)    NULL,
        [TXN_AMOUNT]            DECIMAL(18,4)   NOT NULL,
        [TXN_CURRENCY_ID]       BIGINT          NULL,
        [TXN_EXCHANGE_RATE]     DECIMAL(18,8)   NULL,
        [TXN_BASE_AMOUNT]       DECIMAL(18,4)   NULL,
        [TXN_REFERENCE]         NVARCHAR(100)   NULL,
        [TXN_SOURCE_SERVICE]    NVARCHAR(100)   NOT NULL,
        [TXN_SOURCE_ID]         BIGINT          NULL,
        [TXN_STATUS]            NVARCHAR(20)    NOT NULL DEFAULT 'PENDING', -- PENDING, PROCESSING, COMPLETED, FAILED, REVERSED
        [TXN_REMARKS]           NVARCHAR(500)   NULL,
        [CREATED_BY]            BIGINT          NOT NULL,
        [CREATED_ON]            DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
        [UPDATED_BY]            BIGINT          NULL,
        [UPDATED_ON]            DATETIME2       NULL,
        CONSTRAINT [PK_FINANCIAL_TRANSACTION] PRIMARY KEY CLUSTERED ([TXN_ID]),
        CONSTRAINT [FK_FINANCIAL_TRANSACTION_BATCH] FOREIGN KEY ([TXN_BATCH_ID]) REFERENCES [dbo].[TRANSACTION_BATCH]([BATCH_ID])
    )
END
GO

CREATE NONCLUSTERED INDEX [IX_FINANCIAL_TRANSACTION_BATCH] ON [dbo].[FINANCIAL_TRANSACTION] ([TXN_BATCH_ID])
GO
CREATE NONCLUSTERED INDEX [IX_FINANCIAL_TRANSACTION_STATUS] ON [dbo].[FINANCIAL_TRANSACTION] ([TXN_STATUS])
GO

-- =============================================
-- Table: DEAL_SETTLEMENT_PROC
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DEAL_SETTLEMENT_PROC]'))
BEGIN
    CREATE TABLE [dbo].[DEAL_SETTLEMENT_PROC] (
        [SETTLEMENT_ID]         BIGINT          IDENTITY(1,1) NOT NULL,
        [TXN_ID]                BIGINT          NOT NULL,
        [DEAL_ID]               BIGINT          NOT NULL,
        [SET_ID]                BIGINT          NOT NULL,
        [SETTLEMENT_TYPE]       NVARCHAR(1)     NOT NULL,           -- U (Utilization), C (Cancellation), R (Rollover)
        [SPOT_RATE]             DECIMAL(18,8)   NULL,
        [EXCHANGE_RATE]         DECIMAL(18,8)   NULL,
        [SETTLEMENT_AMOUNT]     DECIMAL(18,4)   NOT NULL,
        [GAIN_LOSS_AMOUNT]      DECIMAL(18,4)   NULL,
        [PREMIUM_AMOUNT]        DECIMAL(18,4)   NULL,
        [WINDING_FEE]           DECIMAL(18,4)   NULL,
        [NET_AMOUNT]            DECIMAL(18,4)   NOT NULL,
        [BANK_ACCOUNT_ID]       BIGINT          NULL,
        [PROCESSING_STATUS]     NVARCHAR(20)    NOT NULL DEFAULT 'PENDING',
        [CREATED_BY]            BIGINT          NOT NULL,
        [CREATED_ON]            DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_DEAL_SETTLEMENT_PROC] PRIMARY KEY CLUSTERED ([SETTLEMENT_ID]),
        CONSTRAINT [FK_DEAL_SETTLEMENT_PROC_TXN] FOREIGN KEY ([TXN_ID]) REFERENCES [dbo].[FINANCIAL_TRANSACTION]([TXN_ID])
    )
END
GO

CREATE NONCLUSTERED INDEX [IX_DEAL_SETTLEMENT_PROC_DEAL] ON [dbo].[DEAL_SETTLEMENT_PROC] ([DEAL_ID])
GO

-- =============================================
-- Table: LOAN_DISBURSEMENT_PROC
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOAN_DISBURSEMENT_PROC]'))
BEGIN
    CREATE TABLE [dbo].[LOAN_DISBURSEMENT_PROC] (
        [DISB_PROC_ID]          BIGINT          IDENTITY(1,1) NOT NULL,
        [TXN_ID]                BIGINT          NOT NULL,
        [LOAN_ID]               BIGINT          NOT NULL,
        [DISB_ID]               BIGINT          NOT NULL,
        [DISB_AMOUNT]           DECIMAL(18,4)   NOT NULL,
        [EXCHANGE_RATE]         DECIMAL(18,8)   NULL,
        [CONVERTED_AMOUNT]      DECIMAL(18,4)   NULL,
        [BANK_ACCOUNT_ID]       BIGINT          NULL,
        [PROCESSING_STATUS]     NVARCHAR(20)    NOT NULL DEFAULT 'PENDING',
        [CREATED_BY]            BIGINT          NOT NULL,
        [CREATED_ON]            DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_LOAN_DISBURSEMENT_PROC] PRIMARY KEY CLUSTERED ([DISB_PROC_ID]),
        CONSTRAINT [FK_LOAN_DISBURSEMENT_PROC_TXN] FOREIGN KEY ([TXN_ID]) REFERENCES [dbo].[FINANCIAL_TRANSACTION]([TXN_ID])
    )
END
GO

CREATE NONCLUSTERED INDEX [IX_LOAN_DISBURSEMENT_PROC_LOAN] ON [dbo].[LOAN_DISBURSEMENT_PROC] ([LOAN_ID])
GO

-- =============================================
-- Table: LOAN_REPAYMENT_PROC
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOAN_REPAYMENT_PROC]'))
BEGIN
    CREATE TABLE [dbo].[LOAN_REPAYMENT_PROC] (
        [REPAY_PROC_ID]         BIGINT          IDENTITY(1,1) NOT NULL,
        [TXN_ID]                BIGINT          NOT NULL,
        [LOAN_ID]               BIGINT          NOT NULL,
        [REPAY_ID]              BIGINT          NOT NULL,
        [REPAY_AMOUNT]          DECIMAL(18,4)   NOT NULL,
        [EXCHANGE_RATE]         DECIMAL(18,8)   NULL,
        [CONVERTED_AMOUNT]      DECIMAL(18,4)   NULL,
        [BANK_ACCOUNT_ID]       BIGINT          NULL,
        [PROCESSING_STATUS]     NVARCHAR(20)    NOT NULL DEFAULT 'PENDING',
        [CREATED_BY]            BIGINT          NOT NULL,
        [CREATED_ON]            DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_LOAN_REPAYMENT_PROC] PRIMARY KEY CLUSTERED ([REPAY_PROC_ID]),
        CONSTRAINT [FK_LOAN_REPAYMENT_PROC_TXN] FOREIGN KEY ([TXN_ID]) REFERENCES [dbo].[FINANCIAL_TRANSACTION]([TXN_ID])
    )
END
GO

CREATE NONCLUSTERED INDEX [IX_LOAN_REPAYMENT_PROC_LOAN] ON [dbo].[LOAN_REPAYMENT_PROC] ([LOAN_ID])
GO

-- =============================================
-- Table: TRANSACTION_AUDIT
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TRANSACTION_AUDIT]'))
BEGIN
    CREATE TABLE [dbo].[TRANSACTION_AUDIT] (
        [AUDIT_ID]              BIGINT          IDENTITY(1,1) NOT NULL,
        [TXN_ID]                BIGINT          NOT NULL,
        [PREVIOUS_STATUS]       NVARCHAR(20)    NOT NULL,
        [NEW_STATUS]            NVARCHAR(20)    NOT NULL,
        [AUDIT_ACTION]          NVARCHAR(200)   NOT NULL,
        [AUDIT_REMARKS]         NVARCHAR(500)   NULL,
        [AUDIT_BY]              BIGINT          NOT NULL,
        [AUDIT_ON]              DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_TRANSACTION_AUDIT] PRIMARY KEY CLUSTERED ([AUDIT_ID]),
        CONSTRAINT [FK_TRANSACTION_AUDIT_TXN] FOREIGN KEY ([TXN_ID]) REFERENCES [dbo].[FINANCIAL_TRANSACTION]([TXN_ID])
    )
END
GO

CREATE NONCLUSTERED INDEX [IX_TRANSACTION_AUDIT_TXN] ON [dbo].[TRANSACTION_AUDIT] ([TXN_ID])
GO

PRINT 'TransactionProcessing schema created successfully.'
GO
