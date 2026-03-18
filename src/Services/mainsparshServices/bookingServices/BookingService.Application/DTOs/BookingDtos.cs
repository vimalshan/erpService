namespace BookingService.Application.DTOs;

public record BookRecordDto(
    long BookRecId,
    long BookingId,
    string LocationCode,
    string? RecDetails,
    string RecStatus,
    long CreatedBy,
    DateTime CreatedOn);

public record AttendeeDto(
    long AttendeeId,
    long BookingId,
    long AttendeeSysId,
    int AttendeeSerial,
    string AttendanceStatus,
    long CreatedBy,
    DateTime CreatedOn);

public record BookingDto(
    long BookingId,
    string BookingAppNo,
    string BookingTitle,
    string? LocationCode,
    DateTime? BookingDate,
    string Status,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn);

public record BookingDetailDto(
    long BookingId,
    string BookingAppNo,
    string BookingTitle,
    string? LocationCode,
    DateTime? BookingDate,
    string Status,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn,
    IEnumerable<BookRecordDto> Records,
    IEnumerable<AttendeeDto> Attendees);
