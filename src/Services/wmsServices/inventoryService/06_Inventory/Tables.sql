-- ======================================================
-- Module: Inventory
-- Tables: StockLevel, InventoryTransaction
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- Current stock snapshot (denormalized for performance)
CREATE TABLE StockLevel (
    stock_level_id     BIGINT IDENTITY(1,1) PRIMARY KEY,
    product_id         INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    warehouse_id       INT NOT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    bin_id             INT NOT NULL FOREIGN KEY REFERENCES Bin(bin_id),
    quantity_on_hand   DECIMAL(18,3) NOT NULL DEFAULT 0,
    quantity_allocated DECIMAL(18,3) NOT NULL DEFAULT 0,
    quantity_reserved  DECIMAL(18,3) NOT NULL DEFAULT 0,
    quantity_available AS (quantity_on_hand - quantity_allocated - quantity_reserved) PERSISTED,
    reorder_level      INT NULL,
    last_count_date    DATETIME2 NULL,
    last_updated       DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_StockLevel_Product_Bin UNIQUE (product_id, bin_id)
);
GO

-- Inventory transactions ledger
CREATE TABLE InventoryTransaction (
    transaction_id   BIGINT IDENTITY(1,1) PRIMARY KEY,
    product_id       INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    warehouse_id     INT NOT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    bin_id           INT NULL FOREIGN KEY REFERENCES Bin(bin_id),
    transaction_type NVARCHAR(30) NOT NULL CHECK (transaction_type IN ('RECEIPT', 'SHIPMENT', 'MOVE_OUT', 'MOVE_IN', 'ADJUSTMENT', 'PICK', 'PACK', 'RETURN', 'IN', 'OUT')),
    quantity_change  DECIMAL(18,3) NOT NULL,
    reference_type   NVARCHAR(30),
    reference_id     INT,
    reference_number NVARCHAR(50),
    transaction_date DATETIME2 NOT NULL DEFAULT GETDATE(),
    created_by       NVARCHAR(50),
    comments         NVARCHAR(255),
    notes            NVARCHAR(MAX)
);
GO
