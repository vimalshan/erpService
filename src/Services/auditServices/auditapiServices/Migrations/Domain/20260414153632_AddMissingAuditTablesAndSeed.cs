using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditService.Migrations.Domain
{
    /// <inheritdoc />
    public partial class AddMissingAuditTablesAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Create missing tables (idempotent) ─────────────────────────────

            migrationBuilder.Sql(@"
IF OBJECT_ID('Audits', 'U') IS NULL
BEGIN
    CREATE TABLE [Audits] (
        [auditId]     INT IDENTITY(1,1) NOT NULL,
        [sites]       NVARCHAR(MAX)     NULL,
        [services]    NVARCHAR(MAX)     NULL,
        [companyId]   INT               NULL,
        [status]      NVARCHAR(50)      NULL,
        [startDate]   DATETIME2         NULL,
        [endDate]     DATETIME2         NULL,
        [leadAuditor] NVARCHAR(100)     NULL,
        [type]        NVARCHAR(50)      NULL,
        CONSTRAINT [PK_Audits] PRIMARY KEY ([auditId])
    );
END");

            migrationBuilder.Sql(@"
IF OBJECT_ID('AuditSites', 'U') IS NULL
BEGIN
    CREATE TABLE [AuditSites] (
        [AuditSiteId]   INT IDENTITY(1,1) NOT NULL,
        [AuditId]       INT               NOT NULL,
        [SiteId]        INT               NOT NULL,
        [IsActive]      BIT               NOT NULL DEFAULT 1,
        [CreatedDate]   DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate]  DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy]     INT               NULL,
        [ModifiedBy]    INT               NULL,
        [Status]        NVARCHAR(MAX)     NULL,
        [ScheduledDate] DATETIME2         NULL,
        [CompletedDate] DATETIME2         NULL,
        [Notes]         NVARCHAR(MAX)     NULL,
        CONSTRAINT [PK_AuditSites] PRIMARY KEY ([AuditSiteId]),
        CONSTRAINT [FK_AuditSites_Audits_AuditId] FOREIGN KEY ([AuditId]) REFERENCES [Audits]([auditId]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AuditSites_AuditId] ON [AuditSites]([AuditId]);
END");

            migrationBuilder.Sql(@"
IF OBJECT_ID('AuditServices', 'U') IS NULL
BEGIN
    CREATE TABLE [AuditServices] (
        [AuditServiceId] INT IDENTITY(1,1) NOT NULL,
        [AuditId]        INT               NOT NULL,
        [ServiceId]      INT               NOT NULL,
        [IsActive]       BIT               NOT NULL DEFAULT 1,
        [CreatedDate]    DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate]   DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy]      INT               NULL,
        [ModifiedBy]     INT               NULL,
        [Status]         NVARCHAR(MAX)     NULL,
        [StartDate]      DATETIME2         NULL,
        [EndDate]        DATETIME2         NULL,
        [Notes]          NVARCHAR(MAX)     NULL,
        CONSTRAINT [PK_AuditServices] PRIMARY KEY ([AuditServiceId]),
        CONSTRAINT [FK_AuditServices_Audits_AuditId] FOREIGN KEY ([AuditId]) REFERENCES [Audits]([auditId]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AuditServices_AuditId] ON [AuditServices]([AuditId]);
END");

            migrationBuilder.Sql(@"
IF OBJECT_ID('AuditTeamMembers', 'U') IS NULL
BEGIN
    CREATE TABLE [AuditTeamMembers] (
        [AuditTeamMemberId] INT IDENTITY(1,1) NOT NULL,
        [AuditId]           INT               NOT NULL,
        [UserId]            INT               NOT NULL,
        [Role]              NVARCHAR(MAX)     NOT NULL,
        [IsActive]          BIT               NOT NULL DEFAULT 1,
        [CreatedDate]       DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate]      DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy]         INT               NULL,
        [ModifiedBy]        INT               NULL,
        [AssignedDate]      DATETIME2         NULL,
        [StartDate]         DATETIME2         NULL,
        [EndDate]           DATETIME2         NULL,
        [Specialization]    NVARCHAR(MAX)     NULL,
        [Notes]             NVARCHAR(MAX)     NULL,
        CONSTRAINT [PK_AuditTeamMembers] PRIMARY KEY ([AuditTeamMemberId]),
        CONSTRAINT [FK_AuditTeamMembers_Audits_AuditId] FOREIGN KEY ([AuditId]) REFERENCES [Audits]([auditId]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AuditTeamMembers_AuditId] ON [AuditTeamMembers]([AuditId]);
END");

            migrationBuilder.Sql(@"
IF OBJECT_ID('AuditSiteAudits', 'U') IS NULL
BEGIN
    CREATE TABLE [AuditSiteAudits] (
        [AuditSiteAuditId]  INT IDENTITY(1,1) NOT NULL,
        [AuditId]           INT               NOT NULL,
        [SiteId]            INT               NOT NULL,
        [AuditTypeId]       INT               NOT NULL,
        [AuditNumber]       NVARCHAR(MAX)     NOT NULL,
        [ScheduledDate]     DATETIME2         NULL,
        [StartDate]         DATETIME2         NULL,
        [EndDate]           DATETIME2         NULL,
        [CompletedDate]     DATETIME2         NULL,
        [Status]            NVARCHAR(MAX)     NOT NULL DEFAULT 'Scheduled',
        [LeadAuditorId]     INT               NULL,
        [IsActive]          BIT               NOT NULL DEFAULT 1,
        [CreatedDate]       DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate]      DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy]         INT               NULL,
        [ModifiedBy]        INT               NULL,
        [Notes]             NVARCHAR(MAX)     NULL,
        [ReportPath]        NVARCHAR(MAX)     NULL,
        [CertificateIssued] BIT               NOT NULL DEFAULT 0,
        [CertificateNumber] NVARCHAR(MAX)     NULL,
        CONSTRAINT [PK_AuditSiteAudits] PRIMARY KEY ([AuditSiteAuditId]),
        CONSTRAINT [FK_AuditSiteAudits_AuditTypes_AuditTypeId] FOREIGN KEY ([AuditTypeId]) REFERENCES [AuditTypes]([AuditTypeId]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AuditSiteAudits_AuditTypeId] ON [AuditSiteAudits]([AuditTypeId]);
END");

            migrationBuilder.Sql(@"
IF OBJECT_ID('AuditSiteRepresentatives', 'U') IS NULL
BEGIN
    CREATE TABLE [AuditSiteRepresentatives] (
        [AuditSiteRepresentativeId] INT IDENTITY(1,1) NOT NULL,
        [AuditSiteAuditId]          INT               NOT NULL,
        [UserId]                    INT               NOT NULL,
        [Role]                      NVARCHAR(MAX)     NULL,
        [IsActive]                  BIT               NOT NULL DEFAULT 1,
        [CreatedDate]               DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate]              DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy]                 INT               NULL,
        [ModifiedBy]                INT               NULL,
        [ContactPhone]              NVARCHAR(MAX)     NULL,
        [ContactEmail]              NVARCHAR(MAX)     NULL,
        [Notes]                     NVARCHAR(MAX)     NULL,
        CONSTRAINT [PK_AuditSiteRepresentatives] PRIMARY KEY ([AuditSiteRepresentativeId]),
        CONSTRAINT [FK_AuditSiteRepresentatives_AuditSiteAudits_AuditSiteAuditId]
            FOREIGN KEY ([AuditSiteAuditId]) REFERENCES [AuditSiteAudits]([AuditSiteAuditId]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AuditSiteRepresentatives_AuditSiteAuditId] ON [AuditSiteRepresentatives]([AuditSiteAuditId]);
END");

            migrationBuilder.Sql(@"
IF OBJECT_ID('AuditSiteServices', 'U') IS NULL
BEGIN
    CREATE TABLE [AuditSiteServices] (
        [AuditSiteServiceId] INT IDENTITY(1,1) NOT NULL,
        [AuditSiteAuditId]   INT               NOT NULL,
        [ServiceId]          INT               NOT NULL,
        [IsActive]           BIT               NOT NULL DEFAULT 1,
        [CreatedDate]        DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate]       DATETIME2         NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy]          INT               NULL,
        [ModifiedBy]         INT               NULL,
        [Status]             NVARCHAR(MAX)     NULL,
        [StartDate]          DATETIME2         NULL,
        [EndDate]            DATETIME2         NULL,
        [Notes]              NVARCHAR(MAX)     NULL,
        [Cost]               DECIMAL(18,2)     NULL,
        [Currency]           NVARCHAR(MAX)     NULL,
        CONSTRAINT [PK_AuditSiteServices] PRIMARY KEY ([AuditSiteServiceId]),
        CONSTRAINT [FK_AuditSiteServices_AuditSiteAudits_AuditSiteAuditId]
            FOREIGN KEY ([AuditSiteAuditId]) REFERENCES [AuditSiteAudits]([AuditSiteAuditId]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AuditSiteServices_AuditSiteAuditId] ON [AuditSiteServices]([AuditSiteAuditId]);
END");

            // ── Seed AuditTypes ────────────────────────────────────────────────
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [AuditTypes])
BEGIN
    SET IDENTITY_INSERT [AuditTypes] ON;
    INSERT INTO [AuditTypes] ([AuditTypeId],[AuditTypeName],[AuditTypeCode],[Description],[Duration],[IsActive],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy],[Category],[RequiredCertifications],[DisplayOrder])
    VALUES
    (1, 'Initial Certification Audit', 'ICA', 'First-time certification audit for new clients',             5, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Certification', NULL, 1),
    (2, 'Surveillance Audit',          'SA',  'Annual surveillance audit to maintain certification',        2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Surveillance',  NULL, 2),
    (3, 'Recertification Audit',       'RCA', 'Three-year recertification audit to renew certificate',      4, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Certification', NULL, 3),
    (4, 'Special Audit',               'SPA', 'Special audit due to significant changes or issues',         3, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Special',       NULL, 4),
    (5, 'Integrated Audit',            'IA',  'Combined audit for multiple management systems',             6, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Integrated',    NULL, 5),
    (6, 'Pre-Assessment',              'PA',  'Optional preliminary assessment before certification',       2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Assessment',    NULL, 6),
    (7, 'Transfer Audit',              'TA',  'Audit for transferring certification from another body',     3, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Certification', NULL, 7),
    (8, 'Extension Audit',             'EA',  'Audit for extending scope of existing certification',        2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Certification', NULL, 8),
    (9, 'Extraordinary Audit',         'EXA', 'Unplanned audit due to complaints or incidents',            2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Special',       NULL, 9),
    (10,'Witness Audit',               'WA',  'Audit to witness client processes and procedures',           1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Surveillance',  NULL, 10);
    SET IDENTITY_INSERT [AuditTypes] OFF;
END");

            // ── Seed Audits ────────────────────────────────────────────────────
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [Audits])
BEGIN
    SET IDENTITY_INSERT [Audits] ON;
    INSERT INTO [Audits] ([auditId],[sites],[services],[companyId],[status],[startDate],[endDate],[leadAuditor],[type])
    VALUES
    (1, '1,2,3',  'ISO 9001',        1, 'Completed',  '2024-03-15', '2024-03-19', 'John Doe',     'Initial Certification Audit'),
    (2, '1,2',    'ISO 9001',        1, 'Completed',  '2025-03-10', '2025-03-12', 'Jane Smith',   'Surveillance Audit'),
    (3, '1,2',    'ISO 14001',       1, 'Completed',  '2024-05-20', '2024-05-24', 'John Doe',     'Initial Certification Audit'),
    (4, '1,2',    'ISO 14001',       1, 'Scheduled',  '2025-05-15', '2025-05-17', 'Jane Smith',   'Surveillance Audit'),
    (5, '1',      'ISO 45001',       1, 'Scheduled',  '2025-07-10', '2025-07-11', 'Mike Brown',   'Pre-Assessment'),
    (6, '4,5,6',  'ISO 9001',        2, 'Completed',  '2023-09-05', '2023-09-09', 'Alice Green',  'Initial Certification Audit'),
    (7, '4,5',    'ISO 9001',        2, 'Completed',  '2024-09-02', '2024-09-04', 'Bob White',    'Surveillance Audit'),
    (8, '4,5',    'ISO 27001',       2, 'InProgress', '2025-04-01', '2025-04-05', 'Alice Green',  'Recertification Audit');
    SET IDENTITY_INSERT [Audits] OFF;
END");

            // ── Seed AuditSites ────────────────────────────────────────────────
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [AuditSites])
BEGIN
    INSERT INTO [AuditSites] ([AuditId],[SiteId],[IsActive],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy],[Status],[ScheduledDate],[CompletedDate])
    VALUES
    (1, 1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2024-03-15', '2024-03-19'),
    (1, 2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2024-03-17', '2024-03-19'),
    (1, 3, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2024-03-18', '2024-03-19'),
    (2, 1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2025-03-10', '2025-03-12'),
    (2, 2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2025-03-11', '2025-03-12'),
    (3, 1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2024-05-20', '2024-05-24'),
    (3, 2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2024-05-22', '2024-05-24'),
    (4, 1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Scheduled',  '2025-05-15', NULL),
    (4, 2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Scheduled',  '2025-05-16', NULL),
    (5, 1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Scheduled',  '2025-07-10', NULL),
    (6, 4, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2023-09-05', '2023-09-09'),
    (6, 5, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2023-09-07', '2023-09-09'),
    (6, 6, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2023-09-08', '2023-09-09'),
    (7, 4, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2024-09-02', '2024-09-04'),
    (7, 5, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed',  '2024-09-03', '2024-09-04'),
    (8, 4, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'InProgress', '2025-04-01', NULL),
    (8, 5, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'InProgress', '2025-04-02', NULL);
END");

            // ── Seed AuditServices ─────────────────────────────────────────────
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [AuditServices])
BEGIN
    INSERT INTO [AuditServices] ([AuditId],[ServiceId],[IsActive],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy],[Status])
    VALUES
    (1, 1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed'),
    (2, 1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed'),
    (3, 2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed'),
    (4, 2, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Scheduled'),
    (5, 3, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Scheduled'),
    (6, 1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed'),
    (7, 1, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'Completed'),
    (8, 4, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 'InProgress');
END");

            // ── Seed AuditTeamMembers ──────────────────────────────────────────
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [AuditTeamMembers])
BEGIN
    INSERT INTO [AuditTeamMembers] ([AuditId],[UserId],[Role],[IsActive],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy],[AssignedDate],[StartDate],[EndDate],[Specialization])
    VALUES
    (1, 4,  'Lead Auditor',   1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2024-02-01', '2024-03-15', '2024-03-19', 'ISO 9001'),
    (1, 7,  'Auditor',        1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2024-02-01', '2024-03-15', '2024-03-19', 'ISO 9001'),
    (2, 7,  'Lead Auditor',   1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2025-02-01', '2025-03-10', '2025-03-12', 'ISO 9001'),
    (3, 4,  'Lead Auditor',   1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2024-04-01', '2024-05-20', '2024-05-24', 'ISO 14001'),
    (3, 8,  'Auditor',        1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2024-04-01', '2024-05-20', '2024-05-24', 'ISO 14001'),
    (4, 7,  'Lead Auditor',   1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2025-04-01', '2025-05-15', '2025-05-17', 'ISO 14001'),
    (5, 8,  'Lead Auditor',   1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2025-06-01', '2025-07-10', '2025-07-11', 'ISO 45001'),
    (6, 5,  'Lead Auditor',   1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2023-08-01', '2023-09-05', '2023-09-09', 'ISO 9001'),
    (6, 12, 'Auditor',        1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2023-08-01', '2023-09-05', '2023-09-09', 'ISO 9001'),
    (7, 12, 'Lead Auditor',   1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2024-08-01', '2024-09-02', '2024-09-04', 'ISO 9001'),
    (8, 5,  'Lead Auditor',   1, GETUTCDATE(), GETUTCDATE(), 1, 1, '2025-03-01', '2025-04-01', '2025-04-05', 'ISO 27001');
END");

            // ── Seed AuditSiteAudits ───────────────────────────────────────────
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [AuditSiteAudits])
BEGIN
    INSERT INTO [AuditSiteAudits] ([AuditId],[SiteId],[AuditTypeId],[AuditNumber],[ScheduledDate],[StartDate],[EndDate],[CompletedDate],[Status],[LeadAuditorId],[IsActive],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy],[CertificateIssued])
    VALUES
    (1, 1, 1, 'AUD-2024-001', '2024-03-15', '2024-03-15', '2024-03-17', '2024-03-19', 'Completed', 4,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 1),
    (1, 2, 1, 'AUD-2024-002', '2024-03-17', '2024-03-17', '2024-03-18', '2024-03-19', 'Completed', 4,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 1),
    (1, 3, 1, 'AUD-2024-003', '2024-03-18', '2024-03-18', '2024-03-19', '2024-03-19', 'Completed', 7,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 1),
    (2, 1, 2, 'AUD-2025-001', '2025-03-10', '2025-03-10', '2025-03-11', '2025-03-12', 'Completed', 7,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 0),
    (2, 2, 2, 'AUD-2025-002', '2025-03-11', '2025-03-11', '2025-03-12', '2025-03-12', 'Completed', 7,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 0),
    (3, 1, 1, 'AUD-2024-004', '2024-05-20', '2024-05-20', '2024-05-22', '2024-05-24', 'Completed', 4,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 1),
    (4, 1, 2, 'AUD-2025-003', '2025-05-15', '2025-05-15', '2025-05-16', NULL,         'Scheduled', 7,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 0),
    (5, 1, 6, 'AUD-2025-004', '2025-07-10', '2025-07-10', '2025-07-11', NULL,         'Scheduled', 8,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 0),
    (6, 4, 1, 'AUD-2023-001', '2023-09-05', '2023-09-05', '2023-09-08', '2023-09-09', 'Completed', 5,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 1),
    (7, 4, 2, 'AUD-2024-005', '2024-09-02', '2024-09-02', '2024-09-03', '2024-09-04', 'Completed', 12, 1, GETUTCDATE(), GETUTCDATE(), 1, 1, 0),
    (8, 4, 3, 'AUD-2025-005', '2025-04-01', '2025-04-01', '2025-04-04', NULL,         'InProgress',5,  1, GETUTCDATE(), GETUTCDATE(), 1, 1, 0);
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Truncate seed data (tables in FK dependency order)
            migrationBuilder.Sql("IF OBJECT_ID('AuditSiteServices',       'U') IS NOT NULL TRUNCATE TABLE [AuditSiteServices];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditSiteRepresentatives','U') IS NOT NULL TRUNCATE TABLE [AuditSiteRepresentatives];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditSiteAudits',         'U') IS NOT NULL TRUNCATE TABLE [AuditSiteAudits];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditTeamMembers',        'U') IS NOT NULL TRUNCATE TABLE [AuditTeamMembers];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditServices',           'U') IS NOT NULL TRUNCATE TABLE [AuditServices];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditSites',              'U') IS NOT NULL TRUNCATE TABLE [AuditSites];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditSiteRepresentatives','U') IS NOT NULL DROP TABLE [AuditSiteRepresentatives];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditSiteServices',       'U') IS NOT NULL DROP TABLE [AuditSiteServices];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditSiteAudits',         'U') IS NOT NULL DROP TABLE [AuditSiteAudits];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditTeamMembers',        'U') IS NOT NULL DROP TABLE [AuditTeamMembers];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditServices',           'U') IS NOT NULL DROP TABLE [AuditServices];");
            migrationBuilder.Sql("IF OBJECT_ID('AuditSites',              'U') IS NOT NULL DROP TABLE [AuditSites];");
            migrationBuilder.Sql("IF OBJECT_ID('Audits',                  'U') IS NOT NULL DROP TABLE [Audits];");
        }
    }
}
