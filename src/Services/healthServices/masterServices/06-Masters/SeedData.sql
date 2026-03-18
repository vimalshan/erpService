-- Seed Data for Masters Database

USE HEALTHDB;
GO

-- Insert LOV Type Masters
INSERT INTO [dbo].[LOV_TYPEMASTER] (LOV_TYPECODE, LOV_TYPENAME) VALUES
('MED', 'Medicine Type'),
('INJ', 'Injury Type'),
('TST', 'Test Type'),
('CVG', 'Coverage Type'),
('CLM', 'Claim Type'),
('SYM', 'Symptom Type');
GO

-- Insert LOV Masters for Medicine Types (MED)
INSERT INTO [dbo].[LOV_MASTER] (LOV_TYPE, LOV_ID, LOV_NAME) VALUES
('MED', 1, 'Tablet'),
('MED', 2, 'Capsule'),
('MED', 3, 'Syrup'),
('MED', 4, 'Injectable'),
('MED', 5, 'Ointment');
GO

-- Insert LOV Masters for Injury Types (INJ)
INSERT INTO [dbo].[LOV_MASTER] (LOV_TYPE, LOV_ID, LOV_NAME) VALUES
('INJ', 10, 'Minor Cut'),
('INJ', 11, 'Fracture'),
('INJ', 12, 'Sprain'),
('INJ', 13, 'Burn'),
('INJ', 14, 'Contusion');
GO

-- Insert LOV Masters for Coverage Types (CVG)
INSERT INTO [dbo].[LOV_MASTER] (LOV_TYPE, LOV_ID, LOV_NAME) VALUES
('CVG', 20, 'EMPLOYEE'),
('CVG', 21, 'FAMILY'),
('CVG', 22, 'DEPENDENT');
GO

-- Insert LOV Masters for Claim Types (CLM)
INSERT INTO [dbo].[LOV_MASTER] (LOV_TYPE, LOV_ID, LOV_NAME) VALUES
('CLM', 30, 'IN_PATIENT'),
('CLM', 31, 'OUT_PATIENT'),
('CLM', 32, 'DENTAL'),
('CLM', 33, 'OPTICAL'),
('CLM', 34, 'EMERGENCY');
GO

-- Insert LOV Masters for Test Types (TST)
INSERT INTO [dbo].[LOV_MASTER] (LOV_TYPE, LOV_ID, LOV_NAME) VALUES
('TST', 40, 'Blood Test'),
('TST', 41, 'Urine Test'),
('TST', 42, 'X-Ray'),
('TST', 43, 'MRI'),
('TST', 44, 'CT Scan');
GO

-- Insert LOV Masters for Symptom Types (SYM)
INSERT INTO [dbo].[LOV_MASTER] (LOV_TYPE, LOV_ID, LOV_NAME) VALUES
('SYM', 50, 'Fever'),
('SYM', 51, 'Cough'),
('SYM', 52, 'Headache'),
('SYM', 53, 'Nausea'),
('SYM', 54, 'Fatigue');
GO

PRINT 'Seed data inserted successfully.';
