using FleetPulse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Abstractions.Repositories
{
    public interface ITelemetryRepository:IRepository<TelemetryData>
    {
        Task<IEnumerable<TelemetryData>> GetLatestTelemetryByAssetIdAsync(string assetId, int count = 10);
        Task<IEnumerable<TelemetryData>> GetSpeedingAlarmsAsync();
    }
}
