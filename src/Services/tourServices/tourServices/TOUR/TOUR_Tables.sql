-- ==========================================
-- Module: TOUR
-- Description: Tour Package & Registration Module
-- Database: TOURDB
-- Created: March 9, 2026
-- NOTE: Missing tables TOUR_PACKAGE and TOUR_REGISTRATION have been added
-- ==========================================

USE TOURDB;
GO

-- Table: TOUR_PACKAGE - Tour Package Master (MISSING - Added)
-- This table was referenced in procedures but not defined in original TOURDB.sql
CREATE TABLE [TOUR_PACKAGE] (
    [TOUR_ID] BIGINT NOT NULL IDENTITY(1,1)  -- Tour Package ID,
    [TOUR_NAME] VARCHAR(200) NOT NULL  -- Tour Package Name,
    [DESTINATION] VARCHAR(100) NOT NULL  -- Tour Destination,
    [START_DATE] DATE NOT NULL  -- Tour Start Date,
    [END_DATE] DATE NOT NULL  -- Tour End Date,
    [TOUR_PACKAGE_COST] DECIMAL(19,0) NOT NULL  -- Total Package Cost,
    [MAX_PARTICIPANTS] INT NOT NULL  -- Maximum Participants Allowed,
    [TOUR_STATUS] CHAR(1) NOT NULL  -- Tour Status (P-Planning, A-Active, C-Completed, X-Cancelled),
    [CREATED_BY] BIGINT NOT NULL  -- Created By Employee ID,
    [CREATED_ON] DATETIME2(3) NOT NULL  -- Created On,
    [MODIFIED_BY] BIGINT NULL  -- Last Modified By,
    [MODIFIED_ON] DATETIME2(3) NULL  -- Last Modified On,
    CONSTRAINT [PK_TOUR_PACKAGE] PRIMARY KEY ([TOUR_ID])
);

-- Table: TOUR_REGISTRATION - Tour Participant Registration (MISSING - Added)
-- This table was referenced in procedures but not defined in original TOURDB.sql
CREATE TABLE [TOUR_REGISTRATION] (
    [REGISTRATION_ID] BIGINT NOT NULL IDENTITY(1,1)  -- Registration ID,
    [TOUR_ID] BIGINT NOT NULL  -- Tour Package ID,
    [PARTICIPANT_ID] BIGINT NOT NULL  -- Participant Employee ID,
    [REGISTRATION_DATE] DATE NOT NULL  -- Registration Date,
    [REGISTRATION_STATUS] CHAR(1) NOT NULL  -- Status (A-Active, C-Cancelled, W-Waitlist),
    [CREATED_BY] BIGINT NOT NULL  -- Created By Employee ID,
    [CREATED_ON] DATETIME2(3) NOT NULL  -- Created On,
    [MODIFIED_BY] BIGINT NULL  -- Last Modified By,
    [MODIFIED_ON] DATETIME2(3) NULL  -- Last Modified On,
    CONSTRAINT [PK_TOUR_REGISTRATION] PRIMARY KEY ([REGISTRATION_ID]),
    CONSTRAINT [FK_TOUR_REG_PACKAGE] FOREIGN KEY ([TOUR_ID]) REFERENCES [TOUR_PACKAGE]([TOUR_ID])
);

PRINT 'TOUR Module - Tables created successfully.';
GO
