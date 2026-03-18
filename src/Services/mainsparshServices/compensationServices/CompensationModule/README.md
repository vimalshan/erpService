# CompensationModule Documentation

## Module Overview
The CompensationModule manages employee compensation grades, salary structures, and allowance configurations.

## Tables

### COMP_GRADE
- **Purpose**: Compensation grade master with salary and allowance definitions
- **Key Columns**: GRADE_CODE, GRADE_NAME, GRADE_LEVEL, BASE_SALARY, HRA_PERCENTAGE, DA_PERCENTAGE

## Key Features
- Grade-based salary structure
- Percentage-based allowance calculation (HRA, DA, etc.)
- Effective date range management
- Multiple grade levels support

## Grade Level Examples
- Level 1: Junior positions
- Level 2: Mid-level positions
- Level 3: Senior positions
- Level 4: Lead/Manager positions

## Deployment
```sql
:r "CompensationModule_Schema.sql"
```

## Sample Data
```sql
INSERT INTO COMP_GRADE (GRADE_CODE, GRADE_NAME, GRADE_LEVEL, BASE_SALARY, HRA_PERCENTAGE, DA_PERCENTAGE, EFFECTIVE_FROM, CREATED_BY)
VALUES ('LEVEL1', 'Entry Level', 1, 25000, 10, 8, GETDATE(), 1);
```

---
**Created**: March 09, 2026
**Version**: 1.0
