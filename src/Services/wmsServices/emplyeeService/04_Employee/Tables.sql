-- ======================================================
-- Module: Employee
-- Tables: Employees
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE Employee (
    employee_id     INT IDENTITY(1,1) PRIMARY KEY,
    user_id         INT NULL UNIQUE,
    first_name      NVARCHAR(50) NOT NULL,
    last_name       NVARCHAR(50) NOT NULL,
    employee_code   NVARCHAR(20) NOT NULL UNIQUE,
    hire_date       DATE NOT NULL,
    job_title       NVARCHAR(50),
    department      NVARCHAR(50),
    warehouse_id    INT NULL,
    phone           NVARCHAR(20),
    email           NVARCHAR(100),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(UserID) ON DELETE SET NULL,
    FOREIGN KEY (warehouse_id) REFERENCES Warehouse(warehouse_id) ON DELETE SET NULL
);
GO
