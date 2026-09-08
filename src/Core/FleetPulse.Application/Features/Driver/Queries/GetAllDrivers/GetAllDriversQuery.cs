using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Driver.Queries.GetAllDrivers
{
    public record GetAllDriversQuery() : IRequest<List<DriverDto>>;
}
