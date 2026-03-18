# TIMESHEET Module Documentation

## Overview
The TIMESHEET Module manages employee timesheets, time entries, project assignments, and task scheduling for the MYWORKDB system. It includes both traditional timesheet tracking and task scheduling components.

## Module Code
**06_TIMESHEET**

## Key Tables

### Core Timesheet Tables
| Table Name | Purpose |
|-----------|---------|
| TIMESHEET_MAIN | Main timesheet entries with daily time tracking |
| TIMESHEET_DET | Detailed timesheet allocation across projects/categories |
| TIMESHEET_USERS | User-to-team associations for timesheet access |
| TIMESHEET_MANPOWER | Aggregated manpower reports from timesheet data |

### TC (Time Collection) Module Tables
| Table Name | Purpose |
|-----------|---------|
| TCTIMESHEET_MAIN | Time collection main entries |
| TCTIMESHEET_DET | Time collection detail allocations |
| TCPROJECT_MASTER | TC projects (Team 2) |
| TCPROJECTCAT_MASTER | TC project categories |
| TCSUBCAT_MASTER | TC sub-categories for detailed tracking |
| TCSUBCAT_EMPMAP | Employee sub-category assignments |

### Timesheet Category Management Tables
| Table Name | Purpose |
|-----------|---------|
| SUBCAT_MASTER | Generic sub-category definitions |
| SUBCAT_PROJECTMAP | Sub-category-to-project mappings |
| SUBCAT_PROCESSMAP | Sub-category-to-process mappings |

### TS (Task Scheduling) Module Tables
| Table Name | Purpose |
|-----------|---------|
| TSPROJECT_MASTER | Task scheduling project definitions |
| TSSTAGE_MASTER | Project stage/phase definitions |
| TSSTAGE_EMPMAP | Stage-to-employee task assignments |
| TSTIMESHEET_DET | Time entries for task scheduling |
| TSACTIVITY_MASTER | Activity type definitions |
| TSMODULE_MAST | Module definitions for applications |
| SC_ROLLINGPLAN | Rolling plan for task management |

## Files in This Module

### Table Definition Scripts
- **06_TIMESHEET_Tables.sql** - All table creation scripts for TIMESHEET module

### Procedure Scripts
- **06_TIMESHEET_Procedures.sql** - Stored procedures and functions (to be populated)

## Implementation Instructions

1. Execute `06_TIMESHEET_Tables.sql` to create all table definitions
2. Execute `06_TIMESHEET_Procedures.sql` to create procedures and functions
3. Verify all tables are created successfully
4. Configure project categories and sub-categories

## Requirements Verification

- ✓ Module folder created: `06_TIMESHEET`
- ✓ Tables script created with module name prefix
- ✓ Procedures script created with module name prefix
- ✓ Folder contains only TIMESHEET-related scripts
- ✓ Documentation included

## Module Components

### 1. Traditional Timesheet (TIMESHEET_*)
- Daily time tracking (in/out times)
- Project allocation at granular level
- Hours distribution across work categories

### 2. Time Collection - TC (TCTIMESHEET_*, TCPROJECT_*, etc.)
- Specialized time collection for Team 2
- Project and sub-category management
- Employee assignment to sub-categories

### 3. Task Scheduling - TS (TSTIMESHEET_*, TSPROJECT_*, etc.)
- Project-based task management
- Stage/phase hierarchies
- Activity classification
- Cross-application support (SPARSH, ShriConnect, Common)

## Notes
- Multiple timesheet approaches supported for different business needs
- Granular time allocation across multiple dimensions
- Historically supports multiple applications and business areas
- Activity-based time tracking for task scheduling
- Aggregated manpower reporting for resource planning

---
Generated: March 9, 2026
