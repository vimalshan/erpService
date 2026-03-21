-- ==========================================
-- Seed Data Script for TRAVELDB
-- Run AFTER InitialCreate migration
-- ==========================================
USE [TRAVELDB];
GO

-- Sample booking requests
IF NOT EXISTS (SELECT 1 FROM BOOK_REQUEST WHERE BK_BOK_NUM = 1)
BEGIN
    INSERT INTO BOOK_REQUEST (BK_BOK_NUM, BK_SRL_NUM, BK_BOK_TYP, BK_USR_COD, BK_USR_NUM,
        BK_FRO_DAT, BK_RET_DAT, BK_FRO_CIT, BK_TO_CIT,
        BK_FRO_LOC, BK_TO_LOC, BK_PER_NAM, BK_APP_STS, BK_ADM_SLF, BK_PER_STS, BK_APP_DAT)
    VALUES
        (1, 1, 'T', 'EMP001', 1001, '2026-04-01', '2026-04-05', 1, 2, 'Mumbai', 'Delhi', 'Ravi Sharma', 'N', 'N', 'S', GETDATE()),
        (2, 1, 'S', 'EMP002', 1002, '2026-04-10', '2026-04-15', 3, 4, 'Chennai', 'Bangalore', 'Priya Patel', 'N', 'N', 'S', GETDATE()),
        (3, 1, 'L', 'EMP003', 1003, '2026-04-02', '2026-04-02', 1, 1, 'Office', 'Airport', 'Amit Verma', 'N', 'N', 'S', GETDATE());
END;
GO

-- Sample coupon request
IF NOT EXISTS (SELECT 1 FROM COUPON_REQUEST WHERE CPN_REQ_ID = 1)
BEGIN
    INSERT INTO COUPON_REQUEST (CPN_REQ_ID, CPN_REQ_DAT, CPN_REQ_USR, CPN_NOF_CPN, CPN_ARL_NAM, CPN_REQ_RMK, CPN_REQ_STS)
    VALUES (1, GETDATE(), 'EMP001', 10, 'AI', 'Quarterly travel coupons for team', 'P');
END;
GO

PRINT 'Seed data applied successfully';
GO
