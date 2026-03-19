-- =====================================================
-- Scheduling Microservice Database Schema
-- Tables for appointment scheduling and slot management
-- =====================================================

USE [DELL_RTU_SCHEDULING]
GO

-- =====================================================
-- SCHEDULE_SLOT (Available time slots)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SCHEDULE_SLOT](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SLOT_DATE] [datetime] NOT NULL,
	[START_TIME] [time] NULL,
	[END_TIME] [time] NULL,
	[ENGINEER_ID] [varchar](15) NULL,
	[BRANCH] [varchar](50) NULL,
	[CAPACITY] [int] NULL,
	[AVAILABLE_SLOTS] [int] NULL,
	[STATUS] [varchar](20) NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_SCHEDULE_SLOT] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- SERVICE_APPOINTMENT (Customer service appointments)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SERVICE_APPOINTMENT](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SERNO_DELL] [varchar](12) NULL,
	[SLOT_ID] [bigint] NULL,
	[APPOINTMENT_DATE] [datetime] NULL,
	[START_TIME] [time] NULL,
	[END_TIME] [time] NULL,
	[ENGINEER_ID] [varchar](15) NULL,
	[CUSTOMER_NAME] [varchar](100) NULL,
	[CONTACT_NO] [varchar](50) NULL,
	[ADDRESS] [varchar](256) NULL,
	[STATUS] [varchar](20) NULL,
	[NOTES] [varchar](500) NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_SERVICE_APPOINTMENT] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- ENGINEER_SCHEDULE (Engineer availability calendar)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ENGINEER_SCHEDULE](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ENGINEER_ID] [varchar](15) NULL,
	[SCHEDULE_DATE] [datetime] NOT NULL,
	[WORKING_HOURS] [varchar](50) NULL,
	[AVAILABILITY_STATUS] [varchar](20) NULL,
	[ASSIGNMENTS_AVAILABLE] [int] NULL,
	[CURRENT_WORKLOAD] [int] NULL,
	[NOTES] [varchar](500) NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_ENGINEER_SCHEDULE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- SLOT_ASSIGNMENT (Assignment of appointments to slots)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SLOT_ASSIGNMENT](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[APPOINTMENT_ID] [bigint] NULL,
	[SLOT_ID] [bigint] NULL,
	[ENGINEER_ID] [varchar](15) NULL,
	[ASSIGNED_DATE] [datetime] NULL,
	[ASSIGNMENT_STATUS] [varchar](20) NULL,
	[DISPATCH_DATE] [datetime] NULL,
	[NOTES] [varchar](500) NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_SLOT_ASSIGNMENT] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

-- =====================================================
-- BLACKOUT_DATE (Non-working dates/holidays)
-- =====================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BLACKOUT_DATE](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BLACKOUT_DATE] [datetime] NOT NULL,
	[REASON] [varchar](200) NULL,
	[APPLICABLE_BRANCHES] [varchar](500) NULL,
	[APPLICABLE_ENGINEERS] [varchar](500) NULL,
	[ISVALID] [bit] NULL,
	[ENTERED_ON] [datetime] NULL,
	[ENTERED_BY] [varchar](15) NULL,
	[CHANGED_ON] [datetime] NULL,
	[CHANGED_BY] [varchar](15) NULL,
	CONSTRAINT [PK_BLACKOUT_DATE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
