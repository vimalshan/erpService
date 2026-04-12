CREATE TABLE [ACC_MASTER] (
    [AC_COM_COD] char(3) NULL,
    [AC_ED_COD] char(6) NULL,
    [AC_ACC_COD] char(6) NULL,
    [AC_GRD_TYP] char(3) NULL,
    [AC_DC_FLG] char(1) NULL,
    [AC_SUB_COD] char(6) NULL,
    [AC_ACC_DES] nvarchar(200) NULL
);
GO


CREATE TABLE [GL_CODE_COMBINATIONS_KFV] (
    [ROW_ID] bigint NOT NULL IDENTITY,
    [CODE_COMBINATION_ID] bigint NOT NULL,
    [CHART_OF_ACCOUNTS_ID] bigint NOT NULL,
    [CONCATENATED_SEGMENTS] nvarchar(207) NULL,
    [PADDED_CONCATENATED_SEGMENTS] nvarchar(26) NULL,
    [GL_ACCOUNT_TYPE] nvarchar(1) NOT NULL,
    [DETAIL_BUDGETING_ALLOWED] nvarchar(1) NOT NULL,
    [DETAIL_POSTING_ALLOWED] nvarchar(1) NOT NULL,
    [ENABLED_FLAG] nvarchar(1) NOT NULL,
    [SUMMARY_FLAG] nvarchar(1) NOT NULL,
    [SEGMENT1] nvarchar(25) NULL,
    [SEGMENT2] nvarchar(25) NULL,
    [SEGMENT3] nvarchar(25) NULL,
    [SEGMENT4] nvarchar(25) NULL,
    [SEGMENT5] nvarchar(25) NULL,
    [SEGMENT6] nvarchar(25) NULL,
    [SEGMENT7] nvarchar(25) NULL,
    [DESCRIPTION] nvarchar(240) NULL,
    [START_DATE_ACTIVE] datetime2(3) NULL,
    [END_DATE_ACTIVE] datetime2(3) NULL,
    [LAST_UPDATE_DATE] datetime2(3) NOT NULL,
    [LAST_UPDATED_BY] decimal(38,0) NOT NULL,
    CONSTRAINT [PK_GL_CODE_COMBINATIONS_KFV] PRIMARY KEY ([ROW_ID])
);
GO


CREATE TABLE [JAI_INTERFACE_LINES_ALL] (
    [INTERFACE_LINE_ID] decimal(38,0) NULL,
    [ORG_ID] decimal(38,0) NOT NULL,
    [ORGANIZATION_ID] decimal(38,0) NULL,
    [LOCATION_ID] decimal(38,0) NULL,
    [PARTY_ID] decimal(38,0) NOT NULL,
    [PARTY_SITE_ID] decimal(38,0) NOT NULL,
    [IMPORT_MODULE] nvarchar(255) NOT NULL,
    [TRANSACTION_ID] decimal(38,0) NULL,
    [TRANSACTION_NUM] nvarchar(240) NOT NULL,
    [TRANSACTION_LINE_NUM] decimal(38,0) NOT NULL,
    [ERROR_FLAG] nvarchar(1) NULL,
    [BATCH_SOURCE_NAME] nvarchar(240) NULL,
    [TAXABLE_BASIS] nvarchar(20) NULL,
    [TAXABLE_EVENT] nvarchar(20) NULL,
    [INCLUSIVE_TAX_AMOUNT] nvarchar(255) NULL,
    [EXCLUSIVE_TAX_AMOUNT] nvarchar(255) NULL,
    [CREATION_DATE] datetime2(3) NOT NULL,
    [CREATED_BY] decimal(38,0) NOT NULL,
    [LAST_UPDATE_DATE] datetime2(3) NOT NULL,
    [LAST_UPDATED_BY] decimal(38,0) NOT NULL,
    [IMPORT_STATUS] nvarchar(30) NULL,
    [HSN_CODE] nvarchar(3) NULL,
    [SAC_CODE] nvarchar(30) NULL,
    [BATCHID] decimal(19,0) NULL,
    [INVOICEID] decimal(19,0) NULL,
    [LINE_NUMBER] decimal(19,0) NULL,
    [BATCH_BU] nvarchar(25) NULL,
    [TYPE] nvarchar(255) NULL,
    [TYPE_TOUR] nvarchar(255) NULL,
    [TRV_CLASS] int NULL,
    [SGSTAMT] decimal(19,0) NULL,
    [CGSTAMT] decimal(19,0) NULL,
    [IGSTAMT] decimal(19,0) NULL,
    [JV_NO] bigint NULL,
    [JAI_AGENCY_ID] bigint NULL,
    [COMBINATION_ID] bigint NULL
);
GO


CREATE TABLE [JAI_INTERFACE_TAX_LINES_ALL] (
    [INTERFACE_TAX_LINE_ID] decimal(38,0) NULL,
    [INTERFACE_LINE_ID] decimal(38,0) NULL,
    [PARTY_ID] decimal(38,0) NOT NULL,
    [PARTY_SITE_ID] decimal(38,0) NOT NULL,
    [IMPORT_MODULE] nvarchar(10) NOT NULL,
    [TRANSACTION_NUM] nvarchar(240) NOT NULL,
    [TRANSACTION_LINE_NUM] decimal(38,0) NOT NULL,
    [TAX_LINE_NO] bigint NOT NULL,
    [EXTERNAL_TAX_CODE] nvarchar(255) NULL,
    [TAX_ID] bigint NULL,
    [TAX_RATE] decimal(38,0) NULL,
    [TAX_AMOUNT] decimal(38,0) NULL,
    [FUNC_TAX_AMOUNT] decimal(38,0) NULL,
    [BASE_TAX_AMOUNT] decimal(38,0) NULL,
    [INCLUSIVE_TAX_FLAG] nvarchar(255) NULL,
    [CODE_COMBINATION_ID] bigint NULL,
    [CREATION_DATE] datetime2(3) NOT NULL,
    [CREATED_BY] decimal(38,0) NOT NULL,
    [LAST_UPDATE_DATE] datetime2(3) NOT NULL,
    [LAST_UPDATED_BY] decimal(38,0) NOT NULL,
    [JV_NO] bigint NULL
);
GO


CREATE TABLE [JV_INTERFACE] (
    [CODE_COMBINATION] decimal(19,0) NULL,
    [SEGMENT1] nvarchar(2) NULL,
    [IO] decimal(19,0) NULL,
    [UNIT] char(3) NULL
);
GO


CREATE TABLE [JV_MISSING_COMBICODE] (
    [AM_AGN_NAM] nvarchar(20) NULL,
    [INVOICE_NUM] nvarchar(4000) NULL,
    [DESCRIPTION] nvarchar(4000) NULL,
    [DIST_CODE_CONCENATED] nvarchar(4000) NULL,
    [JV_NO] bigint NULL,
    [LOG_SYSID] bigint NULL
);
GO


CREATE TABLE [SOURCE_HIST] (
    [CHANGE_DATE] datetime2(3) NULL,
    [NAME] nvarchar(30) NULL,
    [TYPE] nvarchar(12) NULL,
    [LINE] decimal(38,0) NULL,
    [TEXT] nvarchar(4000) NULL
);
GO


CREATE TABLE [TAX_MASTER] (
    [TAX_TYPE] char(5) NOT NULL,
    [TAX_VENDORID] bigint NOT NULL,
    [TAX_RATE] decimal(19,0) NULL,
    [TAX_EFFDAT] datetime2(3) NOT NULL,
    [TAX_CLSDAT] datetime2(3) NULL,
    [MODIFIED_BY] bigint NULL,
    [MODIFIED_ON] datetime2(3) NULL,
    CONSTRAINT [PK_TAX_MASTER] PRIMARY KEY ([TAX_TYPE])
);
GO


CREATE TABLE [TRAVEL_AP_PARAMS] (
    [AP_UNIT_ID] bigint NOT NULL IDENTITY,
    [AP_ACCOUNT_STATUS] char(1) NOT NULL,
    [AP_ACCOUNT_CODE] nvarchar(25) NOT NULL,
    [AP_CONTROLCOMBID] bigint NULL,
    CONSTRAINT [PK_TRAVEL_AP_PARAMS] PRIMARY KEY ([AP_UNIT_ID])
);
GO


CREATE TABLE [TRAVEL_BATCH_SUB_BREAKUP] (
    [SLNO] bigint NOT NULL,
    [BOK_NUM] decimal(38,0) NOT NULL,
    [COST_UNIT] char(3) NOT NULL,
    [COST_CODE] nvarchar(25) NOT NULL,
    [PRODUCT_CODE] nvarchar(25) NULL,
    [SUBACCOUNT_CODE] nvarchar(25) NULL
);
GO


CREATE TABLE [VENDOR_MASTER] (
    [VM_ID] bigint NOT NULL IDENTITY,
    [VM_NAME] nvarchar(65) NOT NULL,
    [VM_ADD_LN1] nvarchar(30) NULL,
    [VM_ADD_LN2] nvarchar(30) NULL,
    [VM_ADD_LIN3] nvarchar(30) NULL,
    [VM_ADD_LN4] nvarchar(30) NULL,
    [VM_ADD_LN5] nvarchar(30) NULL,
    [VM_CIT_COD] bigint NULL,
    [VM_IT_PAN] char(10) NULL,
    [VM_PHN_NO] nvarchar(20) NULL,
    [VM_ACC_NO] nvarchar(20) NULL,
    [VM_BNK_NAM] nvarchar(65) NULL,
    [VM_CAT_TYPE] char(1) NOT NULL,
    CONSTRAINT [PK_VENDOR_MASTER] PRIMARY KEY ([VM_ID])
);
GO


CREATE TABLE [TAX_COMPONENT] (
    [VENDORCODE] bigint NOT NULL,
    [COMPONENT] nvarchar(50) NULL,
    [TaxMasterTaxType] char(5) NULL,
    CONSTRAINT [FK_TAX_COMPONENT_TAX_MASTER_TaxMasterTaxType] FOREIGN KEY ([TaxMasterTaxType]) REFERENCES [TAX_MASTER] ([TAX_TYPE])
);
GO


CREATE INDEX [IX_TAX_COMPONENT_TaxMasterTaxType] ON [TAX_COMPONENT] ([TaxMasterTaxType]);
GO


