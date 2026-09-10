using FleetPulse.Application.Abstractions.DTOs.Telemetry;
using FleetPulse.Application.Abstractions.Services;
using FleetPulse.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FleetPulse.WebAPI.Services
{
    public class SignalRTelemetryPublisher(IHubContext<FleetHub> hubContext) : ITelemetryPublisher
    {
        private readonly IHubContext<FleetHub> _hubContext = hubContext;
        public async Task PublishTelemetryAsync(TelemetryBroadcastDto telemetry)
        {
            await _hubContext.Clients.All.SendAsync(
                "TelemetryReceived",
                telemetry);
        }
    }
}
