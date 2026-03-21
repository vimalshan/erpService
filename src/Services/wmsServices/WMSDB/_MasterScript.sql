-- ========================================================================
-- WMS Database - Master Deployment Script
-- Run this script to create the entire database schema in correct order.
-- ========================================================================
-- Execution Order (respects foreign key dependencies):
--   01_Security       -> Users, Roles, Permissions
--   02_WarehouseStructure -> Warehouse, Zone
--   03_RackingSystem  -> Rack, Shelf, Bin
--   04_Employee       -> Employee (depends on Users, Warehouse)
--   05_Product        -> Category, Product
--   06_Inventory      -> StockLevel, InventoryTransaction (depends on Product, Warehouse, Bin)
--   07_Supplier       -> Supplier
--   08_Customer       -> Customer
--   09_PurchaseOrder  -> PurchaseOrder, PurchaseOrderLine (depends on Supplier, Product)
--   10_Receiving      -> Receiving, ReceivingLine (depends on PO, Product, Bin)
--   11_SalesOrder     -> SalesOrder, SalesOrderLine (depends on Customer, Product)
--   12_Shipment       -> Shipment, ShipmentLine, Package, Tracking (depends on SO, Customer, Product, Bin)
--   13_Order          -> Order, OrderItem (depends on Customer, Product)
--   14_FleetManagement -> Vehicle, Driver, Route, Trip, TripStop, MaintenanceLog, FuelLog
--   15_AuditLog       -> AuditLog
-- ========================================================================

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

PRINT '========================================';
PRINT 'WMS Database Schema Deployment';
PRINT '========================================';
PRINT '';

-- ==================== TABLES ====================
PRINT '>> 01_Security - Tables';
:r .\01_Security\Tables.sql

PRINT '>> 02_WarehouseStructure - Tables';
:r .\02_WarehouseStructure\Tables.sql

PRINT '>> 03_RackingSystem - Tables';
:r .\03_RackingSystem\Tables.sql

PRINT '>> 04_Employee - Tables';
:r .\04_Employee\Tables.sql

PRINT '>> 05_Product - Tables';
:r .\05_Product\Tables.sql

PRINT '>> 06_Inventory - Tables';
:r .\06_Inventory\Tables.sql

PRINT '>> 07_Supplier - Tables';
:r .\07_Supplier\Tables.sql

PRINT '>> 08_Customer - Tables';
:r .\08_Customer\Tables.sql

PRINT '>> 09_PurchaseOrder - Tables';
:r .\09_PurchaseOrder\Tables.sql

PRINT '>> 10_Receiving - Tables';
:r .\10_Receiving\Tables.sql

PRINT '>> 11_SalesOrder - Tables';
:r .\11_SalesOrder\Tables.sql

PRINT '>> 12_Shipment - Tables';
:r .\12_Shipment\Tables.sql

PRINT '>> 13_Order - Tables';
:r .\13_Order\Tables.sql

PRINT '>> 14_FleetManagement - Tables';
:r .\14_FleetManagement\Tables.sql

PRINT '>> 15_AuditLog - Tables';
:r .\15_AuditLog\Tables.sql

-- ==================== INDEXES ====================
PRINT '';
PRINT '>> 02_WarehouseStructure - Indexes';
:r .\02_WarehouseStructure\Indexes.sql

PRINT '>> 03_RackingSystem - Indexes';
:r .\03_RackingSystem\Indexes.sql

PRINT '>> 04_Employee - Indexes';
:r .\04_Employee\Indexes.sql

PRINT '>> 05_Product - Indexes';
:r .\05_Product\Indexes.sql

PRINT '>> 06_Inventory - Indexes';
:r .\06_Inventory\Indexes.sql

PRINT '>> 09_PurchaseOrder - Indexes';
:r .\09_PurchaseOrder\Indexes.sql

PRINT '>> 10_Receiving - Indexes';
:r .\10_Receiving\Indexes.sql

PRINT '>> 11_SalesOrder - Indexes';
:r .\11_SalesOrder\Indexes.sql

PRINT '>> 12_Shipment - Indexes';
:r .\12_Shipment\Indexes.sql

PRINT '>> 13_Order - Indexes';
:r .\13_Order\Indexes.sql

PRINT '>> 14_FleetManagement - Indexes';
:r .\14_FleetManagement\Indexes.sql

-- ==================== FUNCTIONS ====================
PRINT '';
PRINT '>> 03_RackingSystem - Functions';
:r .\03_RackingSystem\Functions.sql

PRINT '>> 06_Inventory - Functions';
:r .\06_Inventory\Functions.sql

PRINT '>> 12_Shipment - Functions';
:r .\12_Shipment\Functions.sql

PRINT '>> 14_FleetManagement - Functions';
:r .\14_FleetManagement\Functions.sql

-- ==================== STORED PROCEDURES ====================
PRINT '';
PRINT '>> 06_Inventory - Stored Procedures';
:r .\06_Inventory\StoredProcedures.sql

PRINT '>> 10_Receiving - Stored Procedures';
:r .\10_Receiving\StoredProcedures.sql

PRINT '>> 12_Shipment - Stored Procedures';
:r .\12_Shipment\StoredProcedures.sql

PRINT '>> 14_FleetManagement - Stored Procedures';
:r .\14_FleetManagement\StoredProcedures.sql

PRINT '';
PRINT '========================================';
PRINT 'Deployment Complete!';
PRINT '========================================';
GO
