-- ==========================================
-- Module: TRANSACTION
-- Description: Financial Transaction Module
-- Database: TOURDB
-- Tables: JVEMP_MAIN, JVEMP_SUB, JVSUP_MAIN, JVSUP_SUB,
--         TRAVEL_BATCHMAIN, TRAVEL_BATCHSUB, TRAVEL_BATCHCC,
--         TRAVEL_BATCHCONTRACT, TRAVEL_BATCHSUBBRK,
--         JVEMPPAY_DET, TRAVEL_EMPPAYDET, TICKET_AIRLINEINOVICE,
--         TEMPJVEMP_MAIN, TEMPJVEMP_SUB, TEMPJVSUP_MAIN, TEMPJVSUP_SUB
-- ==========================================

USE TOURDB;
GO

-- NOTE: These tables already exist in TOURDB.sql.
-- This file documents the transaction-related table definitions
-- used by the TransactionService microservice.

-- Table: JVEMP_MAIN - Employee Journal Voucher Main
-- CREATE TABLE [JVEMP_MAIN] (
--     [JV_BATCHID] BIGINT NOT NULL,
--     [JV_TPID] BIGINT NOT NULL,
--     [JV_TYPE] CHAR(3) NOT NULL,
--     [JV_DATE] DATETIME2(3) NOT NULL,
--     [JV_EMPSYSID] BIGINT NOT NULL,
--     [JV_STATUS] CHAR(1) NOT NULL,
--     [JV_CREATEDBY] BIGINT NOT NULL,
--     [JV_CREATEDON] DATETIME2(3) NOT NULL,
--     [JV_TRNTYPE] CHAR(3) NOT NULL,
--     [JV_ORAREFNO] VARCHAR(50) NULL,
--     [JV_NETAMT] DECIMAL(19,0) NOT NULL,
--     [JV_PAYUNITID] BIGINT NOT NULL,
--     [JV_TRNREFNO] BIGINT NULL,
--     CONSTRAINT [PK_JVEMP_MAIN] PRIMARY KEY ([JV_BATCHID])
-- );

-- Table: JVEMP_SUB - Employee JV Line Items (Debit/Credit)
-- CREATE TABLE [JVEMP_SUB] (...);

-- Table: JVSUP_MAIN - Supplier Journal Voucher Main (INV/CRD/JV)
-- CREATE TABLE [JVSUP_MAIN] (...);

-- Table: JVSUP_SUB - Supplier JV Line Items (Debit/Credit)
-- CREATE TABLE [JVSUP_SUB] (...);

-- Table: TRAVEL_BATCHMAIN - Travel Batch Main
-- CREATE TABLE [TRAVEL_BATCHMAIN] (...);

-- Table: TRAVEL_BATCHSUB - Travel Batch Sub Items
-- CREATE TABLE [TRAVEL_BATCHSUB] (...);

-- Table: TRAVEL_BATCHCC - Batch Cost Centre Details
-- CREATE TABLE [TRAVEL_BATCHCC] (...);

-- Table: TRAVEL_BATCHCONTRACT - Batch Contract Details
-- CREATE TABLE [TRAVEL_BATCHCONTRACT] (...);

-- Table: TRAVEL_BATCHSUBBRK - Batch Sub Breakup
-- CREATE TABLE [TRAVEL_BATCHSUBBRK] (...);

-- Table: JVEMPPAY_DET - Employee Payment Detail (ADV/EXP/ADJ)
-- CREATE TABLE [JVEMPPAY_DET] (...);

-- Table: TRAVEL_EMPPAYDET - Employee Travel Payment Detail
-- CREATE TABLE [TRAVEL_EMPPAYDET] (...);

-- Table: TICKET_AIRLINEINOVICE - Airline Invoice
-- CREATE TABLE [TICKET_AIRLINEINOVICE] (...);

-- Table: TEMPJVEMP_MAIN - Temporary Employee JV Main (staging)
-- CREATE TABLE [TEMPJVEMP_MAIN] (...);

-- Table: TEMPJVEMP_SUB - Temporary Employee JV Sub (staging)
-- CREATE TABLE [TEMPJVEMP_SUB] (...);

-- Table: TEMPJVSUP_MAIN - Temporary Supplier JV Main (staging)
-- CREATE TABLE [TEMPJVSUP_MAIN] (...);

-- Table: TEMPJVSUP_SUB - Temporary Supplier JV Sub (staging)
-- CREATE TABLE [TEMPJVSUP_SUB] (...);

PRINT 'TRANSACTION Module - Tables already exist in TOURDB.sql';
GO
