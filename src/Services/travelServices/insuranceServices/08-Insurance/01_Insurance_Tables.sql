-- ==========================================
-- Module: INSURANCE
-- Description: Travel insurance management
-- Tables: Travel insurance records and tracking
-- ==========================================

USE [TRAVELDB];
GO

-- Table: TRAVEL_INSURANCE - Travel insurance records
CREATE TABLE [TRAVEL_INSURANCE] (
    [IN_COM_COD] CHAR(3) NOT NULL  -- Company Code,
    [IN_PLN_NUM] BIGINT NOT NULL  -- Plan No,
    [IN_INS_TYP] CHAR(3) NOT NULL  -- Insurance Type,
    [IN_PASS_NUM] VARCHAR(50) NULL  -- Passport No,
    [IN_ISS_DAT] DATETIME2(3) NULL  -- Passport Issue Date,
    [IN_VIS_PLC] VARCHAR(50) NULL  -- Visa Issue Place,
    [IN_VIS_DAT] DATETIME2(3) NULL  -- Visa Issue Date,
    [IN_NOM_NAM1] VARCHAR(200) NULL  -- Nomination Name - 1,
    [IN_NOM_NAM2] VARCHAR(200) NULL  -- Nomination Name - 2,
    [IN_INS_STS] CHAR(1) NULL  -- Insurance Status,
    [IN_CRT_NUM] VARCHAR(200) NULL  -- Certificate No,
    [IN_UPD_DAT] DATETIME2(3) NULL  -- Update Date,
    [IN_UPD_UID] VARCHAR(200) NULL  -- Updated By User ID,
    [IN_UPD_UNUM] BIGINT NULL  -- Updated By User Number,
    [IN_REM_MRK] VARCHAR(200) NULL  -- Remarks,
    [IN_FLX_FLD1] VARCHAR(200) NULL  -- Flex Field 1,
    [IN_FLX_FLD2] DECIMAL(19,0) NULL  -- Flex Field 2,
    [IN_FLX_FLD3] DECIMAL(19,0) NULL  -- Flex Field 3,
    [IN_FLX_FLD4] DATETIME2(3) NULL  -- Flex Field 4
);

GO
