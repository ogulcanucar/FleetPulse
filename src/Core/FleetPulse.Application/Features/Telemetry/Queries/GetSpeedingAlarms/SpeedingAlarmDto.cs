using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Telemetry.Queries.GetSpeedingAlarms
{
    public record SpeedingAlarmDto(
     string Id,
     string AssetId,
     double Speed,
     double Latitude,
     double Longitude,
     DateTime Timestamp
 );
}
