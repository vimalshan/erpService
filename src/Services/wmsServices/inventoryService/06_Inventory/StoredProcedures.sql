-- ======================================================
-- Module: Inventory - Stored Procedures
-- ======================================================

-- Procedure: Transfer inventory between bins/warehouses
CREATE OR ALTER PROCEDURE sp_TransferInventory
    @product_id      INT,
    @from_warehouse_id INT,
    @from_bin_id     INT = NULL,
    @to_warehouse_id INT,
    @to_bin_id       INT = NULL,
    @quantity        DECIMAL(18,3),
    @reference_number NVARCHAR(50) = NULL,
    @created_by      NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @available DECIMAL(18,3);
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check available stock
        SELECT @available = SUM(quantity_on_hand - quantity_allocated - quantity_reserved)
        FROM StockLevel
        WHERE product_id = @product_id AND warehouse_id = @from_warehouse_id
          AND (@from_bin_id IS NULL OR bin_id = @from_bin_id);

        IF @available < @quantity
            THROW 50000, 'Insufficient stock for transfer.', 1;

        -- Deduct from source
        UPDATE StockLevel
        SET quantity_on_hand = quantity_on_hand - @quantity,
            last_updated = GETDATE()
        WHERE product_id = @product_id AND warehouse_id = @from_warehouse_id
          AND (@from_bin_id IS NULL OR bin_id = @from_bin_id);

        -- Add to destination (upsert)
        IF EXISTS (SELECT 1 FROM StockLevel WHERE product_id = @product_id AND warehouse_id = @to_warehouse_id AND (@to_bin_id IS NULL OR bin_id = @to_bin_id))
            UPDATE StockLevel SET quantity_on_hand = quantity_on_hand + @quantity, last_updated = GETDATE()
            WHERE product_id = @product_id AND warehouse_id = @to_warehouse_id AND (@to_bin_id IS NULL OR bin_id = @to_bin_id);
        ELSE
            INSERT INTO StockLevel (product_id, warehouse_id, bin_id, quantity_on_hand)
            VALUES (@product_id, @to_warehouse_id, @to_bin_id, @quantity);

        -- Log transactions
        INSERT INTO InventoryTransaction (product_id, warehouse_id, bin_id, transaction_type, quantity_change, reference_number, created_by)
        VALUES (@product_id, @from_warehouse_id, @from_bin_id, 'MOVE_OUT', -@quantity, @reference_number, @created_by);

        INSERT INTO InventoryTransaction (product_id, warehouse_id, bin_id, transaction_type, quantity_change, reference_number, created_by)
        VALUES (@product_id, @to_warehouse_id, @to_bin_id, 'MOVE_IN', @quantity, @reference_number, @created_by);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Get inventory snapshot for a warehouse
CREATE OR ALTER PROCEDURE sp_GetInventoryByWarehouse
    @warehouse_id INT
AS
BEGIN
    SELECT p.sku, p.name AS product_name, 
           s.quantity_on_hand, s.quantity_allocated, s.quantity_reserved,
           s.quantity_available,
           b.code AS bin_code, z.name AS zone_name
    FROM StockLevel s
    INNER JOIN Product p ON s.product_id = p.product_id
    LEFT JOIN Bin b ON s.bin_id = b.bin_id
    LEFT JOIN Zone z ON b.zone_id = z.zone_id
    WHERE s.warehouse_id = @warehouse_id
    ORDER BY p.sku;
END;
GO

-- Procedure: Adjust inventory
CREATE OR ALTER PROCEDURE sp_AdjustInventory
    @product_id     INT,
    @warehouse_id   INT,
    @bin_id         INT,
    @new_quantity   DECIMAL(18,3),
    @reason         NVARCHAR(255),
    @adjusted_by    NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @current_qty DECIMAL(18,3), @diff DECIMAL(18,3);
    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @current_qty = ISNULL(quantity_on_hand, 0)
        FROM StockLevel
        WHERE product_id = @product_id AND bin_id = @bin_id;

        SET @diff = @new_quantity - ISNULL(@current_qty, 0);

        IF EXISTS (SELECT 1 FROM StockLevel WHERE product_id = @product_id AND bin_id = @bin_id)
            UPDATE StockLevel SET quantity_on_hand = @new_quantity, last_updated = GETDATE()
            WHERE product_id = @product_id AND bin_id = @bin_id;
        ELSE
            INSERT INTO StockLevel (product_id, warehouse_id, bin_id, quantity_on_hand)
            VALUES (@product_id, @warehouse_id, @bin_id, @new_quantity);

        INSERT INTO InventoryTransaction (product_id, warehouse_id, bin_id, transaction_type, quantity_change, created_by, comments)
        VALUES (@product_id, @warehouse_id, @bin_id, 'ADJUSTMENT', @diff, @adjusted_by, @reason);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
