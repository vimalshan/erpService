-- =====================================================
-- Damage Tracking Microservice Database Schema
-- Tables for tracking damage, issues, and exceptions
-- =====================================================

USE [DELL_RTU_DAMAGE_TRACKING]
GO

-- =====================================================
-- DAMAGE_UPDATES (Damage reports and updates)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DAMAGE_UPDATES](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NULL,
	[DTS_NAME] [varchar](50) NULL,
	[CASE_ID] [varchar](30) NULL,
	[ENGINEER_COMMENTS] [varchar](500) NULL,
	[ISONLINE_MODE] [bit] NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_DAMAGE_UPDATES] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- EXCEPTION_TABLE (System exceptions and errors)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EXCEPTION_TABLE](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NULL,
	[EXCEPTION_MESSAGE] [varchar](1000) NULL,
	[EXCEPTION_SOURCE] [varchar](200) NULL,
	[EXCEPTION_DATA] [varchar](1000) NULL,
	[INNER_EXCEPTION] [varchar](1000) NULL,
	[STACK_TARGET] [varchar](1000) NULL,
	[ISCORRECTED] [bit] NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_EXCEPTION_TABLE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- TEMPURARY (Temporary data storage for damage details)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TEMPURARY](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NULL,
	[UNIQUE_ID] [varchar](10) NULL,
	[PART_USAGE_TYPE] [varchar](40) NULL,
	[PP_ID] [varchar](30) NULL,
	[DEFECTIVE_PARTNO] [varchar](50) NULL,
	[FAILURE_REASON] [varchar](50) NULL,
	[FAILURE_REASON_OTHRES] [varchar](150) NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[ENTERED_ON] [datetime] NULL,
	CONSTRAINT [PK_TEMPURARY] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
