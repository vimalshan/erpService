-- ======================================================
-- Module: Receiving
-- Tables: Receiving, ReceivingLine
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- Receiving header
CREATE TABLE Receiving (
    receiving_id    INT IDENTITY(1,1) PRIMARY KEY,
    receiving_number NVARCHAR(50) NOT NULL UNIQUE,
    po_id           INT NOT NULL FOREIGN KEY REFERENCES PurchaseOrder(po_id),
    warehouse_id    INT NOT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    received_date   DATETIME2 NOT NULL DEFAULT GETDATE(),
    status          NVARCHAR(30) NOT NULL DEFAULT 'OPEN' CHECK (status IN ('OPEN', 'CLOSED', 'CANCELLED')),
    notes           NVARCHAR(MAX),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

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
GO
