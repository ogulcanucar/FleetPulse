using FleetPulse.Application.Features.Assets.Commands.CreateAsset;
using FleetPulse.Application.Features.Assets.Queries.GetActiveAssets;
using FleetPulse.Application.Features.Assets.Queries.GetAssetDetails;
using Microsoft.AspNetCore.Mvc;

namespace FleetPulse.WebAPI.Controllers
{
    public class AssetsController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssetCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveAssets()
        {
            var response = await Mediator.Send(new GetActiveAssetsQuery());
            return Ok(response);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetAssetDetails(string id)
        {
            var query = new GetAssetDetailsQuery(id);
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}