-- ==========================================
-- MODULE: LOV (List of Values)
-- Component: Tables
-- Description: Master tables for list of values and lookup data
-- Database: ADMINDB
-- ==========================================

USE [ADMINDB];
GO

-- Table: LOV_TYPE
-- Purpose: Defines types of LOV entries
CREATE TABLE [LOV_TYPE] (
    [LOV_TYPE_ID] BIGINT NULL,
    [LOV_TYPE_NAME] VARCHAR(30) NULL,
    CONSTRAINT [PK_LOV_TYPE] PRIMARY KEY ([LOV_TYPE_ID])
);
GO

-- Table: LOV_MASTER
-- Purpose: Master list of values for various dropdowns and lookups
CREATE TABLE [LOV_MASTER] (
    [LOV_ID] BIGINT NOT NULL,
    [LOV_TYPE_ID] BIGINT NOT NULL,
    [LOV_NAME] VARCHAR(30) NOT NULL,
    [LOV_UPDATED_BY] BIGINT NOT NULL,
    [LOV_UPDATED_ON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_LOV_MASTER] PRIMARY KEY ([LOV_ID]),
    CONSTRAINT [FK_LOV_MASTER_TYPE] FOREIGN KEY ([LOV_TYPE_ID]) REFERENCES [LOV_TYPE]([LOV_TYPE_ID])
);
GO

-- Table: ITEMDATA
-- Purpose: Catalog of items with category, pricing, and UOM information
CREATE TABLE [ITEMDATA] (
    [CATNAME] VARCHAR(40) NULL,
    [ITEMNAME] VARCHAR(60) NULL,
    [MAKE] VARCHAR(30) NULL,
    [UOM] VARCHAR(20) NULL,
    [PRICE] INT NULL
);
GO

-- Create indexes for performance
CREATE INDEX [IDX_LOV_MASTER_TYPEID] ON [LOV_MASTER]([LOV_TYPE_ID]);
GO

-- ==========================================
-- END OF LOV TABLES
-- ==========================================
