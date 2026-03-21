-- ======================================================
-- Module: Orders
-- Tables: Order, OrderItem
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- Order header
CREATE TABLE [Order] (
    order_id        INT IDENTITY(1,1) PRIMARY KEY,
    order_number    NVARCHAR(50) NOT NULL UNIQUE,
    customer_id     INT NOT NULL FOREIGN KEY REFERENCES Customer(customer_id),
    order_date      DATETIME2 NOT NULL DEFAULT GETDATE(),
    required_date   DATE NULL,
    shipped_date    DATETIME2 NULL,
    status          NVARCHAR(20) NOT NULL DEFAULT 'NEW' CHECK (status IN ('NEW', 'PROCESSING', 'SHIPPED', 'CANCELLED')),
    total_amount    DECIMAL(18,2),
    created_by      NVARCHAR(50),
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Order line items
CREATE TABLE OrderItem (
    order_item_id   INT IDENTITY(1,1) PRIMARY KEY,
    order_id        INT NOT NULL FOREIGN KEY REFERENCES [Order](order_id) ON DELETE CASCADE,
    product_id      INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    quantity        INT NOT NULL,
    unit_price      DECIMAL(18,4) NOT NULL,
    discount        DECIMAL(18,2) DEFAULT 0,
    notes           NVARCHAR(MAX)
);
GO
