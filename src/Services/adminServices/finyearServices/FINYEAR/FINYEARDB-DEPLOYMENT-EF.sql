-- ==========================================
-- MODULE: FINYEAR
-- Component: Deployment Script
-- Description: Complete deployment script for Financial Year Service Database
-- Database: ADMINDB
-- Connection String: Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name="FinyearAPI";Command Timeout=0
-- Usage: Execute this script in SQL Server Management Studio to deploy the entire database
-- ==========================================

-- Step 1: Execute Database Creation and Migration
:r .\FINYEAR-Migration.sql

-- Step 2: Execute Stored Procedures
:r .\FINYEAR-Procedures.sql

-- Step 3: (Optional) Execute Sample Data
-- Uncomment the line below if you want to insert sample data
-- :r .\FINYEAR-SampleData.sql

-- ==========================================
-- Deployment Summary
-- ==========================================

PRINT '====================================';
PRINT 'FINYEAR Deployment Summary';
PRINT '====================================';
PRINT 'Database: ADMINDB';
PRINT 'Service: Financial Year Management';
PRINT 'Framework: .NET 8 with Entity Framework Core';
PRINT 'ORM Support: Entity Framework Core + Dapper';
PRINT 'API: RESTful API with Swagger Documentation';
PRINT '====================================';
PRINT 'Deployment Status: COMPLETED';
PRINT '====================================';
GO
