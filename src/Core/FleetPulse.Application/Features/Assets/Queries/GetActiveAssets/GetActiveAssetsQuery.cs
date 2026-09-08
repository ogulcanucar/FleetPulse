using MediatR;

namespace FleetPulse.Application.Features.Assets.Queries.GetActiveAssets
{
    public record GetActiveAssetsQuery() : IRequest<IReadOnlyList<AssetDto>>;
}