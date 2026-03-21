-- ======================================================
-- Module: Customer
-- Tables: Customer
-- ======================================================
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE Customer (
    customer_id     INT IDENTITY(1,1) PRIMARY KEY,
    code            NVARCHAR(30) NOT NULL UNIQUE,
    name            NVARCHAR(200) NOT NULL,
    company_name    NVARCHAR(100),
    contact_person  NVARCHAR(100),
    contact_title   NVARCHAR(50),
    email           NVARCHAR(100),
    phone           NVARCHAR(30),
    address         NVARCHAR(200),
    city            NVARCHAR(50),
    state           NVARCHAR(50),
    country         NVARCHAR(50),
    postal_code     NVARCHAR(20),
    is_active       BIT NOT NULL DEFAULT 1,
    created_date    DATETIME2 NOT NULL DEFAULT GETDATE(),
    modified_date   DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO
