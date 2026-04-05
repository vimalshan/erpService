namespace WebsiteContentService.Application.Mappings;

using AutoMapper;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Domain.Entities;

public class WebsiteContentMappingProfile : Profile
{
    public WebsiteContentMappingProfile()
    {
        CreateMap<WebsitePage, WebsitePageDto>()
            .ForCtorParam("PageId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("PageCode", opt => opt.MapFrom(src => src.PageCode.Value))
            .ForCtorParam("IsPublished", opt => opt.MapFrom(src => src.IsPublished.Value.ToString()))
            .ForCtorParam("PageStatus", opt => opt.MapFrom(src => src.PageStatus.Value));

        CreateMap<WebsiteNews, WebsiteNewsDto>()
            .ForCtorParam("NewsId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("IsFeatured", opt => opt.MapFrom(src => src.IsFeatured.Value.ToString()))
            .ForCtorParam("IsPublished", opt => opt.MapFrom(src => src.IsPublished.Value.ToString()))
            .ForCtorParam("NewsStatus", opt => opt.MapFrom(src => src.NewsStatus.Value));
    }
}
