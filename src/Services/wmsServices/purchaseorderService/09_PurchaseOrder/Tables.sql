-- ======================================================
-- Module: Purchase Orders
-- Tables: PurchaseOrder, PurchaseOrderLine
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- Purchase Order header
CREATE TABLE PurchaseOrder (
    po_id           INT IDENTITY(1,1) PRIMARY KEY,
    po_number       NVARCHAR(50) NOT NULL UNIQUE,
    supplier_id     INT NOT NULL FOREIGN KEY REFERENCES Supplier(supplier_id),
    warehouse_id    INT NOT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    order_date      DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    expected_date   DATE,
    status          NVARCHAR(30) NOT NULL DEFAULT 'DRAFT' CHECK (status IN ('DRAFT', 'CONFIRMED', 'RECEIVING', 'COMPLETED', 'CANCELLED')),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

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
GO
