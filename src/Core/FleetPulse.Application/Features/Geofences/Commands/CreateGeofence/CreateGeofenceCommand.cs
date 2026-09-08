using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Geofences.Commands.CreateGeofence
{
    public record CreateGeofenceCommand(
     string Name,
     double CenterLatitude,
     double CenterLongitude,
     double RadiusInMeters
 ) : IRequest<string>;
}
