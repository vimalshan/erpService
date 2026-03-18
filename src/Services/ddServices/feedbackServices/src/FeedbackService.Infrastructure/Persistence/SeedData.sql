-- Feedback Service Database Seed Script
-- This script seeds initial LOV and sample feedback data

USE DDDB;
GO

-- Seed LOV_FEEDBACK table with feedback types
IF NOT EXISTS (SELECT 1 FROM LOV_FEEDBACK)
BEGIN
    INSERT INTO LOV_FEEDBACK (DD_FEEDBACKID, DD_FEEDBACKNAME)
    VALUES 
        (1, 'Service Quality'),
        (2, 'Product Quality'),
        (3, 'Delivery Performance'),
        (4, 'Customer Support'),
        (5, 'Overall Experience');
    
    PRINT 'LOV_FEEDBACK seeded with 5 records';
END
GO

-- Seed sample feedback data
IF NOT EXISTS (SELECT 1 FROM APP_FEEDBACKMAIN WHERE FB_FEEDBACKID = 1000)
BEGIN
    -- Sample Feedback Records
    INSERT INTO APP_FEEDBACKMAIN (FB_FEEDBACKID, FB_REQUESTNO, FB_APPRSYSID, FB_STATUS, FB_REMARKS, CREATEDON, UPDATEDON)
    VALUES
        (1000, 100, 5, 'A', 'Excellent service provided', GETUTCDATE(), NULL),
        (1001, 101, 6, 'A', 'Good quality product', GETUTCDATE(), NULL),
        (1002, 102, 7, 'I', 'Feedback marked as inactive', GETUTCDATE(), NULL);
    
    PRINT 'APP_FEEDBACKMAIN seeded with 3 records';
    
    -- Sample Feedback Details
    INSERT INTO APP_FEEDBACKSUB (FB_FEEDBACKID, FB_QTNNO, FB_ANSNO, UPDATEDON)
    VALUES
        (1000, 1, 101, NULL),
        (1000, 2, 102, NULL),
        (1001, 1, 103, NULL),
        (1001, 2, NULL, NULL),
        (1002, 1, 104, NULL);
    
    PRINT 'APP_FEEDBACKSUB seeded with 5 records';
END
GO

PRINT 'Database seed script executed successfully';
GO
