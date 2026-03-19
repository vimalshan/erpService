# CFS Advanced Scheduling - Implementation Checklist

## Phase 1: Database Setup (Prerequisites)

- [ ] Create 8 DELL_RTU_* databases (Auth, ServiceOrders, Communication, MasterData, ToolkitManagement, DamageTracking, Archive, Scheduling)
- [ ] Execute all 9 schema SQL files in order:
  1. [ ] Auth/schema/01_auth_tables.sql
  2. [ ] ServiceOrders/schema/01_service_orders_tables.sql
  3. [ ] ServiceOrders/schema/02_service_forms_tables.sql
  4. [ ] Communication/schema/01_communication_tables.sql
  5. [ ] MasterData/schema/01_master_data_tables.sql
  6. [ ] ToolkitManagement/schema/01_toolkit_tables.sql
  7. [ ] DamageTracking/schema/01_damage_tracking_tables.sql
  8. [ ] Archive/schema/01_archive_tables.sql
  9. [ ] Scheduling/schema/01_scheduling_tables.sql
  10. [ ] Scheduling/schema/02_cfs_advanced_scheduling_tables.sql (NEW - CFS TABLES)

---

## Phase 2: CFS Foundation Setup

### 2.1 Location Setup
- [ ] Load LOCATION_COORDINATE table with:
  - [ ] Branch/Depot locations (headquarters)
  - [ ] Engineer home locations (if applicable)
  - [ ] Customer locations (from SERVICE_ORDER_HDR)
  - [ ] Service area definitions
- [ ] Verify LATITUDE/LONGITUDE accuracy
- [ ] Sample SQL:
```sql
INSERT INTO LOCATION_COORDINATE (LOCATION_CODE, LOCATION_NAME, LATITUDE, LONGITUDE, CITY, BRANCH)
SELECT DISTINCT BRANCH, BRANCH_NAME, 28.6139, 77.2090, CITY, BRANCH
FROM BRANCH_MASTER WHERE ISVALID = 1
```

### 2.2 Distance Matrix Calculation
- [ ] Calculate distances between all location pairs using API or formula
- [ ] Populate DISTANCE_MATRIX with:
  - [ ] FROM_LOCATION_ID (branch/origin)
  - [ ] TO_LOCATION_ID (customer location)
  - [ ] DISTANCE_KM
  - [ ] TRAVEL_TIME_MINUTES
  - [ ] Route type (SHORTEST/FASTEST)
- [ ] Options:
  - [ ] Use Google Maps Distance Matrix API
  - [ ] Use HERE Maps API
  - [ ] Use offline algorithm if available
- [ ] Refresh cycle: Weekly or on-demand

### 2.3 Engineer Capacity Initialization
- [ ] For each active engineer in LOGIN_MASTER where USER_TYPE='ENGINEER':
  - [ ] Create ENGINEER_CAPACITY record for same day + 90 days
  - [ ] Set MAX_CALLS_PER_DAY = 8 (configurable per engineer/branch)
  - [ ] Set TOTAL_DRIVE_TIME_MINUTES = 480 (8 hours)
  - [ ] Initialize CURRENT_CALLS = 0
- [ ] Sample SQL:
```sql
INSERT INTO ENGINEER_CAPACITY (ENGINEER_ID, CAPACITY_DATE, MAX_CALLS_PER_DAY, TOTAL_DRIVE_TIME_MINUTES)
SELECT LOGIN_ID, CAST(GETDATE() + d.DayNum AS DATE), 8, 480
FROM LOGIN_MASTER lm
CROSS JOIN (SELECT 0 AS DayNum UNION ALL SELECT 1 UNION ALL SELECT 2... SELECT 90) d
WHERE lm.USER_TYPE = 'ENGINEER' AND lm.ISVALID = 1
```

### 2.4 Scheduling Rules Definition
- [ ] Create base rules with priorities:
  - [ ] Priority 1: PROXIMITY (location distance)
  - [ ] Priority 2: AVAILABILITY (engineer has capacity)
  - [ ] Priority 3: LOAD_BALANCE (fair distribution)
  - [ ] Priority 4: EXPERTISE (skill match - optional)
  - [ ] Priority 5: TIME_WINDOW (customer preference)
- [ ] Configure each rule with JSON criteria and logic
- [ ] Sample:
```sql
INSERT INTO SCHEDULING_RULE 
(RULE_NAME, RULE_CODE, PRIORITY, LOCATION_RADIUS_KM, AUTO_ASSIGNMENTS_ENABLED)
VALUES ('Nearest Available Engineer', 'PROXIMITY', 1, 30, 1)
```

---

## Phase 3: Calendar Setup

### 3.1 Holiday Calendar
- [ ] Load CALENDAR_MAINTENANCE with national holidays
- [ ] Set SCHEDULING_DISABLED = 1 for full-day holidays
- [ ] Set RECURRING = 1 and RECURRING_PATTERN = 'YEARLY'
- [ ] Sample holidays to load:
  - [ ] New Year (Jan 1)
  - [ ] National holidays (Republic Day, Independence Day, etc.)
  - [ ] Regional holidays

### 3.2 Maintenance Windows
- [ ] Load planned maintenance windows:
  - [ ] System maintenance dates
  - [ ] Server upgrades
  - [ ] Database maintenance
- [ ] Set SCHEDULING_DISABLED = 1

### 3.3 Peak/Low Season Markers
- [ ] Mark high-demand seasons (e.g., summer for cooling issues)
- [ ] Mark low-demand seasons for capacity planning
- [ ] Used by analytics for trend analysis

### 3.4 Blackout Dates
- [ ] Load BLACKOUT_DATE with engineer-specific off-days
- [ ] Support scenarios:
  - [ ] Personal leave
  - [ ] Training
  - [ ] Certification exams
  - [ ] Rotation/transfer days

---

## Phase 4: Basic CFS Workflow Implementation

### 4.1 Daily Optimization Setup (CRON JOB)
- [ ] Create scheduled job to run every morning at 06:00 AM
- [ ] Job tasks:
  - [ ] Load pending SERVICE_ORDER_HDR for next 3 days
  - [ ] Create CFS_SCHEDULE_OPTIMIZATION record
  - [ ] Invoke optimization engine
  - [ ] Log execution metrics

### 4.2 Optimization Engine Implementation
- [ ] Choose algorithm: GREEDY (fast, 80% efficient) or GENETIC (slow, 95% efficient)
- [ ] Implement pseudo-code:
  ```
  1. Load unassigned service orders
  2. Apply SCHEDULING_RULE in priority order
  3. For each order:
     a. Filter engineers by SCHEDULING_RULE criteria
     b. Score engineers (proximity, availability, workload, skill, performance)
     c. Select highest-scoring engineer
     d. Create ASSIGNMENT_LOG entry
  4. Group assignments by engineer
  5. Generate SERVICE_ROUTE + ROUTE_STOP for each engineer
  6. Calculate route metrics (distance, time, efficiency)
  7. Detect conflicts (SCHEDULING_CONFLICT)
  8. Update CFS_SCHEDULE_OPTIMIZATION with results
  9. Return unassigned orders for manual handling
  ```

### 4.3 Route Generation
- [ ] For each engineer with assignments:
  - [ ] Create SERVICE_ROUTE record
  - [ ] Sequence ROUTE_STOP by optimization algorithm
  - [ ] Calculate STOP_SEQUENCE
  - [ ] Set SCHEDULED_ARRIVAL_TIME based on sequential timing
  - [ ] Calculate DISTANCE_FROM_PREVIOUS_KM from DISTANCE_MATRIX
  - [ ] Derive ESTIMATED_SERVICE_DURATION_MINUTES from order type
  - [ ] Calculate total metrics

### 4.4 Conflict Detection & Resolution
- [ ] Detect automatic conflicts:
  - [ ] DOUBLE_BOOKING (overlapping time windows)
  - [ ] LOCATION_UNREACHABLE (drive time exceeds available time)
  - [ ] CAPACITY_EXCEEDED (order count > MAX_CALLS_PER_DAY)
  - [ ] TIME_WINDOW_CONFLICT (can't meet customer window)
- [ ] For each conflict:
  - [ ] Create SCHEDULING_CONFLICT record
  - [ ] Suggest RECOMMENDED_ACTION
  - [ ] Alert manager for manual resolution
  - [ ] Track RESOLUTION_STATUS

### 4.5 Route Publication
- [ ] Validate all routes are conflict-free
- [ ] Update SERVICE_ROUTE.ROUTE_STATUS = 'PUBLISHED'
- [ ] Sync to SERVICE_ORDER_HDR:
```sql
UPDATE SERVICE_ORDER_HDR
SET APPOINTMENT_ENGINEER = sr.ENGINEER_ID,
    APPOINTMENT_DATE = sr.ROUTE_DATE
FROM SERVICE_ROUTE sr
JOIN ROUTE_STOP rs ON rs.ROUTE_ID = sr.ID
WHERE SERVICE_ORDER_HDR.SERNO_DELL = rs.SERNO_DELL
```
- [ ] Send notifications via Message Corner

---

## Phase 5: Execution Tracking

### 5.1 Real-time Monitoring
- [ ] Engineer mobile app updates ROUTE_STOP status:
  - [ ] Mark STOP_STATUS = 'IN_PROGRESS' on arrival
  - [ ] Enter ACTUAL_ARRIVAL_TIME
  - [ ] Complete service work
  - [ ] Mark STOP_STATUS = 'COMPLETED' on departure
  - [ ] Enter ACTUAL_DEPARTURE_TIME

### 5.2 Route Execution Dashboard
- [ ] Create view showing:
  - [ ] Active routes (ROUTE_STATUS = 'IN_PROGRESS')
  - [ ] Current engineer location (ACTUAL_ARRIVAL_TIME)
  - [ ] Next stops in itinerary
  - [ ] Completion percentage (Completed / Total)
  - [ ] On-time status (vs. SCHEDULED_ARRIVAL_TIME)

### 5.3 Delay Handling
- [ ] If engineer delayed:
  - [ ] Calculate cascade delay to next stops
  - [ ] Alert if next stop unreachable by scheduled time
  - [ ] Suggest rescheduling or skip
  - [ ] Auto-create SCHEDULING_CONFLICT if needed

---

## Phase 6: Analytics & Performance

### 6.1 End-of-Day Performance Calculation
- [ ] For each engineer at end of day:
  - [ ] Calculate SCHEDULING_PERFORMANCE record
  - [ ] Metrics:
    - [ ] SCHEDULED_CALLS (planned assignments)
    - [ ] COMPLETED_CALLS (actually done)
    - [ ] COMPLETION_RATE_PERCENTAGE = completed/scheduled * 100
    - [ ] PLANNED_DISTANCE_KM (route total)
    - [ ] ACTUAL_DISTANCE_KM (actual travelled)
    - [ ] DISTANCE_VARIANCE_PERCENTAGE
    - [ ] CUSTOMER_SATISFACTION_RATING (from feedback)
    - [ ] SCHEDULING_EFFICIENCY_SCORE (composite)

### 6.2 Period Analytics Generation
- [ ] Daily (next morning):
  - [ ] SCHEDULING_ANALYSIS records for each metric
  - [ ] Branch-level metrics
  - [ ] Variance from target analysis
  - [ ] Insights and recommendations

### 6.3 KPI Dashboard
- [ ] Create reports showing:
  - [ ] Assignment success rate (%)
  - [ ] Route efficiency score (0-100)
  - [ ] Completion rate (%)
  - [ ] On-time performance (%)
  - [ ] Customer satisfaction (1-5 stars)
  - [ ] Engineer utilization (%)
  - [ ] Total distance/fuel metrics
  - [ ] Conflict rate (%)
  - [ ] Unassigned orders count

---

## Phase 7: Advanced Features

### 7.1 Machine Learning Integration (Optional)
- [ ] Collect historical ASSIGNMENT_LOG + SCHEDULING_PERFORMANCE data
- [ ] Train model to predict:
  - [ ] Best engineer for given order (classification)
  - [ ] Assignment success rate (probability)
  - [ ] Service duration (regression)
- [ ] Use predictions to improve CONFIDENCE_SCORE

### 7.2 Real-time Traffic Integration (Optional)
- [ ] Integrate with Google Maps/HERE real-time traffic
- [ ] During execution:
  - [ ] Get current ETA to next stop
  - [ ] Compare vs. SCHEDULED_ARRIVAL_TIME
  - [ ] Adjust cascading stops if needed
  - [ ] Alert if delays exceed threshold

### 7.3 Multi-day Planning (Optional)
- [ ] Extend optimization beyond single day
- [ ] Consider:
  - [ ] Engineer's 3-day workload distribution
  - [ ] Preventive maintenance scheduling
  - [ ] Engineer rotation/coverage

---

## Phase 8: Integration Across Microservices

### 8.1 ServiceOrders Integration
- [ ] Bi-directional sync:
  - [ ] Appointment details (date, engineer)
  - [ ] Service completion status
  - [ ] RC form completion
  - [ ] Parts tracking
- [ ] Endpoint: `/serviceorders/{serno}/appointment`
- [ ] Trigger updates when:
  - [ ] Route published
  - [ ] Service completes
  - [ ] Appointment rescheduled

### 8.2 Auth Service Integration
- [ ] Validate engineer IDs before assignment
- [ ] Check engineer's active status
- [ ] Audit log all system actions to AUTH service
- [ ] Endpoint: `/auth/engineer/{engineerId}/validate`

### 8.3 MasterData Service Integration
- [ ] Reference BRANCH_MASTER for branch validation
- [ ] Reference SLA_MASTER for time window constraints
- [ ] Lookup COMMODITY_MASTER for skill requirements
- [ ] Cache master data locally for performance

### 8.4 Communication Service Integration
- [ ] Send notifications when:
  - [ ] Route published (to engineer)
  - [ ] Appointment confirmed (to customer)
  - [ ] Conflict created (to manager)
  - [ ] Performance report ready (to supervisor)
- [ ] Endpoint: `/communication/message/create`

---

## Phase 9: Testing & Validation

### 9.1 Unit Tests
- [ ] Test each SCHEDULING_RULE independently
- [ ] Test distance calculation accuracy
- [ ] Test capacity deduction logic
- [ ] Test conflict detection scenarios

### 9.2 Integration Tests
- [ ] Test full workflow: Order → Assignment → Route → Completion
- [ ] Test bi-directional sync with ServiceOrders
- [ ] Test cross-service notifications
- [ ] Test authorization checks

### 9.3 Load Tests
- [ ] Test optimization performance:
  - [ ] 100 orders, 20 engineers → < 10 sec
  - [ ] 500 orders, 100 engineers → < 60 sec
  - [ ] 1000+ orders → < 2 minutes
- [ ] Test real-time tracking with 100 concurrent engineers
- [ ] Test analytics calculation for 90 days of data

### 9.4 Scenario Tests
- [ ] Test 1: Basic proximity assignment
- [ ] Test 2: Capacity overflow handling
- [ ] Test 3: Time window conflict resolution
- [ ] Test 4: Holiday/blackout date exclusion
- [ ] Test 5: Engineer performance ranking
- [ ] Test 6: Multi-day route planning

---

## Phase 10: Production Deployment

### 10.1 Pre-deployment Checklist
- [ ] All unit/integration tests passing
- [ ] Load tests passed (performance acceptable)
- [ ] Documentation complete and reviewed
- [ ] Training materials prepared for team
- [ ] Backup strategy documented
- [ ] Rollback plan prepared

### 10.2 Initial Data Load
- [ ] Load sample data for testing:
  - [ ] 50 locations with coordinates
  - [ ] 200 service orders
  - [ ] 50 engineers with capacity
  - [ ] 5 years of holiday calendar
- [ ] Validate data integrity
- [ ] Run optimization on sample data

### 10.3 Pilot Deployment
- [ ] Deploy to one branch first
- [ ] Run for 1 week
- [ ] Monitor performance metrics
- [ ] Collect feedback
- [ ] Make adjustments

### 10.4 Full Rollout
- [ ] Deploy to all branches
- [ ] Enable automatic daily optimization
- [ ] Monitor for issues
- [ ] Weekly performance reviews

---

## Phase 11: Maintenance & Operations

### 11.1 Daily Maintenance
- [ ] Review optimization results each morning
- [ ] Check for unresolved SCHEDULING_CONFLICT records
- [ ] Monitor engineer availability
- [ ] Validate real-time tracking data

### 11.2 Weekly Tasks
- [ ] Refresh DISTANCE_MATRIX with latest data
- [ ] Review SCHEDULING_ANALYSIS reports
- [ ] Adjust SCHEDULING_RULE parameters if needed
- [ ] Identify top/bottom performing engineers
- [ ] Archive completed routes

### 11.3 Monthly Tasks
- [ ] Review trends in SCHEDULING_ANALYSIS
- [ ] Update engineer capacity baselines
- [ ] Analyze cost metrics (distance, fuel)
- [ ] Generate executive dashboard report
- [ ] Plan capacity for upcoming season

### 11.4 Quarterly Reviews
- [ ] Assess overall system performance vs. KPIs
- [ ] Collect stakeholder feedback
- [ ] Plan enhancements/optimizations
- [ ] Training refresher for new engineers

---

## Success Metrics

| KPI | Target | Current | Status |
|-----|--------|---------|--------|
| Assignment Success Rate | >95% | - | ⬜ |
| Route Efficiency Score | >85/100 | - | ⬜ |
| Completion Rate | >90% | - | ⬜ |
| On-time Performance | >85% | - | ⬜ |
| Customer Satisfaction | >4.0/5.0 | - | ⬜ |
| Engineer Utilization | 70-85% | - | ⬜ |
| Conflict Resolution Time | <2 hours | - | ⬜ |
| Unassigned Orders | <5% | - | ⬜ |
| Optimization Speed | <60 sec | - | ⬜ |

---

## Document References

- [CFS_ADVANCED_GUIDE.md](CFS_ADVANCED_GUIDE.md) - Full system specification
- [CFS_IMPLEMENTATION_GUIDE.md](CFS_IMPLEMENTATION_GUIDE.md) - Code examples and algorithms
- [README.md](README.md) - Quick reference
- [ServiceOrders/README.md](../ServiceOrders/README.md) - Service order integration
- [RC_FORMS_GUIDE.md](../ServiceOrders/RC_FORMS_GUIDE.md) - RC form lifecycle

---

## Notes

- CFS table creation SQL: `02_cfs_advanced_scheduling_tables.sql`
- Base scheduling tables already existed: `01_scheduling_tables.sql`
- Total: 14 tables in Scheduling service (5 basic + 9 advanced)
- Integration points: ServiceOrders, Auth, MasterData, Communication
- Optimization algorithms: Greedy, Genetic, Simulated Annealing, Dynamic Programming

