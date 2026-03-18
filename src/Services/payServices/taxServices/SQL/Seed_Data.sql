-- Tax Service Seed Data Script
-- SQL script to manually seed the Tax Service database
-- Run this script in SQL Server Management Studio after migrations are applied

USE [TaxService]; -- Change database name if different

-- Clear existing seed data (optional - comment out if you want to preserve existing data)
-- DELETE FROM [dbo].[TaxMarginalDetails];
-- DELETE FROM [dbo].[ConditionalMasters];

-- Insert Sample Conditional Masters (Payees)
IF NOT EXISTS (SELECT 1 FROM [dbo].[ConditionalMasters] WHERE [PayeeId] = 'PAY001')
BEGIN
    INSERT INTO [dbo].[ConditionalMasters]
    (
        [PayeeId],
        [PayeeName],
        [PayeeAddress],
        [PayeePAN],
        [TaxRegime],
        [FinancialYear],
        [TotalExemption],
        [TotalExemptionCurrency],
        [TotalDeduction],
        [TotalDeductionCurrency],
        [IsActive],
        [CreatedAt],
        [CreatedBy],
        [IsDeleted]
    )
    VALUES
    (
        'PAY001',
        'ABC Corporation Ltd.',
        '123 Business Park, New Delhi, 110001',
        'AAAA0001K',
        'Old',
        YEAR(GETUTCDATE()),
        75000.00,
        'INR',
        250000.00,
        'INR',
        1,
        GETUTCDATE(),
        'admin',
        0
    );
    
    PRINT 'Inserted Payee: ABC Corporation Ltd.';
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConditionalMasters] WHERE [PayeeId] = 'PAY002')
BEGIN
    INSERT INTO [dbo].[ConditionalMasters]
    (
        [PayeeId],
        [PayeeName],
        [PayeeAddress],
        [PayeePAN],
        [TaxRegime],
        [FinancialYear],
        [TotalExemption],
        [TotalExemptionCurrency],
        [TotalDeduction],
        [TotalDeductionCurrency],
        [IsActive],
        [CreatedAt],
        [CreatedBy],
        [IsDeleted]
    )
    VALUES
    (
        'PAY002',
        'XYZ Industries Pvt. Ltd.',
        '456 Industrial Area, Mumbai, 400016',
        'BBBB0002K',
        'New',
        YEAR(GETUTCDATE()),
        75000.00,
        'INR',
        50000.00,
        'INR',
        1,
        GETUTCDATE(),
        'admin',
        0
    );
    
    PRINT 'Inserted Payee: XYZ Industries Pvt. Ltd.';
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConditionalMasters] WHERE [PayeeId] = 'PAY003')
BEGIN
    INSERT INTO [dbo].[ConditionalMasters]
    (
        [PayeeId],
        [PayeeName],
        [PayeeAddress],
        [PayeePAN],
        [TaxRegime],
        [FinancialYear],
        [TotalExemption],
        [TotalExemptionCurrency],
        [TotalDeduction],
        [TotalDeductionCurrency],
        [IsActive],
        [CreatedAt],
        [CreatedBy],
        [IsDeleted]
    )
    VALUES
    (
        'PAY003',
        'Global Tech Solutions Pvt. Ltd.',
        '789 Tech Park, Bangalore, 560001',
        'CCCC0003K',
        'Old',
        YEAR(GETUTCDATE()),
        150000.00,
        'INR',
        0.00,
        'INR',
        1,
        GETUTCDATE(),
        'admin',
        0
    );
    
    PRINT 'Inserted Payee: Global Tech Solutions Pvt. Ltd.';
END;

-- Insert Sample Tax Marginal Details (Employees)
-- Employee 1: EMP001 with 900000 gross income
IF NOT EXISTS (SELECT 1 FROM [dbo].[TaxMarginalDetails] WHERE [EmployeeSystemId] = 'EMP001' AND [FinancialYear] = YEAR(GETUTCDATE()))
BEGIN
    INSERT INTO [dbo].[TaxMarginalDetails]
    (
        [EmployeeSystemId],
        [FinancialYear],
        [GrossIncome],
        [GrossIncomeCurrency],
        [StandardDeduction],
        [StandardDeductionCurrency],
        [TaxableIncome],
        [TaxableIncomeCurrency],
        [CalculatedTax],
        [CalculatedTaxCurrency],
        [Exemptions],
        [Remarks],
        [CreatedAt],
        [CreatedBy],
        [IsDeleted]
    )
    VALUES
    (
        'EMP001',
        YEAR(GETUTCDATE()),
        900000.00,
        'INR',
        50000.00,
        'INR',
        850000.00,
        'INR',
        173000.00,  -- Calculated tax = ((850000-500000)*20% + 500000*5%) + 4% cess
        'INR',
        '',
        'Manual Calculation - FY' + CAST(YEAR(GETUTCDATE()) AS VARCHAR),
        GETUTCDATE(),
        'admin',
        0
    );
    
    PRINT 'Inserted Employee: EMP001 with gross income 900000';
END;

-- Employee 2: EMP002 with 1500000 gross income
IF NOT EXISTS (SELECT 1 FROM [dbo].[TaxMarginalDetails] WHERE [EmployeeSystemId] = 'EMP002' AND [FinancialYear] = YEAR(GETUTCDATE()))
BEGIN
    INSERT INTO [dbo].[TaxMarginalDetails]
    (
        [EmployeeSystemId],
        [FinancialYear],
        [GrossIncome],
        [GrossIncomeCurrency],
        [StandardDeduction],
        [StandardDeductionCurrency],
        [TaxableIncome],
        [TaxableIncomeCurrency],
        [CalculatedTax],
        [CalculatedTaxCurrency],
        [Exemptions],
        [Remarks],
        [CreatedAt],
        [CreatedBy],
        [IsDeleted]
    )
    VALUES
    (
        'EMP002',
        YEAR(GETUTCDATE()),
        1500000.00,
        'INR',
        50000.00,
        'INR',
        1450000.00,
        'INR',
        379200.00,  -- Calculated tax
        'INR',
        '',
        'Manual Calculation - FY' + CAST(YEAR(GETUTCDATE()) AS VARCHAR),
        GETUTCDATE(),
        'admin',
        0
    );
    
    PRINT 'Inserted Employee: EMP002 with gross income 1500000';
END;

-- Employee 3: EMP003 with 500000 gross income
IF NOT EXISTS (SELECT 1 FROM [dbo].[TaxMarginalDetails] WHERE [EmployeeSystemId] = 'EMP003' AND [FinancialYear] = YEAR(GETUTCDATE()))
BEGIN
    INSERT INTO [dbo].[TaxMarginalDetails]
    (
        [EmployeeSystemId],
        [FinancialYear],
        [GrossIncome],
        [GrossIncomeCurrency],
        [StandardDeduction],
        [StandardDeductionCurrency],
        [TaxableIncome],
        [TaxableIncomeCurrency],
        [CalculatedTax],
        [CalculatedTaxCurrency],
        [Exemptions],
        [Remarks],
        [CreatedAt],
        [CreatedBy],
        [IsDeleted]
    )
    VALUES
    (
        'EMP003',
        YEAR(GETUTCDATE()),
        500000.00,
        'INR',
        50000.00,
        'INR',
        450000.00,
        'INR',
        0.00,  -- No tax as income is under 500000 after deduction
        'INR',
        '',
        'Manual Calculation - FY' + CAST(YEAR(GETUTCDATE()) AS VARCHAR),
        GETUTCDATE(),
        'admin',
        0
    );
    
    PRINT 'Inserted Employee: EMP003 with gross income 500000';
END;

-- Employee 4: EMP004 with 2000000 gross income
IF NOT EXISTS (SELECT 1 FROM [dbo].[TaxMarginalDetails] WHERE [EmployeeSystemId] = 'EMP004' AND [FinancialYear] = YEAR(GETUTCDATE()))
BEGIN
    INSERT INTO [dbo].[TaxMarginalDetails]
    (
        [EmployeeSystemId],
        [FinancialYear],
        [GrossIncome],
        [GrossIncomeCurrency],
        [StandardDeduction],
        [StandardDeductionCurrency],
        [TaxableIncome],
        [TaxableIncomeCurrency],
        [CalculatedTax],
        [CalculatedTaxCurrency],
        [Exemptions],
        [Remarks],
        [CreatedAt],
        [CreatedBy],
        [IsDeleted]
    )
    VALUES
    (
        'EMP004',
        YEAR(GETUTCDATE()),
        2000000.00,
        'INR',
        50000.00,
        'INR',
        1950000.00,
        'INR',
        518400.00,  -- Calculated tax
        'INR',
        '',
        'Manual Calculation - FY' + CAST(YEAR(GETUTCDATE()) AS VARCHAR),
        GETUTCDATE(),
        'admin',
        0
    );
    
    PRINT 'Inserted Employee: EMP004 with gross income 2000000';
END;

-- Display results
PRINT '
=======================================================
Seed Data Insertion Complete
=======================================================';

PRINT 'Payees in database:';
SELECT [Id], [PayeeId], [PayeeName], [TaxRegime], [IsActive] 
FROM [dbo].[ConditionalMasters] 
ORDER BY [CreatedAt];

PRINT '';
PRINT 'Employee Tax Details in database:';
SELECT [Id], [EmployeeSystemId], [FinancialYear], [GrossIncome], [CalculatedTax]
FROM [dbo].[TaxMarginalDetails]
ORDER BY [CreatedAt];

PRINT '
Seed data successfully inserted!
Total Payees: ' + CAST((SELECT COUNT(*) FROM [dbo].[ConditionalMasters]) AS VARCHAR) + '
Total Tax Records: ' + CAST((SELECT COUNT(*) FROM [dbo].[TaxMarginalDetails]) AS VARCHAR);
