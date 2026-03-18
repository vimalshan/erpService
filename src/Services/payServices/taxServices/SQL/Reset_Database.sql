-- Tax Service Database Reset Script
-- WARNING: This script DELETES all data from the database!
-- Use only for development/testing purposes

USE [TaxService]; -- Change database name if different

PRINT 'WARNING: This script will delete all data from the database!';
PRINT 'Press Ctrl+C to cancel, or wait 5 seconds...';

-- Drop tables in reverse dependency order
PRINT 'Deleting data from TaxMarginalDetails...';
DELETE FROM [dbo].[TaxMarginalDetails];

PRINT 'Deleting data from ConditionalMasters...';
DELETE FROM [dbo].[ConditionalMasters];

-- Reset identity seeds
PRINT 'Resetting identity seeds...';
DBCC CHECKIDENT ('dbo.TaxMarginalDetails', RESEED, 0);
DBCC CHECKIDENT ('dbo.ConditionalMasters', RESEED, 0);

-- Clear migration history if needed (optional)
-- DELETE FROM [dbo].[__EFMigrationsHistory];

PRINT 'Database reset complete!';
PRINT 'Run Seed_Data.sql to repopulate with sample data.';

-- Display table status
PRINT '';
PRINT 'Current table status:';
PRINT 'TaxMarginalDetails records: ' + CAST((SELECT COUNT(*) FROM [dbo].[TaxMarginalDetails]) AS VARCHAR);
PRINT 'ConditionalMasters records: ' + CAST((SELECT COUNT(*) FROM [dbo].[ConditionalMasters]) AS VARCHAR);
