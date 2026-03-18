-- ==========================================
-- TEAM Module - Table Scripts
-- Database: MYWORKDB
-- Module: TEAM
-- Description: Team Management and Organization Module
-- Created: March 9, 2026
-- ==========================================

USE MYWORKDB;
GO

-- =====================================================
-- TEAM Core Tables
-- =====================================================

-- Table: TEAM_MASTER - TEAM MASTER
CREATE TABLE [TEAM_MASTER] (
    [TEAM_ID] BIGINT NULL  -- TEAM ID,
    [TEAM_NAME] VARCHAR(50) NOT NULL  -- TEAM NAME,
    [TEAM_LASTMODIFIEDBY] BIGINT NOT NULL,
    [TEAM_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_TEAM_MASTER] PRIMARY KEY ([TEAM_ID])
);

-- Table: TEAM_EMPMAP - TEAM EMPLOYEE MAP
CREATE TABLE [TEAM_EMPMAP] (
    [TEAMEMP_ID] BIGINT NULL  -- Project Emp Map ID,
    [TEAMEMP_TEAMID] BIGINT NOT NULL  -- Team ID,
    [TEAMEMP_EMPSYSID] BIGINT NOT NULL  -- Employee System ID,
    [TEAMEMP_EFFDATE] DATETIME2(3) NOT NULL  -- Team Effective Date,
    [TEAMEMP_CLOSEDATE] DATETIME2(3) NULL  -- Team Close Date,
    [TEAMEMP_LASTMODIFIEDBY] BIGINT NOT NULL  -- Last Updated By,
    [TEAMEMP_LASTMODIFIEDON] DATETIME2(3) NOT NULL  -- Last Updated On,
    CONSTRAINT [PK_TEAM_EMPMAP] PRIMARY KEY ([TEAMEMP_ID])
);

-- Table: TEAM_UNITMAP - TEAM UNIT MAP
CREATE TABLE [TEAM_UNITMAP] (
    [TEAM_MAPID] BIGINT NOT NULL  -- Map ID,
    [TEAM_ID] BIGINT NOT NULL  -- TEAM_ID,
    [TEAM_UNITID] BIGINT NOT NULL  -- Team Unit ID,
    [TEAM_GRADECATEGORY] CHAR(1) NOT NULL  -- Grade Category,
    [TEAM_CADREID] BIGINT NULL  -- Team GradeCadre ID,
    [TEAM_LASTMODIFIEDBY] BIGINT NOT NULL,
    [TEAM_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_TEAM_UNITMAP] PRIMARY KEY ([TEAM_MAPID])
);

PRINT 'TEAM Module - Tables created successfully.';
GO
