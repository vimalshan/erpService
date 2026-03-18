-- ==========================================
-- ProxyModule
-- Database: SRFSPARSHDB
-- Module Purpose: Proxy User Rights Management
-- Created: March 09, 2026
-- ==========================================

USE SRFSPARSHDB;
GO

-- Drop table if exists
IF OBJECT_ID('[PROXY_RIGHTS]', 'U') IS NOT NULL DROP TABLE [PROXY_RIGHTS];
GO

-- ==========================================
-- Table: PROXY_RIGHTS - Proxy User Access Rights
-- Description: Manages proxy/delegate access rights where one user can act on behalf of another
-- ==========================================
CREATE TABLE [PROXY_RIGHTS] (
    [PROXY_ID] BIGINT PRIMARY KEY IDENTITY(1,1),
    [PROXY_USER_ID] BIGINT NOT NULL,
    [DELEGATED_USER_ID] BIGINT NOT NULL,
    [PROXY_START_DATE] DATE NOT NULL,
    [PROXY_END_DATE] DATE,
    [PROXY_TYPE] VARCHAR(50), -- APPROVAL, SUBMISSION, FULL, etc.
    [PROXY_STATUS] CHAR(1) DEFAULT 'A', -- A=Active, I=Inactive
    [SCOPE] VARCHAR(100), -- Department, Location, specific process, ALL
    [NOTES] NVARCHAR(MAX),
    [CREATED_BY] BIGINT NOT NULL,
    [CREATED_ON] DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    [UPDATED_BY] BIGINT,
    [UPDATED_ON] DATETIME2(3)
);
GO

-- Create Indexes
CREATE INDEX [IX_PROXY_RIGHTS_PROXY_USER_ID] ON [PROXY_RIGHTS]([PROXY_USER_ID]);
CREATE INDEX [IX_PROXY_RIGHTS_DELEGATED_USER_ID] ON [PROXY_RIGHTS]([DELEGATED_USER_ID]);
CREATE INDEX [IX_PROXY_RIGHTS_STATUS] ON [PROXY_RIGHTS]([PROXY_STATUS]);
CREATE INDEX [IX_PROXY_RIGHTS_DATES] ON [PROXY_RIGHTS]([PROXY_START_DATE], [PROXY_END_DATE]);
CREATE INDEX [IX_PROXY_RIGHTS_TYPE] ON [PROXY_RIGHTS]([PROXY_TYPE]);
GO

PRINT 'ProxyModule_Schema created successfully.';
GO
