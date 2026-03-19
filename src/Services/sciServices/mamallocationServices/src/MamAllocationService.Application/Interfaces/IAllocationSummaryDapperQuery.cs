using MamAllocationService.Application.DTOs;

namespace MamAllocationService.Application.Handlers;

public interface IAllocationSummaryDapperQuery
{
    Task<AllocationSummaryDto?> ExecuteAsync(DateTime allocationDate, int rawMaterialCode, CancellationToken ct = default);
}
