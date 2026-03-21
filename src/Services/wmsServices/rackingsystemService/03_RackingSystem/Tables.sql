-- ======================================================
-- Module: Racking System
-- Tables: Rack, Shelf, Bin
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- Rack (physical structure within a zone)
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
GO

-- Shelf (level/position within a rack)
CREATE TABLE Shelf (
    shelf_id        INT IDENTITY(1,1) PRIMARY KEY,
    rack_id         INT NOT NULL FOREIGN KEY REFERENCES Rack(rack_id),
    shelf_level     INT NOT NULL,
    shelf_position  INT NOT NULL,
    code            NVARCHAR(30) NOT NULL,
    capacity_qty    DECIMAL(18,3),
    capacity_weight DECIMAL(18,3),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Shelf_Rack_Level_Position UNIQUE (rack_id, shelf_level, shelf_position)
);
GO

-- Storage bin (specific location)
CREATE TABLE Bin (
    bin_id          INT IDENTITY(1,1) PRIMARY KEY,
    zone_id         INT NOT NULL FOREIGN KEY REFERENCES Zone(zone_id),
    shelf_id        INT NULL FOREIGN KEY REFERENCES Shelf(shelf_id),
    code            NVARCHAR(30) NOT NULL,
    barcode         NVARCHAR(50),
    bin_type        NVARCHAR(50),
    capacity_qty    DECIMAL(18,3),
    capacity_weight DECIMAL(18,3),
    capacity_volume DECIMAL(18,3),
    status          NVARCHAR(20) NOT NULL DEFAULT 'AVAILABLE' CHECK (status IN ('AVAILABLE', 'OCCUPIED', 'BLOCKED', 'FULL')),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Bin_Zone_Code UNIQUE (zone_id, code)
);
GO
