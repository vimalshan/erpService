using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Features.RateMaster.Queries
{
    public record GetAllRatesQuery(string? TrustCode = null) : IRequest<IEnumerable<RateMasterDto>>;
    public record GetRateByIdQuery(string TrustCode, int RateId) : IRequest<RateMasterDto?>;
}

namespace MasterDataService.Application.Features.RateMaster.Commands
{
    public record CreateRateCommand(string TrustCode, string? RateTypeCode, string? RateEffectiveDate, decimal? RateValue) : IRequest<RateMasterDto>;
    public record UpdateRateCommand(string TrustCode, int RateId, decimal? RateValue, string? ClosingDate) : IRequest<bool>;
    public record DeleteRateCommand(string TrustCode, int RateId) : IRequest<bool>;
}
