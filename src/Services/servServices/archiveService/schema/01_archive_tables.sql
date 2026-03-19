-- =====================================================
-- Archive Microservice Database Schema
-- Legacy and backup data tables
-- =====================================================

USE [DELL_RTU_ARCHIVE]
GO

-- =====================================================
-- OLD_SERVICE_ORDER_HDR (Legacy service orders)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OLD_SERVICE_ORDER_HDR](
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
	[CONTACT_NO] [varchar](15) NULL,
	[ADDRESS] [varchar](256) NULL,
	[ALT_CNTNO] [varchar](15) NULL,
	[DISPATCH_DATE] [datetime] NULL,
	[CUSTETA_DATE] [datetime] NULL,
	[PARTETA_DATE] [datetime] NULL,
	[TECH_SUPNAME] [varchar](50) NULL,
	[DSP] [varchar](100) NULL,
	[PRB_DESC] [varchar](250) NULL,
	[LONG_DESC] [varchar](4000) NULL,
	[REASON_CODE] [varchar](15) NULL,
	[ACTIVITY] [varchar](100) NULL,
	[ONSITE_DT] [datetime] NULL,
	[CMPLTD_DT] [datetime] NULL,
	[FLAG] [char](5) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_SERVICE_ORDER_HDR] PRIMARY KEY CLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [IX_SERVICE_ORDER_HDR] UNIQUE NONCLUSTERED 
	(
		[SAP_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- COPY_SERVICE_ORDER_HDR (Backup copy of service orders)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COPY_SERVICE_ORDER_HDR](
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
	[CONTACT_NO] [varchar](15) NULL,
	[ADDRESS] [varchar](256) NULL,
	[ALT_CNTNO] [varchar](15) NULL,
	[DISPATCH_DATE] [datetime] NULL,
	[CUSTETA_DATE] [datetime] NULL,
	[PARTETA_DATE] [datetime] NULL,
	[TECH_SUPNAME] [varchar](50) NULL,
	[DSP] [varchar](100) NULL,
	[PRB_DESC] [varchar](250) NULL,
	[LONG_DESC] [varchar](4000) NULL,
	[REASON_CODE] [varchar](15) NULL,
	[ACTIVITY] [varchar](100) NULL,
	[ONSITE_DT] [datetime] NULL,
	[CMPLTD_DT] [datetime] NULL,
	[FLAG] [char](5) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_COPY_SERVICE_ORDER_HDR] PRIMARY KEY CLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- COPY_OLD_SERVICE_ORDER_HDR (Older backup copy)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COPY_OLD_SERVICE_ORDER_HDR](
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
	[CONTACT_NO] [varchar](15) NULL,
	[ADDRESS] [varchar](256) NULL,
	[ALT_CNTNO] [varchar](15) NULL,
	[DISPATCH_DATE] [datetime] NULL,
	[CUSTETA_DATE] [datetime] NULL,
	[PARTETA_DATE] [datetime] NULL,
	[TECH_SUPNAME] [varchar](50) NULL,
	[DSP] [varchar](100) NULL,
	[PRB_DESC] [varchar](250) NULL,
	[LONG_DESC] [varchar](4000) NULL,
	[REASON_CODE] [varchar](15) NULL,
	[ACTIVITY] [varchar](100) NULL,
	[ONSITE_DT] [datetime] NULL,
	[CMPLTD_DT] [datetime] NULL,
	[FLAG] [char](5) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_COPY_OLD_SERVICE_ORDER_HDR] PRIMARY KEY CLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- service_order_hdr_temp1 (Temporary service order copy)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[service_order_hdr_temp1](
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
	[CONTACT_NO] [varchar](15) NULL,
	[ADDRESS] [varchar](256) NULL,
	[ALT_CNTNO] [varchar](15) NULL,
	[DISPATCH_DATE] [datetime] NULL,
	[CUSTETA_DATE] [datetime] NULL,
	[PARTETA_DATE] [datetime] NULL,
	[TECH_SUPNAME] [varchar](50) NULL,
	[DSP] [varchar](100) NULL,
	[PRB_DESC] [varchar](250) NULL,
	[LONG_DESC] [varchar](4000) NULL,
	[REASON_CODE] [varchar](15) NULL,
	[ACTIVITY] [varchar](100) NULL,
	[ONSITE_DT] [datetime] NULL,
	[CMPLTD_DT] [datetime] NULL,
	[FLAG] [char](5) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_service_order_hdr_temp1] PRIMARY KEY CLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- SERVICE_ORDER_DET_DUP (Backup service order details)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SERVICE_ORDER_DET_DUP](
	[SERNO_DELL] [varchar](12) NULL,
	[PART_NO] [varchar](50) NULL,
	[QUANTITY] [varchar](15) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[UNIQUE_ID] [varchar](10) NULL,
	[PART_STATUS] [varchar](10) NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_SERVICE_ORDER_DET_DUP] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- TOOL_KIT_DUP (Backup toolkit details)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TOOL_KIT_DUP](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[KIT_CODE] [varchar](10) NULL,
	[APP_PASSWORD] [varchar](10) NULL,
	[INST_PASSWORD] [varchar](10) NULL,
	[IMEI_NO] [varchar](50) NULL,
	[ENGINEER_ID] [varchar](15) NULL,
	[FLAG] [char](5) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_TOOL_KIT_DUP] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- TOOLKIT_TRANSACTION_DUP (Backup toolkit transactions)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TOOLKIT_TRANSACTION_DUP](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TOOLKIT_ID] [bigint] NULL,
	[TOOLKIT_NAME_ID] [int] NULL,
	[ENGINEER_ID] [varchar](15) NULL,
	[ISSUER_ID] [varchar](15) NULL,
	[QUANTITY] [int] NULL,
	[STATUS] [varchar](20) NULL,
	[REMARKS] [varchar](20) NULL,
	[ADDITIONAL_REMARKS] [varchar](200) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_TOOLKIT_TRANSACTION_DUP] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- sample (Sample/test table)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sample](
	[drimage] [image] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- =====================================================
-- abcd (Legacy call table data)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[abcd](
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
	[CONTACT_NO] [varchar](15) NULL,
	[ADDRESS] [varchar](256) NULL,
	[ALT_CNTNO] [varchar](15) NULL,
	[DISPATCH_DATE] [datetime] NULL,
	[CUSTETA_DATE] [datetime] NULL,
	[PARTETA_DATE] [datetime] NULL,
	[TECH_SUPNAME] [varchar](50) NULL,
	[DSP] [varchar](100) NULL,
	[PRB_DESC] [varchar](250) NULL,
	[LONG_DESC] [varchar](4000) NULL,
	[REASON_CODE] [varchar](15) NULL,
	[ACTIVITY] [varchar](100) NULL,
	[ONSITE_DT] [datetime] NULL,
	[CMPLTD_DT] [datetime] NULL,
	[FLAG] [char](5) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_abcd] PRIMARY KEY CLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
