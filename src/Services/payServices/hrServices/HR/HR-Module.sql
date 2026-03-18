-- ==========================================
-- Module: HR (Human Resources)
-- Database: PAYDB
-- ==========================================

USE [PAYDB];
GO

-- ==========================================
-- LOOKUP/REFERENCE TABLES
-- ==========================================

-- Table: HR_INTLANGUAGECODE - Language Short Code for Search
CREATE TABLE [HR_INTLANGUAGECODE] (
    [LanguageCode] VARCHAR(10) PRIMARY KEY,
    [LanguageName] VARCHAR(255) NOT NULL,
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE()
);
GO

-- Table: PROFRATE_SITEMAP - Site/Location Management
CREATE TABLE [PROFRATE_SITEMAP] (
    [SiteId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [SiteName] VARCHAR(255) NOT NULL,
    [SiteCode] VARCHAR(50) UNIQUE NOT NULL,
    [Address] VARCHAR(500),
    [City] VARCHAR(100),
    [Country] VARCHAR(100),
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME DEFAULT GETUTCDATE()
);
GO

-- ==========================================
-- DEPARTMENT & ROLE MANAGEMENT
-- ==========================================

CREATE TABLE [HR_Department] (
    [DepartmentId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [DepartmentCode] VARCHAR(50) UNIQUE NOT NULL,
    [DepartmentName] VARCHAR(255) NOT NULL,
    [Description] VARCHAR(1000),
    [ManagerId] UNIQUEIDENTIFIER,
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [HR_Position] (
    [PositionId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [PositionCode] VARCHAR(50) UNIQUE NOT NULL,
    [PositionTitle] VARCHAR(255) NOT NULL,
    [Description] VARCHAR(1000),
    [DepartmentId] UNIQUEIDENTIFIER NOT NULL,
    [ReportsToPositionId] UNIQUEIDENTIFIER,
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME DEFAULT GETUTCDATE(),
    FOREIGN KEY ([DepartmentId]) REFERENCES [HR_Department]([DepartmentId])
);
GO

-- ==========================================
-- EMPLOYEE MANAGEMENT
-- ==========================================

CREATE TABLE [HR_Employee] (
    [EmployeeId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [EmployeeCode] VARCHAR(50) UNIQUE NOT NULL,
    [FirstName] VARCHAR(100) NOT NULL,
    [LastName] VARCHAR(100) NOT NULL,
    [MiddleName] VARCHAR(100),
    [DateOfBirth] DATE NOT NULL,
    [Gender] VARCHAR(10), -- M, F, Other
    [Email] VARCHAR(100) UNIQUE NOT NULL,
    [PhoneNumber] VARCHAR(20),
    [SSN] VARCHAR(50), -- Social Security Number
    [DepartmentId] UNIQUEIDENTIFIER NOT NULL,
    [PositionId] UNIQUEIDENTIFIER NOT NULL,
    [ManagerId] UNIQUEIDENTIFIER,
    [SiteId] UNIQUEIDENTIFIER NOT NULL,
    [JoinDate] DATE NOT NULL,
    [TerminationDate] DATE,
    [EmploymentStatus] VARCHAR(50) DEFAULT 'Active', -- Active, OnLeave, Terminated, Suspended
    [EmploymentType] VARCHAR(50), -- Permanent, Contract, Probation
    [ReportingManagerId] UNIQUEIDENTIFIER,
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME DEFAULT GETUTCDATE(),
    FOREIGN KEY ([DepartmentId]) REFERENCES [HR_Department]([DepartmentId]),
    FOREIGN KEY ([PositionId]) REFERENCES [HR_Position]([PositionId]),
    FOREIGN KEY ([SiteId]) REFERENCES [PROFRATE_SITEMAP]([SiteId]),
    FOREIGN KEY ([ManagerId]) REFERENCES [HR_Employee]([EmployeeId])
);
GO

-- ==========================================
-- LEAVE MANAGEMENT
-- ==========================================

CREATE TABLE [HR_LeaveType] (
    [LeaveTypeId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [LeaveTypeName] VARCHAR(100) NOT NULL,
    [MaxDaysPerYear] INT,
    [IsPaid] BIT DEFAULT 1,
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [HR_EmployeeLeave] (
    [LeaveId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL,
    [LeaveTypeId] UNIQUEIDENTIFIER NOT NULL,
    [StartDate] DATE NOT NULL,
    [EndDate] DATE NOT NULL,
    [NumberOfDays] INT,
    [Reason] VARCHAR(500),
    [Status] VARCHAR(50) DEFAULT 'Pending', -- Pending, Approved, Rejected, Cancelled
    [ApprovedBy] UNIQUEIDENTIFIER,
    [ApprovalDate] DATETIME,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME DEFAULT GETUTCDATE(),
    FOREIGN KEY ([EmployeeId]) REFERENCES [HR_Employee]([EmployeeId]),
    FOREIGN KEY ([LeaveTypeId]) REFERENCES [HR_LeaveType]([LeaveTypeId]),
    FOREIGN KEY ([ApprovedBy]) REFERENCES [HR_Employee]([EmployeeId])
);
GO

-- ==========================================
-- ATTENDANCE & SHIFT MANAGEMENT
-- ==========================================

CREATE TABLE [HR_Shift] (
    [ShiftId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [ShiftCode] VARCHAR(50) UNIQUE NOT NULL,
    [ShiftName] VARCHAR(100) NOT NULL,
    [StartTime] TIME NOT NULL,
    [EndTime] TIME NOT NULL,
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [HR_Attendance] (
    [AttendanceId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL,
    [AttendanceDate] DATE NOT NULL,
    [ShiftId] UNIQUEIDENTIFIER NOT NULL,
    [CheckInTime] TIME,
    [CheckOutTime] TIME,
    [Status] VARCHAR(50) DEFAULT 'Present', -- Present, Absent, Late, EarlyLeave
    [Remarks] VARCHAR(500),
    [CreatedDate] DATETIME DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME DEFAULT GETUTCDATE(),
    FOREIGN KEY ([EmployeeId]) REFERENCES [HR_Employee]([EmployeeId]),
    FOREIGN KEY ([ShiftId]) REFERENCES [HR_Shift]([ShiftId]),
    UNIQUE ([EmployeeId], [AttendanceDate])
);
GO

-- ==========================================
-- PERFORMANCE MANAGEMENT
-- ==========================================

CREATE TABLE [HR_PerformanceReview] (
    [ReviewId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL,
    [ReviewPeriodStart] DATE NOT NULL,
    [ReviewPeriodEnd] DATE NOT NULL,
    [Rating] DECIMAL(3, 2), -- 1-5 scale
    [Comments] VARCHAR(2000),
    [ReviewedBy] UNIQUEIDENTIFIER NOT NULL,
    [Status] VARCHAR(50) DEFAULT 'Draft', -- Draft, Submitted, Approved
    [ReviewDate] DATETIME,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME DEFAULT GETUTCDATE(),
    FOREIGN KEY ([EmployeeId]) REFERENCES [HR_Employee]([EmployeeId]),
    FOREIGN KEY ([ReviewedBy]) REFERENCES [HR_Employee]([EmployeeId])
);
GO

-- ==========================================
-- COMPENSATION & SALARY
-- ==========================================

CREATE TABLE [HR_SalaryComponent] (
    [SalaryComponentId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [ComponentName] VARCHAR(100) NOT NULL,
    [ComponentType] VARCHAR(50), -- Basic, HRA, DA, Allowance, Deduction
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [HR_EmployeeSalary] (
    [SalaryId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL,
    [EffectiveDate] DATE NOT NULL,
    [TotalBaseSalary] DECIMAL(18, 2),
    [Status] VARCHAR(50) DEFAULT 'Active', -- Active, Inactive, Revised
    [CreatedDate] DATETIME DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME DEFAULT GETUTCDATE(),
    FOREIGN KEY ([EmployeeId]) REFERENCES [HR_Employee]([EmployeeId])
);
GO

CREATE TABLE [HR_EmployeeSalaryDetail] (
    [SalaryDetailId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [SalaryId] UNIQUEIDENTIFIER NOT NULL,
    [SalaryComponentId] UNIQUEIDENTIFIER NOT NULL,
    [Amount] DECIMAL(18, 2),
    FOREIGN KEY ([SalaryId]) REFERENCES [HR_EmployeeSalary]([SalaryId]),
    FOREIGN KEY ([SalaryComponentId]) REFERENCES [HR_SalaryComponent]([SalaryComponentId])
);
GO

-- ==========================================
-- EVENT AUDIT TRAIL
-- ==========================================

CREATE TABLE [HR_AuditLog] (
    [AuditLogId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [EntityType] VARCHAR(100) NOT NULL,
    [EntityId] UNIQUEIDENTIFIER NOT NULL,
    [Action] VARCHAR(50) NOT NULL, -- Created, Updated, Deleted
    [ChangedBy] UNIQUEIDENTIFIER,
    [OldValues] VARCHAR(MAX),
    [NewValues] VARCHAR(MAX),
    [CreatedDate] DATETIME DEFAULT GETUTCDATE()
);
GO

-- ==========================================
-- INDEXES FOR PERFORMANCE
-- ==========================================

CREATE INDEX IDX_Employee_DepartmentId ON [HR_Employee]([DepartmentId]);
CREATE INDEX IDX_Employee_PositionId ON [HR_Employee]([PositionId]);
CREATE INDEX IDX_Employee_ManagerId ON [HR_Employee]([ManagerId]);
CREATE INDEX IDX_Employee_Email ON [HR_Employee]([Email]);
CREATE INDEX IDX_Attendance_EmployeeId ON [HR_Attendance]([EmployeeId], [AttendanceDate]);
CREATE INDEX IDX_Leave_EmployeeId ON [HR_EmployeeLeave]([EmployeeId], [Status]);
CREATE INDEX IDX_AuditLog_EntityId ON [HR_AuditLog]([EntityType], [EntityId], [CreatedDate]);
GO
