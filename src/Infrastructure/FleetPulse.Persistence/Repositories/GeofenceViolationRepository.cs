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
    public class GeofenceViolationRepository(MongoDbContext context) : GenericRepository<GeofenceViolation>(context, "GeofenceViolations"), IGeofenceViolationRepository
    {
        public async Task<IEnumerable<GeofenceViolation>> GetViolationsByAssetIdAsync(string assetId)
        {
            return await _collection.Find(v => v.AssetId == assetId) 
                                    .SortByDescending(v => v.ViolationTime)
                                    .ToListAsync();
        }
    }
}