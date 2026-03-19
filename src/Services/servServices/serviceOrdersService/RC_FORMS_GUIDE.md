# Service Forms - RC Tables Documentation

## Overview

RC (Reference) tables contain service checkpoint forms that track the progress and completion of service calls. These forms capture information at different stages of the service lifecycle.

## RC17 - Initial Service Checkpoint (Parts Receipt)
**Purpose**: Document parts receipt and initial service assessment

**Key Fields**:
- `SERNO_DELL` - Service number
- `DATE_TIME` - Checkpoint timestamp
- `REMARKS` - Initial remarks/assessment
- `MOBILE_SUBMITION_DATE` - When submitted from field (mobile)
- `ISONLINE_MODE` - Online submission flag

**Typical Use**:
- Engineer arrives at customer site
- Receives parts or confirms no parts needed
- Documents condition upon arrival
- One entry per service call

---

## RC52 - Audit/Quality Check Form
**Purpose**: Quality assurance and audit findings

**Key Fields**:
- `SERNO_DELL` - Service number
- `RC_AUDIT_DATE` - Audit date
- `RC_FINDINGS` - Initial audit findings
- `RC_CUSTOMER_VOICE` - Customer feedback during audit
- `RC_AUDIT_RESULT` - Pass/Fail result
- `RC_CORRECTIVE_ACTION` - Required corrective actions
- `RC_AUDIT_BY` - Auditor ID
- `HO_AUDIT_DATE` - Head Office audit date (if applicable)
- `HO_FINDINGS`, `HO_AUDIT_RESULT`, `HO_CORRECTIVE_ACTION` - HO audit details

**Typical Use**:
- Quality assurance checkpoint
- May be triggered by customer complaint
- Documents follow-up actions
- Can have multiple entries per service

---

## RC53 - Service Completion Form
**Purpose**: Final service completion documentation

**Key Fields**:
- `SERNO_DELL` - Service number
- `RESULT_CODE` - Service result code
- `COMPL_DATE` - Completion date
- `ESUR_EDU` - Education provided (Yes/No)
- `COLD_BOOT_DONE` - Cold boot test done (Yes/No)
- `PRTS_CLTD` - All parts collected (Yes/No)
- `POH` - Parts on hold (Yes/No)
- `CUST_SAT` - Customer satisfaction rating
- `CUST_SAT_BY_ENG` - Engineer's satisfaction assessment
- `CUST_RECOMENDATION` - Customer recommendation
- `ACTIVITY_DONE` - Detailed work performed
- `ACTIVITY_CARRIER_OUTLIST` - Service carrier activity list
- `START_PLACE`, `END_PLACE`, `DISTANCE` - Travel tracking
- `ADDITION_INFO` - Additional notes

**Typical Use**:
- Engineer completes work and documents results
- Records customer satisfaction
- Captures what was actually done
- Final document in service lifecycle
- One main entry per service call

---

## RC95 - Equipment Return Form
**Purpose**: Document equipment return or exchange

**Key Fields**:
- `SERNO_DELL` - Service number
- `REMARKS` - Return condition/remarks
- `MOBILE_SUBMITION_DATE` - Return submission date
- `MobileIMEI` - Mobile device submitted

**Typical Use**:
- Return of customer equipment
- Received back from repair
- Equipment swap tracking
- Can indicate completion trigger

---

## RC96 - Final Verification Form
**Purpose**: Final verification that service is complete and acceptable

**Key Fields**:
- `SERNO_DELL` - Service number
- `REMARKS` - Final verification remarks
- `MOBILE_SUBMITION_DATE` - Verification date
- `MobileIMEI` - Mobile device ID

**Typical Use**:
- Final quality check
- Confirms service completion
- Release/closure authorization
- Sign-off document

---

## Supporting Tables for Parts Tracking

### PARTS_USED
Parts consumed/installed during service

**Key Fields**:
- `SERNO_DELL` - Service number
- `PART_CODE` - Part code used
- `QUANTITY` - Quantity installed
- Timestamp fields

### PARTS_COLLECTED
Parts removed from equipment/collected from customer

**Key Fields**:
- `SERNO_DELL` - Service number
- `PART_NO` - Part number
- `QUANTITY` - Quantity collected
- `REASON` - Reason for collection
- `TEN_DATE` - Tender/collection date

### DOA_PARTS
Dead On Arrival - parts that arrived defective

**Key Fields**:
- `SERNO_DELL` - Service number
- `PPID` - Part ID
- Timestamp fields

---

## ACTIVITY_HISTORY
Audit trail of service progress through different states

**Key Fields**:
- `SERNO_DELL` - Service number
- `STATUS` - Current status (Pending, Assigned, In-Progress, etc.)
- `RC` - Which RC form handled the transition
- `CUSTETA_DATE` - Customer ETA
- `ONSITE_DATE` - On-site arrival
- `COMPLETION_DATE` - Work completion
- `UPDATED_DATE` - Status update timestamp

**Typical Use**:
- Tracks state changes throughout service lifecycle
- One row per status change
- Provides complete audit trail
- Helps with SLA tracking

---

## Service Lifecycle Flow

```
Service Created (RC17)
      ↓
Parts Received / Inspection (RC17)
      ↓
Service Work (PARTS_USED, PARTS_COLLECTED)
      ↓
QA Audit (RC52) [Optional/Triggered]
      ↓
Service Completion (RC53)
      ↓
Equipment Return (RC95) [If applicable]
      ↓
Final Verification (RC96)
      ↓
Service Closed
```

---

## Common Queries

```sql
-- Get service completion status
SELECT h.SERNO_DELL, 
       CASE WHEN r17.ID IS NOT NULL THEN 'RC17: Received'
            WHEN r52.ID IS NOT NULL THEN 'RC52: In Audit'
            WHEN r53.ID IS NOT NULL THEN 'RC53: Completed'
            WHEN r95.ID IS NOT NULL THEN 'RC95: Return Submitted'
            WHEN r96.ID IS NOT NULL THEN 'RC96: Verified'
            ELSE 'No Form' END AS Status
FROM SERVICE_ORDER_HDR h
LEFT JOIN RC17 r17 ON h.SERNO_DELL = r17.SERNO_DELL
LEFT JOIN RC52 r52 ON h.SERNO_DELL = r52.SERNO_DELL
LEFT JOIN RC53 r53 ON h.SERNO_DELL = r53.SERNO_DELL
LEFT JOIN RC95 r95 ON h.SERNO_DELL = r95.SERNO_DELL
LEFT JOIN RC96 r96 ON h.SERNO_DELL = r96.SERNO_DELL
WHERE h.SERNO_DELL = @ServiceNumber;

-- Get service completion timeline
SELECT SERNO_DELL, STATUS, ONSITE_DATE, COMPLETION_DATE,
       DATEDIFF(HOUR, ONSITE_DATE, COMPLETION_DATE) AS Hours_Taken
FROM ACTIVITY_HISTORY
WHERE SERNO_DELL = @ServiceNumber
ORDER BY UPDATED_DATE;

-- Find incomplete services (RC53 missing)
SELECT h.SERNO_DELL, h.CUSTOMER_NAME, h.CMPLTD_DT, l.LOGIN_NAME
FROM SERVICE_ORDER_HDR h
LEFT JOIN RC53 r ON h.SERNO_DELL = r.SERNO_DELL
LEFT JOIN LOGIN_MASTER l ON h.ENGINEER_ID = l.LOGIN_ID
WHERE r.ID IS NULL AND h.CALL_STATUS IN ('In-Progress', 'Awaiting-Parts')
ORDER BY h.CUSTETA_DATE;
```

---

## Best Practices

1. **RC17** should be created immediately upon service initiation
2. **RC52** is created only when QA audit is triggered
3. **RC53** must be completed before service closure
4. **RC95/RC96** are optional but recommended for equipment tracking
5. Parts should be tracked in PARTS_USED/PARTS_COLLECTED during work
6. ACTIVITY_HISTORY provides the definitive status trail
7. All forms should capture GPS/IMEI info for mobile compliance

## Integration

- All RC forms link back to `SERVICE_ORDER_HDR` via `SERNO_DELL`
- Parts tracking supports inventory management
- Activity history supports SLA tracking and reporting
- Mobile device ID (`MobileIMEI`) enables audit trail verification
