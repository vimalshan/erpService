-- ============================================================
-- TransactionService Seed Data Script
-- Database: ADMINDB
-- ============================================================

-- ============================================================
-- LOCATION ADMIN (Reference Data)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_LOCATION_ADMIN WHERE LA_LOC_ID = 1)
BEGIN
    INSERT INTO SP_LOCATION_ADMIN (LA_LOC_ID, LA_LOC_NAME)
    VALUES
        (1, 'Head Office'),
        (2, 'Branch Office - North'),
        (3, 'Branch Office - South'),
        (4, 'Regional Office - East'),
        (5, 'Regional Office - West');
END
GO

-- ============================================================
-- CATEGORY DEFAULT (Reference Data)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_CATEGORY_DEFAULT WHERE CD_CATEGORY_ID = 1)
BEGIN
    INSERT INTO SP_CATEGORY_DEFAULT (CD_CATEGORY_ID, CD_CATEGORY_NAME, CD_SUB_CATEGORY_ID, CD_SUB_CATEGORY_NAME)
    VALUES
        (1, 'Office Supplies', 101, 'Pens & Markers'),
        (1, 'Office Supplies', 102, 'Paper Products'),
        (1, 'Office Supplies', 103, 'Folders & Binders'),
        (2, 'IT Equipment', 201, 'Peripherals'),
        (2, 'IT Equipment', 202, 'Cables & Adapters'),
        (3, 'Furniture', 301, 'Chairs'),
        (3, 'Furniture', 302, 'Desks'),
        (4, 'Cleaning Supplies', 401, 'General Cleaning'),
        (4, 'Cleaning Supplies', 402, 'Sanitization');
END
GO

-- ============================================================
-- DEPT APPROVER (Approval Hierarchy)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_DEPT_APPROVER WHERE DA_DEPT_ID = 'D001')
BEGIN
    INSERT INTO SP_DEPT_APPROVER (DA_DEPT_ID, DA_APPROVERID, DA_APPROVER_TYPE)
    VALUES
        ('D001', 'EMP001', 'A'),
        ('D001', 'EMP002', 'I'),
        ('D002', 'EMP003', 'A'),
        ('D002', 'EMP004', 'I'),
        ('D003', 'EMP005', 'A');
END
GO

-- ============================================================
-- UNIT APPROVER (Unit-Level Approval Hierarchy)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_UNIT_APPROVER WHERE UA_UNIT_CD = 'U01')
BEGIN
    INSERT INTO SP_UNIT_APPROVER (UA_UNIT_CD, UA_DEPT_ID, UA_APPROVERID, UA_CLOSURE_DATE)
    VALUES
        ('U01', 'D001', 'EMP010', NULL),
        ('U01', 'D002', 'EMP011', NULL),
        ('U02', 'D001', 'EMP012', NULL),
        ('U02', 'D003', 'EMP013', NULL),
        ('U03', 'D002', 'EMP014', '2025-12-31');
END
GO

-- ============================================================
-- DEPT BUDGET (Department Budget Allocation)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_DEPT_BUDGET WHERE DB_DEPT_ID = 'D001')
BEGIN
    INSERT INTO SP_DEPT_BUDGET (DB_DEPT_ID, DB_BUDGET_AMOUNT, DB_FINYEAR, DB_UNIT_CD)
    VALUES
        ('D001', 500000, 2025, 'U01'),
        ('D001', 300000, 2025, 'U02'),
        ('D002', 450000, 2025, 'U01'),
        ('D002', 250000, 2025, 'U02'),
        ('D003', 600000, 2025, 'U01');
END
GO

-- ============================================================
-- UNIT BUDGET (Unit Budget Allocation)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_UNIT_BUDGET WHERE UB_UNIT_CD = 'U01')
BEGIN
    INSERT INTO SP_UNIT_BUDGET (UB_UNIT_CD, UB_DEPT_ID, UB_BUDGET_AMOUNT, UB_FINYEAR)
    VALUES
        ('U01', 'D001', 150000, 2025),
        ('U01', 'D002', 120000, 2025),
        ('U02', 'D001', 100000, 2025),
        ('U02', 'D002', 80000, 2025),
        ('U03', 'D002', 90000, 2025);
END
GO

-- ============================================================
-- REQUEST MAIN (Sample Stationery Requests)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_REQUEST_MAIN WHERE RM_REQUESTID = 1)
BEGIN
    SET IDENTITY_INSERT SP_REQUEST_MAIN ON;
    INSERT INTO SP_REQUEST_MAIN (RM_REQUESTID, RM_REQUESTEDBY, RM_REQUESTEDON, RM_LOCATIONID, RM_DEPT_ID, RM_UNIT_CD, RM_FINYEAR)
    VALUES
        (1, 'EMP101', '2025-01-15', 1, 'D001', 'U01', 2025),
        (2, 'EMP102', '2025-01-20', 2, 'D002', 'U01', 2025),
        (3, 'EMP103', '2025-02-01', 1, 'D001', 'U02', 2025);
    SET IDENTITY_INSERT SP_REQUEST_MAIN OFF;
END
GO

-- ============================================================
-- REQUEST SUB (Request Line Items)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_REQUEST_SUB WHERE RS_REQUESTSUB_ID = 1)
BEGIN
    SET IDENTITY_INSERT SP_REQUEST_SUB ON;
    INSERT INTO SP_REQUEST_SUB (RS_REQUESTSUB_ID, RS_REQUESTID, RS_CATEGORYID, RS_SUBCATEGORYID, RS_STATIONERYID, RS_QUANTITY, RS_APPROX_COST, RS_STATUS, RS_APPROVEDBY, RS_APPROVEDON, RS_INDENTORID, RS_INDENTEDON, RS_RECEIVEDON)
    VALUES
        (1, 1, 1, 101, 1001, 50, 500, 'A', 'EMP001', '2025-01-16', 'EMP050', '2025-01-17', '2025-01-25'),
        (2, 1, 1, 102, 1002, 100, 1000, 'A', 'EMP001', '2025-01-16', 'EMP050', '2025-01-17', NULL),
        (3, 2, 2, 201, 2001, 10, 5000, 'P', NULL, NULL, NULL, NULL, NULL),
        (4, 2, 2, 202, 2002, 20, 2000, 'A', 'EMP003', '2025-01-22', NULL, NULL, NULL),
        (5, 3, 1, 103, 1003, 200, 3000, 'R', 'EMP005', '2025-02-02', NULL, NULL, NULL);
    SET IDENTITY_INSERT SP_REQUEST_SUB OFF;
END
GO

-- ============================================================
-- ORDER MAIN (Purchase Orders)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_ORDER_MAIN WHERE OM_ORDERMAIN_ID = 1)
BEGIN
    SET IDENTITY_INSERT SP_ORDER_MAIN ON;
    INSERT INTO SP_ORDER_MAIN (OM_ORDERMAIN_ID, OM_VENDORID, OM_ORDEREDON, OM_DELIVERYDATE, OM_LOCATIONID)
    VALUES
        (1, 'VND001', '2025-01-18', '2025-02-18', 1),
        (2, 'VND002', '2025-01-25', '2025-02-25', 2);
    SET IDENTITY_INSERT SP_ORDER_MAIN OFF;
END
GO

-- ============================================================
-- ORDER SUB (Order Line Items)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM SP_ORDER_SUB WHERE OS_ORDERSUB_ID = 1)
BEGIN
    SET IDENTITY_INSERT SP_ORDER_SUB ON;
    INSERT INTO SP_ORDER_SUB (OS_ORDERSUB_ID, OS_ORDERMAIN_ID, OS_REQUESTSUB_ID, OS_ORDERED_QTY, OS_UNIT_PRICE, OS_RECEIVEDON, OS_ACTUAL_PRICE)
    VALUES
        (1, 1, 1, 50, 10, '2025-02-10', 9),
        (2, 1, 2, 100, 10, NULL, NULL),
        (3, 2, 4, 20, 100, NULL, NULL);
    SET IDENTITY_INSERT SP_ORDER_SUB OFF;
END
GO

PRINT 'Seed data inserted successfully.';
