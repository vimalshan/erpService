-- ======================================================
-- Module: Orders - Indexes
-- ======================================================
CREATE INDEX IX_Order_Customer ON [Order](customer_id);
CREATE INDEX IX_Order_OrderNumber ON [Order](order_number);
CREATE INDEX IX_OrderItem_Order ON OrderItem(order_id);
CREATE INDEX IX_OrderItem_Product ON OrderItem(product_id);
GO
