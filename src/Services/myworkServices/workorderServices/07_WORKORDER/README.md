# WORKORDER Module Documentation

## Overview
The WORKORDER Module manages work order creation, task assignment, tracking, and completion for the MYWORKDB system. It provides a comprehensive system for managing work items and their associated tasks.

## Module Code
**07_WORKORDER**

## Key Tables

### Core Workorder Tables
| Table Name | Purpose |
|-----------|---------|
| WORK_ORDER | Work order master records with assignment and status tracking |
| WORK_TASK | Individual tasks assigned to work orders with progress tracking |

## Key Functions

| Function Name | Purpose |
|---------------|---------|
| fn_GetTaskCompletionPercentage | Calculates completion % for tasks in a work order |

## Key Procedures

### Work Order Management
| Procedure Name | Purpose |
|----------------|---------|
| usp_CreateWorkOrder | Create a new work order with initial assignment |
| usp_AssignTaskToWorkOrder | Add a task to an existing work order |
| usp_CompleteTask | Mark a task as complete with actual hours and remarks |

## Files in This Module

### Table Definition Scripts
- **07_WORKORDER_Tables.sql** - All table definitions including work orders and tasks

### Procedure Scripts
- **07_WORKORDER_Procedures.sql** - Functions and stored procedures for work order operations
  - Function for task completion percentage calculation
  - Procedure for work order creation
  - Procedure for task assignment
  - Procedure for task completion

## Implementation Instructions

1. Execute `07_WORKORDER_Tables.sql` to create all table definitions
2. Execute `07_WORKORDER_Procedures.sql` to create procedures and functions
3. Verify all tables are created successfully
4. Configure work order status codes and task tracking

## Requirements Verification

- ✓ Module folder created: `07_WORKORDER`
- ✓ Tables script created with module name prefix
- ✓ Procedures script created with module name prefix
- ✓ Folder contains all WORKORDER-related scripts
- ✓ Procedures and functions included
- ✓ Documentation included

## Table Relationships

```
WORK_ORDER (1) ----< (M) WORK_TASK
  - PK: WORK_ORDER_ID
  - FK in WORK_TASK: WORK_ORDER_ID
```

## Status Codes

### Work Order Status
- **O** - Open
- **C** - Closed
- **A** - Archived

### Task Status
- **O** - Open
- **C** - Completed
- **A** - Archived
- **P** - Paused

## Key Features

1. **Work Order Tracking** - Create and manage work orders with due dates and assignments
2. **Task Management** - Break work orders into tasks with estimated hours
3. **Progress Monitoring** - Track actual hours and completion status
4. **Completion Metrics** - Calculate task completion percentage for work orders
5. **Audit Trail** - Track creation, updates, and completion with timestamps

## Notes
- Transactional procedures with error handling and rollback capability
- Support for time tracking with estimated vs. actual hours
- Completion remarks for task documentation
- Supports multi-level assignment tracking (created by, completed by, updated by)
- Index optimization for common query patterns

---
Generated: March 9, 2026
