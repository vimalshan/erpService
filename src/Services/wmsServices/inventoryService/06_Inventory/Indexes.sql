-- ======================================================
-- Module: Inventory - Indexes
-- ======================================================
CREATE INDEX IX_StockLevel_Product ON StockLevel(product_id);
CREATE INDEX IX_StockLevel_Bin ON StockLevel(bin_id);
CREATE INDEX IX_StockLevel_Warehouse ON StockLevel(warehouse_id);
CREATE INDEX IX_InventoryTransaction_Product ON InventoryTransaction(product_id);
CREATE INDEX IX_InventoryTransaction_Bin ON InventoryTransaction(bin_id);
CREATE INDEX IX_InventoryTransaction_Warehouse ON InventoryTransaction(warehouse_id);
CREATE INDEX IX_InventoryTransaction_Date ON InventoryTransaction(transaction_date);
GO
