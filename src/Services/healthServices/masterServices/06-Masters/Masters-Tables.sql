-- ==========================================
-- Module: Masters
-- Purpose: Master Data & LOV Management
-- Generated: 2026-03-09
-- ==========================================

USE HEALTHDB;
GO

-- =====================================================
-- Table: LOV_TYPEMASTER
-- Description: List of Values Type Master
-- =====================================================
IF OBJECT_ID('dbo.LOV_TYPEMASTER', 'U') IS NOT NULL
    DROP TABLE dbo.LOV_TYPEMASTER;
GO

CREATE TABLE [dbo.LOV_TYPEMASTER] (
    [LOV_TYPECODE] CHAR(3) NOT NULL,
    [LOV_TYPENAME] VARCHAR(50) NULL,
    CONSTRAINT [PK_LOV_TYPEMASTER] PRIMARY KEY ([LOV_TYPECODE])
);

-- =====================================================
-- Table: LOV_MASTER
-- Description: List of Values Master
-- =====================================================
IF OBJECT_ID('dbo.LOV_MASTER', 'U') IS NOT NULL
    DROP TABLE dbo.LOV_MASTER;
GO

CREATE TABLE [dbo.LOV_MASTER] (
    [LOV_TYPE] CHAR(3) NULL,
    [LOV_ID] BIGINT NOT NULL,
    [LOV_NAME] VARCHAR(2000) NULL,
    CONSTRAINT [PK_LOV_MASTER] PRIMARY KEY ([LOV_ID])
);

-- =====================================================
-- Indexes
-- =====================================================
CREATE INDEX [IDX_LOV_MASTER_LOV_TYPE] ON [dbo.LOV_MASTER]([LOV_TYPE]);

PRINT 'Masters: Table creation completed successfully.';
GO
