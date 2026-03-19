using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Application.MainCategories.Commands;
using CategoryAndVendorService.Application.SubCategories.Commands;
using CategoryAndVendorService.Application.VendorDocuments.Commands;
using CategoryAndVendorService.Application.SupportDocuments.Commands;
using MediatR;

namespace CategoryAndVendorService.API.GraphQL;

public class Mutation
{
    public async Task<MainCategoryDto> CreateMainCategory([Service] IMediator mediator, CreateMainCategoryCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<MainCategoryDto> UpdateMainCategory([Service] IMediator mediator, UpdateMainCategoryCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteMainCategory([Service] IMediator mediator, long mainCatId, CancellationToken ct)
        => await mediator.Send(new DeleteMainCategoryCommand(mainCatId), ct);

    public async Task<SubCategoryDto> CreateSubCategory([Service] IMediator mediator, CreateSubCategoryCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<SubCategoryDto> UpdateSubCategory([Service] IMediator mediator, UpdateSubCategoryCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteSubCategory([Service] IMediator mediator, long subCatId, CancellationToken ct)
        => await mediator.Send(new DeleteSubCategoryCommand(subCatId), ct);

    public async Task<VendorDocumentDto> CreateVendorDocument([Service] IMediator mediator, CreateVendorDocumentCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<VendorDocumentDto> ApproveVendorDocument([Service] IMediator mediator, ApproveVendorDocumentCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<VendorDocumentDto> RejectVendorDocument([Service] IMediator mediator, RejectVendorDocumentCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<SupportDocumentDto> CreateSupportDocument([Service] IMediator mediator, CreateSupportDocumentCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteSupportDocument([Service] IMediator mediator, long docId, CancellationToken ct)
        => await mediator.Send(new DeleteSupportDocumentCommand(docId), ct);
}
