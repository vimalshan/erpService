-- ======================================================
-- Module: Fleet Management
-- Tables: Vehicle, Driver, Route, Trip, TripStop, MaintenanceLog, FuelLog
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

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
    warehouse_id    INT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    notes           NVARCHAR(MAX),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Driver master
CREATE TABLE Driver (
    driver_id       INT IDENTITY(1,1) PRIMARY KEY,
    code            NVARCHAR(30) NOT NULL UNIQUE,
    employee_id     INT NULL UNIQUE FOREIGN KEY REFERENCES Employee(employee_id),
    full_name       NVARCHAR(100) NOT NULL,
    license_number  NVARCHAR(50) NOT NULL,
    license_expiry  DATE NOT NULL,
    phone           NVARCHAR(30),
    email           NVARCHAR(100),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Route definition
CREATE TABLE Route (
    route_id        INT IDENTITY(1,1) PRIMARY KEY,
    route_name      NVARCHAR(50) NOT NULL UNIQUE,
    description     NVARCHAR(255),
    start_location  NVARCHAR(100),
    end_location    NVARCHAR(100),
    estimated_duration INT,    -- in minutes
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Trip header
CREATE TABLE Trip (
    trip_id         INT IDENTITY(1,1) PRIMARY KEY,
    trip_number     NVARCHAR(50) NOT NULL UNIQUE,
    route_id        INT NULL FOREIGN KEY REFERENCES Route(route_id),
    vehicle_id      INT NOT NULL FOREIGN KEY REFERENCES Vehicle(vehicle_id),
    driver_id       INT NOT NULL FOREIGN KEY REFERENCES Driver(driver_id),
    trip_date       DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    start_time      DATETIME2,
    end_time        DATETIME2,
    origin_type     NVARCHAR(30),
    origin_id       INT,
    destination_type NVARCHAR(30),
    destination_id   INT,
    status          NVARCHAR(30) NOT NULL DEFAULT 'PLANNED' CHECK (status IN ('PLANNED', 'IN_PROGRESS', 'COMPLETED', 'CANCELLED')),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Trip stops (intermediate stops)
CREATE TABLE TripStop (
    stop_id         INT IDENTITY(1,1) PRIMARY KEY,
    trip_id         INT NOT NULL FOREIGN KEY REFERENCES Trip(trip_id) ON DELETE CASCADE,
    stop_sequence   INT NOT NULL,
    stop_type       NVARCHAR(30),
    location_type   NVARCHAR(30),          -- 'WAREHOUSE', 'CUSTOMER', 'SUPPLIER'
    location_id     INT,
    address         NVARCHAR(200),
    planned_arrival DATETIME2,
    actual_arrival  DATETIME2,
    planned_departure DATETIME2,
    actual_departure DATETIME2,
    status          NVARCHAR(20) DEFAULT 'PENDING',
    notes           NVARCHAR(MAX),
    CONSTRAINT UQ_TripStop_Sequence UNIQUE (trip_id, stop_sequence)
);
GO

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
GO

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
GO
