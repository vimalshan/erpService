-- =====================================================
-- Employee Service Database Seed Script
-- Database: PAYDB
-- Purpose: Initialize sample data for testing
-- =====================================================

-- Insert sample employees
INSERT INTO [Employees] 
(EmployeeSystemId, FirstName, LastName, MiddleName, Email, PhoneNumber, EmployeeCode, CostCenterId, 
 GrossCTC, GrossCTC_Currency, BasicSalary, BasicSalary_Currency, CTCEffectiveDate, 
 EmploymentStatus, JoiningDate, TerminationDate, LastCTCModificationDate, 
 CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, IsDeleted)
VALUES
-- Employee 1: Rajesh Kumar Singh
(1001, 'Rajesh', 'Kumar', 'Singh', 'rajesh.kumar@example.com', '+91 9876543210', 'EMP001', 'CC001',
 600000, 'INR', 300000, 'INR', '2020-01-15', 'Active', '2020-01-15', NULL, '2024-01-01', 
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0),

-- Employee 2: Priya Sharma Tanvi
(1002, 'Priya', 'Sharma', 'Tanvi', 'priya.sharma@example.com', '+91 9876543211', 'EMP002', 'CC002',
 550000, 'INR', 275000, 'INR', '2021-03-22', 'Active', '2021-03-22', NULL, '2024-01-01', 
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0),

-- Employee 3: Amit Patel Kumar
(1003, 'Amit', 'Patel', 'Kumar', 'amit.patel@example.com', '+91 9876543212', 'EMP003', 'CC001',
 750000, 'INR', 375000, 'INR', '2019-06-10', 'Active', '2019-06-10', NULL, '2024-01-01', 
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0),

-- Employee 4: Neha Gupta Rani
(1004, 'Neha', 'Gupta', 'Rani', 'neha.gupta@example.com', '+91 9876543213', 'EMP004', 'CC003',
 500000, 'INR', 250000, 'INR', '2022-02-14', 'Active', '2022-02-14', NULL, '2024-01-01', 
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0),

-- Employee 5: Vikram Singh Rajendra
(1005, 'Vikram', 'Singh', 'Rajendra', 'vikram.singh@example.com', '+91 9876543214', 'EMP005', 'CC002',
 650000, 'INR', 325000, 'INR', '2021-09-01', 'Active', '2021-09-01', NULL, '2024-01-01', 
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0);

-- Insert salary increment logs
INSERT INTO [SalaryIncrementLogs]
(EmployeeSystemId, OldCTC, OldCTC_Currency, NewCTC, NewCTC_Currency, IncrementPercentage, 
 EffectiveDate, ApprovedBy, ApprovedOn, ApprovalComments, Status, 
 CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, IsDeleted)
VALUES
-- Increment 1: Rajesh Kumar - 2023 Annual Increment
(1001, 600000, 'INR', 660000, 'INR', 10.00,
 '2023-04-01', 5001, '2023-03-20', 'Annual increment 2023', 'Approved',
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0),

-- Increment 2: Priya Sharma - 2023 Annual Increment
(1002, 550000, 'INR', 605000, 'INR', 10.00,
 '2023-04-01', 5001, '2023-03-20', 'Annual increment 2023', 'Approved',
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0),

-- Increment 3: Amit Patel - 2023 Annual Increment
(1003, 750000, 'INR', 825000, 'INR', 10.00,
 '2023-04-01', 5001, '2023-03-20', 'Annual increment 2023', 'Approved',
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0),

-- Increment 4: Rajesh Kumar - 2024 Annual Increment
(1001, 660000, 'INR', 726000, 'INR', 10.00,
 '2024-04-01', 5001, '2024-03-20', 'Annual increment 2024', 'Approved',
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0),

-- Increment 5: Vikram Singh - 2024 Annual Increment
(1005, 650000, 'INR', 715000, 'INR', 10.00,
 '2024-04-01', 5001, '2024-03-20', 'Annual increment 2024', 'Approved',
 GETUTCDATE(), NULL, 'SYSTEM', NULL, 0);

-- Print confirmation
PRINT 'Database seed completed successfully!';
PRINT 'Total Employees: 5';
PRINT 'Total Salary Increment Logs: 5';
