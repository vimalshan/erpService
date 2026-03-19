-- =====================================================
-- Service Forms and Checkpoints
-- RC17, RC52, RC53 and supporting tables
-- =====================================================

USE [DELL_RTU_SERVICE_ORDERS]
GO

-- =====================================================
-- RC17 (Initial Service Checkpoint - Parts Receipt)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RC17](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NOT NULL,
	[DATE_TIME] [datetime] NULL,
	[REMARKS] [varchar](100) NULL,
	[FLAG] [char](10) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[MOBILE_SUBMITION_DATE] [datetime] NULL,
	[ISONLINE_MODE] [bit] NULL,
	[MobileIMEI] [varchar](25) NULL,
	CONSTRAINT [PK_RC17] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [UX_RC17_SERNO] UNIQUE NONCLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- RC52 (Audit/Quality Check Form)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RC52](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NOT NULL,
	[DATE_TIME] [datetime] NULL,
	[REMARKS] [varchar](100) NULL,
	[FLAG] [char](10) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[MOBILE_SUBMITION_DATE] [datetime] NULL,
	[ISONLINE_MODE] [bit] NULL,
	[BRANCH] [varchar](50) NULL,
	[CUSTETA_DATE] [datetime] NULL,
	[ENGINEER_ID] [varchar](15) NULL,
	[ENGINEER_NAME] [varchar](50) NULL,
	[RC_FINDINGS] [varchar](500) NULL,
	[RC_CUSTOMER_VOICE] [varchar](500) NULL,
	[RC_AUDIT_RESULT] [varchar](10) NULL,
	[RC_CORRECTIVE_ACTION] [varchar](500) NULL,
	[RC_AUDIT_DATE] [datetime] NULL,
	[HO_FINDINGS] [varchar](500) NULL,
	[HO_AUDIT_RESULT] [varchar](10) NULL,
	[HO_CORRECTIVE_ACTION] [varchar](500) NULL,
	[HO_AUDIT_DATE] [datetime] NULL,
	[AUDITOR_NAME] [varchar](50) NULL,
	[STATUS] [varchar](15) NULL,
	[RC_AUDIT_BY] [varchar](15) NULL,
	[MobileIMEI] [varchar](25) NULL,
	CONSTRAINT [PK_RC52] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [UX_RC52_SERNO] UNIQUE NONCLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- RC53 (Service Completion Form)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RC53](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NOT NULL,
	[RESULT_CODE] [varchar](50) NULL,
	[COMPL_DATE] [datetime] NULL,
	[ESUR_EDU] [char](5) NULL,
	[PRTS_INV] [varchar](25) NULL,
	[PRTS_CLTD] [char](5) NULL,
	[COLD_BOOT_DONE] [char](5) NULL,
	[POH] [char](5) NULL,
	[DTS_NAME] [varchar](25) NULL,
	[CASE_ID] [varchar](15) NULL,
	[CUST_SAT] [char](5) NULL,
	[CUST_SAT_BY_ENG] [varchar](5) NULL,
	[CUST_RECOMENDATION] [varchar](5) NULL,
	[ACTIVITY_DONE] [varchar](500) NULL,
	[ACTIVITY_CARRIER_OUTLIST] [varchar](500) NULL,
	[FLAG] [char](10) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[MOBILE_SUBMITION_DATE] [datetime] NULL,
	[ISONLINE_MODE] [bit] NULL,
	[START_PLACE] [varchar](3) NULL,
	[END_PLACE] [varchar](3) NULL,
	[DISTANCE] [varchar](3) NULL,
	[MobileIMEI] [varchar](25) NULL,
	[ADDITION_INFO] [varchar](200) NULL,
	CONSTRAINT [PK_RC53] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [UX_RC53_SERNO] UNIQUE NONCLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- RC95 (Equipment Return Form)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RC95](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NOT NULL,
	[REMARKS] [varchar](100) NULL,
	[FLAG] [char](10) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[ISVALID] [bit] NULL,
	[MOBILE_SUBMITION_DATE] [datetime] NULL,
	[MobileIMEI] [varchar](25) NULL,
	CONSTRAINT [PK_RC95] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [UX_RC95_SERNO] UNIQUE NONCLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- RC96 (Final Verification Form)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RC96](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NOT NULL,
	[REMARKS] [varchar](100) NULL,
	[FLAG] [char](10) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[ISVALID] [bit] NULL,
	[MOBILE_SUBMITION_DATE] [datetime] NULL,
	[MobileIMEI] [varchar](25) NULL,
	CONSTRAINT [PK_RC96] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [UX_RC96_SERNO] UNIQUE NONCLUSTERED 
	(
		[SERNO_DELL] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- PARTS_USED (Parts consumed during service)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PARTS_USED](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NULL,
	[SNO] [int] NULL,
	[PART_CODE] [varchar](10) NULL,
	[QUANTITY] [int] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_PARTS_USED] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- PARTS_COLLECTED (Parts collected from customer)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PARTS_COLLECTED](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NULL,
	[SNO] [int] NULL,
	[PART_NO] [varchar](25) NULL,
	[QUANTITY] [varchar](10) NULL,
	[REASON] [varchar](150) NULL,
	[TEN_DATE] [datetime] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_PARTS_COLLECTED] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- DOA_PARTS (Dead On Arrival parts - defective at arrival)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DOA_PARTS](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NULL,
	[SNO] [int] NULL,
	[PPID] [varchar](50) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_DOA_PARTS] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- ACTIVITY_HISTORY (Audit trail of service progress)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ACTIVITY_HISTORY](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NULL,
	[STATUS] [varchar](50) NULL,
	[RC] [varchar](10) NULL,
	[CUSTETA_DATE] [datetime] NULL,
	[ONSITE_DATE] [datetime] NULL,
	[COMPLETION_DATE] [datetime] NULL,
	[UPDATED_DATE] [datetime] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_ACTIVITY_HISTORY] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
