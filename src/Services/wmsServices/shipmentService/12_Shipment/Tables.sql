-- ======================================================
-- Module: Shipment & Tracking
-- Tables: Shipment, ShipmentLine, Package, TrackingHistory, DeliveryAttempt
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- Shipment header
CREATE TABLE Shipment (
    shipment_id     INT IDENTITY(1,1) PRIMARY KEY,
    shipment_number NVARCHAR(50) NOT NULL UNIQUE,
    so_id           INT NULL FOREIGN KEY REFERENCES SalesOrder(so_id),
    customer_id     INT NOT NULL FOREIGN KEY REFERENCES Customer(customer_id),
    warehouse_id    INT NOT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    shipment_type   NVARCHAR(20) CHECK (shipment_type IN ('INBOUND', 'OUTBOUND')),
    service_type    NVARCHAR(20),          -- 'Express', 'Standard'
    shipped_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    status          NVARCHAR(30) NOT NULL DEFAULT 'PENDING' CHECK (status IN ('PENDING', 'OPEN', 'PICKED_UP', 'IN_TRANSIT', 'SHIPPED', 'DELIVERED', 'EXCEPTION', 'CANCELLED')),
    tracking_number NVARCHAR(100),
    carrier         NVARCHAR(50),
    total_weight    DECIMAL(18,3),
    total_volume    DECIMAL(18,3),
    special_instructions NVARCHAR(MAX),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Shipment line (items picked from bins)
CREATE TABLE ShipmentLine (
    shipment_line_id INT IDENTITY(1,1) PRIMARY KEY,
    shipment_id     INT NOT NULL FOREIGN KEY REFERENCES Shipment(shipment_id),
    so_line_id      INT NULL FOREIGN KEY REFERENCES SalesOrderLine(so_line_id),
    product_id      INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    bin_id          INT NOT NULL FOREIGN KEY REFERENCES Bin(bin_id),
    quantity_shipped DECIMAL(18,3) NOT NULL CHECK (quantity_shipped > 0),
    unit_price      DECIMAL(18,4),
    lot_number      NVARCHAR(50),
    expiry_date     DATE,
    notes           NVARCHAR(MAX)
);
GO

-- Package within a shipment
CREATE TABLE Package (
    package_id      INT IDENTITY(1,1) PRIMARY KEY,
    shipment_id     INT NOT NULL FOREIGN KEY REFERENCES Shipment(shipment_id) ON DELETE CASCADE,
    package_number  NVARCHAR(20) NOT NULL,
    weight          DECIMAL(10,2),
    volume          DECIMAL(10,2),
    dimensions      NVARCHAR(50),          -- e.g., '10x10x10'
    tracking_number NVARCHAR(50),
    contents_description NVARCHAR(255),
    CONSTRAINT UQ_Package_Shipment UNIQUE (shipment_id, package_number)
);
GO

-- Tracking history
CREATE TABLE TrackingHistory (
    tracking_id     INT IDENTITY(1,1) PRIMARY KEY,
    shipment_id     INT NOT NULL FOREIGN KEY REFERENCES Shipment(shipment_id) ON DELETE CASCADE,
    status          NVARCHAR(30) NOT NULL,
    location        NVARCHAR(100),
    description     NVARCHAR(255),
    event_datetime  DATETIME2 NOT NULL DEFAULT GETDATE(),
    created_by      NVARCHAR(50)
);
GO

-- Delivery attempts
CREATE TABLE DeliveryAttempt (
    attempt_id      INT IDENTITY(1,1) PRIMARY KEY,
    shipment_id     INT NOT NULL FOREIGN KEY REFERENCES Shipment(shipment_id) ON DELETE CASCADE,
    attempt_date    DATETIME2 NOT NULL,
    result          NVARCHAR(20) CHECK (result IN ('SUCCESSFUL', 'FAILED')),
    reason          NVARCHAR(255),
    notes           NVARCHAR(MAX)
);
GO
