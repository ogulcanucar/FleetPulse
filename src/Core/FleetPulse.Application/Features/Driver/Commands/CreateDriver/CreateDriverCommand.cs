using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Driver.Commands.CreateDriver
{
    public record CreateDriverCommand(
     string FirstName,
     string LastName,
     string NationalId,
     string PhoneNumber,
     string LicenseType
 ) : IRequest<string>;
}
