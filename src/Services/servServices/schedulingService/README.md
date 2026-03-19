# Scheduling Microservice Database

## Overview
The Scheduling microservice manages service appointment scheduling, engineer availability, and CFS (Customer Field Scheduling) with intelligent, automated assignment based on location, distance, date availability, and engineer capacity. Features include:

- **Basic Scheduling**: Appointment slots, engineer availability, blackout dates
- **Advanced CFS**: Automatic location-wise, distance-wise, date-wise assignment optimization
- **Route Optimization**: Daily route generation with efficiency scoring  
- **Performance Analytics**: Engineer productivity tracking and insights
- **Conflict Detection**: Real-time scheduling conflict detection and resolution

## Database: `DELL_RTU_SCHEDULING`

## Tables (14 Total)

### Basic Scheduling Tables (5)

#### SCHEDULE_SLOT
Available time slots for service appointments.

**Key Fields:**
- `ID` (PK) - Slot identifier
- `SLOT_DATE` - Date of the slot
- `START_TIME` - Slot start time
- `END_TIME` - Slot end time
- `ENGINEER_ID` - Assigned engineer
- `BRANCH` - Service branch
- `CAPACITY` - Total slots available
- `AVAILABLE_SLOTS` - Remaining slots
- `STATUS` - Active/Inactive

#### SERVICE_APPOINTMENT
Customer service appointment bookings.

**Key Fields:**
- `ID` (PK) - Appointment ID
- `SERNO_DELL` (FK) - Links to ServiceOrders
- `SLOT_ID` (FK) - Reserved slot
- `APPOINTMENT_DATE` - Scheduled date
- `ENGINEER_ID` - Assigned engineer
- `CUSTOMER_NAME`, `CONTACT_NO`, `ADDRESS` - Customer details
- `STATUS` - Scheduled/Confirmed/Completed/Cancelled
- `NOTES` - Additional notes

#### ENGINEER_SCHEDULE
Daily engineer availability calendar.

**Key Fields:**
- `ID` (PK)
- `ENGINEER_ID` - Engineer identifier
- `SCHEDULE_DATE` - Working date
- `WORKING_HOURS` - Shift (e.g., "09:00-18:00")
- `AVAILABILITY_STATUS` - Available/On Leave/Working
- `ASSIGNMENTS_AVAILABLE` - Open slots
- `CURRENT_WORKLOAD` - Current assignments

#### SLOT_ASSIGNMENT
Assignment tracking between appointments and engineers.

**Key Fields:**
- `ID` (PK)
- `APPOINTMENT_ID` (FK) - Links to SERVICE_APPOINTMENT
- `SLOT_ID` (FK) - Links to SCHEDULE_SLOT
- `ENGINEER_ID` - Assigned engineer
- `ASSIGNMENT_STATUS` - Assigned/Dispatched/In-Progress

#### BLACKOUT_DATE
Non-working dates (holidays, maintenance, etc.)

**Key Fields:**
- `ID` (PK)
- `BLACKOUT_DATE` - Date when service is unavailable
- `REASON` - Holiday/Maintenance/Training/etc.
- `APPLICABLE_BRANCHES` - Branches affected
- `APPLICABLE_ENGINEERS` - Engineers affected

### Advanced CFS Tables (9)

#### LOCATION_COORDINATE
Customer and service location coordinates for distance-based assignment.

**Key Fields:**
- `LOCATION_CODE` (UX) - Unique location identifier
- `LOCATION_NAME` - Human-readable name
- `LATITUDE/LONGITUDE` - GPS coordinates
- `SERVICE_AREA` - Geographic service area
- `CITY, STATE, PINCODE` - Address components
- `BRANCH` - Service branch reference

#### DISTANCE_MATRIX
Pre-calculated distances between locations (optimization cache).

**Key Fields:**
- `FROM_LOCATION_ID` - Starting location
- `TO_LOCATION_ID` - Destination location
- `DISTANCE_KM` - Distance in kilometers
- `TRAVEL_TIME_MINUTES` - Estimated travel time
- `ROUTE_TYPE` - SHORTEST/FASTEST/ECONOMIC

#### ENGINEER_CAPACITY
Daily capacity planning for each engineer.

**Key Fields:**
- `ENGINEER_ID` - Engineer identifier
- `CAPACITY_DATE` - Date for capacity (UX)
- `MAX_CALLS_PER_DAY` - Maximum service calls
- `CURRENT_CALLS` - Currently scheduled
- `AVAILABLE_CAPACITY` - Remaining slots
- `TOTAL_DRIVE_TIME_MINUTES` - Daily drive time budget
- `WORKLOAD_PERCENTAGE` - Current utilization

#### SERVICE_ROUTE
Optimized daily routes for engineers.

**Key Fields:**
- `ROUTE_ID` (UX) - Unique route identifier
- `ENGINEER_ID` - Assigned engineer
- `ROUTE_DATE` - Route date
- `TOTAL_STOPS` - Number of stops
- `TOTAL_DISTANCE_KM` - Route distance
- `ESTIMATED_TIME_HOURS` - Time to complete
- `OPTIMIZATION_SCORE` - Efficiency rating (0-100)
- `ROUTE_STATUS` - DRAFT/PUBLISHED/IN_PROGRESS/COMPLETED

#### ROUTE_STOP
Individual stops/appointments within a route.

**Key Fields:**
- `ROUTE_ID` - Parent route
- `SERNO_DELL` - Service order reference
- `STOP_SEQUENCE` - Order in route
- `LOCATION_ID` - Service location
- `SCHEDULED_ARRIVAL_TIME` - Expected arrival
- `ESTIMATED_SERVICE_DURATION_MINUTES` - Time needed
- `DISTANCE_FROM_PREVIOUS_KM` - Distance from previous
- `ACTUAL_ARRIVAL_TIME` - Real arrival time
- `STOP_STATUS` - PENDING/IN_PROGRESS/COMPLETED/SKIPPED

#### SCHEDULING_RULE
Rules engine for automatic service order assignment.

**Key Fields:**
- `RULE_CODE` (UX) - Code: PROXIMITY/EXPERTISE/AVAILABILITY/LOAD_BALANCE/TIME_WINDOW
- `RULE_NAME` - Human description
- `PRIORITY` - Execution order
- `MATCH_CRITERIA` - Assignment conditions (JSON)
- `ASSIGNMENT_LOGIC` - How to assign (JSON)
- `LOCATION_RADIUS_KM` - Max distance for matching
- `AUTO_ASSIGNMENTS_ENABLED` - Boolean flag

#### CFS_SCHEDULE_OPTIMIZATION
Main orchestration record for optimization execution.

**Key Fields:**
- `OPTIMIZATION_ID` (UX) - Unique execution identifier
- `SCHEDULE_DATE` - Date being optimized
- `BRANCH` - Branch being optimized
- `OPTIMIZATION_TYPE` - DAILY_SCHEDULE/ROUTE_OPTIMIZATION/REASSIGNMENT
- `TOTAL_SERVICE_ORDERS` - Input orders
- `ASSIGNED_ORDERS` - Successfully assigned
- `TOTAL_DISTANCE_KM` - Total across all routes
- `OPTIMIZATION_SCORE` - Overall efficiency (0-100)
- `OPTIMIZATION_ALGORITHM` - GENETIC/GREEDY/SIMULATED_ANNEALING
- `EXECUTION_TIME_SECONDS` - How long it took

#### ASSIGNMENT_LOG
Complete audit trail of all service order assignments.

**Key Fields:**
- `SERNO_DELL` - Service order
- `OPTIMIZATION_ID` - Which optimization run
- `ASSIGNED_ENGINEER_ID` - Assigned engineer
- `PREVIOUS_ENGINEER_ID` - Former assignment
- `RULE_APPLIED` - Which rule made assignment
- `CONFIDENCE_SCORE` - Reliability (0-100%)
- `ASSIGNMENT_TYPE` - AUTO/MANUAL_OVERRIDE/REASSIGNMENT
- `APPROVED_BY` - Manager approval
- `APPROVAL_TIMESTAMP` - Approval time

### Analytics Tables (3)

#### SCHEDULING_ANALYSIS
Performance analytics and insights from scheduling.

**Key Fields:**
- `ANALYSIS_ID` (UX) - Unique analysis identifier
- `ANALYSIS_DATE` - When generated
- `BRANCH` - Branch analyzed
- `METRIC_NAME` - AVG_ROUTE_DISTANCE/COMPLETION_RATE/ENGINEER_UTILIZATION/etc.
- `METRIC_VALUE` - Current value
- `TARGET_VALUE` - Goal value
- `VARIANCE_PERCENTAGE` - Deviation from target
- `ANALYSIS_TYPE` - DAILY/WEEKLY/MONTHLY/TREND
- `INSIGHTS` - Human-readable analysis
- `RECOMMENDATIONS` - Suggested improvements

#### SCHEDULING_PERFORMANCE
Daily performance metrics for each engineer.

**Key Fields:**
- `PERFORMANCE_ID` (UX) - Unique identifier
- `ENGINEER_ID` - Engineer being tracked
- `PERFORMANCE_DATE` - Date of performance
- `SCHEDULED_CALLS` - Calls assigned
- `COMPLETED_CALLS` - Actually completed
- `COMPLETION_RATE_PERCENTAGE` - Completed / Scheduled
- `PLANNED_DISTANCE_KM` - Original route distance
- `ACTUAL_DISTANCE_KM` - Actual travel distance
- `CUSTOMER_SATISFACTION_RATING` - Average 1-5 stars
- `SCHEDULING_EFFICIENCY_SCORE` - Composite efficiency

#### SCHEDULING_CONFLICT
Conflict detection and resolution tracking.

**Key Fields:**
- `CONFLICT_ID` (UX) - Unique conflict identifier
- `SERNO_DELL` - Affected service order
- `CONFLICT_TYPE` - DOUBLE_BOOKING/TIME_WINDOW_CONFLICT/SKILL_MISMATCH/CAPACITY_EXCEEDED/LOCATION_UNREACHABLE/BLACKOUT_CONFLICT
- `ENGINEER_ID` - Engineer involved
- `PRIORITY` - Severity: HIGH/MEDIUM/LOW
- `RESOLUTION_STATUS` - OPEN/IN_PROGRESS/RESOLVED
- `RECOMMENDED_ACTION` - Auto-suggested solution
- `RESOLVED_BY` - Manager who resolved

#### CALENDAR_MAINTENANCE
Holiday and maintenance calendar for system-wide adjustments.

**Key Fields:**
- `CALENDAR_DATE` - Date of event
- `EVENT_TYPE` - HOLIDAY/MAINTENANCE/PEAK_SEASON/LOW_SEASON
- `EVENT_NAME` - Human-readable name
- `APPLICABLE_BRANCHES` - Branches affected
- `SCHEDULING_DISABLED` - Boolean flag
- `RECURRING` - Does it repeat?
- `RECURRING_PATTERN` - DAILY/WEEKLY/MONTHLY/YEARLY

## Common Queries

### Basic Scheduling Queries

```sql
-- Get available slots for date
SELECT * FROM SCHEDULE_SLOT 
WHERE SLOT_DATE = @Date 
AND STATUS = 'Active'
AND AVAILABLE_SLOTS > 0
ORDER BY START_TIME;

-- Get engineer schedule for week
SELECT * FROM ENGINEER_SCHEDULE
WHERE ENGINEER_ID = @EngineerId
AND SCHEDULE_DATE BETWEEN @StartDate AND @EndDate
ORDER BY SCHEDULE_DATE;

-- Get appointments for engineer
SELECT a.* FROM SERVICE_APPOINTMENT a
WHERE a.ENGINEER_ID = @EngineerId
AND a.APPOINTMENT_DATE BETWEEN @StartDate AND @EndDate
ORDER BY a.APPOINTMENT_DATE;

-- Check if date is blackout
SELECT * FROM BLACKOUT_DATE
WHERE BLACKOUT_DATE = @Date
AND ISVALID = 1;
```

### CFS Advanced Queries

```sql
-- Find available engineers for location
SELECT TOP 5 ec.ENGINEER_ID, dm.DISTANCE_KM, ec.AVAILABLE_CAPACITY
FROM ENGINEER_CAPACITY ec
JOIN DISTANCE_MATRIX dm ON dm.FROM_LOCATION_ID = 1
WHERE ec.CAPACITY_DATE = CAST(GETDATE() AS DATE)
  AND ec.AVAILABLE_CAPACITY > 0
ORDER BY dm.DISTANCE_KM ASC;

-- View today's routes
SELECT ROUTE_ID, ENGINEER_ID, TOTAL_STOPS, TOTAL_DISTANCE_KM, OPTIMIZATION_SCORE
FROM SERVICE_ROUTE
WHERE ROUTE_DATE = CAST(GETDATE() AS DATE)
ORDER BY OPTIMIZATION_SCORE DESC;

-- Get route itinerary
SELECT rs.STOP_SEQUENCE, rs.SERNO_DELL, rs.SCHEDULED_ARRIVAL_TIME, 
       lc.LOCATION_NAME, rs.ESTIMATED_SERVICE_DURATION_MINUTES
FROM ROUTE_STOP rs
JOIN SERVICE_ROUTE sr ON rs.ROUTE_ID = sr.ID
JOIN LOCATION_COORDINATE lc ON rs.LOCATION_ID = lc.ID
WHERE sr.ENGINEER_ID = @EngineerId AND sr.ROUTE_DATE = CAST(GETDATE() AS DATE)
ORDER BY rs.STOP_SEQUENCE;

-- Track assignment history
SELECT SERNO_DELL, ASSIGNED_ENGINEER_ID, RULE_APPLIED, 
       CONFIDENCE_SCORE, ASSIGNMENT_TIMESTAMP
FROM ASSIGNMENT_LOG
WHERE SERNO_DELL = @SerialNumber
ORDER BY ASSIGNMENT_TIMESTAMP DESC;

-- Open scheduling conflicts
SELECT CONFLICT_ID, SERNO_DELL, CONFLICT_TYPE, PRIORITY, RECOMMENDED_ACTION
FROM SCHEDULING_CONFLICT
WHERE RESOLUTION_STATUS IN ('OPEN', 'IN_PROGRESS')
ORDER BY PRIORITY DESC;

-- Engineer performance today
SELECT ENGINEER_ID, COMPLETED_CALLS, SCHEDULED_CALLS, 
       COMPLETION_RATE_PERCENTAGE, CUSTOMER_SATISFACTION_RATING
FROM SCHEDULING_PERFORMANCE
WHERE PERFORMANCE_DATE = CAST(GETDATE() AS DATE)
ORDER BY COMPLETION_RATE_PERCENTAGE DESC;

-- Today's optimization results
SELECT OPTIMIZATION_ID, ASSIGNED_ORDERS, UNASSIGNED_ORDERS, 
       TOTAL_DISTANCE_KM, OPTIMIZATION_SCORE
FROM CFS_SCHEDULE_OPTIMIZATION
WHERE SCHEDULE_DATE = CAST(GETDATE() AS DATE)
AND OPTIMIZATION_STATUS = 'SUCCESS';
```

## API Patterns

### Basic Scheduling Endpoints
```
GET    /scheduling/slots?date={date}&branch={branch}           - Available slots
POST   /scheduling/appointments                                 - Book appointment
GET    /scheduling/appointments/{appointmentId}                - Get appointment
PUT    /scheduling/appointments/{appointmentId}                - Reschedule
GET    /scheduling/engineer/{engineerId}/schedule              - Engineer calendar
GET    /scheduling/engineer/{engineerId}/workload              - Engineer assignments
POST   /scheduling/engineer/{engineerId}/schedule              - Set unavailable
GET    /scheduling/holidays                                    - Get blackout dates
```

### CFS Advanced Endpoints
```
POST   /scheduling/cfs/optimize                                - Run daily optimization
GET    /scheduling/cfs/routes?date={date}                      - Get routes for date
GET    /scheduling/cfs/routes/{routeId}                        - Get route details
GET    /scheduling/cfs/routes/{routeId}/stops                  - Get route itinerary
PUT    /scheduling/cfs/routes/{routeId}/stop/{stopId}          - Update stop status
GET    /scheduling/cfs/assignments?serial={serno}              - Assignment history
GET    /scheduling/cfs/conflicts                               - Get open conflicts
PUT    /scheduling/cfs/conflicts/{conflictId}/resolve          - Resolve conflict
GET    /scheduling/cfs/performance?engineer={engineerId}       - Engineer performance
GET    /scheduling/cfs/analysis?branch={branch}&period={period} - Analytics
POST   /scheduling/cfs/rules                                   - Create assignment rule
GET    /scheduling/cfs/locations                               - Location list
POST   /scheduling/cfs/locations                               - Add location
GET    /scheduling/cfs/distance-matrix/calculate               - Refresh distances
```

## Integration Points

### ServiceOrders Service
- **Link**: `SERNO_DELL` in ROUTE_STOP references SERVICE_ORDER_HDR
- **Sync**: Appointment details bidirectional update
- **Reference**: Service order status impacts scheduling

### Auth Service
- **Link**: `ENGINEER_ID` validates against LOGIN_MASTER
- **Validation**: Engineer must be active with USER_TYPE='ENGINEER'
- **Access**: Route visibility based on engineer's branch

### MasterData Service
- **Link**: `BRANCH` in routes and capacity references BRANCH_MASTER
- **Reference**: SLA_MASTER provides time window constraints
- **Skills**: COMMODITY_MASTER maps to engineer certifications

### Communication Service
- **Route Published**: Send notification to engineer via MESSAGE_CORNER
- **Appointment Confirmed**: Send confirmation SMS to customer
- **Conflict**: Alert manager for manual intervention
- **Performance**: Push weekly performance rankings/coaching

## Business Rules

1. **Appointment Constraints**
   - Appointment date must be after current date
   - Must not conflict with engineer's BLACKOUT_DATE
   - Cannot exceed ENGINEER_CAPACITY.AVAILABLE_CAPACITY
   - Must not exceed ENGINEER_CAPACITY.AVAILABLE_DRIVE_TIME

2. **Route Optimization**
   - All assigned orders must be in ROUTE_STOP for the route
   - TOTAL_DISTANCE_KM calculated from DISTANCE_MATRIX sum
   - OPTIMIZATION_SCORE reflects efficiency (0-100)
   - Routes cannot exceed engineer's daily capacity

3. **Scheduling Conflicts**
   - Detected automatically during route generation
   - Must be resolved before route publication
   - DOUBLE_BOOKING: Two stops with overlapping times
   - LOCATION_UNREACHABLE: Drive time > available time between stops

4. **Assignment Logic**
   - SCHEDULING_RULE applied in priority order
   - First matching rule creates assignment
   - CONFIDENCE_SCORE indicates reliability
   - ASSIGNMENT_LOG provides complete audit trail

5. **Performance Tracking**
   - SCHEDULED_CALLS vs COMPLETED_CALLS drives COMPLETION_RATE_PERCENTAGE
   - CUSTOMER_SATISFACTION_RATING feeds recommendations
   - SCHEDULING_EFFICIENCY_SCORE = composite of multiple KPIs

## Documentation Files

- **[CFS_ADVANCED_GUIDE.md](CFS_ADVANCED_GUIDE.md)** - Complete CFS system specification with all tables and workflows
- **[CFS_IMPLEMENTATION_GUIDE.md](CFS_IMPLEMENTATION_GUIDE.md)** - Implementation patterns, SQL examples, and algorithms
- **[RC_FORMS_GUIDE.md](RC_FORMS_GUIDE.md)** - Service checkpoint forms (RC17/RC52/RC53/RC95/RC96) from ServiceOrders

## Key Features

✅ **Automatic Assignment** - Rules-based engine for autonomous allocation  
✅ **Multi-dimensional Optimization** - Location, distance, date, and capacity aware  
✅ **Route Optimization** - Daily route generation with efficiency scoring  
✅ **Real-time Tracking** - Live engineer location and status updates  
✅ **Conflict Detection** - Automatic detection of scheduling conflicts  
✅ **Performance Analytics** - Detailed metrics and insights  
✅ **Calendar Management** - Holiday and maintenance event handling  
✅ **Audit Trail** - Complete assignment and action history  

## Performance Optimization Tips

1. **Batch Operations** - Load all ENGINEER_CAPACITY for date at once
2. **Cache Data** - Keep LOCATION_COORDINATE and DISTANCE_MATRIX in-memory
3. **Index Usage** - Use indexed columns (date, engineer_id, location_id)
4. **Rule Sequencing** - Apply rules in priority; skip when possible
5. **Parallel Processing** - Generate multiple routes simultaneously
6. **Data Archival** - Archive completed routes monthly

## Future Enhancements

- [x] Automated location-wise scheduling
- [x] Distance-wise optimization  
- [x] Advanced CFS with calendar maintenance
- [ ] ML-based assignment prediction
- [ ] Real-time traffic integration
- [ ] Engineer preference learning
- [ ] Multi-day route planning
- [ ] SLA compliance automation
- [ ] Mobile app integration
- [ ] Calendar sync (Google/Outlook)

