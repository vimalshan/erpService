-- =============================================================================
-- WMS Microservices - Database Initialization Script
-- Creates all 16 databases for the WMS platform
-- Run against SQL Server after container startup
-- =============================================================================

-- Use master database
USE [master];
GO

-- =============================================================================
-- 01 - Security Service Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SecurityServiceDb')
BEGIN
    CREATE DATABASE [SecurityServiceDb];
    PRINT 'Created database: SecurityServiceDb';
END
GO

-- =============================================================================
-- 02 - Warehouse Structure Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'WarehouseStructureDb')
BEGIN
    CREATE DATABASE [WarehouseStructureDb];
    PRINT 'Created database: WarehouseStructureDb';
END
GO

-- =============================================================================
-- 03 - Racking System Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'RackingSystemDb')
BEGIN
    CREATE DATABASE [RackingSystemDb];
    PRINT 'Created database: RackingSystemDb';
END
GO

-- =============================================================================
-- 04 - Employee Service Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'EmployeeServiceDb')
BEGIN
    CREATE DATABASE [EmployeeServiceDb];
    PRINT 'Created database: EmployeeServiceDb';
END
GO

-- =============================================================================
-- 05 - Product Service Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ProductServiceDb')
BEGIN
    CREATE DATABASE [ProductServiceDb];
    PRINT 'Created database: ProductServiceDb';
END
GO

-- =============================================================================
-- 06 - Inventory Service Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'InventoryServiceDb')
BEGIN
    CREATE DATABASE [InventoryServiceDb];
    PRINT 'Created database: InventoryServiceDb';
END
GO

-- =============================================================================
-- 07 - Supplier Service Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SupplierServiceDb')
BEGIN
    CREATE DATABASE [SupplierServiceDb];
    PRINT 'Created database: SupplierServiceDb';
END
GO

-- =============================================================================
-- 08 - Customer Service Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CustomerServiceDb')
BEGIN
    CREATE DATABASE [CustomerServiceDb];
    PRINT 'Created database: CustomerServiceDb';
END
GO

-- =============================================================================
-- 09 - Purchase Order Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PurchaseOrderDb')
BEGIN
    CREATE DATABASE [PurchaseOrderDb];
    PRINT 'Created database: PurchaseOrderDb';
END
GO

-- =============================================================================
-- 10 - Receiving Service Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ReceivingServiceDb')
BEGIN
    CREATE DATABASE [ReceivingServiceDb];
    PRINT 'Created database: ReceivingServiceDb';
END
GO

-- =============================================================================
-- 11 - Sales Order Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SalesOrderDb')
BEGIN
    CREATE DATABASE [SalesOrderDb];
    PRINT 'Created database: SalesOrderDb';
END
GO

-- =============================================================================
-- 12 - Shipment Service Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ShipmentServiceDb')
BEGIN
    CREATE DATABASE [ShipmentServiceDb];
    PRINT 'Created database: ShipmentServiceDb';
END
GO

-- =============================================================================
-- 13 - Order Service Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'OrderServiceDb')
BEGIN
    CREATE DATABASE [OrderServiceDb];
    PRINT 'Created database: OrderServiceDb';
END
GO

-- =============================================================================
-- 14 - Fleet Management Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'FleetManagementDb')
BEGIN
    CREATE DATABASE [FleetManagementDb];
    PRINT 'Created database: FleetManagementDb';
END
GO

-- =============================================================================
-- 15 - Audit Log Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'AuditLogDb')
BEGIN
    CREATE DATABASE [AuditLogDb];
    PRINT 'Created database: AuditLogDb';
END
GO

-- =============================================================================
-- 16 - WM Transactional Database
-- =============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'WMTransactionalDb')
BEGIN
    CREATE DATABASE [WMTransactionalDb];
    PRINT 'Created database: WMTransactionalDb';
END
GO

-- =============================================================================
-- Verification
-- =============================================================================
SELECT name, create_date, state_desc
FROM sys.databases
WHERE name IN (
    'SecurityServiceDb', 'WarehouseStructureDb', 'RackingSystemDb',
    'EmployeeServiceDb', 'ProductServiceDb', 'InventoryServiceDb',
    'SupplierServiceDb', 'CustomerServiceDb', 'PurchaseOrderDb',
    'ReceivingServiceDb', 'SalesOrderDb', 'ShipmentServiceDb',
    'OrderServiceDb', 'FleetManagementDb', 'AuditLogDb', 'WMTransactionalDb'
)
ORDER BY name;
GO

PRINT '=== All 16 WMS databases created successfully ===';
GO
