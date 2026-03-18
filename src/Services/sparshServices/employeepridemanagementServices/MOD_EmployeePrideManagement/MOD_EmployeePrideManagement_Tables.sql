-- ============================================================================
-- Module: Employee Pride Management
-- Purpose: Manage employee pride moments, achievements, and celebrations
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

-- Set database context
USE [SPARSHDB];
GO

-- ============================================================================
-- TABLE: MOMENT_PRIDE
-- Description: Stores employee pride moments and achievement records
-- ============================================================================
IF OBJECT_ID('[dbo].[MOMENT_PRIDE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MOMENT_PRIDE];
GO

CREATE TABLE [dbo].[MOMENT_PRIDE] (
    [MOMENTPRIDE_ID]        DECIMAL(38) NOT NULL,       -- Pride Moment ID (Primary Key)
    [MOMENTPRIDE_TITLE]     VARCHAR(50) NOT NULL,       -- Pride Moment Title
    [MOMENTPRIDE_BODY]      NVARCHAR(MAX) NULL,         -- Complete Description/Body
    [MOMENTPRIDE_EMPSYSID]  DECIMAL(38) NOT NULL,       -- Employee System ID
    [MOMENTPRIDE_FOOTER]    VARCHAR(500) NOT NULL,      -- Footer/Additional Info
    [MOMENTPRIDE_LOCATION]  VARCHAR(100) NOT NULL,      -- Location of Pride Moment
    [MOMENTPRIDE_IMAGE]     VARCHAR(200) NOT NULL,      -- Image/Photo Path or URL
    [MOMENTPRIDE_MODIFIEDBY] BIGINT NOT NULL,           -- Modified By (Employee ID)
    [MOMENTPRIDE_MODIFIEDON] DATETIME2(3) NULL,         -- Modified Timestamp
    CONSTRAINT [PK_MOMENT_PRIDE] PRIMARY KEY ([MOMENTPRIDE_ID])
);

CREATE INDEX [IX_MOMENT_PRIDE_EMPSYSID] ON [dbo].[MOMENT_PRIDE]([MOMENTPRIDE_EMPSYSID]);
CREATE INDEX [IX_MOMENT_PRIDE_MODIFIEDON] ON [dbo].[MOMENT_PRIDE]([MOMENTPRIDE_MODIFIEDON]);
GO

PRINT 'Employee Pride Management Tables created successfully.';
GO
