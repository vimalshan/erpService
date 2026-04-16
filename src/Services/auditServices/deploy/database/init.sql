-- ============================================================
-- ERP Microservices — Initial Database Setup Script
-- Run once against a fresh SQL Server instance.
-- All passwords are injected via environment variables at
-- runtime; this script creates the databases and shared schema.
-- ============================================================
SET NOCOUNT ON;
GO

-- ─────────────────────────────────────────────────────────────
-- 1. Create Databases
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ERPActionDB')
    CREATE DATABASE [ERPActionDB];
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ERPAuditDB')
    CREATE DATABASE [ERPAuditDB];
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ERPCertificateDB')
    CREATE DATABASE [ERPCertificateDB];
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ERPContractDB')
    CREATE DATABASE [ERPContractDB];
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ERPFinanceDB')
    CREATE DATABASE [ERPFinanceDB];
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ERPFindingsDB')
    CREATE DATABASE [ERPFindingsDB];
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ERPNotificationDB')
    CREATE DATABASE [ERPNotificationDB];
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ERPScheduleDB')
    CREATE DATABASE [ERPScheduleDB];
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ERPSettingsDB')
    CREATE DATABASE [ERPSettingsDB];
GO

-- ─────────────────────────────────────────────────────────────
-- 2. ERPAuditDB — Shared Reference & Audit Tables
-- ─────────────────────────────────────────────────────────────
USE [ERPAuditDB];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [UserId]                   INT            IDENTITY(1,1) NOT NULL,
        [Username]                 NVARCHAR(100)  NOT NULL,
        [Email]                    NVARCHAR(255)  NOT NULL,
        [FirstName]                NVARCHAR(100)  NOT NULL,
        [LastName]                 NVARCHAR(100)  NOT NULL,
        [PasswordHash]             NVARCHAR(255)  NOT NULL,
        [IsActive]                 BIT            NOT NULL DEFAULT 1,
        [LastLoginDate]            DATETIME       NULL,
        [CreatedDate]              DATETIME       NOT NULL DEFAULT GETDATE(),
        [ModifiedDate]             DATETIME       NOT NULL DEFAULT GETDATE(),
        [CreatedBy]                INT            NULL,
        [ModifiedBy]               INT            NULL,
        [Phone]                    NVARCHAR(20)   NULL,
        [Position]                 NVARCHAR(100)  NULL,
        [Department]               NVARCHAR(100)  NULL,
        [TimeZone]                 NVARCHAR(50)   NULL DEFAULT 'UTC',
        [Language]                 NVARCHAR(10)   NULL DEFAULT 'EN',
        [IsEmailVerified]          BIT            NOT NULL DEFAULT 0,
        [EmailVerificationToken]   NVARCHAR(255)  NULL,
        [PasswordResetToken]       NVARCHAR(255)  NULL,
        [PasswordResetExpiry]      DATETIME       NULL,
        [TwoFactorEnabled]         BIT            NOT NULL DEFAULT 0,
        [TwoFactorSecret]          NVARCHAR(100)  NULL,
        CONSTRAINT [PK_Users]              PRIMARY KEY CLUSTERED ([UserId]),
        CONSTRAINT [UK_Users_Username]     UNIQUE ([Username]),
        CONSTRAINT [UK_Users_Email]        UNIQUE ([Email])
    );
    CREATE NONCLUSTERED INDEX [IX_Users_Email]         ON [dbo].[Users] ([Email]);
    CREATE NONCLUSTERED INDEX [IX_Users_Username]      ON [dbo].[Users] ([Username]);
    CREATE NONCLUSTERED INDEX [IX_Users_IsActive]      ON [dbo].[Users] ([IsActive]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Roles] (
        [RoleId]      INT           IDENTITY(1,1) NOT NULL,
        [RoleName]    NVARCHAR(50)  NOT NULL,
        [Description] NVARCHAR(255) NULL,
        [IsActive]    BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Roles]          PRIMARY KEY CLUSTERED ([RoleId]),
        CONSTRAINT [UK_Roles_RoleName] UNIQUE ([RoleName])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserRoles' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserRoles] (
        [UserRoleId] INT      IDENTITY(1,1) NOT NULL,
        [UserId]     INT      NOT NULL,
        [RoleId]     INT      NOT NULL,
        [AssignedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_UserRoles]           PRIMARY KEY CLUSTERED ([UserRoleId]),
        CONSTRAINT [FK_UserRoles_Users]     FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId]),
        CONSTRAINT [FK_UserRoles_Roles]     FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([RoleId]),
        CONSTRAINT [UK_UserRoles_UserRole]  UNIQUE ([UserId], [RoleId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Countries' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Countries] (
        [CountryId]   INT           IDENTITY(1,1) NOT NULL,
        [CountryName] NVARCHAR(100) NOT NULL,
        [CountryCode] NVARCHAR(10)  NOT NULL,
        [IsActive]    BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([CountryId]),
        CONSTRAINT [UK_Countries_Code] UNIQUE ([CountryCode])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cities' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Cities] (
        [CityId]    INT           IDENTITY(1,1) NOT NULL,
        [CityName]  NVARCHAR(100) NOT NULL,
        [CountryId] INT           NOT NULL,
        [IsActive]  BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Cities]           PRIMARY KEY CLUSTERED ([CityId]),
        CONSTRAINT [FK_Cities_Countries] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries]([CountryId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Companies' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Companies] (
        [CompanyId]   INT           IDENTITY(1,1) NOT NULL,
        [CompanyName] NVARCHAR(255) NOT NULL,
        [Address]     NVARCHAR(500) NULL,
        [CountryId]   INT           NULL,
        [CityId]      INT           NULL,
        [Phone]       NVARCHAR(50)  NULL,
        [Email]       NVARCHAR(255) NULL,
        [IsActive]    BIT           NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Companies]           PRIMARY KEY CLUSTERED ([CompanyId]),
        CONSTRAINT [FK_Companies_Country]   FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries]([CountryId]),
        CONSTRAINT [FK_Companies_City]      FOREIGN KEY ([CityId])    REFERENCES [dbo].[Cities]([CityId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sites' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Sites] (
        [SiteId]      INT           IDENTITY(1,1) NOT NULL,
        [SiteName]    NVARCHAR(255) NOT NULL,
        [CompanyId]   INT           NOT NULL,
        [Address]     NVARCHAR(500) NULL,
        [CountryId]   INT           NULL,
        [CityId]      INT           NULL,
        [IsActive]    BIT           NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Sites]          PRIMARY KEY CLUSTERED ([SiteId]),
        CONSTRAINT [FK_Sites_Company]  FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies]([CompanyId]),
        CONSTRAINT [FK_Sites_Country]  FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries]([CountryId]),
        CONSTRAINT [FK_Sites_City]     FOREIGN KEY ([CityId])    REFERENCES [dbo].[Cities]([CityId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Services' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Services] (
        [ServiceId]   INT           IDENTITY(1,1) NOT NULL,
        [ServiceName] NVARCHAR(255) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [IsActive]    BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([ServiceId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditTypes' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[AuditTypes] (
        [AuditTypeId]   INT           IDENTITY(1,1) NOT NULL,
        [AuditTypeName] NVARCHAR(100) NOT NULL,
        [Description]   NVARCHAR(500) NULL,
        [IsActive]      BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_AuditTypes] PRIMARY KEY CLUSTERED ([AuditTypeId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Audits' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Audits] (
        [AuditId]       INT           IDENTITY(1,1) NOT NULL,
        [AuditTypeId]   INT           NULL,
        [CompanyId]     INT           NULL,
        [Status]        NVARCHAR(50)  NOT NULL DEFAULT 'Planned',
        [StartDate]     DATETIME      NULL,
        [EndDate]       DATETIME      NULL,
        [LeadAuditorId] INT           NULL,
        [CreatedDate]   DATETIME      NOT NULL DEFAULT GETDATE(),
        [ModifiedDate]  DATETIME      NOT NULL DEFAULT GETDATE(),
        [CreatedBy]     INT           NULL,
        [ModifiedBy]    INT           NULL,
        CONSTRAINT [PK_Audits]           PRIMARY KEY CLUSTERED ([AuditId]),
        CONSTRAINT [FK_Audits_AuditType] FOREIGN KEY ([AuditTypeId]) REFERENCES [dbo].[AuditTypes]([AuditTypeId]),
        CONSTRAINT [FK_Audits_Company]   FOREIGN KEY ([CompanyId])   REFERENCES [dbo].[Companies]([CompanyId]),
        CONSTRAINT [FK_Audits_LeadAudit] FOREIGN KEY ([LeadAuditorId]) REFERENCES [dbo].[Users]([UserId])
    );
    CREATE NONCLUSTERED INDEX [IX_Audits_Status]    ON [dbo].[Audits] ([Status]);
    CREATE NONCLUSTERED INDEX [IX_Audits_StartDate] ON [dbo].[Audits] ([StartDate]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditSites' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[AuditSites] (
        [AuditSiteId] INT IDENTITY(1,1) NOT NULL,
        [AuditId]     INT NOT NULL,
        [SiteId]      INT NOT NULL,
        CONSTRAINT [PK_AuditSites]       PRIMARY KEY CLUSTERED ([AuditSiteId]),
        CONSTRAINT [FK_AuditSites_Audit] FOREIGN KEY ([AuditId]) REFERENCES [dbo].[Audits]([AuditId]),
        CONSTRAINT [FK_AuditSites_Site]  FOREIGN KEY ([SiteId])  REFERENCES [dbo].[Sites]([SiteId]),
        CONSTRAINT [UK_AuditSites]       UNIQUE ([AuditId], [SiteId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditTeamMembers' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[AuditTeamMembers] (
        [TeamMemberId] INT           IDENTITY(1,1) NOT NULL,
        [AuditId]      INT           NOT NULL,
        [UserId]       INT           NOT NULL,
        [Role]         NVARCHAR(50)  NULL,
        CONSTRAINT [PK_AuditTeamMembers]       PRIMARY KEY CLUSTERED ([TeamMemberId]),
        CONSTRAINT [FK_AuditTeamMembers_Audit] FOREIGN KEY ([AuditId]) REFERENCES [dbo].[Audits]([AuditId]),
        CONSTRAINT [FK_AuditTeamMembers_User]  FOREIGN KEY ([UserId])  REFERENCES [dbo].[Users]([UserId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLogs' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[AuditLogs] (
        [LogId]       BIGINT         IDENTITY(1,1) NOT NULL,
        [EntityName]  NVARCHAR(100)  NOT NULL,
        [EntityId]    INT            NOT NULL,
        [Action]      NVARCHAR(50)   NOT NULL,
        [ChangedBy]   INT            NULL,
        [ChangedDate] DATETIME       NOT NULL DEFAULT GETDATE(),
        [OldValues]   NVARCHAR(MAX)  NULL,
        [NewValues]   NVARCHAR(MAX)  NULL,
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([LogId])
    );
    CREATE NONCLUSTERED INDEX [IX_AuditLogs_Entity] ON [dbo].[AuditLogs] ([EntityName], [EntityId]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ErrorLogs' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[ErrorLogs] (
        [ErrorId]     BIGINT         IDENTITY(1,1) NOT NULL,
        [ServiceName] NVARCHAR(100)  NULL,
        [Message]     NVARCHAR(MAX)  NOT NULL,
        [StackTrace]  NVARCHAR(MAX)  NULL,
        [LoggedDate]  DATETIME       NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_ErrorLogs] PRIMARY KEY CLUSTERED ([ErrorId])
    );
END
GO

-- ─────────────────────────────────────────────────────────────
-- 3. ERPFindingsDB — Findings, Chapters, Clauses
-- ─────────────────────────────────────────────────────────────
USE [ERPFindingsDB];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FindingCategories' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[FindingCategories] (
        [CategoryId]   INT           IDENTITY(1,1) NOT NULL,
        [CategoryName] NVARCHAR(100) NOT NULL,
        [Description]  NVARCHAR(500) NULL,
        [IsActive]     BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_FindingCategories] PRIMARY KEY CLUSTERED ([CategoryId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FindingStatuses' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[FindingStatuses] (
        [StatusId]   INT           IDENTITY(1,1) NOT NULL,
        [StatusName] NVARCHAR(50)  NOT NULL,
        [IsActive]   BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_FindingStatuses] PRIMARY KEY CLUSTERED ([StatusId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Chapters' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Chapters] (
        [ChapterId]     INT           IDENTITY(1,1) NOT NULL,
        [ChapterNumber] NVARCHAR(20)  NOT NULL,
        [ChapterTitle]  NVARCHAR(255) NOT NULL,
        [IsActive]      BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Chapters] PRIMARY KEY CLUSTERED ([ChapterId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clauses' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Clauses] (
        [ClauseId]     INT           IDENTITY(1,1) NOT NULL,
        [ChapterId]    INT           NOT NULL,
        [ClauseNumber] NVARCHAR(20)  NOT NULL,
        [ClauseTitle]  NVARCHAR(255) NOT NULL,
        [IsActive]     BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Clauses]           PRIMARY KEY CLUSTERED ([ClauseId]),
        CONSTRAINT [FK_Clauses_Chapter]   FOREIGN KEY ([ChapterId]) REFERENCES [dbo].[Chapters]([ChapterId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FocusAreas' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[FocusAreas] (
        [FocusAreaId]   INT           IDENTITY(1,1) NOT NULL,
        [FocusAreaName] NVARCHAR(255) NOT NULL,
        [IsActive]      BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_FocusAreas] PRIMARY KEY CLUSTERED ([FocusAreaId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Findings' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Findings] (
        [FindingId]    INT            IDENTITY(1,1) NOT NULL,
        [AuditId]      INT            NOT NULL,
        [CategoryId]   INT            NULL,
        [StatusId]     INT            NULL,
        [ClauseId]     INT            NULL,
        [FocusAreaId]  INT            NULL,
        [Title]        NVARCHAR(500)  NOT NULL,
        [Description]  NVARCHAR(MAX)  NULL,
        [Evidence]     NVARCHAR(MAX)  NULL,
        [Severity]     NVARCHAR(50)   NULL,
        [DueDate]      DATETIME       NULL,
        [ClosedDate]   DATETIME       NULL,
        [AssignedTo]   INT            NULL,
        [CreatedDate]  DATETIME       NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] DATETIME       NOT NULL DEFAULT GETDATE(),
        [CreatedBy]    INT            NULL,
        CONSTRAINT [PK_Findings]             PRIMARY KEY CLUSTERED ([FindingId]),
        CONSTRAINT [FK_Findings_Category]    FOREIGN KEY ([CategoryId])  REFERENCES [dbo].[FindingCategories]([CategoryId]),
        CONSTRAINT [FK_Findings_Status]      FOREIGN KEY ([StatusId])    REFERENCES [dbo].[FindingStatuses]([StatusId]),
        CONSTRAINT [FK_Findings_Clause]      FOREIGN KEY ([ClauseId])    REFERENCES [dbo].[Clauses]([ClauseId]),
        CONSTRAINT [FK_Findings_FocusArea]   FOREIGN KEY ([FocusAreaId]) REFERENCES [dbo].[FocusAreas]([FocusAreaId])
    );
    CREATE NONCLUSTERED INDEX [IX_Findings_AuditId]   ON [dbo].[Findings] ([AuditId]);
    CREATE NONCLUSTERED INDEX [IX_Findings_StatusId]  ON [dbo].[Findings] ([StatusId]);
    CREATE NONCLUSTERED INDEX [IX_Findings_DueDate]   ON [dbo].[Findings] ([DueDate]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FindingResponses' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[FindingResponses] (
        [ResponseId]   INT           IDENTITY(1,1) NOT NULL,
        [FindingId]    INT           NOT NULL,
        [Response]     NVARCHAR(MAX) NOT NULL,
        [RespondedBy]  INT           NULL,
        [RespondedDate] DATETIME     NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_FindingResponses]           PRIMARY KEY CLUSTERED ([ResponseId]),
        CONSTRAINT [FK_FindingResponses_Finding]   FOREIGN KEY ([FindingId]) REFERENCES [dbo].[Findings]([FindingId])
    );
END
GO

-- ─────────────────────────────────────────────────────────────
-- 4. ERPCertificateDB
-- ─────────────────────────────────────────────────────────────
USE [ERPCertificateDB];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Certificates' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Certificates] (
        [CertificateId]   INT           IDENTITY(1,1) NOT NULL,
        [CertificateNo]   NVARCHAR(100) NOT NULL,
        [CompanyId]       INT           NOT NULL,
        [AuditId]         INT           NULL,
        [IssuedDate]      DATETIME      NULL,
        [ExpiryDate]      DATETIME      NULL,
        [Status]          NVARCHAR(50)  NOT NULL DEFAULT 'Active',
        [IssuedBy]        INT           NULL,
        [CreatedDate]     DATETIME      NOT NULL DEFAULT GETDATE(),
        [ModifiedDate]    DATETIME      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Certificates]         PRIMARY KEY CLUSTERED ([CertificateId]),
        CONSTRAINT [UK_Certificates_No]      UNIQUE ([CertificateNo])
    );
    CREATE NONCLUSTERED INDEX [IX_Certificates_CompanyId] ON [dbo].[Certificates] ([CompanyId]);
    CREATE NONCLUSTERED INDEX [IX_Certificates_ExpiryDate] ON [dbo].[Certificates] ([ExpiryDate]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CertificateSites' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[CertificateSites] (
        [CertSiteId]    INT IDENTITY(1,1) NOT NULL,
        [CertificateId] INT NOT NULL,
        [SiteId]        INT NOT NULL,
        CONSTRAINT [PK_CertificateSites]            PRIMARY KEY CLUSTERED ([CertSiteId]),
        CONSTRAINT [FK_CertificateSites_Certificate] FOREIGN KEY ([CertificateId]) REFERENCES [dbo].[Certificates]([CertificateId]),
        CONSTRAINT [UK_CertificateSites]            UNIQUE ([CertificateId], [SiteId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CertificateServices' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[CertificateServices] (
        [CertServiceId] INT IDENTITY(1,1) NOT NULL,
        [CertificateId] INT NOT NULL,
        [ServiceId]     INT NOT NULL,
        CONSTRAINT [PK_CertificateServices]             PRIMARY KEY CLUSTERED ([CertServiceId]),
        CONSTRAINT [FK_CertificateServices_Certificate] FOREIGN KEY ([CertificateId]) REFERENCES [dbo].[Certificates]([CertificateId]),
        CONSTRAINT [UK_CertificateServices]             UNIQUE ([CertificateId], [ServiceId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CertificateAdditionalScopes' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[CertificateAdditionalScopes] (
        [ScopeId]       INT           IDENTITY(1,1) NOT NULL,
        [CertificateId] INT           NOT NULL,
        [ScopeText]     NVARCHAR(MAX) NOT NULL,
        CONSTRAINT [PK_CertAdditionalScopes]            PRIMARY KEY CLUSTERED ([ScopeId]),
        CONSTRAINT [FK_CertAdditionalScopes_Certificate] FOREIGN KEY ([CertificateId]) REFERENCES [dbo].[Certificates]([CertificateId])
    );
END
GO

-- ─────────────────────────────────────────────────────────────
-- 5. ERPContractDB
-- ─────────────────────────────────────────────────────────────
USE [ERPContractDB];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Contracts' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Contracts] (
        [ContractId]   INT           IDENTITY(1,1) NOT NULL,
        [ContractNo]   NVARCHAR(100) NOT NULL,
        [CompanyId]    INT           NOT NULL,
        [StartDate]    DATETIME      NULL,
        [EndDate]      DATETIME      NULL,
        [Value]        DECIMAL(18,2) NULL,
        [Currency]     NVARCHAR(10)  NULL DEFAULT 'USD',
        [Status]       NVARCHAR(50)  NOT NULL DEFAULT 'Active',
        [CreatedDate]  DATETIME      NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] DATETIME      NOT NULL DEFAULT GETDATE(),
        [CreatedBy]    INT           NULL,
        CONSTRAINT [PK_Contracts]       PRIMARY KEY CLUSTERED ([ContractId]),
        CONSTRAINT [UK_Contracts_No]    UNIQUE ([ContractNo])
    );
    CREATE NONCLUSTERED INDEX [IX_Contracts_CompanyId] ON [dbo].[Contracts] ([CompanyId]);
    CREATE NONCLUSTERED INDEX [IX_Contracts_Status]    ON [dbo].[Contracts] ([Status]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ContractSites' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[ContractSites] (
        [ContractSiteId] INT IDENTITY(1,1) NOT NULL,
        [ContractId]     INT NOT NULL,
        [SiteId]         INT NOT NULL,
        CONSTRAINT [PK_ContractSites]         PRIMARY KEY CLUSTERED ([ContractSiteId]),
        CONSTRAINT [FK_ContractSites_Contract] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contracts]([ContractId]),
        CONSTRAINT [UK_ContractSites]         UNIQUE ([ContractId], [SiteId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ContractServices' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[ContractServices] (
        [ContractServiceId] INT IDENTITY(1,1) NOT NULL,
        [ContractId]        INT NOT NULL,
        [ServiceId]         INT NOT NULL,
        CONSTRAINT [PK_ContractServices]          PRIMARY KEY CLUSTERED ([ContractServiceId]),
        CONSTRAINT [FK_ContractServices_Contract] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contracts]([ContractId]),
        CONSTRAINT [UK_ContractServices]          UNIQUE ([ContractId], [ServiceId])
    );
END
GO

-- ─────────────────────────────────────────────────────────────
-- 6. ERPFinanceDB
-- ─────────────────────────────────────────────────────────────
USE [ERPFinanceDB];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Invoices' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Invoices] (
        [InvoiceId]     INT           IDENTITY(1,1) NOT NULL,
        [InvoiceNo]     NVARCHAR(100) NOT NULL,
        [CompanyId]     INT           NOT NULL,
        [AuditId]       INT           NULL,
        [ContractId]    INT           NULL,
        [Amount]        DECIMAL(18,2) NOT NULL,
        [Currency]      NVARCHAR(10)  NOT NULL DEFAULT 'USD',
        [IssuedDate]    DATETIME      NOT NULL DEFAULT GETDATE(),
        [DueDate]       DATETIME      NULL,
        [PaidDate]      DATETIME      NULL,
        [Status]        NVARCHAR(50)  NOT NULL DEFAULT 'Pending',
        [CreatedDate]   DATETIME      NOT NULL DEFAULT GETDATE(),
        [ModifiedDate]  DATETIME      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Invoices]       PRIMARY KEY CLUSTERED ([InvoiceId]),
        CONSTRAINT [UK_Invoices_No]    UNIQUE ([InvoiceNo])
    );
    CREATE NONCLUSTERED INDEX [IX_Invoices_CompanyId] ON [dbo].[Invoices] ([CompanyId]);
    CREATE NONCLUSTERED INDEX [IX_Invoices_Status]    ON [dbo].[Invoices] ([Status]);
    CREATE NONCLUSTERED INDEX [IX_Invoices_DueDate]   ON [dbo].[Invoices] ([DueDate]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Financials' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Financials] (
        [FinancialId]  INT           IDENTITY(1,1) NOT NULL,
        [ContractId]   INT           NULL,
        [CompanyId]    INT           NULL,
        [Type]         NVARCHAR(50)  NOT NULL,
        [Amount]       DECIMAL(18,2) NOT NULL,
        [Currency]     NVARCHAR(10)  NOT NULL DEFAULT 'USD',
        [TransDate]    DATETIME      NOT NULL DEFAULT GETDATE(),
        [Reference]    NVARCHAR(255) NULL,
        [CreatedDate]  DATETIME      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Financials] PRIMARY KEY CLUSTERED ([FinancialId])
    );
    CREATE NONCLUSTERED INDEX [IX_Financials_ContractId] ON [dbo].[Financials] ([ContractId]);
    CREATE NONCLUSTERED INDEX [IX_Financials_TransDate]  ON [dbo].[Financials] ([TransDate]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'InvoiceAuditLog' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[InvoiceAuditLog] (
        [LogId]       BIGINT        IDENTITY(1,1) NOT NULL,
        [InvoiceId]   INT           NOT NULL,
        [Action]      NVARCHAR(50)  NOT NULL,
        [ChangedBy]   INT           NULL,
        [ChangedDate] DATETIME      NOT NULL DEFAULT GETDATE(),
        [OldStatus]   NVARCHAR(50)  NULL,
        [NewStatus]   NVARCHAR(50)  NULL,
        CONSTRAINT [PK_InvoiceAuditLog]          PRIMARY KEY CLUSTERED ([LogId]),
        CONSTRAINT [FK_InvoiceAuditLog_Invoice]  FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoices]([InvoiceId])
    );
END
GO

-- ─────────────────────────────────────────────────────────────
-- 7. ERPActionDB
-- ─────────────────────────────────────────────────────────────
USE [ERPActionDB];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Actions' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Actions] (
        [ActionId]        INT           IDENTITY(1,1) NOT NULL,
        [FindingId]       INT           NULL,
        [AuditId]         INT           NULL,
        [Title]           NVARCHAR(500) NOT NULL,
        [Description]     NVARCHAR(MAX) NULL,
        [AssignedTo]      INT           NULL,
        [Status]          NVARCHAR(50)  NOT NULL DEFAULT 'Open',
        [Priority]        NVARCHAR(50)  NULL DEFAULT 'Medium',
        [DueDate]         DATETIME      NULL,
        [CompletedDate]   DATETIME      NULL,
        [CreatedDate]     DATETIME      NOT NULL DEFAULT GETDATE(),
        [ModifiedDate]    DATETIME      NOT NULL DEFAULT GETDATE(),
        [CreatedBy]       INT           NULL,
        CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED ([ActionId])
    );
    CREATE NONCLUSTERED INDEX [IX_Actions_Status]     ON [dbo].[Actions] ([Status]);
    CREATE NONCLUSTERED INDEX [IX_Actions_AssignedTo] ON [dbo].[Actions] ([AssignedTo]);
    CREATE NONCLUSTERED INDEX [IX_Actions_DueDate]    ON [dbo].[Actions] ([DueDate]);
END
GO

-- ─────────────────────────────────────────────────────────────
-- 8. ERPNotificationDB
-- ─────────────────────────────────────────────────────────────
USE [ERPNotificationDB];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NotificationCategories' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[NotificationCategories] (
        [CategoryId]   INT           IDENTITY(1,1) NOT NULL,
        [CategoryName] NVARCHAR(100) NOT NULL,
        [IsActive]     BIT           NOT NULL DEFAULT 1,
        CONSTRAINT [PK_NotificationCategories] PRIMARY KEY CLUSTERED ([CategoryId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Notifications] (
        [NotificationId] INT           IDENTITY(1,1) NOT NULL,
        [CategoryId]     INT           NULL,
        [Title]          NVARCHAR(255) NOT NULL,
        [Body]           NVARCHAR(MAX) NOT NULL,
        [RecipientId]    INT           NULL,
        [IsRead]         BIT           NOT NULL DEFAULT 0,
        [SentDate]       DATETIME      NOT NULL DEFAULT GETDATE(),
        [ReadDate]       DATETIME      NULL,
        [Channel]        NVARCHAR(50)  NULL DEFAULT 'InApp',
        [EntityType]     NVARCHAR(100) NULL,
        [EntityId]       INT           NULL,
        CONSTRAINT [PK_Notifications]            PRIMARY KEY CLUSTERED ([NotificationId]),
        CONSTRAINT [FK_Notifications_Category]   FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[NotificationCategories]([CategoryId])
    );
    CREATE NONCLUSTERED INDEX [IX_Notifications_RecipientId] ON [dbo].[Notifications] ([RecipientId]);
    CREATE NONCLUSTERED INDEX [IX_Notifications_IsRead]      ON [dbo].[Notifications] ([IsRead]);
    CREATE NONCLUSTERED INDEX [IX_Notifications_SentDate]    ON [dbo].[Notifications] ([SentDate]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserNotificationAccess' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserNotificationAccess] (
        [AccessId]   INT IDENTITY(1,1) NOT NULL,
        [UserId]     INT NOT NULL,
        [CategoryId] INT NOT NULL,
        CONSTRAINT [PK_UserNotificationAccess] PRIMARY KEY CLUSTERED ([AccessId]),
        CONSTRAINT [UK_UserNotificationAccess] UNIQUE ([UserId], [CategoryId])
    );
END
GO

-- ─────────────────────────────────────────────────────────────
-- 9. ERPScheduleDB
-- ─────────────────────────────────────────────────────────────
USE [ERPScheduleDB];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Trainings' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Trainings] (
        [TrainingId]   INT           IDENTITY(1,1) NOT NULL,
        [Title]        NVARCHAR(255) NOT NULL,
        [Description]  NVARCHAR(MAX) NULL,
        [StartDate]    DATETIME      NULL,
        [EndDate]      DATETIME      NULL,
        [TrainerId]    INT           NULL,
        [Status]       NVARCHAR(50)  NOT NULL DEFAULT 'Scheduled',
        [CreatedDate]  DATETIME      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Trainings] PRIMARY KEY CLUSTERED ([TrainingId])
    );
    CREATE NONCLUSTERED INDEX [IX_Trainings_StartDate] ON [dbo].[Trainings] ([StartDate]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserTrainings' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserTrainings] (
        [UserTrainingId] INT      IDENTITY(1,1) NOT NULL,
        [UserId]         INT      NOT NULL,
        [TrainingId]     INT      NOT NULL,
        [Status]         NVARCHAR(50) NOT NULL DEFAULT 'Enrolled',
        [EnrolledDate]   DATETIME NOT NULL DEFAULT GETDATE(),
        [CompletedDate]  DATETIME NULL,
        CONSTRAINT [PK_UserTrainings]           PRIMARY KEY CLUSTERED ([UserTrainingId]),
        CONSTRAINT [FK_UserTrainings_Training]  FOREIGN KEY ([TrainingId]) REFERENCES [dbo].[Trainings]([TrainingId]),
        CONSTRAINT [UK_UserTrainings]           UNIQUE ([UserId], [TrainingId])
    );
END
GO

-- ─────────────────────────────────────────────────────────────
-- 10. ERPSettingsDB
-- ─────────────────────────────────────────────────────────────
USE [ERPSettingsDB];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserPreferences' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserPreferences] (
        [PreferenceId] INT           IDENTITY(1,1) NOT NULL,
        [UserId]       INT           NOT NULL,
        [Key]          NVARCHAR(100) NOT NULL,
        [Value]        NVARCHAR(MAX) NULL,
        [UpdatedDate]  DATETIME      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_UserPreferences]    PRIMARY KEY CLUSTERED ([PreferenceId]),
        CONSTRAINT [UK_UserPreferences]    UNIQUE ([UserId], [Key])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserCountryAccess' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserCountryAccess] (
        [AccessId]  INT IDENTITY(1,1) NOT NULL,
        [UserId]    INT NOT NULL,
        [CountryId] INT NOT NULL,
        CONSTRAINT [PK_UserCountryAccess] PRIMARY KEY CLUSTERED ([AccessId]),
        CONSTRAINT [UK_UserCountryAccess] UNIQUE ([UserId], [CountryId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserCityAccess' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserCityAccess] (
        [AccessId] INT IDENTITY(1,1) NOT NULL,
        [UserId]   INT NOT NULL,
        [CityId]   INT NOT NULL,
        CONSTRAINT [PK_UserCityAccess] PRIMARY KEY CLUSTERED ([AccessId]),
        CONSTRAINT [UK_UserCityAccess] UNIQUE ([UserId], [CityId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserCompanyAccess' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserCompanyAccess] (
        [AccessId]  INT IDENTITY(1,1) NOT NULL,
        [UserId]    INT NOT NULL,
        [CompanyId] INT NOT NULL,
        CONSTRAINT [PK_UserCompanyAccess] PRIMARY KEY CLUSTERED ([AccessId]),
        CONSTRAINT [UK_UserCompanyAccess] UNIQUE ([UserId], [CompanyId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserSiteAccess' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserSiteAccess] (
        [AccessId] INT IDENTITY(1,1) NOT NULL,
        [UserId]   INT NOT NULL,
        [SiteId]   INT NOT NULL,
        CONSTRAINT [PK_UserSiteAccess] PRIMARY KEY CLUSTERED ([AccessId]),
        CONSTRAINT [UK_UserSiteAccess] UNIQUE ([UserId], [SiteId])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserServiceAccess' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserServiceAccess] (
        [AccessId]  INT IDENTITY(1,1) NOT NULL,
        [UserId]    INT NOT NULL,
        [ServiceId] INT NOT NULL,
        CONSTRAINT [PK_UserServiceAccess] PRIMARY KEY CLUSTERED ([AccessId]),
        CONSTRAINT [UK_UserServiceAccess] UNIQUE ([UserId], [ServiceId])
    );
END
GO

-- ─────────────────────────────────────────────────────────────
-- 11. Seed: Essential Reference Data (ERPAuditDB)
-- ─────────────────────────────────────────────────────────────
USE [ERPAuditDB];
GO

-- Roles
IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'Administrator')
    INSERT INTO [dbo].[Roles] ([RoleName], [Description]) VALUES
        ('Administrator', 'Full system access'),
        ('LeadAuditor',   'Lead audit team member'),
        ('Auditor',       'Standard auditor'),
        ('Client',        'External client access'),
        ('Finance',       'Finance department access'),
        ('Viewer',        'Read-only access');
GO

-- Audit Types
IF NOT EXISTS (SELECT 1 FROM [dbo].[AuditTypes] WHERE [AuditTypeName] = 'Initial')
    INSERT INTO [dbo].[AuditTypes] ([AuditTypeName], [Description]) VALUES
        ('Initial',       'First certification audit'),
        ('Surveillance',  'Annual surveillance audit'),
        ('Recertification', 'Three-year recertification'),
        ('Special',       'Unannounced or special purpose audit');
GO

-- Seed: Notification Categories
USE [ERPNotificationDB];
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[NotificationCategories] WHERE [CategoryName] = 'AuditAlert')
    INSERT INTO [dbo].[NotificationCategories] ([CategoryName]) VALUES
        ('AuditAlert'),
        ('FindingUpdate'),
        ('CertificateExpiry'),
        ('ActionDue'),
        ('InvoiceReminder'),
        ('SystemMessage');
GO

PRINT 'ERP database initialization complete.';
GO
