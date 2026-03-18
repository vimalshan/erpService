-- Module: Other
USE DDDB;
GO

-- Table: LOG_DD_CAT_DEV_DETAIL
CREATE TABLE [LOG_DD_CAT_DEV_DETAIL] (
    [CT_REQ_NUM] DECIMAL(38) NULL  -- Request Number,
    [CT_QTN_NUM] DECIMAL(38) NULL  -- Question Number,
    [CT_ANS_SRL] DECIMAL(38) NULL  -- Answer serial number,
    [CT_APP_ID] VARCHAR(30) NOT NULL  -- User id,
    [CT_APP_NUM] DECIMAL(38) NOT NULL  -- User Number,
    [CT_ENT_DAT] DATETIME2(3) NULL  -- date,
    [CT_DESC] VARCHAR(400) NULL  -- Areas for Development,
    [CT_NEED] VARCHAR(400) NULL  -- Why do you need it?
);

