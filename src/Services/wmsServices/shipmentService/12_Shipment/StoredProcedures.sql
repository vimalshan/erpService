-- ======================================================
-- Module: Shipment & Tracking - Stored Procedures
-- ======================================================

-- Procedure: Create a new shipment
CREATE OR ALTER PROCEDURE sp_CreateShipment
    @shipment_number    NVARCHAR(50),
    @customer_id        INT,
    @warehouse_id       INT,
    @shipment_type      NVARCHAR(20),
    @service_type       NVARCHAR(20) = NULL,
    @carrier            NVARCHAR(50) = NULL,
    @tracking_number    NVARCHAR(100) = NULL,
    @special_instructions NVARCHAR(MAX) = NULL,
    @created_by         NVARCHAR(50) = NULL,
    @shipment_id        INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO Shipment (
            shipment_number, customer_id, warehouse_id, shipment_type, service_type,
            carrier, tracking_number, special_instructions, created_by
        ) VALUES (
            @shipment_number, @customer_id, @warehouse_id, @shipment_type, @service_type,
            @carrier, @tracking_number, @special_instructions, @created_by
        );
        SET @shipment_id = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Add package to a shipment
CREATE OR ALTER PROCEDURE sp_AddPackage
    @shipment_id        INT,
    @package_number     NVARCHAR(20),
    @weight             DECIMAL(10,2) = NULL,
    @volume             DECIMAL(10,2) = NULL,
    @dimensions         NVARCHAR(50) = NULL,
    @tracking_number    NVARCHAR(50) = NULL,
    @contents_description NVARCHAR(255) = NULL,
    @package_id         INT OUTPUT
AS
BEGIN
    INSERT INTO Package (
        shipment_id, package_number, weight, volume, dimensions, tracking_number, contents_description
    ) VALUES (
        @shipment_id, @package_number, @weight, @volume, @dimensions, @tracking_number, @contents_description
    );
    SET @package_id = SCOPE_IDENTITY();
END;
GO

-- Procedure: Update shipment status and log tracking
CREATE OR ALTER PROCEDURE sp_UpdateShipmentStatus
    @shipment_id    INT,
    @new_status     NVARCHAR(30),
    @location       NVARCHAR(100) = NULL,
    @description    NVARCHAR(255) = NULL,
    @updated_by     NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Shipment SET status = @new_status, modified_date = GETDATE()
        WHERE shipment_id = @shipment_id;

        INSERT INTO TrackingHistory (shipment_id, status, location, description, created_by)
        VALUES (@shipment_id, @new_status, @location, @description, @updated_by);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Ship a sales order (pick from bins, deduct stock, create shipment)
CREATE OR ALTER PROCEDURE sp_ShipSalesOrder
    @shipment_number  NVARCHAR(50),
    @so_id            INT,
    @items_json       NVARCHAR(MAX),   -- JSON array of {so_line_id, product_id, bin_id, quantity, lot_number}
    @carrier          NVARCHAR(50) = NULL,
    @tracking_number  NVARCHAR(100) = NULL,
    @created_by       NVARCHAR(50),
    @shipment_id      INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @customer_id INT, @warehouse_id INT;
        SELECT @customer_id = customer_id, @warehouse_id = warehouse_id
        FROM SalesOrder WHERE so_id = @so_id;

        -- Create shipment
        INSERT INTO Shipment (shipment_number, so_id, customer_id, warehouse_id, shipment_type, carrier, tracking_number, status, created_by)
        VALUES (@shipment_number, @so_id, @customer_id, @warehouse_id, 'OUTBOUND', @carrier, @tracking_number, 'OPEN', @created_by);
        SET @shipment_id = SCOPE_IDENTITY();

        -- Insert shipment lines
        INSERT INTO ShipmentLine (shipment_id, so_line_id, product_id, bin_id, quantity_shipped, lot_number)
        SELECT @shipment_id, so_line_id, product_id, bin_id, quantity, lot_number
        FROM OPENJSON(@items_json)
        WITH (
            so_line_id  INT           '$.so_line_id',
            product_id  INT           '$.product_id',
            bin_id      INT           '$.bin_id',
            quantity    DECIMAL(18,3) '$.quantity',
            lot_number  NVARCHAR(50)  '$.lot_number'
        );

        -- Deduct stock
        UPDATE sl
        SET sl.quantity_on_hand = sl.quantity_on_hand - shl.quantity_shipped,
            sl.last_updated = GETDATE()
        FROM StockLevel sl
        INNER JOIN ShipmentLine shl ON sl.product_id = shl.product_id AND sl.bin_id = shl.bin_id
        WHERE shl.shipment_id = @shipment_id;

        -- Log inventory transactions
        INSERT INTO InventoryTransaction (product_id, warehouse_id, bin_id, transaction_type, quantity_change, reference_type, reference_id, created_by)
        SELECT product_id, @warehouse_id, bin_id, 'SHIPMENT', -quantity_shipped, 'SHIPMENT', @shipment_id, @created_by
        FROM ShipmentLine WHERE shipment_id = @shipment_id;

        -- Update SO line shipped quantities
        UPDATE sol
        SET sol.quantity_shipped = sol.quantity_shipped + shl.quantity_shipped
        FROM SalesOrderLine sol
        INNER JOIN ShipmentLine shl ON sol.so_line_id = shl.so_line_id
        WHERE shl.shipment_id = @shipment_id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
