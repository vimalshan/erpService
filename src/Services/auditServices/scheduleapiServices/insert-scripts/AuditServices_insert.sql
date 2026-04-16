-- Insert sample data for AuditServices table
-- Maps each Audit (1-20) to the certification Service/standard it covers.
-- AuditId refs Audits.auditId; ServiceId refs Services.ServiceId.
-- Services: 1=ISO9001, 2=ISO14001, 3=ISO45001, 4=ISO27001, 5=ISO50001,
--           6=ISO22000, 7=HACCP, 15=ISM

INSERT INTO [dbo].[AuditServices]
    ([AuditId], [ServiceId], [IsActive], [Status], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy])
VALUES
-- Acme Corporation audits
(1,  1, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 1:  Acme ISO9001 ICA
(2,  1, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 2:  Acme ISO9001 Surveillance Y1
(3,  2, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 3:  Acme ISO14001 ICA
(4,  2, 1, 'active',    GETDATE(), GETDATE(), 1, 1),  -- Audit 4:  Acme ISO14001 Surveillance Y1
(5,  3, 1, 'active',    GETDATE(), GETDATE(), 1, 1),  -- Audit 5:  Acme ISO45001 Pre-Assessment

-- TechFlow Industries audits
(6,  1, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 6:  TechFlow ISO9001 ICA
(7,  1, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 7:  TechFlow ISO9001 Surveillance Y1
(8,  1, 1, 'active',    GETDATE(), GETDATE(), 1, 1),  -- Audit 8:  TechFlow ISO9001 Surveillance Y2
(9,  4, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 9:  TechFlow ISO27001 ICA
(10, 4, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 10: TechFlow ISO27001 Surveillance

-- Green Energy Solutions audits
(11, 2, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 11: Green Energy ISO14001 ICA
(12, 5, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 12: Green Energy ISO50001 ICA
(13, 5, 1, 'active',    GETDATE(), GETDATE(), 1, 1),  -- Audit 13: Green Energy ISO50001 Surveillance
(14, 5, 1, 'active',    GETDATE(), GETDATE(), 1, 1),  -- Audit 14: Green Energy ISO50001 Pre-Assessment

-- Maritime Solutions audits
(15, 1,  1, 'completed', GETDATE(), GETDATE(), 1, 1), -- Audit 15: Maritime ISO9001 ICA
(16, 15, 1, 'completed', GETDATE(), GETDATE(), 1, 1), -- Audit 16: Maritime ISM ICA
(17, 15, 1, 'active',    GETDATE(), GETDATE(), 1, 1), -- Audit 17: Maritime ISM Surveillance
(18, 3,  1, 'completed', GETDATE(), GETDATE(), 1, 1), -- Audit 18: Maritime ISO45001 ICA

-- Food Excellence Corp audits
(19, 6, 1, 'completed', GETDATE(), GETDATE(), 1, 1),  -- Audit 19: Food Excellence ISO22000 ICA
(20, 7, 1, 'completed', GETDATE(), GETDATE(), 1, 1);  -- Audit 20: Food Excellence HACCP ICA
