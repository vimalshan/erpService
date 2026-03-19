using AutoMapper;
using EmployeePrideManagement.Application.DTOs;
using EmployeePrideManagement.Domain.Exceptions;
using EmployeePrideManagement.Domain.Interfaces;
using MediatR;

namespace EmployeePrideManagement.Application.Commands.UpdatePrideMoment;

public class UpdatePrideMomentCommandHandler : IRequestHandler<UpdatePrideMomentCommand, PrideMomentDto>
{
    private readonly IPrideMomentRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;

    public UpdatePrideMomentCommandHandler(
        IPrideMomentRepository repository,
        IMapper mapper,
        IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _mapper = mapper;
        _messagePublisher = messagePublisher;
    }

    public async Task<PrideMomentDto> Handle(UpdatePrideMomentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.MomentPrideId, cancellationToken)
            ?? throw new PrideMomentNotFoundException(request.MomentPrideId);

        entity.Update(
            request.Title,
            request.Body,
            request.Footer,
            request.Location,
            request.ImagePath,
            request.ModifiedBy);

        await _repository.UpdateAsync(entity, cancellationToken);

        await _messagePublisher.PublishAsync("pride-moment-updated", new
        {
            entity.MomentPrideId,
            entity.Title,
            entity.ModifiedOn
        }, cancellationToken);

        return _mapper.Map<PrideMomentDto>(entity);
    }
}
