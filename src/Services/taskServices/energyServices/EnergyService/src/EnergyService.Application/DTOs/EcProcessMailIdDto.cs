namespace EnergyService.Application.DTOs;

public record EcProcessMailIdDto(
    int? PmId,
    int PmProcessId,
    string PmMailId,
    string PmDeliveryType,
    DateTime PmStartDate,
    DateTime? PmCloseDate,
    int PmLastModifiedBy,
    string PmLastModifiedOn);
