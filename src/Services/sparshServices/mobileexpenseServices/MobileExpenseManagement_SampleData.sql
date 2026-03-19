-- ============================================================================
-- Sample Data for Mobile Expense Management Testing
-- Purpose: Populate test data for development and testing
-- ============================================================================

USE [SPARSHDB];
GO

-- ============================================================================
-- INSERT Sample Expenses
-- ============================================================================

PRINT 'Inserting sample expenses...';

-- Insert sample expense records (only if table is not too large)
IF (SELECT COUNT(*) FROM dbo.MOBEXP_DET) < 100
BEGIN
    -- Expense for Trip 1
    INSERT INTO dbo.MOBEXP_DET 
        (MOBEXP_ID, MOBEXP_TPID, MOBEXP_CATID, MOBEXP_DATE, MOBEXP_COMMENT, 
         MOBEXP_AMOUNT, MOBEXP_CURRID, MOBEXP_ENTEREDBY, MOBEXP_ENTEREDON)
    VALUES
        (NEXT VALUE FOR dbo.seq_MOBEXP_Id, 1001, 1, '2024-03-01', 'Airfare to Mumbai', 5500.00, 1, 101, GETDATE()),
        (NEXT VALUE FOR dbo.seq_MOBEXP_Id, 1001, 3, '2024-03-02', 'Hotel stay 2 nights', 8000.00, 1, 101, GETDATE()),
        (NEXT VALUE FOR dbo.seq_MOBEXP_Id, 1001, 2, '2024-03-02', 'Dinner with client', 1500.00, 1, 101, GETDATE()),
        (NEXT VALUE FOR dbo.seq_MOBEXP_Id, 1001, 4, '2024-03-03', 'Taxi services', 850.00, 1, 101, GETDATE()),
        (NEXT VALUE FOR dbo.seq_MOBEXP_Id, 1002, 1, '2024-03-05', 'Train ticket', 2500.00, 1, 102, GETDATE()),
        (NEXT VALUE FOR dbo.seq_MOBEXP_Id, 1002, 3, '2024-03-06', 'Hotel accommodation', 6000.00, 1, 102, GETDATE());
    
    PRINT 'Sample expenses inserted successfully';
END
ELSE
    PRINT 'Skipping sample expenses - table already has data';

GO

-- ============================================================================
-- INSERT Sample Expense Files
-- ============================================================================

PRINT 'Inserting sample expense files...';

-- Insert sample file attachments
IF (SELECT COUNT(*) FROM dbo.MOBEXP_FILE) < 50
BEGIN
    INSERT INTO dbo.MOBEXP_FILE 
        (MOBEXPPHT_ID, MOBEXPPHT_EXPID, MOBEXPPHT_FILENAME, MOBEXPPHT_FILEDATA)
    VALUES
        (NEXT VALUE FOR dbo.seq_MOBEXP_File_Id, 1000, 'airfare_receipt.pdf', 'base64_encoded_pdf_data_here'),
        (NEXT VALUE FOR dbo.seq_MOBEXP_File_Id, 1001, 'hotel_invoice.pdf', 'base64_encoded_pdf_data_here'),
        (NEXT VALUE FOR dbo.seq_MOBEXP_File_Id, 1002, 'restaurant_bill.jpg', 'base64_encoded_image_data_here'),
        (NEXT VALUE FOR dbo.seq_MOBEXP_File_Id, 1002, 'receipt_back.jpg', 'base64_encoded_image_data_here');
    
    PRINT 'Sample expense files inserted successfully';
END
ELSE
    PRINT 'Skipping sample files - table already has data';

GO

-- ============================================================================
-- SUMMARY
-- ============================================================================

PRINT '';
PRINT '=== Data Summary ===';
PRINT 'Total Expenses: ' + CAST((SELECT COUNT(*) FROM dbo.MOBEXP_DET) AS VARCHAR(10));
PRINT 'Total Files: ' + CAST((SELECT COUNT(*) FROM dbo.MOBEXP_FILE) AS VARCHAR(10));
PRINT '';
PRINT 'Sample data initialization completed!';
GO
