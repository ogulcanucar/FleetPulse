using FleetPulse.Application.Features.Driver.Commands.AssignDriver;
using FleetPulse.Application.Features.Driver.Commands.CreateDriver;
using FleetPulse.Application.Features.Driver.Queries.GetAllDrivers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FleetPulse.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController(IMediator mediator) : CustomBaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] CreateDriverCommand command)
        {
            var driverId = await _mediator.Send(command);
            return Ok(driverId);
        }

        [HttpPost("{assetId}/assign-driver")]
        public async Task<IActionResult> AssignDriver(string assetId, [FromBody] string driverId)
        {
            var command = new AssignDriverToAssetCommand(assetId, driverId);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            var query = new GetAllDriversQuery();
            var drivers = await _mediator.Send(query);
            return Ok(drivers);
        }
    }
}