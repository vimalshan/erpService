# Service Orders Microservice Database

## Overview
The Service Orders microservice manages the core business logic for service calls, orders, and work completion tracking.

## Database: `DELL_RTU_SERVICE_ORDERS`

## Tables

### SERVICE_ORDER_HDR (Header/Master)
Main service order records containing customer, equipment, and status information.

**Key Fields:**
- `SERNO_DELL` (PK) - Service number (Dell internal)
- `SAP_ID` - SAP connection ID (Unique, for integration)
- `BRANCH` - Service branch/location
- `CUSTOMER_NAME`, `CONTACT_NO`, `ADDRESS` - Customer info
- `SERVICE_TAG` - Equipment identifier
- `SLA` - Service level agreement ID (FK to MasterData.SLA_MASTER)
- `CALL_STATUS` - Current status (Pending, Assigned, In-Progress, Closed, etc.)
- `ENGINEER_ID` - Assigned field engineer
- `DISPATCH_DATE` - When call was dispatched
- `CUSTETA_DATE` - Customer ETA
- `ONSITE_DT` - On-site arrival time
- `CMPLTD_DT` - Completion date/time
- `PART_COUNT` - Expected number of parts
- `ISPARTCALL` - Whether part replacement is needed
- `ISONLINE_MODE` - Remote/online service flag

**Extended Fields:**
- Product info: `PRODUCT_ID`, `LOB` (Line of Business)
- Problem description: `PRB_DESC`, `LONG_DESC`, `REASON_CODE`
- Customer details: Alternative contact `ALT_CNTNO`, email `CUST_MAILID`
- Warranty info: `WAR_EXPIRE`, `SYSTEM_PUR_30DAYS`, `PROSUPPORT`
- Technical data: `CT`, `OS`, `SEV`, `RPT`, `P`, `S`, `D`, `DSPI`

### SERVICE_ORDER_DET (Details/Line Items)
Parts and components involved in each service order.

**Key Fields:**
- `SERNO_DELL` (FK) - Links to SERVICE_ORDER_HDR
- `PART_NO` - Part number
- `QUANTITY` - Quantity needed/provided
- `UNIQUE_ID` - Unique identifier for this line item
- `PART_STATUS` - Status code (1=Pending, 2=Completed, etc.)
- `PART_USAGE_TYPE` - How part is used (FK to MasterData.PART_USAGE_TYPE)
- `COMMODITY` - Commodity type (FK to MasterData.COMMODITY_MASTER)
- `PP_ID` - Planning part ID
- `GOOD_PPID` - Good part PP ID (after replacement)
- `DEFECTIVE_PARTNO` - Original defective part number
- `FAILURE_REASON` (FK) - Failure code from FAILURE_REASON_CODE_MASTER
- `FAILURE_REASON_OTHRES` - Free-text failure reason
- `ISDAMAGED` - Damage tracking flag

### ACTIVITY_DONE
Tracks activities completed during service calls (technical work performed).

**Key Fields:**
- `ACT_DONE` - Activity description
- Timestamps and user tracking (ENTERED_ON, ENTERED_BY, CHANGED_ON, CHANGED_BY)

## Common Queries

```sql
-- Get active service orders
SELECT * FROM SERVICE_ORDER_HDR 
WHERE CALL_STATUS NOT IN ('Closed', 'Cancelled')
ORDER BY CUSTETA_DATE;

-- Get orders by engineer
SELECT * FROM SERVICE_ORDER_HDR 
WHERE ENGINEER_ID = @EngineerId
AND CALL_STATUS IN ('Assigned', 'In-Progress');

-- Get order details with parts
SELECT h.*, d.PART_NO, d.QUANTITY, d.PART_STATUS
FROM SERVICE_ORDER_HDR h
LEFT JOIN SERVICE_ORDER_DET d ON h.SERNO_DELL = d.SERNO_DELL
WHERE h.SERNO_DELL = @SerNoDell
ORDER BY d.UNIQUE_ID;

-- Orders needing parts
SELECT * FROM SERVICE_ORDER_HDR
WHERE ISPARTCALL = 1 AND CALL_STATUS NOT IN ('Closed', 'Cancelled')
ORDER BY PARTETA_DATE;

-- SLA compliance check
SELECT h.SERNO_DELL, h.CMPLTD_DT, h.CUSTETA_DATE,
       DATEDIFF(HOUR, h.CUSTETA_DATE, h.CMPLTD_DT) AS HoursOverdue
FROM SERVICE_ORDER_HDR h
WHERE h.CMPLTD_DT > h.CUSTETA_DATE;
```

## API Patterns

```
GET    /orders                              - List all orders (with filters)
GET    /orders/{serNoDell}                  - Get order details
POST   /orders                              - Create new service order
PUT    /orders/{serNoDell}                  - Update order
PATCH  /orders/{serNoDell}/status           - Update status
GET    /orders/{serNoDell}/parts            - Get parts for order
POST   /orders/{serNoDell}/parts            - Add part to order
PUT    /orders/{serNoDell}/parts/{uniqueId} - Update part details
DELETE /orders/{serNoDell}/parts/{uniqueId} - Remove part
GET    /orders/{serNoDell}/activities       - Get completed activities
POST   /orders/{serNoDell}/activities       - Log activity
GET    /orders/engineer/{engineerId}        - Get engineer's orders
GET    /orders/branch/{branch}              - Get branch orders
```

## Status Codes

- `Pending` - Created, awaiting assignment
- `Assigned` - Assigned to engineer
- `In-Progress` - Engineer working on service
- `Awaiting-Parts` - Waiting for parts arrival
- `Closed` - Service completed
- `Cancelled` - Service cancelled
- `On-Hold` - Temporarily suspended

## Business Rules

1. Service can only be closed when all parts are received (`PART_COUNT` = parts with status '2')
2. CUSTETA_DATE cannot be before DISPATCH_DATE
3. CMPLTD_DT cannot be before ONSITE_DT
4. If ISPARTCALL=1, at least one part must be added to SERVICE_ORDER_DET
5. Engineer assignment required before dispatch

## Integration Points

- **MasterData Service**: SLA, PART codes, COMMODITY, FAILURE_REASON_CODE
- **DamageTracking Service**: Damage updates related to SERNO_DELL
- **ToolkitManagement Service**: Engineer toolkit assignments
- **Auth Service**: Engineer and user validation
- **Communication Service**: Status notifications to customers

## Performance Considerations

- Index on `SERNO_DELL`, `ENGINEER_ID`, `CALL_STATUS`, `CUSTETA_DATE`
- Partition by date for large datasets
- Archive orders older than 2 years to ARCHIVE database
- Use views for common report queries

## Future Enhancements

- [ ] Add appointment/scheduling system
- [ ] Implement customer feedback/satisfaction tracking
- [ ] Add warranty claim integration
- [ ] Parts inventory integration
- [ ] Route optimization for engineer dispatch
- [ ] Predictive maintenance suggestions
- [ ] Service contract management
