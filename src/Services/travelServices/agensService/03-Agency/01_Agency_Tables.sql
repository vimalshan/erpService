-- ==========================================
-- Module: AGENCY & VENDOR
-- Description: Agency and vendor master data management
-- Tables: Master data for agencies, vendors, and carriers
-- ==========================================

USE [TRAVELDB];
GO

-- Table: AGENCY_MASTER - Travel agency master
CREATE TABLE [AGENCY_MASTER] (
    [AM_AGN_CODE] BIGINT NOT NULL  -- Agency Code,
    [AM_AGN_NAM] VARCHAR(100) NULL  -- Agency name,
    [AM_AGN_ADD1] VARCHAR(100) NULL  -- Agency Address1,
    [AM_AGN_ADD2] VARCHAR(100) NULL  -- Agency Address2,
    [AM_AGN_ADD3] VARCHAR(100) NULL  -- Agency Address3,
    [AM_AGN_ADD4] VARCHAR(100) NULL  -- Agency Address4,
    [AM_EML_ID] VARCHAR(250) NULL  -- Email id,
    [AM_PHN_NO] VARCHAR(15) NULL  -- Phone number,
    [AM_PHN_NO1] VARCHAR(15) NULL  -- Additional Phone No,
    [AM_AGN_TYP] VARCHAR(5) NULL  -- Agency Type(Train,Cab,Air,Bus),
    [AM_ADM_UNT] BIGINT NULL,
    [AM_ORA_COD] VARCHAR(10) NULL,
    [AM_ORA_SIT] VARCHAR(100) NULL,
    [AM_TERM_ID] BIGINT NULL,
    [AM_LATECAB_FLAG] CHAR(1) NULL,
    [AM_R12BUCODE] VARCHAR(25) NULL,
    [AM_R12LOCATION] VARCHAR(25) NULL,
    [AM_ORA_ITEMCODE] VARCHAR(40) NULL,
    [AM_GSTRECOVER] CHAR(1) NULL,
    [AM_MODIFIEDBY] BIGINT NULL,
    [AM_MODIFIEDON] DATETIME2(3) NULL,
    CONSTRAINT [PK_AGENCY_MASTER] PRIMARY KEY ([AM_AGN_CODE])
);

-- Table: VENDOR_MASTER - Vendor master
CREATE TABLE [VENDOR_MASTER] (
    [VM_ID] BIGINT NOT NULL  -- Identifiction code,
    [VM_NAME] VARCHAR(65) NOT NULL  -- Name,
    [VM_ADD_LN1] VARCHAR(30) NULL  -- ADRRESS LINE1,
    [VM_ADD_LN2] VARCHAR(30) NULL  -- Address line2,
    [VM_ADD_LIN3] VARCHAR(30) NULL  -- Address line3,
    [VM_ADD_LN4] VARCHAR(30) NULL  -- Address line4,
    [VM_ADD_LN5] VARCHAR(30) NULL  -- Address line5,
    [VM_CIT_COD] BIGINT NULL  -- CITY COD,
    [VM_IT_PAN] CHAR(10) NULL  -- IT PAN NO,
    [VM_PHN_NO] VARCHAR(20) NULL  -- PHONE NO,
    [VM_ACC_NO] VARCHAR(20) NULL  -- bank account no,
    [VM_BNK_NAM] VARCHAR(65) NULL  -- BANK NAME IN WHICH THE ACCOUNT IS,
    [VM_CAT_TYPE] CHAR(1) NOT NULL  -- CATEGORY TYPE : V-VENDOR,H-HOTEL
);

-- Table: AIRLINE_MAST - Airline master
CREATE TABLE [AIRLINE_MAST] (
    [AIR_LIN_COD] CHAR(3) NOT NULL  -- Airline Code,
    [AIR_LIN_NAM] VARCHAR(100) NOT NULL  -- AIRLINE NAME,
    CONSTRAINT [PK_AIRLINE_MAST] PRIMARY KEY ([AIR_LIN_COD])
);

-- Table: AIRLINE_FLIGHTNO_MAPPING - Airline flight number mapping
CREATE TABLE [AIRLINE_FLIGHTNO_MAPPING] (
    [AIR_CODE] CHAR(3) NOT NULL  -- Airline Code,
    [FLIGHT_NO] VARCHAR(10) NOT NULL  -- FLIGHT NUMBER
);

-- Table: TRAVEL_CABCONTRACT - Cab contract master
CREATE TABLE [TRAVEL_CABCONTRACT] (
    [CONT_ID] BIGINT NOT NULL  -- Contract id,
    [CONT_DESC] VARCHAR(4000) NULL  -- Description,
    [CONT_VENDOR] BIGINT NULL  -- vendor name,
    [CONT_LOC] VARCHAR(100) NULL  -- location,
    [CONT_TYPE] VARCHAR(100) NULL  -- car type,
    [CONT_PREFERENCE] VARCHAR(100) NULL  -- car preference,
    [CONT_TYPE1] VARCHAR(100) NULL  -- local/outstation,
    [CONT_KM] VARCHAR(100) NULL  -- Kilometer for display,
    [CONT_HOUR] VARCHAR(100) NULL  -- hour for display,
    [CONT_RATE] VARCHAR(100) NULL  -- rate,
    [CONT_ADDITIONAL1] VARCHAR(100) NULL  -- additional per hour per km rate,
    [CONT_ADDITIONAL2] VARCHAR(100) NULL  -- additional per hour rate,
    [CONT_ADDITIONAL3] VARCHAR(100) NULL  -- additional per night rate,
    [CONT_KM1] BIGINT NULL  -- No of Kms,
    [CONT_HOUR1] BIGINT NULL  -- No of HRs,
    [EFF_DATE] DATETIME2(3) NULL,
    [CLS_DATE] DATETIME2(3) NULL,
    CONSTRAINT [PK_TRAVEL_CABCONTRACT] PRIMARY KEY ([CONT_ID])
);

-- Table: HOTELCATEGORY_MASTER - Hotel category master
CREATE TABLE [HOTELCATEGORY_MASTER] (
    [HOTEL_ID] DECIMAL(38) NULL,
    [HOTEL_CATEGORY] VARCHAR(10) NULL,
    [BAND] BIGINT NULL,
    [ELIGIBITY_AMT] INT NULL,
    [EFF_DATE] DATETIME2(3) NULL,
    [CLS_DATE] DATETIME2(3) NULL
);

-- Table: HOTEL_CITY_MAPPING - Hotel city mapping
CREATE TABLE [HOTEL_CITY_MAPPING] (
    [CITY_COD] BIGINT NULL,
    [HOTEL_ID] BIGINT NULL
);

-- Table: INTERNATIONAL_TRVL_GRPS - International travel groups
CREATE TABLE [INTERNATIONAL_TRVL_GRPS] (
    [IG_GRP_COD] CHAR(1) NOT NULL  -- Group code,
    [IG_EMP_BND] CHAR(1) NOT NULL  -- Employees band code,
    [IG_ENT_ACT] BIGINT NOT NULL  -- entitilement according to band (ACTUALS),
    [IG_ENT_FLT] BIGINT NOT NULL  -- entitilement according to band (FLAT),
    CONSTRAINT [PK_INTERNATIONAL_TRVL_GRPS] PRIMARY KEY ([IG_GRP_COD])
);

-- Table: TAX_MASTER - Tax configuration by vendor
CREATE TABLE [TAX_MASTER] (
    [TAX_VENDORID] BIGINT NOT NULL,
    [TAX_TYPE] CHAR(5) NOT NULL,
    [TAX_RATE] DECIMAL(19,0) NULL,
    [TAX_EFFDAT] DATETIME2(3) NOT NULL,
    [TAX_CLSDAT] DATETIME2(3) NULL,
    [MODIFIED_BY] BIGINT NULL,
    [MODIFIED_ON] DATETIME2(3) NULL,
    CONSTRAINT [PK_TAX_MASTER] PRIMARY KEY ([TAX_TYPE])
);

-- Table: TAX_COMPONENT - Tax component for vendor
CREATE TABLE [TAX_COMPONENT] (
    [VENDORCODE] BIGINT NULL,
    [COMPONENT] VARCHAR(50) NULL
);

GO
