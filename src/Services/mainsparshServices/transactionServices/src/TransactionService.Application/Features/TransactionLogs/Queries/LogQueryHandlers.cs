using AutoMapper;
using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.TransactionLogs.Queries;

public class GetTransactionLogByIdQueryHandler : IRequestHandler<GetTransactionLogByIdQuery, TransactionLogDto?>
{
    private readonly ITransactionLogRepository _repository;
    private readonly IMapper _mapper;

    public GetTransactionLogByIdQueryHandler(ITransactionLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TransactionLogDto?> Handle(GetTransactionLogByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LogId, cancellationToken);
        return entity is null ? null : _mapper.Map<TransactionLogDto>(entity);
    }
}

public class GetTransactionLogsByEntityQueryHandler : IRequestHandler<GetTransactionLogsByEntityQuery, IEnumerable<TransactionLogDto>>
{
    private readonly ITransactionLogRepository _repository;
    private readonly IMapper _mapper;

    public GetTransactionLogsByEntityQueryHandler(ITransactionLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionLogDto>> Handle(GetTransactionLogsByEntityQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByEntityAsync(request.TransactionType, request.TransactionId, cancellationToken);
        return _mapper.Map<IEnumerable<TransactionLogDto>>(entities);
    }
}

public class GetTransactionLogsByActionQueryHandler : IRequestHandler<GetTransactionLogsByActionQuery, IEnumerable<TransactionLogDto>>
{
    private readonly ITransactionLogRepository _repository;
    private readonly IMapper _mapper;

    public GetTransactionLogsByActionQueryHandler(ITransactionLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionLogDto>> Handle(GetTransactionLogsByActionQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByActionAsync(request.Action, cancellationToken);
        return _mapper.Map<IEnumerable<TransactionLogDto>>(entities);
    }
}

public class GetAllTransactionLogsQueryHandler : IRequestHandler<GetAllTransactionLogsQuery, IEnumerable<TransactionLogDto>>
{
    private readonly ITransactionLogRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTransactionLogsQueryHandler(ITransactionLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionLogDto>> Handle(GetAllTransactionLogsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TransactionLogDto>>(entities);
    }
}
