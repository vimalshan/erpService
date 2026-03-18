-- Seed data for PROXY_RIGHTS
USE SRFSPARSHDB;
GO

IF NOT EXISTS (SELECT 1 FROM PROXY_RIGHTS WHERE PROXY_USER_ID = 100 AND DELEGATED_USER_ID = 101)
BEGIN
    INSERT INTO PROXY_RIGHTS (PROXY_USER_ID, DELEGATED_USER_ID, PROXY_START_DATE, PROXY_END_DATE, PROXY_TYPE, PROXY_STATUS, SCOPE, NOTES, CREATED_BY, CREATED_ON)
    VALUES
        (100, 101, CAST(GETDATE() AS DATE), CAST(DATEADD(DAY,30,GETDATE()) AS DATE), 'APPROVAL',   'A', 'DEPARTMENT', 'Approval delegation for Q1 reviews',             1, GETDATE()),
        (200, 201, CAST(GETDATE() AS DATE), CAST(DATEADD(DAY,60,GETDATE()) AS DATE), 'SUBMISSION',  'A', 'ALL',        'Submission delegation during leave',              1, GETDATE()),
        (300, 301, CAST(GETDATE() AS DATE), NULL,                                                    'FULL',       'A', 'LOCATION',  'Permanent full proxy for branch office',         1, GETDATE()),
        (400, 401, CAST(GETDATE() AS DATE), CAST(DATEADD(DAY, 7,GETDATE()) AS DATE), 'READONLY',    'A', 'SPECIFIC',  'Temporary read-only access for audit',            1, GETDATE()),
        (500, 501, CAST(GETDATE() AS DATE), CAST(DATEADD(DAY,90,GETDATE()) AS DATE), 'APPROVAL',   'A', 'ALL',        'Long-term approval proxy for annual cycle',       1, GETDATE());
    PRINT 'Seed data inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Seed data already exists. Skipping.';
END
GO
