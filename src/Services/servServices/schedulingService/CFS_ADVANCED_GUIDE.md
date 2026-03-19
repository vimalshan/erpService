# CFS (Customer Field Scheduling) - Advanced Scheduling System

## Overview

The CFS (Customer Field Scheduling) system provides intelligent, automated service order assignment with multi-dimensional optimization:
- **Date-wise**: Intelligent scheduling considering appointment availability
- **Location-wise**: Proximity-based assignment with distance calculations
- **Distance-wise**: Route optimization minimizing travel time and fuel
- **Automatic Assignment**: Rules-based engine for autonomous allocation
- **Advanced Analytics**: Performance tracking and optimization insights
- **Calendar Management**: Holiday/maintenance event handling

---

## Core Tables

### 1. LOCATION_COORDINATE
Stores all customer and service location coordinates with geospatial data.

**Key Fields:**
- `LOCATION_CODE` (PK) - Unique location identifier
- `LOCATION_NAME` - Human-readable location name
- `LATITUDE/LONGITUDE` - GPS coordinates for distance calculations
- `SERVICE_AREA` - Geographic service area classification
- `CITY, STATE, PINCODE` - Address components for location filtering
- `BRANCH` - Associated service branch

**Use Cases:**
- Store customer locations for service orders
- Store branch/depot locations as reference points
- Store engineer home locations for distance optimization

**Example:**
```sql
INSERT INTO LOCATION_COORDINATE 
(LOCATION_CODE, LOCATION_NAME, LATITUDE, LONGITUDE, CITY, BRANCH)
VALUES ('LOC001', 'ABC Corp Main Office', 28.6139, 77.2090, 'Delhi', 'BRANCH001')
```

---

### 2. DISTANCE_MATRIX
Pre-calculated distances between all location pairs (optimization cache).

**Key Fields:**
- `FROM_LOCATION_ID` - Starting location
- `TO_LOCATION_ID` - Destination location
- `DISTANCE_KM` - Direct distance in kilometers
- `TRAVEL_TIME_MINUTES` - Estimated travel time
- `ROUTE_TYPE` - Type: 'SHORTEST', 'FASTEST', 'ECONOMIC'

**Benefits:**
- Avoids real-time distance API calls
- Speeds up route optimization algorithms
- Pre-calculated for offline scenarios

**Update Strategy:**
- Calculate nightly for all location pairs
- Refresh weekly or on-demand
- Use Google Maps API or similar for calculation

**Example Query - Find nearby locations:**
```sql
SELECT TOP 5 
  lc.LOCATION_NAME, 
  dm.DISTANCE_KM,
  dm.TRAVEL_TIME_MINUTES
FROM DISTANCE_MATRIX dm
JOIN LOCATION_COORDINATE lc ON dm.TO_LOCATION_ID = lc.ID
WHERE dm.FROM_LOCATION_ID = @LocationID
ORDER BY dm.DISTANCE_KM ASC
```

---

### 3. ENGINEER_CAPACITY
Daily capacity tracking for each engineer.

**Key Fields:**
- `ENGINEER_ID` - Engineer identifier
- `CAPACITY_DATE` - Date for capacity
- `MAX_CALLS_PER_DAY` - Maximum service calls allowed
- `CURRENT_CALLS` - Calls currently scheduled
- `AVAILABLE_CAPACITY` - Remaining slots (Max - Current)
- `TOTAL_DRIVE_TIME_MINUTES` - Total daily drive time budget
- `AVAILABLE_DRIVE_TIME` - Remaining drive time
- `WORKLOAD_PERCENTAGE` - Current utilization %

**Dynamic Updates:**
- Decremented when service orders assigned
- Incremented when orders cancelled
- Calculated during route optimization

**Example:**
```sql
-- Check afternoon availability
SELECT ENGINEER_ID, AVAILABLE_CAPACITY, AVAILABLE_DRIVE_TIME
FROM ENGINEER_CAPACITY
WHERE CAPACITY_DATE = CAST(GETDATE() AS DATE)
  AND AVAILABLE_CAPACITY > 0
  AND AVAILABLE_DRIVE_TIME > 120  -- At least 2 hours
```

---

### 4. SERVICE_ROUTE
Optimized daily routes for engineers.

**Key Fields:**
- `ROUTE_ID` - Unique route identifier
- `ENGINEER_ID` - Assigned engineer
- `ROUTE_DATE` - Date of route
- `TOTAL_STOPS` - Number of service locations
- `TOTAL_DISTANCE_KM` - Total route distance
- `ESTIMATED_TIME_HOURS` - Total time to complete
- `OPTIMIZATION_SCORE` - Efficiency rating (0-100)
- `IS_OPTIMIZED` - Flag if route undergone optimization

**Route Status Values:**
- `DRAFT` - Created but not finalized
- `PUBLISHED` - Approved and distributed to engineer
- `IN_PROGRESS` - Engineer executing route
- `COMPLETED` - All stops visited
- `MODIFIED` - Route changed after publication

**Example - Generate daily routes:**
```sql
INSERT INTO SERVICE_ROUTE (ROUTE_ID, ENGINEER_ID, ROUTE_DATE, TOTAL_STOPS, IS_OPTIMIZED)
VALUES (CONCAT('RT', CONVERT(VARCHAR, GETDATE(), 112), '-ENG01'), 
        'ENG001', 
        CAST(GETDATE() AS DATE), 
        5, 
        1)
```

---

### 5. ROUTE_STOP
Individual stops within a service route.

**Key Fields:**
- `ROUTE_ID` - Parent route reference
- `SERNO_DELL` - Service order reference
- `STOP_SEQUENCE` - Order in route (1, 2, 3...)
- `LOCATION_ID` - Service location
- `SCHEDULED_ARRIVAL_TIME` - Expected arrival at stop
- `ESTIMATED_SERVICE_DURATION_MINUTES` - Time needed for service
- `DISTANCE_FROM_PREVIOUS_KM` - Distance from previous stop
- `DRIVE_TIME_FROM_PREVIOUS_MINUTES` - Travel time from previous stop
- `PRIORITY` - Stop importance (HIGH/MEDIUM/LOW)
- `ACTUAL_ARRIVAL_TIME` - Real arrival (when visited)
- `ACTUAL_DEPARTURE_TIME` - Real departure (when left)

**Status Values:**
- `PENDING` - Awaiting execution
- `IN_PROGRESS` - Engineer at location
- `COMPLETED` - Service done
- `SKIPPED` - Not completed today (reschedule)
- `ERROR` - Issue encountered

**Example - View day's itinerary for engineer:**
```sql
SELECT STOP_SEQUENCE, 
       SERNO_DELL,
       SCHEDULED_ARRIVAL_TIME,
       ESTIMATED_SERVICE_DURATION_MINUTES,
       lc.LOCATION_NAME
FROM ROUTE_STOP rs
JOIN SERVICE_ROUTE sr ON rs.ROUTE_ID = sr.ID
JOIN LOCATION_COORDINATE lc ON rs.LOCATION_ID = lc.ID
WHERE sr.ENGINEER_ID = 'ENG001' 
  AND sr.ROUTE_DATE = CAST(GETDATE() AS DATE)
ORDER BY rs.STOP_SEQUENCE
```

---

### 6. SCHEDULING_RULE
Rules engine for automatic service order assignment.

**Key Fields:**
- `RULE_NAME` - Human-readable name: "Proximity Match", "Expertise Match", etc.
- `RULE_CODE` - Code: 'PROXIMITY', 'EXPERTISE', 'AVAILABILITY', 'LOAD_BALANCE'
- `PRIORITY` - Execution order (1=highest)
- `MATCH_CRITERIA` - Conditions (JSON or DSL format)
- `ASSIGNMENT_LOGIC` - How to assign (JSON or formula)
- `APPLICABLE_BRANCHES` - Branches where rule applies
- `LOCATION_RADIUS_KM` - Max distance for proximity match
- `AUTO_ASSIGNMENTS_ENABLED` - Can rule auto-assign?

**Built-in Rules:**

1. **PROXIMITY Rule** - Assign closest available engineer
   - Criteria: Location distance <= LOCATION_RADIUS_KM
   - Logic: Select engineer with minimum distance
   - Best for: Urban areas with multiple engineers

2. **EXPERTISE Rule** - Match engineer skills to required skills
   - Criteria: Engineer has required certifications
   - Logic: Prefer engineers with highest skill match percentage
   - Best for: Complex equipment requiring specialists

3. **AVAILABILITY Rule** - Assign based on capacity
   - Criteria: Engineer has available slots and drive time
   - Logic: Select engineer with most availability
   - Best for: High-volume scheduling days

4. **LOAD_BALANCE Rule** - Distribute workload evenly
   - Criteria: Engineer workload < target percentage
   - Logic: Assign to least-loaded engineer
   - Best for: Fairness and burn-out prevention

5. **TIME_WINDOW Rule** - Match customer time preferences
   - Criteria: Engineer available during customer window
   - Logic: Prefer exact time match, cascade to acceptable windows
   - Best for: Urgent or time-critical calls

**Example Configuration:**
```sql
INSERT INTO SCHEDULING_RULE
(RULE_NAME, RULE_CODE, PRIORITY, MATCH_CRITERIA, APPLICABLE_BRANCHES)
VALUES (
  'Nearest Available Engineer',
  'PROXIMITY',
  1,
  '{"distance_km": 30, "min_capacity": 1}',
  'BRANCH001,BRANCH002'
)
```

---

### 7. CFS_SCHEDULE_OPTIMIZATION
Main orchestration record for optimization execution.

**Key Fields:**
- `OPTIMIZATION_ID` - Unique execution identifier
- `SCHEDULE_DATE` - Date being optimized
- `BRANCH` - Branch being optimized
- `OPTIMIZATION_TYPE` - Type: 'DAILY_SCHEDULE', 'ROUTE_OPTIMIZATION', 'REASSIGNMENT'
- `TOTAL_SERVICE_ORDERS` - Orders input to optimization
- `ASSIGNED_ORDERS` - Successfully assigned
- `UNASSIGNED_ORDERS` - Failed to assign
- `TOTAL_ENGINEERS` - Engineers available
- `ROUTES_GENERATED` - Routes created
- `TOTAL_DISTANCE_KM` - Total distance across all routes
- `OPTIMIZATION_SCORE` - Overall efficiency (0-100)
- `EFFICIENCY_PERCENTAGE` - vs. baseline
- `OPTIMIZATION_ALGORITHM` - Algorithm used ('GENETIC', 'GREEDY', 'SIMULATED_ANNEALING', etc.)
- `EXECUTION_TIME_SECONDS` - How long optimization took

**Example - View yesterday's optimization results:**
```sql
SELECT OPTIMIZATION_ID, 
       ASSIGNED_ORDERS, 
       UNASSIGNED_ORDERS,
       OPTIMIZATION_SCORE,
       TOTAL_DISTANCE_KM,
       EXECUTION_TIME_SECONDS
FROM CFS_SCHEDULE_OPTIMIZATION
WHERE SCHEDULE_DATE = CAST(GETDATE() - 1 AS DATE)
  AND OPTIMIZATION_STATUS = 'SUCCESS'
ORDER BY OPTIMIZATION_SCORE DESC
```

---

### 8. ASSIGNMENT_LOG
Complete audit trail of all service order assignments.

**Key Fields:**
- `SERNO_DELL` - Service order identifier
- `OPTIMIZATION_ID` - Which optimization run assigned it
- `PREVIOUS_ENGINEER_ID` - Former assignment (if reassigned)
- `ASSIGNED_ENGINEER_ID` - Current assignment
- `ASSIGNMENT_REASON` - Human description of why assigned
- `MATCHING_CRITERIA` - Technical criteria met
- `RULE_APPLIED` - Which rule made assignment
- `CONFIDENCE_SCORE` - Reliability 0-100%
- `ASSIGNMENT_TYPE` - 'AUTO', 'MANUAL_OVERRIDE', 'REASSIGNMENT'
- `AUTO_ASSIGNMENT` - Boolean flag
- `APPROVED_BY` - Manager who approved assignment
- `APPROVAL_TIMESTAMP` - Time of approval

**Example - Track order assignment history:**
```sql
SELECT 
  SERNO_DELL,
  ASSIGNED_ENGINEER_ID,
  ASSIGNMENT_TIMESTAMP,
  RULE_APPLIED,
  CONFIDENCE_SCORE,
  ASSIGNMENT_REASON
FROM ASSIGNMENT_LOG
WHERE SERNO_DELL = 'SVC-2024-00001'
ORDER BY ASSIGNMENT_TIMESTAMP DESC
```

---

### 9. SCHEDULING_ANALYSIS
Performance analytics and insights from scheduling operations.

**Key Fields:**
- `ANALYSIS_ID` - Unique analysis identifier
- `ANALYSIS_DATE` - When analysis was generated
- `BRANCH` - Branch analyzed
- `METRIC_NAME` - What is measured: 'AVG_ROUTE_DISTANCE', 'COMPLETION_RATE', 'ENGINEER_UTILIZATION', etc.
- `METRIC_VALUE` - Current value of metric
- `TARGET_VALUE` - Goal value
- `VARIANCE_PERCENTAGE` - How much off from target
- `ENGINEER_ID` - Specific engineer (if applicable)
- `ANALYSIS_TYPE` - 'DAILY', 'WEEKLY', 'MONTHLY', 'TREND'
- `INSIGHTS` - Human-readable analysis results
- `RECOMMENDATIONS` - Suggested improvements

**KPIs Tracked:**
- Average route distance (km)
- Route efficiency score (%)
- Engineer utilization rate (%)
- On-time completion rate (%)
- Customer satisfaction rating (1-5)
- Average service duration (minutes)
- Travel time ratio (actual/planned)
- Scheduling accuracy (%)

**Example - Daily analytics query:**
```sql
SELECT METRIC_NAME,
       METRIC_VALUE,
       TARGET_VALUE,
       VARIANCE_PERCENTAGE,
       INSIGHTS
FROM SCHEDULING_ANALYSIS
WHERE ANALYSIS_DATE >= CAST(GETDATE() - 7 AS DATE)
  AND ANALYSIS_TYPE = 'DAILY'
ORDER BY ANALYSIS_DATE, METRIC_NAME
```

---

### 10. CALENDAR_MAINTENANCE
Holiday and maintenance calendar for system-wide scheduling adjustments.

**Key Fields:**
- `CALENDAR_DATE` - Date of event
- `EVENT_TYPE` - 'HOLIDAY', 'MAINTENANCE', 'PEAK_SEASON', 'LOW_SEASON'
- `EVENT_NAME` - "Diwali Holiday", "System Maintenance Window", etc.
- `DESCRIPTION` - Detailed information
- `APPLICABLE_BRANCHES` - Branches affected
- `APPLICABLE_ENGINEERS` - Specific engineers affected (if null = all)
- `SCHEDULING_DISABLED` - Boolean: can assignments be made?
- `START_TIME / END_TIME` - Time window if partial day
- `RECURRING` - Does it repeat?
- `RECURRING_PATTERN` - 'DAILY', 'WEEKLY', 'MONTHLY', 'YEARLY'

**Use Cases:**
- Block entire days (holidays)
- Mark peak periods for surge planning
- Schedule system downtime (no assignments)
- Exclude specific engineers for personal reasons
- Mark seasonal variations

**Example:**
```sql
INSERT INTO CALENDAR_MAINTENANCE
(CALENDAR_DATE, EVENT_TYPE, EVENT_NAME, SCHEDULING_DISABLED, RECURRING, RECURRING_PATTERN)
VALUES (
  '2024-03-25',
  'HOLIDAY',
  'Holi Festival',
  1,
  1,
  'YEARLY'
)
```

---

### 11. SCHEDULING_PERFORMANCE
Daily performance metrics for each engineer.

**Key Fields:**
- `ENGINEER_ID` - Engineer being tracked
- `PERFORMANCE_DATE` - Date of performance
- `SCHEDULED_CALLS` - Calls assigned for day
- `COMPLETED_CALLS` - Actually completed
- `COMPLETION_RATE_PERCENTAGE` - Completed / Scheduled
- `PLANNED_DISTANCE_KM` - Original route distance
- `ACTUAL_DISTANCE_KM` - Actual travel distance
- `DISTANCE_VARIANCE_PERCENTAGE` - Actual vs planned
- `PLANNED_TIME_HOURS` - Estimated total time
- `ACTUAL_TIME_HOURS` - Real time taken
- `TIME_VARIANCE_PERCENTAGE` - Actual vs planned
- `CANCELLED_CALLS` - Calls cancelled
- `RESCHEDULED_CALLS` - Calls moved to later date
- `CUSTOMER_SATISFACTION_RATING` - Average 1-5 star rating
- `SCHEDULING_EFFICIENCY_SCORE` - Composite efficiency
- `ROUTE_USAGE_SCORE` - How well engineer followed route

**Example - Identify top performers:**
```sql
SELECT TOP 10
  ENGINEER_ID,
  AVG(COMPLETION_RATE_PERCENTAGE) AS AVG_COMPLETION,
  AVG(CUSTOMER_SATISFACTION_RATING) AS AVG_RATING,
  AVG(SCHEDULING_EFFICIENCY_SCORE) AS AVG_EFFICIENCY
FROM SCHEDULING_PERFORMANCE
WHERE PERFORMANCE_DATE >= CAST(GETDATE() - 30 AS DATE)
GROUP BY ENGINEER_ID
ORDER BY AVG_EFFICIENCY DESC
```

---

### 12. SCHEDULING_CONFLICT
Conflict detection and resolution tracking.

**Key Fields:**
- `CONFLICT_ID` - Unique conflict identifier
- `SERNO_DELL` - Affected service order
- `CONFLICT_TYPE` - Type of conflict
- `ENGINEER_ID` - Engineer involved
- `CONFLICTING_APPOINTMENT_ID` - Conflicting appointment
- `PRIORITY` - Severity: HIGH, MEDIUM, LOW
- `RESOLUTION_STATUS` - 'OPEN', 'IN_PROGRESS', 'RESOLVED'
- `RECOMMENDED_ACTION` - Auto-recommended solution
- `RESOLVED_BY` - Manager who resolved it
- `RESOLUTION_NOTES` - How it was resolved

**Conflict Types:**
- `DOUBLE_BOOKING` - Engineer assigned to 2+ locations at same time
- `TIME_WINDOW_CONFLICT` - Can't meet customer time window
- `SKILL_MISMATCH` - Engineer lacks required skills
- `CAPACITY_EXCEEDED` - Engineer over-capacity
- `LOCATION_UNREACHABLE` - Location too far from previous
- `BLACKOUT_CONFLICT` - Assigned on engineer's off day

**Example - Monitor open conflicts:**
```sql
SELECT CONFLICT_ID, SERNO_DELL, CONFLICT_TYPE, PRIORITY
FROM SCHEDULING_CONFLICT
WHERE RESOLUTION_STATUS IN ('OPEN', 'IN_PROGRESS')
ORDER BY PRIORITY DESC, ENTERED_ON
```

---

## Workflow: Automatic Service Order Assignment

### Step 1: Trigger Optimization
```
CFS system initialized for SCHEDULE_DATE = TODAY
- Input: All open service orders for the date
- Available engineers from ENGINEER_SCHEDULE
- Resources: SCHEDULING_RULE set for branch
```

### Step 2: Apply Rules Engine
```
For each SCHEDULING_RULE (ordered by PRIORITY):
  1. Evaluate MATCH_CRITERIA against service order and engineers
  2. Apply ASSIGNMENT_LOGIC
  3. Generate candidate assignments (Engineer, Confidence Score)
  4. Log to ASSIGNMENT_LOG

Example:
  Rule 1 (PROXIMITY): Engineer within 30km of service location
  Rule 2 (AVAILABILITY): Engineer has available slots
  Rule 3 (LOAD_BALANCE): Assign to least-loaded available engineer
```

### Step 3: Generate Routes
```
Grouped assignments by engineer:
  For each engineer with assigned orders:
    1. Create SERVICE_ROUTE record
    2. Query DISTANCE_MATRIX for distances
    3. Sequence ROUTE_STOP records by optimization algorithm
    4. Calculate total distance, time, efficiency score
    5. Store in CFS_SCHEDULE_OPTIMIZATION
```

### Step 4: Validate & Conflict Check
```
For each route:
  1. Check for SCHEDULING_CONFLICT conditions
  2. Validate against CALENDAR_MAINTENANCE blackouts
  3. Check ENGINEER_CAPACITY not exceeded
  4. Reconcile unavoidable conflicts with manager
```

### Step 5: Publish & Notify
```
1. Mark routes as PUBLISHED
2. Send to engineer mobile app
3. Notify customer of appointment details
4. Create calendar entry with location/time details
```

### Step 6: Execute & Track
```
During execution:
  1. Engineer visits ROUTE_STOP locations
  2. Enter ACTUAL_ARRIVAL_TIME, ACTUAL_DEPARTURE_TIME
  3. Complete service work in SERVICE_ORDER
  4. Update ROUTE_STOP status
```

### Step 7: Analyze & Optimize
```
End of day:
  1. Calculate SCHEDULING_PERFORMANCE metrics
  2. Generate SCHEDULING_ANALYSIS insights
  3. Identify optimization opportunities
  4. Feed learnings back to SCHEDULING_RULE
```

---

## Key Queries for Operations

### Find available engineers for next 2 hours
```sql
SELECT ec.ENGINEER_ID, 
       ec.AVAILABLE_CAPACITY,
       ec.AVAILABLE_DRIVE_TIME,
       ec.WORKLOAD_PERCENTAGE
FROM ENGINEER_CAPACITY ec
WHERE ec.CAPACITY_DATE = CAST(GETDATE() AS DATE)
  AND ec.AVAILABLE_CAPACITY > 0
  AND ec.AVAILABLE_DRIVE_TIME > 120
  AND ec.WORKLOAD_PERCENTAGE < 80
ORDER BY ec.WORKLOAD_PERCENTAGE ASC
```

### Find nearest engineers for location
```sql
DECLARE @LocationID BIGINT = 1
DECLARE @MaxDistance DECIMAL(8,2) = 30

SELECT TOP 5
  e.ENGINEER_ID,
  dm.DISTANCE_KM,
  dm.TRAVEL_TIME_MINUTES,
  ec.AVAILABLE_CAPACITY
FROM DISTANCE_MATRIX dm
JOIN ENGINEER_SCHEDULE es ON es.ENGINEER_ID = @EngineerID
JOIN ENGINEER_CAPACITY ec ON ec.ENGINEER_ID = es.ENGINEER_ID 
JOIN (SELECT ENGINEER_ID FROM ENGINEER_SCHEDULE 
      WHERE SCHEDULE_DATE = CAST(GETDATE() AS DATE)) e ON e.ENGINEER_ID = @EngineerID
WHERE dm.FROM_LOCATION_ID = @LocationID
  AND dm.DISTANCE_KM <= @MaxDistance
  AND ec.AVAILABLE_CAPACITY > 0
ORDER BY dm.DISTANCE_KM ASC
```

### Optimization effectiveness report
```sql
SELECT 
  CONVERT(DATE, SCHEDULE_DATE) AS OptDate,
  BRANCH,
  AVG(OPTIMIZATION_SCORE) AS AvgScore,
  AVG(EFFICIENCY_PERCENTAGE) AS AvgEfficiency,
  SUM(ROUTES_GENERATED) AS TotalRoutes,
  AVG(TOTAL_DISTANCE_KM) AS AvgDistance
FROM CFS_SCHEDULE_OPTIMIZATION
WHERE SCHEDULE_DATE >= CAST(GETDATE() - 30 AS DATE)
GROUP BY CONVERT(DATE, SCHEDULE_DATE), BRANCH
ORDER BY OptDate DESC
```

### Engineer productivity ranking
```sql
SELECT TOP 20
  ENGINEER_ID,
  AVG(COMPLETION_RATE_PERCENTAGE) AS AvgCompletion,
  AVG(CUSTOMER_SATISFACTION_RATING) AS AvgRating,
  AVG(SCHEDULING_EFFICIENCY_SCORE) AS AvgEfficiency,
  COUNT(*) AS DaysTracked,
  AVG(SCHEDULED_CALLS) AS AvgCalls
FROM SCHEDULING_PERFORMANCE
WHERE PERFORMANCE_DATE >= CAST(GETDATE() - 30 AS DATE)
GROUP BY ENGINEER_ID
HAVING COUNT(*) >= 10
ORDER BY AvgEfficiency DESC
```

### Unresolved scheduling conflicts
```sql
SELECT 
  CONFLICT_ID,
  SERNO_DELL,
  CONFLICT_TYPE,
  ENGINEER_ID,
  PRIORITY,
  RECOMMENDED_ACTION,
  DATEDIFF(HOUR, ENTERED_ON, GETDATE()) AS HoursOpen
FROM SCHEDULING_CONFLICT
WHERE RESOLUTION_STATUS IN ('OPEN', 'IN_PROGRESS')
ORDER BY PRIORITY DESC, ENTERED_ON
```

---

## Integration Points

### With ServiceOrders Service
- Reference: `SERNO_DELL` in ROUTE_STOP and ASSIGNMENT_LOG
- Bidirectional update of appointment details
- Sync completion status back to SERVICE_ORDER

### With Auth Service
- Reference: `ENGINEER_ID` keys to LOGIN_MASTER
- Engineer access control for route visibility
- Activity audit trail

### With MasterData Service
- Reference: `BRANCH` to BRANCH_MASTER
- Update SLA_MASTER for time window requirements
- Reference skill requirements

### With Communication Service
- Trigger notifications to engineers on route publication
- Customer appointment confirmations
- Conflict resolution alerts

---

## Performance Optimization Tips

1. **Batch Load Capacity** - Load all ENGINEER_CAPACITY for a date at once
2. **Cache Coordinates** - Keep LOCATION_COORDINATE and DISTANCE_MATRIX in-memory
3. **Indexed Queries** - Use indexed columns for date/engineer/location filters
4. **Incremental Rules** - Apply rules in order, stop early when possible
5. **Parallel Routes** - Generate routes for multiple engineers simultaneously
6. **Archive Old Data** - Move completed routes to archive monthly

---

## Advanced Features

### Machine Learning Integration
- Train model on SCHEDULING_PERFORMANCE to predict optimal assignments
- Use SCHEDULING_ANALYSIS metrics as features
- Continuously improve CONFIDENCE_SCORE in ASSIGNMENT_LOG

### Real-time Adjustments
- Monitor ROUTE_STOP actual times vs. planned
- Adjust cascading stops if engineer falling behind
- Auto-trigger conflict resolution for delays

### Demand Forecasting
- Analyze historical SCHEDULING_ANALYSIS
- Predict busy days
- Pre-calculate capacity for upcoming weeks

### Cost Optimization
- Minimize TOTAL_DISTANCE_KM for fuel savings
- Balance engineer utilization to avoid overtime costs
- Factor in premium time windows (same-day service premium)

---

## Maintenance & Administration

### Daily Tasks
1. Execute CFS optimization for next day (CRON job)
2. Publish routes to engineer apps
3. Monitor real-time ROUTE_STOP status
4. Resolve open SCHEDULING_CONFLICT records

### Weekly Tasks
1. Refresh DISTANCE_MATRIX with updated data
2. Review SCHEDULING_ANALYSIS insights
3. Adjust SCHEDULING_RULE parameters if needed
4. Archive completed routes and performance data

### Monthly Tasks
1. Generate SCHEDULING_ANALYSIS trend reports
2. Update engineer capacity baselines (MAX_CALLS_PER_DAY, TOTAL_DRIVE_TIME_MINUTES)
3. Review and update CALENDAR_MAINTENANCE recurring events
4. Identify and promote top engineers (SCHEDULING_PERFORMANCE ranking)

---

## Testing Scenarios

### Test 1: Basic Proximity Assignment
```sql
-- Setup: 3 engineers, 3 service orders in different locations
-- Expected: Each order assigned to nearest engineer
-- Verify: ASSIGNMENT_LOG shows PROXIMITY rule applied, CONFIDENCE_SCORE > 80%
```

### Test 2: Capacity Overflow
```sql
-- Setup: 10 orders, engineers at max capacity
-- Expected: Last orders remain UNASSIGNED_ORDERS
-- Verify: CFS_SCHEDULE_OPTIMIZATION shows unassigned count, ASSIGNED_ORDERS < 10
```

### Test 3: Time Window Conflict
```sql
-- Setup: Order requires 2:00-2:30 PM, engineer at another location until 2:45 PM
-- Expected: SCHEDULING_CONFLICT created type='TIME_WINDOW_CONFLICT'
-- Verify: RECOMMENDED_ACTION suggests adjacent time window or different engineer
```

### Test 4: Skill Mismatch
```sql
-- Setup: Printer repair needs expertise, basic engineer assigned
-- Expected: SCHEDULING_CONFLICT created type='SKILL_MISMATCH'
-- Verify: RECOMMENDED_ACTION suggests reassignment to specialist
```

---

## Summary

The CFS Advanced Scheduling System automates complex, multi-dimensional service order assignment while maintaining visibility, auditability, and control. The rule-based engine allows continuous optimization while SCHEDULING_ANALYSIS provides actionable insights for continuous improvement.

