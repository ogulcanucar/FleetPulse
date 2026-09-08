using FleetPulse.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Domain.Entities
{
    public class TelemetryData : BaseEntity
    {
        public string AssetId { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public double EngineTemperature { get; set; }
        public double FuelLevel { get; set; }
        public bool HasSpeedingAlarm { get; set; }
        public bool EngineStatus { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
