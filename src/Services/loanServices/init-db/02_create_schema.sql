-- =============================================================================
--  02_create_schema.sql
--  Creates all LOANDB tables (idempotent — checks existence before creating).
--  Sourced from LOANDB.sql with IF NOT EXISTS guards added.
-- =============================================================================
USE LOANDB;
GO

SET NOCOUNT ON;
GO

-- ── LOV / Reference Data ─────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOV_CATEGORY')
BEGIN
    CREATE TABLE [LOV_CATEGORY] (
        [LOVC_CATID]       BIGINT       NOT NULL,
        [LOVC_CATCODE]     CHAR(1)      NOT NULL,
        [LOVC_CATDESC]     VARCHAR(100) NOT NULL,
        [LOVC_MODIFIEDBY]  BIGINT       NOT NULL,
        [LOVC_MODIFIEDON]  DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_LOV_CATEGORY] PRIMARY KEY ([LOVC_CATID])
    );
    PRINT 'Table LOV_CATEGORY created.';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOV_DETAILS')
BEGIN
    CREATE TABLE [LOV_DETAILS] (
        [LOVD_DETID]       BIGINT       NOT NULL,
        [LOVD_CATID]       BIGINT       NOT NULL,
        [LOVD_DETCODE]     VARCHAR(10)  NOT NULL,
        [LOVD_DETDESC]     VARCHAR(200) NOT NULL,
        [LOVD_ACTIVE]      CHAR(1)      NOT NULL DEFAULT ('Y'),
        [LOVD_MODIFIEDBY]  BIGINT       NOT NULL,
        [LOVD_MODIFIEDON]  DATETIME2(3) NOT NULL,
        CONSTRAINT [PK_LOV_DETAILS] PRIMARY KEY ([LOVD_DETID])
    );
    PRINT 'Table LOV_DETAILS created.';
END

-- ── Loan Application ──────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOAN_APPLICATION')
BEGIN
    CREATE TABLE [LOAN_APPLICATION] (
        [LOAN_APPID]        BIGINT         NOT NULL,
        [LOAN_EMPSYSID]     BIGINT         NOT NULL,
        [LOAN_ID]           BIGINT         NOT NULL,
        [LOAN_APPLIEDBY]    BIGINT         NOT NULL,
        [LOAN_APPLIEDON]    DATETIME2(3)   NOT NULL,
        [LOAN_SOURCE]       CHAR(3)        NOT NULL,
        [LOAN_AMOUNT]       BIGINT         NOT NULL,
        [LOAN_SUBCLASSID]   BIGINT         NULL,
        [LOAN_REASON]       VARCHAR(200)   NOT NULL,
        [LOAN_APPSTATUS]    CHAR(1)        NOT NULL DEFAULT ('P'),
        [LOAN_GUARANTOR]    BIGINT         NOT NULL,
        [LOAN_APRREMARKS]   VARCHAR(200)   NULL,
        [LOAN_REQUIREDBY]   BIGINT         NOT NULL,
        [LOAN_APPROVEDBY]   BIGINT         NULL,
        [LOAN_APPROVEDON]   DATETIME2(3)   NULL,
        [LOAN_MODIFIEDBY]   BIGINT         NOT NULL,
        [LOAN_MODIFIEDON]   DATETIME2(3)   NOT NULL,
        [LOAN_TENURE]       BIGINT         NULL,
        [LOAN_GUARANTOR2]   BIGINT         NULL,
        [LOAN_SPLSANCTION]  CHAR(1)        NULL,
        CONSTRAINT [PK_LOAN_APPLICATION] PRIMARY KEY ([LOAN_APPID])
    );
    PRINT 'Table LOAN_APPLICATION created.';
END

-- ── Loan Master / Definition ──────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOAN_MASTER')
BEGIN
    CREATE TABLE [LOAN_MASTER] (
        [LOAN_LOANID]         BIGINT         NOT NULL,
        [LOAN_LOANCODE]       VARCHAR(20)    NOT NULL,
        [LOAN_LOANDESC]       VARCHAR(200)   NOT NULL,
        [LOAN_LOANTYPE]       CHAR(1)        NOT NULL,
        [LOAN_ACTIVE]         CHAR(1)        NOT NULL DEFAULT ('Y'),
        [LOAN_MAXAMT]         DECIMAL(18,2)  NOT NULL DEFAULT (0),
        [LOAN_MINAMT]         DECIMAL(18,2)  NOT NULL DEFAULT (0),
        [LOAN_MAXTENURE]      INT            NOT NULL DEFAULT (0),
        [LOAN_MINTENURE]      INT            NOT NULL DEFAULT (0),
        [LOAN_INTRATE]        DECIMAL(5,2)   NOT NULL DEFAULT (0),
        [LOAN_COMPFACTOR]     CHAR(1)        NOT NULL DEFAULT ('S'),
        [LOAN_INTFREQ]        CHAR(1)        NOT NULL DEFAULT ('M'),
        [LOAN_MODIFIEDBY]     BIGINT         NOT NULL,
        [LOAN_MODIFIEDON]     DATETIME2(3)   NOT NULL,
        CONSTRAINT [PK_LOAN_MASTER] PRIMARY KEY ([LOAN_LOANID])
    );
    PRINT 'Table LOAN_MASTER created.';
END

-- ── Loan Account / Transaction ────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOAN_ACCOUNT')
BEGIN
    CREATE TABLE [LOAN_ACCOUNT] (
        [LOANACCT_LOANNO]       BIGINT         NOT NULL,
        [LOANACCT_EMPSYSID]     BIGINT         NOT NULL,
        [LOANACCT_LOANID]       BIGINT         NOT NULL,
        [LOANACCT_APPID]        BIGINT         NULL,
        [LOANACCT_DISBDATE]     DATETIME2(3)   NOT NULL,
        [LOANACCT_DISBAMT]      DECIMAL(18,2)  NOT NULL,
        [LOANACCT_INTRATE]      DECIMAL(5,2)   NOT NULL,
        [LOANACCT_TENURE]       INT            NOT NULL,
        [LOANACCT_STATUS]       CHAR(1)        NOT NULL DEFAULT ('A'),
        [LOANACCT_COMPFACTOR]   CHAR(1)        NOT NULL DEFAULT ('S'),
        [LOANACCT_INTFREQ]      CHAR(1)        NOT NULL DEFAULT ('M'),
        [LOANACCT_REASON]       VARCHAR(200)   NULL,
        [LOANACCT_FIRSTINSDATE] DATETIME2(3)   NOT NULL,
        [LOANACCT_MODIFIEDBY]   BIGINT         NOT NULL,
        [LOANACCT_MODIFIEDON]   DATETIME2(3)   NOT NULL,
        CONSTRAINT [PK_LOAN_ACCOUNT] PRIMARY KEY ([LOANACCT_LOANNO])
    );
    PRINT 'Table LOAN_ACCOUNT created.';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOAN_INS')
BEGIN
    CREATE TABLE [LOAN_INS] (
        [LOANINS_ID]        BIGINT         NOT NULL,
        [LOANINS_UNITID]    BIGINT         NOT NULL,
        [LOANINS_LOANNO]    BIGINT         NOT NULL,
        [LOANINS_INSDATE]   DATETIME2(3)   NOT NULL,
        [LOANINS_INSNO]     BIGINT         NOT NULL,
        [LOANINS_INSAMT]    DECIMAL(18,2)  NOT NULL,
        [LOANINS_PRNOUT]    DECIMAL(18,2)  NOT NULL,
        [LOANINS_PRNADJ]    DECIMAL(18,2)  NOT NULL,
        [LOANINS_INTADJ]    DECIMAL(18,2)  NOT NULL,
        [LOANINS_FRODATE]   DATETIME2(3)   NULL,
        [LOANINS_INTACC]    DECIMAL(18,2)  NOT NULL,
        [LOANINS_UPDATEDBY] BIGINT         NOT NULL,
        [LOANINS_UPDATEDON] DATETIME2(3)   NOT NULL,
        [LOANINS_INTREC]    DECIMAL(18,2)  NOT NULL,
        [LOANINS_PRNREC]    DECIMAL(18,2)  NOT NULL,
        [LOANINS_INTRATE]   INT            NOT NULL,
        [LOANINS_STATUS]    CHAR(1)        NOT NULL DEFAULT ('P'),
        CONSTRAINT [PK_LOAN_INS] PRIMARY KEY ([LOANINS_ID])
    );
    PRINT 'Table LOAN_INS created.';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOAN_LEDGER')
BEGIN
    CREATE TABLE [LOAN_LEDGER] (
        [LOANLGR_ID]        BIGINT         NOT NULL,
        [LOANLGR_LOANNO]    BIGINT         NOT NULL,
        [LOANLGR_TRANSDATE] DATETIME2(3)   NOT NULL,
        [LOANLGR_TRANSTYPE] CHAR(1)        NOT NULL,
        [LOANLGR_DEBIT]     DECIMAL(18,2)  NOT NULL DEFAULT (0),
        [LOANLGR_CREDIT]    DECIMAL(18,2)  NOT NULL DEFAULT (0),
        [LOANLGR_BALANCE]   DECIMAL(18,2)  NOT NULL DEFAULT (0),
        [LOANLGR_REMARK]    VARCHAR(200)   NULL,
        [LOANLGR_MODIFIEDBY] BIGINT        NOT NULL,
        [LOANLGR_MODIFIEDON] DATETIME2(3)  NOT NULL,
        CONSTRAINT [PK_LOAN_LEDGER] PRIMARY KEY ([LOANLGR_ID])
    );
    PRINT 'Table LOAN_LEDGER created.';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOAN_SETTLEMENT')
BEGIN
    CREATE TABLE [LOAN_SETTLEMENT] (
        [LOANSTL_ID]          BIGINT         NOT NULL,
        [LOANSTL_LOANNO]      BIGINT         NOT NULL,
        [LOANSTL_SETTLEDATE]  DATETIME2(3)   NOT NULL,
        [LOANSTL_SETTLEAMT]   DECIMAL(18,2)  NOT NULL,
        [LOANSTL_WAIVEAMT]    DECIMAL(18,2)  NOT NULL DEFAULT (0),
        [LOANSTL_STATUS]      CHAR(1)        NOT NULL DEFAULT ('P'),
        [LOANSTL_MODIFIEDBY]  BIGINT         NOT NULL,
        [LOANSTL_MODIFIEDON]  DATETIME2(3)   NOT NULL,
        CONSTRAINT [PK_LOAN_SETTLEMENT] PRIMARY KEY ([LOANSTL_ID])
    );
    PRINT 'Table LOAN_SETTLEMENT created.';
END

-- ── Documents ─────────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOAN_DOCUMENTS')
BEGIN
    CREATE TABLE [LOAN_DOCUMENTS] (
        [LOANDOC_ID]              BIGINT         NOT NULL,
        [LOANDOC_LOANID]          BIGINT         NOT NULL,
        [LOANDOC_TYPEID]          BIGINT         NOT NULL,
        [LOANDOC_FILENAME]        VARCHAR(500)   NULL,
        [LOANDOC_BLOBURL]         VARCHAR(1000)  NULL,
        [LOANDOC_LASTMODIFIEDBY]  BIGINT         NOT NULL,
        [LOANDOC_LASTMODIFIEDON]  DATETIME2(3)   NOT NULL,
        CONSTRAINT [PK_LOAN_DOCUMENTS] PRIMARY KEY ([LOANDOC_ID])
    );
    PRINT 'Table LOAN_DOCUMENTS created.';
END

-- ── Foreign Keys ──────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys
    WHERE name = 'FK_LOAN_INS_ACCOUNT' AND parent_object_id = OBJECT_ID('LOAN_INS'))
BEGIN
    ALTER TABLE [LOAN_INS]
        ADD CONSTRAINT [FK_LOAN_INS_ACCOUNT]
        FOREIGN KEY ([LOANINS_LOANNO]) REFERENCES [LOAN_ACCOUNT]([LOANACCT_LOANNO]);
END

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys
    WHERE name = 'FK_LOAN_LEDGER_ACCOUNT' AND parent_object_id = OBJECT_ID('LOAN_LEDGER'))
BEGIN
    ALTER TABLE [LOAN_LEDGER]
        ADD CONSTRAINT [FK_LOAN_LEDGER_ACCOUNT]
        FOREIGN KEY ([LOANLGR_LOANNO]) REFERENCES [LOAN_ACCOUNT]([LOANACCT_LOANNO]);
END

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys
    WHERE name = 'FK_LOAN_SETTLEMENT_ACCOUNT' AND parent_object_id = OBJECT_ID('LOAN_SETTLEMENT'))
BEGIN
    ALTER TABLE [LOAN_SETTLEMENT]
        ADD CONSTRAINT [FK_LOAN_SETTLEMENT_ACCOUNT]
        FOREIGN KEY ([LOANSTL_LOANNO]) REFERENCES [LOAN_ACCOUNT]([LOANACCT_LOANNO]);
END

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys
    WHERE name = 'FK_LOAN_ACCOUNT_MASTER' AND parent_object_id = OBJECT_ID('LOAN_ACCOUNT'))
BEGIN
    ALTER TABLE [LOAN_ACCOUNT]
        ADD CONSTRAINT [FK_LOAN_ACCOUNT_MASTER]
        FOREIGN KEY ([LOANACCT_LOANID]) REFERENCES [LOAN_MASTER]([LOAN_LOANID]);
END

PRINT 'Schema creation complete.';
GO
