using MediatR;
using StipendService.Application.DTOs;

namespace StipendService.Application.Features.StipendMaster.Commands;

// ------ Create ------
public record CreateStipendMasterCommand(
    long ResearchCategoryId,
    long SrfRankId,
    decimal SrfMonthlyStipend,
    decimal? AdditionalAllowance,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo,
    long CreatedBy
) : IRequest<StipendMasterDto>;

// ------ Update ------
public record UpdateStipendMasterCommand(
    long StipendId,
    decimal SrfMonthlyStipend,
    decimal? AdditionalAllowance,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo,
    long UpdatedBy
) : IRequest<StipendMasterDto>;

// ------ Deactivate ------
public record DeactivateStipendMasterCommand(
    long StipendId,
    long UpdatedBy
) : IRequest<bool>;
