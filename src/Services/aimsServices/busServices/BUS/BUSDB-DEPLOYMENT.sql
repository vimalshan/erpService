-- ==========================================
-- Database: BUSDB
-- Module: Bus Transport Management System
-- Generated Module Script
-- ==========================================

USE master;
GO

-- ==========================================
-- PHASE 1: Database Creation
-- ==========================================
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'BUSDB')
    DROP DATABASE BUSDB;
GO

CREATE DATABASE BUSDB
ON PRIMARY (
    NAME = 'BUSDB_Data',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\BUSDB_Data.mdf',
    SIZE = 100MB,
    MAXSIZE = 500MB,
    FILEGROWTH = 10MB
)
LOG ON (
    NAME = 'BUSDB_Log',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15\MSSQL\DATA\BUSDB_Log.ldf',
    SIZE = 40MB,
    MAXSIZE = 150MB,
    FILEGROWTH = 5MB
);
GO

PRINT '=== PHASE 1 COMPLETE: Database Created ===';
GO

-- ==========================================
-- PHASE 2: Table Definitions
-- ==========================================
USE BUSDB;
GO

-- Table: BUS_MASTER - Bus Information Master
CREATE TABLE [BUS_MASTER] (
    [BUS_ID] INT NOT NULL,
    [BUS_REGNUM] VARCHAR(50) NOT NULL,
    [BUS_DESCRIPTION] VARCHAR(255) NULL,
    [BUS_CAPACITY] INT NOT NULL,
    [BUS_OPERATINGFROM] DATETIME2(3) NOT NULL,
    [BUS_CAPACITY_RESERVED] INT NULL,
    [BUS_LASTMODIFIEDBY] BIGINT NOT NULL,
    [BUS_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_BUS_MASTER] PRIMARY KEY ([BUS_ID]),
    CONSTRAINT [UQ_BUS_REGNUM] UNIQUE ([BUS_REGNUM])
);

-- Table: BUSROUTE_MASTER - Bus Route Master
CREATE TABLE [BUSROUTE_MASTER] (
    [ROUTE_ID] INT NOT NULL,
    [ROUTE_BUS_ID] INT NOT NULL,
    [ROUTE_NAME] VARCHAR(100) NOT NULL,
    [ROUTE_DESCRIPTION] VARCHAR(255) NULL,
    [ROUTE_STATUS] CHAR(1) NOT NULL,
    [ROUTE_LASTMODIFIEDBY] BIGINT NOT NULL,
    [ROUTE_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_BUSROUTE_MASTER] PRIMARY KEY ([ROUTE_ID]),
    CONSTRAINT [FK_BUSROUTE_BUS] FOREIGN KEY ([ROUTE_BUS_ID]) REFERENCES [BUS_MASTER]([BUS_ID])
);

-- Table: EMPLOYEE_BUS - Employee Bus Assignment
CREATE TABLE [EMPLOYEE_BUS] (
    [EMPBUS_ID] BIGINT NOT NULL,
    [EMPBUS_EMPSYSID] BIGINT NOT NULL,
    [EMPBUS_BUSID] INT NOT NULL,
    [EMPBUS_ROUTEID] INT NOT NULL,
    [EMPBUS_EFFDATE] DATETIME2(3) NOT NULL,
    [EMPBUS_CLSDATE] DATETIME2(3) NULL,
    [EMPBUS_LASTMODIFIEDBY] BIGINT NOT NULL,
    [EMPBUS_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_EMPLOYEE_BUS] PRIMARY KEY ([EMPBUS_ID]),
    CONSTRAINT [FK_EMPLOYEE_BUS_BUS] FOREIGN KEY ([EMPBUS_BUSID]) REFERENCES [BUS_MASTER]([BUS_ID]),
    CONSTRAINT [FK_EMPLOYEE_BUS_ROUTE] FOREIGN KEY ([EMPBUS_ROUTEID]) REFERENCES [BUSROUTE_MASTER]([ROUTE_ID])
);

-- Table: BUS_ARRIVALDET - Bus Arrival Details (Daily Tracking)
CREATE TABLE [BUS_ARRIVALDET] (
    [ARRIVAL_ID] BIGINT NOT NULL,
    [ARRIVAL_BUS_ID] INT NOT NULL,
    [ARRIVAL_DATE] DATETIME2(3) NOT NULL,
    [ARRIVAL_TIME] TIME NOT NULL,
    [ARRIVAL_STATUS] CHAR(1) NOT NULL,
    [ARRIVAL_REMARKS] VARCHAR(255) NULL,
    [ARRIVAL_LASTMODIFIEDBY] BIGINT NOT NULL,
    [ARRIVAL_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_BUS_ARRIVALDET] PRIMARY KEY ([ARRIVAL_ID]),
    CONSTRAINT [FK_ARRIVAL_BUS] FOREIGN KEY ([ARRIVAL_BUS_ID]) REFERENCES [BUS_MASTER]([BUS_ID])
);

-- Table: BUSDEDUCTION_RATEMAST - Bus Deduction Rate Master
CREATE TABLE [BUSDEDUCTION_RATEMAST] (
    [DEDUCT_ID] INT NOT NULL,
    [DEDUCT_BUSID] INT NOT NULL,
    [DEDUCT_AMOUNT] DECIMAL(10,2) NOT NULL,
    [DEDUCT_EFFDATE] DATETIME2(3) NOT NULL,
    [DEDUCT_CLSDATE] DATETIME2(3) NULL,
    [DEDUCT_LASTMODIFIEDBY] BIGINT NOT NULL,
    [DEDUCT_LASTMODIFIEDON] DATETIME2(3) NOT NULL,
    CONSTRAINT [PK_BUSDEDUCTION_RATEMAST] PRIMARY KEY ([DEDUCT_ID]),
    CONSTRAINT [FK_DEDUCT_BUS] FOREIGN KEY ([DEDUCT_BUSID]) REFERENCES [BUS_MASTER]([BUS_ID])
);

-- Create Indexes
CREATE INDEX [IX_BUS_REGNUM] ON [BUS_MASTER] ([BUS_REGNUM]);
CREATE INDEX [IX_BUSROUTE_BUSID] ON [BUSROUTE_MASTER] ([ROUTE_BUS_ID]);
CREATE INDEX [IX_EMPLOYEE_BUS_EMPSYSID] ON [EMPLOYEE_BUS] ([EMPBUS_EMPSYSID]);
CREATE INDEX [IX_BUS_ARRIVALDET_BUSID] ON [BUS_ARRIVALDET] ([ARRIVAL_BUS_ID]);
CREATE INDEX [IX_BUS_ARRIVALDET_DATE] ON [BUS_ARRIVALDET] ([ARRIVAL_DATE]);

PRINT '=== PHASE 2 COMPLETE: Tables Created ===';
GO

-- ==========================================
-- PHASE 3: Stored Procedures
-- ==========================================

-- Procedure: usp_RegisterBus
CREATE OR ALTER PROCEDURE dbo.usp_RegisterBus
(
    @p_RegNumber VARCHAR(50),
    @p_Description VARCHAR(255),
    @p_Capacity INT,
    @p_RegisteredBy BIGINT,
    @p_BusID INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF EXISTS (SELECT 1 FROM BUS_MASTER WHERE BUS_REGNUM = @p_RegNumber)
            THROW 50001, 'Bus already registered with this registration number', 1;
        
        SELECT @p_BusID = ISNULL(MAX(BUS_ID), 0) + 1 FROM BUS_MASTER;
        
        INSERT INTO BUS_MASTER
        (BUS_ID, BUS_REGNUM, BUS_DESCRIPTION, BUS_CAPACITY, BUS_OPERATINGFROM, BUS_LASTMODIFIEDBY, BUS_LASTMODIFIEDON)
        VALUES (@p_BusID, @p_RegNumber, @p_Description, @p_Capacity, GETDATE(), @p_RegisteredBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'Bus registered successfully: ID = ' + CAST(@p_BusID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Bus registration failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- Procedure: usp_AssignEmployeeToBus
CREATE OR ALTER PROCEDURE dbo.usp_AssignEmployeeToBus
(
    @p_EmpSysID BIGINT,
    @p_BusID INT,
    @p_RouteID INT,
    @p_AssignedBy BIGINT,
    @p_AssignmentID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verify bus exists
        IF NOT EXISTS (SELECT 1 FROM BUS_MASTER WHERE BUS_ID = @p_BusID)
            THROW 50002, 'Bus ID does not exist', 1;
        
        -- Verify route exists and belongs to bus
        IF NOT EXISTS (SELECT 1 FROM BUSROUTE_MASTER WHERE ROUTE_ID = @p_RouteID AND ROUTE_BUS_ID = @p_BusID)
            THROW 50003, 'Route does not exist for this bus', 1;
        
        SELECT @p_AssignmentID = ISNULL(MAX(EMPBUS_ID), 0) + 1 FROM EMPLOYEE_BUS;
        
        INSERT INTO EMPLOYEE_BUS
        (EMPBUS_ID, EMPBUS_EMPSYSID, EMPBUS_BUSID, EMPBUS_ROUTEID, EMPBUS_EFFDATE, EMPBUS_LASTMODIFIEDBY, EMPBUS_LASTMODIFIEDON)
        VALUES (@p_AssignmentID, @p_EmpSysID, @p_BusID, @p_RouteID, GETDATE(), @p_AssignedBy, GETDATE());
        
        COMMIT TRANSACTION;
        PRINT 'Employee assigned to bus: Assignment ID = ' + CAST(@p_AssignmentID AS VARCHAR);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Employee assignment to bus failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

PRINT '=== PHASE 3 COMPLETE: Procedures Created ===';
GO

-- ==========================================
-- PHASE 4: Verification Views
-- ==========================================
CREATE VIEW vw_BusDB_Status AS
SELECT 
    'BUSDB' AS DatabaseName,
    'Bus Transport Management Module' AS ModuleDescription,
    (SELECT COUNT(*) FROM BUS_MASTER) AS RegisteredBuses,
    (SELECT COUNT(*) FROM BUSROUTE_MASTER) AS BusRoutes,
    (SELECT COUNT(*) FROM EMPLOYEE_BUS) AS EmployeeAssignments,
    (SELECT COUNT(*) FROM BUS_ARRIVALDET) AS ArrivalRecords,
    GETDATE() AS LastChecked;
GO

PRINT '=== PHASE 4 COMPLETE: Views Created ===';
GO

-- ==========================================
-- PHASE 5: Final Verification
-- ==========================================
PRINT '=== PHASE 5: FINAL VERIFICATION ===';
SELECT 'System Tables' AS Object_Type, COUNT(*) AS Count FROM sys.tables;
SELECT 'Indexes' AS Object_Type, COUNT(*) AS Count FROM sys.indexes WHERE object_id > 0;
SELECT 'Procedures' AS Object_Type, COUNT(*) AS Count FROM sys.objects WHERE type = 'P';
SELECT 'Views' AS Object_Type, COUNT(*) AS Count FROM sys.views;

PRINT '======================================';
PRINT 'BUSDB DEPLOYMENT COMPLETE';
PRINT '======================================';
PRINT 'Database: BUSDB';
PRINT 'Tables: 5';
PRINT 'Indexes: 5';
PRINT 'Procedures: 2';
PRINT 'Views: 1';
PRINT 'Status: READY FOR DEPLOYMENT';
PRINT '======================================';
GO
