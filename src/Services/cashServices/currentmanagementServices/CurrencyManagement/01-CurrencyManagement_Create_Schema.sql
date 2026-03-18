-- ==========================================
-- Module: CurrencyManagement
-- Purpose: Currency Master and Exchange Rate Management
-- Created: March 9, 2026
-- Database: CASHDB
-- ==========================================

USE CASHDB;
GO

-- =====================================================
-- CREATE TABLES FOR CURRENCY MANAGEMENT MODULE
-- =====================================================

-- Table: DEAL_CURRMAST - Currency Master Data
CREATE TABLE [DEAL_CURRMAST] (
    [CURR_ID] BIGINT NOT NULL  -- Currency ID,
    [CURR_NAME] VARCHAR(255) NOT NULL  -- Currency Name,
    [CURR_SYMBOL] VARCHAR(25) NOT NULL  -- Currency Symbol,
    [CURR_MODIFIEDBY] BIGINT NOT NULL  -- Modified By,
    [CURR_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    CONSTRAINT [PK_DEAL_CURRMAST] PRIMARY KEY ([CURR_ID])
);

-- Table: DEAL_CURRATES - Currency Exchange Rates
CREATE TABLE [DEAL_CURRATES] (
    [CURRATE_ID] BIGINT NOT NULL  -- Rate ID,
    [CURRATE_FINYEAR] BIGINT NOT NULL,
    [CURRATE_MONTH] BIGINT NOT NULL,
    [CURRATE_FROMCUR] BIGINT NOT NULL  -- From Currency,
    [CURRATE_TOCUR] BIGINT NOT NULL  -- To Currency,
    [CURRATE_RATE] DECIMAL(19,0) NOT NULL  -- From Currency Rate,
    [CURRATE_MODIFIEDBY] DECIMAL(38) NOT NULL,
    [CURRATE_MODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_DEAL_CURRATES] PRIMARY KEY ([CURRATE_ID])
);

-- Table: DEAL_ORGCURRMAP - Organization Currency Mapping
CREATE TABLE [DEAL_ORGCURRMAP] (
    [ORG_ID] BIGINT NOT NULL  -- Organization ID,
    [ORG_CURRID] BIGINT NOT NULL  -- Currency ID,
    [ORG_MODIFIEDBY] DECIMAL(38) NOT NULL  -- Modified By,
    [ORG_MODIFIEDON] DATETIME2(3) NOT NULL  -- Modified On,
    CONSTRAINT [FK_DEAL_ORGCURRMAP_CURRMAST] FOREIGN KEY ([ORG_CURRID]) REFERENCES [DEAL_CURRMAST]([CURR_ID])
);

-- =====================================================
-- CREATE INDEXES
-- =====================================================

CREATE INDEX [IX_DEAL_CURRATES_FINYEAR_MONTH] ON [DEAL_CURRATES] ([CURRATE_FINYEAR], [CURRATE_MONTH]);
CREATE INDEX [IX_DEAL_CURRATES_FROMCUR_TOCUR] ON [DEAL_CURRATES] ([CURRATE_FROMCUR], [CURRATE_TOCUR]);
CREATE INDEX [IX_DEAL_ORGCURRMAP_ORG_ID] ON [DEAL_ORGCURRMAP] ([ORG_ID]);

-- =====================================================
-- VERIFICATION
-- =====================================================

PRINT 'CurrencyManagement Module Schema created successfully.';
GO

-- Verify table creation
IF OBJECT_ID('DEAL_CURRMAST', 'U') IS NOT NULL
    PRINT 'Table DEAL_CURRMAST: OK'
ELSE
    PRINT 'Table DEAL_CURRMAST: FAILED'
GO

IF OBJECT_ID('DEAL_CURRATES', 'U') IS NOT NULL
    PRINT 'Table DEAL_CURRATES: OK'
ELSE
    PRINT 'Table DEAL_CURRATES: FAILED'
GO

IF OBJECT_ID('DEAL_ORGCURRMAP', 'U') IS NOT NULL
    PRINT 'Table DEAL_ORGCURRMAP: OK'
ELSE
    PRINT 'Table DEAL_ORGCURRMAP: FAILED'
GO
