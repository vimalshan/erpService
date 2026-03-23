namespace VisitorServices.Application.DTOs;

public sealed record VisitorDto(
    long VisitorId,
    string VisitorName,
    string IdType,
    string? IdNumber,
    string? PhoneNumber,
    string? Email,
    string? Company,
    string? Purpose,
    DateTime CheckInTime,
    DateTime? CheckOutTime,
    string Status,
    long WhomToVisit,
    DateTime EnteredOn,
    long EnteredBy);
