namespace VisitorServices.Application.DTOs;

public sealed record VisitorDto(
    long VisitorId,
    string VisitorName,
    char IdType,
    string? IdNumber,
    string? PhoneNumber,
    string? Email,
    string? Company,
    string? Purpose,
    DateTime CheckInTime,
    DateTime? CheckOutTime,
    char Status,
    long WhomToVisit,
    DateTime EnteredOn,
    long EnteredBy);
