# BookingModule Documentation

## Module Overview
The BookingModule manages event and resource bookings with complete attendee tracking and location-based booking records.

## Tables

### BOOK_MAIN
- **Purpose**: Master booking records with application tracking
- **Key Columns**: BOOKING_APPNO, BOOKING_TITLE, LOCATION_CODE, BOOKING_STATUS

### BOOK_REC
- **Purpose**: Booking records by location with detailed information
- **Relationship**: References BOOK_MAIN

### BOOK_ATTENDEES
- **Purpose**: Attendee registrations and attendance tracking
- **Relationship**: References BOOK_MAIN

## Status Values
- DRAFT, SUBMITTED, APPROVED, REJECTED, CANCELLED

## Deployment
```sql
:r "BookingModule_Schema.sql"
```

## Verification Query
```sql
SELECT COUNT(*) AS BookingCount FROM BOOK_MAIN;
SELECT COUNT(*) AS AttendeeCount FROM BOOK_ATTENDEES;
```

---
**Created**: March 09, 2026
**Version**: 1.0
