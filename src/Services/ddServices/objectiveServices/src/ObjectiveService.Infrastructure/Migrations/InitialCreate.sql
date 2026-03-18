-- Initial Seed Data for Objective Service
-- Database: DDDB

USE DDDB;
GO

-- Disable constraints for bulk insert
DISABLE TRIGGER ALL ON dbo.Employees;
DISABLE TRIGGER ALL ON dbo.ControlPoints;
DISABLE TRIGGER ALL ON dbo.Goals;
DISABLE TRIGGER ALL ON dbo.GoalSubGoals;
DISABLE TRIGGER ALL ON dbo.ControlPointRequests;

-- Insert sample employees
IF NOT EXISTS (SELECT 1 FROM dbo.Employees WHERE UserId = 'EMP001')
BEGIN
    INSERT INTO dbo.Employees (Id, UserId, PinNumber, EmployeeSysId, Department, Status, CreatedDate)
    VALUES 
        (1, 'EMP001', 10001, 1001, 'Operations', 'A', GETUTCDATE()),
        (2, 'EMP002', 10002, 1002, 'Finance', 'A', GETUTCDATE()),
        (3, 'EMP003', 10003, 1003, 'HR', 'A', GETUTCDATE()),
        (4, 'EMP004', 10004, 1004, 'IT', 'A', GETUTCDATE()),
        (5, 'EMP005', 10005, 1005, 'Sales', 'A', GETUTCDATE());
END;

-- Insert sample control points
IF NOT EXISTS (SELECT 1 FROM dbo.ControlPoints WHERE EmployeeSysId = 1001)
BEGIN
    INSERT INTO dbo.ControlPoints 
    (Id, EmployeeSysId, DDYearId, Source, RefId, SerialNumber, Description, Category, UnitOfMeasurement, 
     UnitFrom, UnitTo, VersionNumber, Weightage, Status, ModifiedDate)
    VALUES 
        (1, 1001, 2024, 'DD', 101, 1, 'Process Efficiency', 'Performance', '%', '80%', '95%', 1, 20, 'A', GETUTCDATE()),
        (2, 1001, 2024, 'DD', 102, 2, 'Customer Satisfaction', 'Quality', 'Score', '7/10', '9/10', 1, 15, 'A', GETUTCDATE()),
        (3, 1002, 2024, 'DD', 201, 1, 'Revenue Growth', 'Financial', '%', '5%', '15%', 1, 25, 'A', GETUTCDATE()),
        (4, 1002, 2024, 'DD', 202, 2, 'Cost Optimization', 'Financial', '%', '2%', '8%', 1, 20, 'A', GETUTCDATE());
END;

-- Insert sample goals
IF NOT EXISTS (SELECT 1 FROM dbo.Goals WHERE UserId = 'EMP001')
BEGIN
    INSERT INTO dbo.Goals 
    (Id, UserId, PinNumber, PeriodFrom, PeriodTo, ReferenceNumber, FormFlag, Status, HasAttachment, CreatedDate)
    VALUES 
        (1, 'EMP001', 10001, '2024-01-01', '2024-12-31', 2024001, 'D', 'N', 0, GETUTCDATE()),
        (2, 'EMP002', 10002, '2024-01-01', '2024-12-31', 2024002, 'D', 'N', 0, GETUTCDATE()),
        (3, 'EMP003', 10003, '2024-01-01', '2024-12-31', 2024003, 'D', 'N', 0, GETUTCDATE());
END;

-- Insert sample goal sub goals
IF NOT EXISTS (SELECT 1 FROM dbo.GoalSubGoals WHERE GoalId = 1)
BEGIN
    INSERT INTO dbo.GoalSubGoals 
    (Id, GoalId, Description, UnitFrom, UnitTo, UnitOfMeasurement, Category)
    VALUES 
        (1, 1, 'Improve Process Efficiency', '80%', '95%', '%', 'Performance'),
        (2, 1, 'Enhance Customer Satisfaction', '7/10', '9/10', 'Score', 'Quality'),
        (3, 2, 'Increase Revenue', '5%', '15%', '%', 'Financial'),
        (4, 2, 'Optimize Costs', '2%', '8%', '%', 'Financial');
END;

-- Insert sample control point requests
IF NOT EXISTS (SELECT 1 FROM dbo.ControlPointRequests WHERE EmployeeSysId = 1001)
BEGIN
    INSERT INTO dbo.ControlPointRequests 
    (Id, EmployeeSysId, DDYearId, CreatedOn, Status)
    VALUES 
        (1, 1001, 2024, GETUTCDATE(), 'N'),
        (2, 1002, 2024, GETUTCDATE(), 'N'),
        (3, 1003, 2024, GETUTCDATE(), 'N');
END;

-- Re-enable constraints
ENABLE TRIGGER ALL ON dbo.Employees;
ENABLE TRIGGER ALL ON dbo.ControlPoints;
ENABLE TRIGGER ALL ON dbo.Goals;
ENABLE TRIGGER ALL ON dbo.GoalSubGoals;
ENABLE TRIGGER ALL ON dbo.ControlPointRequests;

PRINT 'Seed data inserted successfully.';
GO
