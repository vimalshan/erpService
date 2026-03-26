-- ==========================================
-- Module: ItemMaster
-- Table Scripts
-- ==========================================

-- Table: CANTEEN_ITEM_MASTER
CREATE TABLE [CANTEEN_ITEM_MASTER] (
    [CN_COM_COD] BIGINT NOT NULL,  -- Mapped to Cateen Unit
    [CN_ITM_COD] BIGINT NOT NULL,  -- Item Code
    [CN_ITM_DES] CHAR(50) NULL,  -- Item Description
    [CN_ITM_TYP] CHAR(1) NULL,  -- Item Type
    [CN_ITM_REF] CHAR(10) NULL,  -- Item Reference
    [CN_ENT_DAT] DATETIME2(3) NULL,  -- Entered ON
    [CN_ENT_USR] CHAR(50) NULL  -- Entered USER
);

-- Table: CANTEEN_ITEM_PRICE_MASTER
CREATE TABLE [CANTEEN_ITEM_PRICE_MASTER] (
    [CN_COM_COD] BIGINT NOT NULL,  -- Mapped to Cateen Unit
    [CN_ITM_COD] BIGINT NOT NULL,  -- Item Code
    [CN_EMP_CON] DECIMAL(19,0) NULL,  -- Employee Contribution
    [CN_EPR_CON] DECIMAL(19,0) NULL,  -- Employer Contribution
    [CN_EFF_DAT] DATETIME2(3) NOT NULL,  -- Effective Date
    [CN_CLS_DAT] DATETIME2(3) NULL,  -- CLOSURe Date
    [CN_ENT_DAT] DATETIME2(3) NULL,  -- Entered ON
    [CN_ENT_USR] CHAR(50) NULL  -- Entered USER
);

-- Table: CANTEENGRADE_ITEM_PRICE
CREATE TABLE [CANTEENGRADE_ITEM_PRICE] (
    [CN_COM_COD] BIGINT NOT NULL,  -- Canteen Processing Unit
    [CN_ITM_COD] BIGINT NULL,  -- Item Code
    [CN_EMP_CON] DECIMAL(19,0) NULL,  -- Employee Contribution
    [CN_EPR_CON] DECIMAL(19,0) NULL,  -- Employer Contribution
    [CN_EFF_DAT] DATETIME2(3) NULL,  -- Effective Date
    [CN_CLS_DAT] DATETIME2(3) NOT NULL,  -- CLOSURe Date
    [CN_ENT_DAT] DATETIME2(3) NULL,  -- Entered on
    [CN_ENT_USR] CHAR(50) NOT NULL,  -- EnteredBY
    [CN_GRD_TYP] CHAR(3) NOT NULL,
    CONSTRAINT [PK_CANTEENGRADE_ITEM_PRICE] PRIMARY KEY ([CN_COM_COD])
);
