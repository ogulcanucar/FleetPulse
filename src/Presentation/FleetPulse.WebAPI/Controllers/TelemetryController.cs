using FleetPulse.Application.Features.Assets.Queries.GetAssetTelemetry;
using FleetPulse.Application.Features.Telemetry.Commands.SendTelemetry;
using FleetPulse.Application.Features.Telemetry.Queries.GetSpeedingAlarms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FleetPulse.WebAPI.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController(IMediator mediator) : CustomBaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> SendTelemetry([FromBody] SendTelemetryCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
        [HttpGet("asset/{assetId}")]
        public async Task<IActionResult> GetVehicleTelemetry(string assetId)
        {
            var query = new GetAssetTelemetryQuery(assetId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("speeding-alarms")]
        public async Task<IActionResult> GetSpeedingAlarms()
        {
            var query = new GetSpeedingAlarmsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
