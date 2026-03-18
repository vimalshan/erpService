-- ==========================================
-- Module: TRAVEL REQUEST
-- Description: Travel planning and request management
-- Procedures, Functions, and Triggers
-- ==========================================

USE [TRAVELDB];
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_CalculateTravelDistance
-- Purpose: Get distance between two cities
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_CalculateTravelDistance
(
    @p_FromCity VARCHAR(50),
    @p_ToCity VARCHAR(50)
)
RETURNS INT
AS
BEGIN
    DECLARE @Distance INT;
    
    -- Simplified distance lookup (in production, use distance matrix table)
    SELECT TOP 1 @Distance = DISTANCE
    FROM CITY_DISTANCE_MATRIX
    WHERE (FROM_CITY = @p_FromCity AND TO_CITY = @p_ToCity)
       OR (FROM_CITY = @p_ToCity AND TO_CITY = @p_FromCity);
    
    RETURN ISNULL(@Distance, 0);
END;
GO

-- ------------------------------------------------------------------
-- Function: fn_CalculateTravelAllowance
-- Purpose: Calculate travel allowance based on grade and distance
-- ------------------------------------------------------------------
CREATE OR ALTER FUNCTION dbo.fn_CalculateTravelAllowance
(
    @p_EmpSysID BIGINT,
    @p_TravelMode VARCHAR(10),  -- AIR, TRAIN, CAB, BUS
    @p_Distance INT
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    DECLARE @Grade CHAR(3);
    DECLARE @AllowancePerKM DECIMAL(5,2);
    DECLARE @TotalAllowance DECIMAL(19,0);
    
    -- Get employee grade
    SELECT TOP 1 @Grade = EMP_GRADE
    FROM HRDB.dbo.EMPLOYEE_MASTER
    WHERE EMP_SYSID = @p_EmpSysID;
    
    -- Determine allowance per KM based on mode and grade
    SET @AllowancePerKM = CASE
        WHEN @p_TravelMode = 'AIR' THEN 0  -- Full reimbursement
        WHEN @p_TravelMode = 'TRAIN' AND @Grade = 'UG' THEN 2.5
        WHEN @p_TravelMode = 'TRAIN' AND @Grade = 'PG' THEN 3.0
        WHEN @p_TravelMode = 'CAB' THEN 1.5
        WHEN @p_TravelMode = 'BUS' THEN 1.0
        ELSE 0
    END;
    
    SET @TotalAllowance = CAST((@p_Distance * @AllowancePerKM) AS DECIMAL(19,0));
    
    RETURN ISNULL(@TotalAllowance, 0);
END;
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_SubmitTravelRequest
-- Purpose: Create travel request with itinerary
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_SubmitTravelRequest
(
    @p_EmpSysID BIGINT,
    @p_TravelType VARCHAR(10),  -- AIR, TRAIN, CAB, BUS, MULTI
    @p_FromCity VARCHAR(50),
    @p_ToCity VARCHAR(50),
    @p_DepartDate DATETIME2(3),
    @p_ReturnDate DATETIME2(3),
    @p_Purpose VARCHAR(500),
    @p_EstimatedCost DECIMAL(19,0),
    @p_ApprovedBy BIGINT = NULL,
    @p_TravelReqID BIGINT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate dates
        IF @p_DepartDate < GETDATE()
            THROW 50001, 'Departure date cannot be in past', 1;
        
        IF @p_ReturnDate < @p_DepartDate
            THROW 50002, 'Return date must be after departure date', 1;
        
        -- Generate travel request ID (example - adjust based on your logic)
        SELECT @p_TravelReqID = ISNULL(MAX(TR_REQ_ID), 0) + 1 FROM TRAVEL_MAIN;
        
        -- Insert travel request record
        INSERT INTO TRAVEL_MAIN
        (
            TR_COM_COD, TR_PLN_NUM, TR_USR_NUM, TR_APP_DAT, TR_NAT_COD,
            TR_OBJ_DES, TR_BUD_FLG, TR_PLS_FLG, TR_TVL_TYP, TR_BUD_AMT
        )
        VALUES
        (
            '001', @p_TravelReqID, @p_EmpSysID, GETDATE(), 1,
            @p_Purpose, 'Y', 'N', 
            CASE WHEN @p_TravelType IN ('AIR', 'MULTI') THEN 'INT' ELSE 'DOM' END,
            @p_EstimatedCost
        );
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result], 
               'Travel request submitted successfully' AS [Message],
               @p_TravelReqID AS [TravelRequestID];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message],
               NULL AS [TravelRequestID];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ApproveTravelRequest
-- Purpose: Approve travel request with validation
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ApproveTravelRequest
(
    @p_TravelReqID BIGINT,
    @p_ApprovalAmount DECIMAL(19,0),
    @p_ApprovedBy BIGINT,
    @p_ApprovalRemarks VARCHAR(500) = NULL,
    @p_Status CHAR(1) = 'A'  -- A = Approved, R = Rejected
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Update travel request with approval
        UPDATE TRAVEL_MAIN
        SET TR_PLS_FLG = @p_Status,
            TR_BUD_AMT = @p_ApprovalAmount,
            TR_MOD_DAT = GETDATE(),
            TR_MOD_USR = CAST(@p_ApprovedBy AS VARCHAR(20))
        WHERE TR_PLN_NUM = @p_TravelReqID;
        
        -- Insert approval remarks
        INSERT INTO TRAVEL_APPRREMARKS
        (TR_REQNO, TR_REQTYP, TR_REM, TR_APPBY, TR_APP_ON)
        VALUES
        (@p_TravelReqID, 'APPROVAL', @p_ApprovalRemarks, CAST(@p_ApprovedBy AS VARCHAR(60)), GETDATE());
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS [Result],
               'Travel request processed successfully' AS [Message];
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        SELECT 'ERROR' AS [Result],
               ERROR_MESSAGE() AS [Message];
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_GetTravelRequestDetails
-- Purpose: Retrieve detailed travel request information
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_GetTravelRequestDetails
(
    @p_TravelReqID BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        tm.TR_PLN_NUM AS [TravelPlanNo],
        tm.TR_USR_NUM AS [EmployeeID],
        tm.TR_APP_DAT AS [ApplicationDate],
        tm.TR_NAT_COD AS [Purpose],
        tm.TR_OBJ_DES AS [Objective],
        tm.TR_BUD_AMT AS [BudgetAmount],
        tm.TR_PLS_FLG AS [Status],
        tm.TR_TVL_TYP AS [TravelType],
        ISNULL(tar.TR_REM, 'No remarks') AS [Remarks]
    FROM TRAVEL_MAIN tm
    LEFT JOIN TRAVEL_APPRREMARKS tar ON tm.TR_PLN_NUM = tar.TR_REQNO
    WHERE tm.TR_PLN_NUM = @p_TravelReqID;
END;
GO

-- ==========================================
-- TRIGGERS
-- ==========================================

-- ------------------------------------------------------------------
-- Trigger: trg_TravelRequest_UpdateModifiedDate
-- Purpose: Auto-update modified date on record changes
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_TravelRequest_UpdateModifiedDate
ON dbo.TRAVEL_MAIN
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TRM
    SET TRM.TR_MOD_DAT = GETDATE()
    FROM TRAVEL_MAIN TRM
    INNER JOIN inserted I ON TRM.TR_PLN_NUM = I.TR_PLN_NUM;
END;
GO

-- ------------------------------------------------------------------
-- Trigger: trg_TravelAdvance_ValidateAmount
-- Purpose: Validate advance amount against budget
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_TravelAdvance_ValidateAmount
ON dbo.TRAVEL_ADVANCE
BEFORE INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @BudgetAmount DECIMAL(19,0);
    DECLARE @AdvanceAmount DECIMAL(19,0);
    
    SELECT @BudgetAmount = tm.TR_BUD_AMT
    FROM TRAVEL_MAIN tm
    INNER JOIN inserted i ON tm.TR_REQ_ID = i.AD_REQ_NUM;
    
    SELECT @AdvanceAmount = SUM(AD_ADV_AMT)
    FROM inserted;
    
    IF @AdvanceAmount > @BudgetAmount
    BEGIN
        RAISERROR('Advance amount exceeds budget allocation', 16, 1);
        ROLLBACK;
    END;
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
