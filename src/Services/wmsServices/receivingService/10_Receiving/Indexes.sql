-- ======================================================
-- Module: Receiving - Indexes
-- ======================================================
CREATE INDEX IX_Receiving_PO ON Receiving(po_id);
CREATE INDEX IX_Receiving_ReceivingNumber ON Receiving(receiving_number);
CREATE INDEX IX_ReceivingLine_Receiving ON ReceivingLine(receiving_id);
CREATE INDEX IX_ReceivingLine_POLine ON ReceivingLine(po_line_id);
CREATE INDEX IX_ReceivingLine_Product ON ReceivingLine(product_id);
CREATE INDEX IX_ReceivingLine_Bin ON ReceivingLine(bin_id);
GO
