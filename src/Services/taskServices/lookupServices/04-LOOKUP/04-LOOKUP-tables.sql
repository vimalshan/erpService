-- ==========================================
-- Module: LOOKUP & CONFIGURATION
-- Database: TASKDB
-- Purpose: Master Data & Lookup Tables
-- Tables for system-wide configuration, lookup values, and master data
-- ==========================================

USE [TASKDB];
GO

-- ==========================================
-- LOOKUP & CONFIGURATION TABLES
-- ==========================================

-- Table: LOV_TYPEMASTER
-- Purpose: List of Values - Type Master (categories)
CREATE TABLE [LOV_TYPEMASTER] (
    [LOV_TYPECODE] CHAR(3) NOT NULL,
    [LOV_TYPENAME] VARCHAR(50) NULL,
    CONSTRAINT [PK_LOV_TYPEMASTER] PRIMARY KEY ([LOV_TYPECODE])
);

-- Table: LOV_MASTER
-- Purpose: List of Values - Master data containing all LOV entries
CREATE TABLE [LOV_MASTER] (
    [LOV_TYPE] CHAR(3) NULL,
    [LOV_ID] BIGINT NOT NULL,
    [LOV_NAME] VARCHAR(200) NULL,
    CONSTRAINT [PK_LOV_MASTER] PRIMARY KEY ([LOV_ID])
);

-- Table: LOV_UNITMAP
-- Purpose: Mapping of LOVs to Units
CREATE TABLE [LOV_UNITMAP] (
    [LU_MAPID] DECIMAL(38) NOT NULL,
    [LU_LOVID] DECIMAL(38) NULL,
    [LU_UNITCODE] CHAR(3) NULL,
    [LU_FLAG] CHAR(1) NULL,
    CONSTRAINT [PK_LOV_UNITMAP] PRIMARY KEY ([LU_MAPID])
);

-- Table: LOV_PANELMAP
-- Purpose: Mapping of LOVs to Panels
CREATE TABLE [LOV_PANELMAP] (
    [LP_LOVID] DECIMAL(38) NULL,
    [LP_PANELID] DECIMAL(38) NULL,
    [LP_FLAG] CHAR(1) NULL
);

-- Table: PANEL_MAST
-- Purpose: Panel master configuration
CREATE TABLE [PANEL_MAST] (
    [PANEL_ID] DECIMAL(38) NOT NULL,
    [PANEL_NAME] VARCHAR(65) NULL,
    CONSTRAINT [PK_PANEL_MAST] PRIMARY KEY ([PANEL_ID])
);

-- Table: PROCESS_MASTER
-- Purpose: Process master configuration
CREATE TABLE [PROCESS_MASTER] (
    [PROCESS_ID] DECIMAL(38) NOT NULL,
    [PROCESS_NAME] VARCHAR(50) NULL,
    [PROCESS_LIVFLAG] CHAR(1) NULL,
    CONSTRAINT [PK_PROCESS_MASTER] PRIMARY KEY ([PROCESS_ID])
);

-- Table: UNIT_PROCESS_MAP
-- Purpose: Mapping of Units to Processes
CREATE TABLE [UNIT_PROCESS_MAP] (
    [UP_MAPID] DECIMAL(38) NOT NULL,
    [UP_UNIT_CODE] CHAR(3) NULL,
    [UP_PROCESS_ID] DECIMAL(38) NULL,
    CONSTRAINT [PK_UNIT_PROCESS_MAP] PRIMARY KEY ([UP_MAPID])
);

-- Table: UNITLOV_ACCESSMAST
-- Purpose: Unit LOV Access Master - controls access hierarchy
CREATE TABLE [UNITLOV_ACCESSMAST] (
    [UA_ACCESSMASTID] DECIMAL(38) NOT NULL,
    [UA_UNITLOVMAPID] DECIMAL(38) NULL,
    [UA_DEPARTMENTID] DECIMAL(38) NULL,
    [UA_PROCESSID] DECIMAL(38) NULL,
    CONSTRAINT [PK_UNITLOV_ACCESSMAST] PRIMARY KEY ([UA_ACCESSMASTID])
);

-- Table: UNITLOV_ACCESSDET
-- Purpose: Unit LOV Access Details - granular access control
CREATE TABLE [UNITLOV_ACCESSDET] (
    [UD_ACCESSDETID] DECIMAL(38) NOT NULL,
    [UD_ACCESSMASTID] DECIMAL(38) NULL,
    [UD_ACCESSTYPE] CHAR(2) NULL,
    [UD_EMPSYSID] VARCHAR(300) NULL,
    [UD_ESCDAYS] DECIMAL(38) NULL,
    [UD_EFF_DAT] VARCHAR(255) NULL,
    [UD_CLS_DAT] VARCHAR(255) NULL,
    [UD_UPDATEDBY] DECIMAL(38) NULL,
    [UD_UPDATEDON] DATETIME2(3) NULL,
    CONSTRAINT [PK_UNITLOV_ACCESSDET] PRIMARY KEY ([UD_ACCESSDETID])
);

-- ==========================================
-- END OF SCRIPT - LOOKUP MODULE TABLES
-- ==========================================
