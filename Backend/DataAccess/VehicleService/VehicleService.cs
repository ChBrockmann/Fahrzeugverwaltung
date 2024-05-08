﻿using Model.Vehicle;
using DataAccess.BaseService;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.VehicleService;

public class VehicleService : BaseService<VehicleModel, VehicleModelId>, IVehicleService
{
    public VehicleService(DatabaseContext databaseContext) : base(databaseContext)
    {
        
    }

    public override async Task<VehicleModel?> Get(VehicleModelId id)
    {
        return await Database.VehicleModels.FindAsync(id);
    }
}