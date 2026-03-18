# SPARSHDB - Modularized Database Project

## Project Overview
**Project Name:** SPARSHDB (Scholarship and Mobile Application Database)  
**Version:** 1.0  
**Created:** March 9, 2026  
**Database Engine:** SQL Server 2016+  
**Architecture:** Modular Microservices Database Pattern

## Executive Summary

SPARSHDB has been restructured into **5 independent, self-contained modules**, each managing a specific functional area. This modular architecture enables:
- **Independent Deployment:** Each module can be deployed, updated, or rolled back independently
- **Clear Separation of Concerns:** Each module has its own tables, procedures, and documentation
- **Scalability:** Easy to add new modules without affecting existing ones
- **Maintainability:** Clear boundaries and dependencies between modules
- **Reusability:** Base patterns can be applied to future modules

## Directory Structure

```
SPARSHDB/
│
├── 📄 README.md (This File)
├── 📄 00_SPARSHDB_Sequences_Setup.sql
├── 📄 01_SPARSHDB_Deployment_Guide.md
├── 📄 02_SPARSHDB_Validation_Script.sql
│
├── 📁 MOD_MobileAppManagement/
│   ├── 📄 MOD_MobileAppManagement_Tables.sql (MOB_APPDEVICE_DETAILS, MOB_LOGINDET, MOBAPP_REGISTER)
│   ├── 📄 MOD_MobileAppManagement_Procedures.sql (4 stored procedures)
│   └── 📄 MOD_MobileAppManagement_README.md (Complete documentation)
│
├── 📁 MOD_MobileExpenseManagement/
│   ├── 📄 MOD_MobileExpenseManagement_Tables.sql (MOBEXP_DET, MOBEXP_FILE)
│   ├── 📄 MOD_MobileExpenseManagement_Procedures.sql (4 stored procedures)
│   └── 📄 MOD_MobileExpenseManagement_README.md (Complete documentation)
│
├── 📁 MOD_EmployeePrideManagement/
│   ├── 📄 MOD_EmployeePrideManagement_Tables.sql (MOMENT_PRIDE)
│   ├── 📄 MOD_EmployeePrideManagement_Procedures.sql (4 stored procedures)
│   └── 📄 MOD_EmployeePrideManagement_README.md (Complete documentation)
│
├── 📁 MOD_ProblemManagement/
│   ├── 📄 MOD_ProblemManagement_Tables.sql (9 problem-related tables)
│   ├── 📄 MOD_ProblemManagement_Procedures.sql (5+ stored procedures)
│   └── 📄 MOD_ProblemManagement_README.md (Complete documentation)
│
└── 📁 MOD_ScholarshipManagement/
    ├── 📄 MOD_ScholarshipManagement_Tables.sql (4 scholarship tables)
    ├── 📄 MOD_ScholarshipManagement_Procedures.sql (5 procedures + 2 functions)
    └── 📄 MOD_ScholarshipManagement_README.md (Complete documentation)
```

## Modules at a Glance

| Module | Code | Purpose | Tables | Procedures | Status |
|--------|------|---------|--------|-----------|--------|
| **Mobile App Management** | MAM | Device registration & login tracking | 3 | 4 | ✅ Ready |
| **Mobile Expense Management** | EXP | Field expense tracking | 2 | 4 | ✅ Ready |
| **Employee Pride Management** | PRIDE | Achievement recognition | 1 | 4 | ✅ Ready |
| **Problem Management** | PROBLEM | Problem tracking & solutions | 9 | 5+ | ✅ Ready |
| **Scholarship Management** | SCHOLARSHIP | Scholarship applications & disbursements | 4 | 5P + 2F | ✅ Ready |

## Quick Start Guide

### Prerequisites
- SQL Server 2016 or later
- Database: SPARSHDB (must exist)
- Execution privileges for DDL/DML

### Fast Deployment (5 minutes)

```sql
-- Step 1: Run this first (creates all sequences)
EXECUTE 00_SPARSHDB_Sequences_Setup.sql

-- Step 2: Deploy modules in any order (examples below)

-- Mobile App Management
EXECUTE MOD_MobileAppManagement/MOD_MobileAppManagement_Tables.sql
EXECUTE MOD_MobileAppManagement/MOD_MobileAppManagement_Procedures.sql

-- Mobile Expense Management
EXECUTE MOD_MobileExpenseManagement/MOD_MobileExpenseManagement_Tables.sql
EXECUTE MOD_MobileExpenseManagement/MOD_MobileExpenseManagement_Procedures.sql

-- [Repeat for other modules...]

-- Step 3: Validate deployment
EXECUTE 02_SPARSHDB_Validation_Script.sql
```

### Modular Deployment
Deploy only the modules you need:
```sql
-- Setup sequences
EXECUTE 00_SPARSHDB_Sequences_Setup.sql

-- Deploy only Scholarship module
EXECUTE MOD_ScholarshipManagement/MOD_ScholarshipManagement_Tables.sql
EXECUTE MOD_ScholarshipManagement/MOD_ScholarshipManagement_Procedures.sql
```

## Module Details

### 1. Mobile App Management (MOD_MobileAppManagement)
**Purpose:** Manage mobile device registration and login tracking

**Key Entities:**
- Device registration with IMEI/device type
- Multi-device support per employee
- Login session tracking with GUID

**Sample Usage:**
```sql
EXEC usp_MOB_RegisterDevice
    @p_EmpSysId = 1001,
    @p_DeviceId = 'DEVICE_001',
    @p_DeviceType = 'A',
    @p_ImeiNo = '123456789012345',
    @p_UpdatedBy = 1001,
    @p_ErrorMessage = @ErrorMsg OUTPUT;
```

[📖 Full Documentation](./MOD_MobileAppManagement/MOD_MobileAppManagement_README.md)

---

### 2. Mobile Expense Management (MOD_MobileExpenseManagement)
**Purpose:** Track employee field expenses with attachments

**Key Entities:**
- Expense records by trip/project/category
- File attachments (receipts, photos, invoices)
- Multi-currency support

**Sample Usage:**
```sql
EXEC usp_EXP_RecordExpense
    @p_TripId = 5001,
    @p_CategoryId = 10,
    @p_Comment = 'Fuel for field visit',
    @p_Amount = 2500.00,
    @p_CurrencyId = 1,
    @p_EnteredBy = 1001,
    @p_ExpenseId = @ExpenseId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;
```

[📖 Full Documentation](./MOD_MobileExpenseManagement/MOD_MobileExpenseManagement_README.md)

---

### 3. Employee Pride Management (MOD_EmployeePrideManagement)
**Purpose:** Capture and celebrate employee achievements

**Key Entities:**
- Pride moments with title and description
- Image/photo attachment
- Employee recognition tracking

**Sample Usage:**
```sql
EXEC usp_PRIDE_CreatePrideMoment
    @p_Title = 'Q1 Sales Achievement',
    @p_Body = 'Sales team exceeded Q1 targets by 25%',
    @p_EmployeeSysId = 1001,
    @p_Footer = 'Team Excellence',
    @p_Location = 'Head Office',
    @p_ImagePath = '/images/achievement.jpg',
    @p_ModifiedBy = 1002,
    @p_PrideMomentId = @PrideMomentId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;
```

[📖 Full Documentation](./MOD_EmployeePrideManagement/MOD_EmployeePrideManagement_README.md)

---

### 4. Problem Management (MOD_ProblemManagement)
**Purpose:** End-to-end problem tracking with approval workflow

**Key Entities:**
- Problem creation and status tracking
- Solution proposal and implementation
- Multi-level approvals with audience control
- Comments and attachments

**Sample Usage:**
```sql
EXEC usp_PROBLEM_CreateProblem
    @p_Owner = 1001,
    @p_Description = 'Slow API response times',
    @p_Category = '01',
    @p_Impact = 'Affects user experience',
    @p_ExpectedResult = 'API response < 200ms',
    @p_UnitId = 10,
    @p_SiteId = 1,
    @p_EnteredBy = 1001,
    @p_ProblemId = @ProblemId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;
```

[📖 Full Documentation](./MOD_ProblemManagement/MOD_ProblemManagement_README.md)

---

### 5. Scholarship Management (MOD_ScholarshipManagement)
**Purpose:** Comprehensive scholarship application and disbursement management

**Key Entities:**
- Scholarship schemes with eligibility criteria
- Student application tracking
- Eligibility checking and amount calculation
- Disbursement processing with audit trail

**Sample Usage:**
```sql
-- Apply for scholarship
EXEC usp_SCHOLARSHIP_ApplyForScholarship
    @p_StudentID = 1001,
    @p_ScholarshipID = 100,
    @p_ApplicationDate = '2026-03-09',
    @p_FamilyIncome = 500000,
    @p_ApplicantID = 1001,
    @p_ApplicationID = @ApplicationID OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Approve scholarship
EXEC usp_SCHOLARSHIP_ApproveScholarship
    @p_ApplicationID = @ApplicationID,
    @p_ApprovedBy = 1005,
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Process disbursement
EXEC usp_SCHOLARSHIP_ProcessDisbursement
    @p_DisbursementID = 500,
    @p_ProcessedBy = 1006,
    @p_ReferenceNumber = 'TRF20260309001',
    @p_ErrorMessage = @ErrorMsg OUTPUT;
```

[📖 Full Documentation](./MOD_ScholarshipManagement/MOD_ScholarshipManagement_README.md)

---

## Key Features

### ✅ Data Integrity
- Primary key constraints on all tables
- Foreign key relationships with cascade rules
- Unique constraints where applicable
- Check constraints for valid values

### ✅ Performance Optimization
- Strategically indexed columns
- Sequence objects for high-performance ID generation
- Proper data type selections
- Query optimization-friendly design

### ✅ Audit Trail
- CreatedBy and CreatedOn on all tables
- UpdatedBy and UpdatedOn for modification tracking
- Timestamp fields use DATETIME2(3) for precision
- Procedure-level audit logging

### ✅ Error Handling
- Try-Catch blocks in all procedures
- Transaction rollback on errors
- Meaningful error messages returned to client
- Input validation

### ✅ Scalability
- Decimal(38) for large ID values
- NVARCHAR(MAX) for unlimited text
- Proper precision for financial amounts (19,2)
- Sequence objects for unlimited ID generation

## Database Statistics

| Category | Count | Notes |
|----------|-------|-------|
| **Tables** | 21 | Across 5 modules |
| **Stored Procedures** | 23+ | Business logic |
| **Functions** | 2 | Scholarship module |
| **Sequences** | 8 | Identity generation |
| **Indexes** | 30+ | Performance |
| **Foreign Keys** | 15+ | Data integrity |

## Naming Conventions

### Tables
```
[MODULE_PREFIX]_[ENTITY_NAME]
Example: MOB_APPDEVICE_DETAILS, SCHOLARSHIP_MASTER
```

### Columns
```
[PREFIX]_[COLUMN_NAME]
Example: MD_EMPSYSID, SCHOLARSHIP_CODE
```

### Stored Procedures
```
usp_[MODULE]_[ACTION]_[ENTITY]
Example: usp_MOB_RegisterDevice, usp_SCHOLARSHIP_ApplyForScholarship
```

### Sequences
```
seq_[ENTITY]_[ID]
Example: seq_MOB_LoginId, seq_SCHOLARSHIP_APPLICATION_Id
```

### Indexes
```
IX_[TABLE]_[COLUMN]
Example: IX_MOB_LOGIN_USERID, IX_SCHOLARSHIP_APPLICATION_STATUS
```

## Deployment Checklist

- [ ] Verify SQL Server 2016+ is installed
- [ ] Confirm SPARSHDB database exists
- [ ] Run 00_SPARSHDB_Sequences_Setup.sql
- [ ] Deploy each module:
  - [ ] MOD_MobileAppManagement (Tables + Procedures)
  - [ ] MOD_MobileExpenseManagement (Tables + Procedures)
  - [ ] MOD_EmployeePrideManagement (Tables + Procedures)
  - [ ] MOD_ProblemManagement (Tables + Procedures)
  - [ ] MOD_ScholarshipManagement (Tables + Procedures)
- [ ] Run 02_SPARSHDB_Validation_Script.sql
- [ ] Verify all objects created successfully
- [ ] Test sample procedures
- [ ] Backup database

## Troubleshooting

### Error: "Sequence does not exist"
**Cause:** Sequences not created before tables  
**Solution:** Execute 00_SPARSHDB_Sequences_Setup.sql first

### Error: "Foreign key constraint violation"
**Cause:** Child record being inserted without parent  
**Solution:** Verify parent records exist first, check cascade rules

### Error: "Table already exists"
**Cause:** Re-running scripts without cleanup  
**Solution:** Scripts include "DROP IF EXISTS" - safe to rerun

## Best Practices

### For Deployment
1. Always run sequences setup first
2. Deploy modules in any order after sequences
3. Run validation script after each deployment
4. Backup database before deployment

### For Development
1. Follow naming conventions consistently
2. Include error handling in all procedures
3. Add meaningful comments
4. Test with sample data
5. Update module README when modifying

### For Maintenance
1. Monitor index fragmentation
2. Update statistics regularly
3. Archive old data per retention policies
4. Maintain audit trail integrity
5. Regular backup and recovery testing

## Security Considerations

1. **Access Control:** Implement role-based access at module level
2. **Data Encryption:** Encrypt sensitive fields (IMEI, bank details)
3. **Audit Logging:** Maintain complete audit trail
4. **Input Validation:** Validate all parameters in procedures
5. **SQL Injection:** Use parameterized queries/procedures

## Monitoring & Maintenance

### Daily
- Monitor error logs
- Check job execution

### Weekly
- Update statistics
- Check disk space
- Monitor query performance

### Monthly
- Rebuild fragmented indexes
- Archive old data
- Review audit logs

### Quarterly
- Full database backup test
- Security audit
- Performance analysis
- Capacity planning

## Support & Documentation

### For Module Details
📖 See the README.md file in each module directory

### For Deployment Help
📖 See 01_SPARSHDB_Deployment_Guide.md

### For Technical References
📖 Refer to stored procedure comments and inline documentation

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-03-09 | Initial modularized release - 5 modules, 21 tables, 25+ procedures |

## Future Roadmap

### Planned Modules
- Payroll Management
- Attendance Management
- Leave Management
- Performance Management
- Document Management

### Enhancements
- Add comprehensive views for reporting
- Implement event notification system
- Create dashboard integration points
- Add data migration utilities
- Implement change data capture

## Contributing

When adding new features:
1. Follow naming conventions
2. Include comprehensive comments
3. Add error handling
4. Update module README
5. Add validation tests
6. Document API/procedures

## Questions & Support

For technical support or questions:
- Review module-specific README.md
- Check stored procedure comments
- Refer to troubleshooting section
- Contact Database Administrator

---

## Summary

SPARSHDB v1.0 provides a **robust, modular, scalable foundation** for managing:
- ✅ Mobile application devices and user sessions
- ✅ Employee field expense tracking
- ✅ Employee recognition and achievements
- ✅ Organization problem management
- ✅ Student scholarship programs

**Ready for production deployment!** ✨

---

**Last Updated:** March 9, 2026  
**Status:** ✅ Production Ready  
**Version:** 1.0
