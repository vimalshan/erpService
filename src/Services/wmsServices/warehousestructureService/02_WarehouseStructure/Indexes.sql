-- ======================================================
-- Module: Warehouse Structure - Indexes
-- ======================================================
CREATE INDEX IX_Zone_Warehouse ON Zone(warehouse_id);
CREATE INDEX IX_Warehouse_Code ON Warehouse(code);
GO
