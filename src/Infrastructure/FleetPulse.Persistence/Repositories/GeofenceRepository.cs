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
    public class GeofenceRepository(MongoDbContext context) : GenericRepository<Geofence>(context, "Geofences"), IGeofenceRepository
    {
        public async Task<IEnumerable<Geofence>> GetActiveGeofencesAsync()
        {
            return await _collection.Find(g => g.IsActive).ToListAsync();
        }
    }
}
