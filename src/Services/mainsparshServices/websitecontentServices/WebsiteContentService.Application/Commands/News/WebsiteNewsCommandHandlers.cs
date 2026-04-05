namespace WebsiteContentService.Application.Commands.News;

using AutoMapper;
using MediatR;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Domain.Entities;
using WebsiteContentService.Domain.Repositories;

public class CreateWebsiteNewsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateWebsiteNewsCommand, WebsiteNewsDto>
{
    public async Task<WebsiteNewsDto> Handle(CreateWebsiteNewsCommand request, CancellationToken ct)
    {
        var news = WebsiteNews.Create(
            request.NewsTitle, request.NewsContent, request.NewsSummary,
            request.NewsCategory, request.FeaturedImage,
            request.PublishStartDate, request.PublishEndDate, request.CreatedBy);

        await unitOfWork.WebsiteNews.AddAsync(news, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<WebsiteNewsDto>(news);
    }
}

public class UpdateWebsiteNewsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateWebsiteNewsCommand, WebsiteNewsDto>
{
    public async Task<WebsiteNewsDto> Handle(UpdateWebsiteNewsCommand request, CancellationToken ct)
    {
        var news = await unitOfWork.WebsiteNews.GetByIdAsync(request.NewsId, ct)
            ?? throw new KeyNotFoundException($"News with ID {request.NewsId} not found.");

        news.Update(request.NewsTitle, request.NewsContent, request.NewsSummary,
            request.NewsCategory, request.FeaturedImage,
            request.PublishStartDate, request.PublishEndDate, request.UpdatedBy);

        await unitOfWork.WebsiteNews.UpdateAsync(news, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<WebsiteNewsDto>(news);
    }
}

public class PublishWebsiteNewsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<PublishWebsiteNewsCommand, WebsiteNewsDto>
{
    public async Task<WebsiteNewsDto> Handle(PublishWebsiteNewsCommand request, CancellationToken ct)
    {
        var news = await unitOfWork.WebsiteNews.GetByIdAsync(request.NewsId, ct)
            ?? throw new KeyNotFoundException($"News with ID {request.NewsId} not found.");

        news.Publish(request.UpdatedBy);

        await unitOfWork.WebsiteNews.UpdateAsync(news, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<WebsiteNewsDto>(news);
    }
}

public class ArchiveWebsiteNewsCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ArchiveWebsiteNewsCommand, bool>
{
    public async Task<bool> Handle(ArchiveWebsiteNewsCommand request, CancellationToken ct)
    {
        var news = await unitOfWork.WebsiteNews.GetByIdAsync(request.NewsId, ct)
            ?? throw new KeyNotFoundException($"News with ID {request.NewsId} not found.");

        news.Archive(request.UpdatedBy);

        await unitOfWork.WebsiteNews.UpdateAsync(news, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return true;
    }
}

public class SetNewsFeaturedCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<SetNewsFeaturedCommand, WebsiteNewsDto>
{
    public async Task<WebsiteNewsDto> Handle(SetNewsFeaturedCommand request, CancellationToken ct)
    {
        var news = await unitOfWork.WebsiteNews.GetByIdAsync(request.NewsId, ct)
            ?? throw new KeyNotFoundException($"News with ID {request.NewsId} not found.");

        news.SetFeatured(request.IsFeatured, request.UpdatedBy);

        await unitOfWork.WebsiteNews.UpdateAsync(news, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<WebsiteNewsDto>(news);
    }
}
