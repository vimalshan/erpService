-- ======================================================
-- Module: Shipment & Tracking - Indexes
-- ======================================================
CREATE INDEX IX_Shipment_SO ON Shipment(so_id);
CREATE INDEX IX_Shipment_Customer ON Shipment(customer_id);
CREATE INDEX IX_Shipment_Warehouse ON Shipment(warehouse_id);
CREATE INDEX IX_Shipment_ShipmentNumber ON Shipment(shipment_number);
CREATE INDEX IX_Shipment_TrackingNumber ON Shipment(tracking_number) WHERE tracking_number IS NOT NULL;
CREATE INDEX IX_ShipmentLine_Shipment ON ShipmentLine(shipment_id);
CREATE INDEX IX_ShipmentLine_SOLine ON ShipmentLine(so_line_id);
CREATE INDEX IX_ShipmentLine_Product ON ShipmentLine(product_id);
CREATE INDEX IX_ShipmentLine_Bin ON ShipmentLine(bin_id);
CREATE INDEX IX_Package_Shipment ON Package(shipment_id);
CREATE INDEX IX_TrackingHistory_Shipment ON TrackingHistory(shipment_id);
GO
