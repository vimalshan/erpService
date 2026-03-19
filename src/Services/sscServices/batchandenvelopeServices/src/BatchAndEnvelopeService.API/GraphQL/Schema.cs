using HotChocolate;
using HotChocolate.Types;
using MediatR;
using BatchAndEnvelopeService.Application.DTOs;
using BatchAndEnvelopeService.Application.Queries.Batch;
using BatchAndEnvelopeService.Application.Queries.Envelope;
using BatchAndEnvelopeService.Application.Commands.Batch;
using BatchAndEnvelopeService.Application.Commands.Envelope;

namespace BatchAndEnvelopeService.API.GraphQL;

public class BatchQuery
{
    public async Task<IEnumerable<BatchDto>> GetBatches([Service] IMediator mediator, int page = 1, int pageSize = 20)
        => await mediator.Send(new GetAllBatchesQuery(page, pageSize));

    public async Task<BatchDto?> GetBatch([Service] IMediator mediator, long id)
        => await mediator.Send(new GetBatchByIdQuery(id));

    public async Task<IEnumerable<BatchDto>> GetBatchesByLocation([Service] IMediator mediator, long locationId)
        => await mediator.Send(new GetBatchesByLocationQuery(locationId));
}

[ExtendObjectType(typeof(BatchQuery))]
public class EnvelopeQuery
{
    public async Task<IEnumerable<EnvelopeDto>> GetEnvelopes([Service] IMediator mediator, int page = 1, int pageSize = 20)
        => await mediator.Send(new GetAllEnvelopesQuery(page, pageSize));

    public async Task<EnvelopeDto?> GetEnvelope([Service] IMediator mediator, long id)
        => await mediator.Send(new GetEnvelopeByIdQuery(id));

    public async Task<IEnumerable<EnvelopeDto>> GetEnvelopesByType([Service] IMediator mediator, string envelopeType)
        => await mediator.Send(new GetEnvelopesByTypeQuery(envelopeType));
}

public class BatchMutation
{
    public async Task<BatchDto> CreateBatch([Service] IMediator mediator, CreateBatchCommand input)
        => await mediator.Send(input);

    public async Task<BatchDto> ConfirmBatch([Service] IMediator mediator, long batchId, long confirmedBy)
        => await mediator.Send(new ConfirmBatchCommand(batchId, confirmedBy));

    public async Task<BatchDto> CancelBatch([Service] IMediator mediator, long batchId, long cancelledBy)
        => await mediator.Send(new CancelBatchCommand(batchId, cancelledBy));
}

[ExtendObjectType(typeof(BatchMutation))]
public class EnvelopeMutation
{
    public async Task<EnvelopeDto> CreateEnvelope([Service] IMediator mediator, CreateEnvelopeCommand input)
        => await mediator.Send(input);

    public async Task<EnvelopeDto> ConfirmEnvelope([Service] IMediator mediator, long envelopeId, long confirmedBy)
        => await mediator.Send(new ConfirmEnvelopeCommand(envelopeId, confirmedBy));

    public async Task<EnvelopeDto> CancelEnvelope([Service] IMediator mediator, long envelopeId, long cancelledBy)
        => await mediator.Send(new CancelEnvelopeCommand(envelopeId, cancelledBy));
}

