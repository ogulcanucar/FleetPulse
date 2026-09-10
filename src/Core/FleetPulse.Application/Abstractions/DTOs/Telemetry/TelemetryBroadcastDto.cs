using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Abstractions.DTOs.Telemetry
{
    public class TelemetryBroadcastDto
    {
        public string AssetId { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Speed { get; set; }

        public bool EngineStatus { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
