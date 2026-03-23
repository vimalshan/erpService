using MediatR;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Application.DTOs;

namespace VisitorServices.Application.Visitors.Commands.AddVisitorItem;

public sealed class AddVisitorItemCommandHandler(
    IVisitorRepository visitorRepository,
    IVisitorItemRepository itemRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddVisitorItemCommand, VisitorItemDto>
{
    public async Task<VisitorItemDto> Handle(AddVisitorItemCommand request, CancellationToken cancellationToken)
    {
        var visitor = await visitorRepository.GetByIdAsync(request.VisitorId, cancellationToken)
            ?? throw new KeyNotFoundException($"Visitor {request.VisitorId} not found.");

        var itemId = await itemRepository.GetNextIdAsync(cancellationToken);
        var item = visitor.AddItem(itemId, request.Description, request.Quantity,
            request.MaterialType, request.Notes, request.EnteredBy);

        await itemRepository.AddAsync(item, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new VisitorItemDto(item.Id, item.VisitorId, item.Description,
            item.Quantity, item.MaterialType, item.Notes, item.Status.ToString(),
            item.EnteredOn, item.EnteredBy);
    }
}
