-- ==========================================
-- Canteen Services - All Databases Init
-- ==========================================

-- CanteenUnitDb
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CanteenUnitDb')
BEGIN CREATE DATABASE [CanteenUnitDb]; PRINT 'CanteenUnitDb created.' END
GO
USE [CanteenUnitDb];
GO
:r /sql/CanteenUnit/CanteenUnit-tables.sql
GO
:r /sql/CanteenUnit/CanteenUnit-procedures.sql
GO

-- CardManagementDb
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CardManagementDb')
BEGIN CREATE DATABASE [CardManagementDb]; PRINT 'CardManagementDb created.' END
GO
USE [CardManagementDb];
GO
:r /sql/CardManagement/CardManagement-tables.sql
GO
:r /sql/CardManagement/CardManagement-procedures.sql
GO

-- DeductionServiceDb
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DeductionServiceDb')
BEGIN CREATE DATABASE [DeductionServiceDb]; PRINT 'DeductionServiceDb created.' END
GO
USE [DeductionServiceDb];
GO
:r /sql/Deduction/Deduction-tables.sql
GO
:r /sql/Deduction/Deduction-procedures.sql
GO

-- EligibilityServiceDb
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EligibilityServiceDb')
BEGIN CREATE DATABASE [EligibilityServiceDb]; PRINT 'EligibilityServiceDb created.' END
GO
USE [EligibilityServiceDb];
GO
:r /sql/Eligibility/Eligibility-tables.sql
GO
:r /sql/Eligibility/Eligibility-procedures.sql
GO

-- ItemMasterDb
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ItemMasterDb')
BEGIN CREATE DATABASE [ItemMasterDb]; PRINT 'ItemMasterDb created.' END
GO
USE [ItemMasterDb];
GO
:r /sql/ItemMaster/ItemMaster-tables.sql
GO
:r /sql/ItemMaster/ItemMaster-procedures.sql
GO

-- SwipeTransactionDb
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SwipeTransactionDb')
BEGIN CREATE DATABASE [SwipeTransactionDb]; PRINT 'SwipeTransactionDb created.' END
GO
USE [SwipeTransactionDb];
GO
:r /sql/SwipeTransaction/SwipeTransaction-tables.sql
GO
:r /sql/SwipeTransaction/SwipeTransaction-procedures.sql
GO

-- ReferenceDataDb
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ReferenceDataDb')
BEGIN CREATE DATABASE [ReferenceDataDb]; PRINT 'ReferenceDataDb created.' END
GO
USE [ReferenceDataDb];
GO
:r /sql/ReferenceData/ReferenceData-tables.sql
GO
:r /sql/ReferenceData/ReferenceData-procedures.sql
GO

PRINT 'All canteen databases initialized successfully.'
GO
