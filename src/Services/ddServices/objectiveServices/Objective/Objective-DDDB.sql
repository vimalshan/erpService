-- Module: Objective
USE DDDB;
GO

-- Table: CPREQUEST_APPROVAL
CREATE TABLE [CPREQUEST_APPROVAL] (
    [CPREQUEST_APPID] DECIMAL(38) NOT NULL,
    [CPREQUEST_ID] DECIMAL(38) NULL  -- Table ID,
    [CPREQUEST_APRSYSID] DECIMAL(38) NOT NULL  -- Approver Sys ID,
    [CPREQUEST_STATUS] VARCHAR(1) NOT NULL  -- A- Approved, R-Returned,
    [CPREQUEST_REMARKS] VARCHAR(1000) NULL  -- Remarks is mandatory for Returned.
);



-- Table: CPREQUEST_DETAILS
CREATE TABLE [CPREQUEST_DETAILS] (
    [CPREQUEST_DETID] DECIMAL(38) NOT NULL  -- Detail ID,
    [CPREQUEST_MAINID] DECIMAL(38) NOT NULL  -- CPREQUEST_ID from CPREQUEST_MAIN,
    [CPREQUEST_CPID] DECIMAL(38) NULL  -- CP ID from CP_FINAL,
    [CPREQUEST_DDYEARID] DECIMAL(38) NOT NULL  -- DD year ID,
    [CPREQUEST_EMPSYSID] DECIMAL(38) NOT NULL  -- Employee SysID,
    [CPREQUEST_SOURCE] VARCHAR(5) NOT NULL  -- Control point Source  (DD, CP, PC),
    [CPREQUEST_REFID] DECIMAL(38) NOT NULL  -- For DD, DD Ref No, CP -Change Main Nom PC- Confirmation No,
    [CPREQUEST_SRLNUM] DECIMAL(38) NOT NULL  -- Serial No,
    [CPREQUEST_DESC] VARCHAR(4000) NOT NULL  -- Control Point Description,
    [CPREQUEST_CATEGORY] VARCHAR(100) NOT NULL  -- CP Category,
    [CPREQUEST_UOM] VARCHAR(65) NOT NULL  -- Unit of Measurement,
    [CPREQUEST_FROM] VARCHAR(50) NOT NULL  -- Unit From,
    [CPREQUEST_TO] VARCHAR(50) NOT NULL  -- Unit TO,
    [CPREQUEST_VERNO] DECIMAL(38) NOT NULL  -- Version No,
    [CPREQUEST_WEIGHTAGE] DECIMAL(38) NULL,
    [CPREQUEST_ACCOUNTABILITYID] DECIMAL(38) NULL  -- Accountability ID,
    [CPREQUEST_MODIFIEDDATE] DATETIME2(3) NOT NULL  -- Last Modified Date,
    [CPREQUEST_APPSTATUS] VARCHAR(1) NULL  -- M-Modified , D-Deleted , N-Newly Added Not approved,
    CONSTRAINT [PK_CPREQUEST_DETAILS] PRIMARY KEY ([CPREQUEST_DETID])
);



-- Table: CPREQUEST_MAIN
CREATE TABLE [CPREQUEST_MAIN] (
    [CPREQUEST_ID] DECIMAL(38) NOT NULL,
    [CPREQUEST_EMPSYSID] DECIMAL(38) NOT NULL  -- Employee Sys ID,
    [CPREQUEST_CREATEDON] DATETIME2(3) NOT NULL  -- Request Created /Start Date,
    [CPREQUEST_SUBMITTEDON] DATETIME2(3) NULL  -- Request Submitted Date for approval,
    [CPREQUEST_STATUS] VARCHAR(1) NOT NULL  -- P-Pending, A- Approved,R-Returned, O-Pending with other Appraiser, N- Not Submitted,
    [CPREQUEST_REMARKS] VARCHAR(500) NULL  -- Return Remarks,
    [CPREQUEST_DDYEARID] DECIMAL(38) NOT NULL,
    [CPREQUEST_SUBORDINATE] CHAR(1) NULL,
    CONSTRAINT [PK_CPREQUEST_MAIN] PRIMARY KEY ([CPREQUEST_ID])
);



-- Table: CP_FINAL
CREATE TABLE [CP_FINAL] (
    [CP_ID] DECIMAL(38) NOT NULL,
    [CP_DDYEARID] DECIMAL(38) NOT NULL  -- DD year ID,
    [CP_EMPSYSID] DECIMAL(38) NOT NULL  -- Employee SysID,
    [CP_SOURCE] VARCHAR(5) NOT NULL  -- Control point Source  (DD, CP, PC),
    [CP_REFID] DECIMAL(38) NOT NULL  -- For DD, DD Ref No, CP -Change Main Nom PC- Confirmation No,
    [CP_SRLNUM] DECIMAL(38) NOT NULL  -- Serial No,
    [CP_DESC] VARCHAR(4000) NOT NULL  -- Control Point Description,
    [CP_CATEGORY] VARCHAR(100) NOT NULL  -- CP Category,
    [CP_UOM] VARCHAR(65) NOT NULL  -- Unit of Measurement,
    [CP_FROM] VARCHAR(50) NOT NULL  -- Unit From,
    [CP_TO] VARCHAR(50) NOT NULL  -- Unit To,
    [CP_VERNO] DECIMAL(38) NOT NULL  -- Version No,
    [CP_WEIGHTAGE] DECIMAL(38) NULL  -- Weightage of CP,
    [CP_ACCOUNTABILITYID] DECIMAL(38) NULL  -- Accountability ID,
    [CP_MODIFIEDDATE] DATETIME2(3) NULL  -- Last Modified Date,
    CONSTRAINT [PK_CP_FINAL] PRIMARY KEY ([CP_ID])
);



-- Table: CP_FINAL_LOG
CREATE TABLE [CP_FINAL_LOG] (
    [CP_ID] DECIMAL(38) NOT NULL  -- CP ID from CP_FINAL,
    [CP_DDYEARID] DECIMAL(38) NOT NULL  -- DD year ID,
    [CP_EMPSYSID] DECIMAL(38) NOT NULL  -- Employee SysID,
    [CP_SOURCE] VARCHAR(5) NOT NULL  -- Control point Source  (DD, CP, PC),
    [CP_REFID] DECIMAL(38) NOT NULL  -- For DD, DD Ref No, CP -Change Main Nom PC- Confirmation No,
    [CP_SRLNUM] DECIMAL(38) NOT NULL  -- Serial No,
    [CP_DESC] VARCHAR(4000) NOT NULL  -- Control Point Description,
    [CP_CATEGORY] VARCHAR(100) NOT NULL  -- CP Category,
    [CP_UOM] VARCHAR(65) NOT NULL  -- Unit of Measurement,
    [CP_FROM] VARCHAR(20) NOT NULL  -- Unit From,
    [CP_TO] VARCHAR(20) NOT NULL  -- Unit TO,
    [CP_VERNO] DECIMAL(38) NOT NULL  -- Version No,
    [CP_WEIGHTAGE] DECIMAL(38) NULL  -- Weightage of CP,
    [CP_ACCOUNTABILITYID] DECIMAL(38) NULL  -- Accountability ID,
    [CP_MODIFIEDDATE] DATETIME2(3) NULL  -- Last Modified Date,
    [CP_STATUS] VARCHAR(1) NOT NULL  -- D-Deleted, M- Modified,
    [CP_LOGDATE] DATETIME2(3) NOT NULL  -- Log Created Date,
    [CP_LOGSOURCE] CHAR(3) NULL
);



-- Table: DD_APP_RES_GOAL
CREATE TABLE [DD_APP_RES_GOAL] (
    [GL_REQ_NUM] DECIMAL(38) NOT NULL  -- Request Number,
    [GL_QTN_NUM] DECIMAL(38) NOT NULL  -- Question Number,
    [GL_SRL_NUM] DECIMAL(38) NOT NULL  -- Answer Serial Number,
    [GL_TSK_ACT] VARCHAR(400) NULL  -- Task/Activity details,
    [GL_PRG_ACH] VARCHAR(2000) NULL  -- Progress/Achievement status,
    [GL_DAY_TIM] VARCHAR(400) NULL  -- Duration time,
    [GL_APP_ID] VARCHAR(30) NOT NULL  -- User Id,
    [GL_APP_NUM] DECIMAL(38) NOT NULL  -- User Number,
    [GL_DATE] DATETIME2(3) NULL
);



-- Table: DD_CONTROLPOINTMAIN
CREATE TABLE [DD_CONTROLPOINTMAIN] (
    [GL_GOL_NUM] DECIMAL(38) NOT NULL,
    [GL_USR_ID] VARCHAR(50) NULL,
    [GL_PIN_NUM] DECIMAL(38) NULL,
    [GL_PRD_FRM] DATETIME2(3) NULL,
    [GL_PRD_TO] DATETIME2(3) NULL,
    [GL_STS_FLG] CHAR(1) NULL,
    [GL_REQ_NUM] DECIMAL(38) NULL
);



-- Table: DD_CONTROLPOINTS_SUB
CREATE TABLE [DD_CONTROLPOINTS_SUB] (
    [GL_GOL_NO] DECIMAL(38) NOT NULL,
    [GL_SRL_NO] DECIMAL(38) NOT NULL,
    [GL_GOL_DES] VARCHAR(4000) NULL,
    [GL_GOL_FRM] VARCHAR(20) NULL,
    [GL_GOL_TO] VARCHAR(20) NULL,
    [GL_GOL_ACH] VARCHAR(4000) NULL,
    [GL_GOL_DIFF] VARCHAR(4000) NULL,
    [GL_EXP_COD] VARCHAR(3) NULL,
    [GL_GOL_FLG] VARCHAR(3) NULL,
    [GL_MOD_SRLNO] DECIMAL(38) NULL,
    [GL_UOM] VARCHAR(65) NULL,
    [GL_CATEGORY] VARCHAR(100) NULL,
    [GL_REMARKS] VARCHAR(4000) NULL
);



-- Table: DD_GOALAPPR_REM
CREATE TABLE [DD_GOALAPPR_REM] (
    [GL_REQ_NO] DECIMAL(38) NULL,
    [GL_SNO] DECIMAL(38) NULL,
    [GL_USR_COD] VARCHAR(200) NULL,
    [GL_PIN_NUM] DECIMAL(38) NULL,
    [GL_REM] VARCHAR(4000) NULL
);



-- Table: DD_GOALFEEDBACK
CREATE TABLE [DD_GOALFEEDBACK] (
    [DD_GOL_NO] DECIMAL(38) NULL  -- Goal Number,
    [DD_SRL_NO] DECIMAL(38) NULL  -- Goal Srl No,
    [DD_APP_ID] VARCHAR(50) NULL  -- Approver id,
    [DD_APP_PIN] DECIMAL(38) NULL  -- Approver Pinnum,
    [DD_APP_REM] VARCHAR(4000) NULL  -- Approver Remarks
);



-- Table: DD_GOALMAIN
CREATE TABLE [DD_GOALMAIN] (
    [GL_GOL_NUM] DECIMAL(38) NOT NULL  -- Goal Number,
    [GL_USR_ID] VARCHAR(50) NULL  -- User id of the employee,
    [GL_PIN_NUM] DECIMAL(38) NULL  -- Pin num of the employee,
    [GL_PRD_FRM] DATETIME2(3) NULL  -- Goal For the Period(From),
    [GL_PRD_TO] DATETIME2(3) NULL  -- Goal For the Period(To),
    [GL_APP_RMK] VARCHAR(4000) NULL  -- Remarks of the appraiser,
    [GL_ATT_FLG] CHAR(1) NULL  -- Attachment Present Or not (Y-Yes N-No),
    [GL_ATT_URL] VARCHAR(500) NULL  -- Attachemnt Url if attachment is present,
    [GL_REF_NO] DECIMAL(38) NULL  -- Ref No From DD Or Confirmation,
    [GL_FRM_FLG] CHAR(1) NULL  -- Flg D-For DD and C-Confiramtion,
    [GL_NXT_REV] DATETIME2(3) NULL  -- Next Review Date,
    [GL_CLS_DAT] DATETIME2(3) NULL  -- Closure Date,
    [GL_STS_FLG] CHAR(1) NULL  -- STATUS OF THE GOALSHEET N-with apraisee, Y-completed by apraisee n pending with one of the aprraisers ;C- completed by all appraisers and pending with appriasee for feedback acceptance; A- feedback(oral) accepted by appraisee
);



-- Table: DD_GOALMAIN08
CREATE TABLE [DD_GOALMAIN08] (
    [GL_GOL_NUM] DECIMAL(38) NULL  -- Goal Number,
    [GL_USR_ID] VARCHAR(50) NULL  -- User id of the employee,
    [GL_PIN_NUM] DECIMAL(38) NULL  -- Pin num of the employee,
    [GL_PRD_FRM] DATETIME2(3) NULL  -- Goal For the Period(From),
    [GL_PRD_TO] DATETIME2(3) NULL  -- Goal For the Period(To),
    [GL_APP_RMK] VARCHAR(4000) NULL  -- Remarks of the appraiser,
    [GL_NXT_REV] DATETIME2(3) NULL  -- Next Review Date,
    [GL_CLS_DAT] DATETIME2(3) NULL  -- Closure Date,
    [GL_STS_FLG] CHAR(1) NULL  -- STATUS OF THE GOALSHEET N-with apraisee, Y-completed by apraisee n pending with one of the aprraisers ;,
    [GL_REQ_NUM] DECIMAL(38) NULL  -- Request Number
);



-- Table: DD_GOALMAIN08LOG
CREATE TABLE [DD_GOALMAIN08LOG] (
    [GL_GOL_NUM] DECIMAL(38) NULL,
    [GL_USR_ID] VARCHAR(50) NULL,
    [GL_PIN_NUM] DECIMAL(38) NULL,
    [GL_PRD_FRM] DATETIME2(3) NULL,
    [GL_PRD_TO] DATETIME2(3) NULL,
    [GL_APP_RMK] VARCHAR(4000) NULL,
    [GL_NXT_REV] DATETIME2(3) NULL,
    [GL_CLS_DAT] DATETIME2(3) NULL,
    [GL_STS_FLG] CHAR(1) NULL,
    [GL_REQ_NUM] DECIMAL(38) NULL
);



-- Table: DD_GOALMAIN09
CREATE TABLE [DD_GOALMAIN09] (
    [GL_GOL_NUM] DECIMAL(38) NOT NULL  -- Goal Number(Request Number),
    [GL_USR_ID] VARCHAR(50) NULL  -- User id of the employee,
    [GL_PIN_NUM] DECIMAL(38) NULL  -- Pin num of the employee,
    [GL_PRD_FRM] DATETIME2(3) NULL  -- Goal For the Period(From),
    [GL_PRD_TO] DATETIME2(3) NULL  -- Goal For the Period(To),
    [GL_STS_FLG] CHAR(1) NULL  -- STATUS OF THE GOALSHEET N-with apraisee, Y-completed by apraisee n pending with one of the aprraisers ;R-Resent to AppraiseeC- completed by all appraisers and pending with appriasee for feedback acceptance; A- feedback(oral) accepted by appraisee,
    [GL_REQ_NUM] DECIMAL(38) NULL  -- Request number used in DD
);



-- Table: DD_GOALSUB
CREATE TABLE [DD_GOALSUB] (
    [GL_GOL_NO] DECIMAL(38) NULL  -- Goal  No from Goal Main,
    [GL_SRL_NO] DECIMAL(38) NULL  -- Goal Srl No,
    [GL_GOL_DES] VARCHAR(4000) NULL  -- Goal Description,
    [GL_GOL_FRM] VARCHAR(500) NULL  -- Goal From,
    [GL_GOL_TO] VARCHAR(500) NULL  -- Goal To,
    [GL_GOL_ACH] VARCHAR(4000) NULL  -- Acheivements of the Goal,
    [GL_GOL_DIFF] VARCHAR(4000) NULL  -- Difficulties Faced in the Goal,
    [GL_EXP_COD] VARCHAR(3) NULL  -- EE - Exceeds expectation, BE - Below, ME - Met Exp, DR - Dropped,
    [GL_GOL_FLG] VARCHAR(3) NULL,
    [GL_MOD_SRLNO] DECIMAL(38) NULL  -- new srl_no in case the goal is modified
);



-- Table: DD_GOALSUB08
CREATE TABLE [DD_GOALSUB08] (
    [GL_GOL_NO] DECIMAL(38) NULL  -- Goal  No from Goal Main,
    [GL_SRL_NO] DECIMAL(38) NULL  -- Goal Srl No,
    [GL_GOL_DES] VARCHAR(4000) NULL  -- Goal Description,
    [GL_GOL_FRM] VARCHAR(20) NULL  -- Goal From,
    [GL_GOL_TO] VARCHAR(20) NULL  -- Goal To,
    [GL_GOL_ACH] VARCHAR(4000) NULL  -- Acheivements of the Goal,
    [GL_GOL_DIFF] VARCHAR(4000) NULL  -- Difficulties Faced in the Goal,
    [GL_EXP_COD] VARCHAR(3) NULL  -- EE - Exceeds expectation, BE - Below, ME - Met Exp, DR - Dropped,
    [GL_GOL_FLG] VARCHAR(3) NULL,
    [GL_MOD_SRLNO] DECIMAL(38) NULL  -- new srl_no in case the goal is modified,
    [GL_GOL_CAT] VARCHAR(100) NULL  -- Goal Category,
    [GL_GOL_UOM] VARCHAR(65) NULL  -- UOM,
    [GL_REM] VARCHAR(4000) NULL  -- Remarks
);



-- Table: DD_GOALSUB08_LOG
CREATE TABLE [DD_GOALSUB08_LOG] (
    [GL_GOL_NO] DECIMAL(38) NULL,
    [GL_SRL_NO] DECIMAL(38) NULL,
    [GL_GOL_DES] VARCHAR(4000) NULL,
    [GL_GOL_FRM] VARCHAR(500) NULL,
    [GL_GOL_TO] VARCHAR(500) NULL,
    [GL_GOL_ACH] VARCHAR(4000) NULL,
    [GL_GOL_DIFF] VARCHAR(4000) NULL,
    [GL_EXP_COD] VARCHAR(3) NULL,
    [GL_GOL_FLG] VARCHAR(3) NULL,
    [GL_MOD_SRLNO] DECIMAL(38) NULL,
    [GL_GOL_CAT] VARCHAR(4000) NULL,
    [GL_GOL_UOM] VARCHAR(4000) NULL,
    [GL_USERID] VARCHAR(200) NULL,
    [GL_PINUM] DECIMAL(38) NULL
);



-- Table: DD_GOALSUB09
CREATE TABLE [DD_GOALSUB09] (
    [GL_GOL_NO] DECIMAL(38) NOT NULL  -- Goal  No from Goal Main,
    [GL_SRL_NO] DECIMAL(38) NOT NULL  -- Goal Srl No,
    [GL_GOL_DES] VARCHAR(4000) NULL  -- Goal Description,
    [GL_GOL_FRM] VARCHAR(500) NULL  -- Goal From,
    [GL_GOL_TO] VARCHAR(500) NULL  -- Goal To,
    [GL_GOL_ACH] VARCHAR(4000) NULL  -- Acheivements of the Goal,
    [GL_GOL_DIFF] VARCHAR(4000) NULL  -- Difficulties Faced in the Goal,
    [GL_EXP_COD] VARCHAR(3) NULL  -- LI-Live,MD-Modified,, DR - Dropped,
    [GL_GOL_FLG] VARCHAR(3) NULL  -- N-non editable,V-editable,
    [GL_MOD_SRLNO] DECIMAL(38) NULL  -- new srl_no in case the goal is modified,
    [GL_UOM] VARCHAR(65) NULL  -- UOM,
    [GL_CATEGORY] VARCHAR(100) NULL  -- Category,
    [GL_REMARKS] VARCHAR(4000) NULL  -- Remarks
);

