using FleetPulse.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Domain.Entities
{
    public class Geofence:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public double CenterLatitude { get; set; }       
        public double CenterLongitude { get; set; }     
        public double RadiusInMeters { get; set; }      
        public bool IsActive { get; set; } = true;
    }
}
