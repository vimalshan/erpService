-- ======================================================
-- Module: Shipment & Tracking - Functions
-- ======================================================

-- Function: Calculate shipping cost based on weight and service type
CREATE OR ALTER FUNCTION fn_CalculateShippingCost (
    @weight DECIMAL(10,2),
    @service_type NVARCHAR(20)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @cost DECIMAL(10,2) = 0;
    SET @cost = CASE @service_type
                    WHEN 'Express' THEN 10.00 + (@weight * 2.50)
                    WHEN 'Standard' THEN 5.00 + (@weight * 1.50)
                    ELSE 7.00 + (@weight * 2.00)
                END;
    RETURN @cost;
END;
GO
