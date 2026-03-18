using AutoMapper;
using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.MedicineCredits.Commands;
using MedicineManagement.Application.Features.MedicineCredits.Queries;
using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;

namespace MedicineManagement.Application.Features.MedicineCredits.Handlers;

public class GetStockByMedicineHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetStockByMedicineQuery, long>
{
    public async Task<long> Handle(GetStockByMedicineQuery request, CancellationToken ct)
    {
        return await unitOfWork.MedicineCredits.GetCurrentStockAsync(request.MedicineCode, ct);
    }
}

public class GetTransactionsByDateRangeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetTransactionsByDateRangeQuery, IReadOnlyList<MedicineCreditDto>>
{
    public async Task<IReadOnlyList<MedicineCreditDto>> Handle(GetTransactionsByDateRangeQuery request, CancellationToken ct)
    {
        var transactions = await unitOfWork.MedicineCredits.GetByDateRangeAsync(request.From, request.To, ct);
        return mapper.Map<IReadOnlyList<MedicineCreditDto>>(transactions);
    }
}

public class GetTransactionsByMedicineHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetTransactionsByMedicineQuery, IReadOnlyList<MedicineCreditDto>>
{
    public async Task<IReadOnlyList<MedicineCreditDto>> Handle(GetTransactionsByMedicineQuery request, CancellationToken ct)
    {
        var transactions = await unitOfWork.MedicineCredits.GetByMedicineCodeAsync(request.MedicineCode, ct);
        return mapper.Map<IReadOnlyList<MedicineCreditDto>>(transactions);
    }
}

public class CreateMedicineCreditHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateMedicineCreditCommand, MedicineCreditDto>
{
    public async Task<MedicineCreditDto> Handle(CreateMedicineCreditCommand request, CancellationToken ct)
    {
        var entity = MedicineCredit.Create(
            request.CompanyCode, request.TransactionCode, request.MedicineCode,
            request.RecordType, request.Quantity, request.TransactionDate,
            request.EntryUser, request.EntryUserPin, request.LotNumber);
        await unitOfWork.MedicineCredits.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<MedicineCreditDto>(entity);
    }
}
