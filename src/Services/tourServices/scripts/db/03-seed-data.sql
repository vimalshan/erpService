-- ==========================================
-- TOURDB Seed Data
-- Initial reference data for Tour ERP
-- ==========================================

USE TOURDB;
GO

-- Currency Master
IF NOT EXISTS (SELECT 1 FROM CURRENCY_MASTER WHERE CUR_CODE = 'INR')
INSERT INTO CURRENCY_MASTER (CUR_CODE, CUR_NAME, CUR_SYMBOL, CUR_STATUS) VALUES
('INR', 'Indian Rupee', N'₹', 'A'),
('USD', 'US Dollar', N'$', 'A'),
('EUR', 'Euro', N'€', 'A'),
('GBP', 'British Pound', N'£', 'A'),
('AED', 'UAE Dirham', N'د.إ', 'A'),
('SGD', 'Singapore Dollar', N'S$', 'A'),
('JPY', 'Japanese Yen', N'¥', 'A');
GO

-- Travel Mode Master
IF NOT EXISTS (SELECT 1 FROM TRAVELMODE_MAST WHERE TMODE_ID = 'AIR')
INSERT INTO TRAVELMODE_MAST (TMODE_ID, TMODE_NAME, TMODE_STATUS) VALUES
('AIR', 'Air Travel', 'A'),
('RAIL', 'Rail Travel', 'A'),
('BUS', 'Bus Travel', 'A'),
('CAB', 'Cab/Taxi', 'A'),
('SELF', 'Self Arranged', 'A');
GO

-- Travel Class
IF NOT EXISTS (SELECT 1 FROM TRAVEL_CLASS WHERE TCLASS_ID = 'ECO')
INSERT INTO TRAVEL_CLASS (TCLASS_ID, TCLASS_NAME, TCLASS_MODE) VALUES
('ECO', 'Economy', 'AIR'),
('BUS', 'Business', 'AIR'),
('FIRST', 'First Class', 'AIR'),
('SL', 'Sleeper', 'RAIL'),
('3AC', 'Third AC', 'RAIL'),
('2AC', 'Second AC', 'RAIL'),
('1AC', 'First AC', 'RAIL'),
('CC', 'Chair Car', 'RAIL');
GO

-- Country Master
IF NOT EXISTS (SELECT 1 FROM TRAVEL_COUNTRYMASTER WHERE CTRY_CODE = 'IN')
INSERT INTO TRAVEL_COUNTRYMASTER (CTRY_CODE, CTRY_NAME, CTRY_STATUS) VALUES
('IN', 'India', 'A'),
('US', 'United States', 'A'),
('GB', 'United Kingdom', 'A'),
('AE', 'United Arab Emirates', 'A'),
('SG', 'Singapore', 'A'),
('JP', 'Japan', 'A'),
('DE', 'Germany', 'A'),
('FR', 'France', 'A');
GO

-- LOV Categories
IF NOT EXISTS (SELECT 1 FROM TRAVELLOV_CAT WHERE LOVCAT_ID = 'STAYTYPE')
INSERT INTO TRAVELLOV_CAT (LOVCAT_ID, LOVCAT_NAME, LOVCAT_STATUS) VALUES
('STAYTYPE', 'Accommodation Type', 'A'),
('CABTYPE', 'Cab Type', 'A'),
('BOOKSTATUS', 'Booking Status', 'A'),
('TPSTATUS', 'Tour Plan Status', 'A'),
('PAYMODE', 'Payment Mode', 'A');
GO

-- LOV Values
IF NOT EXISTS (SELECT 1 FROM TRAVELLOV_MAST WHERE LOV_ID = 1)
INSERT INTO TRAVELLOV_MAST (LOV_ID, LOV_CATID, LOV_VALUE, LOV_DESCRIPTION, LOV_STATUS) VALUES
(1, 'STAYTYPE', 'HOTEL', 'Hotel Accommodation', 'A'),
(2, 'STAYTYPE', 'GUESTHOUSE', 'Company Guest House', 'A'),
(3, 'STAYTYPE', 'SERVICED', 'Serviced Apartment', 'A'),
(4, 'CABTYPE', 'SEDAN', 'Sedan Car', 'A'),
(5, 'CABTYPE', 'SUV', 'SUV/MUV', 'A'),
(6, 'CABTYPE', 'TEMPO', 'Tempo Traveller', 'A'),
(7, 'BOOKSTATUS', 'D', 'Draft', 'A'),
(8, 'BOOKSTATUS', 'S', 'Submitted', 'A'),
(9, 'BOOKSTATUS', 'A', 'Approved', 'A'),
(10, 'BOOKSTATUS', 'R', 'Rejected', 'A'),
(11, 'BOOKSTATUS', 'C', 'Cancelled', 'A'),
(12, 'TPSTATUS', 'D', 'Draft', 'A'),
(13, 'TPSTATUS', 'S', 'Submitted', 'A'),
(14, 'TPSTATUS', 'A', 'Approved', 'A'),
(15, 'TPSTATUS', 'R', 'Rejected', 'A'),
(16, 'PAYMODE', 'BANK', 'Bank Transfer', 'A'),
(17, 'PAYMODE', 'CASH', 'Cash', 'A'),
(18, 'PAYMODE', 'CHQ', 'Cheque', 'A');
GO

PRINT 'Seed data inserted successfully.';
GO
