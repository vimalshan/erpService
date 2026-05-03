-- Stub support tables for Finance SPs (Companies, Countries, Contracts, Users, UserCompanyAccess)
-- and extra Invoice columns referenced by SPs but not in the EF entity.

IF OBJECT_ID('dbo.Countries') IS NULL
CREATE TABLE dbo.Countries (
    CountryId INT IDENTITY(1,1) PRIMARY KEY,
    CountryName NVARCHAR(100) NOT NULL,
    CountryCodeAlpha2 NVARCHAR(5) NULL
);

IF OBJECT_ID('dbo.Companies') IS NULL
CREATE TABLE dbo.Companies (
    CompanyId INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(200) NOT NULL,
    CompanyCode NVARCHAR(50) NULL,
    Address NVARCHAR(500) NULL,
    ContactPerson NVARCHAR(200) NULL,
    CountryId INT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

IF OBJECT_ID('dbo.Contracts') IS NULL
CREATE TABLE dbo.Contracts (
    ContractId INT IDENTITY(1,1) PRIMARY KEY,
    ContractNumber NVARCHAR(100) NOT NULL,
    CompanyId INT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

IF OBJECT_ID('dbo.Users') IS NULL
CREATE TABLE dbo.Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

IF OBJECT_ID('dbo.UserCompanyAccess') IS NULL
CREATE TABLE dbo.UserCompanyAccess (
    UserCompanyAccessId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    CompanyId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Add missing columns to Invoices (referenced by SPs)
IF COL_LENGTH('dbo.Invoices','BillingAddress')   IS NULL ALTER TABLE dbo.Invoices ADD BillingAddress   NVARCHAR(500) NULL;
IF COL_LENGTH('dbo.Invoices','ContactPerson')    IS NULL ALTER TABLE dbo.Invoices ADD ContactPerson    NVARCHAR(200) NULL;
IF COL_LENGTH('dbo.Invoices','OriginalInvoice')  IS NULL ALTER TABLE dbo.Invoices ADD OriginalInvoice  NVARCHAR(50)  NULL;
IF COL_LENGTH('dbo.Invoices','ReferenceNumber')  IS NULL ALTER TABLE dbo.Invoices ADD ReferenceNumber  NVARCHAR(100) NULL;
IF COL_LENGTH('dbo.Invoices','ReportingCountry') IS NULL ALTER TABLE dbo.Invoices ADD ReportingCountry NVARCHAR(10)  NULL;
IF COL_LENGTH('dbo.Invoices','ProjectNumber')    IS NULL ALTER TABLE dbo.Invoices ADD ProjectNumber    NVARCHAR(100) NULL;
IF COL_LENGTH('dbo.Invoices','AccountDNVId')     IS NULL ALTER TABLE dbo.Invoices ADD AccountDNVId     NVARCHAR(50)  NULL;
IF COL_LENGTH('dbo.Invoices','DocumentPath')     IS NULL ALTER TABLE dbo.Invoices ADD DocumentPath     NVARCHAR(500) NULL;
IF COL_LENGTH('dbo.Invoices','FileName')         IS NULL ALTER TABLE dbo.Invoices ADD FileName         NVARCHAR(255) NULL;
IF COL_LENGTH('dbo.Invoices','FileContent')      IS NULL ALTER TABLE dbo.Invoices ADD FileContent      VARBINARY(MAX) NULL;
IF COL_LENGTH('dbo.Invoices','FileSize')         IS NULL ALTER TABLE dbo.Invoices ADD FileSize         BIGINT NULL;
IF COL_LENGTH('dbo.Invoices','ContentType')      IS NULL ALTER TABLE dbo.Invoices ADD ContentType      NVARCHAR(100) NULL;
GO

-- Seed stub tables
IF NOT EXISTS (SELECT 1 FROM dbo.Countries)
BEGIN
    SET IDENTITY_INSERT dbo.Countries ON;
    INSERT INTO dbo.Countries (CountryId, CountryName, CountryCodeAlpha2) VALUES
      (1,'Norway','NO'),(2,'United States','US'),(3,'Germany','DE');
    SET IDENTITY_INSERT dbo.Countries OFF;
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Companies)
BEGIN
    SET IDENTITY_INSERT dbo.Companies ON;
    INSERT INTO dbo.Companies (CompanyId, CompanyName, CompanyCode, Address, ContactPerson, CountryId, IsActive) VALUES
      (1,'DNV AS','DNV','Veritasveien 1, 1363 Hovik, Norway','Erik Hansen',1,1),
      (2,'Acme Corporation','ACME','100 Main St, NY, USA','John Smith',2,1),
      (3,'Global Industries','GLOBI','Hauptstr. 12, Berlin','Anna Mueller',3,1);
    SET IDENTITY_INSERT dbo.Companies OFF;
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Contracts)
BEGIN
    SET IDENTITY_INSERT dbo.Contracts ON;
    INSERT INTO dbo.Contracts (ContractId, ContractNumber, CompanyId, IsActive) VALUES
      (1,'CTR-2024-001',2,1),(2,'CTR-2024-002',2,1),(3,'CTR-2024-003',2,1),
      (4,'CTR-2024-004',3,1),(5,'CTR-2024-005',3,1),(6,'CTR-2024-006',3,1),
      (7,'CTR-2023-001',1,1),(8,'CTR-2024-007',1,1),(9,'CTR-2025-PILOT',2,1);
    SET IDENTITY_INSERT dbo.Contracts OFF;
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Users)
BEGIN
    SET IDENTITY_INSERT dbo.Users ON;
    INSERT INTO dbo.Users (UserId, UserName, Email, IsActive) VALUES
      (1,'admin','admin@dnv.com',1);
    SET IDENTITY_INSERT dbo.Users OFF;
END;

IF NOT EXISTS (SELECT 1 FROM dbo.UserCompanyAccess)
BEGIN
    INSERT INTO dbo.UserCompanyAccess (UserId, CompanyId, IsActive) VALUES
      (1,1,1),(1,2,1),(1,3,1);
END;
GO
