using AutoMapper;
using MediatR;
using VehicleTracking.Application.DTOs;
using VehicleTracking.Application.Vehicles.Commands;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.Application.Vehicles.Handlers;

public class RegisterVehicleHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<RegisterVehicleCommand, VehicleMasterDto>
{
    public async Task<VehicleMasterDto> Handle(RegisterVehicleCommand request, CancellationToken ct)
    {
        var vehicle = VehicleMaster.Register(
            request.RegNum1, request.RegNum2, request.RegNum3, request.RegNum4,
            request.RegistrationDate, request.UpdatedBy, request.UpdatedByNum);

        await uow.VehicleMasters.AddAsync(vehicle, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<VehicleMasterDto>(vehicle);
    }
}

public class UpdateVehicleStageHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateVehicleStageCommand, VehicleStageDto>
{
    public async Task<VehicleStageDto> Handle(UpdateVehicleStageCommand request, CancellationToken ct)
    {
        var stage = VehicleStage.Create(
            request.VehicleTracker, request.TrackingNumber, request.StageCode, request.StageDecision,
            request.EnteredBy, request.EnteredByNum);

        await uow.VehicleStages.AddAsync(stage, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<VehicleStageDto>(stage);
    }
}

public class CreateVehicleTransactionHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateVehicleTransactionCommand, VehicleTransactionDto>
{
    public async Task<VehicleTransactionDto> Handle(CreateVehicleTransactionCommand request, CancellationToken ct)
    {
        var transaction = new VehicleTransaction
        {
            VehicleSerial = request.VehicleSerial,
            PartyName = request.PartyName,
            ReportDate = DateTime.UtcNow,
            PurposeCode = request.PurposeCode,
            GateName = request.GateName,
            ProductCode = request.ProductCode,
            ProductQuantity = request.ProductQuantity,
            DriverName = request.DriverName,
            DriverCell = request.DriverCell,
            TyreWeight = request.TyreWeight,
            GrossWeight = request.GrossWeight,
            VehicleStatus = 'A',
            LogEntryUser = request.LogEntryUser,
            LogEntryDate = DateTime.UtcNow,
            MainPurpose = request.MainPurpose,
            SupplierCode = request.SupplierCode
        };

        await uow.VehicleTransactions.AddAsync(transaction, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<VehicleTransactionDto>(transaction);
    }
}

public class CreateVehicleInvoiceHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateVehicleInvoiceCommand, VehicleInvoiceDto>
{
    public async Task<VehicleInvoiceDto> Handle(CreateVehicleInvoiceCommand request, CancellationToken ct)
    {
        var invoice = new VehicleInvoice
        {
            TrackingNumber = request.TrackingNumber,
            ReferenceNumber = request.ReferenceNumber,
            OriginalInvoice = request.OriginalInvoice,
            ChainInvoice = request.ChainInvoice,
            CustomerCode = request.CustomerCode,
            ModifiedUser = request.ModifiedUser,
            ModifiedNumber = request.ModifiedNumber,
            ModifiedDate = DateTime.UtcNow
        };

        await uow.VehicleInvoices.AddAsync(invoice, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<VehicleInvoiceDto>(invoice);
    }
}

public class MakeDecisionHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<MakeDecisionCommand, DecisionFlagDto>
{
    public async Task<DecisionFlagDto> Handle(MakeDecisionCommand request, CancellationToken ct)
    {
        var decision = new DecisionFlag
        {
            TrackingNumber = request.TrackingNumber,
            PurposeCode = request.PurposeCode,
            StageCode = request.StageCode,
            StageDecision = request.StageDecision,
            CancelFlag = 'N',
            Remark = request.Remark,
            UpdateDate = DateTime.UtcNow
        };

        await uow.DecisionFlags.AddAsync(decision, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<DecisionFlagDto>(decision);
    }
}

public class UpdateWeightInfoHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateWeightInfoCommand, WeightInfoDto>
{
    public async Task<WeightInfoDto> Handle(UpdateWeightInfoCommand request, CancellationToken ct)
    {
        var existing = await uow.WeightInfos.GetByTrackingNumberAsync(request.TrackingNumber, ct);
        if (existing is not null)
        {
            existing.TyreWeight = request.TyreWeight;
            existing.GrossWeight = request.GrossWeight;
            existing.NetWeight = request.NetWeight;
            await uow.WeightInfos.UpdateAsync(existing, ct);
        }
        else
        {
            existing = new WeightInformation
            {
                TrackingNumber = request.TrackingNumber,
                TyreWeight = request.TyreWeight,
                GrossWeight = request.GrossWeight,
                NetWeight = request.NetWeight
            };
            await uow.WeightInfos.AddAsync(existing, ct);
        }

        await uow.SaveChangesAsync(ct);
        return mapper.Map<WeightInfoDto>(existing);
    }
}

public class UpdateVehicleMasterHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateVehicleMasterCommand, VehicleMasterDto>
{
    public async Task<VehicleMasterDto> Handle(UpdateVehicleMasterCommand request, CancellationToken ct)
    {
        var vehicle = await uow.VehicleMasters.GetByIdAsync(request.SerialNumber, ct)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Vehicle {request.SerialNumber} not found.");

        vehicle.RegNum1 = request.RegNum1;
        vehicle.RegNum2 = request.RegNum2;
        vehicle.RegNum3 = request.RegNum3;
        vehicle.RegNum4 = request.RegNum4;
        vehicle.RegistrationDate = request.RegistrationDate;
        vehicle.UpdatedBy = request.UpdatedBy;
        vehicle.UpdateNumber = request.UpdatedByNum;
        vehicle.UpdatedDate = DateTime.UtcNow;

        await uow.VehicleMasters.UpdateAsync(vehicle, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<VehicleMasterDto>(vehicle);
    }
}

public class CloseVehicleTransactionHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CloseVehicleTransactionCommand, VehicleTransactionDto>
{
    public async Task<VehicleTransactionDto> Handle(CloseVehicleTransactionCommand request, CancellationToken ct)
    {
        var transactions = await uow.VehicleTransactions.GetByTrackingNumberAsync(request.TrackingNumber, ct);
        var transaction = transactions.FirstOrDefault(t => t.VehicleStatus == 'A')
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Active transaction for tracking {request.TrackingNumber} not found.");

        transaction.VehicleStatus = 'C';
        transaction.LogEntryUser = request.LogEntryUser;
        transaction.LogEntryDate = DateTime.UtcNow;

        await uow.SaveChangesAsync(ct);
        return mapper.Map<VehicleTransactionDto>(transaction);
    }
}
