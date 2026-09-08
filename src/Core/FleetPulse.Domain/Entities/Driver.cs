using FleetPulse.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Domain.Entities
{
    public class Driver : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty; 
        public string PhoneNumber { get; set; } = string.Empty;
        public string LicenseType { get; set; } = string.Empty; 
        public bool IsActive { get; set; } = true;

    }
}
