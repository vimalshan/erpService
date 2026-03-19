-- =====================================================
-- Toolkit Management Microservice Database Schema
-- Tables for toolkit and equipment tracking
-- =====================================================

USE [DELL_RTU_TOOLKIT_MGMT]
GO

-- =====================================================
-- TOOLKIT_NAME_MASTER
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TOOLKIT_NAME_MASTER](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TOOLKIT_NAME] [varchar](40) NULL,
	[PRICE] [money] NULL,
	[IS_SCRAPABLE] [bit] NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_TOOLKIT_NAME_MASTER] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- TOOLKIT_MASTER (Main toolkit inventory)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TOOLKIT_MASTER](
	[TOOLKIT_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DC_NUMBER] [varchar](20) NULL,
	[DC_NO] [int] NULL,
	[BRANCH] [varchar](50) NULL,
	[SLC_ID] [varchar](15) NULL,
	[TOOLKIT_NAME] [varchar](40) NULL,
	[QUANTITY] [int] NULL,
	[REMARKS] [varchar](20) NULL,
	[STATUS] [varchar](20) NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_TOOLKIT_MASTER] PRIMARY KEY CLUSTERED 
	(
		[TOOLKIT_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- TOOLKIT_TRANSACTION (Toolkit allocation/movement)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TOOLKIT_TRANSACTION](
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
	CONSTRAINT [PK_TOOLKIT_TRANSACTION] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- TOOL_KIT_DETAILS (Kit details/configuration)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TOOL_KIT_DETAILS](
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
	CONSTRAINT [PK_TOOL_KIT_DETAILS] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
