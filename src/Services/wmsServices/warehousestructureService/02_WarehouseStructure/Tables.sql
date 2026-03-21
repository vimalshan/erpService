-- ======================================================
-- Module: Warehouse Structure
-- Tables: Warehouse, Zone
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

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
    phone           NVARCHAR(20),
    email           NVARCHAR(100),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Zone within a warehouse (e.g., Receiving, Shipping, Bulk Storage)
CREATE TABLE Zone (
    zone_id         INT IDENTITY(1,1) PRIMARY KEY,
    warehouse_id    INT NOT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    code            NVARCHAR(20) NOT NULL,
    name            NVARCHAR(100) NOT NULL,
    zone_type       NVARCHAR(30) NOT NULL CHECK (zone_type IN ('RECEIVING', 'STORAGE', 'PICKING', 'SHIPPING', 'RETURNS', 'PACKING')),
    description     NVARCHAR(255),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Zone_Warehouse_Code UNIQUE (warehouse_id, code)
);
GO
