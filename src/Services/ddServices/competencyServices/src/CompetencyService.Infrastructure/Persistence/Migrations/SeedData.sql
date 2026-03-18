-- Seed data for Competency Module (DDDB)
-- Run after EF migrations

USE DDDB;
GO

-- ─── Core Competencies ────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM DD_COMPENDMAST WHERE CM_CPD_NUM = 1001)
BEGIN
    INSERT INTO DD_COMPENDMAST (CM_CPD_NUM, CM_CPD_NAM, CM_EFF_DAT, CM_CPD_TYPE, CM_POS_IND, CM_NEG_IND)
    VALUES
        (1001, 'Leadership & Influence', '2024-01-01', 'CORE',
         'Inspires and motivates teams; demonstrates decisiveness',
         'Avoids confrontation; fails to provide direction'),
        (1002, 'Communication Skills', '2024-01-01', 'CORE',
         'Articulates ideas clearly; listens actively',
         'Unclear messaging; poor listening skills'),
        (1003, 'Problem Solving & Analysis', '2024-01-01', 'CORE',
         'Identifies root causes; devises effective solutions',
         'Applies superficial fixes; ignores data'),
        (1004, 'Teamwork & Collaboration', '2024-01-01', 'CORE',
         'Fosters cooperation; supports team goals',
         'Works in silos; undermines team efforts'),
        (1005, 'Customer Focus', '2024-01-01', 'FUNC',
         'Understands customer needs; delivers value',
         'Ignores feedback; prioritises internal processes over customers');
END;
GO

-- ─── Rating Scales ────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM COMPETENCY_RATING_SCALE WHERE COMPETENCY_ID = 1001)
BEGIN
    INSERT INTO COMPETENCY_RATING_SCALE (COMPETENCY_ID, R1_DESC, R2_DESC, R3_DESC, R4_DESC, R5_DESC)
    VALUES
        (1001,
         'Does not demonstrate leadership behaviour',
         'Occasionally shows leadership; needs guidance',
         'Consistently leads effectively in own area',
         'Recognised leader across departments',
         'Exceptional, visionary leader; role model for the organisation'),
        (1002,
         'Communication is unclear and causes confusion',
         'Communicates adequately with prompting',
         'Communicates clearly in most situations',
         'Excellent communicator; adapts to all audiences',
         'Sets the standard for communication excellence'),
        (1003,
         'Unable to identify problems independently',
         'Identifies obvious problems with support',
         'Independently resolves routine problems',
         'Resolves complex problems systematically',
         'Creates frameworks adopted organisation-wide'),
        (1004,
         'Works in isolation; disrupts team cohesion',
         'Participates in team activities with prompting',
         'Actively contributes to team success',
         'Strengthens team through mentoring and support',
         'Builds high-performing teams; recognised team builder'),
        (1005,
         'Unaware of customer needs',
         'Basic awareness; responds reactively',
         'Proactively addresses customer needs',
         'Champions customer-centric approaches',
         'Sets strategic direction for customer experience');
END;
GO

-- ─── Competency Indicators ────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM DD_COMPETENCY_IND WHERE COMP_NUM = 1001)
BEGIN
    INSERT INTO DD_COMPETENCY_IND (SRL_NO, BAND, COMP_NUM, IND_FLAG, IND_DEFN)
    VALUES
        (1, 'M1', 1001, 'P', 'Sets clear vision and direction for the team'),
        (2, 'M1', 1001, 'P', 'Coaches and develops team members proactively'),
        (3, 'M1', 1001, 'N', 'Micromanages or disempowers the team'),
        (4, 'M2', 1002, 'P', 'Actively listens and seeks to understand before responding'),
        (5, 'M2', 1002, 'N', 'Delivers mixed messages; inconsistent communication');
END;
GO

-- ─── Band Core Competency Mappings ────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM BAND_CORECOMPETENCY WHERE BAND_ID = 1)
BEGIN
    INSERT INTO BAND_CORECOMPETENCY (BAND_ID, COMPETENCY_ID)
    VALUES
        (1, 1001), (1, 1002), (1, 1004),
        (2, 1001), (2, 1002), (2, 1003), (2, 1004),
        (3, 1001), (3, 1002), (3, 1003), (3, 1004), (3, 1005);
END;
GO

PRINT 'Seed data inserted successfully.';
GO
