using FleetPulse.Domain.Common;
using FleetPulse.Domain.Enums;


namespace FleetPulse.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public AssetType Type { get; set; }
        public string DriverId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public Dictionary<string, object> Attributes { get; set; } = new();
    }
}
