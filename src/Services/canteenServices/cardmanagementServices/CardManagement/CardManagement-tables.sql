-- ==========================================
-- Module: CardManagement
-- Table Scripts
-- ==========================================

-- Table: CANTEEN_CARD_MAP
CREATE TABLE [CANTEEN_CARD_MAP] (
    [CC_SYSID] DECIMAL(38) NULL,
    [CC_CAN_UNT] BIGINT NULL,
    [CC_CRD_NUM] VARCHAR(50) NULL,
    [CC_EFF_DAT] DATETIME2(3) NULL,
    [CC_CLS_DAT] DATETIME2(3) NULL,
    [CC_UPD_USR] DECIMAL(38) NULL,
    [CC_UPD_DAT] DATETIME2(3) NULL
);

-- Table: GUEST_CARD_MASTER
CREATE TABLE [GUEST_CARD_MASTER] (
    [GC_COM_COD] BIGINT NOT NULL,  -- Canteen Unit
    [GC_CRD_SEQ] BIGINT NOT NULL,  -- Card Id Sequence
    [GC_CRD_NUM] VARCHAR(20) NULL,  -- Card No
    [GC_CRD_NAM] VARCHAR(50) NULL,  -- Card NAME
    [GC_REP_UNT] CHAR(3) NULL,  -- Employee reporting Unit
    [GC_CRD_DEP] DECIMAL(38) NULL,  -- Employee reporting Department
    [GC_CRD_TYP] CHAR(1) NULL,  -- Card Type
    [GC_ENT_USR] DECIMAL(38) NULL,  -- Entered By
    [GC_ENT_DAT] DATETIME2(3) NULL,  -- Entered On
    [GC_EFF_DAT] DATETIME2(3) NULL,
    [GC_CLS_DAT] DATETIME2(3) NULL,
    CONSTRAINT [PK_GUEST_CARD_MASTER] PRIMARY KEY ([GC_COM_COD])
);

-- Table: GUEST_CARD_MASTER_HIS
CREATE TABLE [GUEST_CARD_MASTER_HIS] (
    [GC_COM_COD] BIGINT NOT NULL,  -- Canteen Unit
    [GC_CRD_SEQ] BIGINT NOT NULL,  -- Card Id Sequence
    [GC_CRD_NUM] VARCHAR(20) NULL,  -- Card No
    [GC_CRD_NAM] VARCHAR(50) NULL,  -- Card NAME
    [GC_REP_UNT] CHAR(3) NULL,  -- Employee reporting Unit
    [GC_CRD_DEP] DECIMAL(38) NULL,  -- Employee reporting Department
    [GC_CRD_TYP] CHAR(1) NULL,  -- Card Type
    [GC_MOD_USR] DECIMAL(38) NULL,  -- Entered By
    [GC_MOD_ON] DATETIME2(3) NULL  -- Entered On
);

-- Table: CARD_SETTLEMENT
CREATE TABLE [CARD_SETTLEMENT] (
    [ST_SYSID] DECIMAL(38) NULL,
    [ST_CAN_UNT] BIGINT NULL,
    [ST_CRD_NUM] VARCHAR(50) NULL,
    [ST_SET_DAT] DATETIME2(3) NULL,
    [ST_UPD_USR] DECIMAL(38) NULL,
    [ST_UPD_DAT] DATETIME2(3) NULL
);

-- Indexes
CREATE INDEX [IDX_GUEST_CARD_MASTER_GC_CRD_SEQ] ON [GUEST_CARD_MASTER]([GC_CRD_SEQ]);
