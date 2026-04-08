-- PFDB Stored Procedures, Functions & Triggers
-- Provident Fund Management System
-- Created: February 13, 2026

USE PFDB;
GO

-- =====================================================
-- FUNCTIONS
-- =====================================================

-- Function: Calculate PF Contribution (Employee + Employer)
IF OBJECT_ID('dbo.fn_CalculatePFContribution', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_CalculatePFContribution;
GO

CREATE FUNCTION dbo.fn_CalculatePFContribution (
    @p_BasicSalary DECIMAL(19,0),
    @p_EmployeeContributionRate DECIMAL(5,2) = 12.0,
    @p_EmployerContributionRate DECIMAL(5,2) = 12.0
)
RETURNS TABLE
AS
RETURN (
    SELECT 
        CAST(@p_BasicSalary * (@p_EmployeeContributionRate / 100) AS DECIMAL(19,0)) AS EmpContribution,
        CAST(@p_BasicSalary * (@p_EmployerContributionRate / 100) AS DECIMAL(19,0)) AS ErContribution,
        CAST(@p_BasicSalary * ((@p_EmployeeContributionRate + @p_EmployerContributionRate) / 100) AS DECIMAL(19,0)) AS TotalContribution
);
GO

-- Function: Calculate PF Settlement Amount
IF OBJECT_ID('dbo.fn_CalculatePFSettlement', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_CalculatePFSettlement;
GO

CREATE FUNCTION dbo.fn_CalculatePFSettlement (
    @p_EmpSysID BIGINT,
    @p_SettlementDate DATETIME2(3),
    @p_InterestRate DECIMAL(5,2) = 8.5
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    DECLARE @SettlementAmount DECIMAL(19,0) = 0;
    
    BEGIN TRY
        SELECT @SettlementAmount = CAST(
            ISNULL(PF_ACC_BAL, 0) + 
            (ISNULL(PF_ACC_BAL, 0) * (@p_InterestRate / 100) * 
             DATEDIFF(DAY, DATEADD(YEAR, -1, @p_SettlementDate), @p_SettlementDate) / 365.0)
            AS DECIMAL(19,0)
        )
        FROM dbo.PF_ACCUMULATION
        WHERE EMP_SYS_ID = @p_EmpSysID
          AND PF_ACC_STATUS = 'A';
    END TRY
    BEGIN CATCH
        SET @SettlementAmount = 0;
    END CATCH
    
    RETURN @SettlementAmount;
END;
GO

-- Function: Get PF Eligibility Status
IF OBJECT_ID('dbo.fn_GetPFEligibilityStatus', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_GetPFEligibilityStatus;
GO

CREATE FUNCTION dbo.fn_GetPFEligibilityStatus (
    @p_EmpSysID BIGINT,
    @p_CheckDate DATETIME2(3)
)
RETURNS VARCHAR(50)
AS
BEGIN
    DECLARE @Status VARCHAR(50) = 'INELIGIBLE';
    DECLARE @ServiceMonths INT;
    DECLARE @EmpDOJ DATE;
    
    BEGIN TRY
        -- Get employee DOJ from HRDB
        SELECT @EmpDOJ = EMP_DOJ 
        FROM HRDB.dbo.EMPLOYEE_MASTER 
        WHERE EMP_SYS_ID = @p_EmpSysID;
        
        IF @EmpDOJ IS NOT NULL
        BEGIN
            SET @ServiceMonths = DATEDIFF(MONTH, @EmpDOJ, @p_CheckDate);
            
            -- PF Eligible after 0 days (applicable immediately in India)
            IF @ServiceMonths >= 0
                SET @Status = 'ELIGIBLE';
            
            -- Check withdrawal eligibility (after 5 years of service or unemployment)
            IF @ServiceMonths >= 60
                SET @Status = 'ELIGIBLE_WITHDRAWAL';
        END
    END TRY
    BEGIN CATCH
        SET @Status = 'ERROR';
    END CATCH
    
    RETURN @Status;
END;
GO

-- =====================================================
-- STORED PROCEDURES
-- =====================================================

-- Procedure: Process Monthly PF Contribution
IF OBJECT_ID('dbo.usp_ProcessMonthlyPFContribution', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_ProcessMonthlyPFContribution;
GO

CREATE PROCEDURE dbo.usp_ProcessMonthlyPFContribution
    @p_MonthYear VARCHAR(7),  -- Format: YYYY-MM
    @p_ProcessedBy BIGINT,
    @p_RowsProcessed INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @EmpSysID BIGINT;
        DECLARE @BasicSalary DECIMAL(19,0);
        DECLARE @EmpContribution DECIMAL(19,0);
        DECLARE @ErContribution DECIMAL(19,0);
        DECLARE @TotalContribution DECIMAL(19,0);
        DECLARE @PFAccID BIGINT;
        
        -- Cursor to process all active employees
        DECLARE PF_Cursor CURSOR FOR
        SELECT DISTINCT 
            ps.EMP_SYS_ID,
            ps.PAY_BASIC
        FROM PAYDB.dbo.PAY_SALARY_MASTER ps
        INNER JOIN HRDB.dbo.EMPLOYEE_MASTER em ON ps.EMP_SYS_ID = em.EMP_SYS_ID
        WHERE em.EMP_STATUS = 'A'  -- Active employees
          AND MONTH(ps.PAY_PERIOD_ENDING) = MONTH(CAST(@p_MonthYear + '-01' AS DATE))
          AND YEAR(ps.PAY_PERIOD_ENDING) = YEAR(CAST(@p_MonthYear + '-01' AS DATE));
        
        OPEN PF_Cursor;
        FETCH NEXT FROM PF_Cursor INTO @EmpSysID, @BasicSalary;
        
        SET @p_RowsProcessed = 0;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Check if accumulation exists
            SELECT @PFAccID = PF_ACC_ID 
            FROM dbo.PF_ACCUMULATION 
            WHERE EMP_SYS_ID = @EmpSysID;
            
            -- Calculate contributions
            SELECT 
                @EmpContribution = EmpContribution,
                @ErContribution = ErContribution,
                @TotalContribution = TotalContribution
            FROM dbo.fn_CalculatePFContribution(@BasicSalary, 12.0, 12.0);
            
            IF @PFAccID IS NULL
            BEGIN
                -- Create new accumulation record
                INSERT INTO dbo.PF_ACCUMULATION (
                    EMP_SYS_ID,
                    PF_ACC_BAL,
                    PF_EMP_CONT_TOTAL,
                    PF_ER_CONT_TOTAL,
                    PF_ACC_STATUS,
                    CREATED_BY,
                    CREATED_ON
                ) VALUES (
                    @EmpSysID,
                    @TotalContribution,
                    @EmpContribution,
                    @ErContribution,
                    'A',
                    @p_ProcessedBy,
                    @ProcessDate
                );
            END
            ELSE
            BEGIN
                -- Update accumulation record
                UPDATE dbo.PF_ACCUMULATION
                SET 
                    PF_ACC_BAL = PF_ACC_BAL + @TotalContribution,
                    PF_EMP_CONT_TOTAL = PF_EMP_CONT_TOTAL + @EmpContribution,
                    PF_ER_CONT_TOTAL = PF_ER_CONT_TOTAL + @ErContribution,
                    UPDATED_BY = @p_ProcessedBy,
                    UPDATED_ON = @ProcessDate
                WHERE PF_ACC_ID = @PFAccID;
            END
            
            -- Record PF contribution transaction
            INSERT INTO dbo.PF_CONTRIBUTION_TXN (
                EMP_SYS_ID,
                PF_EMP_CONTRIBUTION,
                PF_ER_CONTRIBUTION,
                PF_TXN_DATE,
                PF_TXN_MONTH,
                PF_TXN_STATUS,
                CREATED_BY,
                CREATED_ON
            ) VALUES (
                @EmpSysID,
                @EmpContribution,
                @ErContribution,
                @ProcessDate,
                CAST(@p_MonthYear + '-01' AS DATE),
                'P',  -- Posted
                @p_ProcessedBy,
                @ProcessDate
            );
            
            SET @p_RowsProcessed = @p_RowsProcessed + 1;
            FETCH NEXT FROM PF_Cursor INTO @EmpSysID, @BasicSalary;
        END
        
        CLOSE PF_Cursor;
        DEALLOCATE PF_Cursor;
        
        COMMIT TRANSACTION;
        
        PRINT 'PF contribution processing completed. Rows processed: ' + CAST(@p_RowsProcessed AS VARCHAR);
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW 50001, 'Error in PF contribution processing', 1;
    END CATCH
END;
GO

-- Procedure: Process PF Settlement/Withdrawal
IF OBJECT_ID('dbo.usp_ProcessPFSettlement', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_ProcessPFSettlement;
GO

CREATE PROCEDURE dbo.usp_ProcessPFSettlement
    @p_EmpSysID BIGINT,
    @p_SettlementType VARCHAR(10),  -- 'WITHDRAWAL', 'RETIREMENT', 'RELIEF'
    @p_SettlementAmount DECIMAL(19,0),
    @p_ApprovedBy BIGINT,
    @p_SettlementID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @AccumulatedBalance DECIMAL(19,0);
        DECLARE @AccID BIGINT;
        
        -- Validate employee eligibility
        IF dbo.fn_GetPFEligibilityStatus(@p_EmpSysID, @ProcessDate) NOT IN ('ELIGIBLE_WITHDRAWAL')
        BEGIN
            THROW 50002, 'Employee not eligible for PF settlement', 1;
        END
        
        -- Get accumulation balance
        SELECT 
            @AccID = PF_ACC_ID,
            @AccumulatedBalance = PF_ACC_BAL
        FROM dbo.PF_ACCUMULATION
        WHERE EMP_SYS_ID = @p_EmpSysID
          AND PF_ACC_STATUS = 'A';
        
        IF @AccID IS NULL
        BEGIN
            THROW 50003, 'No active PF accumulation found for employee', 1;
        END
        
        IF @p_SettlementAmount > @AccumulatedBalance
        BEGIN
            THROW 50004, 'Settlement amount exceeds accumulated PF balance', 1;
        END
        
        -- Create settlement record
        INSERT INTO dbo.PF_SETTLEMENT (
            EMP_SYS_ID,
            PF_SETTLEMENT_AMOUNT,
            PF_SETTLEMENT_TYPE,
            PF_SETTLEMENT_DATE,
            PF_SETTLEMENT_STATUS,
            APPROVED_BY,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_EmpSysID,
            @p_SettlementAmount,
            @p_SettlementType,
            @ProcessDate,
            'A',  -- Approved
            @p_ApprovedBy,
            @p_ApprovedBy,
            @ProcessDate
        );
        
        SET @p_SettlementID = SCOPE_IDENTITY();
        
        -- Update accumulation balance
        UPDATE dbo.PF_ACCUMULATION
        SET 
            PF_ACC_BAL = PF_ACC_BAL - @p_SettlementAmount,
            UPDATED_BY = @p_ApprovedBy,
            UPDATED_ON = @ProcessDate
        WHERE PF_ACC_ID = @AccID;
        
        -- Create settlement transaction
        INSERT INTO dbo.PF_SETTLEMENT_TXN (
            PF_SETTLEMENT_ID,
            EMP_SYS_ID,
            PF_SETTLEMENT_TXN_AMOUNT,
            PF_SETTLEMENT_TXN_DATE,
            PF_SETTLEMENT_TXN_STATUS,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_SettlementID,
            @p_EmpSysID,
            @p_SettlementAmount,
            @ProcessDate,
            'P',  -- Posted
            @p_ApprovedBy,
            @ProcessDate
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Generate PF Withdrawal Certificate
IF OBJECT_ID('dbo.usp_GeneratePFWithdrawalCertificate', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_GeneratePFWithdrawalCertificate;
GO

CREATE PROCEDURE dbo.usp_GeneratePFWithdrawalCertificate
    @p_SettlementID BIGINT,
    @p_GeneratedBy BIGINT,
    @p_CertificateID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @EmpSysID BIGINT;
        DECLARE @SettlementAmount DECIMAL(19,0);
        DECLARE @EmpName VARCHAR(100);
        DECLARE @EmpDOJ DATE;
        DECLARE @SettlementDate DATE;
        
        -- Get settlement details
        SELECT 
            @EmpSysID = EMP_SYS_ID,
            @SettlementAmount = PF_SETTLEMENT_AMOUNT,
            @SettlementDate = CAST(PF_SETTLEMENT_DATE AS DATE)
        FROM dbo.PF_SETTLEMENT
        WHERE PF_SETTLEMENT_ID = @p_SettlementID;
        
        IF @EmpSysID IS NULL
        BEGIN
            THROW 50005, 'Settlement record not found', 1;
        END
        
        -- Get employee details
        SELECT 
            @EmpName = EMP_NAME,
            @EmpDOJ = EMP_DOJ
        FROM HRDB.dbo.EMPLOYEE_MASTER
        WHERE EMP_SYS_ID = @EmpSysID;
        
        -- Generate certificate
        INSERT INTO dbo.PF_WITHDRAWAL_CERTIFICATE (
            PF_SETTLEMENT_ID,
            EMP_SYS_ID,
            CERTIFICATE_AMOUNT,
            CERTIFICATE_DATE,
            CERTIFICATE_STATUS,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_SettlementID,
            @EmpSysID,
            @SettlementAmount,
            @ProcessDate,
            'G',  -- Generated
            @p_GeneratedBy,
            @ProcessDate
        );
        
        SET @p_CertificateID = SCOPE_IDENTITY();
        
        -- Update settlement status
        UPDATE dbo.PF_SETTLEMENT
        SET PF_SETTLEMENT_STATUS = 'C'  -- Certified
        WHERE PF_SETTLEMENT_ID = @p_SettlementID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =====================================================
-- TRIGGERS
-- =====================================================

-- Trigger: Validate PF Contribution
IF OBJECT_ID('dbo.trg_PFContributionTxn_Validate', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_PFContributionTxn_Validate;
GO

CREATE TRIGGER dbo.trg_PFContributionTxn_Validate
ON dbo.PF_CONTRIBUTION_TXN
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validate contribution amounts
        IF EXISTS (SELECT 1 FROM inserted WHERE PF_EMP_CONTRIBUTION < 0 OR PF_ER_CONTRIBUTION < 0)
        BEGIN
            THROW 50006, 'PF contributions cannot be negative', 1;
        END
        
        -- Validate transaction month
        IF EXISTS (SELECT 1 FROM inserted WHERE PF_TXN_MONTH IS NULL)
        BEGIN
            THROW 50007, 'PF transaction month is required', 1;
        END
        
        INSERT INTO dbo.PF_CONTRIBUTION_TXN (
            EMP_SYS_ID,
            PF_EMP_CONTRIBUTION,
            PF_ER_CONTRIBUTION,
            PF_TXN_DATE,
            PF_TXN_MONTH,
            PF_TXN_STATUS,
            CREATED_BY,
            CREATED_ON
        )
        SELECT 
            EMP_SYS_ID,
            PF_EMP_CONTRIBUTION,
            PF_ER_CONTRIBUTION,
            PF_TXN_DATE,
            PF_TXN_MONTH,
            PF_TXN_STATUS,
            CREATED_BY,
            CREATED_ON
        FROM inserted;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Trigger: Audit PF Settlement
IF OBJECT_ID('dbo.trg_PFSettlement_Audit', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_PFSettlement_Audit;
GO

CREATE TRIGGER dbo.trg_PFSettlement_Audit
ON dbo.PF_SETTLEMENT
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO dbo.PF_SETTLEMENT_AUDIT (
        PF_SETTLEMENT_ID,
        EMP_SYS_ID,
        PF_SETTLEMENT_AMOUNT,
        PF_SETTLEMENT_TYPE,
        PF_SETTLEMENT_STATUS,
        AUDIT_ACTION,
        AUDIT_DATE
    )
    SELECT 
        PF_SETTLEMENT_ID,
        EMP_SYS_ID,
        PF_SETTLEMENT_AMOUNT,
        PF_SETTLEMENT_TYPE,
        PF_SETTLEMENT_STATUS,
        CASE WHEN EXISTS (SELECT 1 FROM deleted) THEN 'UPDATE' ELSE 'INSERT' END,
        GETDATE()
    FROM inserted;
END;
GO

-- Trigger: Auto-Create Accumulation if missing
IF OBJECT_ID('dbo.trg_PFContributionTxn_CreateAccum', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_PFContributionTxn_CreateAccum;
GO

CREATE TRIGGER dbo.trg_PFContributionTxn_CreateAccum
ON dbo.PF_CONTRIBUTION_TXN
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO dbo.PF_ACCUMULATION (
        EMP_SYS_ID,
        PF_ACC_BAL,
        PF_EMP_CONT_TOTAL,
        PF_ER_CONT_TOTAL,
        PF_ACC_STATUS,
        CREATED_BY,
        CREATED_ON
    )
    SELECT DISTINCT 
        i.EMP_SYS_ID,
        SUM(i.PF_EMP_CONTRIBUTION + i.PF_ER_CONTRIBUTION),
        SUM(i.PF_EMP_CONTRIBUTION),
        SUM(i.PF_ER_CONTRIBUTION),
        'A',
        i.CREATED_BY,
        GETDATE()
    FROM inserted i
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.PF_ACCUMULATION 
        WHERE EMP_SYS_ID = i.EMP_SYS_ID
    )
    GROUP BY i.EMP_SYS_ID, i.CREATED_BY;
END;
GO

PRINT 'PFDB Procedures, Functions & Triggers created successfully.';
GO
