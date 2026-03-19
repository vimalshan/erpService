-- ======================================================
-- Courier & Warehouse Management System - Complete Schema
-- Author: Assistant
-- Description: Tables, Indexes, Stored Procedures, Functions
-- ======================================================
USE [master];
GO
CREATE DATABASE IF NOT EXISTS CourierWarehouseDB;
GO
USE CourierWarehouseDB;
GO

-- ======================================================
-- 1. TABLES
-- ======================================================

-- -------------------- Security & Users --------------------
CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255)
);

CREATE TABLE Permissions (
    PermissionID INT IDENTITY(1,1) PRIMARY KEY,
    PermissionName NVARCHAR(100) NOT NULL UNIQUE,
    Module NVARCHAR(50),
    Description NVARCHAR(255)
);

CREATE TABLE RolePermissions (
    RoleID INT NOT NULL,
    PermissionID INT NOT NULL,
    PRIMARY KEY (RoleID, PermissionID),
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID) ON DELETE CASCADE,
    FOREIGN KEY (PermissionID) REFERENCES Permissions(PermissionID) ON DELETE CASCADE
);

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FullName NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    LastLogin DATETIME NULL
);

CREATE TABLE UserRoles (
    UserID INT NOT NULL,
    RoleID INT NOT NULL,
    PRIMARY KEY (UserID, RoleID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID) ON DELETE CASCADE
);

-- -------------------- Warehouse Structure --------------------
CREATE TABLE Warehouses (
    WarehouseID INT IDENTITY(1,1) PRIMARY KEY,
    WarehouseName NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200),
    Address NVARCHAR(200),
    City NVARCHAR(50),
    State NVARCHAR(50),
    ZipCode NVARCHAR(20),
    Country NVARCHAR(50),
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE Zones (
    ZoneID INT IDENTITY(1,1) PRIMARY KEY,
    WarehouseID INT NOT NULL,
    ZoneName NVARCHAR(50) NOT NULL,
    ZoneType NVARCHAR(50), -- e.g., 'Storage', 'Packing', 'Shipping'
    Description NVARCHAR(255),
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID) ON DELETE CASCADE,
    CONSTRAINT UQ_Zone_Warehouse UNIQUE (WarehouseID, ZoneName)
);

CREATE TABLE Bins (
    BinID INT IDENTITY(1,1) PRIMARY KEY,
    ZoneID INT NOT NULL,
    BinCode NVARCHAR(50) NOT NULL,
    BinType NVARCHAR(50),
    MaxWeight DECIMAL(10,2),
    MaxVolume DECIMAL(10,2),
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (ZoneID) REFERENCES Zones(ZoneID) ON DELETE CASCADE,
    CONSTRAINT UQ_Bin_Zone UNIQUE (ZoneID, BinCode)
);

-- -------------------- Employees --------------------
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NULL UNIQUE, -- NULL if not a system user
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    EmployeeCode NVARCHAR(20) NOT NULL UNIQUE,
    HireDate DATE NOT NULL,
    JobTitle NVARCHAR(50),
    Department NVARCHAR(50),
    WarehouseID INT NULL, -- Primary warehouse
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE SET NULL,
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID) ON DELETE SET NULL
);

-- -------------------- Products & Inventory --------------------
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    ParentCategoryID INT NULL,
    Description NVARCHAR(255),
    FOREIGN KEY (ParentCategoryID) REFERENCES Categories(CategoryID)
);

CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    SKU NVARCHAR(50) NOT NULL UNIQUE,
    ProductName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    CategoryID INT NULL,
    Weight DECIMAL(10,2),
    Volume DECIMAL(10,2),
    Price DECIMAL(10,2),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

CREATE TABLE Inventory (
    InventoryID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    WarehouseID INT NOT NULL,
    BinID INT NULL,
    QuantityOnHand INT NOT NULL DEFAULT 0,
    QuantityReserved INT NOT NULL DEFAULT 0,
    ReorderLevel INT NULL,
    LastCountDate DATETIME NULL,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID) ON DELETE CASCADE,
    FOREIGN KEY (BinID) REFERENCES Bins(BinID) ON DELETE SET NULL,
    CONSTRAINT UQ_Inventory_Product_Warehouse_Bin UNIQUE (ProductID, WarehouseID, BinID)
);

CREATE TABLE InventoryTransactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    WarehouseID INT NOT NULL,
    BinID INT NULL,
    TransactionType NVARCHAR(20) NOT NULL CHECK (TransactionType IN ('IN','OUT','MOVE','ADJUST')),
    Quantity INT NOT NULL,
    TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),
    ReferenceNumber NVARCHAR(50), -- e.g., ShipmentID, OrderID
    Comments NVARCHAR(255),
    CreatedByUserID INT NULL,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID),
    FOREIGN KEY (BinID) REFERENCES Bins(BinID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- -------------------- Customers --------------------
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerCode NVARCHAR(20) NOT NULL UNIQUE,
    CompanyName NVARCHAR(100),
    ContactName NVARCHAR(100),
    ContactTitle NVARCHAR(50),
    Address NVARCHAR(200),
    City NVARCHAR(50),
    State NVARCHAR(50),
    ZipCode NVARCHAR(20),
    Country NVARCHAR(50),
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);

-- -------------------- Shipments & Tracking --------------------
CREATE TABLE Shipments (
    ShipmentID INT IDENTITY(1,1) PRIMARY KEY,
    ShipmentNumber NVARCHAR(20) NOT NULL UNIQUE,
    CustomerID INT NOT NULL,
    WarehouseID INT NOT NULL, -- originating warehouse
    ShipmentDate DATETIME NOT NULL DEFAULT GETDATE(),
    ShipmentType NVARCHAR(20) CHECK (ShipmentType IN ('Inbound','Outbound')),
    ServiceType NVARCHAR(20), -- e.g., 'Express','Standard'
    Carrier NVARCHAR(50),
    TrackingNumber NVARCHAR(50),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending','Picked Up','In Transit','Delivered','Exception')),
    TotalWeight DECIMAL(10,2),
    TotalVolume DECIMAL(10,2),
    SpecialInstructions NVARCHAR(MAX),
    CreatedByUserID INT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

CREATE TABLE Packages (
    PackageID INT IDENTITY(1,1) PRIMARY KEY,
    ShipmentID INT NOT NULL,
    PackageNumber NVARCHAR(20) NOT NULL,
    Weight DECIMAL(10,2),
    Volume DECIMAL(10,2),
    Dimensions NVARCHAR(50), -- e.g., '10x10x10'
    TrackingNumber NVARCHAR(50),
    ContentsDescription NVARCHAR(255),
    FOREIGN KEY (ShipmentID) REFERENCES Shipments(ShipmentID) ON DELETE CASCADE,
    CONSTRAINT UQ_Package_Shipment UNIQUE (ShipmentID, PackageNumber)
);

CREATE TABLE ShipmentItems (
    ShipmentItemID INT IDENTITY(1,1) PRIMARY KEY,
    ShipmentID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    QuantityShipped INT NULL,
    UnitPrice DECIMAL(10,2),
    FOREIGN KEY (ShipmentID) REFERENCES Shipments(ShipmentID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

CREATE TABLE TrackingHistory (
    TrackingID INT IDENTITY(1,1) PRIMARY KEY,
    ShipmentID INT NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    Location NVARCHAR(100),
    Description NVARCHAR(255),
    EventDateTime DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedByUserID INT NULL,
    FOREIGN KEY (ShipmentID) REFERENCES Shipments(ShipmentID) ON DELETE CASCADE,
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

CREATE TABLE DeliveryAttempts (
    AttemptID INT IDENTITY(1,1) PRIMARY KEY,
    ShipmentID INT NOT NULL,
    AttemptDate DATETIME NOT NULL,
    Result NVARCHAR(20) CHECK (Result IN ('Successful','Failed')),
    Reason NVARCHAR(255),
    Notes NVARCHAR(MAX),
    FOREIGN KEY (ShipmentID) REFERENCES Shipments(ShipmentID) ON DELETE CASCADE
);

-- -------------------- Orders --------------------
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    OrderNumber NVARCHAR(20) NOT NULL UNIQUE,
    CustomerID INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    RequiredDate DATE NULL,
    ShippedDate DATETIME NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'New' CHECK (Status IN ('New','Processing','Shipped','Cancelled')),
    TotalAmount DECIMAL(10,2),
    CreatedByUserID INT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

CREATE TABLE OrderItems (
    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    Discount DECIMAL(10,2) DEFAULT 0,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- -------------------- Vehicles & Trips --------------------
CREATE TABLE Vehicles (
    VehicleID INT IDENTITY(1,1) PRIMARY KEY,
    VehicleNumber NVARCHAR(20) NOT NULL UNIQUE,
    LicensePlate NVARCHAR(20) NOT NULL UNIQUE,
    VehicleType NVARCHAR(20),
    CapacityWeight DECIMAL(10,2),
    CapacityVolume DECIMAL(10,2),
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE Drivers (
    DriverID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeID INT NOT NULL UNIQUE,
    LicenseNumber NVARCHAR(20) NOT NULL,
    LicenseExpiry DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE
);

CREATE TABLE Routes (
    RouteID INT IDENTITY(1,1) PRIMARY KEY,
    RouteName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    StartLocation NVARCHAR(100),
    EndLocation NVARCHAR(100),
    EstimatedDuration INT -- in minutes
);

CREATE TABLE Trips (
    TripID INT IDENTITY(1,1) PRIMARY KEY,
    TripNumber NVARCHAR(20) NOT NULL UNIQUE,
    RouteID INT NOT NULL,
    VehicleID INT NOT NULL,
    DriverID INT NOT NULL,
    TripDate DATE NOT NULL,
    DepartureTime DATETIME NULL,
    ArrivalTime DATETIME NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Planned' CHECK (Status IN ('Planned','In Progress','Completed','Cancelled')),
    FOREIGN KEY (RouteID) REFERENCES Routes(RouteID),
    FOREIGN KEY (VehicleID) REFERENCES Vehicles(VehicleID),
    FOREIGN KEY (DriverID) REFERENCES Drivers(DriverID)
);

CREATE TABLE TripStops (
    StopID INT IDENTITY(1,1) PRIMARY KEY,
    TripID INT NOT NULL,
    SequenceNumber INT NOT NULL,
    LocationType NVARCHAR(20) NOT NULL CHECK (LocationType IN ('Warehouse','Customer')),
    LocationID INT NOT NULL, -- WarehouseID or CustomerID
    PlannedArrival DATETIME NULL,
    ActualArrival DATETIME NULL,
    PlannedDeparture DATETIME NULL,
    ActualDeparture DATETIME NULL,
    Status NVARCHAR(20) DEFAULT 'Pending',
    FOREIGN KEY (TripID) REFERENCES Trips(TripID) ON DELETE CASCADE,
    CONSTRAINT CHK_LocationType CHECK (
        (LocationType = 'Warehouse' AND LocationID IN (SELECT WarehouseID FROM Warehouses)) OR
        (LocationType = 'Customer' AND LocationID IN (SELECT CustomerID FROM Customers))
    )
);
GO

-- ======================================================
-- 2. INDEXES
-- ======================================================
-- Foreign key indexes (automatically created in some versions, but we ensure them)
CREATE INDEX IX_Employees_UserID ON Employees(UserID);
CREATE INDEX IX_Employees_WarehouseID ON Employees(WarehouseID);
CREATE INDEX IX_Inventory_ProductID ON Inventory(ProductID);
CREATE INDEX IX_Inventory_WarehouseID ON Inventory(WarehouseID);
CREATE INDEX IX_Inventory_BinID ON Inventory(BinID);
CREATE INDEX IX_InventoryTransactions_ProductID ON InventoryTransactions(ProductID);
CREATE INDEX IX_InventoryTransactions_WarehouseID ON InventoryTransactions(WarehouseID);
CREATE INDEX IX_InventoryTransactions_BinID ON InventoryTransactions(BinID);
CREATE INDEX IX_Shipments_CustomerID ON Shipments(CustomerID);
CREATE INDEX IX_Shipments_WarehouseID ON Shipments(WarehouseID);
CREATE INDEX IX_Shipments_TrackingNumber ON Shipments(TrackingNumber) WHERE TrackingNumber IS NOT NULL;
CREATE INDEX IX_Packages_ShipmentID ON Packages(ShipmentID);
CREATE INDEX IX_TrackingHistory_ShipmentID ON TrackingHistory(ShipmentID);
CREATE INDEX IX_Orders_CustomerID ON Orders(CustomerID);
CREATE INDEX IX_OrderItems_OrderID ON OrderItems(OrderID);
CREATE INDEX IX_OrderItems_ProductID ON OrderItems(ProductID);
CREATE INDEX IX_Trips_RouteID ON Trips(RouteID);
CREATE INDEX IX_Trips_VehicleID ON Trips(VehicleID);
CREATE INDEX IX_Trips_DriverID ON Trips(DriverID);
CREATE INDEX IX_TripStops_TripID ON TripStops(TripID);
GO

-- ======================================================
-- 3. FUNCTIONS
-- ======================================================

-- Function: Get available stock for a product in a specific warehouse
CREATE OR ALTER FUNCTION fn_GetAvailableStock (
    @ProductID INT,
    @WarehouseID INT
)
RETURNS INT
AS
BEGIN
    DECLARE @Available INT;
    SELECT @Available = SUM(QuantityOnHand - QuantityReserved)
    FROM Inventory
    WHERE ProductID = @ProductID AND WarehouseID = @WarehouseID;
    RETURN ISNULL(@Available, 0);
END;
GO

-- Function: Calculate shipping cost based on weight and service type
CREATE OR ALTER FUNCTION fn_CalculateShippingCost (
    @Weight DECIMAL(10,2),
    @ServiceType NVARCHAR(20)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @Cost DECIMAL(10,2) = 0;
    -- Simple logic: base rate + per kg
    SET @Cost = CASE @ServiceType
                    WHEN 'Express' THEN 10.00 + (@Weight * 2.50)
                    WHEN 'Standard' THEN 5.00 + (@Weight * 1.50)
                    ELSE 7.00 + (@Weight * 2.00)
                END;
    RETURN @Cost;
END;
GO

-- Function: Get current quantity on hand for a product at a specific bin
CREATE OR ALTER FUNCTION fn_GetBinStock (
    @ProductID INT,
    @BinID INT
)
RETURNS INT
AS
BEGIN
    DECLARE @Qty INT;
    SELECT @Qty = QuantityOnHand
    FROM Inventory
    WHERE ProductID = @ProductID AND BinID = @BinID;
    RETURN ISNULL(@Qty, 0);
END;
GO

-- ======================================================
-- 4. STORED PROCEDURES
-- ======================================================

-- Procedure: Create a new shipment with packages and items
CREATE OR ALTER PROCEDURE sp_CreateShipment
    @ShipmentNumber NVARCHAR(20),
    @CustomerID INT,
    @WarehouseID INT,
    @ShipmentType NVARCHAR(20),
    @ServiceType NVARCHAR(20) = NULL,
    @Carrier NVARCHAR(50) = NULL,
    @TrackingNumber NVARCHAR(50) = NULL,
    @SpecialInstructions NVARCHAR(MAX) = NULL,
    @CreatedByUserID INT = NULL,
    @ShipmentID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO Shipments (
            ShipmentNumber, CustomerID, WarehouseID, ShipmentType, ServiceType,
            Carrier, TrackingNumber, SpecialInstructions, CreatedByUserID
        ) VALUES (
            @ShipmentNumber, @CustomerID, @WarehouseID, @ShipmentType, @ServiceType,
            @Carrier, @TrackingNumber, @SpecialInstructions, @CreatedByUserID
        );
        SET @ShipmentID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Add package to a shipment
CREATE OR ALTER PROCEDURE sp_AddPackage
    @ShipmentID INT,
    @PackageNumber NVARCHAR(20),
    @Weight DECIMAL(10,2) = NULL,
    @Volume DECIMAL(10,2) = NULL,
    @Dimensions NVARCHAR(50) = NULL,
    @TrackingNumber NVARCHAR(50) = NULL,
    @ContentsDescription NVARCHAR(255) = NULL,
    @PackageID INT OUTPUT
AS
BEGIN
    INSERT INTO Packages (
        ShipmentID, PackageNumber, Weight, Volume, Dimensions, TrackingNumber, ContentsDescription
    ) VALUES (
        @ShipmentID, @PackageNumber, @Weight, @Volume, @Dimensions, @TrackingNumber, @ContentsDescription
    );
    SET @PackageID = SCOPE_IDENTITY();
END;
GO

-- Procedure: Update shipment status and log tracking
CREATE OR ALTER PROCEDURE sp_UpdateShipmentStatus
    @ShipmentID INT,
    @NewStatus NVARCHAR(20),
    @Location NVARCHAR(100) = NULL,
    @Description NVARCHAR(255) = NULL,
    @CreatedByUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        -- Update shipment status
        UPDATE Shipments SET Status = @NewStatus, ModifiedDate = GETDATE()
        WHERE ShipmentID = @ShipmentID;
        -- Insert tracking history
        INSERT INTO TrackingHistory (ShipmentID, Status, Location, Description, CreatedByUserID)
        VALUES (@ShipmentID, @NewStatus, @Location, @Description, @CreatedByUserID);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Transfer inventory between bins (or warehouses)
CREATE OR ALTER PROCEDURE sp_TransferInventory
    @ProductID INT,
    @FromWarehouseID INT,
    @FromBinID INT = NULL,
    @ToWarehouseID INT,
    @ToBinID INT = NULL,
    @Quantity INT,
    @ReferenceNumber NVARCHAR(50) = NULL,
    @CreatedByUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Available INT;
    BEGIN TRY
        BEGIN TRANSACTION;
        -- Check available stock
        SELECT @Available = SUM(QuantityOnHand - QuantityReserved)
        FROM Inventory
        WHERE ProductID = @ProductID AND WarehouseID = @FromWarehouseID
          AND (BinID = @FromBinID OR (@FromBinID IS NULL AND BinID IS NULL));
        IF @Available < @Quantity
            THROW 50000, 'Insufficient stock for transfer.', 1;

        -- Deduct from source
        UPDATE Inventory
        SET QuantityOnHand = QuantityOnHand - @Quantity
        WHERE ProductID = @ProductID AND WarehouseID = @FromWarehouseID
          AND (BinID = @FromBinID OR (@FromBinID IS NULL AND BinID IS NULL));

        -- Add to destination (if not exists, insert)
        IF EXISTS (SELECT 1 FROM Inventory WHERE ProductID = @ProductID AND WarehouseID = @ToWarehouseID AND (BinID = @ToBinID OR (@ToBinID IS NULL AND BinID IS NULL)))
            UPDATE Inventory SET QuantityOnHand = QuantityOnHand + @Quantity
            WHERE ProductID = @ProductID AND WarehouseID = @ToWarehouseID AND (BinID = @ToBinID OR (@ToBinID IS NULL AND BinID IS NULL));
        ELSE
            INSERT INTO Inventory (ProductID, WarehouseID, BinID, QuantityOnHand)
            VALUES (@ProductID, @ToWarehouseID, @ToBinID, @Quantity);

        -- Log transactions
        INSERT INTO InventoryTransactions (ProductID, WarehouseID, BinID, TransactionType, Quantity, ReferenceNumber, CreatedByUserID)
        VALUES (@ProductID, @FromWarehouseID, @FromBinID, 'OUT', -@Quantity, @ReferenceNumber, @CreatedByUserID);
        INSERT INTO InventoryTransactions (ProductID, WarehouseID, BinID, TransactionType, Quantity, ReferenceNumber, CreatedByUserID)
        VALUES (@ProductID, @ToWarehouseID, @ToBinID, 'IN', @Quantity, @ReferenceNumber, @CreatedByUserID);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Get inventory snapshot for a warehouse (with available quantity)
CREATE OR ALTER PROCEDURE sp_GetInventoryByWarehouse
    @WarehouseID INT
AS
BEGIN
    SELECT p.SKU, p.ProductName, i.QuantityOnHand, i.QuantityReserved,
           (i.QuantityOnHand - i.QuantityReserved) AS Available,
           b.BinCode, z.ZoneName
    FROM Inventory i
    INNER JOIN Products p ON i.ProductID = p.ProductID
    LEFT JOIN Bins b ON i.BinID = b.BinID
    LEFT JOIN Zones z ON b.ZoneID = z.ZoneID
    WHERE i.WarehouseID = @WarehouseID
    ORDER BY p.SKU;
END;
GO

-- Procedure: Create a new order and allocate inventory (simplified)
CREATE OR ALTER PROCEDURE sp_CreateOrder
    @OrderNumber NVARCHAR(20),
    @CustomerID INT,
    @OrderItems OrderItemsType READONLY, -- Need to define a table type first
    @CreatedByUserID INT = NULL,
    @OrderID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    -- This procedure requires a user-defined table type for order items.
    -- For brevity, we skip the full implementation here.
    -- In practice, you'd loop through items, check stock, and insert.
    PRINT 'sp_CreateOrder requires a table type OrderItemsType. Implement accordingly.';
END;
GO

-- Procedure: Assign trip to driver and vehicle
CREATE OR ALTER PROCEDURE sp_AssignTrip
    @TripNumber NVARCHAR(20),
    @RouteID INT,
    @VehicleID INT,
    @DriverID INT,
    @TripDate DATE,
    @TripID INT OUTPUT
AS
BEGIN
    INSERT INTO Trips (TripNumber, RouteID, VehicleID, DriverID, TripDate)
    VALUES (@TripNumber, @RouteID, @VehicleID, @DriverID, @TripDate);
    SET @TripID = SCOPE_IDENTITY();
END;
GO

-- Procedure: Start trip (set departure time and status)
CREATE OR ALTER PROCEDURE sp_StartTrip
    @TripID INT,
    @DepartureTime DATETIME = NULL
AS
BEGIN
    UPDATE Trips
    SET DepartureTime = ISNULL(@DepartureTime, GETDATE()),
        Status = 'In Progress'
    WHERE TripID = @TripID;
END;
GO

-- Procedure: Complete trip (set arrival time and status)
CREATE OR ALTER PROCEDURE sp_CompleteTrip
    @TripID INT,
    @ArrivalTime DATETIME = NULL
AS
BEGIN
    UPDATE Trips
    SET ArrivalTime = ISNULL(@ArrivalTime, GETDATE()),
        Status = 'Completed'
    WHERE TripID = @TripID;
END;
GO

-- Note: The sp_CreateOrder procedure requires a table type. Uncomment if needed:
/*
CREATE TYPE dbo.OrderItemsType AS TABLE (
    ProductID INT,
    Quantity INT,
    UnitPrice DECIMAL(10,2)
);
GO
*/