-- ======================================================
-- Module: Receiving - Stored Procedures
-- ======================================================

-- Procedure: Receive items against a Purchase Order
CREATE OR ALTER PROCEDURE sp_ReceivePurchaseOrder
    @receiving_number  NVARCHAR(50),
    @po_id             INT,
    @warehouse_id      INT,
    @items_json        NVARCHAR(MAX),   -- JSON array of {po_line_id, product_id, bin_id, quantity, lot_number, expiry_date}
    @created_by        NVARCHAR(50),
    @receiving_id      INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Create receiving header
        INSERT INTO Receiving (receiving_number, po_id, warehouse_id, status, created_by)
        VALUES (@receiving_number, @po_id, @warehouse_id, 'OPEN', @created_by);
        SET @receiving_id = SCOPE_IDENTITY();

        -- Insert receiving lines from JSON
        INSERT INTO ReceivingLine (receiving_id, po_line_id, product_id, bin_id, quantity_received, lot_number, expiry_date)
        SELECT 
            @receiving_id,
            po_line_id,
            product_id,
            bin_id,
            quantity,
            lot_number,
            expiry_date
        FROM OPENJSON(@items_json)
        WITH (
            po_line_id   INT           '$.po_line_id',
            product_id   INT           '$.product_id',
            bin_id       INT           '$.bin_id',
            quantity     DECIMAL(18,3) '$.quantity',
            lot_number   NVARCHAR(50)  '$.lot_number',
            expiry_date  DATE          '$.expiry_date'
        );

        -- Update PO line received quantities
        UPDATE pol
        SET pol.quantity_received = pol.quantity_received + rl.quantity_received
        FROM PurchaseOrderLine pol
        INNER JOIN ReceivingLine rl ON pol.po_line_id = rl.po_line_id
        WHERE rl.receiving_id = @receiving_id;

        -- Update stock levels
        MERGE StockLevel AS target
        USING (
            SELECT product_id, bin_id, SUM(quantity_received) AS qty
            FROM ReceivingLine WHERE receiving_id = @receiving_id
            GROUP BY product_id, bin_id
        ) AS source ON target.product_id = source.product_id AND target.bin_id = source.bin_id
        WHEN MATCHED THEN
            UPDATE SET quantity_on_hand = target.quantity_on_hand + source.qty, last_updated = GETDATE()
        WHEN NOT MATCHED THEN
            INSERT (product_id, warehouse_id, bin_id, quantity_on_hand)
            VALUES (source.product_id, @warehouse_id, source.bin_id, source.qty);

        -- Log inventory transactions
        INSERT INTO InventoryTransaction (product_id, warehouse_id, bin_id, transaction_type, quantity_change, reference_type, reference_id, created_by)
        SELECT product_id, @warehouse_id, bin_id, 'RECEIPT', quantity_received, 'RECEIVING', @receiving_id, @created_by
        FROM ReceivingLine WHERE receiving_id = @receiving_id;

        -- Update PO status
        UPDATE PurchaseOrder SET status = 'RECEIVING', modified_date = GETDATE()
        WHERE po_id = @po_id AND status = 'CONFIRMED';

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
