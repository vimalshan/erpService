using AutoMapper;
using MediatR;
using travelTransactionService.Application.Commands;
using travelTransactionService.Application.DTOs;
using travelTransactionService.Domain.Entities;
using travelTransactionService.Domain.Interfaces;

namespace travelTransactionService.Application.Handlers;

public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, VendorMasterDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateVendorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VendorMasterDto> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = VendorMaster.Create(
            request.VendorId,
            request.Name,
            request.CategoryType,
            request.AddressLine1,
            request.PhoneNumber,
            request.ItPanNumber);

        await _unitOfWork.Vendors.AddAsync(vendor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VendorMasterDto>(vendor);
    }
}

public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVendorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _unitOfWork.Vendors.GetByIdAsync(request.VendorId, cancellationToken)
            ?? throw new KeyNotFoundException($"Vendor {request.VendorId} not found.");

        vendor.Update(request.Name, request.AddressLine1, request.PhoneNumber,
            request.ItPanNumber, request.BankName, request.AccountNumber);

        await _unitOfWork.Vendors.UpdateAsync(vendor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVendorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Vendors.DeleteAsync(request.VendorId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class CreateTaxMasterCommandHandler : IRequestHandler<CreateTaxMasterCommand, TaxMasterDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTaxMasterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaxMasterDto> Handle(CreateTaxMasterCommand request, CancellationToken cancellationToken)
    {
        var taxMaster = TaxMaster.Create(
            request.VendorId,
            request.TaxType,
            request.TaxRate,
            request.EffectiveDate);

        await _unitOfWork.TaxMasters.AddAsync(taxMaster, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TaxMasterDto>(taxMaster);
    }
}

public class UpdateTaxRateCommandHandler : IRequestHandler<UpdateTaxRateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaxRateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateTaxRateCommand request, CancellationToken cancellationToken)
    {
        var taxMaster = await _unitOfWork.TaxMasters.GetByTypeAsync(request.TaxType, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax type {request.TaxType} not found.");

        taxMaster.UpdateRate(request.NewRate, request.ModifiedBy);

        await _unitOfWork.TaxMasters.UpdateAsync(taxMaster, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class CreateJaiInterfaceLineCommandHandler : IRequestHandler<CreateJaiInterfaceLineCommand, JaiInterfaceLineDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateJaiInterfaceLineCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<JaiInterfaceLineDto> Handle(CreateJaiInterfaceLineCommand request, CancellationToken cancellationToken)
    {
        var line = JaiInterfaceLine.Create(
            request.OrgId,
            request.PartyId,
            request.PartySiteId,
            request.ImportModule,
            request.TransactionNum,
            request.TransactionLineNum,
            request.CreatedBy);

        foreach (var taxItem in request.TaxLines)
        {
            var taxLine = JaiInterfaceTaxLine.Create(
                request.PartyId,
                request.PartySiteId,
                request.ImportModule,
                request.TransactionNum,
                request.TransactionLineNum,
                taxItem.TaxLineNo,
                taxItem.TaxRate,
                taxItem.TaxAmount,
                request.CreatedBy);
            line.AddTaxLine(taxLine);
        }

        await _unitOfWork.JaiInterfaceLines.AddAsync(line, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<JaiInterfaceLineDto>(line);
    }
}

public class UpdateGstAmountsCommandHandler : IRequestHandler<UpdateGstAmountsCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGstAmountsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateGstAmountsCommand request, CancellationToken cancellationToken)
    {
        var line = await _unitOfWork.JaiInterfaceLines.GetByIdAsync(request.InterfaceLineId, cancellationToken)
            ?? throw new KeyNotFoundException($"Interface line {request.InterfaceLineId} not found.");

        line.UpdateGstAmounts(request.SgstAmount, request.CgstAmount, request.IgstAmount);

        await _unitOfWork.JaiInterfaceLines.UpdateAsync(line, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class CreateTravelApParamsCommandHandler : IRequestHandler<CreateTravelApParamsCommand, TravelApParamsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTravelApParamsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TravelApParamsDto> Handle(CreateTravelApParamsCommand request, CancellationToken cancellationToken)
    {
        var apParams = TravelApParams.Create(
            request.ApUnitId,
            request.AccountStatus,
            request.AccountCode,
            request.ControlCombId);

        // Use raw DbContext via JaiInterfaceLines repo pattern for now
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TravelApParamsDto>(apParams);
    }
}
