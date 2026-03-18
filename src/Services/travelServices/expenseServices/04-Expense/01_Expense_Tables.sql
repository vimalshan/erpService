-- ==========================================
-- Module: EXPENSE & SETTLEMENT
-- Description: Travel expense management and settlement
-- Tables: Expense tracking and settlement rules
-- ==========================================

USE [TRAVELDB];
GO

-- Table: TRAVEL_EXPENSE - Travel expense records
CREATE TABLE [TRAVEL_EXPENSE] (
    [TR_REQ_NUM] BIGINT NOT NULL  -- Request No,
    [TR_SRL_NUM] BIGINT NOT NULL  -- Expense Serial No,
    [TR_EXP_COD] BIGINT NULL  -- Expense Code,
    [TR_CUR_TYP] CHAR(3) NULL  -- Currency Type,
    [TR_ELG_AMT] BIGINT NULL  -- Eligible Amount,
    [TR_BUD_AMT] DECIMAL(19,0) NULL  -- Budget amount,
    [TR_ACT_UNT] CHAR(3) NULL  -- Expense Met by company,
    [TR_ACT_SLF] DECIMAL(19,0) NULL  -- Expense met by self,
    [TR_VAR_AMT] DECIMAL(19,0) NULL  -- Variance amount,
    [TR_EXP_REM] VARCHAR(200) NULL  -- Expense Remarks,
    [TR_TRN_NUM] BIGINT NULL  -- Accounting Transaction No,
    [TR_EXP_ANX] DECIMAL(19,0) NULL,
    CONSTRAINT [PK_TRAVEL_EXPENSE] PRIMARY KEY ([TR_REQ_NUM], [TR_SRL_NUM])
);

-- Table: TRAVEL_EXPENSEALL - Expense allocation
CREATE TABLE [TRAVEL_EXPENSEALL] (
    [TR_REQ_NUM] BIGINT NULL  -- Request NO,
    [TR_SRL_NUM] BIGINT NULL  -- Expense Allocation Serial NO,
    [TR_EXP_SRL] BIGINT NULL  -- Expense Serial NO,
    [TR_UNT_COD] CHAR(1) NULL  -- Unit Code,
    [TR_CST_COD] CHAR(1) NULL  -- COST Centre Code,
    [TR_ALL_TYP] CHAR(1) NULL  -- Allocation TYPE,
    [TR_ALL_PER] DECIMAL(19,0) NULL  -- Allocation Percentage/VALUE
);

-- Table: TRAVEL_EXPENSESUB - Expense sub details
CREATE TABLE [TRAVEL_EXPENSESUB] (
    [TE_REQ_NUM] BIGINT NULL,
    [TE_SRL_NUM] BIGINT NULL,
    [TE_TYP_EXP] BIGINT NULL,
    [TE_BILL_ATT] CHAR(1) NULL,
    [TE_CIT_NAM] VARCHAR(50) NULL,
    [TE_TOT_AMT] BIGINT NULL,
    [TE_STS_COD] CHAR(1) NULL,
    [TE_REM_TXT] VARCHAR(200) NULL,
    [TE_BILL_DAT] DATETIME2(3) NULL
);

-- Table: TRAVEL_CONVEYANCE - Travel conveyance expenses
CREATE TABLE [TRAVEL_CONVEYANCE] (
    [CONV_SRLNO] BIGINT NOT NULL  -- Srlno,
    [CONV_REQNO] BIGINT NOT NULL  -- Request Number,
    [CONV_DATE] DATETIME2(3) NULL  -- Date,
    [CONV_PARTICULARS] VARCHAR(255) NULL  -- Particulars,
    [CONV_MODE] BIGINT NULL  -- Mode,
    [CONV_AMOUNT] BIGINT NULL  -- Amount,
    [CONV_BOOKNUM] BIGINT NULL  -- Book Request Number,
    [CONV_BOOKSTS] VARCHAR(255) NULL
);

-- Table: TRAVEL_CURRENCY - Travel currency request
CREATE TABLE [TRAVEL_CURRENCY] (
    [TC_REQ_NUM] BIGINT NOT NULL  -- Request number,
    [TC_SRL_NO] INT NOT NULL  -- Srl no.,
    [TC_CUR_COD] VARCHAR(5) NULL  -- Curreny required,
    [TC_CSH_AMT] BIGINT NULL  -- Amount required in cash,
    [TC_TC_AMT] BIGINT NULL  -- Amount required as traveller's cheque,
    [TC_DNM_FLG] CHAR(1) NULL  -- flag,
    [TC_DNM_TXT] VARCHAR(2000) NULL  -- Denomination specification,
    CONSTRAINT [PK_TRAVEL_CURRENCY] PRIMARY KEY ([TC_REQ_NUM], [TC_SRL_NO])
);

-- Table: DA_BREAKUP - Dearness allowance breakup
CREATE TABLE [DA_BREAKUP] (
    [DA_REQ_ID] BIGINT NOT NULL  -- Request ID,
    [DA_SRL_NUM] BIGINT NOT NULL  -- Serial No,
    [DA_FRO_DAT] DATETIME2(3) NULL  -- Start Date,
    [DA_TO_DAT] DATETIME2(3) NULL  -- End Date,
    [DA_TYP_COD] CHAR(3) NULL  -- Type Code : ADM - Admin /SLF - Self,
    [DA_HRS] DECIMAL(19,0) NULL  -- Hours Computed,
    CONSTRAINT [PK_DA_BREAKUP] PRIMARY KEY ([DA_SRL_NUM])
);

-- Table: DA_SUMMARY - DA summary calculation
CREATE TABLE [DA_SUMMARY] (
    [DA_REQID] BIGINT NOT NULL,
    [DA_ADMHRS] DECIMAL(19,0) NOT NULL,
    [DA_ADMDYS] DECIMAL(19,0) NOT NULL,
    [DA_ADMRAT] DECIMAL(19,0) NOT NULL,
    [DA_ADMAMT] DECIMAL(19,0) NOT NULL,
    [DA_SLFHRS] DECIMAL(19,0) NOT NULL,
    [DA_SLFDYS] DECIMAL(19,0) NOT NULL,
    [DA_SLFRAT] DECIMAL(19,0) NOT NULL,
    [DA_SLFAMT] DECIMAL(19,0) NOT NULL,
    CONSTRAINT [PK_DA_SUMMARY] PRIMARY KEY ([DA_REQID])
);

-- Table: EXP_SETTLEMENT - Expense settlement
CREATE TABLE [EXP_SETTLEMENT] (
    [EXP_COD] BIGINT NULL,
    [EXP_NAM] VARCHAR(100) NULL,
    [EXP_BUD] DECIMAL(38) NULL,
    [EXP_CMP] DECIMAL(38) NULL,
    [EXP_SLF] DECIMAL(38) NULL,
    [EXP_ANX] DECIMAL(38) NULL,
    [EXP_REM] VARCHAR(200) NULL,
    [EXP_REM1] VARCHAR(200) NULL
);

-- Table: EXP_SETTLEMENTRPT - Expense settlement report
CREATE TABLE [EXP_SETTLEMENTRPT] (
    [EXP_COD] BIGINT NULL,
    [EXP_NAM] VARCHAR(100) NULL,
    [EXP_BUD] DECIMAL(19,0) NULL,
    [EXP_CMP] DECIMAL(19,0) NULL,
    [EXP_SLF] DECIMAL(19,0) NULL,
    [EXP_ANX] DECIMAL(19,0) NULL,
    [EXP_REM] VARCHAR(200) NULL,
    [REQ_NUM] BIGINT NULL
);

-- Table: RULE_DA - Dearness allowance rules
CREATE TABLE [RULE_DA] (
    [RL_COM_COD] CHAR(3) NULL  -- UNIT CODE,
    [RL_BND_COD] CHAR(3) NULL  -- GRADE CODE,
    [RL_LOC_GRP] CHAR(3) NULL  -- LOCATION GROUP CODE,
    [RL_TYP_COD] CHAR(1) NULL  -- TYPE CODE (D)OMESTIC/(I)NTERNATIONAL,
    [RL_ADM_SLF] CHAR(1) NULL  -- ARRANGEMENT MADE SELF YES/NO,
    [RL_CUR_COD] CHAR(3) NULL  -- CURRENCY CODE,
    [RL_DA_TYP] CHAR(1) NULL  -- DA TYPE,
    [RL_BUD_AMT] DECIMAL(19,0) NULL  -- ELIGIBILITY AMOUNT PER DAY,
    [RL_EFF_DAT] DATETIME2(3) NULL  -- EFFECTIVE DATE,
    [RL_CLS_DAT] DATETIME2(3) NULL  -- CLOSURE DATE
);

-- Table: DA_RULE - DA rules definition
CREATE TABLE [DA_RULE] (
    [RL_SRL_NUM] BIGINT NOT NULL  -- Serial No,
    [RL_BND_ID] BIGINT NOT NULL  -- Band ID,
    [RL_CTR_COD] BIGINT NOT NULL  -- Country Code,
    [RL_SLF_FLG] CHAR(1) NOT NULL  -- Self Booking (Y-Yes,N-No),
    [RL_CUR_COD] CHAR(3) NOT NULL  -- Currency Code,
    [RL_BUD_AMT] DECIMAL(19,0) NOT NULL  -- DA Amount,
    [RL_EFF_DAT] DATETIME2(3) NOT NULL  -- Effective Date,
    [RL_CLS_DAT] DATETIME2(3) NULL  -- CLOSe Date,
    CONSTRAINT [PK_DA_RULE] PRIMARY KEY ([RL_SRL_NUM])
);

-- Table: RULE_MODE - Travel mode rules
CREATE TABLE [RULE_MODE] (
    [RL_COM_COD] CHAR(3) NULL  -- UNIT CODE,
    [RL_BND_COD] CHAR(3) NULL  -- BAND CODE,
    [RL_TYP_COD] CHAR(1) NULL  -- TRAVEL TYPE,
    [RL_MOD_COD] BIGINT NULL  -- MODE OF TRAVEL ALLOWED,
    [RL_CLS_TYP] VARCHAR(200) NULL  -- TRAVEL CLASS ALLOWED,
    [RL_BUD_AMT] DECIMAL(19,0) NULL  -- ELIGIBILITY AMOUNT
);

-- Table: RULE_STAY - Stay eligibility rules
CREATE TABLE [RULE_STAY] (
    [RL_COM_COD] CHAR(3) NULL  -- UNIT CODE,
    [RL_BND_COD] CHAR(3) NULL  -- BAND CODE,
    [RL_STY_TYP] BIGINT NULL  -- STAY TYPE,
    [RL_BUD_AMT] DECIMAL(19,0) NULL  -- ELIGIBILITY AMOUNT PER DAY,
    [RL_EFF_DAT] DATETIME2(3) NULL  -- EFFECTIVE DATE,
    [RL_CLS_DAT] DATETIME2(3) NULL  -- CLOSURE DATE
);

GO
