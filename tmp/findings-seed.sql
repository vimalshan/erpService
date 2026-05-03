SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO
USE [ERPFindingsDB];
GO

-- Stub tables (Companies, Sites) used by Dapper repositories
IF OBJECT_ID('dbo.Companies','U') IS NULL
BEGIN
    CREATE TABLE dbo.Companies (
        CompanyId   INT IDENTITY(1,1) PRIMARY KEY,
        CompanyName NVARCHAR(200) NOT NULL,
        Industry    NVARCHAR(100) NULL,
        Status      NVARCHAR(50)  NULL
    );
END;
IF OBJECT_ID('dbo.Sites','U') IS NULL
BEGIN
    CREATE TABLE dbo.Sites (
        SiteId    INT IDENTITY(1,1) PRIMARY KEY,
        SiteName  NVARCHAR(200) NOT NULL,
        CompanyId INT           NOT NULL,
        Location  NVARCHAR(200) NULL,
        Status    NVARCHAR(50)  NULL
    );
END;
GO

DELETE FROM dbo.Sites;
DBCC CHECKIDENT('dbo.Sites', RESEED, 0);
DELETE FROM dbo.Companies;
DBCC CHECKIDENT('dbo.Companies', RESEED, 0);
DELETE FROM dbo.FindingResponses;
DBCC CHECKIDENT('dbo.FindingResponses', RESEED, 0);
DELETE FROM dbo.FindingClauses;
DBCC CHECKIDENT('dbo.FindingClauses', RESEED, 0);
DELETE FROM dbo.FindingFocusAreas;
DBCC CHECKIDENT('dbo.FindingFocusAreas', RESEED, 0);
DELETE FROM dbo.Findings;
DBCC CHECKIDENT('dbo.Findings', RESEED, 0);
DELETE FROM dbo.FindingCategories;
DBCC CHECKIDENT('dbo.FindingCategories', RESEED, 0);
DELETE FROM dbo.FindingStatuses;
DBCC CHECKIDENT('dbo.FindingStatuses', RESEED, 0);
GO

INSERT INTO dbo.Companies (CompanyName, Industry, Status) VALUES
 (N'Acme Corp',       N'Manufacturing', N'Active'),
 (N'Globex Industries', N'Energy',      N'Active'),
 (N'Initech Ltd',     N'Technology',    N'Active');

INSERT INTO dbo.Sites (SiteName, CompanyId, Location, Status) VALUES
 (N'Acme HQ',          1, N'New York',  N'Active'),
 (N'Acme Plant 1',     1, N'Detroit',   N'Active'),
 (N'Globex Refinery',  2, N'Houston',   N'Active'),
 (N'Initech Office',   3, N'Austin',    N'Active');
GO

-- Master data: FindingStatuses
INSERT INTO dbo.FindingStatuses
   (StatusName, StatusCode, [Description], IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy, Color, DisplayOrder, IsClosedStatus)
VALUES
   (N'Open',        N'OPEN',  N'Newly identified finding',         1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, N'#f44336', 1, 0),
   (N'In Progress', N'INPRG', N'Finding is being addressed',       1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, N'#ff9800', 2, 0),
   (N'Closed',      N'CLSD',  N'Finding has been closed',          1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, N'#4caf50', 3, 1),
   (N'Verified',    N'VRFD',  N'Closure verified',                 1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, N'#2196f3', 4, 1),
   (N'Disputed',    N'DSPT',  N'Finding is under dispute',         1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, N'#9c27b0', 5, 0);
GO

-- Master data: FindingCategories
INSERT INTO dbo.FindingCategories
   (CategoryName, CategoryCode, [Description], IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy, ParentCategoryId, Color, DisplayOrder)
VALUES
   (N'Major',         N'MAJ',  N'Major non-conformance',     1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, NULL, N'#d32f2f', 1),
   (N'Minor',         N'MIN',  N'Minor non-conformance',     1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, NULL, N'#fbc02d', 2),
   (N'Critical',      N'CRT',  N'Critical non-conformance',  1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, NULL, N'#b71c1c', 3),
   (N'Observation',   N'OBS',  N'Observation only',          1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, NULL, N'#0288d1', 4),
   (N'Opportunity',   N'OFI',  N'Opportunity for improvement', 1, SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, NULL, N'#388e3c', 5);
GO

-- Sample Findings
INSERT INTO dbo.Findings
   (FindingNumber, AuditId, SiteId, Title, [Description], FindingType, Severity,
    FindingStatusId, FindingCategoryId, IdentifiedDate, DueDate, ClosedDate, IsActive,
    CreatedDate, ModifiedDate, CreatedBy, ModifiedBy, IdentifiedBy, AssignedTo,
    Evidence, RootCause, CorrectiveAction, PreventiveAction, VerificationMethod,
    CompletionDate, VerificationDate, VerifiedBy)
VALUES
   (N'FND-2025-0001', 1001, 1, N'Missing PPE in production area',
    N'Operators observed without required PPE in zone B', N'Safety', N'High',
    1, 1, DATEADD(DAY,-30,SYSUTCDATETIME()), DATEADD(DAY,15,SYSUTCDATETIME()), NULL, 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, 10, 20,
    N'Photographs and shift logs', N'Inadequate training', N'Re-train operators', N'Monthly audits', N'Inspection',
    NULL, NULL, NULL),
   (N'FND-2025-0002', 1001, 2, N'Calibration records incomplete',
    N'Calibration records for line-3 instruments missing for last quarter', N'Quality', N'Medium',
    2, 2, DATEADD(DAY,-25,SYSUTCDATETIME()), DATEADD(DAY,20,SYSUTCDATETIME()), NULL, 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, 11, 21,
    N'Calibration logbook', N'Process gap', N'Update SOP', N'Quarterly review', N'Document review',
    NULL, NULL, NULL),
   (N'FND-2025-0003', 1002, 3, N'Spill kit not accessible',
    N'Spill kits located behind locked door in refinery', N'Environment', N'High',
    1, 3, DATEADD(DAY,-15,SYSUTCDATETIME()), DATEADD(DAY,10,SYSUTCDATETIME()), NULL, 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, 12, 22,
    N'Site walk-down notes', N'Storage policy', N'Relocate kits', N'Layout review', N'Inspection',
    NULL, NULL, NULL),
   (N'FND-2025-0004', 1002, 3, N'Permit-to-work missing signatures',
    N'Three permits issued without supervisor sign-off', N'Process', N'Medium',
    3, 2, DATEADD(DAY,-60,SYSUTCDATETIME()), DATEADD(DAY,-30,SYSUTCDATETIME()), DATEADD(DAY,-5,SYSUTCDATETIME()), 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, 13, 23,
    N'Permit register', N'Workflow gap', N'Enforce sign-off', N'System lock', N'Review',
    DATEADD(DAY,-7,SYSUTCDATETIME()), NULL, NULL),
   (N'FND-2025-0005', 1003, 4, N'Document version mismatch',
    N'Older revision of QMS document in circulation', N'Documentation', N'Low',
    4, 4, DATEADD(DAY,-90,SYSUTCDATETIME()), DATEADD(DAY,-60,SYSUTCDATETIME()), DATEADD(DAY,-40,SYSUTCDATETIME()), 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, 14, 24,
    N'Document control log', N'Distribution list outdated', N'Update list', N'Annual review', N'Document review',
    DATEADD(DAY,-45,SYSUTCDATETIME()), DATEADD(DAY,-30,SYSUTCDATETIME()), 99),
   (N'FND-2025-0006', 1003, 4, N'Training records missing',
    N'New hires lack induction training records', N'HR', N'Medium',
    1, 2, DATEADD(DAY,-5,SYSUTCDATETIME()), DATEADD(DAY,30,SYSUTCDATETIME()), NULL, 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, 15, 25,
    N'HR system extract', N'Process gap', N'Backfill records', N'Onboarding flow', N'Audit',
    NULL, NULL, NULL),
   (N'FND-2025-0007', 1001, 1, N'Emergency exit blocked',
    N'Pallets stored in front of emergency exit B', N'Safety', N'Critical',
    2, 3, DATEADD(DAY,-2,SYSUTCDATETIME()), DATEADD(DAY,3,SYSUTCDATETIME()), NULL, 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, 10, 20,
    N'Photo evidence', N'Poor housekeeping', N'Clear immediately', N'Daily checks', N'Inspection',
    NULL, NULL, NULL),
   (N'FND-2025-0008', 1002, 3, N'Suggestion to improve waste segregation',
    N'Color-coded bins recommended', N'Environment', N'Low',
    1, 5, DATEADD(DAY,-1,SYSUTCDATETIME()), DATEADD(DAY,90,SYSUTCDATETIME()), NULL, 1,
    SYSUTCDATETIME(), SYSUTCDATETIME(), 1, 1, 12, 22,
    N'Auditor notes', NULL, N'Implement segregation', NULL, N'Inspection',
    NULL, NULL, NULL);
GO

-- A couple of FindingResponses
INSERT INTO dbo.FindingResponses
   (FindingId, ResponseText, ResponseType, ResponseDate, RespondedBy, IsSubmittedToDNV, SubmissionDate,
    IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy, AttachmentPath, [Status], ReviewComments,
    ReviewedBy, ReviewDate)
VALUES
   (2, N'Updated calibration logs and trained team', N'CorrectiveAction', SYSUTCDATETIME(), 21, 0, NULL,
    1, SYSUTCDATETIME(), SYSUTCDATETIME(), 21, 21, NULL, N'Submitted', NULL, NULL, NULL),
   (4, N'Permit workflow updated and rolled out', N'ClosureEvidence', DATEADD(DAY,-6,SYSUTCDATETIME()), 23, 1, DATEADD(DAY,-6,SYSUTCDATETIME()),
    1, SYSUTCDATETIME(), SYSUTCDATETIME(), 23, 23, NULL, N'Approved', N'Verified onsite', 99, DATEADD(DAY,-5,SYSUTCDATETIME()));
GO

PRINT 'Findings seed complete.';
GO
