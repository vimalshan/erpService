# RISK Module Documentation

## Overview
The RISK Module manages enterprise risk identification, assessment, mitigation, monitoring, and self-assessment processes for the MYWORKDB system.

## Module Code
**04_RISK**

## Key Tables

### Risk Master Tables
| Table Name | Purpose |
|-----------|---------|
| RISK_MASTER | Core risk records with assessment and mitigation details |
| RISKTYPE_MASTER | Risk type categorization |
| RISKIMPACT_MASTER | Impact rating scale and definitions |
| RISKPROB_MASTER | Probability rating scale and definitions |
| RISKRATING_MASTER | Overall risk rating matrix |
| RISKRESP_MASTER | Risk response strategy types |

### Risk Organizational Tables
| Table Name | Purpose |
|-----------|---------|
| RISKDIVISION_MASTER | Division/Business unit definitions |
| RISKDIVISIONUNIT_MAP | Division-to-unit relationships |
| RISK_FUNCTIONMAST | Functional area definitions |
| RISK_DIVISIONFUNCTIONMAP | Function-to-division mappings |
| RISK_UNITDET | Risk-to-unit assignments |
| RISKUNIT_CHAMPMAP | Risk champion assignments by level |

### Risk Detail Tables
| Table Name | Purpose |
|-----------|---------|
| RISK_CAUSES | Root cause analysis for risks |
| RISK_CONTROLS | Control descriptions and effectiveness |
| RISK_IMPACT | Risk impact assessments |
| RISK_EVENT | Risk event tracking |
| RISK_MONITOR | Risk monitoring frequency settings |
| RISK_FREQUENCYMAP | Monitoring frequency configuration |

### Risk Approval & Mitigation Tables
| Table Name | Purpose |
|-----------|---------|
| RISK_APPDET | Risk approval workflow details |
| RISK_MITIGATION | Mitigation action plans |
| RISK_MITIGATIONACTION | Individual mitigation actions |
| RISK_MITAPPDET | Mitigation approval tracking |

### Risk Assessment Tables
| Table Name | Purpose |
|-----------|---------|
| RISK_SELFASSDET | Self-assessment records |
| RISK_EVENTASSDET | Event assessment tracking |
| RISK_SELFASSCOMMENT | Assessment comments and notes |

## Files in This Module

### Table Definition Scripts
- **04_RISK_Tables.sql** - All table creation scripts for RISK module

### Procedure Scripts
- **04_RISK_Procedures.sql** - Stored procedures and functions (to be populated)

## Implementation Instructions

1. Execute `04_RISK_Tables.sql` to create all table definitions
2. Execute `04_RISK_Procedures.sql` to create procedures and functions
3. Verify all tables are created successfully
4. Configure risk assessment and approval workflows

## Requirements Verification

- ✓ Module folder created: `04_RISK`
- ✓ Tables script created with module name prefix
- ✓ Procedures script created with module name prefix
- ✓ Folder contains only RISK-related scripts
- ✓ Documentation included

## Notes
- Comprehensive risk assessment framework from identification to mitigation
- Multi-level organizational hierarchy support (Organization, Business, Division, Unit)
- Flexible approval workflows for risks and mitigations
- Residual risk tracking after control implementation
- Self-assessment meeting management with evidence tracking

---
Generated: March 9, 2026
