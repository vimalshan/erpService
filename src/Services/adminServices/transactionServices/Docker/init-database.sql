-- ============================================================
-- TransactionService Database Initialization Script
-- Creates the ADMINDB database and transaction-related tables
-- ============================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ADMINDB')
BEGIN
    CREATE DATABASE ADMINDB;
END
GO

USE ADMINDB;
GO

-- Location Admin
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_LOCATION_ADMIN')
BEGIN
    CREATE TABLE SP_LOCATION_ADMIN (
        LA_LOC_ID       INT           NOT NULL,
        LA_LOC_NAME     NVARCHAR(255) NOT NULL
    );
END
GO

-- Category Default
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_CATEGORY_DEFAULT')
BEGIN
    CREATE TABLE SP_CATEGORY_DEFAULT (
        CD_CATEGORY_ID       INT           NOT NULL,
        CD_CATEGORY_NAME     NVARCHAR(255) NOT NULL,
        CD_SUB_CATEGORY_ID   INT           NOT NULL,
        CD_SUB_CATEGORY_NAME NVARCHAR(255) NOT NULL
    );
END
GO

-- Request Main
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_REQUEST_MAIN')
BEGIN
    CREATE TABLE SP_REQUEST_MAIN (
        RM_REQUESTID    INT           IDENTITY(1,1) PRIMARY KEY,
        RM_REQUESTEDBY  NVARCHAR(255) NOT NULL,
        RM_REQUESTEDON  DATETIME      NOT NULL DEFAULT GETDATE(),
        RM_LOCATIONID   INT           NULL,
        RM_DEPT_ID      NVARCHAR(50)  NULL,
        RM_UNIT_CD      NVARCHAR(3)   NULL,
        RM_FINYEAR      INT           NULL
    );
END
GO

-- Request Sub
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_REQUEST_SUB')
BEGIN
    CREATE TABLE SP_REQUEST_SUB (
        RS_REQUESTSUB_ID INT           IDENTITY(1,1) PRIMARY KEY,
        RS_REQUESTID     INT           NOT NULL,
        RS_CATEGORYID    INT           NULL,
        RS_SUBCATEGORYID INT           NULL,
        RS_STATIONERYID  INT           NULL,
        RS_QUANTITY      INT           NULL,
        RS_APPROX_COST   BIGINT        NULL,
        RS_STATUS        NVARCHAR(1)   NULL DEFAULT 'P',
        RS_APPROVEDBY    NVARCHAR(255) NULL,
        RS_APPROVEDON    DATETIME      NULL,
        RS_INDENTORID    NVARCHAR(255) NULL,
        RS_INDENTEDON    DATETIME      NULL,
        RS_RECEIVEDON    DATETIME      NULL,
        CONSTRAINT FK_RequestSub_RequestMain FOREIGN KEY (RS_REQUESTID) REFERENCES SP_REQUEST_MAIN(RM_REQUESTID)
    );
END
GO

-- Order Main
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_ORDER_MAIN')
BEGIN
    CREATE TABLE SP_ORDER_MAIN (
        OM_ORDERMAIN_ID INT           IDENTITY(1,1) PRIMARY KEY,
        OM_VENDORID     NVARCHAR(255) NOT NULL,
        OM_ORDEREDON    DATETIME      NOT NULL DEFAULT GETDATE(),
        OM_DELIVERYDATE DATETIME      NULL,
        OM_LOCATIONID   INT           NULL
    );
END
GO

-- Order Sub
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_ORDER_SUB')
BEGIN
    CREATE TABLE SP_ORDER_SUB (
        OS_ORDERSUB_ID  INT           IDENTITY(1,1) PRIMARY KEY,
        OS_ORDERMAIN_ID INT           NOT NULL,
        OS_REQUESTSUB_ID INT          NULL,
        OS_ORDERED_QTY  INT           NULL,
        OS_UNIT_PRICE   BIGINT        NULL,
        OS_RECEIVEDON   DATETIME      NULL,
        OS_ACTUAL_PRICE BIGINT        NULL,
        CONSTRAINT FK_OrderSub_OrderMain FOREIGN KEY (OS_ORDERMAIN_ID) REFERENCES SP_ORDER_MAIN(OM_ORDERMAIN_ID)
    );
END
GO

-- Dept Budget
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_DEPT_BUDGET')
BEGIN
    CREATE TABLE SP_DEPT_BUDGET (
        DB_DEPT_ID       NVARCHAR(50) NOT NULL,
        DB_BUDGET_AMOUNT BIGINT       NULL,
        DB_FINYEAR       INT          NULL,
        DB_UNIT_CD       NVARCHAR(3)  NULL
    );
END
GO

-- Unit Budget
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_UNIT_BUDGET')
BEGIN
    CREATE TABLE SP_UNIT_BUDGET (
        UB_UNIT_CD       NVARCHAR(3)  NOT NULL,
        UB_DEPT_ID       NVARCHAR(50) NOT NULL,
        UB_BUDGET_AMOUNT BIGINT       NULL,
        UB_FINYEAR       INT          NULL
    );
END
GO

-- Dept Approver
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_DEPT_APPROVER')
BEGIN
    CREATE TABLE SP_DEPT_APPROVER (
        DA_DEPT_ID       NVARCHAR(50)  NOT NULL,
        DA_APPROVERID    NVARCHAR(255) NOT NULL,
        DA_APPROVER_TYPE NVARCHAR(1)   NULL
    );
END
GO

-- Unit Approver
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SP_UNIT_APPROVER')
BEGIN
    CREATE TABLE SP_UNIT_APPROVER (
        UA_UNIT_CD     NVARCHAR(3)   NOT NULL,
        UA_DEPT_ID     NVARCHAR(50)  NOT NULL,
        UA_APPROVERID  NVARCHAR(255) NOT NULL,
        UA_CLOSURE_DATE NVARCHAR(255) NULL
    );
END
GO

PRINT 'Transaction database tables created successfully.';
