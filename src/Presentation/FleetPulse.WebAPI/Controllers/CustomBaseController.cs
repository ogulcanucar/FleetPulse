using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FleetPulse.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class CustomBaseController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}