using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetPulse.Domain.Entities;

namespace FleetPulse.Application.Abstractions.Repositories
{
    public interface IDriverRepository:IRepository<Driver>
    {
        Task<Driver?> GetByNationalIdAsync(string nationalId);
        Task<IEnumerable<Driver>> GetActiveDriversAsync();
    }
}
