# CSA Module Documentation

## Overview
The CSA (Control Self Assessment) Module manages control self-assessment operations, survey management, evidence tracking, and process controls for the MYWORKDB system.

## Module Code
**02_CSA**

## Key Tables

### Core CSA Tables
| Table Name | Purpose |
|-----------|---------|
| CSA_MAIN | Main control definitions and properties |
| CSA_EVIDENCE | Evidence attachments for controls |
| CSA_RCSURVEYMAIN | Survey master records |
| CSA_RCSURVEYQUESTION | Survey questions linked to controls |
| CSA_RCSURVEYFEED | Responses and feedback to survey questions |
| CSA_RCSURVEYATTACHMENT | Attachments for survey feedback |
| CSA_UNITMASTER | Unit hierarchy for CSA framework |
| CSA_PROCESSMAST | Process definitions for controls |
| CSA_SUBPROCESSMAST | Sub-process definitions |
| CSA_RCUNITMAPDET | Control-to-unit mappings with owner assignment |
| CSA_MAIN_UPLOAD | Bulk upload staging table for controls |
| CSA_MAIN_UPLOADERR | Error records from bulk uploads |
| CSA_USERS | User master for CSA access |
| CSADATA | Additional CSA data tracking |

## Files in This Module

### Table Definition Scripts
- **02_CSA_Tables.sql** - All table creation scripts for CSA module
- Includes indexes on foreign key columns for performance

### Procedure Scripts
- **02_CSA_Procedures.sql** - Stored procedures and functions (to be populated)

## Implementation Instructions

1. Execute `02_CSA_Tables.sql` to create all table definitions and indexes
2. Execute `02_CSA_Procedures.sql` to create procedures and functions
3. Verify all tables are created successfully
4. Configure CSA business rules and validation logic

## Requirements Verification

- ✓ Module folder created: `02_CSA`
- ✓ Tables script created with module name prefix
- ✓ Procedures script created with module name prefix
- ✓ Folder contains only CSA-related scripts
- ✓ Documentation included
- ✓ Indexes created on frequently queried columns

## Notes
- Supporting indexes have been created for optimal query performance
- Survey and feedback structure supports multiple assessment cycles
- Upload tables support bulk import operations
- Evidence management links controls to supporting documentation

---
Generated: March 9, 2026
