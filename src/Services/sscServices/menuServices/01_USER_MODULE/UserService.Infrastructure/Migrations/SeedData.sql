-- ==========================================
-- SEED DATA for USER_MODULE
-- Execution: Run after migrations
-- ==========================================

USE SSCDB;
GO

-- Insert sample users
INSERT INTO [USER_MAST] ([USER_ID], [USER_NAME], [USER_PASSWORD], [USER_EMAILID], [USER_EFFECTIVE_DATE], [USER_CLOSURE_DATE], [USER_ENTEREDBY], [CREATED_DATE], [IS_ACTIVE])
VALUES 
    (1, 'Admin User', '$2a$11$..hashed_password..', 'admin@company.com', GETUTCDATE(), NULL, 1, GETUTCDATE(), 1),
    (2, 'John Doe', '$2a$11$..hashed_password..', 'john.doe@company.com', GETUTCDATE(), NULL, 1, GETUTCDATE(), 1),
    (3, 'Jane Smith', '$2a$11$..hashed_password..', 'jane.smith@company.com', GETUTCDATE(), NULL, 1, GETUTCDATE(), 1);
GO

-- Insert role mappings
INSERT INTO [USER_ROLEMAP] ([ROLE_MAPID], [ROLE_USERID], [ROLE_ID], [ROLE_DEFFLAG], [ROLE_CREATEDON], [ROLE_CREATEDBY])
VALUES 
    (1, 1, 7, 1, GETUTCDATE(), 1),  -- Admin role
    (2, 2, 1, 1, GETUTCDATE(), 1),  -- End User role
    (3, 3, 1, 1, GETUTCDATE(), 1);  -- End User role
GO

-- Insert organization mappings
INSERT INTO [USER_ORGMAP] ([ORG_MAPID], [ORG_USERID], [ORG_BUID], [ORG_CREATEDON], [ORG_CREATEDBY])
VALUES 
    (1, 1, 'ORG001', GETUTCDATE(), 1),
    (2, 2, 'ORG002', GETUTCDATE(), 1),
    (3, 3, 'ORG001', GETUTCDATE(), 1);
GO

-- Insert location mappings
INSERT INTO [USER_LOCATIONMAP] ([LOC_MAPID], [LOC_USERID], [LOC_ID], [LOC_CREATEDON], [LOC_CREATEDBY])
VALUES 
    (1, 1, 1, GETUTCDATE(), 1),
    (2, 2, 2, GETUTCDATE(), 1),
    (3, 3, 1, GETUTCDATE(), 1);
GO

PRINT 'Seed data inserted successfully.';
GO
