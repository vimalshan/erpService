-- Database Modification Script for Module: DemandManagement
-- Created: 2026-03-09

USE DDDB;
GO

PRINT 'Starting modification for DemandManagement module...';
:r .\DemandManagement-DDDB.sql
:r .\DemandManagement-DDDB-procedures.sql
PRINT 'DemandManagement module modification completed successfully.';
GO
