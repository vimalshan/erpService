using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Application.MainCategories.Queries;
using CategoryAndVendorService.Application.SubCategories.Queries;
using CategoryAndVendorService.Application.VendorDocuments.Queries;
using CategoryAndVendorService.Application.SupportDocuments.Queries;
using MediatR;

namespace CategoryAndVendorService.API.GraphQL;

public class Query
{
    public async Task<IReadOnlyList<MainCategoryDto>> GetMainCategories([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllMainCategoriesQuery(), ct);

    public async Task<MainCategoryDto?> GetMainCategoryById([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetMainCategoryByIdQuery(id), ct);

    public async Task<IReadOnlyList<SubCategoryDto>> GetSubCategories([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllSubCategoriesQuery(), ct);

    public async Task<SubCategoryDto?> GetSubCategoryById([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetSubCategoryByIdQuery(id), ct);

    public async Task<IReadOnlyList<SubCategoryDto>> GetSubCategoriesByMainCategory([Service] IMediator mediator, long mainCatId, CancellationToken ct)
        => await mediator.Send(new GetSubCategoriesByMainCategoryQuery(mainCatId), ct);

    public async Task<IReadOnlyList<VendorDocumentDto>> GetVendorDocuments([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllVendorDocumentsQuery(), ct);

    public async Task<VendorDocumentDto?> GetVendorDocumentById([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetVendorDocumentByIdQuery(id), ct);

    public async Task<IReadOnlyList<SupportDocumentDto>> GetSupportDocuments([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllSupportDocumentsQuery(), ct);

    public async Task<SupportDocumentDto?> GetSupportDocumentById([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetSupportDocumentByIdQuery(id), ct);
}
