-- ======================================================
-- Module: Racking System - Indexes
-- ======================================================
CREATE INDEX IX_Rack_Zone ON Rack(zone_id);
CREATE INDEX IX_Shelf_Rack ON Shelf(rack_id);
CREATE INDEX IX_Bin_Zone ON Bin(zone_id);
CREATE INDEX IX_Bin_Shelf ON Bin(shelf_id) WHERE shelf_id IS NOT NULL;
CREATE INDEX IX_Bin_Code ON Bin(code);
CREATE INDEX IX_Bin_Barcode ON Bin(barcode) WHERE barcode IS NOT NULL;
GO
