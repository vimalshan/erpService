-- ======================================================
-- Module: Employee - Indexes
-- ======================================================
CREATE INDEX IX_Employee_UserID ON Employee(user_id);
CREATE INDEX IX_Employee_WarehouseID ON Employee(warehouse_id);
GO
