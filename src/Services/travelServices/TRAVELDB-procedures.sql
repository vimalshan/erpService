-- ==========================================
-- Database: TRAVELDB
-- Stored Procedures, Functions, Triggers
-- Travel & Expense Management
-- ==========================================

USE [TRAVELDB];
GO

-- ==========================================
-- FUNCTIONS
-- ==========================================

-- ------------------------------------------------------------------
-- Function: fn_CalculateTravelDistance
-- Purpose:  Get distance between two cities
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
-- Purpose:  Calculate travel allowance based on grade and distance
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
-- Purpose:  Create travel request with itinerary
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
        
        -- Generate travel request ID
        SELECT @p_TravelReqID = ISNULL(MAX(TRAVEL_REQID), 0) + 1 FROM TRAVELDB.[Travel Request];
        
        -- Create main travel request (table name is hypothetical)
        INSERT INTO TRAVEL_REQUEST_MAIN
        (
            TRAVEL_REQID, TRAVEL_EMPSYSID, TRAVEL_TYPE, TRAVEL_FROM, TRAVEL_TO,
            TRAVEL_DEPARTDATE, TRAVEL_RETURNDATE, TRAVEL_PURPOSE, TRAVEL_ESTCOST,
            TRAVEL_STATUS, TRAVEL_APPROVEDBY, TRAVEL_SUBMITTEDON
        )
        VALUES
        (
            @p_TravelReqID, @p_EmpSysID, @p_TravelType, @p_FromCity, @p_ToCity,
            @p_DepartDate, @p_ReturnDate, @p_Purpose, @p_EstimatedCost,
            'S', @p_ApprovedBy, GETDATE()  -- S = Submitted
        );
        
        COMMIT TRANSACTION;
        PRINT 'Travel request submitted: ID = ' + CAST(@p_TravelReqID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Travel request submission failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ApproveTravelRequest
-- Purpose:  Approve travel request with GST handling
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ApproveTravelRequest
(
    @p_TravelReqID BIGINT,
    @p_ApprovalAmount DECIMAL(19,0),
    @p_ApprovedBy BIGINT,
    @p_ApprovalRemarks VARCHAR(500) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Calculate GST (18% standard)
        DECLARE @GSTAmount DECIMAL(19,0) = CAST((@p_ApprovalAmount * 0.18) AS BIGINT);
        DECLARE @TotalAmount DECIMAL(19,0) = @p_ApprovalAmount + @GSTAmount;
        
        -- Update travel request
        UPDATE TRAVEL_REQUEST_MAIN
        SET TRAVEL_STATUS = 'A',  -- A = Approved
            TRAVEL_APPROVEDBY = @p_ApprovedBy,
            TRAVEL_APPROVEDON = GETDATE(),
            TRAVEL_APPROVALAMT = @p_ApprovalAmount,
            TRAVEL_GST = @GSTAmount,
            TRAVEL_TOTALAMT = @TotalAmount,
            TRAVEL_REMARKS = @p_ApprovalRemarks
        WHERE TRAVEL_REQID = @p_TravelReqID;
        
        COMMIT TRANSACTION;
        PRINT 'Travel request approved: Amount = ₹' + CAST(@p_ApprovalAmount AS VARCHAR) + 
              ', GST = ₹' + CAST(@GSTAmount AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Travel approval failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- TRIGGERS
-- ==========================================

-- ------------------------------------------------------------------
-- Trigger: trg_TravelRequest_CalculateGST
-- Purpose:  Auto-calculate GST on travel request amount
-- ------------------------------------------------------------------
CREATE OR ALTER TRIGGER dbo.trg_TravelRequest_CalculateGST
ON dbo.TRAVEL_REQUEST_MAIN
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(TRAVEL_APPROVALAMT)
    BEGIN
        UPDATE TRM
        SET TRM.TRAVEL_GST = CAST((I.TRAVEL_APPROVALAMT * 0.18) AS BIGINT),
            TRM.TRAVEL_TOTALAMT = I.TRAVEL_APPROVALAMT + CAST((I.TRAVEL_APPROVALAMT * 0.18) AS BIGINT)
        FROM TRAVEL_REQUEST_MAIN TRM
        INNER JOIN inserted I ON TRM.TRAVEL_REQID = I.TRAVEL_REQID;
    END
END;
GO

-- ==========================================
-- END OF SCRIPT
-- ==========================================
