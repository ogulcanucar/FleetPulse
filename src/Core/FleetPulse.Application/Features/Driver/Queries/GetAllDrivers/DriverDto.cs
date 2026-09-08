using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Driver.Queries.GetAllDrivers
{
    public record DriverDto(
     string Id,
     string FirstName,
     string LastName,
     string NationalId,
     string PhoneNumber,
     string LicenseType,
     bool IsActive,
     DateTime CreatedAt
 );
}
