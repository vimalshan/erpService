-- ======================================================
-- Module: Sales Orders
-- Tables: SalesOrder, SalesOrderLine
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- Sales Order header
CREATE TABLE SalesOrder (
    so_id           INT IDENTITY(1,1) PRIMARY KEY,
    so_number       NVARCHAR(50) NOT NULL UNIQUE,
    customer_id     INT NOT NULL FOREIGN KEY REFERENCES Customer(customer_id),
    warehouse_id    INT NOT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    order_date      DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    requested_date  DATE,
    status          NVARCHAR(30) NOT NULL DEFAULT 'DRAFT' CHECK (status IN ('DRAFT', 'CONFIRMED', 'PICKING', 'SHIPPING', 'COMPLETED', 'CANCELLED')),
    total_amount    DECIMAL(18,2),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Sales Order line items
CREATE TABLE SalesOrderLine (
    so_line_id      INT IDENTITY(1,1) PRIMARY KEY,
    so_id           INT NOT NULL FOREIGN KEY REFERENCES SalesOrder(so_id),
    product_id      INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    line_number     INT NOT NULL,
    quantity_ordered DECIMAL(18,3) NOT NULL CHECK (quantity_ordered > 0),
    quantity_shipped DECIMAL(18,3) NOT NULL DEFAULT 0,
    unit_price      DECIMAL(18,4),
    discount        DECIMAL(18,2) DEFAULT 0,
    notes           NVARCHAR(MAX),
    CONSTRAINT UQ_SOLine_SO_Line UNIQUE (so_id, line_number)
);
GO
