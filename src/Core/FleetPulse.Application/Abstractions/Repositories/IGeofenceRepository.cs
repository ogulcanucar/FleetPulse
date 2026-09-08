using FleetPulse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Abstractions.Repositories
{
    public interface IGeofenceRepository : IRepository<Geofence>
    {
        Task<IEnumerable<Geofence>> GetActiveGeofencesAsync();
    }
}
