# MeetingModule Documentation

## Module Overview
The MeetingModule manages meeting types, schedules, and polling activities for organizational gatherings.

## Tables

### MEETTYPE_MAST
- **Purpose**: Master definitions for meeting types
- **Key Columns**: MEETTYPE_CODE, MEETTYPE_NAME, MEETTYPE_DESC

### SRF_MEETINGSCH
- **Purpose**: Meeting schedule and event management
- **Relationship**: References MEETTYPE_MAST
- **Key Columns**: MEETING_TITLE, MEETING_DATE, MEETING_LOCATION, MEETING_STATUS, MEETING_DURATION

### SRF_POLL_DETAIL
- **Purpose**: Polls and surveys conducted during meetings
- **Relationship**: References SRF_MEETINGSCH
- **Key Columns**: POLL_QUESTION, POLL_TYPE, POLL_STATUS

## Meeting Status
SCHEDULED, ONGOING, COMPLETED, CANCELLED

## Poll Types
MULTIPLE_CHOICE, YES_NO, RATING, TEXT

## Deployment
```sql
:r "MeetingModule_Schema.sql"
```

## Quick Start
```sql
-- Check upcoming meetings
SELECT * FROM SRF_MEETINGSCH 
WHERE MEETING_STATUS = 'SCHEDULED' 
  AND MEETING_DATE > GETDATE()
ORDER BY MEETING_DATE;
```

---
**Created**: March 09, 2026
**Version**: 1.0
