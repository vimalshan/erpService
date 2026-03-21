-- ======================================================
-- Module: Fleet Management - Stored Procedures
-- ======================================================

-- Procedure: Create a new trip
CREATE OR ALTER PROCEDURE sp_CreateTrip
    @trip_number        NVARCHAR(50),
    @vehicle_id         INT,
    @driver_id          INT,
    @route_id           INT = NULL,
    @origin_type        NVARCHAR(30) = NULL,
    @origin_id          INT = NULL,
    @destination_type   NVARCHAR(30) = NULL,
    @destination_id     INT = NULL,
    @planned_stops      NVARCHAR(MAX) = NULL,   -- JSON array of stops
    @created_by         NVARCHAR(50),
    @trip_id            INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO Trip (trip_number, vehicle_id, driver_id, route_id, origin_type, origin_id, destination_type, destination_id, status, created_by)
        VALUES (@trip_number, @vehicle_id, @driver_id, @route_id, @origin_type, @origin_id, @destination_type, @destination_id, 'PLANNED', @created_by);
        
        SET @trip_id = SCOPE_IDENTITY();
        
        IF @planned_stops IS NOT NULL
        BEGIN
            INSERT INTO TripStop (trip_id, stop_sequence, stop_type, location_type, location_id, address, planned_arrival, planned_departure)
            SELECT 
                @trip_id,
                stop_sequence,
                stop_type,
                location_type,
                location_id,
                address,
                planned_arrival,
                planned_departure
            FROM OPENJSON(@planned_stops)
            WITH (
                stop_sequence      INT             '$.sequence',
                stop_type          NVARCHAR(30)    '$.stop_type',
                location_type      NVARCHAR(30)    '$.location_type',
                location_id        INT             '$.location_id',
                address            NVARCHAR(200)   '$.address',
                planned_arrival    DATETIME2       '$.planned_arrival',
                planned_departure  DATETIME2       '$.planned_departure'
            );
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Start a trip
CREATE OR ALTER PROCEDURE sp_StartTrip
    @trip_id        INT,
    @start_time     DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Trip 
    SET start_time = ISNULL(@start_time, GETDATE()), 
        status = 'IN_PROGRESS',
        modified_date = GETDATE()
    WHERE trip_id = @trip_id AND status = 'PLANNED';
    IF @@ROWCOUNT = 0
        THROW 50000, 'Trip cannot be started (not in PLANNED status or does not exist).', 1;
END;
GO

-- Procedure: Complete a trip
CREATE OR ALTER PROCEDURE sp_CompleteTrip
    @trip_id        INT,
    @end_time       DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Trip 
    SET end_time = ISNULL(@end_time, GETDATE()), 
        status = 'COMPLETED',
        modified_date = GETDATE()
    WHERE trip_id = @trip_id AND status = 'IN_PROGRESS';
    IF @@ROWCOUNT = 0
        THROW 50000, 'Trip cannot be completed (not in IN_PROGRESS status).', 1;
END;
GO

-- Procedure: Log vehicle maintenance
CREATE OR ALTER PROCEDURE sp_LogMaintenance
    @vehicle_id         INT,
    @maintenance_date   DATE,
    @maintenance_type   NVARCHAR(50),
    @description        NVARCHAR(MAX),
    @cost               DECIMAL(18,2) = NULL,
    @odometer_reading   INT = NULL,
    @next_due_date      DATE = NULL,
    @performed_by       NVARCHAR(100) = NULL
AS
BEGIN
    INSERT INTO MaintenanceLog (vehicle_id, maintenance_date, maintenance_type, description, cost, odometer_reading, next_due_date, performed_by)
    VALUES (@vehicle_id, @maintenance_date, @maintenance_type, @description, @cost, @odometer_reading, @next_due_date, @performed_by);
    
    UPDATE Vehicle SET status = 'MAINTENANCE', modified_date = GETDATE()
    WHERE vehicle_id = @vehicle_id AND status = 'AVAILABLE';
END;
GO

-- Procedure: Log fuel purchase
CREATE OR ALTER PROCEDURE sp_LogFuel
    @vehicle_id         INT,
    @gallons            DECIMAL(18,3),
    @cost               DECIMAL(18,2),
    @odometer_reading   INT = NULL,
    @notes              NVARCHAR(MAX) = NULL
AS
BEGIN
    INSERT INTO FuelLog (vehicle_id, fuel_date, gallons, cost, odometer_reading, notes)
    VALUES (@vehicle_id, GETDATE(), @gallons, @cost, @odometer_reading, @notes);
END;
GO

-- Procedure: Get fleet status summary
CREATE OR ALTER PROCEDURE sp_GetFleetStatus
    @warehouse_id   INT = NULL
AS
BEGIN
    SELECT 
        v.vehicle_id,
        v.code,
        v.license_plate,
        v.vehicle_type,
        v.status,
        w.code AS home_warehouse,
        (SELECT COUNT(*) FROM Trip WHERE vehicle_id = v.vehicle_id AND status = 'IN_PROGRESS') AS active_trips,
        (SELECT TOP 1 maintenance_date FROM MaintenanceLog WHERE vehicle_id = v.vehicle_id ORDER BY maintenance_date DESC) AS last_maintenance,
        (SELECT TOP 1 next_due_date FROM MaintenanceLog WHERE vehicle_id = v.vehicle_id ORDER BY maintenance_date DESC) AS next_maintenance_due
    FROM Vehicle v
    LEFT JOIN Warehouse w ON v.warehouse_id = w.warehouse_id
    WHERE (@warehouse_id IS NULL OR v.warehouse_id = @warehouse_id)
    ORDER BY v.status, v.code;
END;
GO
