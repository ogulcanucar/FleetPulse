using MediatR;

namespace FleetPulse.Application.Features.Assets.Queries.GetAssetTelemetry
{
    public record GetAssetTelemetryQuery(string AssetId) : IRequest<List<TelemetryDto>>;
}