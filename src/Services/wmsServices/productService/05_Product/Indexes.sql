-- ======================================================
-- Module: Product - Indexes
-- ======================================================
CREATE INDEX IX_Product_SKU ON Product(sku);
CREATE INDEX IX_Product_Name ON Product(name);
CREATE INDEX IX_Product_CategoryID ON Product(category_id);
GO
