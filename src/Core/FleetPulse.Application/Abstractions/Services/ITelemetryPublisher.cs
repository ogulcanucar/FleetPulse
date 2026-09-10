using FleetPulse.Application.Abstractions.DTOs.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Abstractions.Services
{
    public interface ITelemetryPublisher
    {
        Task PublishTelemetryAsync(TelemetryBroadcastDto telemetry);
    }
}
