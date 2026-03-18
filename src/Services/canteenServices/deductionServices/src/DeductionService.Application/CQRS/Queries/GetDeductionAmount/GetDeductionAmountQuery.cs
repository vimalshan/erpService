using DeductionService.Application.DTOs;
using MediatR;

namespace DeductionService.Application.CQRS.Queries.GetDeductionAmount;

public record GetDeductionAmountQuery(
    long EmpSysId,
    long ItemCode,
    DateTime DateTaken) : IRequest<DeductionAmountDto>;

public interface IDeductionAmountService
{
    Task<DeductionAmountDto> GetDeductionAmountAsync(long empSysId, long itemCode, DateTime dateTaken, CancellationToken ct = default);
}

public class GetDeductionAmountQueryHandler(IDeductionAmountService service)
    : IRequestHandler<GetDeductionAmountQuery, DeductionAmountDto>
{
    public Task<DeductionAmountDto> Handle(GetDeductionAmountQuery request, CancellationToken ct)
        => service.GetDeductionAmountAsync(request.EmpSysId, request.ItemCode, request.DateTaken, ct);
}
