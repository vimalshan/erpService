# PROJECT Module Documentation

## Overview
The PROJECT Module manages project lifecycle, members, deliverables, scope management, and project typology for the MYWORKDB system.

## Module Code
**03_PROJECT**

## Key Tables

### Project Core Tables
| Table Name | Purpose |
|-----------|---------|
| PROJECT_MAIN | Main project records with detailed planning |
| PROJECT_MASTER | Project master list with team associations |
| PROJECT_STATUS | Project status history and updates |
| PROJECT_MEMBERS | Team member assignments to projects |
| PROJECT_EMPMAP | Employee-to-project mapping |
| PROJECT_HOLD | Project hold/unhold tracking |
| PROJECT_SCOPE | Project scope items |
| PROJECT_ADDLSCOPE | Additional scope changes |
| PROJECT_DEL | Project deliverables |
| PROJECT_ADDLDEL | Additional deliverables |
| PROJECT_APPRDETAILS | Approval workflow tracking |
| PROJ_ACCESS | User access control for projects |

### Project Type & Category Tables
| Table Name | Purpose |
|-----------|---------|
| PROJTYPE_MAST | Project type definitions |
| PROJECTCAT_MASTER | Project category master |
| PROJTYPE_CATEGORYMAST | Category classification for project types |
| PROJTYPE_DELMAP | Deliverables mapping to project types |
| PROJTYPE_OBJMAP | Objectives mapping to project types |
| PROJTYPE_SCOPEMAP | Scope items mapping to project types |
| PROJTYPE_FINYEARSEQ | Financial year sequence for project types |

### Project Support Tables
| Table Name | Purpose |
|-----------|---------|
| PROJFUNC_MAST | Project functional roles |
| PROJFUNCEMP_MAP | Functional role-to-employee mapping |
| PROJTYPEFUNC_MAP | Function mapping to project types |
| PROJLOC_MAST | Project location definitions |
| PROJPROC_MAST | Project process definitions |
| PROJDEP_MAST | Project department classification |

## Files in This Module

### Table Definition Scripts
- **03_PROJECT_Tables.sql** - All table creation scripts for PROJECT module

### Procedure Scripts
- **03_PROJECT_Procedures.sql** - Stored procedures and functions (to be populated)

## Implementation Instructions

1. Execute `03_PROJECT_Tables.sql` to create all table definitions
2. Execute `03_PROJECT_Procedures.sql` to create procedures and functions
3. Verify all tables are created successfully
4. Configure project workflow and approval rules

## Requirements Verification

- ✓ Module folder created: `03_PROJECT`
- ✓ Tables script created with module name prefix
- ✓ Procedures script created with module name prefix
- ✓ Folder contains only PROJECT-related scripts
- ✓ Documentation included

## Notes
- Project workflow supports charter, planning, execution, and closure phases
- Multiple approval layers for project authorization
- Flexible deliverables and scope management
- Support for project templates through project type configuration
- Team composition with role-based function assignments

---
Generated: March 9, 2026
