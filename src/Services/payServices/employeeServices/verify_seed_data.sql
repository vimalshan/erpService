-- Verify PAYDB database and seed data
USE PAYDB;

-- Check Employees table
SELECT COUNT(*) as EmployeeCount FROM Employees;
SELECT TOP 5 EmployeeSystemId, FirstName, LastName, Email, EmploymentStatus FROM Employees;

-- Check SalaryIncrementLogs table
SELECT COUNT(*) as IncrementLogCount FROM SalaryIncrementLogs;
SELECT TOP 4 EmployeeSystemId, NewCTC, IncrementPercentage, EffectiveDate,Status FROM SalaryIncrementLogs;

-- Check database structure
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo';

-- Check all columns in Employees
SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Employees' ORDER BY ORDINAL_POSITION;
