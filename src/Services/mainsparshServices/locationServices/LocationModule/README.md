# LocationModule Documentation

## Module Overview
The LocationModule manages physical locations, meeting rooms, and equipment resources required for organizational operations.

## Tables

### LOCATION_CONTACT
- **Purpose**: Location master with contact and address information
- **Key Columns**: LOCATION_CODE, LOCATION_NAME, CITY, STATE, COUNTRY, PHONE, EMAIL

### ROOM_MAST
- **Purpose**: Room definitions for each location
- **Relationship**: References LOCATION_CONTACT
- **Key Columns**: ROOM_CODE, ROOM_NAME, ROOM_CAPACITY, ROOM_TYPE, FLOOR_NUMBER

### ROOM_RESOURCE
- **Purpose**: Equipment and resources allocated to rooms
- **Relationship**: References ROOM_MAST and LOCATION_CONTACT
- **Key Columns**: RESOURCE_CODE, RESOURCE_NAME, RESOURCE_TYPE, RESOURCE_QUANTITY

## Room Types
MEETING, TRAINING, CONFERENCE, OFFICE, LAB, etc.

## Resource Types
PROJECTOR, WHITEBOARD, MICROPHONE, VIDEO_CONFERENCING, etc.

## Deployment
```sql
:r "LocationModule_Schema.sql"
```

## Setup Example
```sql
-- Insert location
INSERT INTO LOCATION_CONTACT (LOCATION_CODE, LOCATION_NAME, CITY, CREATED_BY)
VALUES ('LOC-001', 'Main Office', 'New Delhi', 1);

-- Insert room
INSERT INTO ROOM_MAST (LOCATION_ID, ROOM_CODE, ROOM_NAME, ROOM_CAPACITY, CREATED_BY)
SELECT LOCATION_ID, 'ROOM-101', 'Board Room', 30, 1
FROM LOCATION_CONTACT WHERE LOCATION_CODE = 'LOC-001';
```

---
**Created**: March 09, 2026
**Version**: 1.0
