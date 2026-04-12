using travelTransactionService.Application.DTOs;

namespace travelTransactionService.Application.Interfaces;

public interface IDapperQueryService
{
    Task<IReadOnlyList<AccountMasterDto>> GetAllAccountMastersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GlCodeCombinationDto>> GetAllGlCodeCombinationsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JvInterfaceDto>> GetAllJvInterfacesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JvMissingCombiCodeDto>> GetAllJvMissingCombiCodesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BatchSubBreakupDto>> GetAllBatchSubBreakupsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TravelApParamsDto>> GetAllTravelApParamsAsync(CancellationToken cancellationToken = default);
    Task<TravelApParamsDto?> GetTravelApParamsByIdAsync(long apUnitId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SourceHistoryDto>> GetAllSourceHistoryAsync(CancellationToken cancellationToken = default);
}
