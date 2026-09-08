using FleetPulse.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Domain.Entities
{
    public class GeofenceViolation : BaseEntity
    {
        public string AssetId { get; set; } = string.Empty;
        public string GeofenceId { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty; 
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceInMeters { get; set; }
        public DateTime ViolationTime { get; set; } = DateTime.UtcNow;
    }
}
