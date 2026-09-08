using FleetPulse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Abstractions.Repositories
{
    public interface IAssetRepository : IRepository<Asset>
    {
        Task<Asset?> GetBySerialNumberAsync(string serialNumber);
        Task<IEnumerable<Asset>> GetActiveAssetsAsync();
        Task<Asset?> GetByDriverIdAsync(string driverId);
    }
}
