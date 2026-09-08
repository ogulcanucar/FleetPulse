using FleetPulse.Application.Features.Geofences.Commands.CreateGeofence;
using FleetPulse.Application.Features.Geofences.Queries.GetAssetViolations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FleetPulse.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeofencesController(IMediator mediator) : CustomBaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateGeofence([FromBody] CreateGeofenceCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("violations/{assetId}")]
        public async Task<IActionResult> GetAssetViolations(string assetId)
        {
            var query = new GetAssetViolationsQuery(assetId);
            var violations = await _mediator.Send(query);
            return Ok(violations);
        }
    }
}