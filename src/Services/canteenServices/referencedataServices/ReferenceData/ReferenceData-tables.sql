-- ==========================================
-- Module: ReferenceData
-- Table Scripts
-- ==========================================

-- Table: LOV_MASTER
CREATE TABLE [LOV_MASTER] (
    [LOV_TYPE] CHAR(3) NULL  -- LOV Type,
    [LOV_ID] CHAR(3) NOT NULL  -- LOV Code,
    [LOV_NAME] VARCHAR(200) NULL  -- LOV NAME,
    CONSTRAINT [PK_LOV_MASTER] PRIMARY KEY ([LOV_ID])
);

-- Table: LOV_TYPEMASTER
CREATE TABLE [LOV_TYPEMASTER] (
    [LOV_TYPECODE] CHAR(3) NOT NULL  -- LOV TYPE Code,
    [LOV_TYPENAME] VARCHAR(50) NULL  -- LOV TYPE NAME,
    CONSTRAINT [PK_LOV_TYPEMASTER] PRIMARY KEY ([LOV_TYPECODE])
);

-- Table: PATHTOSQLSERVER
CREATE TABLE [PATHTOSQLSERVER] (
    [COM_COD] CHAR(3) NULL  -- Company Code,
    [SERVER_NAME] VARCHAR(20) NULL  -- Name of the Server,
    [DATABASE_NAME] VARCHAR(20) NULL  -- Name of the Database,
    [USER_ID] VARCHAR(10) NULL  -- User ID,
    [DBPASSWORD] VARCHAR(10) NULL  -- Password
);
