-- ======================================================
-- Module: Sales Orders - Indexes
-- ======================================================
CREATE INDEX IX_SalesOrder_Customer ON SalesOrder(customer_id);
CREATE INDEX IX_SalesOrder_SONumber ON SalesOrder(so_number);
CREATE INDEX IX_SalesOrderLine_SO ON SalesOrderLine(so_id);
CREATE INDEX IX_SalesOrderLine_Product ON SalesOrderLine(product_id);
GO
