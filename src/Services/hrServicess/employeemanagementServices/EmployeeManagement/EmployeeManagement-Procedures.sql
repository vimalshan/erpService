-- EmployeeManagement Module - Procedures
USE [HRDB];
GO

-- ------------------------------------------------------------------
-- Function: fn_GetEmployeeStatus
-- Purpose:  Get current employee status (Active/Inactive/Left)


-- ------------------------------------------------------------------
-- Function: fn_GetServiceTenure
-- Purpose:  Calculate years of service for an employee


-- ------------------------------------------------------------------
-- Procedure: usp_CreateEmployee
-- Purpose:  Create new employee master record


-- ------------------------------------------------------------------
-- Procedure: usp_RecordProbationReview
-- Purpose:  Record probation review and confirmation status


-- ------------------------------------------------------------------
-- Trigger: trg_EmployeeMaster_ValidateGrade
-- Purpose:  Validate grade exists before employee assignment


-- ------------------------------------------------------------------
-- Trigger: trg_EmployeeMaster_Audit
-- Purpose:  Audit all employee master changes


-- ------------------------------------------------------------------
-- Trigger: trg_AAEmpProbation_AutoExtend
-- Purpose:  Auto-extend probation if not reviewed before due date
