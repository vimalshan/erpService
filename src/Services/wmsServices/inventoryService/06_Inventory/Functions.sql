-- ======================================================
-- Module: Inventory - Functions
-- ======================================================

-- Function: Get available stock for a product (optionally filtered by bin or warehouse)
CREATE OR ALTER FUNCTION fn_GetAvailableStock (
    @product_id INT,
    @warehouse_id INT = NULL,
    @bin_id INT = NULL
)
RETURNS DECIMAL(18,3)
AS
BEGIN
    DECLARE @available DECIMAL(18,3);
    IF @bin_id IS NOT NULL
        SELECT @available = quantity_available
        FROM StockLevel
        WHERE product_id = @product_id AND bin_id = @bin_id;
    ELSE IF @warehouse_id IS NOT NULL
        SELECT @available = SUM(quantity_available)
        FROM StockLevel
        WHERE product_id = @product_id AND warehouse_id = @warehouse_id;
    ELSE
        SELECT @available = SUM(quantity_available)
        FROM StockLevel
        WHERE product_id = @product_id;
    RETURN ISNULL(@available, 0);
END;
GO

-- Function: Calculate total value of inventory
CREATE OR ALTER FUNCTION fn_GetInventoryValuation (
    @product_id INT = NULL,
    @as_of_date DATETIME2 = NULL
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @total DECIMAL(18,2);
    IF @as_of_date IS NULL SET @as_of_date = GETDATE();
    WITH LatestPrice AS (
        SELECT TOP 1 WITH TIES 
            pol.product_id, 
            pol.unit_price,
            ROW_NUMBER() OVER (PARTITION BY pol.product_id ORDER BY po.order_date DESC) AS rn
        FROM PurchaseOrderLine pol
        INNER JOIN PurchaseOrder po ON pol.po_id = po.po_id
        WHERE po.status = 'COMPLETED'
        AND (pol.product_id = @product_id OR @product_id IS NULL)
    )
    SELECT @total = SUM(s.quantity_on_hand * lp.unit_price)
    FROM StockLevel s
    INNER JOIN LatestPrice lp ON s.product_id = lp.product_id AND lp.rn = 1
    WHERE (@product_id IS NULL OR s.product_id = @product_id);
    RETURN ISNULL(@total, 0);
END;
GO
