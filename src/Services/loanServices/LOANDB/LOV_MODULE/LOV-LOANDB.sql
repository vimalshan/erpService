-- ==========================================
-- Module: LOV
-- Database: LOANDB
-- Description: List of Values and Lookup Tables
-- ==========================================

USE [LOANDB];
GO

-- Table: LOV_TYPEMAST - LOV Type Master
CREATE TABLE [LOV_TYPEMAST] (
    [LOV_TYPEID] INT NOT NULL  -- LOV Type ID,
    [LOV_TYPENAME] VARCHAR(100) NOT NULL  -- LOV Type Name,
    [LOV_CATEGORY] CHAR(1) NOT NULL  -- LOV Category F - Fixed ; V - Variable,
    [LOV_ORGID] INT NOT NULL  -- Organization ID,
    CONSTRAINT [PK_LOV_TYPEMAST] PRIMARY KEY ([LOV_TYPEID])
);
GO

-- Table: LOV_MASTER - LOV Master
CREATE TABLE [LOV_MASTER] (
    [LOV_ID] BIGINT NOT NULL  -- LOV ID,
    [LOV_TYPEID] INT NOT NULL  -- LOV Type ID,
    [LOV_NAME] VARCHAR(65) NOT NULL  -- LOV Name,
    [LOV_CREATEDON] DATETIME2(3) NOT NULL  -- Created On,
    [LOV_CREATEDBY] BIGINT NOT NULL  -- Created By,
    [LOV_UPDATEDBY] BIGINT NOT NULL  -- Updated By,
    [LOV_UPDATEDON] DATETIME2(3) NOT NULL  -- Updated On,
    CONSTRAINT [PK_LOV_MASTER] PRIMARY KEY ([LOV_ID])
);
GO

-- Table: PROGRAMLOV_MAST - Program LOV Master
CREATE TABLE [PROGRAMLOV_MAST] (
    [PRLOV_TYPECODE] VARCHAR(20) NOT NULL  -- Program LOV Type Code,
    [PRLOV_CODE] VARCHAR(5) NOT NULL  -- Program LOV Code,
    [PRLOV_NAME] VARCHAR(200) NOT NULL  -- Program LOV Name,
    CONSTRAINT [PK_PROGRAMLOV_MAST] PRIMARY KEY ([PRLOV_CODE], [PRLOV_TYPECODE])
);
GO

-- Indexes on Foreign Key Columns
CREATE INDEX [IDX_LOV_MASTER_LOV_TYPEID] ON [LOV_MASTER]([LOV_TYPEID]);
GO
