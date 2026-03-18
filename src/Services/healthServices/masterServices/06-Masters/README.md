# Masters Module

## Purpose
Centralized master data and list of values management used across all HEALTHDB modules.

## Tables

### LOV_TYPEMASTER
**Primary Key**: LOV_TYPECODE (CHAR(3))

**Purpose**: Define types of list of values

**Fields**:
- LOV_TYPECODE: 3-character code (e.g., "MED", "INC", "TST")
- LOV_TYPENAME: Descriptive name

**Examples**:
```
LOV_TYPECODE | LOV_TYPENAME
---------------------------------
MED          | Medicine Type
INJ          | Injury Type
TST          | Test Type
CVG          | Coverage Type
SYM          | Symptom Type
```

### LOV_MASTER
**Primary Key**: LOV_ID (BIGINT)

**Purpose**: Store actual list of values for dropdowns and validations

**Fields**:
- LOV_TYPE: Reference to LOV_TYPEMASTER (CHAR(3))
- LOV_ID: Unique identifier (BIGINT)
- LOV_NAME: Display value (VARCHAR(2000))

**Examples**:

#### Medicine Types (LOV_TYPE = 'MED')
```
LOV_ID | LOV_TYPE | LOV_NAME
-------|----------|------------------
1      | MED      | Tablet
2      | MED      | Capsule
3      | MED      | Syrup
4      | MED      | Injectable
5      | MED      | Ointment
```

#### Injury Injury Types (LOV_TYPE = 'INJ')
```
LOV_ID | LOV_TYPE | LOV_NAME
-------|----------|------------------
10     | INJ      | Minor Cut
11     | INJ      | Fracture
12     | INJ      | Sprain
13     | INJ      | Burn
14     | INJ      | Contusion
```

#### Coverage Types (LOV_TYPE = 'CVG')
```
LOV_ID | LOV_TYPE | LOV_NAME
-------|----------|------------------
20     | CVG      | EMPLOYEE
21     | CVG      | FAMILY
22     | CVG      | DEPENDENT
```

#### Claim Types (LOV_TYPE = 'CLM')
```
LOV_ID | LOV_TYPE | LOV_NAME
-------|----------|------------------
30     | CLM      | IN_PATIENT
31     | CLM      | OUT_PATIENT
32     | CLM      | DENTAL
33     | CLM      | OPTICAL
34     | CLM      | EMERGENCY
```

## Design Patterns

### Hierarchical Master Data
Some LOVs form hierarchies:

```
TST (Test Type)
  ├── BLD (Blood)
  │   └── RBC, WBC, Hemoglobin
  └── URM (Urine)
      └── Protein, Glucose
```

## Data Maintenance

### Adding New LOV Type

```sql
INSERT INTO LOV_TYPEMASTER (LOV_TYPECODE, LOV_TYPENAME)
VALUES ('DIA', 'Diagnosis Type');
GO

INSERT INTO LOV_MASTER (LOV_TYPE, LOV_ID, LOV_NAME)
VALUES 
  ('DIA', 100, 'Hypertension'),
  ('DIA', 101, 'Diabetes'),
  ('DIA', 102, 'Asthma');
GO
```

### Querying LOVs

```sql
-- Get all LOV types
SELECT * FROM LOV_TYPEMASTER;

-- Get values for specific type
SELECT LOV_ID, LOV_NAME 
FROM LOV_MASTER 
WHERE LOV_TYPE = 'MED'
ORDER BY LOV_NAME;

-- Validate a value
SELECT COUNT(*) 
FROM LOV_MASTER 
WHERE LOV_TYPE = 'CVG' 
  AND LOV_NAME = 'EMPLOYEE';
```

## Usage Across Modules

### AccidentManagement
- Injury types (LOV_TYPE = 'INJ')
- Body parts (LOV_TYPE = 'BDP')

### HealthCheckup
- Symptom types (LOV_TYPE = 'SYM')
- Test types (LOV_TYPE = 'TST')
- Field types (LOV_TYPE = 'FLD')

### MedicineManagement
- Medicine types (LOV_TYPE = 'MED')
- Packaging types (LOV_TYPE = 'PKG')

### InsuranceManagement
- Coverage types (LOV_TYPE = 'CVG')
- Claim types (LOV_TYPE = 'CLM')
- Plan types (LOV_TYPE = 'PLN')

### MedicalVisit
- Visit types (LOV_TYPE = 'VIS')
- Diagnosis types (LOV_TYPE = 'DIA')

## Index Strategy

```sql
CREATE INDEX IDX_LOV_MASTER_LOV_TYPE 
ON LOV_MASTER(LOV_TYPE);
```

This improves performance for finding values by type.

## Best Practices

1. **Unique IDs**: Use sequential numbering within type ranges
2. **Naming**: Use uppercase for system codes
3. **Descriptions**: Use descriptive LOV_NAME values
4. **Maintenance**: Regular audit of unused LOVs
5. **Relationships**: Document parent-child relationships
6. **Version Control**: Track LOV changes in history table

## Future Extensions

Consider extending with:
- LOV effective/expiry dates
- LOV descriptions
- LOV ordering sequence
- User-defined LOVs
- Multi-language support

## Sample Integration Query

```sql
-- Get employee list choosing insurance coverage type
SELECT DISTINCT LOV_ID, LOV_NAME 
FROM LOV_MASTER 
WHERE LOV_TYPE = 'CVG' 
ORDER BY LOV_ID;

-- Result shows options: EMPLOYEE, FAMILY, DEPENDENT
```

This makes UI dropdowns data-driven and reduces hardcoding.
