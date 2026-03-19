-- =====================================================
-- Auth Microservice Database Schema
-- Tables for user authentication and login management
-- =====================================================

USE [DELL_RTU_AUTH]
GO

-- Create Users and Schema
CREATE USER [dellusr] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO

CREATE USER [testusr] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[testusr]
GO

CREATE SCHEMA [testusr] AUTHORIZATION [testusr]
GO

-- =====================================================
-- LOGIN_TYPE_MASTER
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LOGIN_TYPE_MASTER](
	[LOGIN_TYPE] [bigint] IDENTITY(0,1) NOT NULL,
	[LOGIN_TYPE_NAME] [varchar](50) NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_LOGIN_TYPE_MASTER] PRIMARY KEY CLUSTERED 
	(
		[LOGIN_TYPE] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- LOGIN_MASTER
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LOGIN_MASTER](
	[LOGIN_ID] [varchar](15) NOT NULL,
	[BRANCH] [varchar](50) NULL,
	[LOGIN_NAME] [varchar](50) NULL,
	[PASWORD] [varchar](10) NULL,
	[USER_TYPE] [char](5) NULL,
	[KIT_CODE] [varchar](10) NULL,
	[IMEI_NO] [varchar](50) NULL,
	[FLAG] [char](5) NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	[C_FLAG] [char](5) NULL,
	[MOBILE_NO] [varchar](10) NULL,
	CONSTRAINT [PK_LOGIN_MASTER] PRIMARY KEY CLUSTERED 
	(
		[LOGIN_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- LOGIN_ERROR
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LOGIN_ERROR](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LOGINID] [varchar](25) NULL,
	[ERROR] [varchar](25) NULL,
	[ENTERDATE] [datetime] NULL,
	CONSTRAINT [PK_LOGIN_ERROR] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
