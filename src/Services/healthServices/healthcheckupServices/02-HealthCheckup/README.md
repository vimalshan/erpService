# HealthCheckup Module

## Purpose
Comprehensive medical checkup and health screening management system for employees.

## Table Structure

### Master Tables
- **FIELD_TYP_MAST**: Custom field type definitions
- **CHKUP_SYMP_MAST**: Symptoms master for health screening
- **TEST_MAST**: Medical test definitions and ranges

### Checkup Definition Tables
- **CHECKUP_MAST**: Checkup types and configurations
- **CHKUP_OTHERS**: Custom fields for specific checkups
- **CHKUP_OTHERS_LOV**: List values for custom fields
- **CHKUP_TEST**: Mapping of tests to checkups

### Test Value Tables
- **HEALTH_MINMAX_VAL**: Min/max valid ranges for tests
- **HEALTH_ENTRY_LOV**: List of valid values for test results
- **HEALTH_COUNTER**: Sequential counters for health records

### Health Record Tables
- **HEALTH_MAIN**: Main health checkup record (HM_HLT_NUM is key)
- **HEALTH_SUB**: Detailed test results and values
- **HEALTH_DYN_DET**: Dynamic field values stored separately

### Special Checkup Tables
- **CHKUP_PRE_MAIN**: Pre-employment medical checkup
- **CHKUP_PFI_HIST**: Personal & Family history with symptoms
- **HLTH_CHKUP_CARD**: Structured checkup card format
- **HLTH_CHKCARD_SUB**: Symptom entries in checkup card

## Data Model

### Checkup Hierarchy
```
Checkup (CHECKUP_MAST)
  ├── Custom Fields (CHKUP_OTHERS)
  │   └── Field Values (CHKUP_OTHERS_LOV)
  └── Tests (CHKUP_TEST)
      └── Test Results (HEALTH_SUB)
```

### Health Record Flow
1. Create HEALTH_MAIN record (parent)
2. Create HEALTH_SUB records for each test
3. Store dynamic values in HEALTH_DYN_DET
4. Record family history in CHKUP_PFI_HIST

## Key Fields

### HEALTH_MAIN
- HM_HLT_NUM (Primary Key): Unique health record number
- HM_EMP_NUM: Employee number
- HM_COM_COD: Company code
- ENT_EMP_NUM: Data entry officer
- HM_CHK_COD: Checkup code
- TEXT2-TEXT5: Flexible text fields

### HEALTH_SUB
- HS_TST_COD: Test code
- HS_TST_VAL: Test result value
- HS_TST_RMK: Remarks
- HS_VALD_FLG: Validation flag

## Indexes for Performance
- CHECKUP_MAST: On CM_CHK_COD
- HEALTH_MAIN: On HM_EMP_NUM
- HEALTH_SUB: On HM_HLT_NUM

## Typical Operations

1. **Define Checkup Type**: Create entries in CHECKUP_MAST and CHKUP_TEST
2. **Record Health Data**: Create HEALTH_MAIN and HEALTH_SUB records
3. **Store Test Values**: Update HEALTH_ENTRY_LOV and HEALTH_MINMAX_VAL
4. **Track History**: Query HEALTH_SUB for employee history

## Notes
- Supports flexible field types via FIELD_TYP_MAST
- Multiple test types per checkup
- Extensible design with custom fields
- Validation ranges per test stored separately
