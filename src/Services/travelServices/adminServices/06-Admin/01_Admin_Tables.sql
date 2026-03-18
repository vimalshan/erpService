-- ==========================================
-- Module: ADMINISTRATION & CONFIGURATION
-- Description: Admin units, user access, and configuration
-- Tables: Admin data and configuration management
-- ==========================================

USE [TRAVELDB];
GO

-- Table: TRAVEL_ADMIN_UNITS - Travel admin units
CREATE TABLE [TRAVEL_ADMIN_UNITS] (
    [AD_ADM_COD] BIGINT NOT NULL  -- Admin Unit Code,
    [AD_ADM_NAM] VARCHAR(50) NULL  -- Admin Unit Name,
    [AD_ADM_TYP] VARCHAR(1) NULL  -- T-Travel ,S-Stay,M- Meeting,
    [AD_ADM_UNT] CHAR(3) NULL,
    [AD_ADM_CAB] BIGINT NULL  -- Cab Unit,
    [AD_IMG_URL] VARCHAR(150) NULL,
    [AD_SORT_BY] BIGINT NULL
);

-- Table: TRAVEL_ADMIN_ACCESS - Admin access configuration
CREATE TABLE [TRAVEL_ADMIN_ACCESS] (
    [TR_ADM_COD] BIGINT NULL  -- Admin Unit Code,
    [TR_COM_COD] CHAR(3) NULL  -- Unit Code,
    [TR_LOC_USR] VARCHAR(20) NULL  -- Admin User code,
    [TR_LOC_COD] BIGINT NULL  -- Location Code,
    [TR_CNT_EML] VARCHAR(100) NULL  -- emailids,
    [TR_EMPSYSID] BIGINT NULL
);

-- Table: TRAVEL_ADMIN_CONTACT - Admin contact details
CREATE TABLE [TRAVEL_ADMIN_CONTACT] (
    [AD_ADM_COD] BIGINT NOT NULL  -- Admin Code,
    [AD_ADM_SRL] BIGINT NOT NULL  -- Admin Srl No,
    [AD_USR_ID] VARCHAR(50) NULL  -- User id,
    [AD_PIN_NUM] BIGINT NULL  -- Pin_num,
    [AD_CNT_PHN1] VARCHAR(50) NULL  -- Contact Phone1,
    [AD_CNT_PHN2] VARCHAR(50) NULL  -- Contact Phone2,
    [AD_CNT_TYP] VARCHAR(50) NULL  -- Contact Type,
    [AD_RES_TYP] BIGINT NULL
);

-- Table: TRAVEL_FINANCE_UNITS - Finance units
CREATE TABLE [TRAVEL_FINANCE_UNITS] (
    [TR_UNT_ID] BIGINT NOT NULL  -- UNIT ID,
    [TR_UNT_COD] CHAR(3) NULL  -- UNIT CODE,
    [TR_UNT_NAM] VARCHAR(50) NULL  -- UNIT NAME,
    [TR_ORA_COD] BIGINT NULL  -- ORACLE CODE FOR THE UNIT,
    [TR_LOC_OPTION] CHAR(1) NULL  -- Location Segment code,
    CONSTRAINT [PK_TRAVEL_FINANCE_UNITS] PRIMARY KEY ([TR_UNT_ID])
);

-- Table: TRAVEL_FINANCE_ACCESS - Finance access control
CREATE TABLE [TRAVEL_FINANCE_ACCESS] (
    [TR_FIN_NO] BIGINT NOT NULL  -- Finance id(unique),
    [TR_UNT_ID] BIGINT NULL  -- Finance unit,
    [TR_USR_ID] VARCHAR(20) NULL  -- User id,
    [TR_USR_NUM] DECIMAL(38) NULL  -- pin number,
    [TR_FIN_EML_ID] VARCHAR(30) NULL,
    CONSTRAINT [PK_TRAVEL_FINANCE_ACCESS] PRIMARY KEY ([TR_FIN_NO])
);

-- Table: ACC_MASTER - Account master
CREATE TABLE [ACC_MASTER] (
    [AC_COM_COD] CHAR(3) NULL,
    [AC_ED_COD] CHAR(6) NULL,
    [AC_ACC_COD] CHAR(6) NULL,
    [AC_GRD_TYP] CHAR(3) NULL,
    [AC_DC_FLG] CHAR(1) NULL,
    [AC_SUB_COD] CHAR(6) NULL,
    [AC_ACC_DES] VARCHAR(200) NULL
);

-- Table: COMP_COUNTER - Company counter
CREATE TABLE [COMP_COUNTER] (
    [CM_COM_COD] CHAR(3) NOT NULL  -- Company Code,
    [CM_CTR_COD] CHAR(3) NOT NULL  -- Number Code,
    [CM_CTR_NUM] INT NOT NULL  -- Running Number,
    [CM_CTR_DES] VARCHAR(200) NOT NULL  -- Description,
    CONSTRAINT [PK_COMP_COUNTER] PRIMARY KEY ([CM_COM_COD])
);

-- Table: BAND_TYPE_CLASS_MAPPING - Band type class mapping
CREATE TABLE [BAND_TYPE_CLASS_MAPPING] (
    [BAND] CHAR(1) NOT NULL,
    [TYPE] DECIMAL(38) NOT NULL,
    [CLASS] DECIMAL(38) NOT NULL
);

-- Table: AREA_MASTER - Area master
CREATE TABLE [AREA_MASTER] (
    [AREA_ID] INT NOT NULL,
    [AREA_NAME] VARCHAR(200) NOT NULL
);

-- Table: AREA_ROUTE_MAP - Area route mapping
CREATE TABLE [AREA_ROUTE_MAP] (
    [ROUTE_ID] INT NOT NULL,
    [AREA_ID] INT NOT NULL
);

-- Table: ROUTE_MASTER - Route master
CREATE TABLE [ROUTE_MASTER] (
    [ROUTE_ID] INT NOT NULL,
    [ROUTE_NAME] VARCHAR(200) NOT NULL,
    CONSTRAINT [PK_ROUTE_MASTER] PRIMARY KEY ([ROUTE_ID])
);

-- Table: TRAVEL_AP_PARAMS - AP parameters
CREATE TABLE [TRAVEL_AP_PARAMS] (
    [AP_UNIT_ID] BIGINT NOT NULL,
    [AP_ACCOUNT_STATUS] CHAR(1) NOT NULL  -- O - Official  , P- Personal,
    [AP_ACCOUNT_CODE] VARCHAR(25) NOT NULL,
    [AP_CONTROLCOMBID] BIGINT NULL,
    CONSTRAINT [PK_TRAVEL_AP_PARAMS] PRIMARY KEY ([AP_UNIT_ID])
);

GO
