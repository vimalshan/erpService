-- ============================================================================
-- HR Database Seed Data Script
-- Date: March 17, 2026
-- Purpose: Populate initial reference data for HR Management System
-- Usage: Run after initial database creation
-- ============================================================================

USE PAYDB;
GO

-- ============================================================================
-- Seed HR_Department
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM HR_Department WHERE DepartmentCode = 'HR')
BEGIN
    INSERT INTO HR_Department (Id, DepartmentCode, DepartmentName, Description, ManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000001'), 'HR', 'Human Resources', 'Human Resources Department', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Department WHERE DepartmentCode = 'IT')
BEGIN
    INSERT INTO HR_Department (Id, DepartmentCode, DepartmentName, Description, ManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000002'), 'IT', 'Information Technology', 'IT Department', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Department WHERE DepartmentCode = 'FIN')
BEGIN
    INSERT INTO HR_Department (Id, DepartmentCode, DepartmentName, Description, ManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000003'), 'FIN', 'Finance', 'Finance Department', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Department WHERE DepartmentCode = 'OPS')
BEGIN
    INSERT INTO HR_Department (Id, DepartmentCode, DepartmentName, Description, ManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000004'), 'OPS', 'Operations', 'Operations Department', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

-- ============================================================================
-- Seed HR_Shift
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM HR_Shift WHERE ShiftCode = 'SHIFT_A')
BEGIN
    INSERT INTO HR_Shift (Id, ShiftCode, ShiftName, StartTime, EndTime, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '20000000-0000-0000-0000-000000000001'), 'SHIFT_A', 'Morning Shift', CAST('08:00:00' AS TIME), CAST('16:00:00' AS TIME), 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Shift WHERE ShiftCode = 'SHIFT_B')
BEGIN
    INSERT INTO HR_Shift (Id, ShiftCode, ShiftName, StartTime, EndTime, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '20000000-0000-0000-0000-000000000002'), 'SHIFT_B', 'Afternoon Shift', CAST('14:00:00' AS TIME), CAST('22:00:00' AS TIME), 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Shift WHERE ShiftCode = 'SHIFT_C')
BEGIN
    INSERT INTO HR_Shift (Id, ShiftCode, ShiftName, StartTime, EndTime, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '20000000-0000-0000-0000-000000000003'), 'SHIFT_C', 'Night Shift', CAST('22:00:00' AS TIME), CAST('06:00:00' AS TIME), 1, GETUTCDATE(), GETUTCDATE(), 0);
END

-- ============================================================================
-- Seed HR_Position
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM HR_Position WHERE PositionCode = 'HR001')
BEGIN
    INSERT INTO HR_Position (Id, PositionCode, PositionTitle, Description, DepartmentId, ReportsToPositionId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000001'), 'HR001', 'HR Manager', 'Manages HR operations', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000001'), NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Position WHERE PositionCode = 'HR002')
BEGIN
    INSERT INTO HR_Position (Id, PositionCode, PositionTitle, Description, DepartmentId, ReportsToPositionId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000002'), 'HR002', 'HR Specialist', 'Handles recruitment and employee relations', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000001'), CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000001'), 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Position WHERE PositionCode = 'IT001')
BEGIN
    INSERT INTO HR_Position (Id, PositionCode, PositionTitle, Description, DepartmentId, ReportsToPositionId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000003'), 'IT001', 'IT Director', 'Manages IT department', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000002'), NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Position WHERE PositionCode = 'IT002')
BEGIN
    INSERT INTO HR_Position (Id, PositionCode, PositionTitle, Description, DepartmentId, ReportsToPositionId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000004'), 'IT002', 'Senior Developer', 'Develops software solutions', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000002'), CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000003'), 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Position WHERE PositionCode = 'FIN001')
BEGIN
    INSERT INTO HR_Position (Id, PositionCode, PositionTitle, Description, DepartmentId, ReportsToPositionId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000005'), 'FIN001', 'Finance Manager', 'Manages financial operations', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000003'), NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

-- ============================================================================
-- Seed HR_LeaveType
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM HR_LeaveType WHERE LeaveTypeName = 'Annual Leave')
BEGIN
    INSERT INTO HR_LeaveType (Id, LeaveTypeName, MaxDaysPerYear, IsPaid, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '40000000-0000-0000-0000-000000000001'), 'Annual Leave', 20, 1, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_LeaveType WHERE LeaveTypeName = 'Sick Leave')
BEGIN
    INSERT INTO HR_LeaveType (Id, LeaveTypeName, MaxDaysPerYear, IsPaid, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '40000000-0000-0000-0000-000000000002'), 'Sick Leave', 12, 1, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_LeaveType WHERE LeaveTypeName = 'Maternity Leave')
BEGIN
    INSERT INTO HR_LeaveType (Id, LeaveTypeName, MaxDaysPerYear, IsPaid, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '40000000-0000-0000-0000-000000000003'), 'Maternity Leave', 90, 1, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_LeaveType WHERE LeaveTypeName = 'Unpaid Leave')
BEGIN
    INSERT INTO HR_LeaveType (Id, LeaveTypeName, MaxDaysPerYear, IsPaid, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '40000000-0000-0000-0000-000000000004'), 'Unpaid Leave', NULL, 0, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

-- ============================================================================
-- Seed HR_SalaryComponent
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM HR_SalaryComponent WHERE ComponentName = 'Basic Salary')
BEGIN
    INSERT INTO HR_SalaryComponent (Id, ComponentName, ComponentType, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '50000000-0000-0000-0000-000000000001'), 'Basic Salary', 'Earning', 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_SalaryComponent WHERE ComponentName = 'HRA')
BEGIN
    INSERT INTO HR_SalaryComponent (Id, ComponentName, ComponentType, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '50000000-0000-0000-0000-000000000002'), 'HRA', 'Earning', 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_SalaryComponent WHERE ComponentName = 'Dearness Allowance')
BEGIN
    INSERT INTO HR_SalaryComponent (Id, ComponentName, ComponentType, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '50000000-0000-0000-0000-000000000003'), 'Dearness Allowance', 'Earning', 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_SalaryComponent WHERE ComponentName = 'Income Tax')
BEGIN
    INSERT INTO HR_SalaryComponent (Id, ComponentName, ComponentType, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '50000000-0000-0000-0000-000000000004'), 'Income Tax', 'Deduction', 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_SalaryComponent WHERE ComponentName = 'PF Contribution')
BEGIN
    INSERT INTO HR_SalaryComponent (Id, ComponentName, ComponentType, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '50000000-0000-0000-0000-000000000005'), 'PF Contribution', 'Deduction', 1, GETUTCDATE(), GETUTCDATE(), 0);
END

-- ============================================================================
-- Seed HR_Employee
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM HR_Employee WHERE EmployeeCode = 'EMP001')
BEGIN
    INSERT INTO HR_Employee (Id, EmployeeCode, FirstName, LastName, MiddleName, DateOfBirth, Gender, Email, PhoneNumber, SSN, DepartmentId, PositionId, ManagerId, SiteId, JoinDate, TerminationDate, Status, EmploymentType, ReportingManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '60000000-0000-0000-0000-000000000001'), 'EMP001', 'John', 'Smith', 'David', CAST('1985-05-15' AS DATE), 'Male', 'john.smith@company.com', '+1-555-0101', '123-45-6789', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000001'), CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000001'), NULL, CONVERT(UNIQUEIDENTIFIER, '70000000-0000-0000-0000-000000000001'), CAST('2020-03-01' AS DATE), NULL, 'Active', 'Permanent', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Employee WHERE EmployeeCode = 'EMP002')
BEGIN
    INSERT INTO HR_Employee (Id, EmployeeCode, FirstName, LastName, MiddleName, DateOfBirth, Gender, Email, PhoneNumber, SSN, DepartmentId, PositionId, ManagerId, SiteId, JoinDate, TerminationDate, Status, EmploymentType, ReportingManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '60000000-0000-0000-0000-000000000002'), 'EMP002', 'William', 'Johnson', 'Robert', CAST('1982-08-22' AS DATE), 'Male', 'william.johnson@company.com', '+1-555-0102', '987-65-4321', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000002'), CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000003'), NULL, CONVERT(UNIQUEIDENTIFIER, '70000000-0000-0000-0000-000000000001'), CAST('2019-01-15' AS DATE), NULL, 'Active', 'Permanent', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Employee WHERE EmployeeCode = 'EMP003')
BEGIN
    INSERT INTO HR_Employee (Id, EmployeeCode, FirstName, LastName, MiddleName, DateOfBirth, Gender, Email, PhoneNumber, SSN, DepartmentId, PositionId, ManagerId, SiteId, JoinDate, TerminationDate, Status, EmploymentType, ReportingManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '60000000-0000-0000-0000-000000000003'), 'EMP003', 'Mary', 'Williams', 'Elizabeth', CAST('1988-03-10' AS DATE), 'Female', 'mary.williams@company.com', '+1-555-0103', '456-78-9123', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000003'), CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000005'), NULL, CONVERT(UNIQUEIDENTIFIER, '70000000-0000-0000-0000-000000000001'), CAST('2021-06-01' AS DATE), NULL, 'Active', 'Permanent', NULL, 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Employee WHERE EmployeeCode = 'EMP004')
BEGIN
    INSERT INTO HR_Employee (Id, EmployeeCode, FirstName, LastName, MiddleName, DateOfBirth, Gender, Email, PhoneNumber, SSN, DepartmentId, PositionId, ManagerId, SiteId, JoinDate, TerminationDate, Status, EmploymentType, ReportingManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '60000000-0000-0000-0000-000000000004'), 'EMP004', 'James', 'Brown', 'Michael', CAST('1990-07-25' AS DATE), 'Male', 'james.brown@company.com', '+1-555-0104', '789-01-2345', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000002'), CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000004'), CONVERT(UNIQUEIDENTIFIER, '60000000-0000-0000-0000-000000000002'), CONVERT(UNIQUEIDENTIFIER, '70000000-0000-0000-0000-000000000001'), CAST('2022-09-15' AS DATE), NULL, 'Active', 'Permanent', CONVERT(UNIQUEIDENTIFIER, '60000000-0000-0000-0000-000000000002'), 1, GETUTCDATE(), GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM HR_Employee WHERE EmployeeCode = 'EMP005')
BEGIN
    INSERT INTO HR_Employee (Id, EmployeeCode, FirstName, LastName, MiddleName, DateOfBirth, Gender, Email, PhoneNumber, SSN, DepartmentId, PositionId, ManagerId, SiteId, JoinDate, TerminationDate, Status, EmploymentType, ReportingManagerId, IsActive, CreatedDate, ModifiedDate, ConcurrencyStamp)
    VALUES (CONVERT(UNIQUEIDENTIFIER, '60000000-0000-0000-0000-000000000005'), 'EMP005', 'Patricia', 'Davis', 'Ann', CAST('1992-11-30' AS DATE), 'Female', 'patricia.davis@company.com', '+1-555-0105', '321-54-6789', CONVERT(UNIQUEIDENTIFIER, '10000000-0000-0000-0000-000000000001'), CONVERT(UNIQUEIDENTIFIER, '30000000-0000-0000-0000-000000000002'), CONVERT(UNIQUEIDENTIFIER, '60000000-0000-0000-0000-000000000001'), CONVERT(UNIQUEIDENTIFIER, '70000000-0000-0000-0000-000000000001'), CAST('2023-02-20' AS DATE), NULL, 'Active', 'Contract', CONVERT(UNIQUEIDENTIFIER, '60000000-0000-0000-0000-000000000001'), 1, GETUTCDATE(), GETUTCDATE(), 0);
END

-- ============================================================================
-- Verification Queries
-- ============================================================================
PRINT 'Seed data verification:';
PRINT '=====================';
SELECT 'Departments' AS TableName, COUNT(*) AS RecordCount FROM HR_Department;
SELECT 'Shifts' AS TableName, COUNT(*) AS RecordCount FROM HR_Shift;
SELECT 'Positions' AS TableName, COUNT(*) AS RecordCount FROM HR_Position;
SELECT 'Leave Types' AS TableName, COUNT(*) AS RecordCount FROM HR_LeaveType;
SELECT 'Salary Components' AS TableName, COUNT(*) AS RecordCount FROM HR_SalaryComponent;
SELECT 'Employees' AS TableName, COUNT(*) AS RecordCount FROM HR_Employee;

PRINT 'Seed data script completed successfully.';
GO
