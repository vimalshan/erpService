-- ======================================================
-- Module: Fleet Management - Functions
-- ======================================================

-- Function: Get vehicle utilization (percentage of time in use over a period)
CREATE OR ALTER FUNCTION fn_GetVehicleUtilization (
    @vehicle_id INT,
    @start_date DATE,
    @end_date DATE
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @total_minutes INT, @in_use_minutes INT;
    SET @total_minutes = DATEDIFF(MINUTE, @start_date, @end_date);
    SELECT @in_use_minutes = SUM(DATEDIFF(MINUTE, start_time, ISNULL(end_time, GETDATE())))
    FROM Trip
    WHERE vehicle_id = @vehicle_id
      AND start_time >= @start_date
      AND start_time < @end_date
      AND status IN ('IN_PROGRESS', 'COMPLETED');
    RETURN ISNULL(100.0 * @in_use_minutes / NULLIF(@total_minutes, 0), 0);
END;
GO
