-- ==========================================
-- Module: TRAVEL REQUEST
-- Description: Travel planning and request management
-- Tables: Core tables for managing travel requests and plans
-- ==========================================

USE [TRAVELDB];
GO

-- Table: TRAVEL_MAIN - Travel request/plan main
CREATE TABLE [TRAVEL_MAIN] (
    [TR_COM_COD] CHAR(3) NOT NULL  -- Company Code,
    [TR_PLN_NUM] BIGINT NOT NULL  -- Plan No,
    [TR_USR_COD] VARCHAR(20) NULL  -- User ID,
    [TR_USR_NUM] BIGINT NULL  -- User Number,
    [TR_APP_DAT] DATETIME2(3) NULL  -- Applied Date,
    [TR_MOD_DAT] DATETIME2(3) NULL  -- Modified On,
    [TR_MOD_USR] VARCHAR(20) NULL  -- Modified By,
    [TR_NAT_COD] BIGINT NULL  -- Purpose of Travel,
    [TR_OBJ_DES] VARCHAR(200) NULL  -- Desired Objective of the trip - Others if any,
    [TR_REM_MRK] VARCHAR(200) NULL  -- Remarks (If any for the Tour Plan),
    [TR_OUT_COM] VARCHAR(200) NULL  -- Outcome of the trip,
    [TR_BUD_FLG] CHAR(1) NULL  -- Budgeted Activity (Y-Yes;N-No),
    [TR_PLS_FLG] CHAR(1) NULL  -- Status Code E-Edit N-Pending For Approval A-approved C-Cancelled,R-Rejected,F-Pending for Expense Approval ,G-Pending For Finance Approval,J-Rejected at Expense Approval ,S-settled,
    [TR_SET_STS] CHAR(1) NULL  -- Status Code N-New request F-Pending for finance Y-Confirmed by finance,
    [TR_TRP_FLG] CHAR(1) NULL  -- Trip flag,
    [TR_BUD_AMT] DECIMAL(19,0) NULL  -- Budget Amount,
    [TR_ACT_AMT] DECIMAL(19,0) NULL  -- Actual Amount,
    [TR_ADV_AMT] DECIMAL(19,0) NULL  -- Advance applied So far against this plan,
    [TR_PAD_AMT] DECIMAL(19,0) NULL  -- Amount Paid so far against this plan,
    [TR_ADJ_AMT] DECIMAL(19,0) NULL  -- Amount adjusted so far against this plan,
    [TR_REQ_ID] DECIMAL(20,0) NULL  -- Request number,
    [TR_TVL_TYP] CHAR(3) NULL  -- Travel tour plan typ- 'DOM'--domestic, 'INT'--- international,
    [TR_CUR_PRF] CHAR(1) NULL  -- flag for currency preference Y--currency preference specified N-- not specified.,
    [TR_ADD_AMT] DECIMAL(19,0) NULL  -- Requested Additional Amount (special sanction is required),
    [TR_SPL_SNC] CHAR(1) NULL  -- Flag for spl sanction,
    [TR_FIN_UNT] BIGINT NULL  -- Settlement Financial unit,
    [TR_CCR_RMK] VARCHAR(200) NULL,
    [TR_BYPASS_APP] CHAR(1) NULL,
    [TR_ACC_TEN] CHAR(1) NULL,
    [TR_BYPASS_REM] VARCHAR(200) NULL,
    CONSTRAINT [PK_TRAVEL_MAIN] PRIMARY KEY ([TR_PLN_NUM], [TR_COM_COD])
);

-- Table: TRAVEL_SUB - Travel request/plan detail
CREATE TABLE [TRAVEL_SUB] (
    [TR_REQ_NUM] BIGINT NOT NULL  -- Request No,
    [TR_SRL_NUM] BIGINT NOT NULL  -- Tour plan serial No,
    [TR_BOK_NUM] BIGINT NULL  -- Booking Request No,
    [TR_MOD_DAT] DATETIME2(3) NULL  -- Modified Date,
    [TR_CAN_DAT] DATETIME2(3) NULL  -- Cancel date,
    [TR_CAN_REM] VARCHAR(200) NULL  -- Cancel Remarks,
    [TR_ADD_FL1] BIGINT NULL  -- Additional Field -1,
    [TR_ADD_FL2] VARCHAR(65) NULL  -- Additional Field -2,
    [TR_ADD_FL3] VARCHAR(65) NULL  -- Additional Field -3,
    [TR_OND_FLG] CHAR(1) NULL  -- On Duty Status(Y-Yes;N-No),
    CONSTRAINT [PK_TRAVEL_SUB] PRIMARY KEY ([TR_REQ_NUM], [TR_SRL_NUM])
);

-- Table: TRAVEL_PERSONAL - Travel request personal details
CREATE TABLE [TRAVEL_PERSONAL] (
    [TRAVEL_SRLNO] DECIMAL(38) NOT NULL  -- Sno,
    [TRAVEL_REQNUM] DECIMAL(38) NOT NULL  -- Reqnum,
    [TRAVEL_STARTDATE] DATETIME2(3) NULL  -- startdate,
    [TRAVEL_ENDDATE] DATETIME2(3) NULL  -- enddate,
    [TRAVEL_REASON] VARCHAR(2000) NULL  -- reason,
    [TRAVEL_HOURS] DECIMAL(19,0) NULL  -- HOURS,
    CONSTRAINT [PK_TRAVEL_PERSONAL] PRIMARY KEY ([TRAVEL_SRLNO])
);

-- Table: TRAVEL_AGENDA - Travel plan agenda/itinerary
CREATE TABLE [TRAVEL_AGENDA] (
    [TA_REQ_NUM] BIGINT NOT NULL  -- Request number,
    [TA_SRL_NO] INT NOT NULL  -- Srl no,
    [TA_MET_DAT] DATETIME2(3) NULL  -- Meeting date,
    [TA_MET_PPL] VARCHAR(200) NULL  -- People/Party to meet,
    [TA_OUT_COM] VARCHAR(200) NULL  -- Desired outcome of the meeting.,
    [TA_CITY_NAM] VARCHAR(200) NULL  -- city name,
    CONSTRAINT [PK_TRAVEL_AGENDA] PRIMARY KEY ([TA_SRL_NO], [TA_REQ_NUM])
);

-- Table: TRAVEL_ADVANCE - Travel advance payment tracking
CREATE TABLE [TRAVEL_ADVANCE] (
    [AD_REQ_NUM] BIGINT NOT NULL  -- Request_number,
    [AD_ADV_NUM] BIGINT NOT NULL  -- Advance Serial No,
    [AD_ADV_DAT] DATETIME2(3) NULL  -- Advance applied on,
    [AD_ADV_AMT] DECIMAL(19,0) NULL  -- Advance amount applied Now,
    [AD_UNT_COD] BIGINT NULL  -- Advance Applied form which unit,
    [AD_APP_AMT] DECIMAL(19,0) NULL  -- Advance Amount Approved,
    [AD_PAY_AMT] DECIMAL(19,0) NULL  -- Advance Paid amount,
    [AD_PAY_DAT] DATETIME2(3) NULL  -- Advance Paid On,
    [AD_ADV_ADJ] DECIMAL(19,0) NULL  -- Advance Adjusted,
    [AD_PAY_NUM] BIGINT NULL  -- Pay No,
    [AD_PAY_TYP] VARCHAR(255) NULL  -- Pay Type(CHQ/BNK/CSH),
    [AD_EMP_UNT] VARCHAR(255) NULL  -- Employee Unit code,
    [AD_EMP_NUM] BIGINT NULL  -- Employee No,
    [AD_TRN_NUM] BIGINT NULL  -- Accounting transaction Number
);

-- Table: TRAVEL_APPRREMARKS - Travel request approval remarks
CREATE TABLE [TRAVEL_APPRREMARKS] (
    [TR_REQNO] BIGINT NOT NULL  -- Request Number,
    [TR_REQTYP] VARCHAR(10) NULL  -- Request Type,
    [TR_REM] VARCHAR(2000) NULL  -- Remarks,
    [TR_APPBY] VARCHAR(60) NULL  -- Approver name,
    [TR_APP_ON] DATETIME2(3) NULL  -- Entered Date,
    [TR_SRLNO] BIGINT NULL  -- Sno
);

-- Table: AGENDA_TEMP - Temporary agenda data
CREATE TABLE [AGENDA_TEMP] (
    [REQ_NUM] BIGINT NULL,
    [FROM_TIME] DATETIME2(3) NULL,
    [CTY_NAM] VARCHAR(200) NULL,
    [SRL_NO] BIGINT NULL,
    [TO_TIME] DATETIME2(3) NULL,
    [FROM_CTY] VARCHAR(200) NULL
);

-- Table: COMPUTE_DA - Dearness allowance computation (temporary)
CREATE TABLE [COMPUTE_DA] (
    [USER_ID] VARCHAR(25) NULL,
    [PIN_NUM] DECIMAL(20,0) NULL,
    [REQ_NUM] BIGINT NULL,
    [SRL_NO] INT NULL,
    [CITY] VARCHAR(25) NULL,
    [FROM_TIME] VARCHAR(255) NULL,
    [TO_TIME] VARCHAR(255) NULL,
    [NO_OF_DAYS] FLOAT NULL,
    [RATE] BIGINT NULL,
    [TOTAL] BIGINT NULL,
    [FROM_CTY] VARCHAR(200) NULL,
    [REMARKS] VARCHAR(200) NULL
);

-- Table: DASH_TOURPLAN - Dashboard/Report view for tour plans
CREATE TABLE [DASH_TOURPLAN] (
    [TOURDATE] DATETIME2(3) NULL  -- Tour Plan Date,
    [BUSINESS] VARCHAR(10) NULL  -- Business,
    [UNIT] VARCHAR(20) NULL  -- Unit,
    [EMPSYSID] BIGINT NULL  -- Employee System ID,
    [EMPNAME] VARCHAR(200) NULL  -- Employee Name,
    [GRADE] VARCHAR(50) NULL  -- Grade,
    [GRADECATEGORY] VARCHAR(65) NULL  -- Grade Category,
    [TOURNO] BIGINT NULL  -- Tour Plan No,
    [EXPAMT] DECIMAL(19,0) NULL  -- Expenses Amount,
    [NATURE] VARCHAR(200) NULL  -- Travel Nature
);

GO
