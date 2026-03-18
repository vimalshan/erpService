# SQL Server Setup Script for Payroll Service

This script sets up the database and necessary objects for the Payroll Microservice.

## Prerequisites

- SQL Server 2019 or LocalDB
- SQL Server Management Studio (SSMS) or SQL Server Data Tools

## Setup Instructions

1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Create a new query and execute this script

```sql
-- Create database
CREATE DATABASE PAYDB;
GO

USE PAYDB;
GO

-- Create tables
-- These will be created by EF Core migrations, but here's the manual SQL for reference

-- Payroll Batch table
CREATE TABLE PAYROLL_BATCH (
    BATCH_ID BIGINT PRIMARY KEY,
    BATCH_MONTH VARCHAR(7) NOT NULL UNIQUE,
    BATCH_STATUS CHAR(1) NOT NULL,
    BATCH_CREATEDBY BIGINT NOT NULL,
    BATCH_CREATEDON DATETIME NOT NULL,
    BATCH_UPDATEDON DATETIME NULL,
    BATCH_UPDATEDBY BIGINT NULL
);

-- Payroll transaction table
CREATE TABLE PAY_TRANDET (
    TRN_ID BIGINT PRIMARY KEY IDENTITY(1,1),
    TRN_EMPSYSID BIGINT NOT NULL,
    TRN_BATCHID BIGINT NOT NULL,
    TRN_MONTH VARCHAR(7) NOT NULL,
    TRN_GROSS DECIMAL(19,0) NOT NULL,
    TRN_DEDUCTIONS DECIMAL(19,0) NOT NULL,
    TRN_NET DECIMAL(19,0) NOT NULL,
    TRN_STATUS CHAR(1) NOT NULL,
    TRN_CREATEDBY BIGINT NOT NULL,
    TRN_CREATEDON DATETIME NOT NULL,
    TRN_UPDATEDON DATETIME NULL,
    TRN_UPDATEDBY BIGINT NULL,
    CONSTRAINT FK_PAY_TRANDET_PAYROLL_BATCH FOREIGN KEY (TRN_BATCHID) REFERENCES PAYROLL_BATCH(BATCH_ID)
);

-- Payroll arrear/adjustment table
CREATE TABLE PAY_ARR (
    AR_ID BIGINT PRIMARY KEY,
    PAY_EMPSYSID BIGINT NOT NULL,
    AR_AMOUNT DECIMAL(19,0) NOT NULL,
    AR_TYPE CHAR(1) NOT NULL,
    AR_DATE DATETIME NOT NULL,
    AR_DESCRIPTION VARCHAR(500) NULL,
    AR_CREATEDBY BIGINT NOT NULL,
    AR_CREATEDON DATETIME NOT NULL,
    AR_APPROVEDON DATETIME NULL,
    AR_APPROVEDBY BIGINT NULL
);

-- Create indexes
CREATE INDEX idx_PAY_TRANDET_EmpMonth ON PAY_TRANDET(TRN_EMPSYSID, TRN_MONTH);
CREATE INDEX idx_PAY_TRANDET_BatchId ON PAY_TRANDET(TRN_BATCHID);
CREATE INDEX idx_PAY_ARR_EmpId ON PAY_ARR(PAY_EMPSYSID);
CREATE INDEX idx_PAY_ARR_EmpDate ON PAY_ARR(PAY_EMPSYSID, AR_DATE);

-- Seed data (optional)
-- INSERT INTO PAYROLL_BATCH VALUES (1, '2024-01', 'C', 1, GETDATE(), NULL, NULL);

PRINT 'Database setup completed successfully!'
```

## Connection String

For .NET applications use:

```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PAYDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name="Payroll Service";Command Timeout=0
```

## Restore Database

To restore from backup:

```sql
RESTORE DATABASE PAYDB FROM DISK = 'C:\Backup\PAYDB.bak'
WITH REPLACE;
```

## Backup Database

To create a backup:

```sql
BACKUP DATABASE PAYDB TO DISK = 'C:\Backup\PAYDB.bak';
```
