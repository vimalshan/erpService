-- ==========================================
-- RISK Module - Table Scripts
-- Database: MYWORKDB
-- Module: RISK
-- Description: Risk Management and Assessment Module
-- Created: March 9, 2026
-- ==========================================

USE MYWORKDB;
GO

-- =====================================================
-- RISK MASTER and SETUP Tables
-- =====================================================

-- Table: RISK_MASTER - Risk Master
CREATE TABLE [RISK_MASTER] (
    [RISK_ID] BIGINT NOT NULL  -- Risk ID,
    [RISK_APPLICABLETO] CHAR(1) NOT NULL  -- Organization(O) / Business(B) / Sub Division(S)  / Unit(U),
    [RISK_ORGID] BIGINT NOT NULL  -- Organization ID,
    [RISK_BUSID] BIGINT NOT NULL  -- Business ID - 0 all Business / Specific Business - HRMS,
    [RISK_DIVISIONID] BIGINT NOT NULL  -- Division ID - 0 for all Sub Division  / Specific Sub division,
    [RISK_UNITID] BIGINT NOT NULL  -- HR Unit 0 - All ; 1 - Selected,
    [RISK_FUNCTIONID] BIGINT NOT NULL  -- Function 0 - All ; 1 - Selected,
    [RISK_EVENTTITLE] VARCHAR(500) NOT NULL  -- Risk event title,
    [RISK_DESC] VARCHAR(4000) NOT NULL  -- Description,
    [RISK_TYPEID] BIGINT NOT NULL  -- Risk Type ID,
    [RISK_IMPACTID] BIGINT NOT NULL  -- Impact Rating ID,
    [RISK_PROBID] BIGINT NOT NULL  -- Probability Rating ID,
    [RISK_RATEID] BIGINT NOT NULL  -- Risk rating ID,
    [RISK_RESIMPACTID] BIGINT NOT NULL  -- Impact after Controls,
    [RISK_RESPROBID] BIGINT NOT NULL  -- Probabilty after Controls,
    [RISK_RESRATEID] BIGINT NOT NULL  -- Residual Risk Rating ID,
    [RISK_RESPID] BIGINT NOT NULL  -- Risk Response ID,
    [RISK_MITFLAG] CHAR(1) NOT NULL  -- Risk Mitigation Flag (Y/N),
    [RISK_OWNER] BIGINT NOT NULL  -- Risk Owner,
    [RISK_APPSTATUS] CHAR(1) NOT NULL  -- IA Approval Status - E-Entry/P-Pending for Approval/A - Approved/ Mitigated after Self Assessment Meeting,
    [RISK_CANCELDATE] DATETIME2(3) NULL  -- Risk Cancelled On,
    [RISK_CANCELREASON] VARCHAR(500) NULL  -- Risk Reason for Cancellation,
    [RISK_CREATEDBY] BIGINT NOT NULL  -- Risk Created By,
    [RISK_CREATEDON] DATETIME2(3) NOT NULL  -- Risk Created On,
    [RISK_MODIFIEDBY] BIGINT NULL  -- Risk Modified By,
    [RISK_MODIFIEDON] DATETIME2(3) NULL  -- Risk Modified On,
    [RISK_ASSESSMENTID] BIGINT NULL  -- Self Assessment  ID,
    [RISK_REVIMPACTID] BIGINT NULL  -- Probabilty after Controls,
    [RISK_REVPROBID] BIGINT NULL  -- Residual Risk Rating ID,
    [RISK_REVRISKRATID] BIGINT NULL  -- Residual Risk Rating ID,
    CONSTRAINT [PK_RISK_MASTER] PRIMARY KEY ([RISK_ID])
);

-- Table: RISKTYPE_MASTER - Risk Type Master
CREATE TABLE [RISKTYPE_MASTER] (
    [TYPE_ID] BIGINT NOT NULL  -- Risk Type ID,
    [TYPE_NAME] VARCHAR(200) NOT NULL  -- Risk Type Name,
    [TYPE_CREATEDBY] BIGINT NOT NULL  -- Rish Type Created By,
    [TYPE_CREATEDON] DATETIME2(3) NOT NULL  -- Rish Type Created On,
    [TYPE_MODIFIEDBY] BIGINT NULL  -- Rish Type Modified By,
    [TYPE_MODIFIEDON] DATETIME2(3) NULL  -- Rish Type Modified On,
    CONSTRAINT [PK_RISKTYPE_MASTER] PRIMARY KEY ([TYPE_ID])
);

-- Table: RISKIMPACT_MASTER - Risk Impact Master
CREATE TABLE [RISKIMPACT_MASTER] (
    [IMPACT_ID] BIGINT NOT NULL  -- Impact ID,
    [IMPACT_RANK] BIGINT NOT NULL  -- Impact Rank,
    [IMPACT_NAME] VARCHAR(200) NOT NULL  -- Impact Description,
    [IMPACT_CREATEDBY] BIGINT NOT NULL  -- Impact Created By,
    [IMPACT_CREATEDON] DATETIME2(3) NOT NULL  -- Impact Created On,
    [IMPACT_MODIFIEDBY] BIGINT NULL  -- Impact Modified By,
    [IMPACT_MODIFIEDON] DATETIME2(3) NULL  -- Impact Modified On,
    CONSTRAINT [PK_RISKIMPACT_MASTER] PRIMARY KEY ([IMPACT_ID])
);

-- Table: RISKPROB_MASTER - Probability Ranking Master
CREATE TABLE [RISKPROB_MASTER] (
    [PROB_ID] BIGINT NOT NULL  -- Probability ID,
    [PROB_RANK] BIGINT NOT NULL  -- Rank,
    [PROB_NAME] VARCHAR(200) NOT NULL  -- Descriptor,
    [PROB_OCC] VARCHAR(200) NOT NULL  -- Likelihood of occurance,
    [PROB_CREATEDBY] BIGINT NOT NULL  -- Response Type Created By,
    [PROB_CREATEDON] DATETIME2(3) NOT NULL  -- Response Type Created On,
    [PROB_MODIFIEDBY] BIGINT NULL  -- Response Type Modified By,
    [PROB_MODIFIEDON] DATETIME2(3) NULL  -- Response Type Modified On,
    CONSTRAINT [PK_RISKPROB_MASTER] PRIMARY KEY ([PROB_ID])
);

-- Table: RISKRATING_MASTER - Risk Rating Master
CREATE TABLE [RISKRATING_MASTER] (
    [RATING_ID] BIGINT NOT NULL  -- Rating ID,
    [RATING_RANK] BIGINT NOT NULL  -- Rank,
    [RATING_FROM] BIGINT NOT NULL  -- Rating From,
    [RATING_TO] BIGINT NOT NULL  -- Rating To,
    [RATING_NAME] VARCHAR(200) NOT NULL  -- Description,
    [RATING_CREATEDBY] BIGINT NOT NULL  -- Rating Created By,
    [RATING_CREATEDON] DATETIME2(3) NOT NULL  -- Rating Created On,
    [RATING_MODIFIEDBY] BIGINT NULL  -- Rating Modified By,
    [RATING_MODIFIEDON] DATETIME2(3) NULL  -- Rating Modified On,
    CONSTRAINT [PK_RISKRATING_MASTER] PRIMARY KEY ([RATING_ID])
);

-- Table: RISKRESP_MASTER - Risk Response Master
CREATE TABLE [RISKRESP_MASTER] (
    [RESP_ID] BIGINT NOT NULL  -- Response Type ID,
    [RESP_NAME] VARCHAR(200) NOT NULL  -- Response Type Name,
    [RESP_CREATEDBY] BIGINT NOT NULL  -- Response Type Created By,
    [RESP_CREATEDON] DATETIME2(3) NOT NULL  -- Response Type Created On,
    [RESP_MODIFIEDBY] BIGINT NULL  -- Response Type Modified By,
    [RESP_MODIFIEDON] DATETIME2(3) NULL  -- Response Type Modified On,
    CONSTRAINT [PK_RISKRESP_MASTER] PRIMARY KEY ([RESP_ID])
);

-- =====================================================
-- RISK ORGANIZATION Tables
-- =====================================================

-- Table: RISKDIVISION_MASTER - Sub Division Group Master
CREATE TABLE [RISKDIVISION_MASTER] (
    [RISKDIVISION_ID] BIGINT NOT NULL  -- Division ID,
    [RISKDIVISION_NAME] VARCHAR(200) NOT NULL  -- Division Name,
    [RISKDIVISION_HRMSBUSID] BIGINT NOT NULL  -- HRMS Business ID,
    [RISKDIVISION_CREATEDBY] BIGINT NOT NULL  -- Risk Division Created By,
    [RISKDIVISION_CREATEDON] DATETIME2(3) NOT NULL  -- Risk Division Created On,
    [RISKDIVISION_MODIFIEDBY] BIGINT NULL  -- Risk Division Modified By,
    [RISKDIVISION_MODIFIEDON] DATETIME2(3) NULL  -- Risk Division Modified On,
    CONSTRAINT [PK_RISKDIVISION_MASTER] PRIMARY KEY ([RISKDIVISION_ID])
);

-- Table: RISKDIVISIONUNIT_MAP - Division Unit Map
CREATE TABLE [RISKDIVISIONUNIT_MAP] (
    [DIVUNIT_MAPID] BIGINT NOT NULL  -- Division Unit Map ID,
    [DIVUNIT_DIVISIONID] BIGINT NOT NULL  -- Division Unit Map Division ID,
    [DIVUNIT_UNITID] BIGINT NOT NULL  -- Division Unit Map HR Unit ID,
    [DIVUNIT_CREATEDBY] BIGINT NOT NULL  -- Division Unit Map Created By,
    [DIVUNIT_CREATEDON] DATETIME2(3) NOT NULL  -- Division Unit Map Created On,
    [DIVUNIT_MODIFIEDBY] BIGINT NULL  -- Division Unit Map Modified By,
    [DIVUNIT_MODIFIEDON] DATETIME2(3) NULL  -- Division Unit Map Modified On,
    CONSTRAINT [PK_RISKDIVISIONUNIT_MAP] PRIMARY KEY ([DIVUNIT_MAPID])
);

-- =====================================================
-- RISK DETAIL Tables
-- =====================================================

-- Table: RISK_CAUSES - Risk Root Cause Map
CREATE TABLE [RISK_CAUSES] (
    [ROOT_ID] BIGINT NOT NULL  -- Root Cause ID,
    [ROOT_RISKID] BIGINT NOT NULL  -- Risk ID,
    [ROOT_DESC] VARCHAR(2000) NOT NULL  -- Root Cause Description,
    [ROOT_LASTMODIFIEDBY] BIGINT NOT NULL  -- Root Cause Last Modified By,
    [ROOT_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Root Cause Last Modified On,
    CONSTRAINT [PK_RISK_CAUSES] PRIMARY KEY ([ROOT_ID])
);

-- Table: RISK_CONTROLS - Risk Controls Map
CREATE TABLE [RISK_CONTROLS] (
    [CONTROL_ID] BIGINT NOT NULL  -- Control ID,
    [CONTROL_RISKID] BIGINT NOT NULL  -- Risk ID,
    [CONTROL_DESC] VARCHAR(2000) NOT NULL  -- Control Description,
    [CONTROL_FILENAME] VARCHAR(500) NOT NULL  -- Control Attachment File Name,
    [CONTROL_LASTMODIFIEDBY] BIGINT NOT NULL  -- Control Map Last Modified By,
    [CONTROL_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Control Map Last Modified On,
    [CONTROL_IMPACTREDPER] BIGINT NULL  -- Impact after Controls,
    [CONTROL_PROBREDPER] BIGINT NULL  -- Probabilty after Controls,
    CONSTRAINT [PK_RISK_CONTROLS] PRIMARY KEY ([CONTROL_ID])
);

-- Table: RISK_IMPACT - Risk Impact Map
CREATE TABLE [RISK_IMPACT] (
    [IMPMAP_ID] BIGINT NOT NULL  -- Impact ID,
    [IMPMAP_RISKID] BIGINT NOT NULL  -- Risk ID,
    [IMPMAP_DESC] VARCHAR(2000) NOT NULL  -- Impact Description,
    [IMPMAP_LASTMODIFIEDBY] BIGINT NOT NULL  -- Impact Map Last Modified By,
    [IMPMAP_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Impact Map Last Modified On,
    CONSTRAINT [PK_RISK_IMPACT] PRIMARY KEY ([IMPMAP_ID])
);

-- Table: RISK_EVENT - Risk Event
CREATE TABLE [RISK_EVENT] (
    [EVENT_ID] BIGINT NOT NULL  -- Event ID,
    [EVENT_RISKID] BIGINT NOT NULL  -- Risk ID,
    [EVENT_DESCRIPTION] VARCHAR(500) NOT NULL  -- Event Description,
    [EVENT_DATE] DATETIME2(3) NOT NULL  -- Event Date,
    [EVENT_LASTMODIFIEDBY] BIGINT NOT NULL  -- Event Last Modified By,
    [EVENT_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Event Last Modified On,
    CONSTRAINT [PK_RISK_EVENT] PRIMARY KEY ([EVENT_ID])
);

-- =====================================================
-- RISK FUNCTION and MONITORING Tables
-- =====================================================

-- Table: RISK_FUNCTIONMAST - Risk Function Master
CREATE TABLE [RISK_FUNCTIONMAST] (
    [FUNCTION_ID] BIGINT NOT NULL  -- Function ID,
    [FUNCTION_NAME] VARCHAR(200) NOT NULL  -- Function Name,
    [FUNCTION_CREATEDBY] BIGINT NOT NULL  -- Function Created By,
    [FUNCTION_CREATEDON] DATETIME2(3) NOT NULL  -- Function Created On,
    [FUNCTION_MODIFIEDBY] BIGINT NULL  -- Function Modified By,
    [FUNCTION_MODIFIEDON] DATETIME2(3) NULL  -- Function Modified On,
    CONSTRAINT [PK_RISK_FUNCTIONMAST] PRIMARY KEY ([FUNCTION_ID])
);

-- Table: RISK_FUNCTIONDET - Risk Function Detail
CREATE TABLE [RISK_FUNCTIONDET] (
    [FUNDET_ID] BIGINT NOT NULL  -- Function Detail ID,
    [FUNDET_RiskID] BIGINT NOT NULL  -- Function Risk ID,
    [FUNDET_FUNCTIONID] BIGINT NOT NULL  -- Function ID,
    [FUNDET_LASTMODIFIEDBY] BIGINT NOT NULL  -- Function ID Last Modified By,
    [FUNDET_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Function ID Last Modified On,
    CONSTRAINT [PK_RISK_FUNCTIONDET] PRIMARY KEY ([FUNDET_ID])
);

-- Table: RISK_DIVISIONFUNCTIONMAP - Risk Division Function Map
CREATE TABLE [RISK_DIVISIONFUNCTIONMAP] (
    [DFM_MAPID] BIGINT NOT NULL  -- Function Map ID,
    [DFM_DIVISIONID] BIGINT NOT NULL  -- Function Map  Division ID,
    [DFM_FUNCTIONID] BIGINT NOT NULL  -- Function Map Function ID,
    [DFM_CREATEDBY] BIGINT NOT NULL  -- Function Map Created By,
    [DFM_CREATEDON] DATETIME2(3) NOT NULL  -- Function Map Created On,
    [DFM_MODIFIEDBY] BIGINT NULL  -- Function Map Modified By,
    [DFM_MODIFIEDON] DATETIME2(3) NULL  -- Function Map Modified On,
    CONSTRAINT [PK_RISK_DIVISIONFUNCTIONMAP] PRIMARY KEY ([DFM_MAPID])
);

-- Table: RISK_MONITOR - Risk Monitor By
CREATE TABLE [RISK_MONITOR] (
    [RISKMON_ID] BIGINT NOT NULL  -- Risk Monitor ID,
    [RISKMON_RISKID] BIGINT NOT NULL  -- Risk ID,
    [RISKMON_BY] CHAR(3) NOT NULL  -- BRD/CLT/BLT/ULT,
    [RISKMON_REVFREQUENCY] CHAR(1) NOT NULL  -- Frequency (M/H/A/Q),
    [RISKMON_LASTMODIFIEDBY] BIGINT NOT NULL  -- Risk Monitor Last Modified By,
    [RISKMON_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Risk Monitor Last Modified On,
    CONSTRAINT [PK_RISK_MONITOR] PRIMARY KEY ([RISKMON_ID])
);

-- Table: RISK_FREQUENCYMAP - Risk Rating Frequency Map
CREATE TABLE [RISK_FREQUENCYMAP] (
    [FREQ_ID] BIGINT NOT NULL  -- Frequency ID,
    [FREQ_RATINGID] BIGINT NOT NULL  -- Rating ID,
    [FREQ_MONITORCODE] CHAR(3) NOT NULL  -- Monitored By  BRD/CLT/BLT/ULT,
    [FREQ_CODE] CHAR(1) NOT NULL  -- Frequency (M/H/A/Q),
    [FREQ_MONTH] VARCHAR(24) NOT NULL  -- Review Month(01020304….12),
    [FREQ_DAY] INT NOT NULL  -- Review Day,
    [FREQ_CREATEDBY] BIGINT NOT NULL  -- Frequency Created By,
    [FREQ_CREATEDON] DATETIME2(3) NOT NULL  -- Frequency Created On,
    [FREQ_MODIFIEDBY] BIGINT NULL  -- Frequency Modified By,
    [FREQ_MODIFIEDON] DATETIME2(3) NULL  -- Frequency Modified On,
    CONSTRAINT [PK_RISK_FREQUENCYMAP] PRIMARY KEY ([FREQ_ID])
);

-- Table: RISKUNIT_CHAMPMAP - Unit Risk Champion Master
CREATE TABLE [RISKUNIT_CHAMPMAP] (
    [CHAMP_ID] BIGINT NOT NULL  -- Unit Champion ID,
    [CHAMP_EMPSYSID] BIGINT NOT NULL  -- Employee System ID,
    [CHAMP_TYPE] CHAR(1) NOT NULL  -- Organization(O) / Business(B) / Sub Division(S)  / Unit(U) / Super User - IAT(A),
    [CHAMP_ORGID] BIGINT NOT NULL  -- Organization ID,
    [CHAMP_BUSID] BIGINT NOT NULL  -- Business ID,
    [CHAMP_DIVISIONID] BIGINT NOT NULL  -- Sub Division ID,
    [CHAMP_UNITID] BIGINT NOT NULL  -- Unit ID,
    [CHAMP_CREATEDBY] BIGINT NOT NULL  -- Champion Created By,
    [CHAMP_CREATEDON] DATETIME2(3) NOT NULL  -- Champion Created On,
    [CHAMP_MODIFIEDBY] BIGINT NOT NULL  -- Champion Modified By,
    [CHAMP_MODIFIEDON] DATETIME2(3) NOT NULL  -- Champion Modified On,
    CONSTRAINT [PK_RISKUNIT_CHAMPMAP] PRIMARY KEY ([CHAMP_ID])
);

-- Table: RISK_UNITDET - Risk HR Unit Details
CREATE TABLE [RISK_UNITDET] (
    [HRUDET_ID] BIGINT NOT NULL  -- HR Unit Detail ID,
    [HRUDET_RISKID] BIGINT NOT NULL  -- HR Unit Risk ID,
    [HRUDET_RISKUNITID] BIGINT NOT NULL  -- HR Unit ID,
    [HRUDET_LASTMODIFIEDBY] BIGINT NOT NULL  -- HR Unit ID Last Modified By,
    [HRUDET_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- HR Unit ID Last Modified On,
    CONSTRAINT [PK_RISK_UNITDET] PRIMARY KEY ([HRUDET_ID])
);

-- =====================================================
-- RISK APPROVAL and MITIGATION Tables
-- =====================================================

-- Table: RISK_APPDET - Risk Approval Details
CREATE TABLE [RISK_APPDET] (
    [APP_ID] BIGINT NOT NULL  -- Approver ID,
    [APP_RISKID] BIGINT NOT NULL  -- Risk ID,
    [APP_EMPSYSID] BIGINT NOT NULL  -- Approver Employee System ID,
    [APP_STATUS] CHAR(1) NOT NULL  -- Approval Status - A / R,
    [APP_REMARKS] VARCHAR(500) NOT NULL  -- Reason for Approval / Rejection,
    [APP_LASTMODIFIEDBY] BIGINT NOT NULL  -- Approver Last Modified By,
    [APP_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Approver Last Modified On,
    [APP_TYPE] CHAR(1) NULL  -- If risk entry - 'R', If selfAssessment Entry - 'S',
    CONSTRAINT [PK_RISK_APPDET] PRIMARY KEY ([APP_ID])
);

-- Table: RISK_MITIGATION - Risk Mitigation
CREATE TABLE [RISK_MITIGATION] (
    [MIT_ID] BIGINT NOT NULL  -- Mitigation ID,
    [MIT_RISKID] BIGINT NOT NULL  -- Risk ID,
    [MIT_ACTION] VARCHAR(2000) NOT NULL  -- Action Description,
    [MIT_ORGDATE] DATETIME2(3) NOT NULL  -- Original Due Date,
    [MIT_DUEDATE] DATETIME2(3) NOT NULL  -- Latest Revised Due Date,
    [MIT_OWNER] BIGINT NOT NULL  -- Owner,
    [MIT_REVIEWER] BIGINT NOT NULL  -- Reviewer,
    [MIT_STATUS] CHAR(1) NOT NULL  -- M - Mitigated / L - Live / D - Dropped,
    [MIT_PROBRED] DECIMAL(38) NULL  -- Probability Reduction Percentage,
    [MIT_IMPACTRED] DECIMAL(38) NULL  -- Impact Reduction Percentage,
    [MIT_APPEMPSYSID] BIGINT NULL  -- IA Approver Employee System ID,
    [MIT_ATTACHMENT] VARCHAR(2000) NULL  -- Attachment if any,
    [MIT_CREATEDBY] BIGINT NOT NULL  -- Mitigation Created By,
    [MIT_CREATEDON] DATETIME2(3) NOT NULL  -- Mitigation Created On,
    [MIT_MODIFIEDBY] BIGINT NULL  -- Mitigation Modified By,
    [MIT_MODIFIEDON] DATETIME2(3) NULL  -- Mitigation Modified On,
    CONSTRAINT [PK_RISK_MITIGATION] PRIMARY KEY ([MIT_ID])
);

-- Table: RISK_MITIGATIONACTION - Risk Mitigation Action
CREATE TABLE [RISK_MITIGATIONACTION] (
    [ACTION_ID] BIGINT NOT NULL  -- Action ID,
    [ACTION_MITID] BIGINT NOT NULL  -- Mitigation ID,
    [ACTION_DUEDATE] DATETIME2(3) NOT NULL  -- Due Date,
    [ACTION_STATUS] CHAR(1) NOT NULL  -- Not Completed / Completed / Partially Completed / Dropped,
    [ACTION_REVDUEDATE] DATETIME2(3) NULL  -- Revised Due Date,
    [ACTION_APPSTATUS] CHAR(1) NOT NULL  -- E/P/A - Entry if Resend / Pending for Approval  / Approval,
    [ACTION_COMMENTS] VARCHAR(500) NOT NULL  -- Action Comments,
    [ACTION_COMPLETIONDATE] DATETIME2(3) NULL  -- Action Completion Date,
    [ACTION_CREATEDBY] BIGINT NOT NULL  -- Mitigation Action Created By,
    [ACTION_CREATEDON] DATETIME2(3) NOT NULL  -- Mitigation Action Created On,
    [ACTION_MODIFIEDBY] BIGINT NULL  -- Mitigation Action Modified By,
    [ACTION_MODIFIEDON] DATETIME2(3) NULL  -- Mitigation Action Modified On,
    CONSTRAINT [PK_RISK_MITIGATIONACTION] PRIMARY KEY ([ACTION_ID])
);

-- Table: RISK_MITAPPDET - Risk Mitigation Approval
CREATE TABLE [RISK_MITAPPDET] (
    [APP_ID] BIGINT NOT NULL  -- Approver ID,
    [APP_ACTIONID] BIGINT NOT NULL  -- Action ID,
    [APP_EMPSYSID] BIGINT NOT NULL  -- Approver Employee System ID,
    [APP_STATUS] CHAR(1) NOT NULL  -- Approval Status - A / R,
    [APP_REMARKS] VARCHAR(50) NOT NULL  -- Reason for Approval / Rejection,
    [APP_LASTMODIFIEDBY] BIGINT NOT NULL  -- Approver Last Modified By,
    [APP_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Approver Last Modified On,
    CONSTRAINT [PK_RISK_MITAPPDET] PRIMARY KEY ([APP_ID])
);

-- =====================================================
-- RISK SELF ASSESSMENT Tables
-- =====================================================

-- Table: RISK_SELFASSDET - Self Assessment Details
CREATE TABLE [RISK_SELFASSDET] (
    [ASS_ID] BIGINT NOT NULL  -- Assessment ID,
    [ASS_TYPE] CHAR(1) NOT NULL  -- Self Assessment Type (O -Org / B - Business / U - Unit),
    [ASS_TYPEREFID] BIGINT NOT NULL  -- Reference ID (ASS_TYPE = O - Org ID / ASS_TYPE = B - Business ID / ASS_TYPE = U - Unit ID),
    [ASS_MONBY] CHAR(3) NOT NULL  -- Monitored By,
    [ASS_DUEDATE] DATETIME2(3) NOT NULL  -- Assessment Due Date,
    [ASS_MEETINGFLAG] CHAR(1) NOT NULL  -- Status of Meeting (P- Pending to be conducted / Y - Conducted / N - Skipped),
    [ASS_STATUS] CHAR(1) NOT NULL  -- Status of Self Assessmen (E - Pending to be conducted / P - Pending for approval / C - Completed  (On Approval If Meeting Conducted Yes) / S - Skipped (If Meeting Conducted No) ),
    [ASS_REASON] VARCHAR(200) NULL  -- Reason for Skipping Meeting / Any notes regarding meeting,
    [ASS_DATE] DATETIME2(3) NOT NULL  -- Assessment Date,
    [ASS_REVIEWFLAG] CHAR(1) NOT NULL  -- Review Done Flag,
    [ASS_NEWFLAG] CHAR(1) NOT NULL  -- New Risks Identified Flag,
    [ASS_NEWLIST] VARCHAR(200) NULL  -- New Risk List,
    [ASS_MITFLAG] CHAR(1) NOT NULL  -- Risk Mitigated Flag,
    [ASS_MITLIST] VARCHAR(200) NULL  -- Mitigated List,
    [ASS_APPSTATUS] CHAR(1) NOT NULL  -- Approval Status ( P - Pending / A -Approved / R - Resend),
    [ASS_LASTMODIFIEDBY] BIGINT NOT NULL  -- Assessment Last Modified By,
    [ASS_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Assessment Last Modified On,
    CONSTRAINT [PK_RISK_SELFASSDET] PRIMARY KEY ([ASS_ID])
);

-- Table: RISK_EVENTASSDET - event ass details
CREATE TABLE [RISK_EVENTASSDET] (
    [EVENTASS_ID] DECIMAL(38) NOT NULL  -- Risk Event Assessment ID,
    [EVENTASS_ASSID] DECIMAL(38) NOT NULL  -- Self Assessment ID for Risk,
    [EVENTASS_RISKID] DECIMAL(38) NOT NULL  -- Risk ID,
    [EVENTASS_LASTMODIFIEDBY] DECIMAL(38) NOT NULL  -- Modified By,
    [EVENTASS_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Modified on
);

-- Table: RISK_SELFASSCOMMENT - Self Assessment Risk Comment
CREATE TABLE [RISK_SELFASSCOMMENT] (
    [COM_ID] BIGINT NOT NULL  -- Comments ID,
    [ASS_ID] BIGINT NOT NULL  -- Assessment ID,
    [RISK ID] BIGINT NOT NULL  -- Risk ID,
    [Comments] VARCHAR(2000) NOT NULL  -- Comments on Risk,
    [Updated On] BIGINT NOT NULL  -- Comments Last Modified By,
    [Updated By] DATETIME2(3) NOT NULL  -- Comments Last Modified On,
    CONSTRAINT [PK_RISK_SELFASSCOMMENT] PRIMARY KEY ([COM_ID])
);

PRINT 'RISK Module - Tables created successfully.';
GO
