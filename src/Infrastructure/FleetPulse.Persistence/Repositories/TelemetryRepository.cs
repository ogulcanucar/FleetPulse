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
    public class TelemetryRepository(MongoDbContext context) : GenericRepository<TelemetryData>(context, nameof(MongoDbContext.TelemetryData)), ITelemetryRepository
    {
        public async Task<IEnumerable<TelemetryData>> GetLatestTelemetryByAssetIdAsync(string assetId , int count = 10)
        {
            return await _collection.Find(x => x.AssetId == assetId)
                .SortByDescending(x => x.Timestamp)
                .Limit(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<TelemetryData>> GetSpeedingAlarmsAsync()
        {
            return await _collection.Find(x => x.Speed > 80).ToListAsync();
        }
    }
}
