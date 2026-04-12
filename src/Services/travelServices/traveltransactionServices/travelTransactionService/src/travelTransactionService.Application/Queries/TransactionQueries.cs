using MediatR;
using travelTransactionService.Application.DTOs;

namespace travelTransactionService.Application.Queries;

public record GetAllVendorsQuery : IRequest<IReadOnlyList<VendorMasterDto>>;

public record GetVendorByIdQuery(long VendorId) : IRequest<VendorMasterDto?>;

public record GetVendorsByCategoryQuery(string CategoryType) : IRequest<IReadOnlyList<VendorMasterDto>>;

public record GetAllTaxMastersQuery : IRequest<IReadOnlyList<TaxMasterDto>>;

public record GetTaxMasterByTypeQuery(string TaxType) : IRequest<TaxMasterDto?>;

public record GetTaxMastersByVendorQuery(long VendorId) : IRequest<IReadOnlyList<TaxMasterDto>>;

public record GetAllJaiInterfaceLinesQuery : IRequest<IReadOnlyList<JaiInterfaceLineDto>>;

public record GetJaiInterfaceLineByIdQuery(decimal InterfaceLineId) : IRequest<JaiInterfaceLineDto?>;

public record GetJaiInterfaceLinesByBatchQuery(decimal BatchId) : IRequest<IReadOnlyList<JaiInterfaceLineDto>>;

public record GetAllAccountMastersQuery : IRequest<IReadOnlyList<AccountMasterDto>>;

public record GetAllGlCodeCombinationsQuery : IRequest<IReadOnlyList<GlCodeCombinationDto>>;

public record GetAllJvInterfacesQuery : IRequest<IReadOnlyList<JvInterfaceDto>>;

public record GetAllJvMissingCombiCodesQuery : IRequest<IReadOnlyList<JvMissingCombiCodeDto>>;

public record GetAllBatchSubBreakupsQuery : IRequest<IReadOnlyList<BatchSubBreakupDto>>;

public record GetAllTravelApParamsQuery : IRequest<IReadOnlyList<TravelApParamsDto>>;

public record GetTravelApParamsByIdQuery(long ApUnitId) : IRequest<TravelApParamsDto?>;

public record GetAllSourceHistoryQuery : IRequest<IReadOnlyList<SourceHistoryDto>>;
