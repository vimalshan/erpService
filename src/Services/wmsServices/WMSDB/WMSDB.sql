-- ========================================================================
-- Warehouse Management System (WMS) Database Schema
-- Includes: Core WMS + Racking System + Fleet Management
-- Tables, Indexes, Functions, Stored Procedures
-- ========================================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- ========================================================================
-- 1. TABLES (Core WMS + Racking + Fleet)
-- ========================================================================

-- Warehouse master
CREATE TABLE Warehouse (
    warehouse_id    INT IDENTITY(1,1) PRIMARY KEY,
    code            NVARCHAR(20) NOT NULL UNIQUE,
    name            NVARCHAR(100) NOT NULL,
    address         NVARCHAR(200),
    city            NVARCHAR(50),
    state           NVARCHAR(50),
    country         NVARCHAR(50),
    postal_code     NVARCHAR(20),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Zone within a warehouse (e.g., Receiving, Shipping, Bulk Storage)
CREATE TABLE Zone (
    zone_id         INT IDENTITY(1,1) PRIMARY KEY,
    warehouse_id    INT NOT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    code            NVARCHAR(20) NOT NULL,
    name            NVARCHAR(100) NOT NULL,
    zone_type       NVARCHAR(30) NOT NULL CHECK (zone_type IN ('RECEIVING', 'STORAGE', 'PICKING', 'SHIPPING', 'RETURNS')),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Zone_Warehouse_Code UNIQUE (warehouse_id, code)
);

-- RACKING SYSTEM: Rack (physical structure within a zone)
CREATE TABLE Rack (
    rack_id         INT IDENTITY(1,1) PRIMARY KEY,
    zone_id         INT NOT NULL FOREIGN KEY REFERENCES Zone(zone_id),
    code            NVARCHAR(30) NOT NULL,
    rack_type       NVARCHAR(30),          -- e.g., 'PALLET', 'CANTILEVER', 'DRIVE-IN'
    max_load_weight DECIMAL(18,3),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Rack_Zone_Code UNIQUE (zone_id, code)
);

-- RACKING SYSTEM: Shelf (level/position within a rack)
CREATE TABLE Shelf (
    shelf_id        INT IDENTITY(1,1) PRIMARY KEY,
    rack_id         INT NOT NULL FOREIGN KEY REFERENCES Rack(rack_id),
    shelf_level     INT NOT NULL,            -- e.g., 1,2,3 for vertical levels
    shelf_position  INT NOT NULL,            -- horizontal position
    code            NVARCHAR(30) NOT NULL,   -- optionally generated from rack code + level + position
    capacity_qty    DECIMAL(18,3),           -- max quantity this shelf can hold
    capacity_weight DECIMAL(18,3),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Shelf_Rack_Level_Position UNIQUE (rack_id, shelf_level, shelf_position)
);

-- Storage bin (specific location) – now linked to shelf (optional)
CREATE TABLE Bin (
    bin_id          INT IDENTITY(1,1) PRIMARY KEY,
    zone_id         INT NOT NULL FOREIGN KEY REFERENCES Zone(zone_id),
    shelf_id        INT NULL FOREIGN KEY REFERENCES Shelf(shelf_id),  -- can be NULL if bin not on a shelf
    code            NVARCHAR(30) NOT NULL,
    barcode         NVARCHAR(50),
    capacity_qty    DECIMAL(18,3),
    capacity_weight DECIMAL(18,3),
    capacity_volume DECIMAL(18,3),
    status          NVARCHAR(20) NOT NULL DEFAULT 'AVAILABLE' CHECK (status IN ('AVAILABLE', 'OCCUPIED', 'BLOCKED', 'FULL')),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Bin_Zone_Code UNIQUE (zone_id, code)
);

-- Product master
CREATE TABLE Product (
    product_id      INT IDENTITY(1,1) PRIMARY KEY,
    sku             NVARCHAR(50) NOT NULL UNIQUE,
    name            NVARCHAR(200) NOT NULL,
    description     NVARCHAR(MAX),
    unit_of_measure NVARCHAR(20) NOT NULL DEFAULT 'EA',
    weight_per_unit DECIMAL(18,3),
    volume_per_unit DECIMAL(18,3),
    reorder_point   DECIMAL(18,3),
    reorder_quantity DECIMAL(18,3),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Supplier master
CREATE TABLE Supplier (
    supplier_id     INT IDENTITY(1,1) PRIMARY KEY,
    code            NVARCHAR(30) NOT NULL UNIQUE,
    name            NVARCHAR(200) NOT NULL,
    contact_person  NVARCHAR(100),
    email           NVARCHAR(100),
    phone           NVARCHAR(30),
    address         NVARCHAR(200),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Customer master
CREATE TABLE Customer (
    customer_id     INT IDENTITY(1,1) PRIMARY KEY,
    code            NVARCHAR(30) NOT NULL UNIQUE,
    name            NVARCHAR(200) NOT NULL,
    contact_person  NVARCHAR(100),
    email           NVARCHAR(100),
    phone           NVARCHAR(30),
    address         NVARCHAR(200),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Purchase Order header
CREATE TABLE PurchaseOrder (
    po_id           INT IDENTITY(1,1) PRIMARY KEY,
    po_number       NVARCHAR(50) NOT NULL UNIQUE,
    supplier_id     INT NOT NULL FOREIGN KEY REFERENCES Supplier(supplier_id),
    order_date      DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    expected_date   DATE,
    status          NVARCHAR(30) NOT NULL DEFAULT 'DRAFT' CHECK (status IN ('DRAFT', 'CONFIRMED', 'RECEIVING', 'COMPLETED', 'CANCELLED')),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Purchase Order line items
CREATE TABLE PurchaseOrderLine (
    po_line_id      INT IDENTITY(1,1) PRIMARY KEY,
    po_id           INT NOT NULL FOREIGN KEY REFERENCES PurchaseOrder(po_id),
    product_id      INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    line_number     INT NOT NULL,
    quantity_ordered DECIMAL(18,3) NOT NULL CHECK (quantity_ordered > 0),
    quantity_received DECIMAL(18,3) NOT NULL DEFAULT 0,
    unit_price      DECIMAL(18,4),
    notes           NVARCHAR(MAX),
    CONSTRAINT UQ_POLine_PO_Line UNIQUE (po_id, line_number)
);

-- Receiving header
CREATE TABLE Receiving (
    receiving_id    INT IDENTITY(1,1) PRIMARY KEY,
    receiving_number NVARCHAR(50) NOT NULL UNIQUE,
    po_id           INT NOT NULL FOREIGN KEY REFERENCES PurchaseOrder(po_id),
    received_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    status          NVARCHAR(30) NOT NULL DEFAULT 'OPEN' CHECK (status IN ('OPEN', 'CLOSED', 'CANCELLED')),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Receiving line (each received item goes to a bin)
CREATE TABLE ReceivingLine (
    receiving_line_id INT IDENTITY(1,1) PRIMARY KEY,
    receiving_id    INT NOT NULL FOREIGN KEY REFERENCES Receiving(receiving_id),
    po_line_id      INT NOT NULL FOREIGN KEY REFERENCES PurchaseOrderLine(po_line_id),
    product_id      INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    bin_id          INT NOT NULL FOREIGN KEY REFERENCES Bin(bin_id),
    quantity_received DECIMAL(18,3) NOT NULL CHECK (quantity_received > 0),
    lot_number      NVARCHAR(50),
    expiry_date     DATE,
    notes           NVARCHAR(MAX)
);

-- Sales Order header
CREATE TABLE SalesOrder (
    so_id           INT IDENTITY(1,1) PRIMARY KEY,
    so_number       NVARCHAR(50) NOT NULL UNIQUE,
    customer_id     INT NOT NULL FOREIGN KEY REFERENCES Customer(customer_id),
    order_date      DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    requested_date  DATE,
    status          NVARCHAR(30) NOT NULL DEFAULT 'DRAFT' CHECK (status IN ('DRAFT', 'CONFIRMED', 'PICKING', 'SHIPPING', 'COMPLETED', 'CANCELLED')),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Sales Order line items
CREATE TABLE SalesOrderLine (
    so_line_id      INT IDENTITY(1,1) PRIMARY KEY,
    so_id           INT NOT NULL FOREIGN KEY REFERENCES SalesOrder(so_id),
    product_id      INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    line_number     INT NOT NULL,
    quantity_ordered DECIMAL(18,3) NOT NULL CHECK (quantity_ordered > 0),
    quantity_shipped DECIMAL(18,3) NOT NULL DEFAULT 0,
    unit_price      DECIMAL(18,4),
    notes           NVARCHAR(MAX),
    CONSTRAINT UQ_SOLine_SO_Line UNIQUE (so_id, line_number)
);

-- Shipment header
CREATE TABLE Shipment (
    shipment_id     INT IDENTITY(1,1) PRIMARY KEY,
    shipment_number NVARCHAR(50) NOT NULL UNIQUE,
    so_id           INT NOT NULL FOREIGN KEY REFERENCES SalesOrder(so_id),
    shipped_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    status          NVARCHAR(30) NOT NULL DEFAULT 'OPEN' CHECK (status IN ('OPEN', 'SHIPPED', 'CANCELLED')),
    tracking_number NVARCHAR(100),
    carrier         NVARCHAR(50),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Shipment line (items picked from bins)
CREATE TABLE ShipmentLine (
    shipment_line_id INT IDENTITY(1,1) PRIMARY KEY,
    shipment_id     INT NOT NULL FOREIGN KEY REFERENCES Shipment(shipment_id),
    so_line_id      INT NOT NULL FOREIGN KEY REFERENCES SalesOrderLine(so_line_id),
    product_id      INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    bin_id          INT NOT NULL FOREIGN KEY REFERENCES Bin(bin_id),
    quantity_shipped DECIMAL(18,3) NOT NULL CHECK (quantity_shipped > 0),
    lot_number      NVARCHAR(50),
    expiry_date     DATE,
    notes           NVARCHAR(MAX)
);

-- Inventory transactions ledger
CREATE TABLE InventoryTransaction (
    transaction_id   BIGINT IDENTITY(1,1) PRIMARY KEY,
    product_id       INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    bin_id           INT NOT NULL FOREIGN KEY REFERENCES Bin(bin_id),
    transaction_type NVARCHAR(30) NOT NULL CHECK (transaction_type IN ('RECEIPT', 'SHIPMENT', 'MOVE_OUT', 'MOVE_IN', 'ADJUSTMENT', 'PICK', 'PACK', 'RETURN')),
    quantity_change  DECIMAL(18,3) NOT NULL,
    reference_type   NVARCHAR(30),
    reference_id     INT,
    transaction_date DATETIME2 NOT NULL DEFAULT GETDATE(),
    created_by       NVARCHAR(50),
    notes            NVARCHAR(MAX)
);

-- Current stock snapshot (denormalized for performance)
CREATE TABLE StockLevel (
    stock_level_id   BIGINT IDENTITY(1,1) PRIMARY KEY,
    product_id       INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    bin_id           INT NOT NULL FOREIGN KEY REFERENCES Bin(bin_id),
    quantity_on_hand DECIMAL(18,3) NOT NULL DEFAULT 0,
    quantity_allocated DECIMAL(18,3) NOT NULL DEFAULT 0,
    quantity_available AS (quantity_on_hand - quantity_allocated) PERSISTED,
    last_updated     DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_StockLevel_Product_Bin UNIQUE (product_id, bin_id)
);

-- Simple user table for audit purposes
CREATE TABLE [User] (
    user_id         INT IDENTITY(1,1) PRIMARY KEY,
    username        NVARCHAR(50) NOT NULL UNIQUE,
    password_hash   NVARCHAR(255),
    full_name       NVARCHAR(100),
    role            NVARCHAR(30),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Audit log
CREATE TABLE AuditLog (
    log_id          BIGINT IDENTITY(1,1) PRIMARY KEY,
    table_name      NVARCHAR(100) NOT NULL,
    record_id       INT NOT NULL,
    action          NVARCHAR(10) NOT NULL CHECK (action IN ('INSERT','UPDATE','DELETE')),
    changed_by      NVARCHAR(50),
    change_date     DATETIME2 NOT NULL DEFAULT GETDATE(),
    old_values      NVARCHAR(MAX),
    new_values      NVARCHAR(MAX)
);

-- ========================================================================
-- FLEET MANAGEMENT TABLES
-- ========================================================================

-- Vehicle master
CREATE TABLE Vehicle (
    vehicle_id      INT IDENTITY(1,1) PRIMARY KEY,
    code            NVARCHAR(30) NOT NULL UNIQUE,
    license_plate   NVARCHAR(20) NOT NULL UNIQUE,
    vehicle_type    NVARCHAR(30) NOT NULL CHECK (vehicle_type IN ('TRUCK', 'FORKLIFT', 'PALLET_JACK', 'VAN', 'OTHER')),
    make            NVARCHAR(50),
    model           NVARCHAR(50),
    year            INT,
    capacity_weight DECIMAL(18,3),
    capacity_volume DECIMAL(18,3),
    status          NVARCHAR(20) NOT NULL DEFAULT 'AVAILABLE' CHECK (status IN ('AVAILABLE', 'IN_USE', 'MAINTENANCE', 'RETIRED')),
    warehouse_id    INT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id), -- home base
    notes           NVARCHAR(MAX),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Driver master
CREATE TABLE Driver (
    driver_id       INT IDENTITY(1,1) PRIMARY KEY,
    code            NVARCHAR(30) NOT NULL UNIQUE,
    full_name       NVARCHAR(100) NOT NULL,
    license_number  NVARCHAR(50),
    license_expiry  DATE,
    phone           NVARCHAR(30),
    email           NVARCHAR(100),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Trip header
CREATE TABLE Trip (
    trip_id         INT IDENTITY(1,1) PRIMARY KEY,
    trip_number     NVARCHAR(50) NOT NULL UNIQUE,
    vehicle_id      INT NOT NULL FOREIGN KEY REFERENCES Vehicle(vehicle_id),
    driver_id       INT NOT NULL FOREIGN KEY REFERENCES Driver(driver_id),
    start_time      DATETIME2,
    end_time        DATETIME2,
    origin_type     NVARCHAR(30),           -- e.g., 'WAREHOUSE', 'CUSTOMER', 'SUPPLIER'
    origin_id       INT,                    -- ID of warehouse/customer/supplier
    destination_type NVARCHAR(30),
    destination_id   INT,
    status          NVARCHAR(30) NOT NULL DEFAULT 'PLANNED' CHECK (status IN ('PLANNED', 'IN_PROGRESS', 'COMPLETED', 'CANCELLED')),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Trip stops (intermediate stops)
CREATE TABLE TripStop (
    stop_id         INT IDENTITY(1,1) PRIMARY KEY,
    trip_id         INT NOT NULL FOREIGN KEY REFERENCES Trip(trip_id),
    stop_sequence   INT NOT NULL,
    stop_type       NVARCHAR(30),
    location_type   NVARCHAR(30),
    location_id     INT,
    address         NVARCHAR(200),
    planned_arrival DATETIME2,
    actual_arrival  DATETIME2,
    planned_departure DATETIME2,
    actual_departure DATETIME2,
    notes           NVARCHAR(MAX),
    CONSTRAINT UQ_TripStop_Sequence UNIQUE (trip_id, stop_sequence)
);

-- Maintenance log
CREATE TABLE MaintenanceLog (
    log_id          INT IDENTITY(1,1) PRIMARY KEY,
    vehicle_id      INT NOT NULL FOREIGN KEY REFERENCES Vehicle(vehicle_id),
    maintenance_date DATE NOT NULL,
    maintenance_type NVARCHAR(50) NOT NULL,
    description     NVARCHAR(MAX),
    cost            DECIMAL(18,2),
    odometer_reading INT,
    next_due_date   DATE,
    performed_by    NVARCHAR(100),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Fuel log
CREATE TABLE FuelLog (
    fuel_log_id     INT IDENTITY(1,1) PRIMARY KEY,
    vehicle_id      INT NOT NULL FOREIGN KEY REFERENCES Vehicle(vehicle_id),
    fuel_date       DATETIME2 NOT NULL DEFAULT GETDATE(),
    gallons         DECIMAL(18,3),
    cost            DECIMAL(18,2),
    odometer_reading INT,
    notes           NVARCHAR(MAX)
);

-- ========================================================================
-- 2. INDEXES (including new foreign keys and search columns)
-- ========================================================================

-- Existing indexes (from original script) plus new ones for racking and fleet
CREATE INDEX IX_Zone_Warehouse ON Zone(warehouse_id);
CREATE INDEX IX_Rack_Zone ON Rack(zone_id);
CREATE INDEX IX_Shelf_Rack ON Shelf(rack_id);
CREATE INDEX IX_Bin_Zone ON Bin(zone_id);
CREATE INDEX IX_Bin_Shelf ON Bin(shelf_id) WHERE shelf_id IS NOT NULL;

CREATE INDEX IX_PurchaseOrder_Supplier ON PurchaseOrder(supplier_id);
CREATE INDEX IX_PurchaseOrderLine_PO ON PurchaseOrderLine(po_id);
CREATE INDEX IX_PurchaseOrderLine_Product ON PurchaseOrderLine(product_id);
CREATE INDEX IX_Receiving_PO ON Receiving(po_id);
CREATE INDEX IX_ReceivingLine_Receiving ON ReceivingLine(receiving_id);
CREATE INDEX IX_ReceivingLine_POLine ON ReceivingLine(po_line_id);
CREATE INDEX IX_ReceivingLine_Product ON ReceivingLine(product_id);
CREATE INDEX IX_ReceivingLine_Bin ON ReceivingLine(bin_id);
CREATE INDEX IX_SalesOrder_Customer ON SalesOrder(customer_id);
CREATE INDEX IX_SalesOrderLine_SO ON SalesOrderLine(so_id);
CREATE INDEX IX_SalesOrderLine_Product ON SalesOrderLine(product_id);
CREATE INDEX IX_Shipment_SO ON Shipment(so_id);
CREATE INDEX IX_ShipmentLine_Shipment ON ShipmentLine(shipment_id);
CREATE INDEX IX_ShipmentLine_SOLine ON ShipmentLine(so_line_id);
CREATE INDEX IX_ShipmentLine_Product ON ShipmentLine(product_id);
CREATE INDEX IX_ShipmentLine_Bin ON ShipmentLine(bin_id);
CREATE INDEX IX_InventoryTransaction_Product ON InventoryTransaction(product_id);
CREATE INDEX IX_InventoryTransaction_Bin ON InventoryTransaction(bin_id);
CREATE INDEX IX_InventoryTransaction_Date ON InventoryTransaction(transaction_date);
CREATE INDEX IX_StockLevel_Product ON StockLevel(product_id);
CREATE INDEX IX_StockLevel_Bin ON StockLevel(bin_id);

-- Search / lookup indexes
CREATE INDEX IX_Product_SKU ON Product(sku);
CREATE INDEX IX_Product_Name ON Product(name);
CREATE INDEX IX_Warehouse_Code ON Warehouse(code);
CREATE INDEX IX_Bin_Code ON Bin(code);
CREATE INDEX IX_Bin_Barcode ON Bin(barcode) WHERE barcode IS NOT NULL;
CREATE INDEX IX_PurchaseOrder_PONumber ON PurchaseOrder(po_number);
CREATE INDEX IX_Receiving_ReceivingNumber ON Receiving(receiving_number);
CREATE INDEX IX_SalesOrder_SONumber ON SalesOrder(so_number);
CREATE INDEX IX_Shipment_ShipmentNumber ON Shipment(shipment_number);

-- Fleet indexes
CREATE INDEX IX_Vehicle_Warehouse ON Vehicle(warehouse_id);
CREATE INDEX IX_Vehicle_Status ON Vehicle(status);
CREATE INDEX IX_Trip_Vehicle ON Trip(vehicle_id);
CREATE INDEX IX_Trip_Driver ON Trip(driver_id);
CREATE INDEX IX_Trip_Status ON Trip(status);
CREATE INDEX IX_TripStop_Trip ON TripStop(trip_id);
CREATE INDEX IX_MaintenanceLog_Vehicle ON MaintenanceLog(vehicle_id);
CREATE INDEX IX_FuelLog_Vehicle ON FuelLog(vehicle_id);

-- ========================================================================
-- 3. FUNCTIONS (including fleet-related)
-- ========================================================================

-- Function: Get available stock for a product in a specific bin (or overall)
CREATE FUNCTION fn_GetAvailableStock (
    @product_id INT,
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
    ELSE
        SELECT @available = SUM(quantity_available)
        FROM StockLevel
        WHERE product_id = @product_id;
    RETURN ISNULL(@available, 0);
END;
GO

-- Function: Calculate total value of inventory (simplified)
CREATE FUNCTION fn_GetInventoryValuation (
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

-- Function: Get bin utilization percentage
CREATE FUNCTION fn_GetBinUtilization (@bin_id INT)
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

-- Function: Get vehicle utilization (percentage of time in use over a period)
CREATE FUNCTION fn_GetVehicleUtilization (
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

-- ========================================================================
-- 4. STORED PROCEDURES (existing plus new fleet procedures)
-- ========================================================================

-- Existing procedures (sp_ReceivePurchaseOrder, sp_ShipSalesOrder, sp_TransferStock, sp_AdjustInventory, sp_AllocateStock, sp_GetInventorySnapshot, sp_GenerateReplenishment)
-- [Include the original procedures here unchanged; for brevity I'll reference them but they are identical to previous version]

-- (Original procedures would be placed here; to save space, I'll include only the new fleet procedures and note that the previous ones are still present.)

-- Procedure: Create a new trip
CREATE PROCEDURE sp_CreateTrip
    @trip_number        NVARCHAR(50),
    @vehicle_id         INT,
    @driver_id          INT,
    @origin_type        NVARCHAR(30),
    @origin_id          INT,
    @destination_type   NVARCHAR(30),
    @destination_id     INT,
    @planned_stops      NVARCHAR(MAX) = NULL,   -- JSON array of stops
    @created_by         NVARCHAR(50),
    @trip_id            INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO Trip (trip_number, vehicle_id, driver_id, origin_type, origin_id, destination_type, destination_id, status, created_by)
        VALUES (@trip_number, @vehicle_id, @driver_id, @origin_type, @origin_id, @destination_type, @destination_id, 'PLANNED', @created_by);
        
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
CREATE PROCEDURE sp_StartTrip
    @trip_id        INT,
    @start_time     DATETIME2 = NULL,
    @updated_by     NVARCHAR(50)
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
CREATE PROCEDURE sp_CompleteTrip
    @trip_id        INT,
    @end_time       DATETIME2 = NULL,
    @updated_by     NVARCHAR(50)
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
CREATE PROCEDURE sp_LogMaintenance
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
    
    -- Optionally update vehicle status to MAINTENANCE if needed
    UPDATE Vehicle SET status = 'MAINTENANCE', modified_date = GETDATE()
    WHERE vehicle_id = @vehicle_id AND status = 'AVAILABLE';
END;
GO

-- Procedure: Log fuel purchase
CREATE PROCEDURE sp_LogFuel
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
CREATE PROCEDURE sp_GetFleetStatus
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

-- ========================================================================
-- (Include all previously defined stored procedures here:
-- sp_ReceivePurchaseOrder, sp_ShipSalesOrder, sp_TransferStock, 
-- sp_AdjustInventory, sp_AllocateStock, sp_GetInventorySnapshot, 
-- sp_GenerateReplenishment)
-- ========================================================================

-- ========================================================================
-- END OF SCRIPT
-- ========================================================================