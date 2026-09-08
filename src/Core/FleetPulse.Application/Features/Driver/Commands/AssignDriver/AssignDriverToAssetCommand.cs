using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Driver.Commands.AssignDriver
{
    public record AssignDriverToAssetCommand(string AssetId, string DriverId) : IRequest<bool>;
}
