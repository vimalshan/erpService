# CFS Automation Implementation Guide

## Quick Start

### 1. Initialize System for a Day

```sql
-- 1.1 Create optimization execution record
DECLARE @OptimizationID VARCHAR(30) = CONCAT('OPT-', CONVERT(VARCHAR, GETDATE(), 112), '-001')
DECLARE @ScheduleDate DATETIME = CAST(GETDATE() AS DATE)

INSERT INTO CFS_SCHEDULE_OPTIMIZATION 
(OPTIMIZATION_ID, SCHEDULE_DATE, BRANCH, OPTIMIZATION_TYPE, OPTIMIZATION_STATUS, EXECUTION_TIMESTAMP)
VALUES (@OptimizationID, @ScheduleDate, 'BRANCH001', 'DAILY_SCHEDULE', 'IN_PROGRESS', GETDATE())

-- 1.2 Load available engineers' capacity for the day
INSERT INTO ENGINEER_CAPACITY (ENGINEER_ID, CAPACITY_DATE, MAX_CALLS_PER_DAY, CURRENT_CALLS, TOTAL_DRIVE_TIME_MINUTES)
SELECT DISTINCT LOGIN_ID, @ScheduleDate, 8, 0, 480  -- 8 hours = 480 minutes
FROM LOGIN_MASTER
WHERE USER_TYPE = 'ENGINEER' 
  AND BRANCH = 'BRANCH001'
  AND ISVALID = 1
  AND NOT EXISTS (
    SELECT 1 FROM ENGINEER_CAPACITY 
    WHERE ENGINEER_ID = LOGIN_MASTER.LOGIN_ID 
    AND CAPACITY_DATE = @ScheduleDate
  )

-- 1.3 Load pending service orders for assignment
SELECT COUNT(*) AS PendingOrders
FROM SERVICE_ORDER_HDR
WHERE CONVERT(DATE, APPOINTMENT_DATE) = @ScheduleDate
  AND APPOINTMENT_ENGINEER IS NULL
```

### 2. Apply Proximity Assignment Rule

```sql
-- Find closest engineers for each unassigned order
DECLARE @LocationID BIGINT
DECLARE @MaxDistance DECIMAL(8,2) = 30  -- 30km radius

INSERT INTO ASSIGNMENT_LOG 
(SERNO_DELL, OPTIMIZATION_ID, ASSIGNED_ENGINEER_ID, RULE_APPLIED, CONFIDENCE_SCORE, 
 ASSIGNMENT_REASON, ASSIGNMENT_TYPE, AUTO_ASSIGNMENT, ASSIGNMENT_TIMESTAMP)

SELECT TOP 1
  soh.SERNO_DELL,
  'OPT-20240314-001' AS OPTIMIZATION_ID,
  es.ENGINEER_ID,
  'PROXIMITY' AS RULE_APPLIED,
  CAST((100 - (dm.DISTANCE_KM / @MaxDistance * 100)) AS DECIMAL(5,2)) AS CONFIDENCE_SCORE,
  CONCAT('Assigned based on proximity (', CAST(dm.DISTANCE_KM AS VARCHAR(5)), ' km away)') AS ASSIGNMENT_REASON,
  'AUTO' AS ASSIGNMENT_TYPE,
  1 AS AUTO_ASSIGNMENT,
  GETDATE() AS ASSIGNMENT_TIMESTAMP
FROM SERVICE_ORDER_HDR soh
JOIN LOCATION_COORDINATE loc ON soh.SERNO_DELL = loc.LOCATION_CODE -- or use customer location lookup
JOIN DISTANCE_MATRIX dm ON dm.TO_LOCATION_ID = loc.ID
JOIN ENGINEER_SCHEDULE es ON es.ENGINEER_ID = (
  SELECT ENGINEER_ID FROM ENGINEER_CAPACITY 
  WHERE CAPACITY_DATE = CAST(GETDATE() AS DATE)
  AND AVAILABLE_CAPACITY > 0
  LIMIT 1
)
WHERE soh.APPOINTMENT_ENGINEER IS NULL
  AND soh.APPOINTMENT_DATE = GETDATE()
  AND dm.DISTANCE_KM <= @MaxDistance
ORDER BY dm.DISTANCE_KM ASC
```

### 3. Create Route from Assignments

```sql
-- Aggregate assigned orders per engineer and create route
DECLARE @EngineerID VARCHAR(15) = 'ENG001'
DECLARE @RouteDate DATETIME = CAST(GETDATE() AS DATE)

-- Create route header
DECLARE @RouteID VARCHAR(20) = CONCAT('RT-', CONVERT(VARCHAR, @RouteDate, 112), '-', @EngineerID)

INSERT INTO SERVICE_ROUTE 
(ROUTE_ID, ENGINEER_ID, ROUTE_DATE, ROUTE_STATUS)
VALUES (@RouteID, @EngineerID, @RouteDate, 'DRAFT')

-- Add stops for assigned orders
INSERT INTO ROUTE_STOP 
(ROUTE_ID, SERNO_DELL, LOCATION_ID, STOP_SEQUENCE, SCHEDULED_ARRIVAL_TIME, STOP_STATUS)

SELECT 
  sr.ID,
  al.SERNO_DELL,
  lc.ID,
  ROW_NUMBER() OVER (ORDER BY dm.DISTANCE_KM) AS STOP_SEQUENCE,
  DATEADD(HOUR, ROW_NUMBER() OVER (ORDER BY dm.DISTANCE_KM) * 1, @RouteDate) AS SCHEDULED_ARRIVAL_TIME,
  'PENDING'
FROM ASSIGNMENT_LOG al
JOIN SERVICE_ROUTE sr ON sr.ROUTE_ID = @RouteID
JOIN LOCATION_COORDINATE lc ON lc.LOCATION_CODE = al.SERNO_DELL -- adjust based on lookup logic
JOIN DISTANCE_MATRIX dm ON dm.TO_LOCATION_ID = lc.ID
WHERE al.ASSIGNED_ENGINEER_ID = @EngineerID
  AND al.OPTIMIZATION_ID = 'OPT-20240314-001'
```

### 4. Calculate Route Efficiency

```sql
-- Update route with calculated metrics
DECLARE @RouteID BIGINT = 1

UPDATE SERVICE_ROUTE
SET 
  TOTAL_STOPS = (SELECT COUNT(*) FROM ROUTE_STOP WHERE ROUTE_ID = @RouteID),
  TOTAL_DISTANCE_KM = (
    SELECT SUM(DISTANCE_FROM_PREVIOUS_KM) 
    FROM ROUTE_STOP 
    WHERE ROUTE_ID = @RouteID
  ),
  ESTIMATED_TIME_HOURS = (
    SELECT SUM(DRIVE_TIME_FROM_PREVIOUS_MINUTES) + 
           SUM(ESTIMATED_SERVICE_DURATION_MINUTES)
    FROM ROUTE_STOP 
    WHERE ROUTE_ID = @RouteID
  ) / 60.0
WHERE ID = @RouteID

-- Calculate optimization score (80-100 is excellent, <50 is poor)
UPDATE SERVICE_ROUTE
SET OPTIMIZATION_SCORE = CASE 
  WHEN TOTAL_DISTANCE_KM < 50 AND TOTAL_STOPS >= 5 THEN 95
  WHEN TOTAL_DISTANCE_KM < 80 AND TOTAL_STOPS >= 4 THEN 85
  WHEN TOTAL_DISTANCE_KM < 150 THEN 75
  ELSE 60
END
WHERE ID = @RouteID
```

### 5. Handle Conflicts

```sql
-- Detect scheduling conflicts
INSERT INTO SCHEDULING_CONFLICT
(CONFLICT_ID, SERNO_DELL, CONFLICT_TYPE, ENGINEER_ID, AFFECTED_DATE, PRIORITY, RESOLUTION_STATUS)

-- Double-booking check
SELECT 
  CONCAT('CONF-', CONVERT(VARCHAR, GETDATE(), 112), '-', ROW_NUMBER() OVER (ORDER BY rs.ID)),
  rs.SERNO_DELL,
  'DOUBLE_BOOKING',
  sr.ENGINEER_ID,
  sr.ROUTE_DATE,
  'HIGH',
  'OPEN'
FROM ROUTE_STOP rs1
JOIN ROUTE_STOP rs2 ON rs1.ROUTE_ID = rs2.ROUTE_ID 
  AND rs1.ID < rs2.ID
  AND (rs1.SCHEDULED_ARRIVAL_TIME < rs2.SCHEDULED_DEPARTURE_TIME)
  AND (rs2.SCHEDULED_ARRIVAL_TIME < rs1.SCHEDULED_DEPARTURE_TIME)
JOIN SERVICE_ROUTE sr ON sr.ID = rs1.ROUTE_ID
WHERE sr.ROUTE_STATUS IN ('DRAFT', 'PUBLISHED')
  AND NOT EXISTS (
    SELECT 1 FROM SCHEDULING_CONFLICT 
    WHERE SERNO_DELL = rs1.SERNO_DELL
  )

-- Location unreachable check (distance too far in available time)
INSERT INTO SCHEDULING_CONFLICT
SELECT 
  CONCAT('CONF-', CONVERT(VARCHAR, GETDATE(), 112), '-UNREACH-', rs.ID),
  rs.SERNO_DELL,
  'LOCATION_UNREACHABLE',
  sr.ENGINEER_ID,
  sr.ROUTE_DATE,
  'MEDIUM',
  'OPEN'
FROM ROUTE_STOP rs
JOIN SERVICE_ROUTE sr ON sr.ID = rs.ROUTE_ID
WHERE rs.DRIVE_TIME_FROM_PREVIOUS_MINUTES > 
      DATEDIFF(MINUTE, DATEADD(MINUTE, -rs.ESTIMATED_SERVICE_DURATION_MINUTES, rs.SCHEDULED_ARRIVAL_TIME),
               rs.SCHEDULED_ARRIVAL_TIME)
```

### 6. Publish Route to Engineer

```sql
-- Mark route complete and ready for engineer
UPDATE SERVICE_ROUTE
SET ROUTE_STATUS = 'PUBLISHED',
    CHANGED_ON = GETDATE(),
    CHANGED_BY = 'SYSTEM'
WHERE ID = @RouteID

-- Notify engineer (integration with Communication service)
INSERT INTO MESSAGE_CORNER 
(FROM_USER_ID, TO_USER_ID, SUBJECT, MESSAGE, CREATED_DATE, MSG_TYPE)
SELECT 
  'SYSTEM',
  ENGINEER_ID,
  CONCAT('Your route for ', CONVERT(DATE, ROUTE_DATE)),
  CONCAT('Daily route published with ', TOTAL_STOPS, ' stops, ', TOTAL_DISTANCE_KM, ' km'),
  GETDATE(),
  'ROUTE_ASSIGNMENT'
FROM SERVICE_ROUTE
WHERE ID = @RouteID
```

### 7. Track Real-time Execution

```sql
-- Engineer executes route - update actual times
DECLARE @RouteStopID BIGINT = 1

UPDATE ROUTE_STOP
SET 
  ACTUAL_ARRIVAL_TIME = GETDATE(),
  STOP_STATUS = 'IN_PROGRESS',
  CHANGED_ON = GETDATE()
WHERE ID = @RouteStopID

-- When service completes
UPDATE ROUTE_STOP
SET 
  ACTUAL_DEPARTURE_TIME = GETDATE(),
  STOP_STATUS = 'COMPLETED',
  CHANGED_ON = GETDATE()
WHERE ID = @RouteStopID

-- Update SERVICE_ORDER completion details
UPDATE SERVICE_ORDER_HDR
SET 
  APPOINTMENT_ENGINEER = sr.ENGINEER_ID,
  APPOINTMENT_DATE = sr.ROUTE_DATE,
  SERVICE_COMPLETED_DATE = GETDATE()
FROM SERVICE_ROUTE sr
JOIN ROUTE_STOP rs ON rs.ROUTE_ID = sr.ID
WHERE SERVICE_ORDER_HDR.SERNO_DELL = rs.SERNO_DELL
```

### 8. Analyze Performance

```sql
-- End-of-day performance calculation
DECLARE @EngineerID VARCHAR(15) = 'ENG001'
DECLARE @PerfDate DATETIME = CAST(GETDATE() AS DATE)

INSERT INTO SCHEDULING_PERFORMANCE
(PERFORMANCE_ID, ENGINEER_ID, PERFORMANCE_DATE, SCHEDULED_CALLS, COMPLETED_CALLS, 
 PLANNED_DISTANCE_KM, ACTUAL_DISTANCE_KM, COMPLETION_RATE_PERCENTAGE)

SELECT 
  CONCAT('PERF-', CONVERT(VARCHAR, @PerfDate, 112), '-', @EngineerID),
  @EngineerID,
  @PerfDate,
  (SELECT COUNT(*) FROM ROUTE_STOP 
   WHERE ROUTE_ID IN (SELECT ID FROM SERVICE_ROUTE WHERE ENGINEER_ID = @EngineerID AND ROUTE_DATE = @PerfDate)),
  (SELECT COUNT(*) FROM ROUTE_STOP 
   WHERE ROUTE_ID IN (SELECT ID FROM SERVICE_ROUTE WHERE ENGINEER_ID = @EngineerID AND ROUTE_DATE = @PerfDate)
   AND STOP_STATUS = 'COMPLETED'),
  (SELECT SUM(TOTAL_DISTANCE_KM) FROM SERVICE_ROUTE WHERE ENGINEER_ID = @EngineerID AND ROUTE_DATE = @PerfDate),
  (SELECT SUM(ACTUAL_DISTANCE_KM) FROM ROUTE_STOP 
   WHERE ROUTE_ID IN (SELECT ID FROM SERVICE_ROUTE WHERE ENGINEER_ID = @EngineerID AND ROUTE_DATE = @PerfDate)),
  CAST((SELECT COUNT(*) FROM ROUTE_STOP 
        WHERE ROUTE_ID IN (SELECT ID FROM SERVICE_ROUTE WHERE ENGINEER_ID = @EngineerID AND ROUTE_DATE = @PerfDate)
        AND STOP_STATUS = 'COMPLETED') * 100.0 / 
       (SELECT COUNT(*) FROM ROUTE_STOP 
        WHERE ROUTE_ID IN (SELECT ID FROM SERVICE_ROUTE WHERE ENGINEER_ID = @EngineerID AND ROUTE_DATE = @PerfDate)) AS DECIMAL(5,2))
```

---

## Data Flow Diagram

```
Unassigned Service Orders (SERVICE_ORDER_HDR) 
         ↓
    CFS Trigger (SCHEDULE_DATE)
         ↓
[1] Load Resources
    ├─ ENGINEER_SCHEDULE (who's available)
    ├─ ENGINEER_CAPACITY (how many slots)
    └─ SCHEDULING_RULE (assignment rules)
         ↓
[2] Apply Rules Engine
    ├─ PROXIMITY (nearest location)
    ├─ AVAILABILITY (capacity check)
    ├─ EXPERTISE (skill match)
    └─ LOAD_BALANCE (fair distribution)
         ↓
    ASSIGNMENT_LOG (candidate assignments)
         ↓
[3] Generate Routes
    ├─ GROUP by ENGINEER_ID
    ├─ ORDER by location optimization
    └─ CREATE SERVICE_ROUTE + ROUTE_STOPs
         ↓
[4] Conflict Detection
    ├─ SCHEDULING_CONFLICT (overlap, unreachable)
    └─ Manager review & approval
         ↓
[5] Publish to Engineers
    ├─ Update ROUTE_STATUS = 'PUBLISHED'
    └─ NOTIFICATION via MESSAGE_CORNER
         ↓
[6] Execution
    ├─ ROUTE_STOP.ACTUAL_ARRIVAL_TIME
    ├─ SERVICE work completes
    └─ ROUTE_STOP.ACTUAL_DEPARTURE_TIME
         ↓
[7] Analytics
    ├─ SCHEDULING_PERFORMANCE (metrics)
    ├─ SCHEDULING_ANALYSIS (insights)
    └─ Rules Optimization (improve next time)
         ↓
    SUCCESS: All orders served efficiently
```

---

## Advanced Assignment Algorithm

### Multi-Criteria Decision Making (MCDM) Approach

```sql
-- Rank engineers by combined score
WITH EngineerScores AS (
  SELECT 
    @EngineerID AS ENGINEER_ID,
    -- Proximity score (0-30 points): closer is better
    CASE 
      WHEN dm.DISTANCE_KM < 10 THEN 30
      WHEN dm.DISTANCE_KM < 20 THEN 25
      WHEN dm.DISTANCE_KM < 30 THEN 20
      ELSE 0
    END AS proximity_score,
    
    -- Availability score (0-25 points): more capacity is better
    CASE 
      WHEN ec.AVAILABLE_CAPACITY >= 5 THEN 25
      WHEN ec.AVAILABLE_CAPACITY >= 3 THEN 20
      WHEN ec.AVAILABLE_CAPACITY >= 1 THEN 10
      ELSE 0
    END AS availability_score,
    
    -- Workload balance score (0-25 points): lower utilization is better
    CASE 
      WHEN ec.WORKLOAD_PERCENTAGE < 50 THEN 25
      WHEN ec.WORKLOAD_PERCENTAGE < 70 THEN 15
      WHEN ec.WORKLOAD_PERCENTAGE < 85 THEN 5
      ELSE 0
    END AS balance_score,
    
    -- Skill match score (0-20 points): specialty match is better
    CASE 
      WHEN es.SPECIALTY LIKE CONCAT('%', @RequiredSkill, '%') THEN 20
      ELSE 10
    END AS skill_score,
    
    -- Performance history score (0-20 points): top performers preferred
    CASE 
      WHEN sp.COMPLETION_RATE_PERCENTAGE > 95 THEN 20
      WHEN sp.COMPLETION_RATE_PERCENTAGE > 85 THEN 15
      WHEN sp.COMPLETION_RATE_PERCENTAGE > 70 THEN 10
      ELSE 0
    END AS performance_score,
    
    dm.DISTANCE_KM,
    ec.WORKLOAD_PERCENTAGE
    
  FROM ENGINEER_SCHEDULE es
  JOIN ENGINEER_CAPACITY ec ON es.ENGINEER_ID = ec.ENGINEER_ID
  JOIN DISTANCE_MATRIX dm ON dm.FROM_LOCATION_ID = @Origin AND es.ENGINEER_ID = (SELECT LOGIN_ID FROM LOGIN_MASTER WHERE LOGIN_ID = es.ENGINEER_ID)
  LEFT JOIN SCHEDULING_PERFORMANCE sp ON sp.ENGINEER_ID = es.ENGINEER_ID 
    AND sp.PERFORMANCE_DATE >= DATEADD(DAY, -7, GETDATE())
  WHERE es.SCHEDULE_DATE = CAST(GETDATE() AS DATE)
    AND ec.AVAILABLE_CAPACITY > 0
)
SELECT TOP 1
  ENGINEER_ID,
  (proximity_score + availability_score + balance_score + skill_score + performance_score) AS total_score,
  proximity_score, availability_score, balance_score, skill_score, performance_score
FROM EngineerScores
ORDER BY total_score DESC, DISTANCE_KM ASC
```

---

## Optimization Algorithms

### 1. Nearest Neighbor (Greedy) - Fast but suboptimal
```
Start: current location
Loop:
  Find unvisited location closest to current location
  Add to route
  current_location = new location
End when all locations visited
Time: O(n²), Efficiency: ~70-80%
```

### 2. Genetic Algorithm - Slow but near-optimal
```
Population: 100 random routes
Generations: 1000
For each generation:
  Evaluate fitness (distance, time, satisfaction)
  Keep top 50 routes (selection)
  Cross-breed routes (recombination)
  Randomly modify routes (mutation)
Result: Best route found
Time: O(n³), Efficiency: ~90-98%
```

### 3. Simulated Annealing - Balanced approach
```
Start: Initial feasible route
Temperature: high
While temperature > threshold:
  Generate neighbor route (small change)
  If better: accept
  If worse: accept with probability e^(-cost_delta/temperature)
  Lower temperature
Result: Near-optimal route  
Time: O(n²), Efficiency: ~85-92%
```

### 4. Dynamic Programming - For smaller problems
```
Subproblem: minimum cost to visit subset of locations
Build table of: min_cost[visited_set][last_location]
Result: Optimal route
Constraint: Only feasible for ~20 locations max
Time: O(2^n * n²), Efficiency: 100%
```

---

## Monitoring & Alerts

### Real-time Metrics

```sql
-- Current status dashboard
SELECT 
  CURRENT_TIMESTAMP,
  (SELECT COUNT(*) FROM SERVICE_ROUTE WHERE ROUTE_STATUS = 'IN_PROGRESS') AS active_routes,
  (SELECT COUNT(*) FROM ROUTE_STOP WHERE STOP_STATUS = 'IN_PROGRESS') AS stops_in_progress,
  (SELECT COUNT(*) FROM SCHEDULING_CONFLICT WHERE RESOLUTION_STATUS = 'OPEN') AS open_conflicts,
  (SELECT AVG(WORKLOAD_PERCENTAGE) FROM ENGINEER_CAPACITY WHERE CAPACITY_DATE = CAST(GETDATE() AS DATE)) AS avg_utilization,
  (SELECT COUNT(*) FROM SERVICE_ORDER_HDR 
   WHERE APPOINTMENT_ENGINEER IS NULL AND APPOINTMENT_DATE <= GETDATE()) AS unassigned_overdue
```

### Alert Conditions

```sql
-- Alert if unassigned orders exist
IF (SELECT COUNT(*) FROM SERVICE_ORDER_HDR 
    WHERE APPOINTMENT_ENGINEER IS NULL 
    AND APPOINTMENT_DATE <= GETDATE()) > 5
  EXECUTE sp_SendAlert 'Unassigned Orders Alert'

-- Alert if conflicts not resolved
IF (SELECT COUNT(*) FROM SCHEDULING_CONFLICT 
    WHERE RESOLUTION_STATUS = 'OPEN'
    AND DATEDIFF(HOUR, ENTERED_ON, GETDATE()) > 2) > 0
  EXECUTE sp_SendAlert 'Scheduling Conflicts Unresolved'

-- Alert if engineer over capacity
IF (SELECT COUNT(*) FROM ENGINEER_CAPACITY 
    WHERE WORKLOAD_PERCENTAGE > 100) > 0
  EXECUTE sp_SendAlert 'Engineer Over Capacity'
```

---

## Integration with ServiceOrders

### Bi-directional Updates

```sql
-- When route published, update SERVICE_ORDER_HDR
UPDATE SERVICE_ORDER_HDR
SET 
  APPOINTMENT_ENGINEER = (SELECT ENGINEER_ID FROM SERVICE_ROUTE WHERE ID = @RouteID),
  APPOINTMENT_DATE = (SELECT ROUTE_DATE FROM SERVICE_ROUTE WHERE ID = @RouteID),
  CHANGED_ON = GETDATE()
WHERE SERNO_DELL IN (
  SELECT SERNO_DELL FROM ROUTE_STOP WHERE ROUTE_ID = @RouteID
)

-- When service completes, update ROUTE_STOP
UPDATE ROUTE_STOP
SET 
  STOP_STATUS = 'COMPLETED',
  ACTUAL_DEPARTURE_TIME = GETDATE()
WHERE SERNO_DELL IN (
  SELECT SERNO_DELL FROM SERVICE_ORDER_HDR 
  WHERE SERVICE_COMPLETED_DATE = GETDATE()
)
```

---

## Success Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| **Assignment Success** | >95% | Assigned / Total Orders |
| **Route Efficiency** | >85/100 | Optimization score |
| **Completion Rate** | >90% | Completed / Scheduled |
| **Time Accuracy** | ±15 min | Actual vs Planned |
| **Distance Accuracy** | ±10% | Actual vs Planned |
| **Customer Satisfaction** | >4.0/5.0 | Rating average |
| **Engineer Utilization** | 70-85% | Workload % |
| **Conflict Rate** | <5% | Conflicts / Orders |
| **Optimization Speed** | <60 sec | For 100 orders |
| **Data Freshness** | <5 min | Last update vs now |


🌟 Core Features Implemented
1. Location-wise Scheduling
Customer location GPS coordinates (LOCATION_COORDINATE)
Distance-based matching (DISTANCE_MATRIX)
Proximity-based engineer assignment (30km radius)
2. Date-wise Scheduling
Daily capacity tracking per engineer (ENGINEER_CAPACITY)
Holiday/blackout calendar (CALENDAR_MAINTENANCE)
Availability-based assignment
3. Distance-wise Optimization
Daily route generation (SERVICE_ROUTE)
Stop sequencing for efficiency (ROUTE_STOP)
Travel time calculation
4 optimization algorithms supported
4. Automatic Assignment
Rule-based assignment engine (SCHEDULING_RULE)
5 built-in rules: Proximity, Availability, Load Balance, Expertise, Time Window
Priority-based execution
Confidence scoring (0-100%)
5. Route Optimization
Minimum distance routing
Time-based sequencing
Efficiency scoring (0-100)
Multiple algorithm support
6. Conflict Detection
6 automatic conflict types detected
Real-time resolution tracking
Manager notification system
7. Advanced Analytics
Engineer performance tracking
Daily/weekly/monthly metrics
Insights & recommendations
Performance ranking
8. Calendar Management
Holiday management
Maintenance windows
Peak/low season marking
Recurring event support