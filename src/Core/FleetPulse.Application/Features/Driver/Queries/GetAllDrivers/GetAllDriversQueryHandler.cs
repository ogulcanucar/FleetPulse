using FleetPulse.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Driver.Queries.GetAllDrivers
{
    public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, List<DriverDto>>
    {
        private readonly IDriverRepository _driverRepository;

        public GetAllDriversQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<List<DriverDto>> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _driverRepository.GetActiveDriversAsync();

            return drivers.Select(d => new DriverDto(
                d.Id,
                d.FirstName,
                d.LastName,
                d.NationalId,
                d.PhoneNumber,
                d.LicenseType,
                d.IsActive,
                d.CreatedAt
            )).ToList();
        }
    }
}