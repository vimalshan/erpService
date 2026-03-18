using AutoMapper;
using DeductionService.Application.DTOs;
using DeductionService.Domain.Entities;

namespace DeductionService.Application.Mapping;

public class DeductionMappingProfile : Profile
{
    public DeductionMappingProfile()
    {
        CreateMap<AdhocPayDeduction, AdhocPayDeductionDto>()
            .ConstructUsing(src => new AdhocPayDeductionDto(
                src.SystemId, src.CanteenUnit, src.SerialNumber, src.BatchNumber,
                src.TransactionDate, src.EarningDeductionCode, src.ReferenceNumber,
                src.PayAmount, src.OppositeAmount, src.EntryDate, src.EnteredByUserId,
                src.CancelFlag, src.AttachmentNumber, src.CompanyCode,
                src.EmployeeNumber, src.UpdateFlag, src.SequenceNumber, src.GradeType));

        CreateMap<AdhocPayDeductionHistory, AdhocPayDeductionHistoryDto>()
            .ConstructUsing(src => new AdhocPayDeductionHistoryDto(
                src.SystemId, src.CanteenUnit, src.SerialNumber, src.BatchNumber,
                src.TransactionDate, src.EarningDeductionCode, src.PayAmount,
                src.EntryDate, src.EnteredByUserId, src.CancelFlag,
                src.CompanyCode, src.EmployeeNumber));

        CreateMap<DeductionAccess, DeductionAccessDto>()
            .ConstructUsing(src => new DeductionAccessDto(
                src.AccessNumber, src.UnitCode, src.DeductionType,
                src.SystemId, src.EnteredByUserId, src.EnteredOn, src.ClosedOn, src.IsActive));
    }
}
