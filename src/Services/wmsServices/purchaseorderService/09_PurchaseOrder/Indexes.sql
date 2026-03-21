-- ======================================================
-- Module: Purchase Orders - Indexes
-- ======================================================
CREATE INDEX IX_PurchaseOrder_Supplier ON PurchaseOrder(supplier_id);
CREATE INDEX IX_PurchaseOrder_PONumber ON PurchaseOrder(po_number);
CREATE INDEX IX_PurchaseOrderLine_PO ON PurchaseOrderLine(po_id);
CREATE INDEX IX_PurchaseOrderLine_Product ON PurchaseOrderLine(product_id);
GO
