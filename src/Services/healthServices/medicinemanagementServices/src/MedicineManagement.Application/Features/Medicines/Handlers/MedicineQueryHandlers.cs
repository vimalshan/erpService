using AutoMapper;
using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.Medicines.Queries;
using MedicineManagement.Domain.Interfaces;

namespace MedicineManagement.Application.Features.Medicines.Handlers;

public class GetAllMedicinesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllMedicinesQuery, IReadOnlyList<MedicineDto>>
{
    public async Task<IReadOnlyList<MedicineDto>> Handle(GetAllMedicinesQuery request, CancellationToken ct)
    {
        var medicines = await unitOfWork.Medicines.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<MedicineDto>>(medicines);
    }
}

public class GetMedicineByCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetMedicineByCodeQuery, MedicineDto?>
{
    public async Task<MedicineDto?> Handle(GetMedicineByCodeQuery request, CancellationToken ct)
    {
        var medicine = await unitOfWork.Medicines.GetByCodeAsync(request.MedicineCode, ct);
        return medicine is null ? null : mapper.Map<MedicineDto>(medicine);
    }
}

public class SearchMedicinesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<SearchMedicinesQuery, IReadOnlyList<MedicineDto>>
{
    public async Task<IReadOnlyList<MedicineDto>> Handle(SearchMedicinesQuery request, CancellationToken ct)
    {
        var medicines = await unitOfWork.Medicines.SearchByNameAsync(request.Name, ct);
        return mapper.Map<IReadOnlyList<MedicineDto>>(medicines);
    }
}

public class GetLowStockMedicinesHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetLowStockMedicinesQuery, IReadOnlyList<StockSummaryDto>>
{
    public async Task<IReadOnlyList<StockSummaryDto>> Handle(GetLowStockMedicinesQuery request, CancellationToken ct)
    {
        var medicines = await unitOfWork.Medicines.GetLowStockMedicinesAsync(ct);
        var result = new List<StockSummaryDto>();
        foreach (var med in medicines)
        {
            var stock = await unitOfWork.MedicineCredits.GetCurrentStockAsync(med.MedicineCode, ct);
            if (med.IsBelowMinimumStock(stock))
            {
                result.Add(new StockSummaryDto(med.MedicineCode, med.MedicineName, stock, med.OrderLevelMin, med.OrderLevelMax));
            }
        }
        return result;
    }
}
