namespace WebsiteContentService.Application.Behaviors;

using FluentValidation;
using WebsiteContentService.Application.Commands.Pages;
using WebsiteContentService.Application.Commands.News;

public class CreateWebsitePageCommandValidator : AbstractValidator<CreateWebsitePageCommand>
{
    public CreateWebsitePageCommandValidator()
    {
        RuleFor(x => x.PageCode).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PageTitle).NotEmpty().MaximumLength(255);
        RuleFor(x => x.MetaDescription).MaximumLength(500).When(x => x.MetaDescription != null);
        RuleFor(x => x.MetaKeywords).MaximumLength(500).When(x => x.MetaKeywords != null);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class UpdateWebsitePageCommandValidator : AbstractValidator<UpdateWebsitePageCommand>
{
    public UpdateWebsitePageCommandValidator()
    {
        RuleFor(x => x.PageId).GreaterThan(0);
        RuleFor(x => x.PageTitle).NotEmpty().MaximumLength(255);
        RuleFor(x => x.MetaDescription).MaximumLength(500).When(x => x.MetaDescription != null);
        RuleFor(x => x.MetaKeywords).MaximumLength(500).When(x => x.MetaKeywords != null);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class PublishWebsitePageCommandValidator : AbstractValidator<PublishWebsitePageCommand>
{
    public PublishWebsitePageCommandValidator()
    {
        RuleFor(x => x.PageId).GreaterThan(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class ChangeWebsitePageStatusCommandValidator : AbstractValidator<ChangeWebsitePageStatusCommand>
{
    public ChangeWebsitePageStatusCommandValidator()
    {
        RuleFor(x => x.PageId).GreaterThan(0);
        RuleFor(x => x.NewStatus).NotEmpty()
            .Must(s => new[] { "ACTIVE", "INACTIVE", "DRAFT", "PUBLISHED", "ARCHIVED" }.Contains(s, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Status must be ACTIVE, INACTIVE, DRAFT, PUBLISHED, or ARCHIVED.");
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class CreateWebsiteNewsCommandValidator : AbstractValidator<CreateWebsiteNewsCommand>
{
    public CreateWebsiteNewsCommandValidator()
    {
        RuleFor(x => x.NewsTitle).NotEmpty().MaximumLength(500);
        RuleFor(x => x.NewsContent).NotEmpty();
        RuleFor(x => x.NewsSummary).MaximumLength(500).When(x => x.NewsSummary != null);
        RuleFor(x => x.NewsCategory).MaximumLength(100).When(x => x.NewsCategory != null);
        RuleFor(x => x.FeaturedImage).MaximumLength(500).When(x => x.FeaturedImage != null);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class UpdateWebsiteNewsCommandValidator : AbstractValidator<UpdateWebsiteNewsCommand>
{
    public UpdateWebsiteNewsCommandValidator()
    {
        RuleFor(x => x.NewsId).GreaterThan(0);
        RuleFor(x => x.NewsTitle).NotEmpty().MaximumLength(500);
        RuleFor(x => x.NewsContent).NotEmpty();
        RuleFor(x => x.NewsSummary).MaximumLength(500).When(x => x.NewsSummary != null);
        RuleFor(x => x.NewsCategory).MaximumLength(100).When(x => x.NewsCategory != null);
        RuleFor(x => x.FeaturedImage).MaximumLength(500).When(x => x.FeaturedImage != null);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class PublishWebsiteNewsCommandValidator : AbstractValidator<PublishWebsiteNewsCommand>
{
    public PublishWebsiteNewsCommandValidator()
    {
        RuleFor(x => x.NewsId).GreaterThan(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class ArchiveWebsiteNewsCommandValidator : AbstractValidator<ArchiveWebsiteNewsCommand>
{
    public ArchiveWebsiteNewsCommandValidator()
    {
        RuleFor(x => x.NewsId).GreaterThan(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class SetNewsFeaturedCommandValidator : AbstractValidator<SetNewsFeaturedCommand>
{
    public SetNewsFeaturedCommandValidator()
    {
        RuleFor(x => x.NewsId).GreaterThan(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}
