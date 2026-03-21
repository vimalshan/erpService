-- ======================================================
-- Module: Racking System - Functions
-- ======================================================

-- Function: Get bin utilization percentage
CREATE OR ALTER FUNCTION fn_GetBinUtilization (@bin_id INT)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @capacity DECIMAL(18,3), @current_qty DECIMAL(18,3), @util DECIMAL(5,2);
    SELECT @capacity = b.capacity_qty FROM Bin b WHERE b.bin_id = @bin_id;
    SELECT @current_qty = SUM(quantity_on_hand) FROM StockLevel WHERE bin_id = @bin_id;
    IF @capacity IS NULL OR @capacity = 0 RETURN NULL;
    SET @util = (@current_qty / @capacity) * 100;
    RETURN @util;
END;
GO
