-- Seed Data Script for DemandManagement Module (DDDB)
-- Run AFTER EF migration has created the DEMAND_MASTER table.

USE DDDB;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.DEMAND_MASTER WHERE DEMAND_ID = 1)
BEGIN
    SET IDENTITY_INSERT dbo.DEMAND_MASTER ON;

    INSERT INTO dbo.DEMAND_MASTER
        (DEMAND_ID, DEMAND_TYPE, DEPARTMENT_ID, DEMAND_DESCRIPTION, REQUIRED_DATE,
         PRIORITY, DEMAND_STATUS, CREATED_BY, CREATED_ON)
    VALUES
        (1, 'Stationery', 1, 'Monthly office stationery supply request',  DATEADD(day, 7, GETDATE()), 'High',   'O', 1, GETDATE()),
        (2, 'IT Equipment', 2, 'Request for new laptops for the dev team',  DATEADD(day, 14, GETDATE()), 'High',  'O', 2, GETDATE()),
        (3, 'Furniture',   3, 'New chairs for the conference room',         DATEADD(day, 30, GETDATE()), 'Medium','O', 1, GETDATE()),
        (4, 'Stationery',  1, 'Printer paper refill - urgent',              DATEADD(day, 3, GETDATE()),  'High',  'A', 1, GETDATE()),
        (5, 'Cleaning',    4, 'Additional cleaning supplies',               DATEADD(day, 5, GETDATE()),  'Low',   'C', 2, GETDATE());

    SET IDENTITY_INSERT dbo.DEMAND_MASTER OFF;

    PRINT 'Seed data inserted into DEMAND_MASTER.';
END
ELSE
BEGIN
    PRINT 'Seed data already exists — skipping.';
END
GO
