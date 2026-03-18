-- ==========================================
-- Module: CLUB MEMBERSHIP MODULE
-- Description: Club and membership management system
-- Created: March 9, 2026
-- Database: SSCDB
-- ==========================================

USE SSCDB;
GO

-- ==========================================
-- CLUB_MASTER - Club Master (Table will be created as per requirement)
-- ==========================================
-- Note: This table structure is inferred from procedures
-- Adjust column definitions based on your actual requirements
IF OBJECT_ID('[CLUB_MASTER]', 'U') IS NOT NULL DROP TABLE [CLUB_MASTER];
GO
CREATE TABLE [CLUB_MASTER] (
    [CLUB_ID] BIGINT NOT NULL,
    [CLUB_NAME] VARCHAR(100) NOT NULL,
    [CLUB_STATUS] CHAR(1) NOT NULL DEFAULT 'A',  -- A=Active, I=Inactive
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [MODIFIED_BY] BIGINT NULL,
    [MODIFIED_ON] DATETIME2(3) NULL,
    CONSTRAINT [PK_CLUB_MASTER] PRIMARY KEY ([CLUB_ID])
);
GO

-- ==========================================
-- CLUB_MEMBERSHIP - Club Membership Details
-- ==========================================
IF OBJECT_ID('[CLUB_MEMBERSHIP]', 'U') IS NOT NULL DROP TABLE [CLUB_MEMBERSHIP];
GO
CREATE TABLE [CLUB_MEMBERSHIP] (
    [MEMBERSHIP_ID] BIGINT NOT NULL,
    [CLUB_ID] BIGINT NOT NULL,
    [MEMBER_ID] BIGINT NOT NULL,
    [JOIN_DATE] DATE NOT NULL,
    [MEMBERSHIP_FEE] DECIMAL(19,2) NULL,
    [MEMBERSHIP_STATUS] CHAR(1) NOT NULL DEFAULT 'A',  -- A=Active, I=Inactive
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [MODIFIED_BY] BIGINT NULL,
    [MODIFIED_ON] DATETIME2(3) NULL,
    CONSTRAINT [PK_CLUB_MEMBERSHIP] PRIMARY KEY ([MEMBERSHIP_ID]),
    CONSTRAINT [FK_CLUB_MEMBERSHIP_CLUB] FOREIGN KEY ([CLUB_ID]) REFERENCES [CLUB_MASTER]([CLUB_ID])
);
GO

-- ==========================================
-- CLUB_ACTIVITY - Club Activities
-- ==========================================
IF OBJECT_ID('[CLUB_ACTIVITY]', 'U') IS NOT NULL DROP TABLE [CLUB_ACTIVITY];
GO
CREATE TABLE [CLUB_ACTIVITY] (
    [ACTIVITY_ID] BIGINT NOT NULL,
    [CLUB_ID] BIGINT NOT NULL,
    [ACTIVITY_NAME] VARCHAR(100) NOT NULL,
    [ACTIVITY_DATE] DATE NOT NULL,
    [ACTIVITY_BUDGET] DECIMAL(19,2) NULL,
    [ORGANIZER_ID] BIGINT NOT NULL,
    [ACTIVITY_STATUS] CHAR(1) NOT NULL DEFAULT 'P',  -- P=Planned, O=Ongoing, C=Completed
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [MODIFIED_BY] BIGINT NULL,
    [MODIFIED_ON] DATETIME2(3) NULL,
    CONSTRAINT [PK_CLUB_ACTIVITY] PRIMARY KEY ([ACTIVITY_ID]),
    CONSTRAINT [FK_CLUB_ACTIVITY_CLUB] FOREIGN KEY ([CLUB_ID]) REFERENCES [CLUB_MASTER]([CLUB_ID])
);
GO

PRINT 'CLUB_MEMBERSHIP_MODULE Schema created successfully.';
GO
