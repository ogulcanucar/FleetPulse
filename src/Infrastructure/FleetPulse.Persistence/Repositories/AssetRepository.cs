using FleetPulse.Application.Abstractions.Repositories;
using FleetPulse.Domain.Entities;
using FleetPulse.Persistence.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Persistence.Repositories
{
    public class AssetRepository(MongoDbContext context) : GenericRepository<Asset>(context, nameof(MongoDbContext.Assets)), IAssetRepository
    {
        public async Task<IEnumerable<Asset>> GetActiveAssetsAsync()
        {
            return await _collection.Find(x => x.IsActive).ToListAsync();
        }

        public async Task<Asset?> GetByDriverIdAsync(string driverId)
        {
            return await _collection.Find(x => x.DriverId == driverId).FirstOrDefaultAsync();
        }

        public async Task<Asset?> GetBySerialNumberAsync(string serialNumber)
        {
            return await _collection.Find(x => x.SerialNumber == serialNumber).FirstOrDefaultAsync();
        }
    }
}
