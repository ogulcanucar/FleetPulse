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
    public class DriverRepository(MongoDbContext context) : GenericRepository<Driver>(context, nameof(MongoDbContext.Drivers)), IDriverRepository
    {
        public async Task<Driver?> GetByNationalIdAsync(string nationalId)
        {
            return await _collection.Find(x => x.NationalId == nationalId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Driver>> GetActiveDriversAsync()
        {
            return await _collection.Find(x => x.IsActive).ToListAsync();
        }
    }
}
