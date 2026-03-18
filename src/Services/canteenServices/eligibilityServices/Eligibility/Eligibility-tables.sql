-- ==========================================
-- Module: Eligibility
-- Table Scripts
-- ==========================================

-- Table: CAN_ELIGIBILITY_MASTER
CREATE TABLE [CAN_ELIGIBILITY_MASTER] (
    [CN_COM_COD] BIGINT NOT NULL  -- Canteen Unit,
    [CN_SFT_COD] CHAR(1) NOT NULL  -- Shift Code,
    [CN_ITM_COD] DECIMAL(38) NOT NULL  -- Item Code,
    [CN_ELG_LMT] INT NULL  -- Eligible Limit,
    [CN_ENT_USR] BIGINT NULL  -- Entered User,
    [CN_ENT_DAT] DATETIME2(3) NULL  -- Entered On,
    [CN_TIM_UNT] CHAR(3) NULL  -- Time Office Unit
);

-- Table: CAN_ELIGIBILITY_MASTER_HIS
CREATE TABLE [CAN_ELIGIBILITY_MASTER_HIS] (
    [CN_COM_COD] BIGINT NOT NULL  -- Canteen Unit,
    [CN_SFT_COD] CHAR(1) NOT NULL  -- Shift Code,
    [CN_ITM_COD] DECIMAL(38) NOT NULL  -- Item Code,
    [CN_ELG_LMT] INT NULL  -- Eligible Limit,
    [CN_MOD_USR] DECIMAL(38) NULL  -- Entered User,
    [CN_MOD_DAT] DATETIME2(3) NULL  -- Entered ON
);

-- Table: CAN_SHIFT_MAPPING
CREATE TABLE [CAN_SHIFT_MAPPING] (
    [CN_COM_COD] BIGINT NOT NULL  -- Company Code,
    [CN_SFT_COD] CHAR(1) NOT NULL  -- Shift Code,
    [CN_SFT_BEF] CHAR(1) NOT NULL  -- Before Shift Code,
    [CN_SFT_AFT] CHAR(1) NOT NULL  -- AFTER Shift Code
);

-- Table: CANTEEN_DAYWISE_ELIGIBILITY
CREATE TABLE [CANTEEN_DAYWISE_ELIGIBILITY] (
    [CN_SRL_NUM] BIGINT NOT NULL  -- Serial Number,
    [CN_COM_COD] BIGINT NOT NULL  -- Company Code,
    [CN_SYS_ID] BIGINT NOT NULL  -- Employee Number,
    [CN_ATT_DAT] DATETIME2(3) NULL  -- Employee Type,
    [CN_PRC_NUM] BIGINT NULL  -- Canteen Swipe Date,
    [CN_SFT_COD] CHAR(1) NULL  -- Item Code,
    [CN_ITM_COD] BIGINT NULL  -- Item Type,
    [CN_SFT_QTY] INT NULL  -- Employee Contribution,
    [CN_SFT_BEF] INT NULL  -- Employer Contribution,
    [CN_SFT_AFT] INT NULL  -- Canteen Number,
    [CN_ENT_USR] BIGINT NULL  -- Item Quantity,
    [CN_ENT_DAT] DATETIME2(3) NULL  -- Entry User,
    [CN_FLEX1] VARCHAR(20) NULL  -- Entry Date,
    [CN_GRD_TYP] CHAR(3) NULL  -- Flex Field,
    CONSTRAINT [PK_CANTEEN_DAYWISE_ELIGIBILITY] PRIMARY KEY ([CN_SRL_NUM])
);

-- Indexes
CREATE INDEX [IDX_CANTEEN_DAYWISE_ELIGIBILITY_CN_COM_COD] ON [CANTEEN_DAYWISE_ELIGIBILITY]([CN_COM_COD]);
