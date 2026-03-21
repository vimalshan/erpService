-- ======================================================
-- Module: Product
-- Tables: Category, Product
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- Category (hierarchical)
CREATE TABLE Category (
    category_id       INT IDENTITY(1,1) PRIMARY KEY,
    category_name     NVARCHAR(100) NOT NULL,
    parent_category_id INT NULL,
    description       NVARCHAR(255),
    FOREIGN KEY (parent_category_id) REFERENCES Category(category_id)
);
GO

-- Product master
CREATE TABLE Product (
    product_id      INT IDENTITY(1,1) PRIMARY KEY,
    sku             NVARCHAR(50) NOT NULL UNIQUE,
    name            NVARCHAR(200) NOT NULL,
    description     NVARCHAR(MAX),
    category_id     INT NULL FOREIGN KEY REFERENCES Category(category_id),
    unit_of_measure NVARCHAR(20) NOT NULL DEFAULT 'EA',
    weight_per_unit DECIMAL(18,3),
    volume_per_unit DECIMAL(18,3),
    price           DECIMAL(18,4),
    reorder_point   DECIMAL(18,3),
    reorder_quantity DECIMAL(18,3),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO
