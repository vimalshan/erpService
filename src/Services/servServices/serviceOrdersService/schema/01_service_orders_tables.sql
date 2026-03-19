-- =====================================================
-- Service Orders Microservice Database Schema
-- Tables for service order management
-- =====================================================

USE [DELL_RTU_SERVICE_ORDERS]
GO

-- =====================================================
-- SERVICE_ORDER_HDR (Main service orders)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SERVICE_ORDER_HDR](
	[SERNO_DELL] [varchar](12) NOT NULL,
	[BRANCH] [varchar](15) NULL,
	[SAP_LOGIN] [varchar](15) NULL,
	[POSTING_DATE] [datetime] NULL,
	[SAP_ID] [varchar](12) NULL,
	[SLA] [varchar](15) NULL,
	[PRODUCT_ID] [varchar](50) NULL,
	[SERVICE_TAG] [varchar](25) NULL,
	[RELATED_CASE] [varchar](25) NULL,
	[LOB] [varchar](25) NULL,
	[CALL_STATUS] [varchar](50) NULL,
	[CURRENT_RC] [varchar](25) NULL,
	[ENGINEER_ID] [varchar](15) NULL,
	[ENGINEER_NAME] [varchar](50) NULL,
	[ENGMOB_NO] [varchar](15) NULL,
	[ORG_NAME] [varchar](50) NULL,
	[CUSTOMER_NAME] [varchar](25) NULL,
	[CONTACT_NO] [varchar](50) NULL,
	[ADDRESS] [varchar](256) NULL,
	[ALT_CNTNO] [varchar](50) NULL,
	[DISPATCH_DATE] [datetime] NULL,
	[CUSTETA_DATE] [datetime] NULL,
	[PARTETA_DATE] [datetime] NULL,
	[TECH_SUPNAME] [varchar](50) NULL,
	[DSP] [varchar](250) NULL,
	[PRB_DESC] [varchar](250) NULL,
	[LONG_DESC] [varchar](4000) NULL,
	[REASON_CODE] [varchar](250) NULL,
	[ACTIVITY] [varchar](100) NULL,
	[ONSITE_DT] [datetime] NULL,
	[CMPLTD_DT] [datetime] NULL,
	[FLAG] [char](5) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[DISCLAIMER] [varchar](250) NULL,
	[SIGN_CUSTOMER_NAME] [varchar](30) NULL,
	[SIGN_CUST_JOBTITLE] [varchar](40) NULL,
	[CUS_COMMENTS] [varchar](150) NULL,
	[CUST_MAILID] [varchar](60) NULL,
	[PART_COUNT] [int] NULL,
	[ISPARTCALL] [bit] NULL,
	[ISONLINE_MODE] [bit] NULL,
	[FSD_LEGACY] [varchar](25) NULL,
	[CALL_STATUS_TXT] [varchar](25) NULL,
	[PRI_EMAIL_ID] [varchar](60) NULL,
	[ALT_CUSTOMER_NAME] [varchar](50) NULL,
	[ALT_SEC_CONTACT_NO] [varchar](60) NULL,
	[ALT_SEC_EMAIL_ID] [varchar](60) NULL,
	[SYSTEM_PUR_30DAYS] [varchar](25) NULL,
	[KYHD] [varchar](250) NULL,
	[PROSUPPORT] [varchar](25) NULL,
	[ACCIDENTAL_DAMAGE] [varchar](250) NULL,
	[Hour_Qty] [varchar](250) NULL,
	[PPID] [varchar](250) NULL,
	[WAR_EXPIRE] [varchar](250) NULL,
	[CT] [varchar](4000) NULL,
	[OS] [varchar](4000) NULL,
	[SEV] [varchar](4000) NULL,
	[RPT] [varchar](4000) NULL,
	[P] [varchar](4000) NULL,
	[S] [varchar](4000) NULL,
	[D] [varchar](4000) NULL,
	[DSPI] [varchar](4000) NULL,
	[ALT_CONTACT_NO1] [varchar](50) NULL,
	[PART_ACTION] [varchar](4000) NULL,
	[SWAPPED_SERVICETAG] [varchar](250) NULL,
	[CUST_START_ETA] [datetime] NULL,
	[E_ROFP] [varchar](10) NULL,
	[E_ROFA] [varchar](10) NULL,
	CONSTRAINT [PK_SERVICE_ORDER_HDR_1] PRIMARY KEY CLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [IX_SERVICE_ORD_HRD] UNIQUE NONCLUSTERED 
	(
		[SAP_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- SERVICE_ORDER_DET (Service order line items)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SERVICE_ORDER_DET](
	[SERNO_DELL] [varchar](12) NULL,
	[PART_NO] [varchar](50) NULL,
	[QUANTITY] [varchar](15) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[UNIQUE_ID] [varchar](10) NULL,
	[PART_STATUS] [varchar](10) NULL,
	[PART_USAGE_TYPE] [varchar](40) NULL,
	[PP_ID] [varchar](30) NULL,
	[COMMODITY] [varchar](20) NULL,
	[FAILURE_REASON] [varchar](50) NULL,
	[FAILURE_REASON_OTHRES] [varchar](150) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[GOOD_PPID] [varchar](30) NULL,
	[DEFECTIVE_PARTNO] [varchar](50) NULL,
	[ISDAMAGED] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- ACTIVITY_DONE (Service activities completed)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ACTIVITY_DONE](
	[ACT_DONE] [varchar](300) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_ACTIVITY_DONE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
