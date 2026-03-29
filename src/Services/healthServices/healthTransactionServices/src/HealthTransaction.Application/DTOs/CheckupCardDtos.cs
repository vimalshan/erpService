namespace HealthTransaction.Application.DTOs;

public record CheckupCardSubDto(
    decimal HlthNum,
    decimal SympId,
    string? FlagYn,
    string? SympVal,
    decimal EmpNum);

public record CheckupCardDto(
    decimal HlthNum,
    decimal EmpNum,
    DateTime? EmpDate,
    string? ComCode,
    string? PersonalDetails,
    string? ComplaintDetails,
    string? AdvRemark1,
    string? AdvRemark2,
    DateTime? DocDate1,
    DateTime? DocDate2,
    string? AdvFollow1,
    string? AdvFollow2,
    IList<CheckupCardSubDto> SubRecords);

public record CreateCheckupCardSubDto(
    decimal SympId,
    string? FlagYn,
    string? SympVal,
    decimal EmpNum);

public record CreateCheckupCardDto(
    decimal HlthNum,
    decimal EmpNum,
    DateTime? EmpDate,
    string? ComCode,
    string? PersonalDetails,
    string? ComplaintDetails,
    string? AdvRemark1,
    string? AdvRemark2,
    DateTime? DocDate1,
    DateTime? DocDate2,
    string? AdvFollow1,
    string? AdvFollow2,
    IList<CreateCheckupCardSubDto>? SubRecords);
