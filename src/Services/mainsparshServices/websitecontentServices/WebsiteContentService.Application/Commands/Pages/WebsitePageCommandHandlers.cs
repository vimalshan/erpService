namespace WebsiteContentService.Application.Commands.Pages;

using AutoMapper;
using MediatR;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Domain.Entities;
using WebsiteContentService.Domain.Repositories;

public class CreateWebsitePageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateWebsitePageCommand, WebsitePageDto>
{
    public async Task<WebsitePageDto> Handle(CreateWebsitePageCommand request, CancellationToken ct)
    {
        var existing = await unitOfWork.WebsitePages.GetByCodeAsync(request.PageCode, ct);
        if (existing is not null)
            throw new InvalidOperationException($"Page with code '{request.PageCode}' already exists.");

        var page = WebsitePage.Create(
            request.PageCode, request.PageTitle, request.PageContent,
            request.MetaDescription, request.MetaKeywords,
            request.PageOrder, request.ParentPageId, request.CreatedBy);

        await unitOfWork.WebsitePages.AddAsync(page, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<WebsitePageDto>(page);
    }
}

public class UpdateWebsitePageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateWebsitePageCommand, WebsitePageDto>
{
    public async Task<WebsitePageDto> Handle(UpdateWebsitePageCommand request, CancellationToken ct)
    {
        var page = await unitOfWork.WebsitePages.GetByIdAsync(request.PageId, ct)
            ?? throw new KeyNotFoundException($"Page with ID {request.PageId} not found.");

        page.Update(request.PageTitle, request.PageContent,
            request.MetaDescription, request.MetaKeywords,
            request.PageOrder, request.ParentPageId, request.UpdatedBy);

        await unitOfWork.WebsitePages.UpdateAsync(page, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<WebsitePageDto>(page);
    }
}

public class PublishWebsitePageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<PublishWebsitePageCommand, WebsitePageDto>
{
    public async Task<WebsitePageDto> Handle(PublishWebsitePageCommand request, CancellationToken ct)
    {
        var page = await unitOfWork.WebsitePages.GetByIdAsync(request.PageId, ct)
            ?? throw new KeyNotFoundException($"Page with ID {request.PageId} not found.");

        page.Publish(request.UpdatedBy);

        await unitOfWork.WebsitePages.UpdateAsync(page, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<WebsitePageDto>(page);
    }
}

public class ChangeWebsitePageStatusCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeWebsitePageStatusCommand, bool>
{
    public async Task<bool> Handle(ChangeWebsitePageStatusCommand request, CancellationToken ct)
    {
        var page = await unitOfWork.WebsitePages.GetByIdAsync(request.PageId, ct)
            ?? throw new KeyNotFoundException($"Page with ID {request.PageId} not found.");

        page.ChangeStatus(request.NewStatus, request.UpdatedBy);

        await unitOfWork.WebsitePages.UpdateAsync(page, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return true;
    }
}
