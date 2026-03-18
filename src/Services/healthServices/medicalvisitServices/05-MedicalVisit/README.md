# MedicalVisit Module

## Purpose
Tracks employee medical clinic visits, diagnoses, treatments, and recommendations.

## Tables

### VISIT_MAIN
**Primary Key**: Composite based on VM_COM_COD + VM_VIS_NUM

**Purpose**: Main medical visit record

**Key Fields**:
- VM_COM_COD: Company code
- VM_VIS_NUM: Unique visit number
- VM_VIS_DAT: Visit date and time
- VM_USR_ID: Medical professional ID
- VM_PIN_NUM: PIN number of medical professional
- VM_WRK_NAM: Worker/employee name
- VM_CONTRCT_ID, VM_CONTRCT_NAM: Contractor details (if applicable)

**Clinical Information**:
- VM_PAT_DIA: Patient diagnosis
- VM_TRT_REM: Treatment remarks/recommendations
- VM_TST_ADV: Advice for tests
- VM_MED_GIV: Medicine given (yes/no/code)
- VM_NXT_REV: Next review/follow-up date

**Personnel**:
- VM_DOC_COD: Doctor code (Required)
- VM_ATT_COD: Attendant code
- VM_OTH_HOSP: Other hospital if referred

**Metadata**:
- VM_VIS_SHIFT: Shift information
- VM_VIS_TYP: Type of visit
- VM_DIA_CAT, VM_DIA_SUBCAT: Diagnosis category/subcategory
- VM_DOC_REMARKS: Doctor's remarks (up to 1000 chars)

**Audit Fields**:
- VM_ENT_USR: Entry user
- VM_ENT_NUM: Entry user PIN
- VM_ENT_DAT: Entry date
- VM_MOD_USR: Modified user
- VM_MOD_NUM: Modified user PIN
- VM_MOD_DAT: Modified date
- VM_CAN_FLG: Cancellation flag

### VISIT_SUB
**Primary Key**: Composite VM_COM_COD + VS_VIS_NUM + [implicit]

**Purpose**: Sub-records for visit details (vitals, tests, measurements)

**Fields**:
- VS_COM_COD: Company code (Required)
- VS_VIS_NUM: Visit number (Required) - Foreign key to VISIT_MAIN
- VS_TST_TYP: Test/vital type (e.g., "BP", "Temperature", "Weight")
- VS_TST_VAL: Test/vital value
- VS_SRL_NUM: Serial number for ordering entries

## Indexes

```sql
IDX_VISIT_MAIN_VM_COM_COD     -- For company-based queries
IDX_VISIT_MAIN_VM_VIS_DAT     -- For date range queries
IDX_VISIT_SUB_VS_VIS_NUM      -- Foreign key performance
```

## Data Model

### Visit Hierarchy

```
VISIT_MAIN (one record per visit)
  │
  └── VISIT_SUB (multiple vitals/tests)
      ├── Blood Pressure
      ├── Temperature
      ├── Weight
      ├── Pulse
      └── Other measurements
```

## Typical Workflows

### 1. Record a Clinic Visit

```sql
-- Insert main visit
INSERT INTO VISIT_MAIN (
    VM_COM_COD, VM_VIS_NUM, VM_VIS_DAT, VM_DOC_COD,
    VM_PAT_DIA, VM_TRT_REM, VM_ENT_USR, VM_ENT_DAT, VM_CAN_FLG
) VALUES (...)

-- Add vital signs
INSERT INTO VISIT_SUB (VS_COM_COD, VS_VIS_NUM, VS_TST_TYP, VS_TST_VAL)
VALUES ('001', <visit_num>, 'BP', '120/80')
VALUES ('001', <visit_num>, 'Temp', '98.6')
VALUES ('001', <visit_num>, 'Weight', '75')
```

### 2. Track Contractor Visits
- Use VM_CONTRCT_ID and VM_CONTRCT_NAM for contractor medical visits
- Useful for accident injury follow-ups

### 3. Follow-up Management
- VM_NXT_REV: Schedule next visit
- VM_TST_ADV: Request specific tests
- VM_DOC_REMARKS: Clinical notes

## Integration Points

### With Other Modules

1. **HealthCheckup**:
   - Visit recommendations may trigger health checkups
   - Test advice (VM_TST_ADV) can relate to TEST_MAST

2. **MedicineManagement**:
   - Medicines given in VM_MED_GIV
   - Links to MEDICINE_ISSUE via visit number

3. **AccidentManagement**:
   - Contractor visit follow-ups for accidents
   - Links VM_CONTRCT_ID to ACC_CONTRCT_LST

## Key Considerations

- **Next Review**: Must be after current visit date
- **Diagnosis Categories**: Reference VM_DIA_CAT for standardization
- **Multi-company**: VM_COM_COD ensures data isolation
- **Audit Trail**: Complete entry/modification tracking
- **Cancellation**: VM_CAN_FLG for soft deletes

## Indexes Strategy

- Company + Visit Date: For billing/monthly reports
- Visit Number: For detail lookups (VISIT_SUB)
- Entry Date: For audit trails

## Sample Queries

### Get Recent Visits
```sql
SELECT * FROM VISIT_MAIN
WHERE VM_COM_COD = '001' 
  AND VM_VIS_DAT >= DATEADD(DAY, -30, GETDATE())
ORDER BY VM_VIS_DAT DESC;
```

### Get Vitals for a Visit
```sql
SELECT VS_TST_TYP, VS_TST_VAL 
FROM VISIT_SUB
WHERE VS_VIS_NUM = @visit_num
ORDER BY VS_SRL_NUM;
```

### Pending Follow-ups
```sql
SELECT * FROM VISIT_MAIN
WHERE VM_NXT_REV < GETDATE() 
  AND VM_CAN_FLG IS NULL;
```
