using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Events.Geofences
{

    public record GeofenceViolationCreatedEvent(
       string ViolationId,
       string AssetId,
       string GeofenceId,
       string DriverId,
       double Latitude,
       double Longitude,
       double DistanceInMeters,
       DateTime ViolationTime
    );
}

