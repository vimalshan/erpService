-- ==========================================
-- Module: HealthCheckup
-- Purpose: Medical Checkup & Health Screening
-- Generated: 2026-03-09
-- ==========================================

USE HEALTHDB;
GO

-- =====================================================
-- Table: FIELD_TYP_MAST
-- Description: Field Type Master
-- =====================================================
IF OBJECT_ID('dbo.FIELD_TYP_MAST', 'U') IS NOT NULL
    DROP TABLE dbo.FIELD_TYP_MAST;
GO

CREATE TABLE [dbo.FIELD_TYP_MAST] (
    [FTM_TYP_NAM] VARCHAR(30) NULL  -- FIELD TYPE NAME,
    [FTM_TYP_COD] DECIMAL(38) NOT NULL  -- FIELD TYPE CODE,
    [FTM_CONTROL_SRC] VARCHAR(50) NULL  -- FILED TYPE CONTROL SOURCE,
    CONSTRAINT [PK_FIELD_TYP_MAST] PRIMARY KEY ([FTM_TYP_COD])
);

-- =====================================================
-- Table: CHKUP_SYMP_MAST
-- Description: Checkup Symptoms Master
-- =====================================================
IF OBJECT_ID('dbo.CHKUP_SYMP_MAST', 'U') IS NOT NULL
    DROP TABLE dbo.CHKUP_SYMP_MAST;
GO

CREATE TABLE [dbo.CHKUP_SYMP_MAST] (
    [CSM_SYMP_ID] DECIMAL(38) NOT NULL  -- SYMPTOM ID,
    [CSM_SYMP_NAM] VARCHAR(50) NULL  -- SYMPTOM NAME,
    [CSM_FLAG] VARCHAR(3) NULL  -- FH-FAMILY HISTORY, PH-PERSONAL HISTORY, IM-IMMUNIZATION,CO-COMMON,
    CONSTRAINT [PK_CHKUP_SYMP_MAST] PRIMARY KEY ([CSM_SYMP_ID])
);

-- =====================================================
-- Table: TEST_MAST
-- Description: Test Master
-- =====================================================
IF OBJECT_ID('dbo.TEST_MAST', 'U') IS NOT NULL
    DROP TABLE dbo.TEST_MAST;
GO

CREATE TABLE [dbo.TEST_MAST] (
    [TM_TEST_COD] DECIMAL(38) NOT NULL  -- TEST CODE,
    [TM_TEST_NAM] VARCHAR(50) NOT NULL  -- TEST  NAME,
    [TM_CHKBOX_FLAG] CHAR(1) NULL  -- FLAG FOR SELECTING CHECKED TESTS,
    [TM_EFFEC_DAT] DATETIME2(3) NULL  -- EFFECTIVE DATE,
    [TM_CLOSE_DAT] DATETIME2(3) NULL  -- CLOSURE DATE,
    [TM_CLOS_FLAG] CHAR(1) NULL  -- FLAG FOR SHOWING CLOSED TESTS,
    [TM_RNG_VAL] VARCHAR(100) NULL,
    [TM_GROUP] VARCHAR(100) NULL,
    CONSTRAINT [PK_TEST_MAST] PRIMARY KEY ([TM_TEST_COD])
);

-- =====================================================
-- Table: CHECKUP_MAST
-- Description: Checkup Master
-- =====================================================
IF OBJECT_ID('dbo.CHECKUP_MAST', 'U') IS NOT NULL
    DROP TABLE dbo.CHECKUP_MAST;
GO

CREATE TABLE [dbo.CHECKUP_MAST] (
    [CM_COM_COD] VARCHAR(3) NOT NULL  -- COMPANY CODE,
    [CM_CHK_NAM] VARCHAR(50) NOT NULL  -- CHECKUP NAME,
    [CM_CHK_COD] DECIMAL(38) NOT NULL  -- CHECKUP CODE,
    [CM_EFFEC_DAT] DATETIME2(3) NOT NULL  -- EFFECTIVE DATE,
    [CM_CLOS_DAT] VARCHAR(255) NULL  -- CLOSURE DATE,
    [CM_FLAG] CHAR(1) NULL  -- FLAG TO IDENTIFY B/W CHECKUPS AND PREEMP,CHKUPCARD,
    CONSTRAINT [PK_CHECKUP_MAST] PRIMARY KEY ([CM_COM_COD])
);

-- =====================================================
-- Table: CHKUP_OTHERS
-- Description: Checkup Other Fields
-- =====================================================
IF OBJECT_ID('dbo.CHKUP_OTHERS', 'U') IS NOT NULL
    DROP TABLE dbo.CHKUP_OTHERS;
GO

CREATE TABLE [dbo.CHKUP_OTHERS] (
    [CO_COM_COD] VARCHAR(3) NULL  -- COMPANY CODE,
    [CO_CHK_COD] DECIMAL(38) NULL  -- CHECKUP CODE,
    [CO_OTHER_SRLNO] DECIMAL(38) NULL  -- SERIAL NO FOR OTHER INFO,
    [CO_FIELD_LABEL] VARCHAR(200) NULL  -- LABEL TEXT,
    [CO_MAND_FLAG] CHAR(1) NULL  -- MANDATORY FLAG,
    [CO_FIELD_TYPCOD] DECIMAL(38) NULL  -- FIELD TYPE CODE,
    [CO_EFFEC_DAT] DATETIME2(3) NOT NULL  -- EFFECTIVE DATE,
    [CO_CLOS_DAT] DATETIME2(3) NULL  -- CLOSURE DATE,
    [CO_FIELD_TYPNAM] VARCHAR(50) NULL  -- FIELD TYPE NAME
);

-- =====================================================
-- Table: CHKUP_OTHERS_LOV
-- Description: Checkup Other Fields List of Values
-- =====================================================
IF OBJECT_ID('dbo.CHKUP_OTHERS_LOV', 'U') IS NOT NULL
    DROP TABLE dbo.CHKUP_OTHERS_LOV;
GO

CREATE TABLE [dbo.CHKUP_OTHERS_LOV] (
    [COL_COM_COD] VARCHAR(10) NULL  -- COMPANY CODE,
    [COL_CHK_COD] DECIMAL(38) NULL  -- CHECKUP CODE,
    [COL_OTHER_SRLNO] DECIMAL(38) NULL  -- OTHER SERIAL NO,
    [COL_LOV_SRLNO] DECIMAL(38) NOT NULL  -- LIST OF VALUES SERIAL NO,
    [COL_LOV_DESC] VARCHAR(50) NULL  -- LIST OF VALUES DESCRIPTION,
    CONSTRAINT [PK_CHKUP_OTHERS_LOV] PRIMARY KEY ([COL_LOV_SRLNO])
);

-- =====================================================
-- Table: CHKUP_TEST
-- Description: Checkup Test Mapping
-- =====================================================
IF OBJECT_ID('dbo.CHKUP_TEST', 'U') IS NOT NULL
    DROP TABLE dbo.CHKUP_TEST;
GO

CREATE TABLE [dbo.CHKUP_TEST] (
    [CT_CHK_COD] DECIMAL(38) NULL  -- CHECKUP CODE,
    [CT_COM_COD] VARCHAR(10) NULL  -- COMPANY CODE,
    [CT_TEST_COD] DECIMAL(38) NULL  -- TEST CODE,
    [CT_ORD_NUM] DECIMAL(38) NULL  -- ORDER  NUMBER,
    [CT_SRL_NO] DECIMAL(38) NOT NULL  -- SERIAL NUMBER,
    [CT_CHKBOX_FLG] CHAR(1) NULL  -- FLAG FOR SELECTING CHECKED TESTS,
    [CT_EFFEC_DAT] DATETIME2(3) NULL  -- EFFECTIVE DATE,
    [CT_CLOS_DAT] DATETIME2(3) NULL  -- CLOSURE DATE,
    [CT_CLOS_FLAG] CHAR(1) NULL,
    CONSTRAINT [PK_CHKUP_TEST] PRIMARY KEY ([CT_SRL_NO])
);

-- =====================================================
-- Table: HEALTH_COUNTER
-- Description: Health Counter
-- =====================================================
IF OBJECT_ID('dbo.HEALTH_COUNTER', 'U') IS NOT NULL
    DROP TABLE dbo.HEALTH_COUNTER;
GO

CREATE TABLE [dbo.HEALTH_COUNTER] (
    [HC_COM_COD] CHAR(3) NOT NULL  -- Company Code,
    [HC_CNT_COD] CHAR(3) NOT NULL  -- Counter Code,
    [MC_CNT_NUM] BIGINT NOT NULL  -- Counter value
);

-- =====================================================
-- Table: HEALTH_MINMAX_VAL
-- Description: Health Test Min/Max Values
-- =====================================================
IF OBJECT_ID('dbo.HEALTH_MINMAX_VAL', 'U') IS NOT NULL
    DROP TABLE dbo.HEALTH_MINMAX_VAL;
GO

CREATE TABLE [dbo.HEALTH_MINMAX_VAL] (
    [HMV_TEST_COD] DECIMAL(38) NOT NULL  -- TEST CODE,
    [HMV_TYP_COD] CHAR(2) NULL  -- TYPE CODE (NUMERIC SINGLE-NS,NUMERIC DOUBLE-ND, STRING VALUE-SV,LO- LIST OF VALUES),
    [HMV_UNT_COD] VARCHAR(10) NULL  -- UNIT CODE OF TEST,
    [HMV_SING_VAL] DECIMAL(38) NULL  -- SINGLE VALUE,
    [HMV_MIN_VAL] DECIMAL(38) NULL  -- MINIMUM VALUE,
    [HMV_MAX_VAL] DECIMAL(38) NULL  -- MAX VALUE,
    [HMV_MIN_TEXT] VARCHAR(20) NULL  -- MINIMUM TEXT LABEL,
    [HMV_MAX_TEXT] VARCHAR(20) NULL  -- MAXIMUM TEXT LABEL,
    [HMV_LOV_TEXT] VARCHAR(20) NULL  -- LOV TEXT LABEL
);

-- =====================================================
-- Table: HEALTH_ENTRY_LOV
-- Description: Health Entry List of Values
-- =====================================================
IF OBJECT_ID('dbo.HEALTH_ENTRY_LOV', 'U') IS NOT NULL
    DROP TABLE dbo.HEALTH_ENTRY_LOV;
GO

CREATE TABLE [dbo.HEALTH_ENTRY_LOV] (
    [HEL_TEST_COD] DECIMAL(38) NOT NULL  -- TEST CODE,
    [HEL_LOV_VAL] VARCHAR(50) NULL  -- LOV VALUES
);

-- =====================================================
-- Table: HEALTH_MAIN
-- Description: Health Check Main Record
-- =====================================================
IF OBJECT_ID('dbo.HEALTH_MAIN', 'U') IS NOT NULL
    DROP TABLE dbo.HEALTH_MAIN;
GO

CREATE TABLE [dbo.HEALTH_MAIN] (
    [HM_EMP_NUM] VARCHAR(10) NOT NULL  -- Employee number,
    [HM_COM_COD] VARCHAR(10) NOT NULL  -- Company code,
    [HM_CHK_DAT] VARCHAR(100) NULL  -- CHECKUP DATE,
    [HM_HLT_NUM] INT NOT NULL  -- health number,
    [ENT_EMP_NUM] VARCHAR(10) NOT NULL  -- data feeder employee number,
    [HM_CHK_COD] VARCHAR(10) NOT NULL  -- CHECK UP CODE,
    [TEXT2] VARCHAR(10) NOT NULL  -- FLEXI FIELD,
    [TEXT3] VARCHAR(10) NOT NULL  -- FLEXI FIELD,
    [TEXT4] VARCHAR(10) NOT NULL  -- FLEXI FIELD,
    [TEXT5] VARCHAR(10) NOT NULL  -- FLEXI FIELD,
    CONSTRAINT [PK_HEALTH_MAIN] PRIMARY KEY ([HM_HLT_NUM])
);

-- =====================================================
-- Table: HEALTH_SUB
-- Description: Health Check Sub Record (Test Results)
-- =====================================================
IF OBJECT_ID('dbo.HEALTH_SUB', 'U') IS NOT NULL
    DROP TABLE dbo.HEALTH_SUB;
GO

CREATE TABLE [dbo.HEALTH_SUB] (
    [HM_HLT_NUM] INT NOT NULL  -- Health Number,
    [HS_TST_COD] NVARCHAR(10) NULL  -- TEST CODE,
    [HS_TST_TYP] NVARCHAR(50) NULL  -- Test Type,
    [HS_TST_VAL] NVARCHAR(50) NULL  -- Type Value,
    [HS_EMP_NUM] NVARCHAR(20) NOT NULL  -- Employee Num,
    [HS_TST_RMK] NVARCHAR(200) NULL  -- TEST REMARKS,
    [HS_TST_DAT] DATETIME2(3) NULL  -- TEST DATE,
    [HS_VALD_FLG] CHAR(1) NULL  -- FLAG FOR VALIDATION,
    [TEXT2] VARCHAR(255) NULL  -- FLEXI FIELD,
    [TEXT3] VARCHAR(255) NULL  -- FLEXI FIELD,
    [TEXT4] VARCHAR(255) NULL  -- FLEXI FIELD,
    [TEXT5] VARCHAR(255) NULL  -- FLEXI FIELD,
    [HS_DOC_RMK] VARCHAR(255) NULL
);

-- =====================================================
-- Table: HEALTH_DYN_DET
-- Description: Health Dynamic Details
-- =====================================================
IF OBJECT_ID('dbo.HEALTH_DYN_DET', 'U') IS NOT NULL
    DROP TABLE dbo.HEALTH_DYN_DET;
GO

CREATE TABLE [dbo.HEALTH_DYN_DET] (
    [CDD_HLTH_NUM] DECIMAL(38) NULL  -- HEALTH NUMBER,
    [CDD_CHKUP_COD] DECIMAL(38) NULL  -- CHECKUP CODE,
    [CDD_COM_COD] VARCHAR(3) NULL  -- COMPANY CODE,
    [CDD_CTRLSRC_ID] DECIMAL(38) NULL  -- CONTROL SRC ID,
    [CDD_DYN_VAL] VARCHAR(100) NULL  -- DYNAMIC VALUE TO BE STORED,
    [CDD_EMP_NUM] DECIMAL(38) NULL  -- EMPLOYEE NUM,
    [CDD_SYS_DAT] DATETIME2(3) NULL  -- SYSTEM DATE
);

-- =====================================================
-- Table: CHKUP_PRE_MAIN
-- Description: Pre-Employment Checkup Main
-- =====================================================
IF OBJECT_ID('dbo.CHKUP_PRE_MAIN', 'U') IS NOT NULL
    DROP TABLE dbo.CHKUP_PRE_MAIN;
GO

CREATE TABLE [dbo.CHKUP_PRE_MAIN] (
    [CPM_EMP_NUM] DECIMAL(38) NULL  -- EMPLOYEE NUM,
    [CPM_COM_COD] VARCHAR(3) NULL  -- COMPANY CODE,
    [CPM_HLTH_NUM] DECIMAL(38) NULL  -- HEALTH NUMBER,
    [CPM_PHYS_HAND] VARCHAR(150) NULL  -- DESCRIPTION OF PHYSICAL HANDICAP,
    [CPM_PROP_EMP] VARCHAR(30) NULL  -- PROPOSED DESIGNATION OF EMP,
    [CPM_IDENT_MARKS] VARCHAR(30) NULL  -- IDENTIFICATION MARKS OF EMPLOYEE,
    [CPM_FINAL_RMKS] VARCHAR(15) NULL  -- FINAL REMARKS,
    [CPM_FIT_PH] CHAR(3) NULL  -- FIT/UNFIT,
    [CPM_FIT_FINAL] VARCHAR(6) NULL  -- FINAL FIT/UNFIT,
    [CPM_CHK_DAT] DATETIME2(3) NULL  -- CHECKUP DATE
);

-- =====================================================
-- Table: CHKUP_PFI_HIST
-- Description: Checkup Personal & Family History
-- =====================================================
IF OBJECT_ID('dbo.CHKUP_PFI_HIST', 'U') IS NOT NULL
    DROP TABLE dbo.CHKUP_PFI_HIST;
GO

CREATE TABLE [dbo.CHKUP_PFI_HIST] (
    [CPH_HLTH_NUM] DECIMAL(38) NULL  -- HEALTH NUMBER OF THE EMPLOYEE,
    [CPH_EMP_NUM] DECIMAL(38) NULL  -- EMPLOYEE NUMBER,
    [CPH_SYMP_ID] DECIMAL(38) NULL  -- SYMPTOM ID,
    [CPH_YN_FLAG] CHAR(1) NULL  -- YES/NO FLAG,
    [CPH_IMM_DAT] DATETIME2(3) NULL  -- IMMUNIZATION DATE,
    [CPH_TEST_VAL] VARCHAR(30) NULL  -- TEST VALUES
);

-- =====================================================
-- Table: HLTH_CHKUP_CARD
-- Description: Health Checkup Card
-- =====================================================
IF OBJECT_ID('dbo.HLTH_CHKUP_CARD', 'U') IS NOT NULL
    DROP TABLE dbo.HLTH_CHKUP_CARD;
GO

CREATE TABLE [dbo.HLTH_CHKUP_CARD] (
    [HCC_HLTH_NUM] DECIMAL(38) NULL  -- HEALTH NUM,
    [HCC_EMP_NUM] DECIMAL(38) NULL  -- EMP NUM,
    [HCC_EMP_DATE] DATETIME2(3) NULL  -- EMP DATE,
    [HCC_COM_COD] VARCHAR(3) NULL  -- COMPANY CODE,
    [HCC_PER_DET] VARCHAR(200) NULL  -- PERSONAL DETAILS,
    [HCC_COMPL_DET] VARCHAR(150) NULL  -- SCR DETAILS,
    [HCC_ADV_RMK1] VARCHAR(150) NULL  -- ADV 1,
    [HCC_DOC_DATE1] DATETIME2(3) NULL  -- DOC DATE 1,
    [HCC_ADV_FOLLOW1] VARCHAR(150) NULL  -- FOLLOW 1,
    [HCC_ADV_RMK2] VARCHAR(150) NULL  -- DOC DATE2,
    [HCC_DOC_DATE2] DATETIME2(3) NULL  -- FOLLOW 2,
    [HCC_ADV_FOLLOW2] VARCHAR(150) NULL
);

-- =====================================================
-- Table: HLTH_CHKCARD_SUB
-- Description: Health Checkup Card Sub
-- =====================================================
IF OBJECT_ID('dbo.HLTH_CHKCARD_SUB', 'U') IS NOT NULL
    DROP TABLE dbo.HLTH_CHKCARD_SUB;
GO

CREATE TABLE [dbo.HLTH_CHKCARD_SUB] (
    [HCS_HLTH_NUM] DECIMAL(38) NULL  -- HEALTH NUM,
    [HCS_SYMP_ID] DECIMAL(38) NULL  -- SYMPTOMS ID,
    [HCS_FLAG_YN] VARCHAR(30) NULL  -- FLAG Y/N,
    [HCS_SYMP_VAL] VARCHAR(150) NULL  -- SYMPTOMS VALUE,
    [HCS_EMP_NUM] DECIMAL(38) NULL  -- EMP NUMBER
);

-- =====================================================
-- Indexes
-- =====================================================
CREATE INDEX [IDX_CHECKUP_MAST_CM_CHK_COD] ON [dbo.CHECKUP_MAST]([CM_CHK_COD]);
CREATE INDEX [IDX_HEALTH_MAIN_HM_EMP_NUM] ON [dbo.HEALTH_MAIN]([HM_EMP_NUM]);
CREATE INDEX [IDX_HEALTH_SUB_HM_HLT_NUM] ON [dbo.HEALTH_SUB]([HM_HLT_NUM]);

PRINT 'HealthCheckup: Table creation completed successfully.';
GO
