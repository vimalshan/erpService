-- =====================================================
-- Communication Microservice Database Schema
-- Tables for messaging and knowledge base management
-- =====================================================

USE [DELL_RTU_COMMUNICATION]
GO

-- =====================================================
-- MESSAGE_CORNER (User messages/notifications)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MESSAGE_CORNER](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CREATOR] [varchar](15) NULL,
	[LOCATION_TYPE] [varchar](10) NULL,
	[BRANCH] [varchar](50) NULL,
	[RECEIVER_TYPE] [varchar](10) NULL,
	[RECEIVER] [varchar](15) NULL,
	[MESSGE_EFFECTIVE_DATE] [datetime] NULL,
	[MESSAGE_EXPITY_DATE] [datetime] NULL,
	[SUBJECT] [varchar](200) NULL,
	[MESSAGE] [varchar](2000) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[ISVALID] [bit] NULL,
	[ISREADED] [bit] NULL,
	CONSTRAINT [PK_MESSAGE_CORNER] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- MSG_CORNER (Alternative messages table)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MSG_CORNER](
	[MSG_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MSGSUBJECT] [varchar](400) NULL,
	[EXPIRED_DATE] [datetime] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](50) NULL,
	[ISVALID] [bit] NULL,
	CONSTRAINT [PK_MSG_CORNER] PRIMARY KEY CLUSTERED 
	(
		[MSG_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- KNOWLEDGE_BASE (Knowledge articles and documentation)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[KNOWLEDGE_BASE](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CREATOR] [varchar](15) NULL,
	[RECEIVER_TYPE] [varchar](10) NULL,
	[KN_BASE_EFFECTIVE_DATE] [datetime] NULL,
	[KN_BASE_EXPITY_DATE] [datetime] NULL,
	[TITLE] [varchar](100) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[ISVALID] [bit] NULL,
	[ISREADED] [bit] NULL,
	[FILENAME] [varchar](500) NULL,
	[KN_BASE_ID] [int] NULL,
	CONSTRAINT [PK_KNOWLEDGE_BASE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- MAILID_MASTER (Email configuration and contacts)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MAILID_MASTER](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BRANCH] [varchar](50) NULL,
	[TYPE] [varchar](50) NULL,
	[NAME] [varchar](50) NULL,
	[MAILID] [varchar](200) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](50) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](50) NULL,
	[ISVALID] [bit] NULL,
	CONSTRAINT [PK_MAILID_MASTER] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- SMTP_SERVER (SMTP configuration)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SMTP_SERVER](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERVER_IP] [varchar](25) NULL,
	[PORT_NO] [int] NULL,
	CONSTRAINT [PK_SMTP_SERVER] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
