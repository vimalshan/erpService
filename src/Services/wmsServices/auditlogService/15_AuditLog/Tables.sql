-- ======================================================
-- Module: Audit Log
-- Tables: AuditLog
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE AuditLog (
    log_id          BIGINT IDENTITY(1,1) PRIMARY KEY,
    table_name      NVARCHAR(100) NOT NULL,
    record_id       INT NOT NULL,
    action          NVARCHAR(10) NOT NULL CHECK (action IN ('INSERT', 'UPDATE', 'DELETE')),
    changed_by      NVARCHAR(50),
    change_date     DATETIME2 NOT NULL DEFAULT GETDATE(),
    old_values      NVARCHAR(MAX),
    new_values      NVARCHAR(MAX)
);
GO
