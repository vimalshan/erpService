using AutoMapper;
using EmployeePrideManagement.Application.DTOs;
using EmployeePrideManagement.Domain.Entities;
using EmployeePrideManagement.Domain.Interfaces;
using MediatR;

namespace EmployeePrideManagement.Application.Commands.CreatePrideMoment;

public class CreatePrideMomentCommandHandler : IRequestHandler<CreatePrideMomentCommand, PrideMomentDto>
{
    private readonly IPrideMomentRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;

    public CreatePrideMomentCommandHandler(
        IPrideMomentRepository repository,
        IMapper mapper,
        IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _mapper = mapper;
        _messagePublisher = messagePublisher;
    }

    public async Task<PrideMomentDto> Handle(CreatePrideMomentCommand request, CancellationToken cancellationToken)
    {
        var entity = new MomentPride(
            request.Title,
            request.Body,
            request.EmployeeSysId,
            request.Footer,
            request.Location,
            request.ImagePath,
            request.ModifiedBy);

        var created = await _repository.AddAsync(entity, cancellationToken);

        await _messagePublisher.PublishAsync("pride-moment-created", new
        {
            created.MomentPrideId,
            created.Title,
            created.EmployeeSysId,
            created.ModifiedOn
        }, cancellationToken);

        return _mapper.Map<PrideMomentDto>(created);
    }
}
